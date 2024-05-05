using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES.Model.tbs {

	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_label_content", DisableSyncStructure = true)]
	public partial class T_label_content {

		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 客户编码
		/// </summary>
		[JsonProperty, Column(Name = "customerCode", StringLength = 40)]
		public string CustomerCode { get; set; }

		/// <summary>
		/// 物料代码
		/// </summary>
		[JsonProperty, Column(Name = "materialCode", StringLength = 40)]
		public string MaterialCode { get; set; }

		/// <summary>
		/// 线盘类型
		/// </summary>
		[JsonProperty, Column(Name = "preheaterCode", StringLength = 20)]
		public string PreheaterCode { get; set; }

		/// <summary>
		/// 打印标签模版路径
		/// </summary>
		[JsonProperty, Column(Name = "printFilePath")]
		public string PrintFilePath { get; set; }

		/// <summary>
		/// 状态：0.无效；1.有效
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; }

		/// <summary>
		/// 技术标准：是用户技术标准，还是什么，跟施部长确认
		/// </summary>
		[JsonProperty, Column(Name = "technoladge", StringLength = 40)]
		public string Technoladge { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
