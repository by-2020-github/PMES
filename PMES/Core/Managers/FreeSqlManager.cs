using PMES.Model.tbs;
using Serilog;

namespace PMES.Core.Managers;

public class FreeSqlManager
{
    private static readonly Lazy<FSqlHelper>
        Holder = new(() => new FSqlHelper(DbLogger, ConnStr));


    //private static string _connStr = @"Data Source=127.0.0.1;Port=3308;User ID=root;Password=Qq123.456; Initial Catalog=avant_sicd_automatic.1;Charset=utf8; SslMode=none;Min pool size=1;AllowPublicKeyRetrieval=true";

     public static string ConnStr { get; } =
         @"Data Source=139.196.120.197;Port=3306;User ID=root;Password=Qq123.456; Initial Catalog=my_test;Charset=utf8; SslMode=none;Min pool size=1";

    //public static string ConnStr { get; } =
    //    @"Data Source=8.142.72.79;Port=3306;User ID=pmes;Password=12345678; Initial Catalog=my_test;Charset=utf8; SslMode=none;Min pool size=1";

    public static ILogger DbLogger { get; set; }

    public static FSqlHelper FSqlHelper => Holder.Value;

    public static IFreeSql FSql => Holder.Value.FSql;

    public static void SyncDbStructure()
    {
        FSql.CodeFirst.SyncStructure<T_label>();
        FSql.CodeFirst.SyncStructure<T_label_template>();
    }

    public static void ConfigNavigate()
    {
    }
}