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
	/// 打印标签表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_print_label", DisableSyncStructure = true)]
	public partial class T_print_label {

		/// <summary>
		/// 标签定义表主键ID
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 标签建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 客户代码
		/// </summary>
		[JsonProperty, Column(Name = "custormerCode", StringLength = 40)]
		public string CustormerCode { get; set; }

		/// <summary>
		/// 是否系统默认：1.系统默认；2.客户指定
		/// </summary>
		[JsonProperty, Column(Name = "defaultType", DbType = "int")]
		public int? DefaultType { get; set; }

		/// <summary>
		/// 标签名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 20)]
		public string Name { get; set; }

		/// <summary>
		/// 装箱件数
		/// </summary>
		[JsonProperty, Column(Name = "numOfPackedItems")]
		public string NumOfPackedItems { get; set; }

		/// <summary>
		/// 线盘代码
		/// </summary>
		[JsonProperty, Column(Name = "preheaterCode", StringLength = 40)]
		public string PreheaterCode { get; set; }

		/// <summary>
		/// 标签类型：0. 盘内标签；1. 盘外标签；3. 发货标签
		/// </summary>
		[JsonProperty, Column(Name = "printLabelType", DbType = "int")]
		public int? PrintLabelType { get; set; }

		/// <summary>
		/// 产品代码
		/// </summary>
		[JsonProperty, Column(Name = "productCode", StringLength = 40)]
		public string ProductCode { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 100)]
		public string Remark { get; set; }

		/// <summary>
		/// 状态 0无效；1有效
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; }

		/// <summary>
		/// 标签文件
		/// </summary>
		[JsonProperty, Column(Name = "templateFile", DbType = "blob")]
		public byte[] TemplateFile { get; set; }

		/// <summary>
		/// 标签更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 技术标准代码
		/// </summary>
		[JsonProperty, Column(Name = "userStandardCode", StringLength = 40)]
		public string UserStandardCode { get; set; }

	}

}
