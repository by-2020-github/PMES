using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_admin", DisableSyncStructure = true)]
public class T_admin
{
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     登录用户名
    /// </summary>
    [JsonProperty]
    [Column(Name = "loginName", StringLength = 20)]
    public string LoginName { get; set; }

    /// <summary>
    ///     登录密码
    /// </summary>
    [JsonProperty]
    [Column(Name = "loginPwd", StringLength = 20)]
    public string LoginPwd { get; set; }

    /// <summary>
    ///     管理员姓名
    /// </summary>
    [JsonProperty]
    [Column(Name = "name", StringLength = 10)]
    public string Name { get; set; }

    /// <summary>
    ///     管理员手机号
    /// </summary>
    [JsonProperty]
    [Column(Name = "phone", StringLength = 18)]
    public string Phone { get; set; }
}