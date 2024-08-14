using System.Windows.Controls;
using System.Windows.Media;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBox.Themes;

namespace PMES_Automatic_Net6.Core.Managers;

public class SerilogManager
{
    /// <summary>
    ///     全局日志输出窗口
    /// </summary>
    public static System.Windows.Controls.RichTextBox LogViewTextBox = new RichTextBox()
    {
        VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto, FontSize = 12,
        Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0))
    }; //日志输出窗口

    private static readonly string Template1 =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{ThreadId}] {Message:lj}{NewLine}{Exception}";

    private static readonly string Template2 =
        "系统日志：{Timestamp:yyyyMMdd HH:mm:ss}  [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    public static ILogger DataLogger;
    private static readonly Dictionary<string, ILogger> LoggersDic = new();


    private SerilogManager()
    {
    }

    //private static readonly string Template2 = " [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    public static SerilogManager Instance => new();

    public static void InitDefaultLogger()
    {
        var fileName = $"log-{DateTime.Now.Hour,2}-{DateTime.Now.Minute,2}-";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.With(new ThreadIdEnricher())
            //.WriteTo.MySQL(FreeSqlManager.ConnStr, nameof(LogInfo))
            //.WriteTo.RichTextBox(Program.LogViewTextBox, LogEventLevel.Warning, Template2, theme: RichTextBoxConsoleTheme.Colored)
            .WriteTo.Console(outputTemplate: Template1)
            .WriteTo.File($"log\\{fileName}.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true,
                outputTemplate: Template1)
            .CreateLogger();
        LoggersDic["default"] = Log.Logger;
    }

    public static ILogger GetOrCreateLogger(string loggerName = "default")
    {
        if (LoggersDic.TryGetValue(loggerName, out var logger)) return logger;

        var fileName = $"log-{DateTime.Now.Hour,2}-{DateTime.Now.Minute,2}-";
        logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.With(new ThreadIdEnricher())
            //.WriteTo.MySQL(FreeSqlManager.ConnStr, nameof(LogInfo), LogEventLevel.Error)
            .WriteTo.RichTextBox(LogViewTextBox, LogEventLevel.Information, outputTemplate: Template2,
                theme: RichTextBoxConsoleTheme.Colored)
            .WriteTo.Console(outputTemplate: Template1)
            .WriteTo.File($"log\\{loggerName}\\{fileName}.txt", LogEventLevel.Verbose,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true, outputTemplate: Template2)
            .CreateLogger();
        LoggersDic[loggerName] = logger;
        return logger;
    }
}

public class ThreadIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "ThreadId", Thread.CurrentThread.ManagedThreadId.ToString("D4")));
    }
}