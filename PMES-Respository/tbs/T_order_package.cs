using FreeSql.DatabaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES.Model.tbs
{

    /// <summary>
    /// 包装要求表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_order_package", DisableSyncStructure = true)]
    public partial class T_order_package
    {

        /// <summary>
        /// 标签定义表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime", DbType = "datetime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 发货子托盘名称
        /// </summary>
        [JsonProperty, Column(Name = "deliverySubTrayName", StringLength = 50)]
        public string DeliverySubTrayName { get; set; }

        /// <summary>
        /// 线盘满盘重量
        /// </summary>
        [JsonProperty, Column(Name = "fullCoilWeight")]
        public string FullCoilWeight { get; set; }

        /// <summary>
        /// 最大重量
        /// </summary>
        [JsonProperty, Column(Name = "maxWeight", DbType = "int")]
        public double? MaxWeight { get; set; }

        /// <summary>
        /// 最小重量
        /// </summary>
        [JsonProperty, Column(Name = "minWeight", DbType = "int")]
        public double? MinWeight { get; set; }

        /// <summary>
        /// 包装要求代码
        /// </summary>
        [JsonProperty, Column(Name = "packagingReqCode", StringLength = 20)]
        public string PackagingReqCode { get; set; }

        /// <summary>
        /// 包装要求名称
        /// </summary>
        [JsonProperty, Column(Name = "packagingReqName")]
        public string PackagingReqName { get; set; }

        /// <summary>
        /// 装箱件数
        /// </summary>
        [JsonProperty, Column(Name = "packingQuantity", DbType = "int")]
        public int? PackingQuantity { get; set; }

        /// <summary>
        /// 纸板名称
        /// </summary>
        [JsonProperty, Column(Name = "paperboard_name", StringLength = 20)]
        public string Paperboard_name { get; set; }

        /// <summary>
        /// 纸板编码
        /// </summary>
        [JsonProperty, Column(Name = "paperboard_number", StringLength = 20)]
        public string Paperboard_number { get; set; }

        /// <summary>
        /// 纸板规格
        /// </summary>
        [JsonProperty, Column(Name = "paperboard_spec", StringLength = 20)]
        public string Paperboard_spec { get; set; }

        /// <summary>
        /// 线盘表ID
        /// </summary>
        [JsonProperty, Column(Name = "preheaterCodeId", DbType = "int")]
        public int? PreheaterCodeId { get; set; }

        /// <summary>
        /// 线盘内包装名称
        /// </summary>
        [JsonProperty, Column(Name = "preheaterInsidePackageName", StringLength = 20)]
        public string PreheaterInsidePackageName { get; set; }

        /// <summary>
        /// 线盘外包装名称
        /// </summary>
        [JsonProperty, Column(Name = "preheaterOutsidePackageName", StringLength = 20)]
        public string PreheaterOutsidePackageName { get; set; }

        /// <summary>
        /// 堆垛层数
        /// </summary>
        [JsonProperty, Column(Name = "stackingLayers", DbType = "int")]
        public int? StackingLayers { get; set; }

        /// <summary>
        /// 每层堆垛数量
        /// </summary>
        [JsonProperty, Column(Name = "stackingPerLayer", DbType = "int")]
        public int? StackingPerLayer { get; set; }

        /// <summary>
        /// 超宽子托盘: 
        /// </summary>
        [JsonProperty, Column(Name = "super_wide_sub_tray", DbType = "tinyint(1)")]
        public sbyte? Super_wide_sub_tray { get; set; }

        /// <summary>
        /// 包装皮重
        /// </summary>
        [JsonProperty, Column(Name = "tareWeight", StringLength = 20)]
        public string TareWeight { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
        public DateTime? UpdateTime { get; set; }

    }

}
