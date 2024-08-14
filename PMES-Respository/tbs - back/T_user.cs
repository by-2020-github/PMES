using FreeSql.DatabaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs
{

    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_user" )]
    public partial class T_user
    {

        /// <summary>
        /// 员工id
        /// </summary>
        [JsonProperty, Column(Name = "id" , IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 操作工编号
        /// </summary>
        [JsonProperty, Column(Name = "code" )]
        public string Code { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime" )]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否删除 1是0否
        /// </summary>
        [JsonProperty, Column(Name = "isDel" )]
        public int? IsDel { get; set; } = 1;

        /// <summary>
        /// 是否运行登录自动线：1-可；2-不可
        /// </summary>
        [JsonProperty, Column(Name = "isLoginAutoLine" )]
        public int? IsLoginAutoLine { get; set; }

        /// <summary>
        /// 操作工登录账号
        /// </summary>
        [JsonProperty, Column(Name = "loginUsername", StringLength = 11)]
        public string LoginUsername { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty, Column(Name = "password" )]
        public string Password { get; set; }

        /// <summary>
        /// 状态 1有效0无效
        /// </summary>
        [JsonProperty, Column(Name = "status" )]
        public int? Status { get; set; } = 1;

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime" )]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 操作工姓名
        /// </summary>
        [JsonProperty, Column(Name = "username", StringLength = 11)]
        public string Username { get; set; }

    }

}
