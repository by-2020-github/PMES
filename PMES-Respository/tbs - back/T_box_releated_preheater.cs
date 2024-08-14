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
    /// 箱码-关联盘内编码
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_box_releated_preheater" )]
    public partial class T_box_releated_preheater
    {

        /// <summary>
        /// 箱盘关联表id
        /// </summary>
        [JsonProperty, Column(Name = "id" , IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 箱码表-主键id
        /// </summary>
        [JsonProperty, Column(Name = "boxCodeId" )]
        public int BoxCodeId { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime" )]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否删除 1是0否
        /// </summary>
        [JsonProperty, Column(Name = "isDel" )]
        public int? IsDel { get; set; } = 0;

        /// <summary>
        /// 盘码表-主键id
        /// </summary>
        [JsonProperty, Column(Name = "preheaterCodeId" )]
        public int PreheaterCodeId { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime" )]
        public DateTime? UpdateTime { get; set; }

    }

}
