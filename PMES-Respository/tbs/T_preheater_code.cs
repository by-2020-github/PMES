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
    [JsonObject(MemberSerialization.OptIn), Table(Name = "t_preheater_code", DisableSyncStructure = true)]
    public partial class T_preheater_code
    {

        /// <summary>
        /// 盘码表主键ID
        /// </summary>
        [JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 自动线 称重2
        /// </summary>
        [JsonProperty, Column(Name = "weight2", IsPrimary = true)]
        public double Weight2 { get; set; }

        /// <summary>
        /// 批次号，规则<生产工单号+月日+R或Y;8:00~20:00定义为R>; 目前暂时的实现方式，对应pc界面上的-生产工单：批次号暂时先用生产订单号，后面要调整的，理论批次号来源于生产标签打印的时候产生-施部长回答
        /// </summary>
        [JsonProperty, Column(Name = "batchNO", StringLength = 100)]
        public string BatchNO { get; set; }

        /// <summary>
        /// 记录建立时间
        /// </summary>
        [JsonProperty, Column(Name = "createTime", DbType = "datetime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 客户代码
        /// </summary>
        [JsonProperty, Column(Name = "customerCode", StringLength = 50)]
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [JsonProperty, Column(Name = "customerId", DbType = "int")]
        public int? CustomerId { get; set; }

        /// <summary>
        /// 客户物料代码
        /// </summary>
        [JsonProperty, Column(Name = "customerMaterialCode", StringLength = 50)]
        public string CustomerMaterialCode { get; set; }

        /// <summary>
        /// 客户物料名称
        /// </summary>
        [JsonProperty, Column(Name = "customerMaterialName", StringLength = 100)]
        public string CustomerMaterialName { get; set; }

        /// <summary>
        /// 客户物料规格
        /// </summary>
        [JsonProperty, Column(Name = "customerMaterialSpec", StringLength = 50)]
        public string CustomerMaterialSpec { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [JsonProperty, Column(Name = "customerName", StringLength = 50)]
        public string CustomerName { get; set; }

        /// <summary>
        /// [填写]产品盘毛重: 1. 人工线：由电子称设备获取，人工3次称重的平均值；2.自动线，二成称重后【erp称重校验】获得。
        /// </summary>
        [JsonProperty, Column(Name = "grossWeight")]
        public double? GrossWeight { get; set; }

        /// <summary>
        /// 生产工单号
        /// </summary>
        [JsonProperty, Column(StringLength = 50)]
        public string ICMOBillNO { get; set; }

        /// <summary>
        /// 是否删除 1是0否
        /// </summary>
        [JsonProperty, Column(Name = "isDel", DbType = "int")]
        public int? IsDel { get; set; } = 0;

        /// <summary>
        /// isQualified是否合格：0.不合格；1.合格
        /// </summary>
        [JsonProperty, Column(Name = "isQualified", DbType = "int")]
        public int? IsQualified { get; set; } = 1;

        /// <summary>
        /// 打印标签模板Id
        /// </summary>
        [JsonProperty, Column(Name = "labelTemplateId", DbType = "int")]
        public int? LabelTemplateId { get; set; }

        /// <summary>
        /// 机台编号
        /// </summary>
        [JsonProperty, Column(Name = "machineCode", StringLength = 50)]
        public string MachineCode { get; set; }

        /// <summary>
        /// 机台Id
        /// </summary>
        [JsonProperty, Column(Name = "machineId", DbType = "int")]
        public int? MachineId { get; set; }

        /// <summary>
        /// 机台名称
        /// </summary>
        [JsonProperty, Column(Name = "machineName", StringLength = 50)]
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
        [JsonProperty, Column(Name = "operatorCode", StringLength = 50)]
        public string OperatorCode { get; set; }

        /// <summary>
        /// 操作工姓名
        /// </summary>
        [JsonProperty, Column(Name = "operatorName", StringLength = 50)]
        public string OperatorName { get; set; }

        /// <summary>
        /// 线盘代码
        /// </summary>
        [JsonProperty, Column(Name = "preheaterCode", StringLength = 50)]
        public string PreheaterCode { get; set; }

        /// <summary>
        /// 线盘ID
        /// </summary>
        [JsonProperty, Column(Name = "preheaterId", DbType = "int")]
        public int? PreheaterId { get; set; }

        /// <summary>
        /// 线盘名称
        /// </summary>
        [JsonProperty, Column(Name = "preheaterName", StringLength = 100)]
        public string PreheaterName { get; set; }

        /// <summary>
        /// 线盘规格
        /// </summary>
        [JsonProperty, Column(Name = "preheaterSpec", StringLength = 50)]
        public string PreheaterSpec { get; set; }

        /// <summary>
        /// 线盘皮重
        /// </summary>
        [JsonProperty, Column(Name = "preheaterWeight", DbType = "double(18,2)")]
        public double? PreheaterWeight { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        [JsonProperty, Column(Name = "productCode", StringLength = 50)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        [JsonProperty, Column(Name = "productDate", DbType = "datetime")]
        public DateTime? ProductDate { get; set; }

        /// <summary>
        /// 国际标准
        /// </summary>
        [JsonProperty, Column(Name = "productGBName", StringLength = 100)]
        public string ProductGBName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [JsonProperty, Column(Name = "productId", DbType = "int")]
        public int? ProductId { get; set; }

        /// <summary>
        /// 生产条码,对应线盘上生产部门贴的二维码 根据此U8，从星空云获取订单信息
        /// </summary>
        [JsonProperty, Column(Name = "productionBarcode", StringLength = 50)]
        public string ProductionBarcode { get; set; }

        /// <summary>
        /// 生产组织
        /// </summary>
        [JsonProperty, Column(Name = "productionOrgNO", StringLength = 50)]
        public string ProductionOrgNO { get; set; }

        /// <summary>
        /// 产品助记码
        /// </summary>
        [JsonProperty, Column(Name = "productMnemonicCode", StringLength = 50)]
        public string ProductMnemonicCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [JsonProperty, Column(Name = "productName", StringLength = 100)]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        [JsonProperty, Column(Name = "productSpec", StringLength = 50)]
        public string ProductSpec { get; set; }

        /// <summary>
        /// 产品执行标准
        /// </summary>
        [JsonProperty, Column(Name = "productStandardName", StringLength = 50)]
        public string ProductStandardName { get; set; }

        /// <summary>
        /// 手动|自动线线的箱内盘码 【包装组编号 + MMdd + 四位流水号 】
        /// </summary>
        [JsonProperty, Column(StringLength = 50)]
        public string PSN { get; set; }

        /// <summary>
        /// 状态：1.待完成装箱；2.完成装箱；
        /// </summary>
        [JsonProperty, Column(Name = "status", DbType = "int unsigned")]
        public uint Status { get; set; } = 1;

        /// <summary>
        /// 入库仓库代码
        /// </summary>
        [JsonProperty, Column(Name = "stockCode", StringLength = 50)]
        public string StockCode { get; set; }

        /// <summary>
        /// 入库仓库ID
        /// </summary>
        [JsonProperty, Column(Name = "stockId", DbType = "int")]
        public int? StockId { get; set; }

        /// <summary>
        /// 入库仓库名称
        /// </summary>
        [JsonProperty, Column(Name = "stockName", StringLength = 50)]
        public string StockName { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 用户代码标准
        /// </summary>
        [JsonProperty, Column(Name = "userStandardCode", StringLength = 50)]
        public string UserStandardCode { get; set; }

        /// <summary>
        /// 用户标准ID
        /// </summary>
        [JsonProperty, Column(Name = "userStandardId", DbType = "int")]
        public int? UserStandardId { get; set; }

        /// <summary>
        /// 用户标准名称
        /// </summary>
        [JsonProperty, Column(Name = "userStandardName", StringLength = 100)]
        public string UserStandardName { get; set; }

        /// <summary>
        /// 自动线 称重1
        /// </summary>
        [JsonProperty, Column(Name = "weight1")]
        public double? Weight1 { get; set; }

        /// <summary>
        /// 称重员ID
        /// </summary>
        [JsonProperty, Column(Name = "weightUserId", DbType = "int")]
        public int? WeightUserId { get; set; }

    }

}
