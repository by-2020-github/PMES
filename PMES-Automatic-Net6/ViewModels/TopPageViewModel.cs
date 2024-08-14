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
        private Queue<T_plc_command> _plcCommands = new Queue<T_plc_command>();

        [ObservableProperty] private string? _productCode = "";
        [ObservableProperty] private string? _preScanCode1 = "s1";
        [ObservableProperty] private string? _preScanCode2 = "s2";
        [ObservableProperty] private string? _boxCode1 = "s1";
        [ObservableProperty] private string? _boxCode2 = "s2";
        [ObservableProperty] private string? _weight1 = "2.22";
        [ObservableProperty] private string? _weight2 = "3.55";

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
            HardwareManager.Instance.OnBoxArrived += BoxStacked;
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Task.Delay(2000);
                        //拆垛
                        if (_plcCommands.Count(s => s.PlcComandType == 1) == 0) //添加任务
                        {
                            var plcCommand = _fSql.Select<T_plc_command>()
                                .Where(s => s.PlcComandType == 1 && s.Status == 0).First();
                            if (plcCommand != null)
                            {
                                _plcCommands.Enqueue(plcCommand);
                            }
                        }
                        else //执行任务
                        {
                            var plcCommand = _plcCommands.FirstOrDefault(s => s is { PlcComandType: 1, Status: 0 });
                            if (plcCommand != null && plcCommand.Status != 2)
                            {
                                var writeObject =
                                    JsonConvert.DeserializeObject<PmesCmdUnStacking>(plcCommand.PlcComandContent);
                                var plcCmdAttribute = writeObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                                if (plcCmdAttribute != null)
                                {
                                    var dbBlock = plcCmdAttribute.DbBlock;
                                    Plc.WriteClass(writeObject, dbBlock);
                                }

                                plcCommand.Status = 2;
                            }
                        }

                        //码垛
                        if (_plcCommands.Count(s => s.PlcComandType == 2) == 0)
                        {
                            var plcCommand = _fSql.Select<T_plc_command>()
                                .Where(s => s.PlcComandType == 2 && s.Status == 0).First();
                            if (plcCommand != null)
                            {
                                _plcCommands.Enqueue(plcCommand);
                            }
                        }
                        else
                        {
                            var plcCommand = _plcCommands.FirstOrDefault(s => s is { PlcComandType: 2, Status: 0 });
                            if (plcCommand != null && plcCommand.Status != 2)
                            {
                                var writeObject =
                                    JsonConvert.DeserializeObject<PmesStacking>(plcCommand.PlcComandContent);
                                var plcCmdAttribute = writeObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                                if (plcCmdAttribute != null)
                                {
                                    var dbBlock = plcCmdAttribute.DbBlock;
                                    Plc.WriteClass(writeObject, dbBlock);
                                }

                                plcCommand.Status = 2;
                            }
                        }

                        //子母托
                        if (_plcCommands.Count(s => s.PlcComandType == 3) == 0)
                        {
                            var plcCommand = _fSql.Select<T_plc_command>()
                                .Where(s => s.PlcComandType == 3 && s.Status == 0).First();
                            if (plcCommand != null)
                            {
                                _plcCommands.Enqueue(plcCommand);
                            }
                        }
                        else
                        {
                            var plcCommand = _plcCommands.FirstOrDefault(s => s is { PlcComandType: 3, Status: 0 });
                            if (plcCommand != null && plcCommand.Status != 2)
                            {
                                var writeObject =
                                    JsonConvert.DeserializeObject<PmesCmdCombinationMotherChildTray>(plcCommand
                                        .PlcComandContent);
                                var plcCmdAttribute = writeObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
                                if (plcCmdAttribute != null)
                                {
                                    var dbBlock = plcCmdAttribute.DbBlock;
                                    Plc.WriteClass(writeObject, dbBlock);
                                }

                                plcCommand.Status = 2;
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

        private System.Collections.Concurrent.ConcurrentQueue<MyProductInfo> _productInfoQueue = new();

        /// <summary>
        ///     PLC状态监测
        /// </summary>
        /// <param name="obj"></param>
        private async Task Handler(object obj)
        {
            Logger?.Verbose($"状态改变:\n\t{obj}");
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
                if (int.Parse(obj[4].Value?.ToString() ?? "0") == 2)
                {
                    ErrorStop(int.Parse(obj[0].Value.ToString()), $"WeightAndCodeChanged 当前标志位是：{obj[4]},其它信息却发生了更改。");
                    return;
                }

                Weight1 = ((double.Parse(obj[2].Value.ToString())) / 100d).ToString("f2");
                Weight2 = ((double.Parse(obj[3].Value.ToString())) / 100d).ToString("f2");
              
                ProductCode = obj[1].Value.ToString();

                if (string.IsNullOrEmpty(ProductCode))
                {
                    ErrorStop((int.Parse(obj[0].Value.ToString())), "获取到错误的条码");
                }

                ProductInfo product = null;
                try
                {
                    product = await WebService.Instance.Get<ProductInfo>(
                        $"{ApiUrls.QueryOrder}{ProductCode}&format=json");
                }
                catch (Exception e)
                {
                    //todo: 以后会校验错误 先放过
                    Logger?.Error($"获取不到条码！new  一个临时默认的。\n{e}");
                    product = new ProductInfo();
                }


                try
                {
                    //任务队列添加对象
                    var myProductInfo = new MyProductInfo
                    {
                        Weight1 = (double.Parse(obj[2].Value.ToString())),
                        Weight2 = (double.Parse(obj[3].Value.ToString())),
                        ProductBarCode = obj[1].Value.ToString(),
                        WorkShop = EnumWorkShop.Weighted,
                        ProductInfo = product
                    };
                    _productInfoQueue.Enqueue(myProductInfo);
                    await UpdateProductInfo(myProductInfo);

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
                    items[^2].Value = short.Parse(myProductInfo.ProductInfo.package_info.is_naked ? "2" : "1");
                    Plc.Write(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    Plc.ReadMultipleVarsAsync(items);
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

        private async Task UpdateProductInfo(MyProductInfo productInfo, bool review = false, string order = "")
        {
            var product = productInfo.ProductInfo;
            if (!double.TryParse(product.package_info.tare_weight, out double packageWeight))
            {
                Logger?.Error("获取皮重失败，默认是0.");
            }

            var tPreheaterCode = new T_preheater_code
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
                IsQualified = 1, //是否合格 默认是合格的
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = double.Parse(Weight1 ?? "0") - packageWeight,
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
                Weight1 = null,
                WeightUserId = GlobalVar.CurrentUserInfo.userId
            };
            productInfo.PreheaterCode = tPreheaterCode;
            //这里判断是否合格
            try
            {
                if (product.product_order_no.Equals("777"))
                {
                    tPreheaterCode.NoQualifiedReason = "TEST";
                    tPreheaterCode.IsQualified = 0;
                }
                else if (product.product_order_no.Equals("999"))
                {
                    tPreheaterCode.NoQualifiedReason = "API ERROR";
                    tPreheaterCode.IsQualified = 0;
                }
                else if (string.IsNullOrEmpty(ProductCode))
                {
                    tPreheaterCode.NoQualifiedReason = "No BarCode";
                    tPreheaterCode.IsQualified = 0;
                }
                else
                {
                    var validateOrder = await ValidateOrder(double.Parse(Weight1 ?? "0"), ProductCode);
                    if (!validateOrder.Item1)
                    {
                        tPreheaterCode.NoQualifiedReason = validateOrder.Item2;
                        tPreheaterCode.IsQualified = 0;
                    }
                }
            }
            catch (Exception e)
            {
                tPreheaterCode.NoQualifiedReason = "API ERROR";
                tPreheaterCode.IsQualified = 0;
                Logger?.Error($"校验失败！\n{e}");
            }

            #region 打印盘码（合格证）

            // 箱码 最后五位是重量 放到插入数据那里更新
            // 52.00 -> [0.05200] ->  05200  #####
            //包装条码；产品助记码 + 线盘分组代码 + 用户标准代码 + 包装组编号 + 年月 + 4位流水号 + 装箱净重，
            //eg1:{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}"
            Count++;
            var wei = (int)(100 * double.Parse(Weight1));
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{Count:D4}-{wei:D4}";
            productInfo.GenerateBoxCode = boxCode;
            productInfo.GenerateReelCode = boxCode;
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
            reportReel.DrawWatermark = tPreheaterCode.IsQualified == 1;
            reportReel.ExportToImage($"D:\\reel_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\xp_{ProductCode}.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 
            reportReel.Print("152");
            Logger?.Information($"打印盘码（合格证），打印机：152!");
            SendXinJieCmd();

            #endregion

            #region boxCode 入队

            var boxReportModels = new List<BoxReportModel>()
            {
                new BoxReportModel
                {
                    MaterialNo = tPreheaterCode.CustomerMaterialCode,
                    Model = tPreheaterCode.ProductSpec,
                    Specifications = tPreheaterCode.PreheaterSpec,
                    NetWeight = Weight1,
                    BatchNum = "1",
                    No = tPreheaterCode.PSN,
                    Standard = tPreheaterCode.ProductStandardName,
                    ProductNo = tPreheaterCode.ProductCode,
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    GrossWeight = Weight1,
                    BoxCode = boxCode,
                    WaterMark = tPreheaterCode.NoQualifiedReason
                    //BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                },
            };
            var reportP = new TemplateBox();
            reportP.DataSource = boxReportModels;
            reportP.Watermark.Text = boxReportModels.Last().WaterMark;
            reportP.Watermark.ShowBehind = false;
            reportP.DrawWatermark = tPreheaterCode.IsQualified == 1;
            reportP.ExportToImage($"D:\\box_{ProductCode}.png");
            Logger?.Information($"导出模板:D:\\box.png");
            _reportsBox.Enqueue(reportP);

            #endregion


            #region 更新到界面表格

            var topGridModel = new TopGridModel
            {
                BarCode = tPreheaterCode.PSN,
                ProductCode = tPreheaterCode.ProductCode,
                Weight1 = double.Parse(Weight1 ?? "0"),
                Weight2 = double.Parse(Weight2 ?? "0"),
                NetWeight = (double)tPreheaterCode.NetWeight,
                Result = tPreheaterCode.IsQualified == 1 ? "合格" : "不合格"
            };
            ListProcess.Add(topGridModel);
            ListHistory.Add(topGridModel);

            #endregion


            if (product.package_info.packing_quantity == 0)
            {
                product.package_info.packing_quantity = 1;
                product.package_info.stacking_layers = 2;
                product.package_info.stacking_per_layer = 4;
            }
        }

        private async Task<Tuple<bool, string>> ValidateOrder(double weight, string order)
        {
            var validate =
                await WebService.Instance.GetJObjectValidate(
                    $"{ApiUrls.ValidateOrder}net_weight={weight:F2}&semi_finished={order}");

            return validate;
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
                if ((int.Parse(obj[3].Value.ToString())) == 2)
                {
                    ErrorStop((int.Parse(obj[0].Value.ToString())), $"ReelCodeChanged 当前标志位是：{obj[4]},其它信息却发生了更改。");
                    return;
                }

                PreScanCode1 = obj[1].Value.ToString();
                PreScanCode2 = obj[2].Value.ToString();
                var info = _productInfoQueue.First(s => s.WorkShop == EnumWorkShop.Weighted);
                info.ReelCode1 = PreScanCode1;
                info.ReelCode2 = PreScanCode2;
                info.WorkShop = EnumWorkShop.ReelCodeChecked;
                if (PreScanCode2 != info.GenerateReelCode)
                {
                    ErrorStop((int.Parse(obj[0].Value.ToString())),
                        $"当前条码：{PreScanCode2} 不等于上位机生成的条码：{info.GenerateReelCode}。");
                    //return;
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
                items[^2].Value = short.Parse(info.ProductInfo.package_info.is_naked ? "2" : "1");
                Plc.Write(items.TakeLast(3).ToArray());
                Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                Plc.ReadMultipleVarsAsync(items);
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
                var info = _productInfoQueue.FirstOrDefault(s => s.WorkShop == EnumWorkShop.ReelCodeChecked);
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

                //这里判断标签类型（打印几个，然后通知PLC放行）
                if ((bool)!info.ProductInfo!.package_info.is_naked) //如果不是裸装 打印侧贴
                {
                    reportP.Print("161");
                }

                //顶贴必须要打印
                reportP.Print("160");

                //这里判断标签类型（是否需要装箱）
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
                Plc.Write(items.TakeLast(3).ToArray());
                Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                Plc.ReadMultipleVarsAsync(items);
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
                if ((int)obj[3].Value == 2)
                {
                    ErrorStop((int.Parse(obj[0].Value.ToString())),
                        $"BoxBarCodeChanged 当前标志位是：{obj[4].Value},其它信息却发生了更改。");
                    return;
                }

                BoxCode1 = obj[1].Value.ToString();
                BoxCode2 = obj[2].Value.ToString();
                var info = _productInfoQueue.First(s => s.WorkShop == EnumWorkShop.ReelCodeChecked);

                if (string.IsNullOrEmpty(BoxCode1))
                {
                    if (BoxCode1 != info.GenerateBoxCode)
                    {
                        ErrorStop((int.Parse(obj[0].Value.ToString())),
                            $"当前条码1：{BoxCode1} 不等于生成的条码：{info.GenerateBoxCode}。");
                        //return;
                    }
                }

                if (string.IsNullOrEmpty(BoxCode2))
                {
                    if (BoxCode2 != info.GenerateBoxCode)
                    {
                        ErrorStop((int.Parse(obj[0].Value.ToString())),
                            $"当前条码2：{BoxCode1} 不等于生成的条码：{info.GenerateBoxCode}。");
                        //return;
                    }
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

                if (info.PreheaterCode.IsQualified == 1) //合格
                {
                    _reverse2 = 1; //Ok
                    items[^2].Value = _reverse2;
                    Plc.Write(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    Plc.ReadMultipleVarsAsync(items);
                    Logger?.Information(
                        ($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2读取结果：\n{HardwareManager.PrintDataItems(items.TakeLast(3).ToList())}"));
                }
                else //不合格
                {
                    //1 先更改码垛指令
                    var firstOrDefault = _plcCommands.FirstOrDefault(s => s is { PlcComandType: 2, Status: 2 });
                    if (firstOrDefault != null)
                    {
                        firstOrDefault.Status = 3; //先暂停这个码垛指令
                    }

                    //2 发送错误的码垛指令
                    Plc.WriteClass(_pmesStackingError, 540);
                    Logger.Information($"发送新的码垛指令,码垛到异常工位:\n{_pmesStackingError}");

                    //3 通知放行
                    _reverse2 = 2; //Ng
                    items[^2].Value = _reverse2;
                    Plc.Write(items.TakeLast(3).ToArray());
                    Logger?.Information($"DB{items[^3].DB}.{items[^3].StartByteAdr} 标志位置2！");
                    Plc.ReadMultipleVarsAsync(items);
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
                var info = _productInfoQueue.FirstOrDefault(s => s.WorkShop == EnumWorkShop.BoxCodeChecked);
                if (info != null)
                {
                    info.WorkShop = EnumWorkShop.Stacked;
                }

                //1 恢复码垛指令
                var firstOrDefault = _plcCommands.FirstOrDefault(s => s is { PlcComandType: 2, Status: 3 });
                if (firstOrDefault != null)
                {
                    firstOrDefault.Status = 0; //这里置0，队列会重新检测到 开始执行
                    Logger?.Information($"恢复之前的拆垛指令:\n{firstOrDefault.PlcComandContent}");
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"异常:\n{e}");
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

        [ObservableProperty] private ObservableCollection<ModbusCmd> _modbusCmds = new ObservableCollection<ModbusCmd>()
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
                IsQualified = 0, //是否合格 默认是合格的
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
                Weight1 = null,
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
            reportReel.DrawWatermark = tPreheaterCode.IsQualified != 1;
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

        #endregion
    }

    public class MyProductInfo
    {
        public EnumWorkShop WorkShop { get; set; }

        /// <summary>
        ///     根据ProductBarCode获取到的产品信息
        /// </summary>
        public ProductInfo? ProductInfo { get; set; }

        #region 称重条码数据

        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public string? ProductBarCode { get; set; }

        #endregion

        #region code 复核

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

        public T_preheater_code PreheaterCode { get; set; }
        public T_box TBox { get; set; }

        #endregion
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
        public string BarCode { get; set; }
        public string ProductCode { get; set; }
        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public double NetWeight { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string? Result { get; set; }
    }
}