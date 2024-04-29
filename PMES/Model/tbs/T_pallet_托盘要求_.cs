using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_pallet(托盘要求）", DisableSyncStructure = true)]
public class T_pallet_托盘要求_
{
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     码垛层数，默认1，多个则为多层码垛
    /// </summary>
    [JsonProperty]
    [Column(Name = "cellNum", DbType = "int")]
    public int? CellNum { get; set; }

    /// <summary>
    ///     木托图纸名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "name", StringLength = 20)]
    public string Name { get; set; }

    /// <summary>
    ///     单层个数
    /// </summary>
    [JsonProperty]
    [Column(Name = "numUnitOfCell", DbType = "int")]
    public int? NumUnitOfCell { get; set; }

    /// <summary>
    ///     上货架实物图片
    /// </summary>
    [JsonProperty]
    [Column(Name = "realDrawings")]
    public string RealDrawings { get; set; }

    /// <summary>
    ///     母托图纸
    /// </summary>
    [JsonProperty]
    [Column(Name = "woodenPalletDrawings")]
    public string WoodenPalletDrawings { get; set; }

    /// <summary>
    ///     线盘类型
    /// </summary>
    [JsonProperty]
    [Column(Name = "xp", StringLength = 10)]
    public string Xp { get; set; }
}