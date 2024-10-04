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
	/// 子托盘组表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_child_tray_group", DisableSyncStructure = true)]
	public partial class T_child_tray_group {

		/// <summary>
		/// 子母托盘表 - 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 暂存区工位编号
		/// </summary>
		[JsonProperty, Column(Name = "areaWorkshopId", DbType = "int")]
		public int? AreaWorkshopId { get; set; }

		/// <summary>
		/// 子托盘物料代码
		/// </summary>
		[JsonProperty, Column(Name = "childTrayMaterialSpecification", StringLength = 40)]
		public string ChildTrayMaterialSpecification { get; set; }

		/// <summary>
		/// 组合机器人-组托工位
		/// </summary>
		[JsonProperty, Column(Name = "combinateWorkshopId", DbType = "int")]
		public int? CombinateWorkshopId { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 母托盘编号
		/// </summary>
		[JsonProperty, Column(Name = "motherTrayBarCode", StringLength = 80)]
		public string MotherTrayBarCode { get; set; }

		/// <summary>
		/// 子托盘个数
		/// </summary>
		[JsonProperty, Column(Name = "num", DbType = "int")]
		public int? Num { get; set; }

		/// <summary>
		/// 使用状态：0.未使用；1.已使用
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 0;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
