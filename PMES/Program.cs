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
using PMES.Properties;
using PMES_Respository.tbs_sqlServer;
namespace PMES;

internal static class Program
{
    private static ILogger _logger;
    private static IFreeSql _freeSql;
    private static IFreeSql _freeSqlServer;

    /// <summary>
    ///     ȫ����־�������
    /// </summary>
    public static System.Windows.Controls.RichTextBox LogViewTextBox = new RichTextBox() { VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto }; //��־�������

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
        //ǰ��ʼ���¼�������ȫ��δ�����쳣
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += NUiExceptionHandler; //�����UI�߳��쳣
        Application.ThreadException +=
            new System.Threading.ThreadExceptionEventHandler(UiExceptionHandler); //����UI�߳��쳣

        //1 ��ʼ����־��¼����֤ץȡ���е�sql���
        SerilogManager.InitDefaultLogger();
        _logger = SerilogManager.GetOrCreateLogger();
        //2 ��ʼ��freeSqlHelper����������
        InitDb();


        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
        Application.Run(new LoginForm());
    }

    private static void InitDb()
    {
        if (!string.IsNullOrEmpty(PMES_Settings.Default.ConnStrMySql))
        {
            FreeSqlManager.ConnStrMySql = PMES_Settings.Default.ConnStrMySql;
        }
        if (!string.IsNullOrEmpty(PMES_Settings.Default.SqlServerConnStr))
        {
            FreeSqlManager.ConnStrSqlServer = PMES_Settings.Default.SqlServerConnStr;
        }

        FreeSqlManager.DbLogger = _logger;
        _freeSql = FreeSqlManager.FSql;
        _freeSqlServer = FreeSqlManager.FSql;
        FreeSqlManager.SyncDbStructure();
        //var ret = _freeSqlServer.Insert(new OldTest()).ExecuteAffrows();
    }

    private static void NUiExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var msg = (e.ExceptionObject as Exception)?.StackTrace;
        _logger.Error($"��̨�߳�δ�����쳣\n\t{msg}");
        XtraMessageBox.Show($"��̨�߳�δ�����쳣\n\t{msg}");
    }

    private static void UiExceptionHandler(object sender, ThreadExceptionEventArgs e)
    {
        var msg = e.Exception.StackTrace;
        _logger.Error($"Ui�߳�δ�����쳣\n\t{msg}");
        XtraMessageBox.Show($"Ui�߳�δ�����쳣\n\t{msg}");
    }
}