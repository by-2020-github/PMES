using FreeSql;
using Serilog;

namespace PMES.TemplteEdit.Core.Managers{
public class FSqlHelper
{
    private readonly string _connStr =
        @"Data Source=127.0.0.1;Port=3308;User ID=root;Password=123456; Initial Catalog=avant_sicd_automatic;Charset=utf8; SslMode=none;Min pool size=1;AllowPublicKeyRetrieval=true";

    private readonly ILogger _logger;

    public FSqlHelper(ILogger logger, string connStr = "")
    {
        _logger = logger;
        if (!string.IsNullOrEmpty(connStr))
            _connStr = connStr;

        FSql = new FreeSqlBuilder()
            //.UseMonitorCommand(cmd => Trace.WriteLine($"Sql：{cmd.CommandText}")) //监听SQL语句,Trace在输出选项卡中查看
            //.UseMonitorCommand(cmd => _logger.Information($"Sql：{cmd.CommandText}")) //监听SQL语句,在日志中输出
            .UseConnectionString(DataType.SqlServer, _connStr)
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
            .Build();
        FSql.CodeFirst.IsSyncStructureToLower = true; //设置表名为小写
        logger?.Debug("fSql已加载!");
    }

    public IFreeSql FSql { get; }
}

}