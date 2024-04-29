using DevExpress.DataAccess.Wizard.Services;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using PMES.Core.Managers;
using PMES.UI.Login;
using PMES.UI.MainWindow;
using Serilog;
using System.Data;

namespace PMES;

internal static class Program
{
    private static ILogger _logger;
    private static IFreeSql _freeSql;

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        //ǰ��ʼ���¼�������ȫ��δ�����쳣
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += NUiExceptionHandler; //�����UI�߳��쳣
        Application.ThreadException +=
            new System.Threading.ThreadExceptionEventHandler(UiExceptionHandler); //����UI�߳��쳣

        //1 ��ʼ����־��¼����֤ץȡ���е�sql���
        SerilogManager.InitDefaultLogger();
        _logger = SerilogManager.GetOrCreateLogger();
        //2 ��ʼ��freeSqlHelper����������
        FreeSqlManager.DbLogger = _logger;
        _freeSql = FreeSqlManager.FSql;

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
        Application.Run(new LoginForm());
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