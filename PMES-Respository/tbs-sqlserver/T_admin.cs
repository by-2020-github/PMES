using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_admin", DisableSyncStructure = true)]
	public partial class T_admin {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public string Account { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? IsDel { get; set; }

		public string Phone { get; set; }

		public string Pwd { get; set; }

		public string RealName { get; set; }

		public int? Status { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
