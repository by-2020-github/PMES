using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_package_group_releated_line", DisableSyncStructure = true)]
	public partial class T_package_group_releated_line {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public string Package_group_id { get; set; }

		public string Production_line_id { get; set; }

		public string Remark { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
