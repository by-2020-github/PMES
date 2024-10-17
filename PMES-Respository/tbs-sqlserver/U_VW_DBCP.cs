using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(DisableSyncStructure = true)]
	public partial class U_VW_DBCP {

		public string 产成品漆膜等级 { get; set; }

		public string 产成品漆膜等级代号 { get; set; }

		public int 产成品漆膜等级ID { get; set; }

		public string 产成品形态 { get; set; }

		public string 产成品形态代号 { get; set; }

		public int 产成品形态ID { get; set; }

		public string 产品材质 { get; set; }

		public string 产品材质代号 { get; set; }

		public int 产品材质ID { get; set; }

		public string 产品操作代码 { get; set; }

		public string 产品规格 { get; set; }

		public string 产品规格代号 { get; set; }

		public int 产品规格ID { get; set; }

		public string 产品型号 { get; set; }

		public string 产品型号代号 { get; set; }

		public int 产品型号ID { get; set; }

		public string 国际通用型号 { get; set; }

		public decimal? 满盘率系数 { get; set; }

		public string 美标 { get; set; }

		public string 执行标准 { get; set; }

		public string 执行标准代号 { get; set; }

		public string 执行标准ID { get; set; }

		public int FItemID { get; set; }

		public string FMB { get; set; }

		public string FModel { get; set; }

		public string FName { get; set; }

		public string FNumber { get; set; }

		public int FUSEORGID { get; set; }

		public string FYB { get; set; }

	}

}
