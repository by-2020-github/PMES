using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.Model;
using PMES_Respository.reportModel;
using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES.Manual.Net6.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private ILogger Logger => SerilogManager.GetOrCreateLogger("running");
        public IFreeSql FreeSql => FreeSqlManager.FSqlServer;

        #region 全局计数器

        private int _countReelInBox = 0;
        private int _countReelTotal = 0;

        #endregion

        [ObservableProperty] private bool _isAuto = true;
        [ObservableProperty] private bool _isManual = false;

        [ObservableProperty] private ObservableCollection<string> _logList = new ObservableCollection<string>();

        /// <summary>
        ///  产品码
        /// </summary>
        [ObservableProperty] private string? _currentProductQrCode = "GB";

        /// <summary>
        /// 母托盘码
        /// </summary>
        [ObservableProperty] private string? _currentMotherTrayQrCode = "TP";

        /// <summary>
        /// 扫码枪的值 可以是【母托盘，子托盘，纸箱，产品码】
        /// </summary>
        [ObservableProperty] private string? _currentScanValue = "TP";

        [ObservableProperty] private ViewProductModel _viewProductModel;

        #region 中间称重 条码信息

        private WeighingMachine _weighingMachine;
        [ObservableProperty] private double? _paperWeight = 0d;
        [ObservableProperty] private string? _trayQrCode;

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

        [ObservableProperty] private MyReelInfo _selectedMyReelInfo;

        [ObservableProperty]
        private ObservableCollection<MyBoxInfo> _boxListTemp = new ObservableCollection<MyBoxInfo>();

        [ObservableProperty] private MyBoxInfo _selectedMyBoxInfo;

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

        public MainViewModel()
        {
            _weighingMachine = new WeighingMachine(Logger);
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

            CurrentStackInfo = "已码x层 ，共xx个";
            CurrentUnStackInfo = "剩余个数：";
            CurrentStackMode = "竖方式";
            CurrentStackNumPerLayer = 16;
            CurrentStackLayer = 1;
            for (int i = 0; i < 5; i++)
            {
                ReelListTemp.Add(new MyReelInfo());
                BoxListTemp.Add(new MyBoxInfo());
            }

            Task.Run(() =>
            {
                while (true)
                {
                    Debug.WriteLine(CurrentScanValue);
                    Thread.Sleep(1000);
                    //ViewProductModel = new ViewProductModel(new ProductInfo(), "123") { ProductOrder = new Random().Next(100).ToString() };
                }
            });
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
        partial void OnCurrentScanValueChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            if (value.EndsWith("\n"))
                return;
            if (value.Length < 5)
                return;
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

            //扫到的是真实的条码
            CurrentProductQrCode = value;
        }

        /// <summary>
        ///     扫到母托盘码（预留功能）
        /// </summary>
        /// <param name="value"></param>
        partial void OnTrayQrCodeChanged(string? value)
        {
        }

        /// <summary>
        ///     扫到的是产品订单
        /// </summary>
        /// <param name="value"></param>
        partial void OnCurrentProductQrCodeChanged(string? value)
        {
            if (string.IsNullOrEmpty(CurrentProductQrCode))
                return; //这里不会执行到，只有扫描到才会触发changed 前面已经过滤掉了

            #region 1 ERP订单信息

            var url = ApiUrls.QueryOrder + CurrentProductQrCode + "&format=json";
            var product = WebService.Instance.Get1<ProductInfo>(url);
            if (product == null)
            {
                ShowError($"访问ERP失败,url：{url}");
                return;
            }

            var viewProductModel = ViewProductModel.GetViewProductModel(product, CurrentProductQrCode);

            #endregion

            #region 2 称重--更新称重信息 并创建线盘信息

            var weight = _weighingMachine.ReadWeight();
            if (weight < 0)
            {
                ShowError($"称重失败!");
                return;
            }

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

            viewProductModel.ReelWeight = reelWeight;
            viewProductModel.PackagePaperWeight = packageWeight;

            var tReelCode = GetTReelCode(product, CurrentProductQrCode, netWeight, weight);

            #endregion

            #region 3 更新码垛信息

            if (product.package_info.stacking_per_layer is 0 or null)
            {
                ShowError($"包装信息错误：stacking_per_layer = 0 !");
                return;
            }

            if (product.package_info.stacking_layers is 0 or null)
            {
                ShowError($"包装信息错误：stacking_layers = 0 !");
                return;
            }

            if (product.package_info.packing_quantity is 0 or null)
            {
                ShowError($"包装信息错误：packing_quantity = 0 !");
                return;
            }

            CurrentStackLayer = (int)product.package_info.stacking_layers;
            CurrentStackNumPerLayer = (int)product.package_info.stacking_per_layer;
            CurrentReelNumPerBox = (int)product.package_info.packing_quantity;

            #endregion

            #region 4 获取界面model

            viewProductModel.ProductionBatchNumber = @$"{CurrentProductQrCode}-{DateTime.Now: MMdd}A";
            viewProductModel.PackingGroupCode = GlobalVar.CurrentUserInfo.code;
            viewProductModel.PackingGroupName = GlobalVar.CurrentUserInfo.packageGroupName;
            viewProductModel.GrossWeight = weight;
            viewProductModel.PackagePaperWeight = PaperWeight ?? 0d;
            viewProductModel.MotherTrayCode = CurrentMotherTrayQrCode;

            #endregion

            #region 5 获取标签

            var template = FreeSql.Select<T_label_template>()
                .Where(s => s.PackageCode == product.package_info.code && s.LabelType == 101)
                .First();
            if (template == null)
            {
                ShowError("找不到对应的标签模板！无法继续下一步！");
                return;
            }

            viewProductModel.LabelTemplateName = $"{template.PackageCode} - {template.PackageName}";
            viewProductModel.ImageBox = null;
            viewProductModel.BoxQrCode = "";
            ViewProductModel = viewProductModel;

            #endregion

            // 更新队列信息并打印标签
            UpdateAndPrint(viewProductModel, product, weight, template, tReelCode);
        }

        private void UpdateAndPrint(ViewProductModel viewProductModel, ProductInfo product, double weight,
            T_label_template template, T_preheater_code preheaterCode)
        {
            _countReelInBox++;
            _countReelTotal++;
            var myReelInfo = new MyReelInfo
            {
                ReelProductCode = CurrentProductQrCode!,
                GrossWeight = weight,
                NetWeight = viewProductModel.NetWeight,
                CountOneBox = CurrentReelNumPerBox,
                PreheaterCode = preheaterCode
            };
            if (BoxListTemp.Count == 0)
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

            var latest = BoxListTemp.Last();
            //更新界面
            SelectedMyBoxInfo = latest;
            SelectedMyBoxInfo?.ReelList.ForEach(s => ReelListTemp.Add(s));
            //打印标签
            if (latest.IsFull() && !latest.HasPrint)
            {
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
                    @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:yyMM}{_countReelInBox:D4}-{wei:D5}";

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
                report.LoadLayout(template.TemplateFileName);
                report.DataSource = boxReportModel;
                latest.Report = report;

                var type = FreeSql.Select<T_label_type>().Where(s => s.LabelType == template.LabelType).First();
                if (type == null)
                {
                    MessageBox.Show($"找不到LabelType:{template.LabelType}对应的打印机，请前往配置打印机页面配置！");
                }
                else
                {
                    latest.Report?.Print(PMES.Default.PrinterName1);
                }

                //清空计数
                _countReelInBox = 0;
                MyReelInfo.Reset();
                //MyBoxInfo.Reset();
            }
        }

        private void UpdateQueueInfo()
        {
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

        #endregion
    }
}