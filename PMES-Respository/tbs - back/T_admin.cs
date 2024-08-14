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

    /// <summary>
    /// 管理员信息表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_admin" )]
    public partial class T_admin
    {

        /// <summary>
        /// 管理员表-主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id" , IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 后台管理员登录账号
        /// </summary>
        [JsonProperty, Column(Name = "account", StringLength = 32)]
        public string Account { get; set; }

        /// <summary>
        /// 后台管理员添加时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime" )]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否删除 1是0否
        /// </summary>
        [JsonProperty, Column(Name = "isDel" )]
        public int? IsDel { get; set; } = 0;

        /// <summary>
        /// 手机号
        /// </summary>
        [JsonProperty, Column(Name = "phone", StringLength = 18)]
        public string Phone { get; set; }

        /// <summary>
        /// 后台管理员密码
        /// </summary>
        [JsonProperty, Column(Name = "pwd", StringLength = 32)]
        public string Pwd { get; set; }

        /// <summary>
        /// 后台管理员姓名
        /// </summary>
        [JsonProperty, Column(Name = "realName" )]
        public string RealName { get; set; }

        /// <summary>
        /// 后台管理员状态：1有效；0无效
        /// </summary>
        [JsonProperty, Column(Name = "status" )]
        public int? Status { get; set; }

        /// <summary>
        /// 后台管理员最后一次登录时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime" )]
        public DateTime? UpdateTime { get; set; }

    }

}
