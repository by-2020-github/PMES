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
using DevExpress.Mvvm.POCO;
using DevExpress.XtraReports.UI;
using Serilog;
using Path = System.IO.Path;
using PMES.Core.Managers;
using DevExpress.XtraReports.Wizards.Templates;
using DevExpress.XtraRichEdit.Import.Doc;
using PMES.UC.reports;

namespace PMES.UI.Report
{
    public partial class ReportTemplate : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly ILogger _logger;
        private readonly IFreeSql _freeSql = FreeSqlManager.FSql;
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
                    try
                    {
                        await Task.Delay(500);
                        var files = Directory.GetFiles(templatePath).Where(s => s.EndsWith(".repx")).ToList();
                        foreach (var file in files.Where(file => !_templates.Contains(file)))
                        {
                            Trace.WriteLine($"增加文件：{file}");
                            _templates.Add(file);
                            //todo:弹窗录入信息
                            XtraMessageBox.Show("请输入保存的标签信息！");
                            var addNew = new NewLabelInput();
                            if (addNew.ShowDialog() == DialogResult.OK)
                            {
                                var labelTemplate = GlobalVar.NewLabelTemplate;
                                labelTemplate.TemplateFile = await File.ReadAllBytesAsync(file);
                                var report = new XtraReport();
                                report.LoadLayout(file);
                                report.ExportToImage("tmp.jpg");
                                labelTemplate.TemplatePicture = await File.ReadAllBytesAsync("tmp.jpg");
                                labelTemplate.TemplateFileName = file;
                                var tId = _freeSql.Insert(labelTemplate).ExecuteIdentity();
                                XtraMessageBox.Show(tId <= 0 ? "插入打印标签失败！" : "保存标签成功!");
                            }
                            else
                            {
                                XtraMessageBox.Show($"没有输入必要的信息，不会生效！", "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e);
                    }
                }
            });
        }


        private void btnBoxCode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var report = new TemplateXianPan();
            reportDesigner1.OpenReport(report);
        }

        private void btnPCode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var report = new TemplateBox();
            reportDesigner1.OpenReport(report);
        }

        private void NullMessage(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("暂时未添加该标签模板！");
        }

        private void ReportTemplate_Load(object sender, EventArgs e)
        {

        }
    }
}