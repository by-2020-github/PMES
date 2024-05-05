using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Wizards.Templates;
using DevExpress.XtraRichEdit.Import.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PMES.Core;
using PMES.Core.Managers;
using PMES.Model;
using PMES.Model.ApiResponse;
using PMES.Model.Packages;
using PMES.Model.report;
using PMES.UI.MainWindow.ChildPages;
using PMES.UI.Report;
using PMES.UI.Settings;
using Serilog;
using SICD_Automatic.Core;

namespace PMES.UI.MainWindow;

public partial class MainForm : XtraForm
{
    private int _totalNum = 1;
    private readonly ILogger _logger;
    private readonly IFreeSql _freeSql = FreeSqlManager.FSql;
    private ProductInfo _productInfo = new();
    private readonly double _skinWeight = 2.01;
    private readonly double _tareWeight = 0;
    private readonly double _totalWeight = 52.03;
    private int _num = 0;
    private GridControl _currentControl;

    #region 线盘 | 箱子(子托) | 母托信息

    /// <summary>
    ///     自动入库的临时列表
    /// </summary>
    private List<XPanInfo> _panInfos = new List<XPanInfo>();

    /// <summary>
    ///     自动入库的临时列表
    /// </summary>
    private List<BoxTemplateInfo> _boxInfos = new List<BoxTemplateInfo>();

    /// <summary>
    ///     手动入库的话使用这个列表
    /// </summary>
    private List<XPanInfo> _xPanInfosManualPost = new List<XPanInfo>();

    /// <summary>
    ///     手动入库的话使用这个列表
    /// </summary>
    private List<BoxTemplateInfo> _boxInfosManualPost = new List<BoxTemplateInfo>();

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
        lb_user.Text = GlobalVar.CurrentUserInfo.username;
        _logger = SerilogManager.GetOrCreateLogger();
        _weighingMachine = new WeighingMachine(_logger);
        _weighingMachine.Open("COM3", 115200);
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
                    $"{ApiUrls.QueryOrder}{txtScanCode.Text}");
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
        lb_userName.Text = lb_userName.Tag + GlobalVar.CurrentUserInfo.username;
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
        if (double.TryParse(product.package_info.tare_weight, out double sw))
        {
            _currentSkinWeight = sw;
        }

        lbSkinWeight.Text = @$"<color=red>{double.Parse(product.xpzl_weight):F2}</color> kg";

        //3 更新净重 todo:这里需要加入最大值最小值校验
        lbNetWeight.Text = @$"<color=red>{_currentNetWeight:F2}</color>";
        lb_xpzl_weight.Text = @$"线盘重量：<color=red>{_currentNetWeight:F2} kg</color>";

        //4 箱码 最后五位是重量
        // 52.00 -> [0.05200] ->  05200  #####
        //包装条码；产品助记码 + 线盘分组代码 + 用户标准代码 + 包装组编号 + 年月 + 4位流水号 + 装箱净重，
        //如TY4121050 - 14 - BZ001 - B12310001 - 04903

        //eg1:{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}"
        var weight = (_currentNetWeight / 1000).ToString("F5").Split(".")[1];
        lbBoxCode.Text =
            @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{_totalNum:D4}-{weight}";

        #endregion

        #region 创建盘码 | 箱码 | 判断是否装够

        var xPanInfo = new XPanInfo
        {
            batchNo = lb_BatchCode.Text,
            customerCode = $"{GlobalVar.CurrentUserInfo.code}{DateTime.Now:MMdd}{_num:D4}",
            customerId = product.customer_id.ToString(),
            customerName = product.customer_name,
            custormerMaterialCode = product.customer_material_number,
            custormerMaterialName = product.customer_material_name,
            custormerMaterialSpec = product.customer_material_name,
            grossWeight = _currentTotalWeight.ToString("F2"),
            icmobillNO = "", //todo:接口无说明,订单号，现在先不用管 2024年5月2日 22:47:34
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
                packagingReqCode = product.package_info.code,
                packagingReqName = product.package_info.name,
                packingQuantity = product.package_info.packing_quantity,
                preheaterInsidePackageName = product.package_info.wire_reel_inside_package_name ?? "",
                preheaterOutsidePackageName = product.package_info.wire_reel_external_package_name ?? "",
                productionCode = product.package_info.code,
                stackingLayers = product.package_info.stacking_layers.ToString(),
                stackingPerLayer = product.package_info.stacking_per_layer.ToString(),
                tareWeight = _currentNetWeight.ToString("F2")
            },
            preheaterCode = product.xpzl_number,
            preheaterId = product.xpzl_id.ToString(),
            preheaterName = product.xpzl_name,
            preheaterSpec = product.xpzl_spec,
            preheaterWeight = product.xpzl_weight,
            productCode = product.material_number,
            productDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            productGBName = product.material_ns_model,
            productId = product.material_id.ToString(),
            productMnemonicCode = product.material_mnemonic_code,
            productName = product.material_name,
            productSpec = product.customer_material_spec, //这里与接口不一样，已更新 2024年5月2日 22:47:46
            productStandardName = product.material_execution_standard,
            productionBarCode = txtScanCode.Text,
            productionOrgNO = product.product_org_number,
            psn = "",
            status = 0,
            stockCode = product.stock_number.ToString(),
            stockId = product.stock_id.ToString(),
            stockName = product.stock_name.ToString(),
            userId = GlobalVar.CurrentUserInfo.userId,
            userStandardCode = product.jsbz_number, //技术标准 2024年5月2日 22:54:33
            userStandardId = product.jsbz_id.ToString(), //
            userStandardName = product.jsbz_name,
            weightUserID = GlobalVar.CurrentUserInfo.userId.ToString()
        };
        _num++;
        _panInfos.Add(xPanInfo);
        if (!cbxAutoMode.Checked)
        {
            _xPanInfosManualPost.Add(xPanInfo);
        }

        Trace.WriteLine($"{JsonConvert.SerializeObject(xPanInfo)}");
        //todo:这里Post之后需要返回盘码的Id,并判断返回结果
        var post = await WebService.Instance.Post<XPanInfo>(xPanInfo, ApiUrls.BarcodeGeneratePreheaterCode);
        BoxTemplateInfo boxInfo;
        //1 首先判断是否是一个装的，如果是直接post 不post 箱码
        if (xPanInfo.packageInfo.packingQuantity <= 1)
        {
            boxInfo = new BoxTemplateInfo
            {
                boxCode = lbBoxCode.Text,
                grossWeightTotal = xPanInfo.grossWeight,
                netWeightTotal = xPanInfo.netWeight,
                num = product.package_info.packing_quantity.ToString(),
                preheaterModelList = new List<PreheaterTemplateInfo>(),
                userId = GlobalVar.CurrentUserInfo.userId
            };
            boxInfo.preheaterModelList.Add(new PreheaterTemplateInfo
            {
                crossWeight = xPanInfo.grossWeight,
                netWeight = xPanInfo.netWeight,
                preheaterCode = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}0001",
                productionCode = xPanInfo.productionBarCode,
            });
            _boxInfos.Add(boxInfo);
            _totalNum++;
            _panInfos.Clear(); //直接清空盘码信息
        }
        else
        {
            for (var i = 1; i < _panInfos.Count; i++)
            {
                if (_panInfos[0].Equals(_panInfos[i])) continue;
                XtraMessageBox.Show("同一箱中盘码不同！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _panInfos.Clear(); //直接清空盘码信息
                return;
            }

            if (xPanInfo.packageInfo.packingQuantity == _panInfos.Count)
            {
                boxInfo = new BoxTemplateInfo
                {
                    grossWeightTotal = _panInfos.Select(s => double.Parse(s.grossWeight)).Sum().ToString("F2"),
                    netWeightTotal = _panInfos.Select(s => double.Parse(s.netWeight)).Sum().ToString("F2"),
                    num = _panInfos[0].packageInfo.packingQuantity.ToString("D"),
                    userId = GlobalVar.CurrentUserInfo.userId
                };
                var index = 1;
                _panInfos.ForEach(xp =>
                {
                    boxInfo.preheaterModelList.Add(new PreheaterTemplateInfo
                    {
                        crossWeight = xp.grossWeight,
                        netWeight = xp.netWeight,
                        preheaterCode = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{DateTime.Now:MMdd}{index:D4}",
                        productionCode = xp.productionBarCode,
                    });
                    index++;
                });

                var w = ((int)double.Parse(boxInfo.netWeightTotal) * 100).ToString("D5");
                lbBoxCode.Text =
                    @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{_totalNum:D4}-{w}";

                boxInfo.boxCode = lbBoxCode.Text;
                _boxInfos.Add(boxInfo);
                if (!cbxAutoMode.Checked)
                {
                    _boxInfosManualPost.Add(boxInfo);
                }
                _totalNum++;

                //todo:这里post箱码
                var boxPost = new BoxInfoPost
                {
                    boxCode = boxInfo.boxCode,
                    grossWeightTotal = boxInfo.grossWeightTotal,
                    netWeightTotal = boxInfo.netWeightTotal,
                    lableId = 0,
                    numOfPackedItems = boxInfo.preheaterModelList.Count,
                    preheaterIds = string.Join(",", boxInfo.preheaterModelList.Select(s => s.Id.ToString())),
                    weightUserId = boxInfo.userId
                };
                var res =await WebService.Instance.Post<BoxInfoPost>(boxPost, ApiUrls.BarcodeGenerateBoxCode);
                if (res == null)
                {
                    XtraMessageBox.Show("提交箱码失败！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _panInfos.Clear(); //直接清空盘码信息,并移除此箱码信息
                    _boxInfos.RemoveAt(_boxInfos.Count - 1);
                    _boxInfosManualPost.RemoveAt(_boxInfos.Count - 1);
                    return;
                }
            }


            _panInfos.Clear(); //直接清空盘码信息
        }

        #endregion

        #region 更新表格

        this.gridControlBoxChild.DataSource = null;
        this.gridControlBoxChild.DataSource = _boxInfos.Last().preheaterModelList;
        gridControlBox.DataSource = null;
        gridControlBox.DataSource = _boxInfos;
        gridViewBox.Columns.ForEach(s => s.BestFit());
        gridViewXp.Columns.ForEach(s => s.BestFit());

        #endregion

        #region 更新码垛信息

        lb_putStyle.Text = @$"放置方式：竖方式";
        lb_layers.Text = @$"码垛层数：{product.package_info.stacking_layers}";
        lb_numsPerLayer.Text = @$"每层轴数：{product.package_info.stacking_per_layer}";

        var layers = product.package_info.stacking_layers;
        layers = layers <= 0 ? 1 : layers;
        var perNum = product.package_info.stacking_per_layer;
        perNum = perNum <= 0 ? 2 : perNum;
        lb_currentInfo.Text = @$"已码{Math.Ceiling(_num * 1f / perNum):F0}层,共{_num}个";
        lb_leftNum.Text = @$"剩余个数：{layers * perNum - _num}";

        if (_num == layers * perNum)
        {
            XtraMessageBox.Show("托盘已满，请移走.", "Info:", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //清空内容
            _num = 0;
            _boxInfos.Clear();
            _panInfos.Clear();
        }

        #endregion
    }

    #region 按钮事件

    /// <summary>
    ///     选择不同的打印标签模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LabelSettings(object sender, EventArgs e)
    {
        var form = new TemplateManage(_logger);
        form.ShowDialog();
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
    ///     打印模板设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EditPrintTemplate(object sender, EventArgs e)
    {
        //var report = new ReportCertificateXD();
        //report.DataSource = new List<Certificate>()
        //{
        //    new Certificate()
        //};
        //report.ShowPreview();

        //var report = new ReportPackingList();
        //report.DataSource = new List<PackingList>
        //{
        //    new()
        //};
        //report.ShowDesigner();
        //report.ShowPreview();
        //report.ExportToImage("boxList.jpg");

        var form = new ReportTemplate(_logger);
        form.ShowDialog();
    }

    private void Exit(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void Save(object sender, EventArgs e)
    {
    }

    private async void Delete(object sender, EventArgs e)
    {
        if (_currentControl == gridControlBoxChild) //删除盘码
        {
            var rowXp = gridViewXp.FocusedRowHandle;
            if (rowXp < 0) return;
            var templateInfo = gridViewXp.GetRow(rowXp) as PreheaterTemplateInfo;
            if (templateInfo == null) return;

            var result = XtraMessageBox.Show("是否确认删除？", "", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //todo:执行真实的删除操作
                gridViewXp.DeleteSelectedRows();
                var content = new Dictionary<string, string> { { "preheaterCodeId", $"{templateInfo.Id}" } };
                var res = await WebService.Instance.Post<ResponseBase<object>>(content,
                    ApiUrls.BarcodeDeletePreheaterCode);
                XtraMessageBox.Show("删除成功！", "", MessageBoxButtons.OK);
                //级联删除箱码，如果盘码已经删除完了 就把箱码也删了
                if (gridControlBoxChild.DataSource == null)
                {
                    var rowBox = gridViewBox.FocusedRowHandle;
                    if (rowBox < 0) return;
                    var boxTemplateInfo = gridViewBox.GetRow(rowBox) as BoxTemplateInfo;
                    if (boxTemplateInfo == null) return;
                    var c = new Dictionary<string, string> { { "boxCodeId", $"{boxTemplateInfo.Id}" } };
                    var r = await WebService.Instance.Post<ResponseBase<object>>(content,
                        ApiUrls.BarcodeDeleteBoxCode);
                    XtraMessageBox.Show("箱码级联删除成功！", "", MessageBoxButtons.OK);
                }

            }
            else if (result == DialogResult.Cancel)
            {
                XtraMessageBox.Show("取消删除！", "", MessageBoxButtons.OK);
            }
        }
        else if (_currentControl == gridControlBox) //删除箱码
        {
            var rowBox = gridViewBox.FocusedRowHandle;
            if (rowBox < 0) return;
            var boxTemplateInfo = gridViewBox.GetRow(rowBox) as BoxTemplateInfo;
            if (boxTemplateInfo == null) return;

            var result = XtraMessageBox.Show("是否确认删除？", "", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //todo:执行真实的删除操作
                gridViewBox.DeleteSelectedRows();
                var content = new Dictionary<string, string> { { "boxCodeId", $"{boxTemplateInfo.Id}" } };
                var res = await WebService.Instance.Post<ResponseBase<object>>(content,
                    ApiUrls.BarcodeDeleteBoxCode);
                XtraMessageBox.Show("删除成功！", "", MessageBoxButtons.OK);
                //级联删除盘码
                foreach (var t in boxTemplateInfo.preheaterModelList)
                {
                    var c = new Dictionary<string, string> { { "preheaterCodeId", $"{t.Id}" } };
                    var r = await WebService.Instance.Post<ResponseBase<object>>(content,
                        ApiUrls.BarcodeDeletePreheaterCode);
                    XtraMessageBox.Show("盘码级联删除成功！", "", MessageBoxButtons.OK);
                }
            }
            else if (result == DialogResult.Cancel)
            {
                XtraMessageBox.Show("取消删除！", "", MessageBoxButtons.OK);
            }
        }

    }

    private void Print(object sender, EventArgs e)
    {
        //todo:打印盘码和箱码
        var rowXp = gridViewXp.FocusedRowHandle;
        var rowBox = gridViewBox.FocusedRowHandle;

        var report = new ReportPackingList();
        report.DataSource = new List<PackingList>
        {
            new()
        };
        report.Print();
    }

    private async void HistorySearch(object sender, EventArgs e)
    {
        var form = new HistoryFilter();
        var result = form.ShowDialog();
        if (result == DialogResult.OK)
        {
            var response =
                await WebService.Instance.PostJsonT<ResponseReport>(GlobalVar.ReportFilters, ApiUrls.BarcodeSearchHis);

            #region 合法性校验

            if (response == null)
            {
                var msg = $"查询异常，查询条件:{JsonConvert.SerializeObject(GlobalVar.ReportFilters)}";
                _logger?.Error(msg);
                XtraMessageBox.Show(msg, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.data == null)
            {
                var msg = $"后台接口返回异常，查询条件:{JsonConvert.SerializeObject(GlobalVar.ReportFilters)}";
                _logger?.Error(msg);
                XtraMessageBox.Show(msg, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.data.total == 0)
            {
                var msg = $"没有符合要求的数据，查询条件:{JsonConvert.SerializeObject(GlobalVar.ReportFilters)}";
                _logger?.Error(msg);
                XtraMessageBox.Show(msg, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            #region 显示结果

            var boxInfos = new List<BoxTemplateInfo>();
            response.data!.data!.ForEach(s =>
            {
                var boxInfo = new BoxTemplateInfo
                {
                    boxCode = s.box.packagingCode,
                    preheaterModelList = s.preheaterCodeList.Select(p => new PreheaterTemplateInfo
                    {
                        crossWeight = p.grossWeight,
                        netWeight = p.netWeight,
                        preheaterCode = p.preheaterCode,
                        productionCode = p.productCode
                    }).ToList(),
                    grossWeightTotal = s.preheaterCodeList.Select(x => double.Parse(x.grossWeight)).ToList().Sum()
                        .ToString("F2"),
                    netWeightTotal = s.preheaterCodeList.Select(x => double.Parse(x.netWeight)).ToList().Sum()
                        .ToString("F2")
                };
                boxInfos.Add(boxInfo);
            });
            gridControlBox.DataSource = null;
            gridControlBox.DataSource = boxInfos;
            if (boxInfos.Count > 0)
            {
                gridViewBox.FocusedRowHandle = 0;
                if (boxInfos[0].preheaterModelList.Count > 0)
                {
                    gridControlBoxChild.DataSource = null;
                    gridControlBoxChild.DataSource = boxInfos[0].preheaterModelList;
                }
            }

            gridViewBox.Columns.ForEach(s => s.BestFit());
            gridViewXp.Columns.ForEach(s => s.BestFit());

            #endregion
        }
        else
        {
            XtraMessageBox.Show("取消查询");
        }

        form.Dispose();
    }

    private void gridViewXp_FocusedRowChanged(object sender,
        DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        gridViewXp.FocusedRowChanged -= gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged -= gridViewBox_FocusedRowChanged;

        if (gridControlBoxChild.DataSource == null) return;
        if (e.FocusedRowHandle < 0) return;
        var preheaterModes = gridControlBoxChild.DataSource as List<PreheaterTemplateInfo>;
        if (preheaterModes == null) return;
        var preheaterMode = preheaterModes[e.FocusedRowHandle];

        var boxInfos = gridControlBox.DataSource as List<BoxTemplateInfo>;
        if (boxInfos == null) return;
        var boxRow = boxInfos.FindIndex(s => s.preheaterModelList.Contains(preheaterMode));
        if (boxRow == -1) return;
        gridViewBox.FocusedRowHandle = boxRow;

        gridViewXp.FocusedRowChanged += gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged += gridViewBox_FocusedRowChanged;
    }

    private void gridViewBox_FocusedRowChanged(object sender,
        DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        gridViewXp.FocusedRowChanged -= gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged -= gridViewBox_FocusedRowChanged;

        if (gridControlBox.DataSource == null) return;
        if (e.FocusedRowHandle < 0) return;
        var boxInfos = gridControlBox.DataSource as List<BoxTemplateInfo>;
        if (boxInfos == null) return;
        gridControlBoxChild.DataSource = null;
        gridControlBoxChild.DataSource = boxInfos?[e.FocusedRowHandle].preheaterModelList;

        gridViewXp.FocusedRowChanged += gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged += gridViewBox_FocusedRowChanged;
    }

    private void gridViewBox_CustomDrawRowIndicator(object sender,
        DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
    {
        if (e.Info.IsRowIndicator && e.RowHandle >= 0)
        {
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
    }

    private void gridControlBox_Click(object sender, EventArgs e)
    {
        _currentControl = gridControlBox;
    }

    private void gridControlBoxChild_Click(object sender, EventArgs e)
    {
        _currentControl = gridControlBoxChild;
    }

    #endregion


    private void MainForm_Load(object sender, EventArgs e)
    {
    }
}