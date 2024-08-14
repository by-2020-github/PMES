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
	/// 产线物料表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_line_material", DisableSyncStructure = true)]
	public partial class T_line_material {

		/// <summary>
		/// 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 物料编码
		/// </summary>
		[JsonProperty, Column(Name = "code", StringLength = 40)]
		public string Code { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 是否裸装：1. 否；2.是
		/// </summary>
		[JsonProperty, Column(Name = "isNaked", DbType = "int")]
		public int? IsNaked { get; set; } = 1;

		/// <summary>
		/// 产线id
		/// </summary>
		[JsonProperty, Column(Name = "lineNo", DbType = "int")]
		public int? LineNo { get; set; }

		/// <summary>
		/// 物料名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 20)]
		public string Name { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
