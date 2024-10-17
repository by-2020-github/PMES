using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// agv表
	/// </summary>
	[Table(Name = "t_agv", DisableSyncStructure = true)]
	public partial class T_agv {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 触发地
		/// </summary>
		public string From { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }

		/// <summary>
		/// 目的地
		/// </summary>
		public string To { get; set; }

		/// <summary>
		/// 工位条码
		/// </summary>
		public string WorkshopBarcode { get; set; }

	}

}
