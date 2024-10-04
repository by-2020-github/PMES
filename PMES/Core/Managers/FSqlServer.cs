using FreeSql;
using Newtonsoft.Json;
using Serilog;

namespace PMES.Core.Managers;

public class FSqlServer
{
    private readonly string _connStr =
        @"Data Source=172.16.3.253;User Id=sa;Password=Aa123.321;Initial Catalog=GKStrip;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";

    private readonly ILogger _logger;

    public FSqlServer(ILogger logger, string connStr = "")
    {
        _logger = logger;
        if (!string.IsNullOrEmpty(connStr))
            _connStr = connStr;

        FSql = new FreeSqlBuilder()
            //.UseMonitorCommand(cmd => Trace.WriteLine($"Sql：{cmd.CommandText}")) //监听SQL语句,Trace在输出选项卡中查看
            //.UseMonitorCommand(cmd => _logger.Information($"Sql：{cmd.CommandText}")) //监听SQL语句,在日志中输出
            .UseConnectionString(DataType.SqlServer, _connStr, typeof(FreeSql.SqlServer.SqlServerProvider<>))
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
            .Build();
        //FSql.CodeFirst.IsSyncStructureToLower = true; //设置表名为小写
        logger?.Debug("fSql已加载!");
    }

    public IFreeSql FSql { get; }


   
}