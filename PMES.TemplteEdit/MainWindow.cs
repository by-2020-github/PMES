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
                    var customEdit = new CustomEdit();
                    var label1 = new T_label_template();
                    customEdit.SetObj(label1);
                    if (customEdit.ShowDialog() == DialogResult.OK)
                    {
                        label1 = (T_label_template)customEdit.GetObj();
                    }
                }
            }
        }
    }
}
