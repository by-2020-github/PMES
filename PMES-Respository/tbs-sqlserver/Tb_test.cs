using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "tb_test", DisableSyncStructure = true)]
	public partial class Tb_test {

        [Column(IsIdentity = true, IsPrimary = true)] public long FInterID { get; set; }

		public string FBanCi { get; set; }

		public string FBH { get; set; }

		public string FBHMX { get; set; }

		public int? FBQID { get; set; }

		public int FBQJS { get; set; }

		public decimal FBZZPZQty { get; set; }

		public string FComputerName { get; set; }

		public string FCPGG { get; set; }

		public string FCPGGID { get; set; }

		public string FCPGGNO { get; set; }

		public int? FCustomer { get; set; }

		public int FCZID { get; set; }

		public string FCZM { get; set; }

		public string FCZName { get; set; }

		public string FCZNO { get; set; }

		public string FCZR { get; set; }

		public int? FCZRID { get; set; }

		public DateTime? FDate { get; set; }

		public DateTime? FDate2 { get; set; }

		public int FEmp { get; set; }

		public string FGJTYXH { get; set; }

		public string FHGZQty { get; set; }

		public string FICMOBillNO { get; set; }

		public string FICMOID { get; set; }

		public string FInflag { get; set; }

		public int FISNEW { get; set; }

		public int? FItemID { get; set; }

		public int? FJSBZID { get; set; }

		public string FJSBZNumber { get; set; }

		public string FJTH { get; set; }

		public string FJYR { get; set; }

		public decimal FJZQty { get; set; }

		public string FKCorgno { get; set; }

		public string FLinkStacklabel { get; set; }

		public decimal FMZQty { get; set; }

		public string FNumber { get; set; }

		public string FOLDBarcode { get; set; }

		public int FOLDBarcodeID { get; set; }

		public string FOldSP { get; set; }

		public string FOutflag { get; set; }

		public string FPCH { get; set; }

		public string FPDFlag { get; set; }

		public decimal FPZQty { get; set; }

		public string FQMDJ { get; set; }

		public int FQMDJID { get; set; }

		public string FQMDJNO { get; set; }

		public int FQPLBZ { get; set; }

		public string FSCorgno { get; set; }

		public DateTime FSPTime { get; set; }

		public int? FStockID { get; set; }

		public string FStockPlace { get; set; }

		public string FStrip { get; set; }

		public string FStrip2 { get; set; }

		public int FSXH { get; set; }

		public string FTHFlag { get; set; }

		public string FType { get; set; }

		public int FTypeID { get; set; }

		public string FTypeNO { get; set; }

		public string FTypeTemp { get; set; }

		public string FXPCZM { get; set; }

		public string FXPGG { get; set; }

		public int? FXPItemID { get; set; }

		public string FXPName { get; set; }

		public string FXPNumber { get; set; }

		public decimal FXPQty { get; set; }

		public int FXTID { get; set; }

		public string FXTName { get; set; }

		public string FXTNO { get; set; }

		public decimal FZQty { get; set; }

		public string FZXBZ { get; set; }
		
		public int FZXBZID { get; set; }
		public int BoxID { get; set; }

	}

}
