﻿using PMES.Core.Managers;
using PMES.Model;
using PMES.Model.report;
using PMES.Model.users;
using PMES_Respository.tbs;

namespace PMES.Core;

public class GlobalVar
{
    private static IFreeSql FreeSql => FreeSqlManager.FSql;

    public static string TemplatePath = "C:\\ProgramData\\PMES_Templates";

    public static UserInfo CurrentUserInfo { get; set; } = new UserInfo();

    public static string CurrentTemplate { get; set; }

    public static ReportFilters ReportFilters { get; set; }
    public static T_label_template NewLabelTemplate { get; set; }

    public static WeighingMachine? WeighingMachine { get; set; }
}

