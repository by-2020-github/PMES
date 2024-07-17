using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlServer
{
    [Table(Name = "tb_test")]
    public class OldTest
    {
        /// <summary>
        /// 自增ID src:productId
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public long FInterID { get; set; }

        /// <summary>
        /// 产品ID src:productCode
        /// </summary>
        public int? FItemID { get; set; }

        /// <summary>
        /// 产品代码 src:productMnemonicCode
        /// </summary>
        public string FNumber { get; set; }

        /// <summary>
        /// 产品助记码 src:productGBName
        /// </summary>
        public string FCZM { get; set; }

        /// <summary>
        /// 国标型号 src:productName
        /// </summary>
        public string FGJTYXH { get; set; }

        /// <summary>
        /// 产品型号 src:产品型号ID
        /// </summary>
        public string FType { get; set; }

        /// <summary>
        /// 产品型号ID src:产品型号代号
        /// </summary>
        public int FTypeID { get; set; }

        /// <summary>
        /// 产品型号代码 src:产成品漆膜等级ID
        /// </summary>
        public string FTypeNO { get; set; }

        /// <summary>
        /// 漆膜等级ID src:产成品漆膜等级代号
        /// </summary>
        public int FQMDJID { get; set; }

        /// <summary>
        /// 漆膜等级代码 src:产成品漆膜等级
        /// </summary>
        public string FQMDJNO { get; set; }

        /// <summary>
        /// 漆膜等级 src:产品规格ID
        /// </summary>
        public string FQMDJ { get; set; }

        /// <summary>
        /// 产品规格ID src:产品规格代号
        /// </summary>
        public string FCPGGID { get; set; }

        /// <summary>
        /// 产品规格代码 src:产品材质ID
        /// </summary>
        public string FCPGGNO { get; set; }

        /// <summary>
        /// 产品材质ID src:产品材质代号
        /// </summary>
        public int FCZID { get; set; }

        /// <summary>
        /// 产品材质代码 src:产品材质
        /// </summary>
        public string FCZNO { get; set; }

        /// <summary>
        /// 产品材质名称 src:产成品形态ID
        /// </summary>
        public string FCZName { get; set; }

        /// <summary>
        /// 产品形态ID src:产成品形态代号
        /// </summary>
        public int FXTID { get; set; }

        /// <summary>
        /// 产品形态代码 src:产成品形态
        /// </summary>
        public string FXTNO { get; set; }

        /// <summary>
        /// 产品形态名称 src:productSpec
        /// </summary>
        public string FXTName { get; set; }

        /// <summary>
        /// 产品规格 src:preheaterId
        /// </summary>
        public string FCPGG { get; set; }

        /// <summary>
        /// 线盘ID src:preheaterCode
        /// </summary>
        public int? FXPItemID { get; set; }

        /// <summary>
        /// 线盘代码 src:
        /// </summary>
        public string FXPNumber { get; set; }

        /// <summary>
        /// 线盘助记码 src:preheaterName
        /// </summary>
        public string FXPCZM { get; set; }

        /// <summary>
        /// 线盘名称 src:preheaterSpe
        /// </summary>
        public string FXPName { get; set; }

        /// <summary>
        /// 线盘规格 src:preheaterWeight
        /// </summary>
        public string FXPGG { get; set; }

        /// <summary>
        /// 线盘重量 src:wrapperWeight
        /// </summary>
        public decimal FXPQty { get; set; }

        /// <summary>
        /// 包装纸皮重 src:machineCode
        /// </summary>
        public decimal FBZZPZQty { get; set; }

        /// <summary>
        /// 机台名称 src:
        /// </summary>
        public string FJTH { get; set; }

        /// <summary>
        /// 特殊标识 src:packagingSN
        /// </summary>
        public string FBanCi { get; set; }

        /// <summary>
        /// 编号 src:PSN
        /// </summary>
        public string FBH { get; set; }

        /// <summary>
        /// 明细编号 src:batchNO
        /// </summary>
        public string FBHMX { get; set; }

        /// <summary>
        /// 产品批次号 src:createTime
        /// </summary>
        public string FPCH { get; set; }

        /// <summary>
        /// 称重日期 src:
        /// </summary>
        public DateTime? FDate { get; set; }

        /// <summary>
        /// 顺序号 src:packingQty
        /// </summary>
        public int FSXH { get; set; }

        /// <summary>
        /// 装箱件数 src:operatorName
        /// </summary>
        public string FHGZQty { get; set; }

        /// <summary>
        /// 操作工姓名 src:weightUserId
        /// </summary>
        public string FJYR { get; set; }

        /// <summary>
        /// 打包员ID src:packagingWorker
        /// </summary>
        public int? FCZRID { get; set; }

        /// <summary>
        /// 打包员姓名 src:
        /// </summary>
        public string FCZR { get; set; }

        /// <summary>
        /// 执行标准ID src:productStandardName
        /// </summary>
        public int FZXBZID { get; set; }

        /// <summary>
        /// 执行标准名称 src:packingGrossWeight
        /// </summary>
        public string FZXBZ { get; set; }

        /// <summary>
        /// 毛重 src:preheaterWeight
        /// </summary>
        public decimal FMZQty { get; set; }

        /// <summary>
        /// 线盘皮重 src:netWeight
        /// </summary>
        public decimal FPZQty { get; set; }

        /// <summary>
        /// 净重 src:packingBarCode
        /// </summary>
        public decimal FJZQty { get; set; }

        /// <summary>
        /// 产品条码 src:packagingCode
        /// </summary>
        public string FStrip { get; set; }

        /// <summary>
        /// 称重计算机名 src:
        /// </summary>
        public string FComputerName { get; set; }

        /// <summary>
        /// 入库状态标志 src:
        /// </summary>
        public string FInflag { get; set; }

        /// <summary>
        /// 出库状态标志 src:
        /// </summary>
        public string FOutflag { get; set; }

        /// <summary>
        /// 退货状态标志 src:packingWeight
        /// </summary>
        public string FTHFlag { get; set; }

        /// <summary>
        /// 总净重 src:labelTemplateId
        /// </summary>
        public decimal FZQty { get; set; }

        /// <summary>
        /// 标签类型ID src:packingQty
        /// </summary>
        public int? FBQID { get; set; }

        /// <summary>
        /// 标签件数 src:
        /// </summary>
        public int FBQJS { get; set; }

        /// <summary>
        /// 盘点标志 src:productName
        /// </summary>
        public string FPDFlag { get; set; }

        /// <summary>
        /// 产品型号 src:
        /// </summary>
        public string FTypeTemp { get; set; }

        /// <summary>
        /// 库位 src:ICMOID
        /// </summary>
        public string FStockPlace { get; set; }

        /// <summary>
        /// 派工单ID src:ICMOBillNO
        /// </summary>
        public int FICMOID { get; set; }

        /// <summary>
        /// 派工单号 src:
        /// </summary>
        public string FICMOBillNO { get; set; }

        /// <summary>
        /// 入库时间 src:
        /// </summary>
        public DateTime FSPTime { get; set; }

        /// <summary>
        /// 老库位 src:
        /// </summary>
        public string FOldSP { get; set; }

        /// <summary>
        /// 新条码标志 src:
        /// </summary>
        public int FISNEW { get; set; }

        /// <summary>
        /// 线盘浅盘率 src:
        /// </summary>
        public int FQPLBZ { get; set; }

        /// <summary>
        /// 老条码ID src:
        /// </summary>
        public int FOLDBarcodeID { get; set; }

        /// <summary>
        /// 老条码 src:productionBarcode
        /// </summary>
        public string FOLDBarcode { get; set; }

        /// <summary>
        /// 产品二维码 src:createTime
        /// </summary>
        public string FStrip2 { get; set; }

        /// <summary>
        /// 称重时间 src:userStandardId
        /// </summary>
        public DateTime? FDate2 { get; set; }

        /// <summary>
        /// 技术标准ID src:userStandardCode
        /// </summary>
        public int? FJSBZID { get; set; }

        /// <summary>
        /// 技术标准代码 src:productionOrgNO
        /// </summary>
        public string FJSBZNumber { get; set; }

        /// <summary>
        /// 生产组织 src:
        /// </summary>
        public string FSCorgno { get; set; }

        /// <summary>
        /// 库存组织 src:stockId
        /// </summary>
        public string FKCorgno { get; set; }

        /// <summary>
        /// 仓库ID src:customerId
        /// </summary>
        public int? FStockID { get; set; }

        /// <summary>
        /// 客户ID src:
        /// </summary>
        public int? FCustomer { get; set; }

        /// <summary>
        /// 业务员ID src:trayBarcode
        /// </summary>
        public int FEmp { get; set; }

        /// <summary>
        /// 栈板号 src:
        /// </summary>
        public string FLinkStacklabel { get; set; }
    }
}