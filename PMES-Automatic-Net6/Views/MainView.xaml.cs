using System.Windows;
using PMES.Model;
using PMES.Model.tbs;
using PMES_Automatic_Net6.Core;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Automatic_Net6.ViewModels;
using PMES_Respository.DataStruct;
using PMES_Respository.reportModel;
using S7.Net;
using Newtonsoft.Json;
using Serilog;
using System.Data;
using System.IO;
using System.Net.NetworkInformation;
using PMES.UC.reports;

namespace PMES_Automatic_Net6.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainView : Window
    {
        public static Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger();
        private MainViewModel viewModel;
        private DebugViewModel _debugViewModel;
        private TopPageViewModel _topPageViewModel;
        private SlidePageViewModel _slidePageViewModel;
        private IFreeSql _freeSql;

        public MainView()
        {
            //var top = new BoxReportTop();
            //top.SetQrCode("1231231231-12312312312-12412412-12412421-12412124");
            //var ls = new List<BoxReportModel>() { new BoxReportModel() { BoxCode = "222" } };
            //top.DataSource = ls;
            //top.Print("160");
            //top.ExportToImage("d:\\a.png");
            InitializeComponent();
            ConfigDev();
            InitDb();
            InitPlc();

            viewModel = new MainViewModel();
            _debugViewModel = new DebugViewModel();
            _topPageViewModel = new TopPageViewModel();
            _slidePageViewModel = new SlidePageViewModel();
            this.DataContext = viewModel;
            this.TopPage.DataContext = _topPageViewModel;
            this.TaskPage.DataContext = _topPageViewModel;
            this.SlidePage.DataContext = _slidePageViewModel;
            this.DebugView.DataContext = _debugViewModel;
            Logger.Information("启动成功！");

            GlobalVar.MainView = this;
        }

        private static void InitPlc()
        {
            var ping = new Ping();
            var pingReply = ping.Send(PMESConfig.Default.PlcIp, 3000);
            if (pingReply.Status == IPStatus.TimedOut)
            {
                ShowError($"PLC无法连接,IP:{PMESConfig.Default.PlcIp}。跳过初始化，请检查PLC 网络是否正常。");
            }
            else if (!HardwareManager.Instance.InitPlc())
            {
                ShowError("打开PLC失败，请检查配置文件中的PLC配置，即将退出！", true);
            }
        }

        private void InitDb()
        {
            try
            {
                FreeSqlManager.DbLogger = Logger;
                FreeSqlManager.ConnStrMySql = PMESConfig.Default.ConnLocalMysql;
                var fSqlMysql = FreeSqlManager.FSqlMysql;
                _freeSql = fSqlMysql;
                if (fSqlMysql.Select<T_station_status>().Count() == 0)
                {
                    ShowError("工位位置为空，请先添加工位位置管理表[T_station_status]，退出！", true);
                }

                if (fSqlMysql.Select<T_plc_command>().Count() == 0)
                {
                    ShowWarring("[T_plc_command]当前没有指令,插入三条虚拟的指令，状态为已执行，数据库状态改为0，软件会检测到自动开始执行。");
                    fSqlMysql.Insert(new T_plc_command
                    {
                        CreateTime = DateTime.Now,
                        PlcComandContent = JsonConvert.SerializeObject(new PmesCmdUnStacking()),
                        PlcComandType = 1,
                        Status = 1,
                        UpdateTime = DateTime.Now
                    }).ExecuteAffrows();
                    fSqlMysql.Insert(new T_plc_command
                    {
                        CreateTime = DateTime.Now,
                        PlcComandContent = JsonConvert.SerializeObject(new PmesStacking()),
                        PlcComandType = 2,
                        Status = 1,
                        UpdateTime = DateTime.Now
                    }).ExecuteAffrows();
                    fSqlMysql.Insert(new T_plc_command
                    {
                        CreateTime = DateTime.Now,
                        PlcComandContent = JsonConvert.SerializeObject(new PmesCmdCombinationMotherChildTray()),
                        PlcComandType = 3,
                        Status = 1,
                        UpdateTime = DateTime.Now
                    }).ExecuteAffrows();
                }
            }
            catch (Exception e)
            {
                ShowError($"数据库初始化失败。{e.Message}", true);
            }

            Logger?.Information("数据库检查正常。");
        }

        private static void ConfigDev()
        {
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(PackingList));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Certificate));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(BoxReportModel));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(XianPanReportModel));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedAssembly(typeof(Certificate).Assembly);
            DevExpress.XtraReports.Configuration.Settings.Default.StorageOptions.RootDirectory =
                "C:\\ProgramData\\PMES_Templates";
        }

        private static void ShowError(string msg, bool exit = false)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            if (exit)
            {
                Application.Current.Shutdown();
            }
        }

        private static void ShowWarring(string msg)
        {
            Logger?.Error(msg);
            MessageBox.Show(msg, "War", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}