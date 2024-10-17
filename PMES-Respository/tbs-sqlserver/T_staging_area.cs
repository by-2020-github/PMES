using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 人工上料数据
	/// </summary>
	[Table(Name = "t_staging_area", DisableSyncStructure = true)]
	public partial class T_staging_area {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? Ff { get; set; } = 0;

		public string OperatorU8 { get; set; }

		/// <summary>
		/// 线盘规格
		/// </summary>
		public string PmesAgvTaskCode { get; set; }

		public string ReelNum { get; set; }

		public string ReelSpecification { get; set; }

		public int? Status { get; set; } = 0;

		public string UpdateTime { get; set; }

		public string VirtualMotherStayCode { get; set; }

		/// <summary>
		/// 线盘个数
		/// </summary>
		public int? WorkshopId { get; set; }

	}

}
