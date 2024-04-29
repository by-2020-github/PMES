using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

/// <summary>
///     盘码表
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_preheater_ok（盘码）", DisableSyncStructure = true)]
public class T_preheater_ok_盘码_
{
    /// <summary>
    ///     盘码表主键ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     批次号:生产工单号+月日+R或Y;8:00~20:00定义为R，20:00~次日8:00定义为Y
    /// </summary>
    [JsonProperty]
    [Column(Name = "batchNO", StringLength = 100)]
    public string BatchNO { get; set; }

    /// <summary>
    ///     客户ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "custID", DbType = "int")]
    public int? CustID { get; set; }

    /// <summary>
    ///     客户物料代码
    /// </summary>
    [JsonProperty]
    [Column(Name = "custMaterialCode", StringLength = 50)]
    public string CustMaterialCode { get; set; }

    /// <summary>
    ///     客户物料名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "custMaterialName", StringLength = 100)]
    public string CustMaterialName { get; set; }

    /// <summary>
    ///     客户物料规格
    /// </summary>
    [JsonProperty]
    [Column(Name = "custMaterialSKU", StringLength = 50)]
    public string CustMaterialSKU { get; set; }

    /// <summary>
    ///     客户要求最大重量
    /// </summary>
    [JsonProperty]
    [Column(DbType = "decimal(18,2)")]
    public decimal? CustMaxWeightReq { get; set; }

    /// <summary>
    ///     客户要求最小重量
    /// </summary>
    [JsonProperty]
    [Column(DbType = "decimal(18,2)")]
    public decimal? CustMinWeightReq { get; set; }

    /// <summary>
    ///     客户代码
    /// </summary>
    [JsonProperty]
    [Column(Name = "custNo", StringLength = 50)]
    public string CustNo { get; set; }

    /// <summary>
    ///     盘码，其规则如下：产品助记码+线盘分组代码+用户标准代码+包装组编号+年月+4位流水号+装箱净重，如TY4121050-14-BZ001-B123100001-04903
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 50)]
    public string FSN { get; set; }

    /// <summary>
    ///     产品助记码
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 100)]
    public string GBName { get; set; }

    /// <summary>
    ///     产品盘毛重，需要称重
    /// </summary>
    [JsonProperty]
    [Column(DbType = "decimal(18,2)")]
    public decimal? GrossWeight { get; set; }

    /// <summary>
    ///     生产工单号
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 50)]
    public string ICMOBillNO { get; set; }

    /// <summary>
    ///     产品代码
    /// </summary>
    [JsonProperty]
    [Column(Name = "itemCode", StringLength = 50)]
    public string ItemCode { get; set; }

    /// <summary>
    ///     产品ID
    /// </summary>
    [JsonProperty]
    [Column(DbType = "int")]
    public int? ItemID { get; set; }

    /// <summary>
    ///     产品名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "itemName", StringLength = 100)]
    public string ItemName { get; set; }

    /// <summary>
    ///     产品助记码
    /// </summary>
    [JsonProperty]
    [Column(Name = "mnemonicCode", StringLength = 50)]
    public string MnemonicCode { get; set; }

    /// <summary>
    ///     产品盘净重，需公式计算：毛重-2个皮重
    /// </summary>
    [JsonProperty]
    [Column(DbType = "decimal(18,2)")]
    public decimal? NetWeight { get; set; }

    /// <summary>
    ///     操作工编号
    /// </summary>
    [JsonProperty]
    [Column(Name = "operatorCode", StringLength = 50)]
    public string OperatorCode { get; set; }

    /// <summary>
    ///     操作工姓名
    /// </summary>
    [JsonProperty]
    [Column(Name = "operatorName", StringLength = 50)]
    public string OperatorName { get; set; }

    /// <summary>
    ///     包装要求代码
    /// </summary>
    [JsonProperty]
    [Column(Name = "packagingReqCode", StringLength = 50)]
    public string PackagingReqCode { get; set; }

    /// <summary>
    ///     包装要求名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "packagingReqName")]
    public string PackagingReqName { get; set; }

    /// <summary>
    ///     生产条码, 根据此U8，从星空云获取订单信息
    /// </summary>
    [JsonProperty]
    [Column(Name = "productionBarcode", StringLength = 50)]
    public string ProductionBarcode { get; set; }

    /// <summary>
    ///     生产组织
    /// </summary>
    [JsonProperty]
    [Column(Name = "productionOrgNO", StringLength = 50)]
    public string ProductionOrgNO { get; set; }

    /// <summary>
    ///     产品执行标准
    /// </summary>
    [JsonProperty]
    [Column(Name = "productStandard", StringLength = 50)]
    public string ProductStandard { get; set; }

    /// <summary>
    ///     产品规格
    /// </summary>
    [JsonProperty]
    [Column(Name = "size", StringLength = 50)]
    public string Size { get; set; }

    /// <summary>
    ///     入库仓库代码
    /// </summary>
    [JsonProperty]
    [Column(Name = "stockCode", StringLength = 50)]
    public string StockCode { get; set; }

    /// <summary>
    ///     入库仓库ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "stockID", DbType = "int")]
    public int? StockID { get; set; }

    /// <summary>
    ///     入库仓库名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "stockName", StringLength = 50)]
    public string StockName { get; set; }

    /// <summary>
    ///     用户代码标准
    /// </summary>
    [JsonProperty]
    [Column(Name = "userStandardCode", StringLength = 50)]
    public string UserStandardCode { get; set; }

    /// <summary>
    ///     用户标准ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "userStandardID", DbType = "int")]
    public int? UserStandardID { get; set; }

    /// <summary>
    ///     用户标准名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "userStandardName", StringLength = 100)]
    public string UserStandardName { get; set; }

    /// <summary>
    ///     包装纸皮重
    /// </summary>
    [JsonProperty]
    [Column(DbType = "decimal(18,2)")]
    public decimal? WrapperWeight { get; set; }

    /// <summary>
    ///     线盘代码
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 50)]
    public string XPCode { get; set; }

    /// <summary>
    ///     线盘ID
    /// </summary>
    [JsonProperty]
    [Column(DbType = "int")]
    public int? XPID { get; set; }

    /// <summary>
    ///     线盘名称
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 100)]
    public string XPName { get; set; }

    /// <summary>
    ///     线盘规格
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 50)]
    public string XPSize { get; set; }

    /// <summary>
    ///     线盘皮重
    /// </summary>
    [JsonProperty]
    [Column(DbType = "decimal(18,2)")]
    public decimal? XPWeight { get; set; }
}