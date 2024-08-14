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
	/// Agv日志表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_agv_log", DisableSyncStructure = true)]
	public partial class T_agv_log {

		/// <summary>
		/// agv日志表 -- 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		[JsonProperty, Column(Name = "content")]
		public string Content { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 请求号
		/// </summary>
		[JsonProperty, Column(Name = "reqCode", StringLength = 40)]
		public string ReqCode { get; set; }

		/// <summary>
		/// agv日志类型
		/// </summary>
		[JsonProperty, Column(Name = "type", StringLength = 20)]
		public string Type { get; set; }

	}

}
