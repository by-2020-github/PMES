using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_pmes_material", DisableSyncStructure = true)]
	public partial class T_pmes_material {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public string Code { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? IsNaked { get; set; }

		public int? LineNo { get; set; }

		public string Name { get; set; }

		public int? PreheaterNumOfPerStay { get; set; }

		public int? Status { get; set; } = 0;

		public DateTime? UpdateTime { get; set; }

	}

}
