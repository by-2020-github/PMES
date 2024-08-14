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
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_order_package" )]
    public partial class T_order_package
    {
        /// <summary>
        /// 标签定义表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 发货子托盘名称
        /// </summary>
        [JsonProperty, Column(Name = "deliverySubTrayName" )]
        public string DeliverySubTrayName { get; set; }

        /// <summary>
        /// 线盘满盘重量
        /// </summary>
        [JsonProperty, Column(Name = "fullCoilWeight" )]
        public string FullCoilWeight { get; set; }

        /// <summary>
        /// 最大重量
        /// </summary>
        [JsonProperty, Column(Name = "maxWeight")]
        public int? MaxWeight { get; set; }

        /// <summary>
        /// 最小重量
        /// </summary>
        [JsonProperty, Column(Name = "minWeight")]
        public int? MinWeight { get; set; }

        /// <summary>
        /// 包装要求代码
        /// </summary>
        [JsonProperty, Column(Name = "packagingReqCode")]
        public string PackagingReqCode { get; set; }

        /// <summary>
        /// 包装要求名称
        /// </summary>
        [JsonProperty, Column(Name = "packagingReqName")]
        public string PackagingReqName { get; set; }

        /// <summary>
        /// 装箱件数
        /// </summary>
        [JsonProperty, Column(Name = "packingQuantity")]
        public int? PackingQuantity { get; set; }

        /// <summary>
        /// 线盘内包装名称
        /// </summary>
        [JsonProperty, Column(Name = "preheaterInsidePackageName")]
        public string PreheaterInsidePackageName { get; set; }

        /// <summary>
        /// 线盘外包装名称
        /// </summary>
        [JsonProperty, Column(Name = "preheaterOutsidePackageName")]
        public string PreheaterOutsidePackageName { get; set; }

        /// <summary>
        /// 用于关联盘码表，半成品生产条码，线盘上的二维码，用于关联 - 盘码
        /// </summary>
        [JsonProperty, Column(Name = "productionBarCode" )]
        public string ProductionBarCode { get; set; }

        /// <summary>
        /// 堆垛层数
        /// </summary>
        [JsonProperty, Column(Name = "stackingLayers" )]
        public int? StackingLayers { get; set; }

        /// <summary>
        /// 每层堆垛数量
        /// </summary>
        [JsonProperty, Column(Name = "stackingPerLayer" )]
        public int? StackingPerLayer { get; set; }

        /// <summary>
        /// 包装皮重
        /// </summary>
        [JsonProperty, Column(Name = "tareWeight" )]
        public string TareWeight { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime" )]
        public DateTime? UpdateTime { get; set; }
    }
}