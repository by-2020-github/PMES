﻿using FreeSql.DatabaseModel;
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
    /// 箱码表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_box")]
    public partial class T_box
    {
        /// <summary>
        /// 箱码表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 包装日期;取写入至数据库的日期和时间，精确到秒
        /// </summary>
        [JsonProperty, Column(Name = "createTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否删除：0未删除；1.删除
        /// </summary>
        [JsonProperty, Column(Name = "isDel")]
        public int? IsDel { get; set; } = 0;

        /// <summary>
        /// 标签ID
        /// </summary>
        [JsonProperty, Column(Name = "labelId")]
        public int? LabelId { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [JsonProperty, Column(Name = "labelName")]
        public string LabelName { get; set; }

        /// <summary>
        /// 打印标签模板Id
        /// </summary>
        [JsonProperty, Column(Name = "labelTemplateId")]
        public int? LabelTemplateId { get; set; }

        /// <summary>
        /// 包装组编号：PMES系统自定义---几号包装线或人工包装线
        /// </summary>
        [JsonProperty, Column(Name = "packagingCode")]
        public string PackagingCode { get; set; }

        /// <summary>
        /// 包装箱编号：PMES系统自定义：包装组编号+四位流水线号
        /// </summary>
        [JsonProperty, Column(Name = "packagingSN")]
        public string PackagingSN { get; set; }

        /// <summary>
        /// 包装组名称，PMES系统自定义
        /// </summary>
        [JsonProperty, Column(Name = "packagingWorker")]
        public string PackagingWorker { get; set; }

        /// <summary>
        /// 包装条码；产品助记码+线盘分组代码+用户标准代码+包装组编号+年月+4位流水号+装箱净重，如TY4121050-14-BZ001-B12310001-04903
        /// </summary>
        [JsonProperty, Column(Name = "packingBarCode")]
        public string PackingBarCode { get; set; }

        /// <summary>
        /// 装箱总毛重
        /// </summary>
        [JsonProperty, Column(Name = "packingGrossWeight")]
        public double? PackingGrossWeight { get; set; }

        /// <summary>
        /// 装箱件数
        /// </summary>
        [JsonProperty, Column(Name = "packingQty")]
        public string PackingQty { get; set; }

        /// <summary>
        /// 装箱净重
        /// </summary>
        [JsonProperty, Column(Name = "packingWeight")]
        public double? PackingWeight { get; set; }

        /// <summary>
        /// 字母合托 0未合托 1已合托
        /// </summary>
        [JsonProperty, Column(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// 母托盘条码
        /// </summary>
        [JsonProperty, Column(Name = "trayBarcode")]
        public string TrayBarcode { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime")]
        public DateTime? UpdateTime { get; set; }
    }
}