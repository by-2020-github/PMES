﻿using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Windows.Forms.Integration;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.Design;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Wizards.Templates;
using DevExpress.XtraRichEdit.Import.Html;
using FreeSql;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PMES.Core;
using PMES.Core.Managers;
using PMES.Model;
using PMES.Model.report;
using PMES.Model.tbs;
using PMES.UI.MainWindow.ChildPages;
using PMES.UI.Report;
using PMES.UI.Settings;
using Serilog;
using SICD_Automatic.Core;

namespace PMES.UI.MainWindow;

public partial class MainForm : XtraForm
{
    private readonly ILogger _logger;
    private readonly IFreeSql _freeSql = FreeSqlManager.FSql;
    private ProductInfo _productInfo = new();
    private readonly double _skinWeight = 2.01;
    private readonly double _tareWeight = 0;
    private readonly double _totalWeight = 52.03;

    /// <summary>
    ///     线盘数量
    /// </summary>
    private int _preheaterNum = 0;

    /// <summary>
    ///     箱子数量
    /// </summary>
    private int _totalBoxNum = 1;

    private GridControl _currentControl;

    #region 线盘 | 箱子(子托) | 母托信息

    private List<int> _boxIdList = new List<int>();

    /// <summary>
    ///     自动入库的临时列表
    /// </summary>
    private List<T_preheater_code> _tPreheaterCodes = new List<T_preheater_code>();

    /// <summary>
    ///     自动入库的临时列表
    /// </summary>
    private List<T_box> _tBoxes = new List<T_box>();

    /// <summary>
    ///     手动入库的话使用这个列表
    /// </summary>
    private List<T_preheater_code> _tPreheaterCodesManual = new List<T_preheater_code>();

    /// <summary>
    ///     手动入库的话使用这个列表
    /// </summary>
    private List<T_box> _tBoxesManual = new List<T_box>();

    #endregion

    #region 扫码枪扫到的当前字段

    //a. 扫码枪扫母托，获得[母托盘码]
    //b.扫码生产条码，获取订单信息
    //c.扫皮重码，获皮重重量，

    /// <summary>
    ///     当前母托盘信息
    /// </summary>
    public string CurrentTrayCode { get; set; } = "TP12345678";

    /// <summary>
    ///     包装纸皮重
    /// </summary>
    public static string PackingPaperTareWeight { get; set; } = "0.02";

    /// <summary>
    ///     线盘皮重
    /// </summary>
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
    private WeighingMachine? _weighingMachine => GlobalVar.WeighingMachine;

    #endregion

    #region 打印相关

    private T_label _currentLabel;
    private T_label_template _printTemplatePCode;
    private T_label_template _printTemplateBoxCode;
    private T_label_template _printTemplateDeliveryCode;

    #endregion

    public MainForm()
    {
        InitializeComponent();
        this.WindowState = FormWindowState.Maximized;
        var element = new ElementHost() { Dock = DockStyle.Fill };
        tableLayoutPanelHeader.Controls.Add(element, 1, 0);
        Program.LogViewTextBox.FontSize = 12;
        element.Child = Program.LogViewTextBox;
        StartPosition = FormStartPosition.CenterScreen;
        lb_user.Text = GlobalVar.CurrentUserInfo.username;
        _logger = SerilogManager.GetOrCreateLogger();
        // _weighingMachine = new WeighingMachine(_logger);
        // _weighingMachine.Open("COM3", 115200);
        UpdateProductInfo(new ProductInfo());
    }

    private int _change = 1;

    /// <summary>
    ///     扫码枪触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ScanCodeChanged(object sender, EventArgs e)
    {
        if (_weighingMachine == null)
        {
            ShowErrorMsg("请在【通信设置里】先设置称的信息！");
            return;
        }

        if (!_weighingMachine.IsOpen)
        {
            ShowErrorMsg("端口号未打开，请先初始化称！");
            return;
        }

        if (cbxMigration.Checked)
        {
            if (_change == 1)
            {
                lbOld.Text = txtScanCode.Text;
                _change++;
            }
            else
            {
                lbNew.Text = txtScanCode.Text;
                if (await _freeSql.Insert<T_order_exchange>(new T_order_exchange
                    {
                        CreateTime = DateTime.Now,
                        NewCode = lbNew.Text,
                        OldCode = lbOld.Text,
                        WeightUserId = GlobalVar.CurrentUserInfo.userId
                    }).ExecuteAffrowsAsync() > 0)
                {
                    ShowInfoMsg("改线入库成功!");

                    lbNew.Text = "";
                    lbOld.Text = "";
                }

                _change--;
            }

            return;
        }

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
                if (product.package_info.packing_quantity == 0)
                {
                    product.package_info.packing_quantity = 1;
                    product.package_info.stacking_layers = 2;
                    product.package_info.stacking_per_layer = 4;
                }

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

        //4 箱码 最后五位是重量 放到插入数据那里更新
        // 52.00 -> [0.05200] ->  05200  #####
        //包装条码；产品助记码 + 线盘分组代码 + 用户标准代码 + 包装组编号 + 年月 + 4位流水号 + 装箱净重，
        //eg1:{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{weight}"

        #endregion

        #region 创建盘码 | 箱码 | 判断是否装够

        var tPreheaterCode = new T_preheater_code
        {
            BatchNO = @$"{txtScanCode.Text}-{DateTime.Now: MMdd}A",
            CreateTime = DateTime.Now,
            CustomerCode = product.customer_number,
            CustomerId = product.customer_id,
            CustomerMaterialCode = product.customer_material_number,
            CustomerMaterialName = product.customer_material_name,
            CustomerMaterialSpec = product.customer_material_spec,
            CustomerName = product.customer_name,
            GrossWeight = _currentTotalWeight,
            ICMOBillNO = product.product_order_no, //生产工单 之前是null，todo:确认是否需要
            IsDel = 0,
            IsQualified = 0, //是否合格
            MachineCode = product.machine_number,
            MachineId = product.machine_id,
            MachineName = product.machine_name,
            NetWeight = _currentNetWeight,
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
            ProductionBarcode = txtScanCode.Text,
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
        {
            var max = double.Parse(product.package_info.cu_max_weight);
            var min = double.Parse(product.package_info.cu_min_weight);
            if (_currentNetWeight < min)
            {
                tPreheaterCode.NoQualifiedReason = $"小于最小值:{min}";
                tPreheaterCode.IsQualified = 0;
            }

            if (_currentNetWeight > max)
            {
                tPreheaterCode.NoQualifiedReason = $"大于最大值:{max}";
                tPreheaterCode.IsQualified = 0;
            }
        }
        _preheaterNum++; //增加了一个线盘

        _tPreheaterCodes.Add(tPreheaterCode);
        _tPreheaterCodesManual.Add(tPreheaterCode);

        if (product.package_info.packing_quantity == 1) //如果是一箱只有一个
        {
            var totalNet = _tPreheaterCodes.Sum(s => (s.NetWeight));
            var totalGross = _tPreheaterCodes.Sum(s => (s.GrossWeight));
            var w = (int)(totalNet * 100);
            lbBoxCode.Text =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{_totalBoxNum:D4}-{w:D5}";

            var tBox = new T_box
            {
                CreateTime = DateTime.Now,
                IsDel = 0,
                LabelId = 1, //标签Id
                LabelName = "",
                PackagingCode = GlobalVar.CurrentUserInfo.packageGroupCode,
                PackagingSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{_totalBoxNum:D4}",
                PackagingWorker = GlobalVar.CurrentUserInfo.username,
                PackingBarCode = lbBoxCode.Text,
                PackingQty = product.package_info.packing_quantity.ToString(),
                PackingWeight = _currentNetWeight,
                PackingGrossWeight = _currentTotalWeight,
                TrayBarcode = txtScanCode.Text,
                UpdateTime = DateTime.Now
            };
            var boxId = await _freeSql.Insert<T_box>(tBox).ExecuteIdentityAsync();
            _boxIdList.Add((int)boxId);
            tBox.Id = (uint)boxId;
            _tBoxes.Add(tBox);
            _tBoxesManual.Add(tBox);
            var preId = await _freeSql.Insert<T_preheater_code>(tPreheaterCode).ExecuteIdentityAsync();
            var rel = new T_box_releated_preheater
            {
                BoxCodeId = (int)boxId,
                CreateTime = DateTime.Now,
                IsDel = 0,
                PreheaterCodeId = (int)preId,
                UpdateTime = DateTime.Now
            };
            if (await _freeSql.Insert<T_box_releated_preheater>(rel).ExecuteAffrowsAsync() <= 0)
            {
                ShowErrorMsg("插入关系表失败！");
                return;
            }

            ClearData();
        }
        else //一箱多个
        {
            for (var i = 1; i < _tPreheaterCodes.Count; i++)
            {
                if (_tPreheaterCodes[0].Equals(_tPreheaterCodes[i])) continue;
                ShowErrorMsg("同一箱中盘码不同");
                return;
            }

            var totalNet = _tPreheaterCodes.Sum(s => (s.NetWeight));
            var totalGross = _tPreheaterCodes.Sum(s => (s.GrossWeight));
            var w = (int)(totalNet * 100);
            lbBoxCode.Text =
                @$"{product.material_mnemonic_code}-{product.package_info.code}-{product.jsbz_number}-{GlobalVar.CurrentUserInfo.packageGroupCode}-B{DateTime.Now:MMdd}{_totalBoxNum:D4}-{w:D5}";

            if (_tPreheaterCodes.Count == product.package_info.packing_quantity)
            {
                _tPreheaterCodes = await _freeSql.Insert(_tPreheaterCodes).ExecuteInsertedAsync();
                var tBox = new T_box
                {
                    CreateTime = DateTime.Now,
                    IsDel = 0,
                    LabelId = 1, //标签Id
                    LabelName = "",
                    PackagingCode = GlobalVar.CurrentUserInfo.packageGroupCode,
                    PackagingSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{_totalBoxNum:D4}",
                    PackagingWorker = GlobalVar.CurrentUserInfo.username,
                    PackingBarCode = lbBoxCode.Text,
                    PackingQty = product.package_info.packing_quantity.ToString(),
                    PackingWeight = totalNet,
                    PackingGrossWeight = totalGross,
                    TrayBarcode = txtScanCode.Text,
                    UpdateTime = DateTime.Now
                };
                var boxId = await _freeSql.Insert<T_box>(tBox).ExecuteIdentityAsync();
                _boxIdList.Add((int)boxId);
                tBox.Id = (uint)boxId;
                _tBoxes.Add(tBox);
                _tBoxesManual.Add(tBox);
                var relList = _tPreheaterCodes.Select(s => new T_box_releated_preheater
                {
                    BoxCodeId = (int)boxId,
                    CreateTime = DateTime.Now,
                    IsDel = 0,
                    PreheaterCodeId = (int)s.Id,
                    UpdateTime = DateTime.Now
                }).ToList();
                if (await _freeSql.Insert<T_box_releated_preheater>(relList).ExecuteAffrowsAsync() <= 0)
                {
                    ShowErrorMsg("插入关系表失败！");
                    return;
                }

                ClearData();
                _totalBoxNum++;
            }
        }

        #endregion

        #region 更新表格

        UpdateGridControl(_boxIdList);

        #endregion

        #region 更新码垛信息

        lb_putStyle.Text = @$"放置方式：竖方式";
        lb_layers.Text = @$"码垛层数：{product.package_info.stacking_layers}";
        lb_numsPerLayer.Text = @$"每层轴数：{product.package_info.stacking_per_layer}";

        var layers = product.package_info.stacking_layers;
        layers = layers <= 0 ? 1 : layers;
        var perNum = product.package_info.stacking_per_layer;
        perNum = perNum <= 0 ? 2 : perNum;
        lb_currentInfo.Text = @$"已码{Math.Ceiling(_preheaterNum * 1f / perNum):F0}层,共{_preheaterNum}个";
        lb_leftNum.Text = @$"剩余个数：{layers * perNum - _preheaterNum}";

        if (_preheaterNum == layers * perNum)
        {
            XtraMessageBox.Show("托盘已满，请移走.", "Info:", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //清空内容
            _preheaterNum = 0;
            ClearData();
        }

        #endregion

        #region 标签打印和预览

        var _currentLabel = _freeSql.Select<T_label>().Where(s => (bool)s.IsCurrent).First();
        if (_currentLabel == null)
        {
            ShowErrorMsg("没有可用的标签！");
            return;
        }

        var labelTs = await _freeSql.Select<T_label_template>()
            .Where(s => s.LabelId == _currentLabel.Id).ToListAsync();
        _printTemplatePCode = labelTs.First(s => s.PrintLabelType == 0);

        if (_printTemplatePCode == null)
        {
            ShowErrorMsg("没有可用的盘标签！");
            return;
        }

        _printTemplateBoxCode = _freeSql.Select<T_label_template>()
            .Where(s => s.LabelId == _currentLabel.Id && s.PrintLabelType == 1).First();

        if (_printTemplateBoxCode == null)
        {
            ShowErrorMsg("没有可用的箱标签！");
            return;
        }

        var reportP = new XtraReport();
        var reportBox = new XtraReport();
        reportP.LoadLayout(_printTemplatePCode.TemplateFileName);
        reportBox.LoadLayout(_printTemplateBoxCode.TemplateFileName);

        reportP.DataSource = new List<Certificate>()
        {
            new Certificate
            {
                Title = null,
                MaterialNo = null,
                Model = null,
                Specifications = null,
                GrossWeight = null,
                NetWeight = null,
                BatchNum = null,
                No = null,
                DateTime = null
            }
        };

        reportBox.DataSource = new List<PackingList>()
        {
            new PackingList
            {
                MaterialNo = null,
                Model = null,
                Specifications = null,
                NetWeight = null,
                BatchNum = null,
                No = null,
                Standard = null,
                ProductNo = null,
                DateTime = null
            }
        };

        reportP.ExportToImage("xp.png");
        reportBox.ExportToImage("box.png");
        picCertificate.Image = new Bitmap("xp.png");
        picBoxList.Image = new Bitmap("box.png");

        // reportP.Print();
        // reportBox.Print();

        #endregion
    }

    private void UpdateGridControl(List<int> boxIds)
    {
        gridViewXp.FocusedRowChanged -= gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged -= gridViewBox_FocusedRowChanged;

        if (boxIds.Count == 0)
        {
            gridControlBox.DataSource = null;
            gridControlBoxChild.DataSource = null;
            return;
        }

        //1 更新箱码
        var tBoxes = _freeSql.Select<T_box>().Where(s => boxIds.Contains((int)s.Id)).ToList();
        gridControlBox.DataSource = null;
        gridControlBox.DataSource = tBoxes;
        gridViewBox.FocusedRowHandle = boxIds.Count - 1;

        //2 更新盘码
        var boxId = boxIds[^1];
        var rel = _freeSql.Select<T_box_releated_preheater>().Where(s => s.BoxCodeId == boxId).ToList();
        var preIdList = rel.Select(s => s.PreheaterCodeId).ToList();
        var tPreList = _freeSql.Select<T_preheater_code>().Where(s => preIdList.Contains((int)s.Id)).ToList();
        gridControlBoxChild.DataSource = null;
        gridControlBoxChild.DataSource = tPreList;

        gridViewBox.Columns.ForEach(s => s.BestFit());
        gridViewXp.Columns.ForEach(s => s.BestFit());

        gridViewXp.FocusedRowChanged += gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged += gridViewBox_FocusedRowChanged;
    }

    private void ClearData()
    {
        _tBoxes.Clear();
        _tPreheaterCodes.Clear();
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
        var form = new ReportTemplate(_logger);
        form.ShowDialog();
    }

    private void Exit(object sender, EventArgs e)
    {
        Application.Exit();
    }


    /// <summary>
    ///     装箱不满时仍保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Save(object sender, EventArgs e)
    {
        if (_tPreheaterCodes.Count == 0)
        {
            ShowInfoMsg("没有需要手动保存的数据！");
            return;
        }

        _tPreheaterCodes = await _freeSql.Insert(_tPreheaterCodes).ExecuteInsertedAsync();
        var tBox = new T_box
        {
            CreateTime = DateTime.Now,
            IsDel = 0,
            LabelId = 1, //标签Id
            LabelName = "",
            PackagingCode = GlobalVar.CurrentUserInfo.packageGroupCode,
            PackagingSN = $"{GlobalVar.CurrentUserInfo.packageGroupCode}{_totalBoxNum:D4}",
            PackagingWorker = GlobalVar.CurrentUserInfo.username,
            PackingBarCode = lbBoxCode.Text,
            PackingQty = _tPreheaterCodes.Count.ToString(),
            PackingWeight = _tPreheaterCodes.Sum(s => s.NetWeight),
            PackingGrossWeight = _tPreheaterCodes.Sum(s => s.GrossWeight),
            TrayBarcode = txtScanCode.Text,
            UpdateTime = DateTime.Now
        };
        var boxId = await _freeSql.Insert<T_box>(tBox).ExecuteIdentityAsync();
        _boxIdList.Add((int)boxId);
        tBox.Id = (uint)boxId;
        _tBoxes.Add(tBox);
        _tBoxesManual.Add(tBox);
        var relList = _tPreheaterCodes.Select(s => new T_box_releated_preheater
        {
            BoxCodeId = (int)boxId,
            CreateTime = DateTime.Now,
            IsDel = 0,
            PreheaterCodeId = (int)s.Id,
            UpdateTime = DateTime.Now
        }).ToList();
        if (await _freeSql.Insert<T_box_releated_preheater>(relList).ExecuteAffrowsAsync() <= 0)
        {
            ShowErrorMsg("插入关系表失败！");
            return;
        }

        ClearData();
        _totalBoxNum++;

        ShowInfoMsg("保存成功！");
        //清空内容
        _preheaterNum = 0;
        ClearData();
    }

    private async void Delete(object sender, EventArgs e)
    {
        if (_currentControl == gridControlBoxChild) //删除盘码
        {
            var rowXp = gridViewXp.FocusedRowHandle;
            if (rowXp < 0) return;
            var tPCode = gridViewXp.GetRow(rowXp) as T_preheater_code;
            if (tPCode == null) return;

            var result = XtraMessageBox.Show("是否确认删除？", "QA", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //todo:执行真实的删除操作
                gridViewXp.DeleteSelectedRows();
                var rel = await _freeSql.Select<T_box_releated_preheater>().Where(s => s.PreheaterCodeId == tPCode.Id)
                    .FirstAsync();
                var boxId = rel.BoxCodeId;
                var relBoxList = await _freeSql.Select<T_box_releated_preheater>().Where(s => s.BoxCodeId == boxId)
                    .ToListAsync();
                if (relBoxList.Count == 1)
                {
                    if (await _freeSql.Update<T_box>().Where(s => s.Id == boxId).Set(s => s.IsDel, 1)
                            .ExecuteAffrowsAsync() <= 0)
                    {
                        ShowErrorMsg($"删除箱码{boxId}失败");
                    }
                }

                if (await _freeSql.Update<T_preheater_code>().Where(s => s.Id == tPCode.Id).Set(s => s.IsDel, 1)
                        .ExecuteAffrowsAsync() <= 0)
                {
                    ShowErrorMsg($"删除盘码{boxId}失败");
                }

                if (await _freeSql.Update<T_box_releated_preheater>().Where(s => s.Id == rel.Id).Set(s => s.IsDel, 1)
                        .ExecuteAffrowsAsync() <= 0)
                {
                    ShowErrorMsg($"删除关系表{boxId}失败");
                }

                await SearchExec();
                ShowInfoMsg("删除成功！");
            }
            else if (result == DialogResult.Cancel)
            {
                ShowInfoMsg("取消删除！");
            }
        }
        else if (_currentControl == gridControlBox) //删除箱码
        {
            var rowBox = gridViewBox.FocusedRowHandle;
            if (rowBox < 0) return;
            var box = gridViewBox.GetRow(rowBox) as T_box;
            if (box == null) return;

            var result = XtraMessageBox.Show("是否确认删除？", "QA", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //todo:执行真实的删除操作
                gridViewBox.DeleteSelectedRows();
                var relList = await _freeSql.Select<T_box_releated_preheater>().Where(s => s.BoxCodeId == box.Id)
                    .ToListAsync();
                if (await _freeSql.Update<T_box_releated_preheater>().Set(s => s.IsDel, 1).SetSource(relList)
                        .ExecuteAffrowsAsync() <= 0)
                {
                    ShowErrorMsg($"删除关系表失败,{string.Join(",", relList.Select(s => s.Id).ToList())}");
                }

                var pIds = relList.Select(s => s.PreheaterCodeId).ToList();
                if (await _freeSql.Update<T_preheater_code>().Where(s => pIds.Contains((int)s.Id)).Set(s => s.IsDel, 1)
                        .ExecuteAffrowsAsync() <= 0)
                {
                    ShowErrorMsg($"删除盘码表失败,{string.Join(",", pIds)}");
                }

                if (await _freeSql.Update<T_box>().Where(s => s.Id == box.Id).Set(s => s.IsDel, 1)
                        .ExecuteAffrowsAsync() <= 0)
                {
                    ShowErrorMsg($"删除箱码表失败,{box.Id}");
                }

                await SearchExec();
                ShowInfoMsg("删除成功！");
            }
            else if (result == DialogResult.Cancel)
            {
                ShowInfoMsg("取消删除！");
            }
        }
    }

    private async void Print(object sender, EventArgs e)
    {
        //todo:打印盘码和箱码
        if (_currentControl == gridControlBoxChild) //打印盘码（合格证）
        {
            var rowXp = gridViewXp.FocusedRowHandle;
            if (rowXp < 0) return;
            var tPCode = gridViewXp.GetRow(rowXp) as T_preheater_code;
            if (tPCode == null) return;
            var result = XtraMessageBox.Show("是否确认打印？", "QA", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                //todo:打印

                // var report = new ReportPackingList();
                // report.DataSource = new List<PackingList>
                // {
                //     new()
                // };
                // report.Print();
            }
            else if (result == DialogResult.Cancel)
            {
                ShowInfoMsg("取消打印！");
            }
        }
        else if (_currentControl == gridControlBox) //打印合格证（盘码）和箱码
        {
            var rowBox = gridViewBox.FocusedRowHandle;
            if (rowBox < 0) return;
            var box = gridViewBox.GetRow(rowBox) as T_box;
            if (box == null) return;

            var result = XtraMessageBox.Show("是否确认打印？", "QA", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                var relList = await _freeSql.Select<T_box_releated_preheater>()
                    .Where(s => s.IsDel != 1 && s.BoxCodeId == box.Id).ToListAsync();
                var pIds = relList.Select(s => s.PreheaterCodeId).ToList();
                var preList = await _freeSql.Select<T_preheater_code>().Where(s => pIds.Contains((int)s.Id))
                    .ToListAsync();
                //todo:打印

                // var report = new ReportPackingList();
                // report.DataSource = new List<PackingList>
                // {
                //     new()
                // };
                // report.Print();
            }
            else if (result == DialogResult.Cancel)
            {
                ShowInfoMsg("取消打印！");
            }
        }
    }

    private async void HistorySearch(object sender, EventArgs e)
    {
        var form = new HistoryFilter();
        var result = form.ShowDialog();
        if (result == DialogResult.OK)
        {
            await SearchExec();
        }
        else
        {
            XtraMessageBox.Show("取消查询");
        }

        form.Dispose();
    }

    private async Task SearchExec()
    {
        var tPCodes = _freeSql.Select<T_preheater_code>()
            .Where(s => s.ProductDate >= DateTime.Today && s.IsDel != 1);
        var filter = GlobalVar.ReportFilters;
        if (!string.IsNullOrEmpty(filter.PreheaterCode))
        {
            tPCodes = tPCodes.Where(s => s.PreheaterCode.Contains(filter.PreheaterCode));
        }

        if (!string.IsNullOrEmpty(filter.PreheaterSpec))
        {
            tPCodes = tPCodes.Where(s => s.PreheaterSpec.Contains(filter.PreheaterSpec));
        }

        if (!string.IsNullOrEmpty(filter.ProductCode))
        {
            tPCodes = tPCodes.Where(s => s.ProductCode.Contains(filter.ProductCode));
        }

        if (!string.IsNullOrEmpty(filter.ProductSpec))
        {
            tPCodes = tPCodes.Where(s => s.ProductSpec.Contains(filter.ProductSpec));
        }

        if (!string.IsNullOrEmpty(filter.ProductBatchNo))
        {
            tPCodes = tPCodes.Where(s => s.BatchNO.Contains(filter.ProductBatchNo));
        }

        if (!string.IsNullOrEmpty(filter.UserStandardCode))
        {
            tPCodes = tPCodes.Where(s => s.UserStandardCode.Contains(filter.UserStandardCode));
        }

        if (filter.NetWeightMin > 0)
        {
            tPCodes = tPCodes.Where(s => s.NetWeight > filter.NetWeightMin);
        }

        if (filter.NetWeightMax > 0)
        {
            tPCodes = tPCodes.Where(s => s.NetWeight < filter.NetWeightMax);
        }

        var tPCodesData = await tPCodes.ToListAsync();
        var pIds = tPCodesData.Select(s => (int)s.Id).ToList();
        var relLists = await _freeSql.Select<T_box_releated_preheater>()
            .Where(s => pIds.Contains(s.PreheaterCodeId)).ToListAsync();
        var boxIds = relLists.Select(s => s.BoxCodeId).Distinct().ToList();
        UpdateGridControl(boxIds);
    }

    private async void gridViewXp_FocusedRowChanged(object sender,
        DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        gridViewXp.FocusedRowChanged -= gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged -= gridViewBox_FocusedRowChanged;

        if (gridControlBoxChild.DataSource == null) return;
        if (e.FocusedRowHandle < 0) return;
        var preheaterModes = gridControlBoxChild.DataSource as List<T_preheater_code>;
        if (preheaterModes == null) return;
        var preheaterMode = preheaterModes[e.FocusedRowHandle];

        var boxInfos = gridControlBox.DataSource as List<T_box>;
        if (boxInfos == null) return;
        var rel = await _freeSql.Select<T_box_releated_preheater>().Where(s => s.PreheaterCodeId == preheaterMode.Id)
            .FirstAsync();
        var boxId = rel.BoxCodeId;
        var boxRow = boxInfos.FindIndex(s => s.Id == boxId);
        if (boxRow == -1) return;
        gridViewBox.FocusedRowHandle = boxRow;

        gridViewXp.FocusedRowChanged += gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged += gridViewBox_FocusedRowChanged;
    }

    private async void gridViewBox_FocusedRowChanged(object sender,
        DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
        gridViewXp.FocusedRowChanged -= gridViewXp_FocusedRowChanged;
        gridViewBox.FocusedRowChanged -= gridViewBox_FocusedRowChanged;

        if (gridControlBox.DataSource == null) return;
        if (e.FocusedRowHandle < 0) return;
        var boxInfos = gridControlBox.DataSource as List<T_box>;
        if (boxInfos == null) return;
        var boxInfo = boxInfos[e.FocusedRowHandle];
        var rel = await _freeSql.Select<T_box_releated_preheater>().Where(s => s.BoxCodeId == boxInfo.Id)
            .ToListAsync();
        var tPIds = rel.Select(s => s.PreheaterCodeId).ToList();

        var pCodes = await _freeSql.Select<T_preheater_code>().Where(s => tPIds.Contains((int)s.Id)).ToListAsync();
        gridControlBoxChild.DataSource = null;
        gridControlBoxChild.DataSource = pCodes;

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

    private void ShowErrorMsg(string msg)
    {
        _logger?.Error(msg);
        XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowInfoMsg(string msg)
    {
        XtraMessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void lbErroInfo_DoubleClick(object sender, EventArgs e)
    {
        var list = new List<string>()
        {
            "G24040656G190008",
            "G24040540G950046",
            "G24031931G840292",
            "G24040684G190047",
            "G24040421G780038",
            "G24040721G170005",
            "G24040443G690002",
            "G24040725G690020",
            "G24040600G910008",
            "G24040733G910011"
        };
        var next = new Random().Next(0, 20);
        txtScanCode.Text = list[next % 10];
    }

    private void cbxMigration_CheckedChanged(object sender, EventArgs e)
    {
        lbNew.Visible = cbxMigration.Checked;
        lbOld.Visible = cbxMigration.Checked;
    }
}