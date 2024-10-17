using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_production_line_number", DisableSyncStructure = true)]
	public partial class T_production_line_number {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public string Name { get; set; }

		public string Remark { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
