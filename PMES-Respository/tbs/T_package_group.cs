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
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_package_group" )]
    public partial class T_package_group
    {

        /// <summary>
        /// 标签定义表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id" , IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 包装组编码
        /// </summary>
        [JsonProperty, Column(Name = "code" )]
        public string Code { get; set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime" )]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 包装组-名称
        /// </summary>
        [JsonProperty, Column(Name = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty, Column(Name = "remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 状态：0.删除；1.未删除
        /// </summary>
        [JsonProperty, Column(Name = "status" )]
        public int? Status { get; set; } = 1;

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime" )]
        public DateTime? UpdateTime { get; set; }

    }

}
