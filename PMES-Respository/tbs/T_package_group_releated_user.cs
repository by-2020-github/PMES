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
    /// 包装要求表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_package_group_releated_user", DisableSyncStructure = true)]
    public partial class T_package_group_releated_user
    {

        /// <summary>
        /// 标签定义表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime", DbType = "datetime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 包装组ID
        /// </summary>
        [JsonProperty, Column(Name = "packageGroupId", StringLength = 20)]
        public string PackageGroupId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty, Column(Name = "remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 用户组ID
        /// </summary>
        [JsonProperty, Column(Name = "userId", StringLength = 20)]
        public string UserId { get; set; }

    }

}
