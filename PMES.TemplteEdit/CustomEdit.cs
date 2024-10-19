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
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using PMES_Respository.tbs_sqlserver;
using Newtonsoft.Json;

namespace PMES.TemplteEdit
{
    public partial class CustomEdit : Form
    {
        private IFreeSql _freeSql;
        private List<Package> _packages;

        private string _printerName = "";
        private string _packageName = "";

        public CustomEdit(IFreeSql freeSql)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _freeSql = freeSql;
        }

        public void LoadData()
        {
            if (_freeSql != null)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get,
                            "https://test-chengzhong-api.xiandeng.com:3443/api/package_info/");
                        var response = client.SendAsync(request).Result;
                        response.EnsureSuccessStatusCode();
                        var result = response.Content.ReadAsStringAsync().Result;
                        _packages = JsonConvert.DeserializeObject<List<Package>>(result);
                        cbxCode.Properties.Items.Clear();
                        cbxCode.Properties.Items.AddRange(_packages.Select(s => s.Code).ToList());
                    }

                    var labelTypes = _freeSql.Select<T_label_type>().ToList(a => a.LabelType).Distinct().ToList();

                    cbxType.Properties.Items.Clear();
                    cbxType.Properties.Items.AddRange(labelTypes);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public List<string> GetData()
        {
            var list = new List<string>
            {
                cbxCode.Text,
                cbxType.Text,
               _packageName,
               _printerName
            };
            return list;
        }

        private void LabelTypeChanged(object sender, EventArgs e)
        {
            if (_freeSql == null)
            {
                return;
            }

            try
            {
                var select = _freeSql.Select<T_label_type>().Where(s => s.LabelType == int.Parse(cbxType.Text)).First();
                _printerName = select.PrinterName;
                lbLabelTypeInfo.Text = $"当前打印机是:{select?.PrinterName},Remark:{select?.Remark}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PackageCodeChanged(object sender, EventArgs e)
        {
            if (_freeSql == null)
            {
                return;
            }

            try
            {
                var select = _packages.FirstOrDefault(s => s.Code == cbxCode.Text);
                _packageName = select?.Name;
                lbPakageName.Text = $"包装名称:{select?.Name}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Save(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }

    class Package
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}