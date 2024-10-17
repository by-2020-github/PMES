using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_material", DisableSyncStructure = true)]
	public partial class T_material {

        /// <summary>
        /// 主键id
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)] public int Id { get; set; }

		/// <summary>
		/// 类型：1.发货木托盘；2. 母托盘组；3.盖板；4.纸质类；5.其他
		/// </summary>
		public int? CategoryId { get; set; }

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
