using PMES_Automatic_Net6.Core.Managers;
using PMES_Automatic_Net6.Model.users;
using PMES_Respository.DataStruct;
using PMES_Respository.tbs;

namespace PMES_Automatic_Net6.Core;

public class GlobalVar
{
    private static IFreeSql FreeSql => FreeSqlManager.FSql;

    public static string TemplatePath = "C:\\ProgramData\\PMES_Templates";

    public static UserInfo CurrentUserInfo { get; set; } = new UserInfo();

    public static string CurrentTemplate { get; set; }

    //public static ReportFilters ReportFilters { get; set; }
    public static T_label_template NewLabelTemplate { get; set; }

    public static WeighingMachine? WeighingMachine { get; set; }

    #region 交互信息

    //DB501
    public static PmesCmdUnStacking PmesCmdUnStacking { get; set; }

    //DB502
    public static PlcCmdUnStacking PlcCmdUnStacking1 { get; set; }

    //DB503                       
    public static PlcCmdUnStacking PlcCmdUnStacking2 { get; set; }

    //DB504                      
    public static PlcCmdUnStacking PlcCmdUnStacking3 { get; set; }

    //DB505                    
    public static PlcCmdUnStacking PlcCmdUnStacking4 { get; set; }

    //DB506                     
    public static PlcCmdUnStacking PlcCmdUnStacking5 { get; set; }

    //DB507                    
    public static PlcCmdUnStacking PlcCmdUnStacking6 { get; set; }


    public static PmesDataItemList PmesDataItems = new PmesDataItemList();

    #endregion
}