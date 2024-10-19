using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_label_type", DisableSyncStructure = true)]
	public partial class T_label_type {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public int LabelType { get; set; }

        public string PrinterName { get; set; }

		/// <summary>
		/// s时间类型
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 物理删除
		/// </summary>
		public string IsDel { get; set; }

		public string Name { get; set; }

        public string Remark { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
