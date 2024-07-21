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
    /// 线盘
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_preheater_code")]
    public partial class T_preheater_code
    {
        /// <summary>
        /// 盘码表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        [Column(Name = "boxId")] public int BoxId { get; set; }

        /// <summary>
        /// 自动线 称重2
        /// </summary>
        [JsonProperty, Column(Name = "weight2")]
        public double Weight2 { get; set; }

        /// <summary>
        /// 批次号，规则<生产工单号+月日+R或Y;8:00~20:00定义为R>; 目前暂时的实现方式，对应pc界面上的-生产工单：批次号暂时先用生产订单号，后面要调整的，理论批次号来源于生产标签打印的时候产生-施部长回答
        /// </summary>
        [JsonProperty, Column(Name = "batchNO")]
        public string BatchNO { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        [JsonProperty, Column(Name = "customerCode")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [JsonProperty, Column(Name = "customerId")]
        public int? CustomerId { get; set; }

        /// <summary>
        /// 客户物料代码
        /// </summary>
        [JsonProperty, Column(Name = "customerMaterialCode")]
        public string CustomerMaterialCode { get; set; }

        /// <summary>
        /// 客户物料名称
        /// </summary>
        [JsonProperty, Column(Name = "customerMaterialName")]
        public string CustomerMaterialName { get; set; }

        /// <summary>
        /// 客户物料规格
        /// </summary>
        [JsonProperty, Column(Name = "customerMaterialSpec")]
        public string CustomerMaterialSpec { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [JsonProperty, Column(Name = "customerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// [填写]产品盘毛重: 1. 人工线：由电子称设备获取，人工3次称重的平均值；2.自动线，二成称重后【erp称重校验】获得。
        /// </summary>
        [JsonProperty, Column(Name = "grossWeight")]
        public double? GrossWeight { get; set; }

        /// <summary>
        /// 生产工单号
        /// </summary>
        [JsonProperty, Column()]
        public string ICMOBillNO { get; set; }

        /// <summary>
        /// 是否删除 1是0否
        /// </summary>
        [JsonProperty, Column(Name = "isDel")]
        public int? IsDel { get; set; } = 0;

        /// <summary>
        /// isQualified是否合格：0.不合格；1.合格
        /// </summary>
        [JsonProperty, Column(Name = "isQualified")]
        public int? IsQualified { get; set; } = 1;

        /// <summary>
        /// 打印标签模板Id
        /// </summary>
        [JsonProperty, Column(Name = "labelTemplateId")]
        public int? LabelTemplateId { get; set; }

        /// <summary>
        /// 机台编号
        /// </summary>
        [JsonProperty, Column(Name = "machineCode")]
        public string MachineCode { get; set; }

        /// <summary>
        /// 机台Id
        /// </summary>
        [JsonProperty, Column(Name = "machineId")]
        public int? MachineId { get; set; }

        /// <summary>
        /// 机台名称
        /// </summary>
        [JsonProperty, Column(Name = "machineName")]
        public string MachineName { get; set; }

        /// <summary>
        /// [填写]产品盘净重: 计算所得（grossWeight产品盘毛重） - preheaterWeight(线盘重量) - tareWeight（皮重）
        /// </summary>
        [JsonProperty, Column(Name = "netWeight")]
        public double? NetWeight { get; set; }

        /// <summary>
        /// 不合格原因
        /// </summary>
        [JsonProperty, Column(Name = "noQualifiedReason")]
        public string NoQualifiedReason { get; set; }

        /// <summary>
        /// 操作工编号
        /// </summary>
        [JsonProperty, Column(Name = "operatorCode")]
        public string OperatorCode { get; set; }

        /// <summary>
        /// 操作工姓名
        /// </summary>
        [JsonProperty, Column(Name = "operatorName")]
        public string OperatorName { get; set; }

        /// <summary>
        /// 线盘代码
        /// </summary>
        [JsonProperty, Column(Name = "preheaterCode")]
        public string PreheaterCode { get; set; }

        /// <summary>
        /// 线盘ID
        /// </summary>
        [JsonProperty, Column(Name = "preheaterId")]
        public int? PreheaterId { get; set; }

        /// <summary>
        /// 线盘名称
        /// </summary>
        [JsonProperty, Column(Name = "preheaterName")]
        public string PreheaterName { get; set; }

        /// <summary>
        /// 线盘规格
        /// </summary>
        [JsonProperty, Column(Name = "preheaterSpec")]
        public string PreheaterSpec { get; set; }

        /// <summary>
        /// 线盘皮重
        /// </summary>
        [JsonProperty, Column(Name = "preheaterWeight")]
        public double? PreheaterWeight { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [JsonProperty, Column(Name = "productCode")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        [JsonProperty, Column(Name = "productDate")]
        public DateTime? ProductDate { get; set; }

        /// <summary>
        /// 国际标准
        /// </summary>
        [JsonProperty, Column(Name = "productGBName")]
        public string ProductGBName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [JsonProperty, Column(Name = "productId")]
        public int? ProductId { get; set; }

        /// <summary>
        /// 生产条码,对应线盘上生产部门贴的二维码 根据此U8，从星空云获取订单信息
        /// </summary>
        [JsonProperty, Column(Name = "productionBarcode")]
        public string ProductionBarcode { get; set; }

        /// <summary>
        /// 生产组织
        /// </summary>
        [JsonProperty, Column(Name = "productionOrgNO")]
        public string ProductionOrgNO { get; set; }

        /// <summary>
        /// 产品助记码
        /// </summary>
        [JsonProperty, Column(Name = "productMnemonicCode")]
        public string ProductMnemonicCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [JsonProperty, Column(Name = "productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        [JsonProperty, Column(Name = "productSpec")]
        public string ProductSpec { get; set; }

        /// <summary>
        /// 产品执行标准
        /// </summary>
        [JsonProperty, Column(Name = "productStandardName")]
        public string ProductStandardName { get; set; }

        /// <summary>
        /// 手动|自动线线的箱内盘码 【包装组编号 + MMdd + 四位流水号 】
        /// </summary>
        [JsonProperty, Column(StringLength = 50)]
        public string PSN { get; set; }

        /// <summary>
        /// 状态：1.待完成装箱；2.完成装箱；
        /// </summary>
        [JsonProperty, Column(Name = "status")]
        public uint Status { get; set; } = 1;

        /// <summary>
        /// 入库仓库代码
        /// </summary>
        [JsonProperty, Column(Name = "stockCode")]
        public string StockCode { get; set; }

        /// <summary>
        /// 入库仓库ID
        /// </summary>
        [JsonProperty, Column(Name = "stockId")]
        public int? StockId { get; set; }

        /// <summary>
        /// 入库仓库名称
        /// </summary>
        [JsonProperty, Column(Name = "stockName")]
        public string StockName { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 用户代码标准
        /// </summary>
        [JsonProperty, Column(Name = "userStandardCode")]
        public string UserStandardCode { get; set; }

        /// <summary>
        /// 用户标准ID
        /// </summary>
        [JsonProperty, Column(Name = "userStandardId")]
        public int? UserStandardId { get; set; }

        /// <summary>
        /// 用户标准名称
        /// </summary>
        [JsonProperty, Column(Name = "userStandardName")]
        public string UserStandardName { get; set; }

        /// <summary>
        /// 自动线 称重1
        /// </summary>
        [JsonProperty, Column(Name = "weight1")]
        public double? Weight1 { get; set; }

        /// <summary>
        /// 称重员ID
        /// </summary>
        [JsonProperty, Column(Name = "weightUserId")]
        public int? WeightUserId { get; set; }
    }
}