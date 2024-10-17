using PMES_Respository.tbs_sqlserver;
using Serilog;

namespace PMES.Manual.Net6.Core.Managers;

public class FreeSqlManager
{
    public static ILogger DbLogger { get; set; }

    public static string ConnStrMySql { get; set; } =
        @"Data Source=172.16.3.253;User Id=pmes;Password=N9XRNi9S4R5Kjs7b;Initial Catalog=pmes;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";
    ///@"Data Source=139.196.120.197;Port=3306;User ID=root;Password=Qq123.456; Initial Catalog=my_test;Charset=utf8; SslMode=none;Min pool size=1";

    public static string ConnStrSqlServer { get; set; } =
        @"Data Source=172.16.3.253;User Id=pmes;Password=N9XRNi9S4R5Kjs7b;Initial Catalog=pmes;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";

    #region sql server

    private static readonly Lazy<FSqlServer>
        HolderSqlServer = new(() => new FSqlServer(DbLogger, ConnStrSqlServer));

    public static IFreeSql FSqlServer => HolderSqlServer.Value.FSql;

    #endregion

    #region mysql

    private static readonly Lazy<FSqlHelper>
        Holder = new(() => new FSqlHelper(DbLogger, ConnStrMySql));


    public static FSqlHelper FSqlHelper => Holder.Value;

    public static IFreeSql FSql => Holder.Value.FSql;

    public static void SyncDbStructure()
    {
        FSql.CodeFirst.SyncStructure<T_label>();
        FSql.CodeFirst.SyncStructure<T_label_template>();
        FSql.CodeFirst.SyncStructure<T_admin>();
        FSql.CodeFirst.SyncStructure<T_box>();
        FSql.CodeFirst.SyncStructure<T_preheater_code>();
        FSql.CodeFirst.SyncStructure<T_order_exchange>();
        FSql.CodeFirst.SyncStructure<T_order_package>();
    }

    #endregion


    public static void ConfigNavigate()
    {
    }
}