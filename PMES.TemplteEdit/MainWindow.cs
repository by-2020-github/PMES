using DevExpress.XtraReports.UI;
using PMES.UC.reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.DataAccess.UI.Native.Excel;
using PMES.TemplteEdit.Core.Managers;
using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES.TemplteEdit
{
    public partial class MainWindow : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private ILogger _logger;
        private IFreeSql _fsSql;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            try
            {
                _logger = SerilogManager.GetOrCreateLogger();
                FreeSqlManager.DbLogger = _logger;
                _fsSql = FreeSqlManager.FSqlServer;
            }
            catch (Exception e)
            {
                MessageBox.Show($"初始化失败！\n{e.Message}");
            }
        }

        private void QuickLoadTemplate(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var buttonItem = e.Item as DevExpress.XtraBars.BarButtonItem;
            var report = new XtraReport();
            switch (buttonItem?.Tag.ToString())
            {
                case "1":
                    report = new ReelReportAuto();
                    break;
                case "2":
                    report = new BoxReportAuto();
                    break;
                case "3":
                    report = new BoxReportTop();
                    break;
                case "4":
                    report = new MaunalDefaultReport();
                    break;
            }

            reportDesigner1.OpenReport(report);
        }

        private void UpLoad(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = "C:\\ProgramData\\PMES_Templates",
                Multiselect = false
            };
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                if (File.Exists(fileName))
                {
                    var customEdit = new CustomEdit(_fsSql);
                    customEdit.LoadData();
                    if (customEdit.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show("退出，不保存上传！");
                        return;
                    }

                    var data = customEdit.GetData();
                    if (data.Any(string.IsNullOrEmpty))
                    {
                        MessageBox.Show($"返回结果:{string.Join(" , ", data)}有空值，无法保存，退出，不保存上传！");
                        return;
                    }

                    var report = new XtraReport();
                    report.LoadLayout(fileName);
                    var stream = new MemoryStream();
                    var picName = fileName.Replace("repx", "png");
                    report.ExportToImage(picName);

                    var label1 = new T_label_template
                    {
                        LabelType = int.Parse(data[1]),
                        PackageCode = data[0],
                        PackageName = data[2],
                        Remark = null,
                        TemplateFile = File.ReadAllBytes(fileName),
                        TemplateFileName = fileName,
                        TemplatePicture = File.ReadAllBytes(picName),
                        UpdateTime = DateTime.Now
                    };

                    var labelTemplates = _fsSql.Select<T_label_template>().ToList();
                    if (labelTemplates.Any(s => s.PackageCode == label1.PackageCode && s.LabelType == label1.LabelType))
                    {
                        var dialogResult =
                            MessageBox.Show(
                                $"已经存在包装代码为:{label1.PackageCode},标签类型为：{label1.LabelType}的标签，是否上传覆盖，点击OK覆盖，点击Cancel取消",
                                "QA", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        switch (dialogResult)
                        {
                            case DialogResult.Cancel:
                                return;
                            case DialogResult.OK:
                            {
                                var executeAffrows = _fsSql.Update<T_label_template>()
                                    .Where(s => s.PackageCode == label1.PackageCode && s.LabelType == label1.LabelType)
                                    .SetSource(label1).ExecuteAffrows();
                                MessageBox.Show(executeAffrows > 0 ? "更新成功！" : "更新失败！");
                                break;
                            }
                        }
                    }
                    else
                    {
                        var executeAffrows = _fsSql.Insert(label1).ExecuteAffrows();
                        MessageBox.Show(executeAffrows > 0 ? "插入成功！" : "插入失败！");
                    }
                }
            }
        }
    }
}