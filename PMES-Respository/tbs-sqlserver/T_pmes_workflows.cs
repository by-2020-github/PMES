using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 工作流
	/// </summary>
	[Table(Name = "t_pmes_workflows", DisableSyncStructure = true)]
	public partial class T_pmes_workflows {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public string Content { get; set; }

		public DateTime? CreateTime { get; set; }

		public string Flow { get; set; }

		/// <summary>
		/// 工段
		/// </summary>
		public string Section { get; set; }

		/// <summary>
		/// Pmes工作流
		/// </summary>
		public int? SectionId { get; set; }

	}

}
