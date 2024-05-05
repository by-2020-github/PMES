using DevExpress.Map.Kml.Model;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PMES.Core;
using System.Net.Http;
using System.Windows.Shapes;
using Serilog;

namespace PMES.UI.Report
{
    public partial class ReportTemplate : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private ILogger _logger;
        private string templatePath = "C:\\ProgramData\\PMES_Templates";
        private List<string> _templates = new List<string>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public ReportTemplate(ILogger logger)
        {
            _logger = logger;
            InitializeComponent();
            this.Closed += (sender, args) =>
            {
                _cts.Cancel();
                this.Dispose();
            };
            if (!Directory.Exists(templatePath))
            {
                Directory.CreateDirectory(templatePath);
            }
            _templates = Directory.GetFiles(templatePath, "*.repx").ToList();
            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    await Task.Delay(500);
                    var files = Directory.GetFiles(templatePath).Where(s => s.EndsWith(".repx")).ToList();
                    foreach (var file in files.Where(file => !_templates.Contains(file)))
                    {
                        Trace.WriteLine($"增加文件：{file}");
                        _templates.Add(file);
                        //var res = await PostTemplate(file);
                    }
                }
            });
        }

        //todo:post 新增标签
        private async Task<bool> PostTemplate(string fileName)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("customerCode"), "");//客户代码
            content.Add(new StringContent("gy"), "");//cookie工艺代码
            content.Add(new StringContent("meaterialCode"), "");//物料代码
            content.Add(new StringContent("name"), "");//标签名称
            content.Add(new StringContent("preheaterCode"), "");//线盘代码
            content.Add(new StringContent("servletContext.defaultSessionTrackingModes"), "");//cookie 枚举类型,可用值:COOKIE,URL,SSL
            content.Add(new StringContent("servletContext.effectiveSessionTrackingModes"), "");//cookie 枚举类型,可用值:COOKIE,URL,SSL
            content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(fileName)), "file", fileName);
            var res = await WebService.Instance.UploadReportTemplate<UploadTemplateRes>(content,
                ApiUrls.LabelUploadLabelTemplate);
            if (res == null)
            {
                XtraMessageBox.Show("上传失败！", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnBoxCode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var report = new ReportPackingList();
            reportDesigner1.OpenReport(report);
        }

        private void btnPCode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var report = new ReportCertificateXD();
            reportDesigner1.OpenReport(report);
        }
    }
}