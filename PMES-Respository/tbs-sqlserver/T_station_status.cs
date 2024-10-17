using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_station_status", DisableSyncStructure = true)]
	public partial class T_station_status {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public string Assign { get; set; }

		public string AttachInfo { get; set; }

		public string BarCode { get; set; }

		public DateTime? CreateTime { get; set; }

		public short? IsEfficient { get; set; }

		public int? IsNotifySuccess { get; set; }

		public long? LineId { get; set; }

		public string Remark { get; set; }

		public int? Status { get; set; }

		public DateTime? UpdateTime { get; set; }

		public int? Worksection { get; set; }

		public int WorkshopId { get; set; }

	}

}
