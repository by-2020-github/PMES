using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 包装登记表
	/// </summary>
	[Table(Name = "t_order_package", DisableSyncStructure = true)]
	public partial class T_order_package {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public string DeliverySubTrayName { get; set; }

		public string FullCoilWeight { get; set; }

		public double? MaxWeight { get; set; }

		public double? MinWeight { get; set; }

		public string PackagingReqCode { get; set; }

		public string PackagingReqName { get; set; }

		/// <summary>
		/// 装箱件数
		/// </summary>
		public int? PackingQuantity { get; set; }

		public string Paperboard_name { get; set; }

		/// <summary>
		/// 纸板编号
		/// </summary>
		public int? Paperboard_number { get; set; }

		public string Paperboard_spec { get; set; }

		/// <summary>
		/// 线盘表id
		/// </summary>
		public int PreheaterCodeId { get; set; }

		public string PreheaterInsidePackageName { get; set; }

		public string PreheaterOutsidePackageName { get; set; }

		public string ProductionBarcode { get; set; }

		public int? StackingLayers { get; set; }

		public int? StackingPerLayer { get; set; }

		public bool Super_wide_sub_tray { get; set; } = false;

		public double TareWeight { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
