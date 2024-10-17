using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_staging_area_stay", DisableSyncStructure = true)]
	public partial class T_staging_area_stay {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? LineNo { get; set; }

		public int? PreaheateNumOfPerStay { get; set; }

		public string RackNo { get; set; }

		public string Remark { get; set; }

		public int? Status { get; set; }

		public string StayCode { get; set; }

	}

}
