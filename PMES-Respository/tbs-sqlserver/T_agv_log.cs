using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// agv调度日志
	/// </summary>
	[Table(Name = "t_agv_log", DisableSyncStructure = true)]
	public partial class T_agv_log {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

		/// <summary>
		/// 日志内容
		/// </summary>
		public string Content { get; set; }

		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 目的地
		/// </summary>
		public int? Dest { get; set; }

		/// <summary>
		/// pmes申请agv指令内容
		/// </summary>
		public string PmesReqContent { get; set; }

		/// <summary>
		/// agv请求代码
		/// </summary>
		public string ReqCode { get; set; }

		/// <summary>
		/// 出发点
		/// </summary>
		public int? Src { get; set; }

		/// <summary>
		/// agv日志类型
		/// </summary>
		public string Type { get; set; }

	}

}
