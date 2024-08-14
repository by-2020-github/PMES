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
	/// 标签表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_label", DisableSyncStructure = true)]
	public partial class T_label {

		/// <summary>
		/// 标签表主键ID
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 颜色
		/// </summary>
		[JsonProperty, Column(Name = "color", StringLength = 20)]
		public string Color { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 0有效；1删除
		/// </summary>
		[JsonProperty, Column(Name = "isDel", DbType = "int")]
		public int? IsDel { get; set; }

		/// <summary>
		/// 是否是人工线：1.人工；2.自动线
		/// </summary>
		[JsonProperty, Column(Name = "isManual", DbType = "int")]
		public int? IsManual { get; set; }

		/// <summary>
		/// 打印模版表Id
		/// </summary>
		[JsonProperty, Column(Name = "lableTemplateId", DbType = "int")]
		public int? LableTemplateId { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 20)]
		public string Name { get; set; }

		/// <summary>
		/// 装箱件数
		/// </summary>
		[JsonProperty, Column(Name = "numOfPackedItems")]
		public string NumOfPackedItems { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 100)]
		public string Remark { get; set; }

		/// <summary>
		/// 尺寸
		/// </summary>
		[JsonProperty, Column(Name = "size", StringLength = 80)]
		public string Size { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
