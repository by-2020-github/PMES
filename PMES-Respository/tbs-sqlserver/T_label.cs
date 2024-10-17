using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_label", DisableSyncStructure = true)]
	public partial class T_label {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		/// <summary>
		/// 颜色
		/// </summary>
		public string Color { get; set; }

		public DateTime? Createtime { get; set; }

		public bool? Iscurrent { get; set; }

		public int? IsDel { get; set; }

		/// <summary>
		/// 标签模版Id
		/// </summary>
		public int? LabelTemplateId { get; set; }

		public string Name { get; set; }

		public string Numofpackeditems { get; set; }

		public string Remark { get; set; }

		/// <summary>
		/// 尺寸
		/// </summary>
		public string Size { get; set; }

		public int? Sortnum { get; set; }

		public DateTime? UpdateTime { get; set; }
        public bool IsCurrent { get; set; }
    }

}
