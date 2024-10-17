using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES_Automatic_Net6.Core.Managers;

public class FreeSqlManager
{
    public static ILogger DbLogger { get; set; }

    public static string ConnStrMySql { get; set; } =
    //    @"Data Source=139.196.120.197;Port=3306;User ID=root;Password=Qq123.456; Initial Catalog=my_test;Charset=utf8; SslMode=none;Min pool size=1";

    @"Data Source=172.16.3.253;User Id=pmes;Password=N9XRNi9S4R5Kjs7b;Initial Catalog=pmes;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";

    public static string ConnStrSqlServer { get; set; } =
      //  @"Data Source=.;User Id=sa;Password=Aa123.321;Initial Catalog=test;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";
      @"Data Source=172.16.3.253;User Id=pmes;Password=N9XRNi9S4R5Kjs7b;Initial Catalog=pmes;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";

    #region sql server

    private static readonly Lazy<FSqlServerHelper>
        HolderSqlServer = new(() => new FSqlServerHelper(DbLogger, ConnStrSqlServer));

    public static FSqlServerHelper FSqlServerHelper => HolderSqlServer.Value;
    public static IFreeSql FSqlServer => HolderSqlServer.Value.FSql;

    #endregion

    #region mysql

    private static readonly Lazy<FSqlMysqlHelper>
        HolderMysql = new(() => new FSqlMysqlHelper(DbLogger, ConnStrMySql));


    public static FSqlMysqlHelper FSqlMysqlHelper => HolderMysql.Value;

    public static IFreeSql FSqlMysql => HolderMysql.Value.FSql;

    public static void SyncDbStructure(IFreeSql freeSql)
    {
        freeSql.CodeFirst.SyncStructure<T_label>();
        freeSql.CodeFirst.SyncStructure<T_label_template>();
        freeSql.CodeFirst.SyncStructure<T_admin>();
        freeSql.CodeFirst.SyncStructure<T_box>();
        freeSql.CodeFirst.SyncStructure<T_preheater_code>();
        freeSql.CodeFirst.SyncStructure<T_order_exchange>();
        freeSql.CodeFirst.SyncStructure<T_order_package>();
    }

    #endregion


    public static void ConfigNavigate()
    {
    }
}