using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 操作工
	/// </summary>
	[Table(Name = "t_user", DisableSyncStructure = true)]
	public partial class T_user {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public string Code { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? IsDel { get; set; }

		public int? IsLoginAutoLine { get; set; }

		public string LoginUsername { get; set; }

		public string Password { get; set; }

		public int? Status { get; set; }

		public string U8 { get; set; }

		public DateTime? UpdateTime { get; set; }

		public string Username { get; set; }

		public string WorkNo { get; set; }

		public string WorkshopIds { get; set; }

	}

}
