using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_label", DisableSyncStructure = true)]
public class T_label
{
    /// <summary>
    ///     标签定义表主键ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     标签建立时间
    /// </summary>
    [JsonProperty]
    [Column(Name = "createTime", DbType = "datetime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    ///     文件地址
    /// </summary>
    [JsonProperty]
    [Column(Name = "filePath")]
    public string FilePath { get; set; }

    /// <summary>
    ///     标签名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "name", StringLength = 20)]
    public string Name { get; set; }

    /// <summary>
    ///     标签更新时间
    /// </summary>
    [JsonProperty]
    [Column(Name = "updateTime", DbType = "datetime")]
    public DateTime? UpdateTime { get; set; }
}