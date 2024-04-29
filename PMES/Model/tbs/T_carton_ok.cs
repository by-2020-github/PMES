using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

/// <summary>
///     箱码表
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_carton_ok", DisableSyncStructure = true)]
public class T_carton_ok
{
    /// <summary>
    ///     箱码表主键ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     包装日期;取写入至数据库的日期和时间，精确到秒
    /// </summary>
    [JsonProperty]
    [Column(Name = "createDate", DbType = "datetime")]
    public DateTime? CreateDate { get; set; }

    /// <summary>
    ///     包装箱编号
    /// </summary>
    [JsonProperty]
    [Column(StringLength = 50)]
    public string FPackagingSN { get; set; }

    /// <summary>
    ///     标签ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "label_id", DbType = "int")]
    public int? Label_id { get; set; }

    /// <summary>
    ///     包装组编号;原FComputerName，几号包装线或人工包装线
    /// </summary>
    [JsonProperty]
    [Column(Name = "packagingCode", StringLength = 50)]
    public string PackagingCode { get; set; }

    /// <summary>
    ///     包装组名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "packagingWorker", StringLength = 50)]
    public string PackagingWorker { get; set; }

    /// <summary>
    ///     包装条码；产品助记码+线盘分组代码+用户标准代码+包装组编号+年月+4位流水号+装箱净重，如TY4121050-14-BZ001-B12310001-04903
    /// </summary>
    [JsonProperty]
    [Column(Name = "packingBarCode", StringLength = 50)]
    public string PackingBarCode { get; set; }

    /// <summary>
    ///     装箱件数
    /// </summary>
    [JsonProperty]
    [Column(Name = "packingQty", StringLength = 50)]
    public string PackingQty { get; set; }

    /// <summary>
    ///     装箱净重
    /// </summary>
    [JsonProperty]
    [Column(Name = "packingWeight", DbType = "decimal(18,2)")]
    public decimal? PackingWeight { get; set; }

    /// <summary>
    ///     母托盘条码
    /// </summary>
    [JsonProperty]
    [Column(Name = "trayBarcode", StringLength = 50)]
    public string TrayBarcode { get; set; }
}