using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_mother_tray_group", DisableSyncStructure = true)]
	public partial class T_mother_tray_group {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public int? CombinateWorkshopId { get; set; }

		public DateTime? CreateTime { get; set; }

		public string MotherTrayBarcode { get; set; }

		public int? Num { get; set; }

		public int? Status { get; set; } = 0;

		public DateTime? UpdateTime { get; set; }

		public int? WorkshopId { get; set; }

	}

}
