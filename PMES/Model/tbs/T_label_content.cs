using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_label_content", DisableSyncStructure = true)]
public class T_label_content
{
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     客户名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "customer", StringLength = 200)]
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    ///     包装代码
    /// </summary>
    [JsonProperty]
    [Column(Name = "labelCode", StringLength = 20)]
    public string LabelCode { get; set; } = string.Empty;

    /// <summary>
    ///     包装名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "labelName", StringLength = 40)]
    public string LabelName { get; set; } = string.Empty;

    /// <summary>
    ///     线盘类型
    /// </summary>
    [JsonProperty]
    [Column(Name = "xpType", StringLength = 40)]
    public string XpType { get; set; } = string.Empty;
}