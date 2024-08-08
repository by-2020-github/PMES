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
using PMES_Respository.tbs;
using DateTime = System.DateTime;
using PMES_Respository.reportModel;
using PMES.UC.reports;
using System.Drawing;
using DevExpress.XtraReports.UI;
using HslCommunication.ModBus;
using FluentModbus;
using Newtonsoft.Json;
using DevExpress.XtraReports;


namespace PMES_Automatic_Net6.ViewModels
{
    public partial class TopPageViewModel : ObservableObject
    {
        private Plc Plc => HardwareManager.Instance.Plc;
        private ModbusTcpNet PlcXj => HardwareManager.Instance.PlcXj;
        public Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger();
        private Queue<XtraReport> _reports = new Queue<XtraReport>();

        [ObservableProperty] private string? _productCode = "";
        [ObservableProperty] private string? _preScanCode1 = "s1";
        [ObservableProperty] private string? _preScanCode2 = "s2";
        [ObservableProperty] private string? _boxCode1 = "s1";
        [ObservableProperty] private string? _boxCode2 = "s2";
        [ObservableProperty] private string? _weight1 = "2.22d";
        [ObservableProperty] private string? _weight2 = "3.55d";

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
        }

        private Task BoxArrived(List<DataItem> arg)
        {
            if (_reports.Count > 0)
            {
                var reportP = _reports.Dequeue();
                reportP.Print("161");
                reportP.Print("160");
            }

            return Task.FromResult(true);
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

        #region 称重处理

        /// <summary>
        ///     读取重量和条码
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async Task WeightAndCodeChanged(List<DataItem> obj)
        {
            Logger.Information($"WeightAndCodeChanged,{HardwareManager.PrintDataItems(obj)}");
            try
            {
                Logger?.Verbose($"WeightAndCodeChanged,当前标志位是：{obj[4].Value}");
                if (int.Parse(obj[4].Value?.ToString() ?? "0") == 2)
                {
                    ErrorStop(int.Parse(obj[0].Value.ToString()), $"WeightAndCodeChanged 当前标志位是：{obj[4]},其它信息却发生了更改。");
                    return;
                }

                //处理完毕把标志位置1
                //var items = new List<DataItem>();
                //obj.ForEach(s =>
                //{
                //    items.Add(new DataItem
                //    {
                //        DataType = s.DataType,
                //        VarType = s.VarType,
                //        DB = s.DB,
                //        StartByteAdr = s.StartByteAdr,
                //        BitAdr = s.BitAdr,
                //        Count = s.Count,
                //        Value = s.Value
                //    });
                //});
                //items[4].Value = byte.Parse("2");
                ////obj[5].Value = 0;
                //Logger?.Information($"WeightAndCodeChanged 把标志位置2：\n{HardwareManager.PrintDataItems(items)}");
                //await Plc.WriteAsync(items.TakeLast(3).ToArray());
                //await Task.Delay(50);
                //await Plc.ReadMultipleVarsAsync(items.ToList());
                //Logger?.Information($"WeightAndCodeChanged 把标志位置2读取结果：\n{HardwareManager.PrintDataItems(items)}");
                //错误码人工置0

                Weight1 = ((double.Parse(obj[2].Value.ToString())) / 100d).ToString("f2");
                Weight2 = ((double.Parse(obj[3].Value.ToString())) / 100d).ToString("f2");
                ProductCode = obj[3].Value.ToString();

                if (string.IsNullOrEmpty(ProductCode))
                {
                    ErrorStop((int.Parse(obj[0].Value.ToString())), "获取到错误的条码");
                }

                //var product = await WebService.Instance.Get<ProductInfo>($"{ApiUrls.QueryOrder}{ProductCode}");
                var product = new ProductInfo();
                if (product.Equals(null))
                {
                    //todo: 以后会校验错误 先放过
                    Logger.Error($"获取不到条码！new  一个临时默认的。");
                    //return;
                }

                await UpdateProductInfo(product ?? new ProductInfo());

                //任务队列添加对象
                try
                {
                    _productInfoQueue.Enqueue(new MyProductInfo
                    {
                        Weight1 = (double.Parse(obj[2].Value.ToString())),
                        Weight2 = (double.Parse(obj[3].Value.ToString())),
                        ProductBarCode = obj[1].Value.ToString(),
                        WorkShop = EnumWorkShop.Weight,
                        ProductInfo = product
                    });
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

        private async Task UpdateProductInfo(ProductInfo product, bool review = false, string order = "")
        {
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
                IsQualified = 0, //是否合格
                MachineCode = product.machine_number,
                MachineId = product.machine_id,
                MachineName = product.machine_name,
                NetWeight = double.Parse(Weight1 ?? "0"),
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

            //这里判断是否合格
            //var validateOrder = await ValidateOrder(double.Parse(Weight1 ?? "0"), ProductCode);
            //if (!validateOrder.Item1)
            //{
            //    tPreheaterCode.NoQualifiedReason = validateOrder.Item2;
            //    tPreheaterCode.IsQualified = 0;
            //}

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
                DateTime = DateTime.Now.ToString("yy-MM-dd"),
                WaterMark = tPreheaterCode.NoQualifiedReason
            };
            var xianPanReportModels = new List<XianPanReportModel>() { xianPanReportModel };
            //var reportP = new TemplateXianPan();
            //reportP.DataSource = xianPanReportModels;
            //reportP.Watermark.Text = xianPanReportModels.Last().WaterMark;
            //reportP.Watermark.ShowBehind = false;
            //reportP.ExportToImage($"D:\\xp_{ProductCode}.png");
            //Logger?.Information($"导出模板:D:\\xp_{ProductCode}.png");
            ////TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 
            //reportP.Print("152");
            //reportP.Print("161");
            //reportP.Print("160");
            //Logger?.Information($"调用默认打印机打印!");
            //SendXinJieCmd();
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{1:D4}-{Weight1}";

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
                    //BoxCode = boxCode
                    BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                },
               
            };
            var reportP = new TemplateBox();
            reportP.DataSource = boxReportModels;
            reportP.Watermark.Text = boxReportModels.Last().WaterMark;
            reportP.Watermark.ShowBehind = false;
            reportP.ExportToImage($"D:\\box.png");
            Logger?.Information($"导出模板:D:\\box.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 

            reportP.Print("Honeywell PX240 (300 dpi)");

            Logger?.Information($"调用默认打印机打印!");
            SendXinJieCmd();
            _reports.Enqueue(reportP);

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
            // 箱码 最后五位是重量 放到插入数据那里更新
            // 52.00 -> [0.05200] ->  05200  #####
            //包装条码；产品助记码 + 线盘分组代码 + 用户标准代码 + 包装组编号 + 年月 + 4位流水号 + 装箱净重，
            //eg1:{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}"
        }

        private async Task<Tuple<bool, string>> ValidateOrder(double weight, string order)
        {
            var validate =
                await WebService.Instance.GetJObjectValidate(
                    $"{ApiUrls.ValidateOrder}net_weight={weight:F2}&semi_finished={order}");

            return validate;
        }

        #endregion


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

        private async Task BoxBarCodeChanged(List<DataItem> obj)
        {
            if ((int)obj[3].Value == 2)
            {
                ErrorStop((int.Parse(obj[0].Value.ToString())), $"BoxBarCodeChanged 当前标志位是：{obj[4].Value},其它信息却发生了更改。");
                return;
            }

            ////处理完毕把标志位置1
            //var items = new List<DataItem>();
            //obj.ForEach(s =>
            //{
            //    items.Add(new DataItem
            //    {
            //        DataType = s.DataType,
            //        VarType = s.VarType,
            //        DB = s.DB,
            //        StartByteAdr = s.StartByteAdr,
            //        BitAdr = s.BitAdr,
            //        Count = s.Count,
            //        Value = s.Value
            //    });
            //});
            //items[3].Value = byte.Parse("2");
            //Logger?.Information($"BoxBarCodeChanged 把标志位置2：\n{HardwareManager.PrintDataItems(items)}");
            //await Plc.WriteAsync(items.TakeLast(3).ToArray());

            BoxCode1 = obj[1].Value.ToString();
            BoxCode2 = obj[2].Value.ToString();
            var info = _productInfoQueue.First(s => s.WorkShop == EnumWorkShop.ReelCodeCheck);
            info.BoxCode1 = BoxCode1;
            info.BoxCode2 = BoxCode2;
            info.WorkShop = EnumWorkShop.BoxCodeCheck;
            if (BoxCode1 != BoxCode2)
            {
                ErrorStop((int.Parse(obj[0].Value.ToString())), $"当前条码1：{BoxCode1} 不等于条码2：{BoxCode2}。");
                return;
            }
        }

        private async Task ReelCodeChanged(List<DataItem> obj)
        {
            if ((int.Parse(obj[3].Value.ToString())) == 2)
            {
                ErrorStop((int.Parse(obj[0].Value.ToString())), $"ReelCodeChanged 当前标志位是：{obj[4]},其它信息却发生了更改。");
                return;
            }

            //处理完毕把标志位置1
            //var items = new List<DataItem>();
            //obj.ForEach(s =>
            //{
            //    items.Add(new DataItem
            //    {
            //        DataType = s.DataType,
            //        VarType = s.VarType,
            //        DB = s.DB,
            //        StartByteAdr = s.StartByteAdr,
            //        BitAdr = s.BitAdr,
            //        Count = s.Count,
            //        Value = s.Value
            //    });
            //});
            //items[3].Value = byte.Parse("2");
            //Logger?.Information($"ReelCodeChanged 把标志位置2：\n{HardwareManager.PrintDataItems(items)}");
            //await Plc.WriteAsync(items.TakeLast(3).ToArray());

            PreScanCode1 = obj[1].Value.ToString();
            PreScanCode2 = obj[2].Value.ToString();
            var info = _productInfoQueue.First(s => s.WorkShop == EnumWorkShop.Weight);
            info.ReelCode1 = PreScanCode1;
            info.ReelCode2 = PreScanCode2;
            info.WorkShop = EnumWorkShop.ReelCodeCheck;
            if (PreScanCode1 != PreScanCode2)
            {
                ErrorStop((int.Parse(obj[0].Value.ToString())), $"当前条码1：{PreScanCode1} 不等于条码2：{PreScanCode2}。");
                return;
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

        [RelayCommand]
        private void PrintReelCode()
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
                    BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                    DateTime = "22222222222",
                    GrossWeight = "22222222222",
                    WaterMark = "22222222222"
                }
            };
            var reportP = new TemplateBox();
            reportP.DataSource = boxReportModels;
            reportP.Watermark.Text = boxReportModels.Last().WaterMark;
            reportP.Watermark.ShowBehind = false;
            _reports.Enqueue(reportP);

            reportP.ExportToImage($"D:\\box.png");
            Logger?.Information($"导出模板:D:\\box.png");
            //TODO:先打印一个标签，后续根据客户需求 可以对应多个标签 2024年8月3日 

            reportP.Print("Honeywell PX240 (300 dpi)");

            Logger?.Information($"调用默认打印机打印!");
            SendXinJieCmd();
        }

        [RelayCommand]
        private void PrintBoxCode()
        {
            if (_reports.Count == 0)
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
                        BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                        DateTime = "22222222222",
                        GrossWeight = "22222222222",
                        WaterMark = "22222222222"
                    }
                };
                var reportP = new TemplateBox();
                reportP.DataSource = boxReportModels;
                reportP.Watermark.Text = boxReportModels.Last().WaterMark;
                reportP.Watermark.ShowBehind = false;
                _reports.Enqueue(reportP);
            }

            var reportP1 = _reports.Dequeue();
            reportP1.Print("161");
            reportP1.Print("160");
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
    }

    public enum EnumWorkShop
    {
        None = 0,
        UnStacking,
        Weight,
        ReelCodeCheck,
        BoxCodeCheck,
        Stacking
    }

    public class TopGridModel
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