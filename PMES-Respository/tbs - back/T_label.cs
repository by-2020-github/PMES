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
    /// 标签表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_label")]
    public partial class T_label
    {

        /// <summary>
        /// 标签表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id",   IsPrimary = true, IsIdentity = true)]
        public long? Id { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        public bool? IsCurrent { get; set; }

        /// <summary>
        /// 0有效；1删除
        /// </summary>
        public int? IsDel { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 装箱件数
        /// </summary>
        public string NumOfPackedItems { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }

}
