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
	/// 标签模版表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_label_template" )]
	public partial class T_label_template {

		/// <summary>
		/// 标签模版表 -- 主键ID
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
		[JsonProperty, Column(Name = "custormerCode", StringLength = 40)]
		public string CustormerCode { get; set; }

		/// <summary>
		/// 是否系统默认：1.系统默认；2.客户指定
		/// </summary>
		[JsonProperty, Column(Name = "defaultType", DbType = "int")]
		public int? DefaultType { get; set; } = 1;

		/// <summary>
		/// 0有效；1删除
		/// </summary>
		[JsonProperty, Column(Name = "isDel", DbType = "int")]
		public int? IsDel { get; set; }

		/// <summary>
		/// 标签表-主键id
		/// </summary>
		[JsonProperty, Column(Name = "labelId", DbType = "int")]
		public int? LabelId { get; set; }

		/// <summary>
		/// 线盘代码
		/// </summary>
		[JsonProperty, Column(Name = "preheaterCode", StringLength = 40)]
		public string PreheaterCode { get; set; }

		/// <summary>
		/// 标签类型：0. 盘标签；1. 箱标签；3. 发货标签
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
		/// 标签打印文件
		/// </summary>
		[JsonProperty, Column(Name = "templateFile", DbType = "blob")]
		public byte[] TemplateFile { get; set; }

		[JsonProperty, Column(Name = "templateFileName", StringLength = -1)]
		public string TemplateFileName { get; set; }

		/// <summary>
		/// 标签预览图片
		/// </summary>
		[JsonProperty, Column(Name = "templatePicture", DbType = "blob")]
		public byte[] TemplatePicture { get; set; }

		/// <summary>
		/// 记录更新时间
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
