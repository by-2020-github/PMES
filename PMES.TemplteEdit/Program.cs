using System;
using System.IO;
using System.Windows.Forms;
using PMES.TemplteEdit.Core.Managers;
using PMES.UC.reports;
using PMES_Respository.reportModel;
using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES.TemplteEdit
{
    internal static class Program
    {
        private static IFreeSql _freeSqlServer;
        private static ILogger _logger;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _logger = SerilogManager.GetOrCreateLogger();
            InitDev();
            InitTemplate();
            InitDb();
            Application.Run(new MainWindow());
        }

        static void InitDev()
        {
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(BoxReportModel));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(XianPanReportModel));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedAssembly(typeof(BoxReportModel).Assembly);
            DevExpress.XtraReports.Configuration.Settings.Default.StorageOptions.RootDirectory =
                "C:\\ProgramData\\PMES_Templates";
        }

        static void InitDb()
        {
            FreeSqlManager.DbLogger = _logger;
            _freeSqlServer = FreeSqlManager.FSqlServer;
            var count = _freeSqlServer.Select<T_preheater_code>().Count();
            if (count > 22_0000 && count < 22_6000)
            {
                if (MessageBox.Show("软件未授权，请购买license！点击OK退出，点击Cancel忽略并继续运行，将不定时锁机！", "未授权", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            if (count > 22_6000)
            {
                MessageBox.Show("软件未授权,已锁机，请购买license！点击OK退出！", "未授权", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }


        }

        static void InitTemplate()
        {
            var path = "C:\\ProgramData\\PMES_Templates";
            var labels = Directory.GetFiles(path, "*.repx");
            if (labels.Length == 0)
            {
                var boxReportAuto = new BoxReportAuto();
                boxReportAuto.SaveLayout($"C:\\ProgramData\\PMES_Templates\\auto-edge.repx");
                var top = new BoxReportTop();
                top.SaveLayout($"C:\\ProgramData\\PMES_Templates\\auto-top.repx");
                var reel = new ReelReportAuto();
                reel.SaveLayout($"C:\\ProgramData\\PMES_Templates\\auto-edge.repx");
                var manual = new MaunalDefaultReport();
                manual.SaveLayout($"C:\\ProgramData\\PMES_Templates\\auto-manual.repx");
            }


        }
    }
}