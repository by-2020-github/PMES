using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(DisableSyncStructure = true)]
	public partial class U_VW_SCDD {

		public string F_103 { get; set; }

		public string F_108 { get; set; }

		public decimal? F_110 { get; set; }

		public decimal? F_115 { get; set; }

		public int F_UC_CUSTOMER { get; set; }

		public int? F_UC_MACHINE { get; set; }

		public string F_UC_PRINTTYPE { get; set; }

		public int? F_UC_WORKJT { get; set; }

		public string FAB { get; set; }

		public string FBILLNO { get; set; }

		public DateTime? FDATE { get; set; }

		public int? FENTRYID { get; set; }

		public int FID { get; set; }

		public int? FJSBZ { get; set; }

		public int? FMATERIALID { get; set; }

		public string FMESSTATUS { get; set; }

		public string FNAME { get; set; }

		public string FNUMBER { get; set; }

		public int Fpjs { get; set; }

		public int? FPRDORGID { get; set; }

		public decimal? FQTY { get; set; }

		public int? FREQUESTORGID { get; set; }

		public string Fsczt { get; set; }

		public string FSPECIFICATION { get; set; }

		public string FSTATUS { get; set; }

		public int? FWORKSHOPID { get; set; }

		public int? FXP { get; set; }

		public int? Fyjs { get; set; }

	}

}
