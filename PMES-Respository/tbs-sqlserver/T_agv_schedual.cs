using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// agv调度表
	/// </summary>
	[Table(Name = "t_agv_schedual", DisableSyncStructure = true)]
	public partial class T_agv_schedual {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		/// <summary>
		/// agv代码
		/// </summary>
		public string AgvCode { get; set; }

		/// <summary>
		/// agv消息数据
		/// </summary>
		public string AgvMsgData { get; set; }

		/// <summary>
		/// agv产生的请求代码
		/// </summary>
		public string AgvReqCode { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// agv调度状态机
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// agv状态机处理状态 ：处理状态：0.待处理；1.已处理
		/// </summary>
		public int? PmesProcessFlag { get; set; } = 0;

		/// <summary>
		/// pmes任务调度编号
		/// </summary>
		public string PmesReqCode { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }

	}

}
