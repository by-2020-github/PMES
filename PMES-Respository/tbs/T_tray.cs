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
	/// 母托盘表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_tray", DisableSyncStructure = true)]
	public partial class T_tray {

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
		/// 母托盘条码
		/// </summary>
		[JsonProperty, Column(Name = "plants", StringLength = 80)]
		public string Plants { get; set; }

		/// <summary>
		/// 合托状态：1.未开始；2.组托中；3.组托完成
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; }

		/// <summary>
		/// 子托盘类型
		/// </summary>
		[JsonProperty, Column(Name = "subPlants", StringLength = 80)]
		public string SubPlants { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
