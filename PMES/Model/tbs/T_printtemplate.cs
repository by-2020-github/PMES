using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_printtemplate", DisableSyncStructure = true)]
public class T_printtemplate
{
    /// <summary>
    ///     打印模版主键ID
    /// </summary>
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     打印模版建立时间
    /// </summary>
    [JsonProperty]
    [Column(Name = "createTime", DbType = "datetime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    ///     打印模版文件路径
    /// </summary>
    [JsonProperty]
    [Column(Name = "filePath")]
    public string FilePath { get; set; }

    /// <summary>
    ///     打印模版名称
    /// </summary>
    [JsonProperty]
    [Column(Name = "name", StringLength = 20)]
    public string Name { get; set; }

    /// <summary>
    ///     打印模版更新时间
    /// </summary>
    [JsonProperty]
    [Column(Name = "updateTime", DbType = "datetime")]
    public DateTime? UpdateTime { get; set; }
}