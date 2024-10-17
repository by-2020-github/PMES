using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 工段表
	/// </summary>
	[Table(Name = "t_work_section", DisableSyncStructure = true)]
	public partial class T_work_section {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		/// <summary>
		/// 工段Code
		/// </summary>
		public string Code { get; set; }

		public string Name { get; set; }

	}

}
