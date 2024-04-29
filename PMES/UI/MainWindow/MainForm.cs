using System.Drawing.Imaging;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using PMES.Core;
using PMES.Core.Managers;
using PMES.Model;
using PMES.Model.Packages;
using PMES.UI.Report;
using PMES.UI.Settings;
using Serilog;
using SICD_Automatic.Core;

namespace PMES.UI.MainWindow;

public partial class MainForm : XtraForm
{
    private readonly ILogger _logger;
    private ProductInfo _productInfo = new();
    private readonly double _skinWeight = 2.01;
    private readonly double _tareWeight = 0;
    private readonly double _totalWeight = 52.03;
    private int _num = 0;

    #region 线盘 | 箱子(子托) | 母托信息

    private List<XPanInfo> PanInfos = new List<XPanInfo>();
    private List<BoxInfo> BoxInfos = new List<BoxInfo>();
    private TrayInfo TrayInfo = new TrayInfo();

    #endregion

    #region 扫码枪扫到的当前字段

    //a. 扫码枪扫母托，获得[母托盘码]
    //b.扫码生产条码，获取订单信息
    //c.扫皮重码，获皮重重量，

    /// <summary>
    ///     当前母托盘信息
    /// </summary>
    public string CurrentTrayCode { get; set; }

    /// <summary>
    ///     包装纸皮重
    /// </summary>
    public static string PackingPaperTareWeight { get; set; }

    public double PackingPaperWeight => string.IsNullOrEmpty(PackingPaperTareWeight) ? 0.02 : 1.01;

    /// <summary>
    ///     其他-生产订单信息
    /// </summary>
    public string CurrentProductCode { get; set; }

    #endregion

    #region 称重模块

    /// <summary>
    ///     当前称重
    /// </summary>
    private double _currentTotalWeight = 51.28;

    /// <summary>
    ///     当前皮重
    /// </summary>
    private double _currentSkinWeight = 0;

    /// <summary>
    ///     当前净重
    /// </summary>
    private double _currentNetWeight => _currentTotalWeight - _currentSkinWeight - PackingPaperWeight;

    /// <summary>
    ///     称对象
    /// </summary>
    private WeighingMachine _weighingMachine;

    #endregion

    public MainForm()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
        _logger = SerilogManager.GetOrCreateLogger();
        _weighingMachine = new WeighingMachine(_logger);
        _weighingMachine.Open("COM3", 115200);
        //Task.Run(async () =>
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            await Task.Delay(1000);
        //            var ret = await _weighingMachine.GetWeight();
        //            Console.WriteLine($"取三次均值的重量：{ret}");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //    }
        //});
        UpdateProductInfo(new ProductInfo());
    }


    /// <summary>
    ///     扫码枪触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ScanCodeChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtScanCode.Text)) return;

        if (txtScanCode.Text.Length < 5) return;

        if (txtScanCode.Text.StartsWith("TP"))
        {
            CurrentTrayCode = txtScanCode.Text;
            lb_parentCode.Text = @$"母托编码：<color=red> {CurrentTrayCode}</color> ";
        }
        else if (txtScanCode.Text.StartsWith("999"))
        {
            PackingPaperTareWeight = txtScanCode.Text;
            lbTareWeight.Text = PackingPaperWeight.ToString("F2");
        }
        else
        {
            CurrentProductCode = txtScanCode.Text;
            try
            {
                var product = await WebService.Instance.Get<ProductInfo>(
                    $"https://test.chengzhong-api.site.xiandeng.com:3443/api/product-info?semi_finished={txtScanCode.Text}");
                if (product.Equals(null)) return;
                await UpdateProductInfo(product);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
            }
        }
    }

    private async Task UpdateProductInfo(ProductInfo product)
    {
        if (txtScanCode.Text.Length < 10) return;

        #region 产品信息

        lb_product_order_no.Text = lb_product_order_no.Tag + product.product_order_no;
        lb_customer_number.Text = lb_customer_number.Tag + product.customer_number;
        lb_customer_name.Text = lb_customer_name.Tag + product.customer_name;
        lb_material_ns_model.Text = lb_material_ns_model.Tag + product.material_ns_model;
        lb_customer_material_spec.Text = lb_customer_material_spec.Tag + product.xpzl_spec;
        lb_jsbz_number.Text = lb_jsbz_number.Tag + product.jsbz_number;
        lb_jsbz_name.Text = lb_jsbz_name.Tag + product.jsbz_name;
        lb_xpzl_number.Text = lb_xpzl_number.Tag + product.xpzl_number;
        lb_xpzl_name.Text = lb_xpzl_name.Tag + product.xpzl_name;
        lb_package_info_code.Text = lb_package_info_code.Tag + product.package_info.code;
        lb_package_info_name.Text = lb_package_info_name.Tag + product.package_info.name;
        lb_material_execution_standard.Text =
            lb_material_execution_standard.Tag + product.material_execution_standard;
        lb_product_date.Text = lb_product_date.Tag + product.product_date;
        lb_machine_number.Text = lb_machine_number.Tag + product.machine_number;
        lb_operator_code.Text = lb_operator_code.Tag + product.operator_code;
        lb_userName.Text = lb_userName.Tag + GlobalVar.CurrentUserInfo.name;
        lb_userCode.Text = lb_userCode.Tag + GlobalVar.CurrentUserInfo.code;
        lb_BatchCode.Text = @$"{lb_BatchCode.Tag}{txtScanCode.Text}-{DateTime.Now: MMdd}A";

        lb_fix_prod_code.Text = lb_fix_prod_code.Tag + txtScanCode.Text;

        #endregion

        #region 子母托

        lb_childCode.AllowHtmlString = true;
        lb_parentCode.AllowHtmlString = true;
        lb_PackageNums.AllowHtmlString = true;


        lb_parentCode.Text = @$"母托盘码：<color=red>{CurrentTrayCode ?? "testTray"}</color>";
        var t1 = product.package_info.wire_reel_inside_package_name ?? "发货木托";
        lb_childCode.Text = @$"子托盘：<color=red>{t1}</color>";
        var t2 = product.package_info.wire_reel_external_package_name ?? "纸箱";
        lb_paperBox.Text = @$"纸箱：<color=red>{t2}</color>";
        lb_PackageNums.Text = @$"装箱件数：<color=red>{product.package_info.packing_quantity}</color>";

        #endregion

        #region 计重区域

        //1 称重->更新毛重
        lbGrossWeight.AllowHtmlString = true;
        lbTotalWeight.AllowHtmlString = true;
        lbSkinWeight.AllowHtmlString = true;
        lbNetWeight.AllowHtmlString = true;
        lb_xpzl_weight.AllowHtmlString = true;
        lbBoxCode.AllowHtmlString = true;

        _currentTotalWeight = await _weighingMachine.GetWeight();
        lbGrossWeight.Text = @$"<color=red>{_currentTotalWeight:F2}</color>";
        lbTotalWeight.Text = @$"<color=red><b>{_currentTotalWeight:F2}</b></color>";

        //2 更新皮重 --> 净重 和 总重需要在称的代码里实现
        if (double.TryParse(product.xpzl_weight, out double sw))
        {
            _currentSkinWeight = sw;
        }

        lbSkinWeight.Text = @$"<color=red>{double.Parse(product.xpzl_weight):F2}</color> kg";

        //3 更新净重 todo:这里需要加入最大值最小值校验
        lbNetWeight.Text = @$"<color=red>{_currentNetWeight:F2}</color>";
        lb_xpzl_weight.Text = @$"线盘重量：<color=red>{_currentNetWeight:F2} kg</color>";

        //4 箱码 最后五位是重量
        var weight = (_currentNetWeight / 1000).ToString("F5").Split(".")[1];
        lbBoxCode.Text =
            @$"{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}";

        #endregion

        #region 创建盘码

        _num++;
        PanInfos.Add(new XPanInfo
        {
            batchNo = lb_BatchCode.Text,
            customerCode = $"{GlobalVar.CurrentUserInfo.code}{DateTime.Now:MMdd}{_num:D4}",
            customerId = product.customer_id.ToString(),
            customerName = product.customer_name,
            custormerMaterialCode = null,
            custormerMaterialName = null,
            custormerMaterialSpec = null,
            grossWeight = _currentTotalWeight.ToString("F2"),
            icmobillNO = null,
            machineCode = product.machine_number,
            machineId = product.machine_id.ToString(),
            machineName = product.machine_name,
            netWeight = _currentNetWeight.ToString("F2"),
            operatorCode = product.operator_code,
            operatorName = product.operator_name,
            packageInfo = new PackageInfoPanCode
            {
                deliverySubTrayName = product.package_info.delivery_sub_tray_name,
                fullCoilWeight = product.package_info.cu_full_coil_weight,
                maxWeight = product.package_info.cu_max_weight,
                minWeight = product.package_info.cu_min_weight,
                packagingReqCode = null,
                packagingReqName = null,
                packingQuantity = product.package_info.packing_quantity,
                preheaterInsidePackageName = product.package_info.wire_reel_inside_package_name,
                preheaterOutsidePackageName = product.package_info.wire_reel_external_package_name,
                productionCode = product.package_info.code,
                stackingLayers = product.package_info.stacking_layers.ToString(),
                stackingPerLayer = product.package_info.stacking_per_layer.ToString(),
                tareWeight = _currentNetWeight.ToString("F2")
            },
            preheaterCode = $"{GlobalVar.CurrentUserInfo.code}{DateTime.Now:MMdd}{_num:D4}",
            preheaterId = null,
            preheaterName = null,
            preheaterSpec = null,
            preheaterWeight = null,
            productCode = txtScanCode.Text,
            productDate = DateTime.Now.ToString(),
            productGBName = null,
            productId = null,
            productMnemonicCode = null,
            productName = null,
            productSpec = null,
            productStandardName = null,
            productionBarCode = null,
            productionOrgNO = null,
            psn = null,
            status = 0,
            stockCode = null,
            stockId = null,
            stockName = null,
            userId = GlobalVar.CurrentUserInfo.userId,
            userStandardCode = null,
            userStandardId = null,
            userStandardName = null,
            weightUserID = null
        });

        this.gridControlBoxChild.DataSource = null;
        this.gridControlBoxChild.DataSource = PanInfos;

        var boxInfo = BoxInfos.FirstOrDefault(x => x.boxCode == lbBoxCode.Text);
        if (boxInfo == null)
        {
            boxInfo = new BoxInfo
            {
                boxCode = lbBoxCode.Text,
                grossWeightTotal = "",
                netWeightTotal = "",
                num = null,
                preheaterModelList = new List<PreheaterMode>(),
                userId = GlobalVar.CurrentUserInfo.userId
            };
            BoxInfos.Add(boxInfo);
        }

        boxInfo.preheaterModelList.Add(new PreheaterMode
        {
            crossWeight = _currentTotalWeight.ToString("F2"),
            netWeight = _currentNetWeight.ToString("F2"),
            preheaterCode = "null",
            productionCode = "null"
        });
        //箱子重新计重
        boxInfo.grossWeightTotal =
            boxInfo.preheaterModelList.Select(s => double.Parse(s.crossWeight)).Sum().ToString("F2");
        boxInfo.netWeightTotal = boxInfo.preheaterModelList.Select(s => double.Parse(s.netWeight)).Sum().ToString("F2");

        gridControlBox.DataSource = null;
        gridControlBox.DataSource = BoxInfos;

        gridView1.Columns.ForEach(s => s.BestFit());
        gridView2.Columns.ForEach(s => s.BestFit());
        #endregion

        #region 创建箱码

        #endregion
    }

    private void PreviewReport()
    {
        var report = new ReportCertificateXD();
        report.DataSource = new List<Certificate>
        {
            new()
        };
        //report.ShowPreview();
        report.ExportToImage("certificate.Png", ImageFormat.Png);
        var bitmap = new Bitmap("certificate.Png");
        picCertificate.Image?.Dispose();
        picCertificate.Image = bitmap;

        var report2 = new ReportPackingList();
        report2.DataSource = new List<PackingList>
        {
            new()
        };
        //report2.ShowPreview();
        report2.ExportToImage("boxList.Png", ImageFormat.Png);
        var bitmap2 = new Bitmap("boxList.Png");
        picBoxList.Image?.Dispose();
        picBoxList.Image = bitmap2;
    }


    #region 按钮事件

    /// <summary>
    ///     选择不同的打印标签模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LabelSettings(object sender, EventArgs e)
    {
    }

    /// <summary>
    ///     串口通讯设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CommunicationSettings(object sender, EventArgs e)
    {
        var comSettings = new SerialPortSettings();
        comSettings.ShowDialog();
    }

    /// <summary>
    ///     打印机设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PrinterSettings(object sender, EventArgs e)
    {
        //var report = new ReportCertificateXD();
        //report.DataSource = new List<Certificate>()
        //{
        //    new Certificate()
        //};
        //report.ShowPreview();

        var report = new ReportPackingList();
        report.DataSource = new List<PackingList>
        {
            new()
        };
        report.ShowDesigner();
        //report.ShowPreview();
        //report.ExportToImage("boxList.jpg");
    }

    private void Exit(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void Save(object sender, EventArgs e)
    {
    }

    private void Delete(object sender, EventArgs e)
    {
    }

    private void Print(object sender, EventArgs e)
    {
    }

    private void HistorySearch(object sender, EventArgs e)
    {
    }

    #endregion

    private void gridView1_CustomDrawRowIndicator(object sender,
        DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
    {
        if (e.Info.IsRowIndicator && e.RowHandle >= 0)
        {
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        gridView1.Columns.ForEach(s=>s.BestFit());
        gridView2.Columns.ForEach(s=>s.BestFit());
    }
}