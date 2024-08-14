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
	/// 子母盘合托表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_tray_wms", DisableSyncStructure = true)]
	public partial class T_tray_wms {

		/// <summary>
		/// 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 母托盘条码ID
		/// </summary>
		[JsonProperty, Column(Name = "plantsId", StringLength = 80)]
		public string PlantsId { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 称重条码
		/// </summary>
		[JsonProperty, Column(Name = "weightBarCode", StringLength = 80)]
		public string WeightBarCode { get; set; }

	}

}
