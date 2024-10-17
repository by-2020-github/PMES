using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_line_material", DisableSyncStructure = true)]
	public partial class T_line_material {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public string Code { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? LineNo { get; set; }

		public string Name { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
