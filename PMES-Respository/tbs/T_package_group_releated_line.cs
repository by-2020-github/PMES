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
    /// 包装组关联产线表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_package_group_releated_line", DisableSyncStructure = true)]
    public partial class T_package_group_releated_line
    {

        /// <summary>
        /// 主键ID
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
        [JsonProperty, Column(Name = "package_group_id", StringLength = 20)]
        public string Package_group_id { get; set; }

        /// <summary>
        /// 产线表ID
        /// </summary>
        [JsonProperty, Column(Name = "production_line_id", StringLength = 20)]
        public string Production_line_id { get; set; }

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

    }

}
