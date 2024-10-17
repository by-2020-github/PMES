using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_formula", DisableSyncStructure = true)]
	public partial class T_formula {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public string Content { get; set; }

		public DateTime? CreateTime { get; set; }

		public string Remark { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		public int? Status { get; set; } = 0;

		public int? Type { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
