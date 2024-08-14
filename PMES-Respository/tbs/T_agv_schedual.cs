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
	/// Agv任务调度表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_agv_schedual", DisableSyncStructure = true)]
	public partial class T_agv_schedual {

		/// <summary>
		/// agv表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// agv消息数据
		/// </summary>
		[JsonProperty, Column(Name = "agvMsgData")]
		public string AgvMsgData { get; set; }

		/// <summary>
		/// AGV产生的任务号
		/// </summary>
		[JsonProperty, Column(Name = "agvReqCode", StringLength = 40)]
		public string AgvReqCode { get; set; } = "1";

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// AGV调度状态
		/// </summary>
		[JsonProperty, Column(Name = "method", StringLength = 40)]
		public string Method { get; set; }

		/// <summary>
		/// 调度任务号- pmes产生调度任务时产生的
		/// </summary>
		[JsonProperty, Column(Name = "pmesTaskCode", StringLength = 40)]
		public string PmesTaskCode { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
