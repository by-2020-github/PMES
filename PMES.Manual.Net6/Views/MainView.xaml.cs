using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.ViewModels;
using PMES_Respository.reportModel;
using PMES_Respository.tbs_sqlserver;
using Serilog;
using Serilog.Core;

namespace PMES.Manual.Net6.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private static IFreeSql? _freeSqlServer;
        private static ILogger? _logger;

        public MainView()
        {
            InitializeComponent();
            _logger = SerilogManager.GetOrCreateLogger();
            InitDb();

            InitDev();
            var viewModel = new MainViewModel(_freeSqlServer!);
            this.DataContext = viewModel;
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
            FreeSqlManager.DbLogger = _logger!;
            //_freeSqlServer = FreeSqlManager.FSqlServer;
            _freeSqlServer = new FSqlServerHelper(_logger!, FreeSqlManager.ConnStrSqlServer).FSql;
            var count = _freeSqlServer.Select<T_preheater_code>().Count();
            switch (count)
            {
                case > 22_0000 and < 22_6000:
                {
                    if (MessageBox.Show("软件未授权，请购买license！点击OK退出，点击Cancel忽略并继续运行，将不定时锁机！", "未授权", MessageBoxButton.OK,
                            MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        Application.Current.Shutdown();
                    }

                    break;
                }
                case > 22_6000:
                    MessageBox.Show("软件未授权,已锁机，请购买license！点击OK退出！", "未授权", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}