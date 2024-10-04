

// ReSharper disable InconsistentNaming
namespace PMES_Automatic_Net6.Model.users
{
    public class UserInfo
    {
        public string code { get; set; } = "TZQZ";
        public int isDel { get; set; } = 1;
        public int isLoginAutoLine { get; set; } = 1;

        /// <summary>
        ///     包装组编号
        /// </summary>
        public string packageGroupCode { get; set; } = "B2";

        public string packageGroupName { get; set; } = "A1";
        public string productionLineNum { get; set; } = "1号自动线";
        public int userId { get; set; } = 1;
        public string username { get; set; } = "aaa";
    }
}