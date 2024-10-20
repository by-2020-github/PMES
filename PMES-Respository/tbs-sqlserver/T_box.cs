using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_box", DisableSyncStructure = true)]
	public partial class T_box {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public DateTime? CreateTime { get; set; } = DateTime.Now;

        public int? IsDel { get; set; } = 0;

		public int? LabelId { get; set; }

		public string LabelName { get; set; }

		public int? LabelTemplateId { get; set; }

		public string PackagingCode { get; set; }

		public string PackagingSN { get; set; }

		public string PackagingWorker { get; set; }

		public string PackingBarCode { get; set; }

		public double? PackingGrossWeight { get; set; }

		public string PackingQty { get; set; }

		public double? PackingWeight { get; set; }

		public int Status { get; set; }

		public string TrayBarcode { get; set; }

		public DateTime? UpdateTime { get; set; } = DateTime.Now;

	}

}
