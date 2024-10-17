using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 子母空托盘
	/// </summary>
	[Table(Name = "t_child_mother_tray", DisableSyncStructure = true)]
	public partial class T_child_mother_tray {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		/// <summary>
		/// 子母托缓存工位
		/// </summary>
		public int? AreaWorkshopId { get; set; }

		/// <summary>
		/// 子托盘规格
		/// </summary>
		public string ChildTraySpecification { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 母托盘条码
		/// </summary>
		public string MotherTrayBarCode { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }

		/// <summary>
		/// 码垛工位
		/// </summary>
		public int? StackingWorkshopId { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		public int? Status { get; set; } = 0;

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }

	}

}
