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
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_print", DisableSyncStructure = true)]
	public partial class T_print {

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
		/// 打印机IP
		/// </summary>
		[JsonProperty, Column(Name = "ip", StringLength = 80)]
		public string Ip { get; set; }

		/// <summary>
		/// 打印机名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 80)]
		public string Name { get; set; }

		/// <summary>
		/// 打印机备注
		/// </summary>
		[JsonProperty, Column(Name = "remark")]
		public string Remark { get; set; }

		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
