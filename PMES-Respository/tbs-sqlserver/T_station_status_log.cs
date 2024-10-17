using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 工位状态调度表
	/// </summary>
	[Table(Name = "t_station_status_log", DisableSyncStructure = true)]
	public partial class T_station_status_log {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 日志
		/// </summary>
		public string Log { get; set; }

		/// <summary>
		/// 工段id
		/// </summary>
		public int? Section { get; set; }

		public int? WorkshopId { get; set; }

	}

}
