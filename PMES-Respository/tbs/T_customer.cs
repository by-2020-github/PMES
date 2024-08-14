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
	/// 客户表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_customer", DisableSyncStructure = true)]
	public partial class T_customer {

		/// <summary>
		/// 客户表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 客户代码
		/// </summary>
		[JsonProperty, Column(Name = "customerCode", StringLength = 20)]
		public string CustomerCode { get; set; }

		/// <summary>
		/// 客户ID
		/// </summary>
		[JsonProperty, Column(Name = "customerId", StringLength = 40)]
		public string CustomerId { get; set; }

		/// <summary>
		/// 客户名称
		/// </summary>
		[JsonProperty, Column(Name = "customerName", StringLength = 20)]
		public string CustomerName { get; set; }

		/// <summary>
		/// 状态 1有效0无效
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 1;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
