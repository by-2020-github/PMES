using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_child_tray_mode_config", DisableSyncStructure = true)]
	public partial class T_child_tray_mode_config {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		/// <summary>
		/// 类型：1.发货木托盘；2. 母托盘组；3.盖板；4.纸质类；5.其他
		/// </summary>
		public int? Category { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		public int? Num { get; set; }

		public string Spec { get; set; }

		public string Unit { get; set; }

	}

}
