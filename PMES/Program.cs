using DevExpress.DataAccess.Wizard.Services;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using PMES.Core.Managers;
using PMES.UI.Login;
using PMES.UI.MainWindow;
using Serilog;
using System.Data;
using System.IO;
using PMES.Model;
using RichTextBox = System.Windows.Controls.RichTextBox;
namespace PMES;

internal static class Program
{
    private static ILogger _logger;
    private static IFreeSql _freeSql;

    /// <summary>
    ///     全局日志输出窗口
    /// </summary>
    public static System.Windows.Controls.RichTextBox LogViewTextBox = new RichTextBox() { VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto }; //日志输出窗口

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        if (!Directory.Exists("C:\\ProgramData\\PMES_Templates"))
        {
            Directory.CreateDirectory("C:\\ProgramData\\PMES_Templates");
        }

        DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(PackingList));
        DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Certificate));
        DevExpress.XtraReports.Configuration.Settings.Default.StorageOptions.RootDirectory = "C:\\ProgramData\\PMES_Templates";
        //前初始化事件，处理全局未处理异常
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += NUiExceptionHandler; //处理非UI线程异常
        Application.ThreadException +=
            new System.Threading.ThreadExceptionEventHandler(UiExceptionHandler); //处理UI线程异常

        //1 初始化日志记录，保证抓取所有的sql语句
        SerilogManager.InitDefaultLogger();
        _logger = SerilogManager.GetOrCreateLogger();
        //2 初始化freeSqlHelper并加载数据
        FreeSqlManager.DbLogger = _logger;
        _freeSql = FreeSqlManager.FSql;
        FreeSqlManager.SyncDbStructure();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        //Application.Run(new MainForm());
        Application.Run(new LoginForm());
    }

    private static void NUiExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var msg = (e.ExceptionObject as Exception)?.StackTrace;
        _logger.Error($"后台线程未处理异常\n\t{msg}");
        XtraMessageBox.Show($"后台线程未处理异常\n\t{msg}");
    }

    private static void UiExceptionHandler(object sender, ThreadExceptionEventArgs e)
    {
        var msg = e.Exception.StackTrace;
        _logger.Error($"Ui线程未处理异常\n\t{msg}");
        XtraMessageBox.Show($"Ui线程未处理异常\n\t{msg}");
    }
}