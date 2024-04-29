using FreeSql.DataAnnotations;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace PMES.Model.tbs;

[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "t_operator(操作工包装组)", DisableSyncStructure = true)]
public class T_operator_操作工包装组_
{
    /// <summary>
    ///     员工id
    /// </summary>
    [JsonProperty]
    [Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
    public uint Id { get; set; }

    /// <summary>
    ///     1-可删除；2.不可删除
    /// </summary>
    [JsonProperty]
    [Column(Name = "authory", DbType = "int")]
    public int? Authory { get; set; } = 1;

    /// <summary>
    ///     操作工编号
    /// </summary>
    [JsonProperty]
    [Column(Name = "code", StringLength = 20)]
    public string Code { get; set; }

    /// <summary>
    ///     系统建立时间
    /// </summary>
    [JsonProperty]
    [Column(Name = "createTime", DbType = "datetime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    ///     部门
    /// </summary>
    [JsonProperty]
    [Column(Name = "department", StringLength = 100)]
    public string Department { get; set; }

    /// <summary>
    ///     是否运行登录自动线：1-可；2-不可
    /// </summary>
    [JsonProperty]
    [Column(Name = "isLoginAutoLine", DbType = "int")]
    public int? IsLoginAutoLine { get; set; }

    /// <summary>
    ///     操作工姓名
    /// </summary>
    [JsonProperty]
    [Column(Name = "name", StringLength = 11)]
    public string Name { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [JsonProperty]
    [Column(Name = "password", StringLength = 20)]
    public string Password { get; set; }
}