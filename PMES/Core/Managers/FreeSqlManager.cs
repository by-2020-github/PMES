﻿using PMES_Respository.tbs;
using Serilog;

namespace PMES.Core.Managers;

public class FreeSqlManager
{
    public static ILogger DbLogger { get; set; }
    public static string ConnStrMySql { get; set; } = @"Data Source=139.196.120.197;Port=3306;User ID=root;Password=Qq123.456; Initial Catalog=my_test;Charset=utf8; SslMode=none;Min pool size=1";
    public static string ConnStrSqlServer { get; set; } = @"Data Source=.;User Id=sa;Password=Aa123.321;Initial Catalog=test;Encrypt=True;TrustServerCertificate=True;Pooling=true;Min Pool Size=1";

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
    }

    #endregion



    public static void ConfigNavigate()
    {
    }
}