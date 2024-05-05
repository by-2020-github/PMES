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
	/// 箱码-关联盘内编码
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_box_releated_preheater", DisableSyncStructure = true)]
	public partial class T_box_releated_preheater {

		/// <summary>
		/// 箱盘关联表id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 箱码表-主键id
		/// </summary>
		[JsonProperty, Column(Name = "boxCodeId", StringLength = 100)]
		public string BoxCodeId { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 是否删除 1是0否
		/// </summary>
		[JsonProperty, Column(Name = "isDel", DbType = "int")]
		public int? IsDel { get; set; } = 0;

		/// <summary>
		/// 盘码表-主键id
		/// </summary>
		[JsonProperty, Column(Name = "preheaterCodeId", DbType = "int")]
		public int? PreheaterCodeId { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
