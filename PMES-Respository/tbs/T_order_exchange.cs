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
    /// 改线入库表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_order_exchange", DisableSyncStructure = true)]
    public partial class T_order_exchange
    {

        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty, Column(Name = "id", DbType = "int", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime", DbType = "datetime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 改线入库之后的新条码
        /// </summary>
        [JsonProperty, Column(Name = "newCode")]
        public string NewCode { get; set; }

        /// <summary>
        /// 改线入库前的旧条码
        /// </summary>
        [JsonProperty, Column(Name = "oldCode")]
        public string OldCode { get; set; }

        /// <summary>
        /// 登录用户ID
        /// </summary>
        [JsonProperty, Column(Name = "weightUserId", DbType = "int")]
        public int? WeightUserId { get; set; }

    }

}
