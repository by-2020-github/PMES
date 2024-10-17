using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 包材辅料分类
	/// </summary>
	[Table(Name = "t_material_category", DisableSyncStructure = true)]
	public partial class T_material_category {

        /// <summary>
        /// 分类id-主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 分类名称
		/// </summary>
		public string Name { get; set; }

		public string Remark { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
