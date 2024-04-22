using PMES.Core.Managers;

namespace SICD_Automatic.Core
{
    public class GlobalVar
    {
        private static IFreeSql FreeSql => FreeSqlManager.FSql;
        public static string User { get; set; } = "admin";
    }
}