using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 子托盘组
	/// </summary>
	[Table(Name = "t_child_tray_group", DisableSyncStructure = true)]
	public partial class T_child_tray_group {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		/// <summary>
		/// 暂存区工位编号
		/// </summary>
		public int? AreaWorkshopId { get; set; }

		public string ChildTrayMaterialSpecification { get; set; }

		/// <summary>
		/// 组托盘工位
		/// </summary>
		public int? CombinateWorkshopId { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		public DateTime? CreateTime { get; set; }

		public string MotherTrayBarCode { get; set; }

		public int? Num { get; set; }

		/// <summary>
		/// 状态:0.wms入pmes缓存工位；1.运送至【子盘组】工作工位；2. 组托中；3.组托完成
		/// </summary>
		public int? Status { get; set; } = 0;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }

	}

}
