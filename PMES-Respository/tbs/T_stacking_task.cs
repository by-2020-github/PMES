using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES.Model.tbs {

	/// <summary>
	/// 拆垛机任务表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_stacking_task", DisableSyncStructure = true)]
	public partial class T_stacking_task {

		/// <summary>
		/// 拆盘任务表ID
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 线盘格式
		/// </summary>
		[JsonProperty, Column(Name = "reelNum", DbType = "int")]
		public int? ReelNum { get; set; }

		/// <summary>
		/// 线盘规格
		/// </summary>
		[JsonProperty, Column(Name = "reelSpecification", StringLength = 10)]
		public string ReelSpecification { get; set; }

		/// <summary>
		/// 任务开始时间
		/// </summary>
		[JsonProperty, Column(Name = "startTime", DbType = "datetime")]
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// 任务结束时间
		/// </summary>
		[JsonProperty, Column(Name = "stopTIme", DbType = "datetime")]
		public DateTime? StopTIme { get; set; }

		/// <summary>
		/// 拆盘状态：1. 托盘空闲；2. 已上料；3.拆盘中；3.拆盘完成；4.拆盘暂停
		/// </summary>
		[JsonProperty, Column(Name = "stutas", DbType = "int")]
		public int? Stutas { get; set; }

		/// <summary>
		/// 工位号
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", DbType = "int")]
		public int? WorkshopId { get; set; }

	}

}
