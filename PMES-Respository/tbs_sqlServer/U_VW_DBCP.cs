using System;
using System.Collections.Generic;
using System.Text;

namespace PMES_Respository.tbs_sqlServer
{
    /// <summary>
    /// 视图名称：[PMES].[dbo].[U_VW_DBCP]
    /// </summary>
    public class U_VW_DBCP
    {
        public string FItemID { get; set; }//产品ID
        public string FNumber { get; set; }//产品代码
        public string FName { get; set; }//产品名称
        public string FSPECIFICATION { get; set; }//产品规格
        public string 产品操作代码 { get; set; }//产品操作代码
        public string 国际通用型号 { get; set; }//国际通用型号
        public int 产品型号ID { get; set; }//产品型号ID
        public string 产品型号代号 { get; set; }//产品型号代号
        public string 产品型号 { get; set; }//产品型号
        public int 产成品漆膜等级ID { get; set; }//产成品漆膜等级ID
        public string 产成品漆膜等级代号 { get; set; }//产成品漆膜等级代号
        public string 产成品漆膜等级 { get; set; }//产成品漆膜等级
        public int 产品材质ID { get; set; }//产品材质ID
        public string 产品材质代号 { get; set; }//产品材质代号
        public string 产品材质 { get; set; }//产品材质
        public int 产成品形态ID { get; set; }//产成品形态ID
        public string 产成品形态代号 { get; set; }//产成品形态代号
        public string 产成品形态 { get; set; }//产成品形态
        public string 产品规格ID { get; set; }//产品规格ID
        public string 产品规格代号 { get; set; }//产品规格代号
        public string 产品规格 { get; set; }//产品规格
        public string 执行标准ID { get; set; }//执行标准ID
        public string 执行标准代号 { get; set; }//执行标准代号
        public string 执行标准 { get; set; }//执行标准
        public string 美标 { get; set; }//美标
        public string 满盘率系数 { get; set; }//满盘率系数
        public string FMB { get; set; }//
        public string FYB { get; set; }//
        public string FUSEORGID { get; set; }//使用组织ID

    }
}
