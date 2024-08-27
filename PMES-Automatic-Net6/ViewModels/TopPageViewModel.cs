using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PMES_Automatic_Net6.Core;
using S7.Net;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Respository.DataStruct;
using S7.Net.Types;
using PMES.Model;
using DateTime = System.DateTime;
using PMES_Respository.reportModel;
using PMES.UC.reports;
using System.Drawing;
using DevExpress.XtraReports.UI;
using HslCommunication.ModBus;
using FluentModbus;
using Newtonsoft.Json;
using DevExpress.XtraReports;
using static DevExpress.XtraEditors.Filtering.DataItemsExtension;
using DevExpress.Diagram.Core.Shapes;
using DevExpress.Utils.About;
using PMES.Model.tbs;
using ProductInfo = PMES.Model.ProductInfo;
using T_preheater_code = PMES_Respository.tbs.T_preheater_code;
using System.Reflection;
using DevExpress.Mvvm.Native;
using PMES_Common;


namespace PMES_Automatic_Net6.ViewModels
{
    public partial class TopPageViewModel : ObservableObject
    {
        public int Count { get; set; } = 1000;
        private Plc Plc => HardwareManager.Instance.Plc;
        private ModbusTcpNet PlcXj => HardwareManager.Instance.PlcXj;
        public Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger();
        private Queue<XtraReport> _reportsBox = new Queue<XtraReport>();
        private FSqlMysqlHelper _fSqlHelper = FreeSqlManager.FSqlMysqlHelper;
        private IFreeSql _fSql = FreeSqlManager.FSqlMysqlHelper.FSql;
        private const byte _readFlag = 2;
        private ushort _reverse1 = 0;
        private ushort _reverse2 = 0;
        [ObservableProperty] private int _reelCount = 0;

        private PmesStacking _pmesStackingError = new()
        {
            DeviceId = 2,
            WorkPositionId = 251,
            ReelSpecification = 1,
            StackModel = 1,
            StackingSpeed = 20,
            Reserve1 = 0,
            Reserve2 = 2,
            PmesAndPlcReadWriteFlag = 2
        };

        /// <summary>
        ///     记录当前的任务队列
        /// </summary>
        private List<T_plc_command> _plcCommandsRunning = new List<T_plc_command>();

        [ObservableProperty] private string? _productCode = "";
        [ObservableProperty] private string? _preScanCode1 = "";
        [ObservableProperty] private string? _preScanCode2 = "";
        [ObservableProperty] private string? _boxCode1 = "";
        [ObservableProperty] private string? _boxCode2 = "";
        [ObservableProperty] private string? _weight1 = "0";
        [ObservableProperty] private string? _weight2 = "0";

        [ObservableProperty]
        private ObservableCollection<TopGridModel> _listProcess = new ObservableCollection<TopGridModel>();

        [ObservableProperty]
        private ObservableCollection<TopGridModel> _listHistory = new ObservableCollection<TopGridModel>();

        public TopPageViewModel()
        {
            HardwareManager.Instance.OnReceive += Handler;
            HardwareManager.Instance.OnWeightAndCodeChanged += WeightAndCodeChanged;
            HardwareManager.Instance.OnReelCodeChanged += ReelCodeChanged;
            HardwareManager.Instance.OnBoxBarCodeChanged += BoxBarCodeChanged;
            HardwareManager.Instance.OnBoxArrived += BoxArrived;
            HardwareManager.Instance.OnBoxStacked += BoxStacked;
            StartMonitorPlcCommonTb();
        }

        /// <summary>
        ///     开始检测指令表
        /// </summary>
        private void StartMonitorPlcCommonTb()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Task.Delay(2000);
                        var cmdNeedExec = await _fSql.Select<T_plc_command>().Where(s => s.Status == 0).ToListAsync();
                        //拆垛[1]
                        if (_plcCommandsRunning.Count(s => s.PlcComandType == 1) == 0) //添加任务
                        {
                            var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 1 && s.Status == 0);
                            if (plcCommand != null)
                            {
                                _plcCommandsRunning.Add(plcCommand);
                            }
                        }
                        else //执行任务
                        {
                            var plcCommand =
                                _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 1, Status: 0 });
                            if (plcCommand != null)
                            {
                                plcCommand.Status = 2;
                                await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                    .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                                var writeObject =
                                    JsonConvert.DeserializeObject<PmesCmdUnStacking>(plcCommand.PlcComandContent);
                                var plcCmdAttribute = writeObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                                if (plcCmdAttribute != null)
                                {
                                    var dbBlock = plcCmdAttribute.DbBlock;
                                    await Plc.WriteClassAsync(writeObject, dbBlock);
                                }
                            }
                        }

                        //码垛[2]
                        if (_plcCommandsRunning.Count(s => s.PlcComandType == 2) == 0)
                        {
                            var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 2 && s.Status == 0);
                            if (plcCommand != null)
                            {
                                _plcCommandsRunning.Add(plcCommand);
                            }
                        }

                        //子母托[3]
                        if (_plcCommandsRunning.Count(s => s.PlcComandType == 3) == 0)
                        {
                            var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 3 && s.Status == 0);
                            if (plcCommand != null)
                            {
                                _plcCommandsRunning.Add(plcCommand);
                            }
                        }
                        else
                        {
                            var plcCommand =
                                _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 3, Status: 0 });
                            if (plcCommand != null)
                            {
                                plcCommand.Status = 2;
                                await _fSql.Update<T_plc_command>().Set(s => s.Status, 2)
                                    .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                                var writeObject =
                                    JsonConvert.DeserializeObject<PmesCmdCombinationMotherChildTray>(plcCommand
                                        .PlcComandContent);
                                var plcCmdAttribute = writeObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                                if (plcCmdAttribute != null)
                                {
                                    var dbBlock = plcCmdAttribute.DbBlock;
                                    await Plc.WriteClassAsync(writeObject, dbBlock);
                                }
                                //TODO:执行了之后要检测工位表，看什么时候组托完成，然后通知后台
                                //QA:在哪个工位组 PmesCmdCombinationMotherChildTray 没有说明
                                //Task.Run(async () =>
                                //{
                                //    var start = DateTime.Now;
                                //    while ((DateTime.Now - start).TotalMilliseconds < 300_000)
                                //    {
                                //        await Task.Delay(3000);
                                //        //_fSql.Select<T_station_status>().Where(s=>s.WorkshopId == writeObject.ValToBinString())
                                //    }
                                //});
                            }
                        }

                        //子母托，子母托盘上料[4]
                        if (_plcCommandsRunning.Count(s => s.PlcComandType == 4) > 0)
                        {
                            var plcCommand = cmdNeedExec.FirstOrDefault(s => s.PlcComandType == 4);
                            try
                            {
                                var pmesCmdTrayFeeding = JsonConvert.DeserializeObject<PmesCmdTrayFeeding>(plcCommand!
                                    .PlcComandContent);
                                await Plc.WriteClassAsync(pmesCmdTrayFeeding!, 551);
                                var ret = await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                                    .Where(s => s.Id == pmesCmdTrayFeeding!.ChildTrayWorkShopId).ExecuteAffrowsAsync();
                                Logger?.Verbose($"执行指令:{plcCommand.PlcComandContent}，结果：{ret}");
                            }
                            catch (Exception e)
                            {
                                Logger?.Verbose($"执行指令:{plcCommand!.PlcComandContent}，失n败.\n{e}");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger?.Error($"任务队列异常:\te");
                    }
                }
            });
        }

        #region PLC状态改变处理事件

        [ObservableProperty]
        private ObservableCollection<MyProductTaskInfo>
            _taskQueue = new();

        /// <summary>
        ///     PLC状态监测
        /// </summary>
        /// <param name="obj"></param>
        private async Task Handler(object obj)
        {
            Logger?.Verbose($"状态改变:\n\t{obj}");
            try
            {
                switch (obj)
                {
                    case PmesCmdUnStacking pmesCmdUnStacking:
                        {
                            if (pmesCmdUnStacking.PmesAndPlcReadWriteFlag == 1) //PLC读走指令后置1 表示空闲
                            {
                            }

                            break;
                        }
                    case PlcCmdUnStacking plcUnStacking:
                        break;
                    case PlcCmdStacking plcCmdStacking:
                        if (plcCmdStacking.StackingFinished == 2)
                        {
                            var firstOrDefault =
                                _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 0 });
                            if (firstOrDefault != null)
                            {
                                _plcCommandsRunning.Remove(firstOrDefault);
                                var deserializeObject =
                                    JsonConvert.DeserializeObject<PmesStacking>(firstOrDefault.PlcComandContent);
                                if (_fSql.Update<T_plc_command>().Where(s => s.Id == firstOrDefault.Id)
                                        .Set(s => s.Status, 1).ExecuteAffrows() > 0)
                                {
                                    Logger?.Verbose($"T_plc_command,Id = {firstOrDefault.Id} ,数据库更新成功");
                                }
                                else
                                {
                                    Logger?.Verbose($"T_plc_command,Id = {firstOrDefault.Id} ,数据库更新失败");
                                }
                            }


                            ShowAlarm($"{plcCmdStacking.WorkPositionId} 码垛完成！请发送清跺指令，调度Agv小车拉走。");
                        }

                        break;
                    //组合机器人 -- 组合工位571、572
                    case PlcCmdCombinationMotherChildTray1 plcCmdCombinationMotherChild:
                        if (plcCmdCombinationMotherChild.Num == 1)
                        {
                            //组盘完成, 呼叫agv，拉走【子母托盘组】到缓存位
                            ////?????,buys
                            /// ShowAlarm($"{plcCmdCombinationMotherChild.ChildMotherWorkPostionId} 组盘完成, 呼叫agv，拉走【子母托盘组】到缓存位。");
                        }

                        break;

                    case PlcCmdCombinationMotherChildTray2 plcCmdCombinationMotherChild:
                        if (plcCmdCombinationMotherChild.Num == 1)
                        {
                            //组盘完成, 呼叫agv，拉走【子母托盘组】到缓存位
                            ////?????,buys
                            ///  ShowAlarm($"{plcCmdCombinationMotherChild.ChildMotherWorkPostionId} 组盘完成, 呼叫agv，拉走【子母托盘组】到缓存位。");
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                ShowError($"TopViewModel.Handler Exception!\n{e}");
            }
        }


        /// <summary>
        ///     1 --> 读取重量和条码
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async Task WeightAndCodeChanged(List<DataItem> obj)
        {
            Logger?.Information($"WeightAndCodeChanged,\n{HardwareManager.PrintDataItems(obj)}");
            try
            {
                Logger?.Verbose($"WeightAndCodeChanged,当前标志位是：{obj[4].Value}");

                Weight1 = ((double.Parse(obj[2].Value!.ToString()!)) / 100d).ToString("f2");
                Weight2 = ((double.Parse(obj[3].Value!.ToString()!)) / 100d).ToString("f2");

                ProductCode = obj[1].Value!.ToString();

                var product = new ProductInfo();
                if (ProductCode!.Equals("888"))
                {
                    ErrorStop((int.Parse(obj[0].Value!.ToString()!)), "称重读取条码失败，采用默认条码888");
                    product.product_order_no = "888";
                }

                try
                {
                    product = await WebService.Instance.Get<ProductInfo>(
                                  $"{ApiUrls.QueryOrder}{ProductCode}&format=json") ??
                              new ProductInfo() { product_order_no = "999" };
                }
                catch (Exception e)
                {
                    Logger?.Error($"获取不到条码！API Error!\n{e}");
                }


                try
                {
                    //任务队列添加对象
                    var taskInfo = new MyProductTaskInfo
                    {
                        Weight1 = (double.Parse(obj[2].Value!.ToString()!)),
                        Weight2 = (double.Parse(obj[3].Value!.ToString()!)),
                        ProductOrderBarCode = ProductCode,
                        WorkShop = EnumWorkShop.Weighted,
                        ProductOrderInfo = product
                    };
                    TaskQueue.Add(taskInfo);

                    var topGridModel = await UpdateProductInfo(taskInfo);
                    try
                    {
                        GlobalVar.MainView.Dispatcher.Invoke(() =>
                        {
                            ListHistory.Add(topGridModel);
                            ListProcess.Add(topGridModel);
                        });
                    }
                    catch (Exception e)
                    {
                        Logger?.Error($"更新界面失败！\n{e}");
                    }


                    //这里判断标签类型（是否需要装箱）
                    var items = obj.Select(s => new DataItem
                    {
                        DataType = s.DataType,
                        VarType = s.VarType,
                        DB = s.DB,
                        StartByteAdr = s.StartByteAdr,
                        BitAdr = s.BitAdr,
                        Count = s.Count,
                        Value = s.Value
                    }).ToList();
                    items[^3].Value = byte.Parse("2");
                    items[^2].Value = !taskInfo.TbReelInfo.IsQualified
                        ? (short)2
                        : short.Parse(taskInfo.ProductOrderInfo.package_info.is_naked ? "2" : "1");
                    await Plc.WriteAsync(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    await Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }
                catch (Exception e)
                {
                    Logger?.Error($"添加任务队列失败：{e}");
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"WeightAndCodeChanged，异常：{e}");
            }
        }

        private async Task<TopGridModel> UpdateProductInfo(MyProductTaskInfo productTaskInfo)
        {
            var product = productTaskInfo.ProductOrderInfo;
            if (!double.TryParse(product!.package_info.tare_weight, out double packageWeight))
            {
                Logger?.Error("获取皮重失败，默认是0.");
            }

            if (!double.TryParse(product!.xpzl_weight, out double reelWeight))
            {
                Logger?.Error("获取reelWeight失败，默认是0.");
            }

            Logger?.Information(
                $"[KEY STEP]ProductCode:{ProductCode},tare_weight:{packageWeight},xpzl_weight:{reelWeight}");
            var netWeight = double.Parse(Weight1 ?? "0") - reelWeight - packageWeight;

            var tReelCode = GetTReelCode(product, netWeight);
            productTaskInfo.TbReelInfo = tReelCode;

            //这里判断是否合格
            var weightValidate = await productTaskInfo.WeightValidate();

            #region 打印盘码（合格证）

            // 箱码 最后五位是重量 放到插入数据那里更新
            // 52.00 -> [0.05200] ->  05200  #####
            //包装条码；产品助记码 + 线盘分组代码 + 用户标准代码 + 包装组编号 + 年月 + 4位流水号 + 装箱净重，
            //eg1:{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}"
            Count++;
            var wei = (int)(100 * netWeight);
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{Count:D4}-{wei:D4}";
            productTaskInfo.GenerateBoxCode = boxCode;
            productTaskInfo.GenerateReelCode = boxCode;
            //打印标签
            var xianPanReportModel = new XianPanReportModel
            {
                Title = "",
                MaterialNo = product.material_number,
                Model = product.material_name,
                Specifications = product.material_spec,
                GrossWeight = tReelCode.GrossWeight.ToString(),
                NetWeight = tReelCode.NetWeight.ToString(),
                BatchNum = tReelCode.BatchNO,
                No = tReelCode.PSN,
                BoxCode = boxCode,
                DateTime = DateTime.Now.ToString("yy-MM-dd"),
                WaterMark = tReelCode.NoQualifiedReason
            };
            var xianPanReportModels = new List<XianPanReportModel>() { xianPanReportModel };
            var reportReel = new ReelReportAuto();
            reportReel.DataSource = xianPanReportModels;
            reportReel.Watermark.Text = xianPanReportModel.WaterMark;
            reportReel.Watermark.ShowBehind = false;
            reportReel.DrawWatermark = !tReelCode.IsQualified;
            reportReel.ExportToImage($"D:\\reel_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\xp_{ProductCode}.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 

            try
            {
                reportReel.Print("152");
                Logger?.Information($"打印盘码（合格证）成功，打印机：152!");
                SendXinJieCmd();
            }
            catch (Exception e)
            {
                Logger?.Information($"打印盘码（合格证）失败，打印机：152！\n{e}");
            }

            #endregion

            #region boxCode 入队

            var boxReportModels = new List<BoxReportModel>()
            {
                new BoxReportModel
                {
                    MaterialNo = tReelCode.CustomerMaterialCode,
                    Model = product.material_name,
                    GBNo = product.material_ns_model,
                    Specifications = product.material_spec,
                    NetWeight = netWeight.ToString("f2"),
                    BatchNum = product.product_order_no,
                    No = tReelCode.PSN, //包装线真实的编码
                    Standard = tReelCode.ProductStandardName,
                    ProductNo = tReelCode.ProductCode,
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    GrossWeight = Weight1,
                    BoxCode = boxCode,
                    WaterMark = tReelCode.NoQualifiedReason,
                    //BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                    ReJi = product.material_thermal_grade,
                    JsbzShortName = product.jsbz_short_name,
                },
            };
            Logger?.Information(
                $"[KEY STEP][PRINT RECORD]:ReJi:{product.material_thermal_grade},JsbzShortName{product.jsbz_short_name}\nproduct:{JsonConvert.SerializeObject(product)}");
            var reportP = new BoxReportAuto();
            reportP.DataSource = boxReportModels;
            reportP.Watermark.Text = boxReportModels.Last().WaterMark;
            reportP.Watermark.ShowBehind = false;
            reportP.DrawWatermark = !tReelCode.IsQualified;
            reportP.ExportToImage($"D:\\box_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\box.png");
            _reportsBox.Enqueue(reportP);

            #endregion


            if (product.package_info.packing_quantity == 0)
            {
                product.package_info.packing_quantity = 1;
                product.package_info.stacking_layers = 2;
                product.package_info.stacking_per_layer = 4;
            }

            #region 更新到界面表格

            var topGridModel = new TopGridModel
            {
                BarCode = tReelCode.PSN,
                ProductCode = tReelCode.ProductCode,
                Weight1 = double.Parse(Weight1 ?? "0"),
                Weight2 = double.Parse(Weight2 ?? "0"),
                NetWeight = (double)tReelCode.NetWeight,
                Result = tReelCode.IsQualified ? "合格" : "不合格",
                Reason = tReelCode.NoQualifiedReason
            };
            return topGridModel;

            #endregion
        }

        private T_preheater_code GetTReelCode(ProductInfo product, double netWeight)
        {
            var tReelCode = new T_preheater_code
            {
                BatchNO = @$"{ProductCode}-{DateTime.Now: MMdd}A",
                CreateTime = DateTime.Now,
                CustomerCode = product.customer_number,
                CustomerId = product.customer_id,
                CustomerMaterialCode = product.customer_material_number,
                CustomerMaterialName = product.customer_material_name,
                CustomerMaterialSpec = product.customer_material_spec,
                CustomerName = product.customer_name,
                GrossWeight = double.Parse(Weight1 ?? "0"),
                ICMOBillNO = product.product_order_no, //生产工单 之前是null，todo:确认是否需要
                IsDel = 0,
                IsQualified = true, //是否合格 默认是合格的
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = netWeight,
                NoQualifiedReason = "",
                OperatorCode = product.operator_code,
                OperatorName = product.operator_name,
                PreheaterCode = product.xpzl_number,
                PreheaterId = product.xpzl_id,
                PreheaterName = product.xpzl_name,
                PreheaterSpec = product.xpzl_spec,
                PreheaterWeight = double.Parse(product.xpzl_weight),
                ProductCode = product.material_number,
                ProductDate = DateTime.Parse(product.product_date), // product.product_date
                ProductGBName = product.material_ns_model,
                ProductId = product.material_id,
                ProductionBarcode = ProductCode,
                ProductionOrgNO = product.product_org_number,
                ProductMnemonicCode = product.material_mnemonic_code,
                ProductName = product.material_name,
                ProductSpec = product.customer_material_spec,
                ProductStandardName = product.material_execution_standard,
                PSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}{0001}",
                Status = 1, //装箱状态
                StockCode = product.stock_number,
                StockId = product.stock_id,
                StockName = product.stock_name,
                UpdateTime = DateTime.Now,
                UserStandardCode = product.jsbz_number,
                UserStandardId = product.jsbz_id,
                UserStandardName = product.jsbz_name,
                WeightUserId = GlobalVar.CurrentUserInfo.userId,
                material_themal_grade = product.material_thermal_grade,
                material_spec = product.material_spec,
                jsbz_short_name = product.jsbz_short_name,
            };
            return tReelCode;
        }


        /// <summary>
        ///     所有任务的异常处理
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="msg"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ErrorStop(int deviceId, string msg)
        {
            Logger.Error($"设备{deviceId}出错，错误信息：{msg}");
        }

        /// <summary>
        ///     2 --> 条码校验
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task ReelCodeChanged(List<DataItem> obj)
        {
            try
            {
                PreScanCode1 = obj[1].Value!.ToString();
                PreScanCode2 = obj[2].Value!.ToString();
                var info = _taskQueue.First(s => s.WorkShop == EnumWorkShop.Weighted);
                info.ReelCode1 = PreScanCode1;
                info.ReelCode2 = PreScanCode2;
                info.WorkShop = EnumWorkShop.ReelCodeChecked;

                if (PreScanCode2 != info.GenerateReelCode && PreScanCode1 != info.GenerateReelCode)
                {
                    ErrorStop((int.Parse(obj[0].Value!.ToString()!)),
                        $"当前条码：{PreScanCode2}|{PreScanCode1} 不等于上位机生成的条码：{info.GenerateReelCode}。");
                    info.TbReelInfo.IsQualified = false;
                    info.TbReelInfo.NoQualifiedReason = "ReelValidateError";
                }

                //这里判断标签类型（是否需要装箱）
                var items = obj.Select(s => new DataItem
                {
                    DataType = s.DataType,
                    VarType = s.VarType,
                    DB = s.DB,
                    StartByteAdr = s.StartByteAdr,
                    BitAdr = s.BitAdr,
                    Count = s.Count,
                    Value = s.Value
                }).ToList();
                items[^3].Value = byte.Parse("2");
                items[^2].Value = !info.TbReelInfo.IsQualified
                    ? short.Parse("2")
                    : short.Parse(info.ProductOrderInfo!.package_info.is_naked ? "2" : "1");
                await Plc.WriteAsync(items.TakeLast(3).ToArray());
                Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                await Plc.ReadMultipleVarsAsync(items);
                Logger?.Information(
                    ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));

                info.WorkShop = EnumWorkShop.ReelCodeChecked;
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        /// <summary>
        ///     3 --> 条箱子到达位置，贴箱码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task BoxArrived(List<DataItem> arg)
        {
            try
            {
                //1 首先把码垛指令清空，等待箱码校验完成之后写入新的指令
                ClearStackCmd();

                var info = _taskQueue.FirstOrDefault(s => s.WorkShop == EnumWorkShop.ReelCodeChecked);
                if (info == null)
                {
                    Logger?.Error("_productInfoQueue队列里没有【WorkShop == EnumWorkShop.ReelCodeChecked】任务");
                    return;
                }

                if (_reportsBox.Count == 0)
                {
                    Logger?.Error($"_reportsBox队列中没有对应的箱码标签，不做任何处理！");
                    return;
                }

                var reportP = _reportsBox.Dequeue();

                if (info.TbReelInfo.IsQualified)
                {
                    //这里判断标签类型（打印几个，然后通知PLC放行）
                    if ((bool)!info.ProductOrderInfo!.package_info.is_naked) //如果不是裸装 打印侧贴
                    {
                        reportP.Print("161");
                    }

                    //顶贴必须要打印
                    reportP.Print("160");
                    Thread.Sleep(5000);
                }


                //1 发送规格之后才能放行
                var pmesStacking = new PmesStacking
                {
                    WorkPositionId = 0,
                    ReelSpecification = 1,
                    StackModel = 0,
                    PmesAndPlcReadWriteFlag = 2
                };
                var plcCmdAttribute = pmesStacking!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                if (plcCmdAttribute != null)
                {
                    var dbBlock = plcCmdAttribute.DbBlock;
                    Plc.WriteClass(pmesStacking, dbBlock);
                }

                //2 放行
                var items = arg.Select(s => new DataItem
                {
                    DataType = s.DataType,
                    VarType = s.VarType,
                    DB = s.DB,
                    StartByteAdr = s.StartByteAdr,
                    BitAdr = s.BitAdr,
                    Count = s.Count,
                    Value = s.Value
                }).ToList();
                items[^3].Value = byte.Parse("2");
                _reverse2 = (ushort)(!info.TbReelInfo.IsQualified ? 2 :
                    info.ProductOrderInfo!.package_info.is_naked ? 2 : 1);
                items[^1].Value = _reverse2;
                await Plc.WriteAsync(items.TakeLast(3).ToArray());
                Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                await Plc.ReadMultipleVarsAsync(items);
                Logger?.Information(
                    ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        /// <summary>
        ///     4 --> 贴完箱码，箱码校验
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task BoxBarCodeChanged(List<DataItem> obj)
        {
            try
            {
                BoxCode1 = obj[1].Value!.ToString();
                BoxCode2 = obj[2].Value!.ToString();
                var info = _taskQueue.First(s => s.WorkShop == EnumWorkShop.ReelCodeChecked);

                //不会出现
                if (string.IsNullOrEmpty(BoxCode1) && string.IsNullOrEmpty(BoxCode2))
                {
                    info.TbReelInfo.IsQualified = false;
                    info.TbReelInfo.NoQualifiedReason = "NoBoxCode";
                }

                if (BoxCode1!.Equals("888") || BoxCode2!.Equals("888"))
                {
                    info.TbReelInfo.IsQualified = false;
                    info.TbReelInfo.NoQualifiedReason = "ScanError";
                }


                if (await _fSql.Insert(info.TbReelInfo).ExecuteAffrowsAsync() > 0)
                {
                    Logger?.Information($"{info.ProductOrderBarCode} 盘码数据写入数据库成功！");
                }
                else
                {
                    ShowError($"{info.ProductOrderBarCode} 盘码数据写入数据库失败！");
                }

                //判断是否Ng
                var items = obj.Select(s => new DataItem
                {
                    DataType = s.DataType,
                    VarType = s.VarType,
                    DB = s.DB,
                    StartByteAdr = s.StartByteAdr,
                    BitAdr = s.BitAdr,
                    Count = s.Count,
                    Value = s.Value
                }).ToList();
                items[^3].Value = _readFlag; //已读

                if (info.TbReelInfo.IsQualified) //合格的话就根据是否装箱来
                {
                    //1-->执行正常码垛指令
                    if (!ExecuteStack())
                    {
                        await Task.Delay(2500);
                        if (!ExecuteStack())
                        {
                            Logger?.Error("码垛指令执行失败");
                        }
                    }

                    //1装箱  2不装箱
                    _reverse2 = (ushort)(!info.TbReelInfo.IsQualified ? 2 :
                        info.ProductOrderInfo!.package_info.is_naked ? 2 : 1);
                    items[^1].Value = _reverse2;
                    //2-->放行
                    await Plc.WriteAsync(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    await Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }
                else //不合格
                {
                    //2 发送错误的码垛指令
                    await Plc.WriteClassAsync(_pmesStackingError, 540);
                    Logger?.Information($"发送新的码垛指令,码垛到异常工位:\n{_pmesStackingError}");

                    //2 通知放行
                    _reverse2 = 2; //Ng,不装箱
                    items[^1].Value = _reverse2;
                    await Plc.WriteAsync(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    await Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }

                info.WorkShop = EnumWorkShop.BoxCodeChecked;
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        /// <summary>
        ///     5 --> 码垛完成后
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task BoxStacked(List<DataItem> arg)
        {
            try
            {
                var info = _taskQueue.FirstOrDefault(s => s.WorkShop == EnumWorkShop.BoxCodeChecked);
                if (info != null)
                {
                    info.WorkShop = EnumWorkShop.Stacked;
                    Logger?.Information($"{info.ProductOrderBarCode} 码垛完成！码垛位置：{arg[1]?.Value?.ToString()}");
                }

                ReelCount++;
                var plcCommand = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 1, Status: 2 });
                if (plcCommand != null)
                {
                    plcCommand.Status = 1;
                    Logger?.Information($"拆垛指令执行完毕：{plcCommand.PlcComandContent}");

                    var cmd = JsonConvert.DeserializeObject<PmesCmdUnStacking>(plcCommand.PlcComandContent);
                    if (cmd?.ReelNum == ReelCount)
                    {
                        await _fSql.Update<T_plc_command>().Set(s => s.Status, 1)
                            .Where(s => s.Id == plcCommand.Id).ExecuteAffrowsAsync();
                        _plcCommandsRunning.Remove(plcCommand);
                        Logger?.Information($"[KEY STEP]移除拆垛指令:{JsonConvert.SerializeObject(plcCommand)}");
                        ReelCount = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
            }
        }

        /// <summary>
        ///     执行队列中正常的堆垛指令
        /// </summary>
        /// <returns></returns>
        public bool ExecuteStack()
        {
            //var plcCommand = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });
            var plcCommand = _plcCommandsRunning.FirstOrDefault(s => s is { PlcComandType: 2 });
            if (plcCommand == null) //如果队列中没有需要的指令，则直接返回
            {
                Logger?.Error("队列中没有满足条件的码垛指令。");
                return false;
            }

            var writeObject =
                JsonConvert.DeserializeObject<PmesStacking>(plcCommand.PlcComandContent);
            var plcCmdAttribute = writeObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
            if (plcCmdAttribute != null)
            {
                var dbBlock = plcCmdAttribute.DbBlock;
                Plc.WriteClass(writeObject, dbBlock);
            }

            return true;
        }

        /// <summary>
        ///     当box到达的时候，清空码垛指令。
        ///     当箱码校验完毕，发送新的指令。
        /// </summary>
        public void ClearStackCmd()
        {
            var pmesStacking = new PmesStacking
            {
                WorkPositionId = 0,
                ReelSpecification = 0,
                StackModel = 0,
                PmesAndPlcReadWriteFlag = 2
            };
            var plcCmdAttribute = pmesStacking!.GetType().GetCustomAttribute<PlcCmdAttribute>();
            if (plcCmdAttribute != null)
            {
                var dbBlock = plcCmdAttribute.DbBlock;
                Plc.WriteClass(pmesStacking, dbBlock);
            }
        }

        #endregion

        #region 表示状态

        /// <summary>
        ///     第一次扫码
        /// </summary>
        [ObservableProperty] private Status? _barCodeScannerStatus1 = Status.Unknown;

        /// <summary>
        ///     扫描校验
        /// </summary>
        [ObservableProperty] private Status? _barCodeScannerStatus2 = Status.Unknown;

        /// <summary>
        ///     第一次称重
        /// </summary>
        [ObservableProperty] private Status? _weighingStatus1 = Status.Unknown;

        /// <summary>
        ///     称重校验
        /// </summary>
        [ObservableProperty] private Status? _weighingStatus2 = Status.Unknown;

        /// <summary>
        ///     打印机
        /// </summary>
        [ObservableProperty] private Status? _printerStatus = Status.Unknown;

        /// <summary>
        ///     条码校验结果
        /// </summary>
        [ObservableProperty] private bool? _barCodeCheck = false;

        /// <summary>
        ///     PLC状态
        /// </summary>
        [ObservableProperty] private Status? _plcStatus = Status.Unknown;

        /// <summary>
        ///     服务器状态
        /// </summary>
        [ObservableProperty] private Status? _serviceStatus = Status.Unknown;

        /// <summary>
        ///     所有设备（工作指示灯）
        /// </summary>
        [ObservableProperty] private Status? _allStatus = Status.Unknown;

        #endregion

        #region cmds

        [RelayCommand]
        private void Close()
        {
            Application.Current.Shutdown();
            //Application.Current.Shutdown();
        }

        #endregion

        #region 信捷PLC

        [ObservableProperty]
        private ObservableCollection<ModbusCmd> _modbusCmds = new ObservableCollection<ModbusCmd>()
        {
            new() //数量
            {
                DeviceId = 1,
                Address = 1000,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 1
            },
            new() //类型
            {
                DeviceId = 1,
                Address = 1002,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 1
            },
            new() //偏移量
            {
                DeviceId = 1,
                Address = 1004,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new() //预留位
            {
                DeviceId = 1,
                Address = 1006,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new() //预留位
            {
                DeviceId = 1,
                Address = 1008,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new() //是否贴完
            {
                DeviceId = 1,
                Address = 800,
                Method = ModbusMethods.ReadCoils,
                Value = 0
            },
        };


        [RelayCommand]
        private void SendXinJieCmd()
        {
            Logger?.Verbose($"信捷PLC写指令，长度：{ModbusCmds.Count}");
            foreach (var modbusCmd in ModbusCmds)
            {
                try
                {
                    var isSuccess = PlcXj.WriteOneRegister(modbusCmd.Address.ToString(), modbusCmd.Value)
                        .IsSuccess;
                    Logger?.Information(
                        $"信捷PLC写指令,address = {modbusCmd.Address},value = {modbusCmd.Value}执行结果:{isSuccess}");


                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Logger?.Error($"信捷写入错误:{e.Message}");
                }
            }
        }

        [RelayCommand]
        private void ReadXinJieCmd()
        {
            foreach (var modbusCmd in ModbusCmds)
            {
                switch (modbusCmd.Method)
                {
                    case ModbusMethods.ReadCoils:
                        var ret1 = PlcXj.ReadCoil(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = (short)(ret1[0] ? 1 : 0);
                        break;
                    case ModbusMethods.ReadHoldingRegisters:
                        var ret2 = PlcXj.Read(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = ret2[0];
                        break;
                    case ModbusMethods.ReadInputRegisters:
                        var ret3 = PlcXj.Read(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = ret3[0];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Thread.Sleep(50);
            }
        }

        #endregion

        #region Debug

        private T_preheater_code GetReelCode(ProductInfo product)
        {
            var t = new T_preheater_code
            {
                BatchNO = @$"{ProductCode}-{DateTime.Now: MMdd}A",
                CreateTime = DateTime.Now,
                CustomerCode = product.customer_number,
                CustomerId = product.customer_id,
                CustomerMaterialCode = product.customer_material_number,
                CustomerMaterialName = product.customer_material_name,
                CustomerMaterialSpec = product.customer_material_spec,
                CustomerName = product.customer_name,
                GrossWeight = double.Parse(Weight1 ?? "0"),
                ICMOBillNO = product.product_order_no, //生产工单 之前是null，todo:确认是否需要
                IsDel = 0,
                IsQualified = true, //是否合格 默认是合格的
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = double.Parse(Weight1 ?? "0") - 0d,
                NoQualifiedReason = "TEST",
                OperatorCode = product.operator_code,
                OperatorName = product.operator_name,
                PreheaterCode = product.xpzl_number,
                PreheaterId = product.xpzl_id,
                PreheaterName = product.xpzl_name,
                PreheaterSpec = product.xpzl_spec,
                PreheaterWeight = double.Parse(product.xpzl_weight),
                ProductCode = product.material_number,
                ProductDate = DateTime.Parse(product.product_date), // product.product_date
                ProductGBName = product.material_ns_model,
                ProductId = product.material_id,
                ProductionBarcode = ProductCode,
                ProductionOrgNO = product.product_org_number,
                ProductMnemonicCode = product.material_mnemonic_code,
                ProductName = product.material_name,
                ProductSpec = product.customer_material_spec,
                ProductStandardName = product.material_execution_standard,
                PSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}{0001}",
                Status = 1, //装箱状态
                StockCode = product.stock_number,
                StockId = product.stock_id,
                StockName = product.stock_name,
                UpdateTime = DateTime.Now,
                UserStandardCode = product.jsbz_number,
                UserStandardId = product.jsbz_id,
                UserStandardName = product.jsbz_name,
                Weight1 = 0,
                WeightUserId = GlobalVar.CurrentUserInfo.userId
            };
            return t;
        }

        [RelayCommand]
        private void PrintReelCode()
        {
            var product = new ProductInfo();
            var tPreheaterCode = GetReelCode(product);
            var wei = (int)(100 * double.Parse(Weight1));
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{1000}-{wei:D4}";
            //打印标签
            var xianPanReportModel = new XianPanReportModel
            {
                Title = "",
                MaterialNo = product.material_number,
                Model = product.xpzl_spec,
                Specifications = tPreheaterCode.PreheaterSpec,
                GrossWeight = tPreheaterCode.GrossWeight.ToString(),
                NetWeight = tPreheaterCode.NetWeight.ToString(),
                BatchNum = tPreheaterCode.BatchNO,
                No = tPreheaterCode.PSN,
                BoxCode = boxCode,
                DateTime = DateTime.Now.ToString("yy-MM-dd"),
                WaterMark = tPreheaterCode.NoQualifiedReason
            };
            var xianPanReportModels = new List<XianPanReportModel>() { xianPanReportModel };
            var reportReel = new ReelReportAuto();
            reportReel.DataSource = xianPanReportModels;
            reportReel.Watermark.Text = xianPanReportModel.WaterMark;
            reportReel.Watermark.ShowBehind = false;
            reportReel.DrawWatermark = !tPreheaterCode.IsQualified;
            reportReel.ExportToImage($"D:\\reel_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\xp_{ProductCode}.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 

            reportReel.Print("152");

            Logger?.Information($"调用默认打印机打印!");
            SendXinJieCmd();
        }

        [RelayCommand]
        private void PrintBoxCode()
        {
            if (_reportsBox.Count == 0)
            {
                var boxReportModels = new List<BoxReportModel>()
                {
                    new BoxReportModel
                    {
                        MaterialNo = "22222222222",
                        Model = "22222222222",
                        Specifications = "22222222222",
                        NetWeight = null,
                        BatchNum = "22222222222",
                        No = "22222222222",
                        Standard = "22222222222",
                        ProductNo = "22222222222",
                        BoxCode = Guid.NewGuid().ToString().Substring(1, 10),
                        DateTime = "22222222222",
                        GrossWeight = "22222222222",
                        WaterMark = "TEST"
                    }
                };
                var reportP = new BoxReportAuto();
                //var reportP = new TemplateBox();
                reportP.Watermark.ShowBehind = false;
                reportP.DataSource = boxReportModels;
                reportP.Watermark.Text = boxReportModels.Last().WaterMark;
                reportP.Watermark.ShowBehind = false;
                reportP.DrawWatermark = true;
                reportP.ExportToImage($"D:\\box_{ProductCode}.png");
                reportP.Print("161");
                reportP.Print("160");
                _reportsBox.Enqueue(reportP);
            }


            var reportP1 = _reportsBox.Dequeue();
            reportP1.Print("161");
            reportP1.Print("160");
        }

        [RelayCommand]
        private void AddData()
        {
            var topGridModel = new TopGridModel
            {
                BarCode = Guid.NewGuid().ToString().Substring(1, 10),
                ProductCode = Guid.NewGuid().ToString().Substring(1, 10),
                Weight1 = new Random().NextDouble() * 25,
                Weight2 = new Random().NextDouble() * 25,
                NetWeight = 0,
                CreateTime = DateTime.Now,
                Result = null
            };
            ListProcess.Add(topGridModel);
            ListHistory.Add(topGridModel);
        }

        [RelayCommand]
        private void AddTask()
        {
            var myProductTaskInfo = new MyProductTaskInfo
            {
                ProductOrderBarCode = "test",
                WorkShop = EnumWorkShop.None,
                ProductOrderInfo = new ProductInfo(),
                Weight1 = 0,
                Weight2 = 0,
                ReelCode1 = "t1",
                ReelCode2 = "t2",
                BoxCode1 = "null",
                BoxCode2 = "null",
                GenerateReelCode = "null",
                GenerateBoxCode = "null",
                TbReelInfo = new T_preheater_code(),
                TbBoxInfo = new T_box(),
                ReportReel = new ReelReportAuto(),
                ReportTop = new BoxReportAuto(),
                ReportEdge = new BoxReportAuto(),
                NeedPack = true
            };
            GlobalVar.MainView.Dispatcher.Invoke(() => { TaskQueue.Add(myProductTaskInfo); });
        }

        #endregion

        private void ShowError(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowAlarm(string msg)
        {
            Logger?.Warning(msg);
            MessageBox.Show(msg, "Alarm", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }

    public class MyProductTaskInfo
    {
        /// <summary>
        ///     产品订单编号
        /// </summary>
        public string ProductOrderBarCode { get; set; } = "";

        /// <summary>
        ///     产品所处位置
        /// </summary>
        public EnumWorkShop WorkShop { get; set; }

        /// <summary>
        ///     根据ProductBarCode获取到的产品信息
        /// </summary>
        public ProductInfo ProductOrderInfo { get; set; } = new ProductInfo();

        #region 称重条码数据

        /// <summary>
        ///     称1的原始重量
        /// </summary>
        public double Weight1 { get; set; }

        /// <summary>
        ///     称2的原始重量
        /// </summary>
        public double Weight2 { get; set; }

        #endregion

        #region code 复核

        /// <summary>
        ///     校验时顶部会扫到盘码（合格证）和产品生产码，1/2只要有一个合格就Ok
        /// </summary>
        public string? ReelCode1 { get; set; }

        public string? ReelCode2 { get; set; }


        public string? BoxCode1 { get; set; }
        public string? BoxCode2 { get; set; }

        #endregion

        #region 生成的条码

        public string? GenerateReelCode { get; set; }
        public string? GenerateBoxCode { get; set; }

        #endregion

        #region 盘码和箱码的信息也保存下

        /// <summary>
        ///     线盘顶部盘码信息（合格证）
        ///     包括毛重，净重，是否合格，不合格原因
        /// </summary>
        public T_preheater_code TbReelInfo { get; set; } = new T_preheater_code();

        public T_box TbBoxInfo { get; set; } = new T_box();

        #endregion

        #region 标签信息

        public XtraReport? ReportReel { get; set; }
        public XtraReport? ReportTop { get; set; }
        public XtraReport? ReportEdge { get; set; }

        #endregion

        public bool NeedPack { get; set; }

        /// <summary>
        ///     盘码校验
        /// </summary>
        public bool ReelCodeValidate => (ReelCode1 == GenerateReelCode || ReelCode2 == GenerateReelCode) &&
                                        (!string.IsNullOrEmpty(GenerateReelCode));


        /// <summary>
        ///     箱码校验
        /// </summary>
        public bool BoxCodeValidate => (BoxCode1 == GenerateBoxCode || BoxCode1 == GenerateBoxCode) &&
                                       (!string.IsNullOrEmpty(GenerateBoxCode));


        public async Task<bool> WeightValidate()
        {
            Debug.Assert(TbReelInfo != null, nameof(TbReelInfo) + " != null");

            switch (ProductOrderInfo!.product_order_no)
            {
                case "777":
                    TbReelInfo.NoQualifiedReason = "TEST";
                    TbReelInfo.IsQualified = false;
                    break;
                case "888":
                    TbReelInfo.NoQualifiedReason = "Scan Error";
                    TbReelInfo.IsQualified = false;
                    break;
                case "999":
                    TbReelInfo.NoQualifiedReason = "API ERROR";
                    TbReelInfo.IsQualified = false;
                    break;
                default:
                    {
                        try
                        {
                            var validateOrder =
                                await ValidateOrder(TbReelInfo.NetWeight, ProductOrderBarCode!);
                            if (validateOrder.Item1) return true;
                            TbReelInfo.NoQualifiedReason = validateOrder.Item2;
                            TbReelInfo.IsQualified = false;
                        }
                        catch (Exception e)
                        {
                            TbReelInfo.NoQualifiedReason = "API ERROR";
                            TbReelInfo.IsQualified = false;
                        }

                        break;
                    }
            }

            return false;
        }


        private async Task<Tuple<bool, string>> ValidateOrder(double weight, string order)
        {
            var validate =
                await WebService.Instance.GetJObjectValidate(
                    $"{ApiUrls.ValidateOrder}net_weight={weight:F2}&semi_finished={order}");

            return validate;
        }
    }

    public enum EnumWorkShop
    {
        None = 0,
        UnStacking,
        Weighted,
        ReelCodeChecked,
        BoxCodeChecked,
        Stacking,
        Stacked,
    }

    public class TopGridModel : ObservableObject
    {
        public string? BarCode { get; set; }
        public string? ProductCode { get; set; }
        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public double NetWeight { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string Result { get; set; } = "";
        public string Reason { get; set; } = "";
    }
}