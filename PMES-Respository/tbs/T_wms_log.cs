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
	/// WMS调度日志表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_wms_log", DisableSyncStructure = true)]
	public partial class T_wms_log {

		/// <summary>
		/// agv日志表 -- 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 日志类型
		/// </summary>
		[JsonProperty, Column(Name = "pmes")]
		public string Pmes { get; set; }

		/// <summary>
		/// 日志类型
		/// </summary>
		[JsonProperty, Column(Name = "pmesContext")]
		public string PmesContext { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		[JsonProperty, Column(Name = "wms")]
		public string Wms { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		[JsonProperty, Column(Name = "wmsContext")]
		public string WmsContext { get; set; }

	}

}
