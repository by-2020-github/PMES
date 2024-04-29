using PMES.Core.Managers;
using PMES.Model.users;

namespace SICD_Automatic.Core;

public class GlobalVar
{
    private static IFreeSql FreeSql => FreeSqlManager.FSql;
 

    public static UserInfo CurrentUserInfo { get; set; } = new UserInfo();
}

