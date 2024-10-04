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
	/// 子托盘垛型配置表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_child_tray_mode_config", DisableSyncStructure = true)]
	public partial class T_child_tray_mode_config {

		/// <summary>
		/// 子母托盘表 - 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 层数
		/// </summary>
		[JsonProperty, Column(Name = "cellNum", DbType = "int")]
		public int? CellNum { get; set; }

		/// <summary>
		/// 子托盘物料名称
		/// </summary>
		[JsonProperty, Column(Name = "childTrayName", StringLength = 80)]
		public string ChildTrayName { get; set; }

		/// <summary>
		/// 子托盘物料代码
		/// </summary>
		[JsonProperty, Column(Name = "childTraySpecification", StringLength = 40)]
		public string ChildTraySpecification { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 满托数量
		/// </summary>
		[JsonProperty, Column(Name = "fullTrayNum", DbType = "int")]
		public int? FullTrayNum { get; set; }

		/// <summary>
		/// 厚度
		/// </summary>
		[JsonProperty, Column(Name = "hight", DbType = "int")]
		public int? Hight { get; set; }

		/// <summary>
		/// 是否裸妆:1. 裸装；2.不裸装
		/// </summary>
		[JsonProperty, Column(Name = "isNaked", DbType = "int")]
		public int? IsNaked { get; set; }

		/// <summary>
		/// 长度
		/// </summary>
		[JsonProperty, Column(Name = "length", DbType = "int")]
		public int? Length { get; set; }

		/// <summary>
		/// PLC对应的类型值
		/// </summary>
		[JsonProperty, Column(Name = "plcValue", DbType = "int")]
		public int? PlcValue { get; set; }

		/// <summary>
		/// 线盘
		/// </summary>
		[JsonProperty, Column(Name = "reel", StringLength = 20)]
		public string Reel { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 20)]
		public string Remark { get; set; }

		/// <summary>
		/// 规格
		/// </summary>
		[JsonProperty, Column(Name = "sizeInfo", StringLength = 20)]
		public string SizeInfo { get; set; }

		/// <summary>
		/// 类型：1木托盘；2.其他
		/// </summary>
		[JsonProperty, Column(Name = "type", DbType = "int")]
		public int? Type { get; set; } = 1;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 宽带
		/// </summary>
		[JsonProperty, Column(Name = "width", DbType = "int")]
		public int? Width { get; set; }

	}

}
