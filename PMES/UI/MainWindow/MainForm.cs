using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PMES.Core;
using PMES.Model;
using MySqlX.XDevAPI;
using System.Net.Http.Headers;
using DevExpress.Map.Native;
using DevExpress.Mvvm.POCO;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using PMES.Core.Managers;
using RestSharp;
using Serilog;

namespace PMES.UI.MainWindow
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private ProductInfo _productInfo = new ProductInfo();
        private ILogger _logger;
        private WeighingMachine _weighingMachine;
        private HttpClient _httpClient;

        public MainForm()
        {
            InitializeComponent();
            SerilogManager.InitDefaultLogger();
            _logger = SerilogManager.GetOrCreateLogger();
            _weighingMachine = new WeighingMachine(_logger);
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            _httpClient = new HttpClient(handler);
            UpdateProductInfo(new ProductInfo());
        }


        /// <summary>
        ///     扫码枪触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ScanCodeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtScanCode.Text))
            {
                return;
            }

            if (txtScanCode.Text.Length < 10)
            {
                return;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://test.chengzhong-api.site.xiandeng.com:3443/api/product-info?semi_finished={txtScanCode.Text}");
                request.Headers.Add("Cookie",
                    "csrftoken=4GjfFB1WhRHfI30HeenFN6CEyYSarg0R; sl-session=l/jXE8c+KGZOFwujhtgpVg==");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ProductInfo>(res);
                if (product == null) return;
                UpdateProductInfo(product);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
            }
        }

        private double _netWeight => _totalWeight - _skinWeight - _tareWeight;
        private double _totalWeight = 52.03;
        private double _skinWeight = 2.01;
        private double _tareWeight =0;

        private void UpdateProductInfo(ProductInfo product)
        {
            lb_product_order_no.Text = lb_product_order_no.Tag + product.product_order_no;
            lb_customer_number.Text = lb_customer_number.Tag + product.customer_number;
            lb_customer_name.Text = lb_customer_name.Tag + product.customer_name;
            lb_material_ns_model.Text = lb_material_ns_model.Tag + product.material_ns_model;
            lb_customer_material_spec.Text = lb_customer_material_spec.Tag + product.xpzl_spec;
            lb_jsbz_number.Text = lb_jsbz_number.Tag + product.jsbz_number;
            lb_jsbz_name.Text = lb_jsbz_name.Tag + product.jsbz_name;


            lb_xpzl_number.Text = lb_xpzl_number.Tag + product.xpzl_number;
            lb_xpzl_name.Text = lb_xpzl_name.Tag + product.xpzl_name;


            //线盘重量 这里输入净重
            lb_xpzl_weight.AllowHtmlString = true;
            //lb_xpzl_weight.Text = lb_xpzl_weight.Tag + $" <color=red><b>20</b></color> kg"
            lb_xpzl_weight.Text = $"{lb_xpzl_weight.Tag} <color=red><b>{_netWeight.ToString("F2")}</b></color> kg";
            lb_package_info_code.Text = lb_package_info_code.Tag + product.package_info.code;
            lb_package_info_name.Text = lb_package_info_name.Tag + product.package_info.name;
            lb_material_execution_standard.Text =
                lb_material_execution_standard.Tag + product.material_execution_standard;
            lb_product_date.Text = lb_product_date.Tag + product.product_date;
            lb_machine_number.Text = lb_machine_number.Tag + product.machine_number;
            //lb_
            lb_operator_code.Text = lb_operator_code.Tag + product.operator_code;
            // lb_
            lb_operator_name.Text = lb_operator_name.Tag + product.operator_name;

            lb_fix_prod_code.Text = lb_fix_prod_code.Tag + txtScanCode.Text;
            if (!double.TryParse(lbNetWeight.Text, out double res)) return;
            if (txtScanCode.Text.Length < 10) return;
            lbBoxCode.Text =
                $"{product.material_number.Substring(3).Replace(".", "")}-{product.package_info.code}-{product.jsbz_number}-B{DateTime.Now:MMdd}{txtScanCode.Text.Substring(txtScanCode.Text.Length - 4, 4)}-{_netWeight * 100:#####}";
        }


        private void UpdateUi(ProductInfo product)
        {
            lbBoxCode.Text = "";
            lbTotalWeight.Text = ""; //毛重=总重量
            lbGrossWeight.Text = ""; //毛重
            lbSkinWeight.Text = ""; //皮重
            lbNetWeight.Text = ""; //净重
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
        }

        /// <summary>
        ///     打印机设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrinterSettings(object sender, EventArgs e)
        {
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
    }
}