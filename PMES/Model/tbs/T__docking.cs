using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_ docking", DisableSyncStructure = true)]
public class T__docking
{
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     停驳位名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "dockingPosition", StringLength = 11)]
    public string DockingPosition { get; set; }

    /// <summary>
    ///     停驳位编号
    /// </summary>
    [JsonProperty]
    [Column(Name = "dockingPositionCode", StringLength = 20)]
    public string DockingPositionCode { get; set; }

    /// <summary>
    ///     产线号
    /// </summary>
    [JsonProperty]
    [Column(Name = "lineNo", DbType = "int")]
    public int? LineNo { get; set; }

    /// <summary>
    ///     机台编号
    /// </summary>
    [JsonProperty]
    [Column(Name = "name", StringLength = 10)]
    public string Name { get; set; }
}