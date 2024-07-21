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
    /// 生产产线表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_production_line_number" )]
    public partial class T_production_line_number
    {

        /// <summary>
        /// 主键id
        /// </summary>
        [JsonProperty, Column(Name = "id" , IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime" )]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        [JsonProperty, Column(Name = "name" )]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty, Column(Name = "remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime" )]
        public DateTime? UpdateTime { get; set; }

    }

}
