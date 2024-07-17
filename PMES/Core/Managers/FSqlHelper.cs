using FreeSql;
using Newtonsoft.Json;
using Serilog;

namespace PMES.Core.Managers;

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

    /// <summary>
    ///     查询指定模块的参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleName"></param>
    /// <returns>查询结果，如果没有或参数错误返回null</returns>
    public T QueryParam<T>(string moduleName) where T : class
    {
        if (string.IsNullOrEmpty(moduleName)) return null;
        var moduleParam = FSql.Select<ModuleParam>().Where(s => s.Name == moduleName).First();
        return moduleParam == null ? null : JsonConvert.DeserializeObject<T>(moduleParam.Parameter);
    }

    /// <summary>
    ///     更新或插入指定模块的参数
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <param name="moduleName">模块名字</param>
    /// <param name="param">参数</param>
    /// <returns>是否成功</returns>
    public bool InsertOrUpdateParam<T>(string moduleName, T param) where T : class
    {
        if (string.IsNullOrEmpty(moduleName)) return false;
        if (param == null)
            //_logger.Error("{LogInfo}", LogInfo.CreateLogInfo("参数不能为空,返回空结果", false));
            return false;

        var repository = FSql.GetRepository<ModuleParam>();
        var p = FSql.Select<ModuleParam>().Where(s => s.Name == moduleName).First();

        var res = 0;
        if (p == null)
        {
            p = new ModuleParam(moduleName, JsonConvert.SerializeObject(param));
            res = FSql.Insert(p).ExecuteAffrows();
        }
        else
        {
            p.Parameter = JsonConvert.SerializeObject(param);
            res = repository.Update(p);
            //res = FSql.Update<ModuleParam>(p).ExecuteAffrows();
        }

        return res == 1;
    }

    /// <summary>
    ///     插入指定模块的参数(moduleName不是主键，所以可以重复插入，可以通过日期字段判断插入顺序)
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    /// <param name="moduleName">模块名字</param>
    /// <param name="param">参数</param>
    /// <returns>是否成功</returns>
    public bool InsertParam<T>(string moduleName, T? param) where T : class
    {
        if (string.IsNullOrEmpty(moduleName)) return false;
        if (param == null)
        {
            _logger.Error("参数不能为空,返回空结果");
            return false;
        }

        var res = FSql.Insert(new ModuleParam(moduleName, JsonConvert.SerializeObject(param)))
            .ExecuteAffrows();
        return res == 1;
    }
}