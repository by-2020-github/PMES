using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using FreeSql.DataAnnotations;
using K4os.Hash.xxHash;
using Newtonsoft.Json;
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.Model;
using PMES_Respository.DataStruct;
using PMES_Respository;
using PMES_Respository.reportModel;
using PMES_Respository.tbs_sqlserver;
using Serilog;
using System.Reflection.Emit;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using DevExpress.DirectX.Common.DirectWrite;
using Org.BouncyCastle.Ocsp;
using Xceed.Wpf.AvalonDock.Layout;
using Google.Protobuf.WellKnownTypes;
using PMES.Manual.Net6.Views;

namespace PMES.Manual.Net6.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private ILogger Logger => SerilogManager.GetOrCreateLogger("running");
        private IFreeSql _fsql;

        #region 全局计数器

        private int _countReelInBox = 0;
        private int _countReelTotal = 0;
        private int _countBoxTotal = 0;

        #endregion

        [ObservableProperty] private bool _isAuto = false;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private bool _isManual = true;

        [ObservableProperty] private ObservableCollection<string> _logList = new ObservableCollection<string>();

        /// <summary>
        ///  产品码
        /// </summary>
        [ObservableProperty] private string? _currentProductQrCode = "";

        /// <summary>
        /// 母托盘码
        /// </summary>
        [ObservableProperty] private string? _currentMotherTrayQrCode = "";

        /// <summary>
        /// 扫码枪的值 可以是【母托盘，子托盘，纸箱，产品码】
        /// </summary>
        [ObservableProperty] private string? _currentScanValue = "";

        [ObservableProperty] private ViewProductModel _viewProductModel = new ViewProductModel(new ProductInfo(), "");

        #region 中间称重 条码信息

        private WeighingMachine? _weighingMachine;
        [ObservableProperty] private double? _paperWeight = 0d;
        [ObservableProperty] private string? _trayQrCode;
        [ObservableProperty] private string? _grossWeight;

        partial void OnGrossWeightChanged(string? value)
        {
            if (double.TryParse(GrossWeight, out var w))
            {
                ViewProductModel.GrossWeight = w;
            }
        }

        #endregion

        #region 右侧堆垛信息

        /// <summary>
        ///     已码进度
        /// </summary>
        [ObservableProperty] private string? _currentStackInfo;

        /// <summary>
        ///     未完成进度
        /// </summary>
        [ObservableProperty] private string? _currentUnStackInfo;

        [ObservableProperty] private string? _currentStackMode;
        [ObservableProperty] private int _currentStackLayer = 0;
        [ObservableProperty] private int _currentStackNumPerLayer = 0;
        [ObservableProperty] private int _currentReelNumPerBox = 0;

        public double StatisticsBoxGrossWeight => BoxListTemp.Sum(s => s.GrossWeight);
        public double StatisticsBoxNetWeight => BoxListTemp.Sum(s => s.NetWeight);

        #endregion

        #region 中间列表展示

        [ObservableProperty]
        private ObservableCollection<MyReelInfo> _reelListTemp = new ObservableCollection<MyReelInfo>();

        [ObservableProperty] private MyReelInfo? _selectedMyReelInfo;

        [ObservableProperty]
        private ObservableCollection<MyBoxInfo> _boxListTemp = new ObservableCollection<MyBoxInfo>();

        [ObservableProperty] private MyBoxInfo? _selectedMyBoxInfo;

        [ObservableProperty]
        private ObservableCollection<MyBoxInfo> _boxListHistory = new ObservableCollection<MyBoxInfo>();

        #endregion

        #region 临时变量

        /// <summary>
        ///     存储当前 tray-->box-->reel 的信息
        /// </summary>
        private readonly List<MyBoxInfo> _currentBoxOnTray = new List<MyBoxInfo>();

        /// <summary>
        ///     箱子计数，满一个tray清零
        /// </summary>
        private int _currentBoxNum = 0;

        /// <summary>
        ///     线盘计数，满一个Box清零
        /// </summary>
        private int _currentReelNum = 0;

        #endregion

        public MainViewModel(IFreeSql freeSql)
        {
            RefreshPorts();
            RefreshPrinters();
            LoadSettings();
            _fsql = freeSql;
            CurrentStackInfo = "已码x层 ，共xx个";
            CurrentUnStackInfo = "剩余个数：";
            CurrentStackMode = "竖方式";
            CurrentStackNumPerLayer = 16;
            CurrentStackLayer = 1;
            //for (int i = 0; i < 5; i++)
            //{
            //    ReelListTemp.Add(new MyReelInfo());
            //    BoxListTemp.Add(new MyBoxInfo());
            //}

            //Task.Run(() =>
            //{
            //    var time = 1;
            //    while (true)
            //    {
            //        time++;
            //        Debug.WriteLine(CurrentScanValue);
            //        Thread.Sleep(1000);
            //        ViewProductModel = new ViewProductModel(new ProductInfo(), $"{time}")
            //            { ProductOrder = new Random().Next(100).ToString() };
            //        Application.Current.Dispatcher.Invoke((Action)(() => { LogList.Add($"{time}"); }));
            //    }
            //});
        }


        private string lastQr = "";

        partial void OnCurrentScanValueChanged(string? value)
        {
            if (value != null && value.EndsWith("\r\n"))
            {
                if (lastQr.Equals(_currentScanValue))
                {
                    return;
                }

                lastQr = _currentScanValue;
                CurrentScanValueChanged(_currentScanValue);
            }
        }

        partial void OnSelectedMyReelInfoChanged(MyReelInfo? value)
        {
            if (value == null)
                return;
            Debug.WriteLine(value.Number);
        }

        /// <summary>
        ///     扫码枪 扫到新的条码
        /// </summary>
        /// <param name="value"></param>
        private async Task CurrentScanValueChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            //if (value.EndsWith("\n"))
            //    return;

            //999 开头是包装纸皮重
            if (value.StartsWith("999"))
            {
                //Todo:更新包装纸皮重
                PaperWeight = 0.01d;
                return;
            }

            //扫到的是tray QrCode
            if (value.StartsWith("TP"))
            {
                TrayQrCode = value;
                return;
            }

            //条码长度验证
            //if (value.Length < "G240804830995121".Length)
            if (value.Length < 13)
                return;
            //扫到的是真实的条码
            CurrentProductQrCode = value;


            #region 1 ERP订单信息

            _viewProductModel = new ViewProductModel(new ProductInfo(), "");
            var url = ApiUrls.QueryOrder + CurrentProductQrCode + "&format=json";
            var product = await WebService.Instance.Get<ProductInfo>(url);
            if (product == null)
            {
                ShowError($"访问ERP失败,url：{url}");
                return;
            }

            //如果校验通过就删除报警
            LogList.Clear();
            product ??= new ProductInfo();
            _viewProductModel = ViewProductModel.GetViewProductModel(product, CurrentProductQrCode);

            #endregion

            if (LoadUiModelFail(product))
                return;
            _manualCurrentProductInfo = product;
            if (IsAuto)
            {
                if (_weighingMachine is null)
                {
                    ShowError("称没有初始化，选择人工模式后会初始化称！");
                    return;
                }

                if (!_weighingMachine.IsOpen)
                {
                    ShowError("称没有打开!！");
                    //return;
                }

                AutoCurrentProductQrCodeChanged(value, product);
                CurrentScanValue = "";
            }
        }

        /// <summary>
        ///     扫到母托盘码（预留功能）
        /// </summary>
        /// <param name="value"></param>
        partial void OnTrayQrCodeChanged(string? value)
        {
        }

        /// <summary>
        ///     1 [自动模式]扫到的是产品订单
        /// </summary>
        /// <param name="value"></param>
        private void AutoCurrentProductQrCodeChanged(string? value, ProductInfo product)
        {
            if (string.IsNullOrEmpty(CurrentProductQrCode))
                return; //这里不会执行到，只有扫描到才会触发changed 前面已经过滤掉了


            #region 2 称重--更新称重信息 并创建线盘信息

            var weight = _weighingMachine!.ReadWeight(20_000);
            if (weight < 0)
            {
                ShowError($"称重失败!");
                //return;
            }

            _viewProductModel.GrossWeight = weight;

            if (!double.TryParse(product!.package_info.tare_weight, out double packageWeight))
            {
                Logger?.Error("获取皮重失败，默认是0.");
            }

            if (!double.TryParse(product!.xpzl_weight, out double reelWeight))
            {
                Logger?.Error("获取reelWeight失败，默认是0.");
            }

            Logger?.Information(
                $"[KEY STEP]ProductCode:{product},tare_weight:{packageWeight},xpzl_weight:{reelWeight}");
            var netWeight = weight - reelWeight - packageWeight;

            _viewProductModel.ReelWeight = reelWeight;
            _viewProductModel.PackagePaperWeight = packageWeight;

            var tReelCode = GetTReelCode(product, CurrentProductQrCode, netWeight, weight);

            #endregion

            #region 5 获取标签

            var template = _fsql.Select<T_label_template>()
                .Where(s => s.PackageCode == product.package_info.code && s.LabelType == 101)
                .First();
            if (template == null)
            {
                ShowError("找不到对应的标签模板！采用默认的标签模板！");
                template = _fsql.Select<T_label_template>()
                    .Where(s => s.PackageCode == "default" && s.LabelType == 101)
                    .First();
                //return;
            }

            _viewProductModel.LabelTemplateName = $"{template.PackageCode} - {template.PackageName}";

            #endregion

            // 更新队列信息并打印标签
            UpdateAndPrint(product, weight, template, tReelCode);
        }

        private bool LoadUiModelFail(ProductInfo product)
        {
            #region 1 更新码垛信息

            if (product.package_info.stacking_per_layer is 0 or null)
            {
                ShowError($"包装信息错误：stacking_per_layer = 0 !");
                return true;
            }

            if (product.package_info.stacking_layers is 0 or null)
            {
                ShowError($"包装信息错误：stacking_layers = 0 !");
                return true;
            }

            if (product.package_info.packing_quantity is 0 or null)
            {
                ShowError($"包装信息错误：packing_quantity = 0 !");
                return true;
            }

            CurrentStackLayer = (int)product.package_info.stacking_layers;
            CurrentStackNumPerLayer = (int)product.package_info.stacking_per_layer;
            CurrentReelNumPerBox = (int)product.package_info.packing_quantity;

            #endregion

            #region 2 获取界面model

            if (double.TryParse(product.xpzl_weight, out var w))
            {
                _viewProductModel.ReelWeight = w;
            }
            else
            {
                ShowError($"获取线盘重量信息失败:{product.xpzl_weight}");
            }


            var ap = DateTime.Now.Hour < 12 ? "A" : "P";
            _viewProductModel.ProductionBatchNumber = @$"{product.product_order_no}-{DateTime.Now: dd}{ap}";
            _viewProductModel.PackingGroupCode = GlobalVar.CurrentUserInfo.code;
            _viewProductModel.PackingGroupName = GlobalVar.CurrentUserInfo.packageGroupName;
            _viewProductModel.PackagePaperWeight = PaperWeight ?? 0d;
            _viewProductModel.MotherTrayCode = CurrentMotherTrayQrCode;

            OnPropertyChanged(nameof(ViewProductModel));

            #endregion

            return false;
        }

        /// <summary>
        ///     1' [手动模式]扫到的是产品订单
        /// </summary>
        /// <param name="value"></param>
        private void ManualCurrentProductQrCodeChanged(string? value, ProductInfo? product)
        {
            if (string.IsNullOrEmpty(CurrentProductQrCode))
                return; //这里不会执行到，只有扫描到才会触发changed 前面已经过滤掉了
            if (product == null)
            {
                ShowError($"当前条码:{CurrentProductQrCode}访问ERP失败，无法加载数据！");
                return;
            }

            #region 1 ERP订单信息

            //var url = ApiUrls.QueryOrder + CurrentProductQrCode + "&format=json";
            //var product = WebService.Instance.Get1<ProductInfo>(url);
            //if (product == null)
            //{
            //    ShowError($"访问ERP失败,url：{url}");
            //    return;
            //}

            //var viewProductModel = ViewProductModel.GetViewProductModel(product, CurrentProductQrCode);

            #endregion

            #region 2 称重--更新称重信息 并创建线盘信息

            var weight = ViewProductModel.GrossWeight;
            //if (weight < 0)
            //{
            //    ShowError($"称重失败!");
            //    //return;
            //}

            if (!double.TryParse(product!.package_info.tare_weight, out double packageWeight))
            {
                Logger?.Error("获取皮重失败，默认是0.");
            }

            if (!double.TryParse(product!.xpzl_weight, out double reelWeight))
            {
                Logger?.Error("获取reelWeight失败，默认是0.");
            }

            Logger?.Information(
                $"[KEY STEP]ProductCode:{product},tare_weight:{packageWeight},xpzl_weight:{reelWeight}");
            var netWeight = weight - reelWeight - packageWeight;

            _viewProductModel.ReelWeight = reelWeight;
            _viewProductModel.PackagePaperWeight = packageWeight;

            var tReelCode = GetTReelCode(product, CurrentProductQrCode, netWeight, weight);

            #endregion

            #region 3 更新码垛信息

            //if (product.package_info.stacking_per_layer is 0 or null)
            //{
            //    ShowError($"包装信息错误：stacking_per_layer = 0 !");
            //    return;
            //}

            //if (product.package_info.stacking_layers is 0 or null)
            //{
            //    ShowError($"包装信息错误：stacking_layers = 0 !");
            //    return;
            //}

            //if (product.package_info.packing_quantity is 0 or null)
            //{
            //    ShowError($"包装信息错误：packing_quantity = 0 !");
            //    return;
            //}

            //CurrentStackLayer = (int)product.package_info.stacking_layers;
            //CurrentStackNumPerLayer = (int)product.package_info.stacking_per_layer;
            //CurrentReelNumPerBox = (int)product.package_info.packing_quantity;

            #endregion

            #region 4 获取界面model

            //_viewProductModel.ProductionBatchNumber = @$"{CurrentProductQrCode}-{DateTime.Now: MMdd}A";
            //_viewProductModel.PackingGroupCode = GlobalVar.CurrentUserInfo.code;
            //_viewProductModel.PackingGroupName = GlobalVar.CurrentUserInfo.packageGroupName;
            //_viewProductModel.GrossWeight = weight;
            //_viewProductModel.PackagePaperWeight = PaperWeight ?? 0d;
            //_viewProductModel.MotherTrayCode = CurrentMotherTrayQrCode;

            #endregion

            #region 5 获取标签

            var template = _fsql.Select<T_label_template>()
                .Where(s => s.PackageCode == product.package_info.code && s.LabelType == 101)
                .First();
            if (template == null)
            {
                ShowError("找不到对应的标签模板！采用默认的标签模板！");
                template = _fsql.Select<T_label_template>()
                    .Where(s => s.PackageCode == "default" && s.LabelType == 101)
                    .First();
                //return;
            }

            _viewProductModel.LabelTemplateName = $"{template.PackageCode} - {template.PackageName}";
            _viewProductModel.ImageBox = null;
            _viewProductModel.BoxQrCode = "";

            #endregion

            // 更新队列信息并打印标签
            UpdateAndPrint(product, weight, template, tReelCode);
        }

        /// <summary>
        ///     1.1 更新界面并且打印标签
        /// </summary>
        /// <param name="viewProductModel"></param>
        /// <param name="product"></param>
        /// <param name="weight"></param>
        /// <param name="template"></param>
        /// <param name="preheaterCode"></param>
        private void UpdateAndPrint(ProductInfo product, double weight,
            T_label_template template, T_preheater_code preheaterCode)
        {
            _countReelInBox++;
            _countReelTotal++;
            var myReelInfo = new MyReelInfo
            {
                ReelProductCode = CurrentProductQrCode!,
                GrossWeight = weight,
                NetWeight = ViewProductModel.NetWeight,
                CountOneBox = CurrentReelNumPerBox,
                PreheaterCode = preheaterCode,
                ProductInfo = product
            };
            //2024年10月20日 07:40:04 新增已经打印过的条件 也new一个新的箱子
            if (BoxListTemp.Count == 0 || BoxListTemp.LastOrDefault()?.HasPrint == true)
            {
                var myBoxInfo = new MyBoxInfo();
                myBoxInfo.ReelList.Add(myReelInfo);
                BoxListTemp.Add(myBoxInfo);
                BoxListHistory.Add(myBoxInfo);
            }
            else
            {
                var myBoxInfo = BoxListTemp.Last();
                if (myBoxInfo.ReelList.Count < (int)product.package_info.packing_quantity!)
                {
                    myBoxInfo.ReelList.Add(myReelInfo);
                }
                else
                {
                    var myBoxInfo2 = new MyBoxInfo();
                    myBoxInfo2.ReelList.Add(myReelInfo);
                    BoxListTemp.Add(myBoxInfo2);
                    BoxListHistory.Add(myBoxInfo);
                }
            }

            BoxListTemp.Last().LabelTemplate = template;
            var latest = BoxListTemp.Last();
            //更新界面
            SelectedMyBoxInfo = latest;
            _reelListTemp.Clear();
            SelectedMyBoxInfo?.ReelList.ForEach(s => _reelListTemp.Add(s));
            OnPropertyChanged(nameof(ReelListTemp));
            //打印标签
            if (latest.IsFull() && !latest.HasPrint)
            {
                BoxHasFull(ViewProductModel, latest);
            }
        }

        /// <summary>
        ///     1.2 如果盒子装满|或者手动点了保存 触发保存，计数器清零
        /// </summary>
        /// <param name="viewProductModel"></param>
        /// <param name="latest"></param>
        private void BoxHasFull(ViewProductModel viewProductModel,
            MyBoxInfo latest)
        {
            _countBoxTotal = GetTodayCount();

            var product = latest.ReelList.FirstOrDefault()?.ProductInfo;
            if (product == null)
            {
                ShowError("box 里不存在线盘，无法打印标签！这里理论不会触发！");
            }

            latest.ViewProductModel = viewProductModel;
            var reelList = Enumerable.Range(1, 4).Select(s => new XianPanReportModel()).ToList();
            //盘码数据
            for (var i = 0; i < latest.ReelList.Count; i++)
            {
                var tReelCode = latest.ReelList[i].PreheaterCode;
                var w = (int)(100 * tReelCode.NetWeight);
                var reelCode =
                    @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:yyMM}{(i + 1):D4}-{w:D5}";
                reelList[i] = new XianPanReportModel
                {
                    Title = "",
                    MaterialNo = product.material_number,
                    Model = product.material_name,
                    Specifications = product.material_spec,
                    GrossWeight = tReelCode.GrossWeight,
                    NetWeight = tReelCode.NetWeight,
                    BatchNum = tReelCode.BatchNO,
                    No = tReelCode.PSN,
                    BoxCode = reelCode,
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    WaterMark = tReelCode.NoQualifiedReason,
                    Reji = null
                };
            }

            //箱码数据
            var wei = (int)(100 * latest.ReelList.Sum(s => s.NetWeight));
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:yyMM}{_countBoxTotal:D4}-{wei:D5}";
            latest.BoxQrCode = boxCode;
            viewProductModel.BoxQrCode = boxCode;
            var boxReportModel = new List<BoxReportModel>
            {
                new()
                {
                    MaterialNo = latest.ReelList.First().PreheaterCode.CustomerMaterialCode,
                    Model = product.material_name,
                    GBNo = product.material_ns_model,
                    Specifications = product.material_spec,
                    NetWeight = reelList.Sum(s => s.NetWeight),
                    BatchNum = product.product_order_no,
                    No = latest.ReelList.First().PreheaterCode.PSN, //包装线真实的编码
                    Standard = latest.ReelList.First().PreheaterCode.ProductStandardName,
                    ProductNo = latest.ReelList.First().PreheaterCode.ProductCode,
                    DateTime = DateTime.Now.ToString("yy-MM-dd"),
                    GrossWeight = reelList.Sum(s => s.GrossWeight),
                    BoxCode = boxCode,
                    WaterMark = latest.ReelList.First().PreheaterCode.NoQualifiedReason,
                    //BoxCode = (new Random().Next(100_000_000, 200_000_000)).ToString(),
                    ReJi = product.material_thermal_grade,
                    JsbzShortName = product.jsbz_short_name,
                    ReelList = reelList
                }
            };

            //TODO:加载正式的模板 --- 人工线采购的纸尺寸固定（1 箱码，4 合格证） 2024年10月17日 17:01:55
            var report = new XtraReport();
            var template = latest.LabelTemplate;
            report.LoadLayout(template.TemplateFileName);
            report.DataSource = boxReportModel;
            latest.Report = report;

            var type = _fsql.Select<T_label_type>().Where(s => s.LabelType == template.LabelType).First();
            if (type == null)
            {
                MessageBox.Show($"找不到LabelType:{template.LabelType}对应的打印机，请前往配置打印机页面配置！");
            }
            else
            {
                try
                {
                    Directory.CreateDirectory("d://print_view");
                    latest.Report?.ExportToImage($"d://print_view//{boxCode}.png");
                    latest.Report?.Print(PrinterName);
                    latest.HasPrint = true; //更新为标签已打印状态
                    var bitmap = new BitmapImage(new Uri($"d://print_view//{boxCode}.png"));
                    _viewProductModel.ImageBox = bitmap;
                    OnPropertyChanged(nameof(ViewProductModel));
                }
                catch (Exception e)
                {
                    Logger?.Error($"打印标签失败:{e.Message}");
                }
            }

            try
            {
                InsertReeTb(latest, template);
            }
            catch (Exception e)
            {
                Logger?.Error($"插入数据库异常:{e.Message}");
            }

            //清空计数
            _countReelInBox = 0;
            MyReelInfo.Reset();
            //MyBoxInfo.Reset();
        }

        private void InsertReeTb(MyBoxInfo boxInfo, T_label_template template)
        {
            try
            {
                Logger?.Verbose(LogInfo.Info($"准备插入数据库,数据：\n{JsonConvert.SerializeObject(boxInfo)}"));
                var myReelInfo = boxInfo.ReelList.First();
                var productInfo = myReelInfo.ProductInfo;
                var box = new T_box
                {
                    CreateTime = DateTime.Now,
                    LabelId = (int?)template.Id,
                    LabelName = template.TemplateFileName,
                    LabelTemplateId = (int?)template.Id,
                    PackagingCode = GlobalVar.CurrentUserInfo.packageGroupCode,
                    PackagingSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{_countReelTotal:D4}",
                    PackagingWorker = GlobalVar.CurrentUserInfo.username,
                    PackingBarCode = boxInfo.BoxQrCode,
                    PackingQty = productInfo.package_info.packing_quantity.ToString(),
                    PackingWeight = boxInfo.ReelList.Sum(s => s.NetWeight),
                    PackingGrossWeight = boxInfo.ReelList.Sum(s => s.GrossWeight),
                    Status = 1,
                    TrayBarcode = string.IsNullOrEmpty(CurrentMotherTrayQrCode)
                        ? GetTimeStamp()
                        : CurrentMotherTrayQrCode,
                    UpdateTime = DateTime.Now
                };
                var id = _fsql.Insert(box).ExecuteIdentity();
                box.Id = (uint)id;
                ShowInfo($"插入成功BoxId:{id}");
                var tReels = boxInfo.ReelList.Select(s => s.PreheaterCode);
                foreach (var preheaterCode in tReels)
                {
                    preheaterCode.BoxId = (int)id;
                    var rId = _fsql.Insert(preheaterCode).ExecuteIdentity();
                    ShowInfo($"插入成功ReelId:{rId}");
                    var order = new T_order_package
                    {
                        CreateTime = DateTime.Now,
                        DeliverySubTrayName = productInfo.package_info.delivery_sub_tray_name,
                        FullCoilWeight = productInfo.package_info.cu_full_coil_weight,
                        MaxWeight = double.Parse(productInfo.package_info.cu_max_weight ?? "0"),
                        MinWeight = double.Parse(productInfo.package_info.cu_min_weight ?? "0"),
                        PackagingReqCode = productInfo.package_info.code,
                        PackagingReqName = productInfo.package_info.name,
                        PackingQuantity = productInfo.package_info.packing_quantity,
                        Paperboard_name = productInfo.package_info.delivery_sub_tray_name,
                        Paperboard_number = 1,
                        Paperboard_spec = "1",
                        PreheaterCodeId = (int)rId,
                        PreheaterInsidePackageName = productInfo.package_info.wire_reel_inside_package_name,
                        PreheaterOutsidePackageName = productInfo.package_info.wire_reel_external_package_name,
                        StackingLayers = productInfo.package_info.stacking_layers,
                        StackingPerLayer = productInfo.package_info.stacking_per_layer,
                        Super_wide_sub_tray = false,
                        TareWeight = double.Parse(productInfo.package_info.tare_weight ?? "0"),
                        UpdateTime = DateTime.Now
                    };
                    var orderId = _fsql.Insert(order).ExecuteIdentity();
                    ShowInfo($"插入成功orderId:{orderId}");
                    //2024年10月17日 06:25:43 顺便插入老数据库
                    UpdateOldDbSqlServer(preheaterCode, box,
                        new T_order_package() { TareWeight = ViewProductModel.ReelWeight });
                }
            }
            catch (Exception e)
            {
                ShowError($"插入箱码|盘码到数据库失败！\n{e}");
            }
        }

        /// <summary>
        ///     更新到老数据库
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateOldDbSqlServer(T_preheater_code preheaterCode, T_box boxCode, T_order_package packageOrder)
        {
            try
            {
                var view = _fsql.Select<U_VW_DBCP>().WithSql("SELECT * FROM U_VW_DBCP").ToList()
                    .First(s => s.FItemID.ToString().Equals(preheaterCode.ProductId.ToString()));
                if (view == null)
                {
                    ShowError("视图查询为空！");
                    return;
                }

                var old = new OldTest
                {
                    FItemID = preheaterCode.ProductId,
                    FNumber = preheaterCode.ProductCode,
                    FCZM = preheaterCode.ProductMnemonicCode,
                    FGJTYXH = preheaterCode.ProductGBName,
                    FType = preheaterCode.ProductName,
                    FTypeID = view.产品型号ID,
                    FTypeNO = view.产品型号代号,
                    FQMDJID = view.产成品漆膜等级ID,
                    FQMDJNO = view.产成品漆膜等级代号,
                    FQMDJ = view.产成品漆膜等级,
                    FCPGGID = view.产品规格ID,
                    FCPGGNO = view.产品规格代号,
                    FCZID = view.产品材质ID,
                    FCZNO = view.产品材质代号,
                    FCZName = view.产品材质,
                    FXTID = view.产成品形态ID,
                    FXTNO = view.产成品形态代号,
                    FXTName = view.产成品形态,
                    FCPGG = preheaterCode.Material_spec,
                    FXPItemID = preheaterCode.PreheaterId,
                    FXPNumber = preheaterCode.PreheaterCode,
                    FXPName = preheaterCode.PreheaterName,
                    FXPGG = preheaterCode.PreheaterSpec,
                    FXPQty = (decimal)preheaterCode.NetWeight,
                    FBZZPZQty = (decimal)packageOrder.TareWeight, //修改为包装代码里的【皮重】。
                    FJTH = preheaterCode.MachineCode,
                    FBH = boxCode.PackagingSN, //以天为单位进行联系，保留4位。
                    FBHMX = preheaterCode.PSN,
                    FPCH = preheaterCode.BatchNO, //修改为：FIcmibILLno-天A(上午A，下午M），day保留2位..

                    FDate = boxCode.CreateTime, //只写日期，
                    FSXH = 0, //一天的连续号..
                    FHGZQty = packageOrder.TareWeight,
                    FJYR = preheaterCode.OperatorName,
                    FZXBZID = 0,
                    FZXBZ = preheaterCode.ProductStandardName,
                    FMZQty = (decimal)boxCode.PackingGrossWeight,
                    FPZQty = (decimal)preheaterCode.PreheaterWeight,
                    FJZQty = (decimal)preheaterCode.NetWeight,
                    FStrip = boxCode.PackingBarCode,
                    FComputerName = "2#自动包装线", //boxCode.PackagingCode,
                    FZQty = (decimal)boxCode.PackingWeight,
                    FBQID = boxCode.LabelTemplateId,
                    FBQJS = int.Parse(boxCode.PackingQty),
                    FTypeTemp = preheaterCode.ProductName,
                    FICMOID = preheaterCode.ICMOBillNO,
                    FICMOBillNO = preheaterCode.ICMOBillNO,
                    FStrip2 = preheaterCode.ProductionBarcode,
                    FDate2 = preheaterCode.CreateTime,
                    FJSBZID = preheaterCode.UserStandardId,
                    FJSBZNumber = preheaterCode.UserStandardCode,
                    FSCorgno = preheaterCode.ProductionOrgNO, //
                    FStockID = preheaterCode.StockId,
                    FCustomer = preheaterCode.CustomerId,
                    FLinkStacklabel = $"{GetTimeStamp}-{boxCode.TrayBarcode}", //时间戳+实托盘条码
                    FSPTime = DateTime.Now,
                    BoxID = (int)boxCode.Id
                };
                var identity = _fsql.Insert(old).ExecuteIdentity();
                ShowInfo($"插入成功OldId:{identity}");
            }
            catch (Exception e)
            {
                ShowError($"插入老数据库失败！\n{e.Message}");
            }
        }

        #region Command

        [RelayCommand]
        private void OpenCommunicationSettings()
        {
        }

        [RelayCommand]
        private void OpenPrinterSettings()
        {
        }

        [RelayCommand]
        private void OpenLabelTemplateSettings()
        {
        }

        [RelayCommand]
        private void ManualMergerTray()
        {
        }

        [RelayCommand]
        private void ClearAll()
        {
        }

        private ProductInfo _manualCurrentProductInfo;

        /// <summary>
        ///     触发提前保存
        /// </summary>
        [RelayCommand(CanExecute = nameof(SaveCanExec))]
        private void Save()
        {
            try
            {
                ManualCurrentProductQrCodeChanged(CurrentProductQrCode, _manualCurrentProductInfo);
            }
            catch (Exception e)
            {
                ShowError($"手动保存异常：\n{e.Message}");
            }
            /*var latest = BoxListTemp.Last();
            var reel = latest.ReelList.First();
            if (!latest.IsFull())
            {
                Logger?.Information($"未满手动触发保存:{CurrentProductQrCode}");
            }

            BoxHasFull(ViewProductModel, latest);*/
        }

        private bool SaveCanExec()
        {
            return IsManual;
        }

        /// <summary>
        ///     切换到自动模式会初始化称
        /// </summary>
        /// <param name="value"></param>
        partial void OnIsAutoChanged(bool value)
        {
            if (!value)
            {
                return;
            }

            _weighingMachine ??= new WeighingMachine(Logger);
            if (!_weighingMachine.IsOpen)
            {
                var success = _weighingMachine.Open(PMES.Default.Port,
                    PMES.Default.BaudRate,
                    (System.IO.Ports.Parity)PMES.Default.Parity,
                    PMES.Default.DataBits,
                    (StopBits)PMES.Default.StopBits
                );
                if (!success)
                {
                    ShowError("称初始化失败，请检查配置信息！");
                }
            }
        }

        [RelayCommand]
        private void PrintLabel()
        {
            if (SelectedMyBoxInfo == null)
            {
                ShowError($"选中为空，无法打印！");
                return;
            }

            //人工线只有一个打印机 设置为默认即可
            SelectedMyBoxInfo.Report?.Print(PrinterName);
            ShowInfo($"手动打印标签！BoxCode:{SelectedMyBoxInfo.BoxQrCode}");
        }


        [RelayCommand]
        private void DeleteOne()
        {
            try
            {
                var tmp = SelectedMyBoxInfo;
                BoxListTemp.Remove(tmp);
                BoxListHistory.Remove(tmp);
            }
            catch (Exception e)
            {
                ShowError($"删除失败:{e.Message}");
            }
        }

        [RelayCommand]
        private void HistorySearch()
        {
        }

        #endregion

        #region UI交互

        partial void OnSelectedMyBoxInfoChanged(global::PMES.Manual.Net6.ViewModels.MyBoxInfo value)
        {
            ReelListTemp.Clear();
            value.ReelList.ForEach(ReelListTemp.Add);
            if (ReelListTemp.Count > 0)
            {
                SelectedMyReelInfo = ReelListTemp.First();
            }

            OnPropertyChanged(nameof(SelectedMyReelInfo));

            //ViewProductModel = value.ViewProductModel;
            //OnPropertyChanged(nameof(ViewProductModel));
        }

        #endregion
    }
}