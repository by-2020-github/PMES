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
	/// 事件
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_event", DisableSyncStructure = true)]
	public partial class T_event {

		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 停驳位编号
		/// </summary>
		[JsonProperty, Column(Name = "content")]
		public string Content { get; set; }

		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 机台编号
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 10)]
		public string Name { get; set; }

		/// <summary>
		/// 停驳位名称
		/// </summary>
		[JsonProperty, Column(Name = "startTime", DbType = "datetime")]
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// 产线号
		/// </summary>
		[JsonProperty, Column(Name = "stopTIme", DbType = "datetime")]
		public DateTime? StopTIme { get; set; }

	}

}
