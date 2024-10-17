using System.Windows;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Automatic_Net6.Model.users;
using PMES_Automatic_Net6.Views;
using PMES_Respository.DataStruct;
using PMES_Respository.tbs_sqlserver;

namespace PMES_Automatic_Net6.Core;

public class GlobalVar
{
    private static IFreeSql FreeSql => FreeSqlManager.FSqlMysql;

    public static string TemplatePath = "C:\\ProgramData\\PMES_Templates";

    public static UserInfo CurrentUserInfo { get; set; } = new UserInfo();

    public static string CurrentTemplate { get; set; }

    //public static ReportFilters ReportFilters { get; set; }
    public static T_label_template NewLabelTemplate { get; set; }

    public static WeighingMachine? WeighingMachine { get; set; }

    #region 交互信息

    //DB501
    public static PmesCmdUnStacking PmesCmdUnStacking { get; set; } = new PmesCmdUnStacking
    {
        DeviceId = 2,
        WorkPositionId = 241,
        ReelSpecification = 1,
        ReelNum = 1,
        UnStackSpeed = 20,
        ReelHeight = 0,
        Reserve1 = 0,
        Reserve2 = 0,
        PmesAndPlcReadWriteFlag = 0
    };

    //DB502
    public static PlcCmdUnStacking PlcCmdUnStacking1 { get; set; } = new PlcCmdUnStacking();

    //DB503                       
    public static PlcCmdUnStacking PlcCmdUnStacking2 { get; set; } = new PlcCmdUnStacking();

    //DB504                      
    public static PlcCmdUnStacking PlcCmdUnStacking3 { get; set; } = new PlcCmdUnStacking();

    //DB505                    
    public static PlcCmdUnStacking PlcCmdUnStacking4 { get; set; } = new PlcCmdUnStacking();

    //DB506                     
    public static PlcCmdUnStacking PlcCmdUnStacking5 { get; set; } = new PlcCmdUnStacking();

    //DB507                    
    public static PlcCmdUnStacking PlcCmdUnStacking6 { get; set; } = new PlcCmdUnStacking();

    public static List<PlcCmdUnStacking> PmesCmdUnStackingList { get; } = new List<PlcCmdUnStacking>()
    {
        PlcCmdUnStacking1,
        PlcCmdUnStacking2,
        PlcCmdUnStacking3,
        PlcCmdUnStacking4,
        PlcCmdUnStacking5,
        PlcCmdUnStacking6,
    };


    public static PmesDataItemList PmesDataItems = new PmesDataItemList();

    public static string IsBoxOnPos = "0";

    #endregion

    #region 码垛

    //DB540
    public static PmesStacking PmesStacking { get; set; } = new PmesStacking();

    //DB541
    public static PlcCmdStacking PlcCmdStacking1 { get; set; } = new PlcCmdStacking();
    public static PlcCmdStacking PlcCmdStacking2 { get; set; } = new PlcCmdStacking();
    public static PlcCmdStacking PlcCmdStacking3 { get; set; } = new PlcCmdStacking();
    public static PlcCmdStacking PlcCmdStacking4 { get; set; } = new PlcCmdStacking();
    public static PlcCmdStacking PlcCmdStacking5 { get; set; } = new PlcCmdStacking();

    public static PlcCmdStacking PlcCmdStacking6 { get; set; } = new PlcCmdStacking();

    //D547
    public static PlcCmdStacking PlcCmdStacking7 { get; set; } = new PlcCmdStacking();

    public static List<PlcCmdStacking> PlcCmdStackingList { get; } = new List<PlcCmdStacking>()
    {
        PlcCmdStacking1,
        PlcCmdStacking2,
        PlcCmdStacking3,
        PlcCmdStacking4,
        PlcCmdStacking5,
        PlcCmdStacking6,
        PlcCmdStacking7
    };

    //DB551 子母托上料
    public static PmesCmdTrayFeeding PmesCmdTrayFeeding { get; set; } = new PmesCmdTrayFeeding();

    //DB571
    public static PmesCmdCombinationMotherChildTray pmesCmdCombinationMotherChildTray { get; set; } = new PmesCmdCombinationMotherChildTray();

    public static PlcCmdCombinationMotherChildTray1 plcCmdCombinationMotherChildTray1 { get; set; } = new PlcCmdCombinationMotherChildTray1();
    public static PlcCmdCombinationMotherChildTray2 plcCmdCombinationMotherChildTray2 { get; set; } = new PlcCmdCombinationMotherChildTray2();

    public static PlcCmdCombinationMotherChildTray3 plcCmdCombinationMotherChildTray3 { get; set; } = new PlcCmdCombinationMotherChildTray3();
    public static PlcCmdCombinationMotherChildTray4 plcCmdCombinationMotherChildTray4 { get; set; } = new PlcCmdCombinationMotherChildTray4();


    public static PlcCmdCombinationMotherChildTray5 plcCmdCombinationMotherChildTray5 { get; set; } = new PlcCmdCombinationMotherChildTray5();
    public static PlcCmdCombinationMotherChildTray6 plcCmdCombinationMotherChildTray6 { get; set; } = new PlcCmdCombinationMotherChildTray6();


    public static PlcCmdCombinationMotherChildTray7 plcCmdCombinationMotherChildTray7 { get; set; } = new PlcCmdCombinationMotherChildTray7();
    public static PlcCmdCombinationMotherChildTray8 plcCmdCombinationMotherChildTray8 { get; set; } = new PlcCmdCombinationMotherChildTray8();
    public static System.Windows.Window MainView { get; set; }

    #endregion
}