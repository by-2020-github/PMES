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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreeSql.DataAnnotations;
using K4os.Hash.xxHash;
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.Model;
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

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        Thread.Sleep(1000);
            //        ViewProductModel = new ViewProductModel() { ProductOrder = new Random().Next(100).ToString() };
            //    }
            //});
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
            var product = WebService.Instance.Get<ProductOrderInfo>(url);
            if (product == null)
            {
                ShowError($"访问ERP失败,url：{url}");
                return;
            }

            #endregion

            #region 2 称重--更新称重信息

            var weight = _weighingMachine.ReadWeight();
            if (weight < 0)
            {
                ShowError($"称重失败!");
                return;
            }

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

            var viewProductModel = ViewProductModel.GetViewProductModel(product, CurrentProductQrCode);
            viewProductModel.ProductionBatchNumber = @$"{CurrentProductQrCode}-{DateTime.Now: MMdd}A";
            viewProductModel.PackingGroupCode = GlobalVar.CurrentUserInfo.code;
            viewProductModel.PackingGroupName = GlobalVar.CurrentUserInfo.packageGroupName;
            viewProductModel.GrossWeight = weight;
            viewProductModel.PackagePaperWeight = PaperWeight ?? 0d;

            viewProductModel.MotherTrayCode = CurrentMotherTrayQrCode;

            #endregion


            #region 5 获取标签

            viewProductModel.LabelTemplate = "";
            viewProductModel.ImageReel = null;
            viewProductModel.ImageBox = null;
            viewProductModel.BoxQrCode = "";

            #endregion


            ViewProductModel = viewProductModel;

            #region 判断是否满箱了

            _countReelInBox++;
            _countReelTotal++;
            var wei = (int)(100 * viewProductModel.NetWeight);
            var boxCode =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:yyMM}{_countReelInBox:D4}-{wei:D5}";
            var myReelInfo = new MyReelInfo
            {
                ReelQrCode = boxCode,
                ReelProductCode = CurrentProductQrCode,
                GrossWeight = weight,
                NetWeight = viewProductModel.NetWeight,
                CountOneBox = CurrentReelNumPerBox
            };
            if (BoxListTemp.Count == 0)
            {
                var myBoxInfo = new MyBoxInfo();
                myBoxInfo.ReelList.Add(myReelInfo);
                BoxListTemp.Add(myBoxInfo);
            }
            else
            {
                var myBoxInfo = BoxListTemp.Last();
                if (myBoxInfo.ReelList.Count < (int)product.package_info.packing_quantity)
                {
                    myBoxInfo.ReelList.Add(myReelInfo);
                }
                else
                {
                    var myBoxInfo2 = new MyBoxInfo();
                    myBoxInfo2.ReelList.Add(myReelInfo);
                    BoxListTemp.Add(myBoxInfo2);
                }
            }

            ReelListTemp.Add(myReelInfo);

            #endregion
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

        #region 通用方法

        void ShowError(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void ShowInfo(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }

    public class ViewProductModel : ObservableObject
    {
        public ViewProductModel(ProductOrderInfo productOrderInfo, string productQrCode)
        {
            ProductOrderInfo = productOrderInfo;
            ProductQrCode = productQrCode;
        }

        public ProductOrderInfo ProductOrderInfo { get; set; }

        #region 额外包装信息

        public string? MotherTrayCode { get; set; }
        public string? ChildTrayCode { get; set; }
        public string? PackagePaperBox { get; set; }
        public int? ReelNumPerBox { get; set; }

        #endregion

        public string LabelTemplate { get; set; } = "Default";

        /// <summary>
        ///     生产工单
        /// </summary>
        public string ProductOrder { get; set; } = "";

        /// <summary>
        ///    产品代码
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        ///    产品型号
        /// </summary>
        public string ProductModel { get; set; } = "";

        /// <summary>
        ///    国际型号
        /// </summary>
        public string InternationalModel { get; set; } = "";

        /// <summary>
        ///    产品规格
        /// </summary>
        public string ProductSpecification { get; set; } = "";

        /// <summary>
        ///    用户标准代码
        /// </summary>
        public string UserStandardCode { get; set; } = "";

        /// <summary>
        ///    用户标准名称
        /// </summary>
        public string UserStandardName { get; set; } = "";

        public string ReelCode { get; set; } = "";
        public string ReelName { get; set; } = "";
        public double ReelWeight { get; set; } = new Random().NextDouble();

        /// <summary>
        ///     包装代码
        /// </summary>
        public string PackingCode { get; set; } = "";

        /// <summary>
        ///     包装名称
        /// </summary>
        public string PackingName { get; set; } = "";

        public string ExecutionStandard { get; set; } = "";
        public string ProductionDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// 生产机台
        /// </summary>
        public string ProductionMachine { get; set; } = "";

        /// <summary>
        /// 生产批号
        /// </summary>
        public string ProductionBatchNumber { get; set; } = "";

        /// <summary>
        /// 生产工号
        /// </summary>
        public string ProductionNumber { get; set; } = "";

        /// <summary>
        /// 包装组编号
        /// </summary>
        public string PackingGroupCode { get; set; } = "";

        /// <summary>
        /// 包装组名称
        /// </summary>
        public string PackingGroupName { get; set; } = "";

        /// <summary>
        /// 产品码（生产条码 扫描到的码）
        /// </summary>
        public string ProductQrCode { get; set; }

        /// <summary>
        ///     合格证
        /// </summary>
        public Bitmap? ImageReel { get; set; }

        /// <summary>
        ///     箱码
        /// </summary>
        public Bitmap? ImageBox { get; set; }

        #region 称重信息

        public double GrossWeight { get; set; } = 30.12d;
        public double NetWeight => GrossWeight - PackagePaperWeight - SkinWeight;
        public double PackagePaperWeight { get; set; } = 0.02d;
        public double SkinWeight { get; set; } = 0.09;

        #endregion

        #region 条码信息

        public string BoxQrCode { get; set; } = "TY4121050-A206-BZ001-B12310001-04903";

        #endregion

        public static ViewProductModel GetViewProductModel(ProductOrderInfo product, string productQrCode)
        {
            var viewProductModel = new ViewProductModel(product, productQrCode)
            {
                ProductOrder = product.product_order_no,
                ProductCode = product.customer_number,
                ProductModel = product.customer_name,
                InternationalModel = product.material_ns_model,
                ProductSpecification = product.material_spec,
                UserStandardCode = product.jsbz_number,
                UserStandardName = product.jsbz_name,
                ReelCode = product.xpzl_number,
                ReelName = product.xpzl_name,
                PackingCode = product.package_info.code ?? "",
                PackingName = product.package_info.name ?? "",
                ExecutionStandard = product.material_execution_standard,
                ProductionDate = product.product_date,
                ProductionMachine = product.machine_number,
                ProductionNumber = product.operator_code,
                ChildTrayCode = product.package_info.delivery_sub_tray_number,
                //TODO: 纸箱不知道是哪个字段
                PackagePaperBox = product.package_info.wire_reel_external_package_name,
                ReelNumPerBox = (int)product.package_info.packing_quantity!
            };
            if (double.TryParse(product.xpzl_weight, out var w))
            {
                viewProductModel.ReelWeight = w;
            }
            else
            {
                viewProductModel.ReelWeight = -1;
            }

            if (double.TryParse(product.package_info.tare_weight, out double sw))
            {
                viewProductModel.SkinWeight = sw;
            }
            else
            {
                viewProductModel.SkinWeight = -1;
            }

            return viewProductModel;
        }
    }

    /// <summary>
    ///     箱码信息
    /// </summary>
    public class MyBoxInfo
    {
        private static int _number = 0;

        public int Number { get; set; }
        public string BoxQrCoed { get; set; } = "TY4121050-A206-BZ001-B12310001-04903";
        public List<MyReelInfo> ReelList { get; set; } = new List<MyReelInfo>();

        public double GrossWeight { get; set; } = new Random().NextDouble();
        public double NetWeight { get; set; } = new Random().NextDouble();
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public MyBoxInfo()
        {
            Number = ++_number;
        }

        public static void Reset()
        {
            _number = 0;
        }
    }

    /// <summary>
    ///     盘码信息
    /// </summary>
    [Table(Name = "tb_temp_reels")]
    public class MyReelInfo
    {
        private static int _number = 0;

        public int Number { get; set; }

        /// <summary>
        ///     每箱装多少个
        /// </summary>
        public int CountOneBox { get; set; }

        public string ReelQrCode { get; set; } = "B10001-1";
        public string ReelProductCode { get; set; } = "G23100061G010001";
        public double GrossWeight { get; set; } = new Random().NextDouble();
        public double NetWeight { get; set; } = new Random().NextDouble();
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public MyReelInfo()
        {
            Number = ++_number;
        }

        public static void Reset()
        {
            _number = 0;
        }
    }
}