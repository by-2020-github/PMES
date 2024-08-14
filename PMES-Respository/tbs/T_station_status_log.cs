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
	/// 工位状态改变日志表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_station_status_log", DisableSyncStructure = true)]
	public partial class T_station_status_log {

		/// <summary>
		/// 工位编号表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 工位日志
		/// </summary>
		[JsonProperty, Column(Name = "log")]
		public string Log { get; set; }

		/// <summary>
		/// 工单名称
		/// </summary>
		[JsonProperty, Column(Name = "section", StringLength = 40)]
		public string Section { get; set; }

		/// <summary>
		/// 工位编号
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", StringLength = 50)]
		public string WorkshopId { get; set; }

	}

}
