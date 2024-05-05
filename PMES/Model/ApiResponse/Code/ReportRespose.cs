using PMES.Model.ApiResponse;

namespace PMES.Model.report;

public class ResponseReport : ResponseBase<HistoryData>
{
 
}

public class HistoryData
{
    public List<SingleRecord>? data { get; set; }
    public int total { get; set; }
}

/// <summary>
///     单条记录
/// </summary>
public class SingleRecord
{
    /// <summary>
    ///     一个箱码
    /// </summary>
    public BoxInfoRecord box { get; set; }
    
    /// <summary>
    ///     多个盘码
    /// </summary>
    public List<PreheaterCodeInfoRecord> preheaterCodeList { get; set; }
}

/// <summary>
///     箱信息
/// </summary>
public class BoxInfoRecord
{
    public string createTime { get; set; } = "";
    public int id { get; set; } = 0;
    public int labelId { get; set; } = 0;
    public string labelName { get; set; } = "";
    public string packagingCode { get; set; } = "";
    public string packagingSN { get; set; } = "";
    public string packagingWorker { get; set; } = "";
    public string packingBarCode { get; set; } = "";
    public int packingQty { get; set; } = 0;
    public string packingWeight { get; set; } = "";
    public string trayBarcode { get; set; } = "";
}

/// <summary>
///     线盘的信息
/// </summary>
public class PreheaterCodeInfoRecord
{
    public string batchNo { get; set; } = "";
    public string customerCode { get; set; } = "";
    public string customerId { get; set; } = "";
    public string customerMaterialCode { get; set; } = "";
    public string customerMaterialName { get; set; } = "";
    public string customerMaterialSpec { get; set; } = "";
    public string customerName { get; set; } = "";
    public string grossWeight { get; set; } = "";
    public int id { get; set; } = 0;
    public int isDel { get; set; } = 0;
    public int isQualified { get; set; } = 0;
    public string machineCode { get; set; } = "";
    public string machineId { get; set; } = "";
    public string machineName { get; set; } = "";
    public string netWeight { get; set; } = "";
    public string noQualifiedReason { get; set; } = "";
    public string operatorCode { get; set; } = "";
    public string operatorName { get; set; } = "";
    public string preheaterCode { get; set; } = "";
    public string preheaterId { get; set; } = "";
    public string preheaterName { get; set; } = "";
    public string preheaterSpec { get; set; } = "";
    public string preheaterWeight { get; set; } = "";
    public string productCode { get; set; } = "";
    public string productDate { get; set; } = "";
    public string productGBName { get; set; } = "";
    public string productId { get; set; } = "";
    public string productMnemonicCode { get; set; } = "";
    public string productName { get; set; } = "";
    public string productSpec { get; set; } = "";
    public string productStandardName { get; set; } = "";
    public string productionBarCode { get; set; } = "";
    public string productionOrgNo { get; set; } = "";
    public string psn { get; set; } = "";
    public int status { get; set; } = 0;
    public string stockCode { get; set; } = "";
    public string stockId { get; set; } = "";
    public string stockName { get; set; } = "";
    public string userStandardCode { get; set; } = "";
    public string userStandardId { get; set; } = "";
    public string userStandardName { get; set; } = "";
    public int weightUserId { get; set; } = 0;
}