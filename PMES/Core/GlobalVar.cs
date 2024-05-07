using PMES.Core.Managers;
using PMES.Model;
using PMES.Model.report;
using PMES.Model.tbs;
using PMES.Model.users;

namespace SICD_Automatic.Core;

public class GlobalVar
{
    private static IFreeSql FreeSql => FreeSqlManager.FSql;

    public static string TemplatePath = "C:\\ProgramData\\PMES_Templates";

    public static UserInfo CurrentUserInfo { get; set; } = new UserInfo();

    public static string CurrentTemplate { get; set; }
    
    public static ReportFilters ReportFilters { get; set; }
    public static T_label_template NewLabelTemplate { get; set; }
}

