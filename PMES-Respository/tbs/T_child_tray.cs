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
	/// 子母托盘表， 组盘时，扫码读取母托盘的记录
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_child_tray", DisableSyncStructure = true)]
	public partial class T_child_tray {

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
		/// 子托盘垛型, 预留位
		/// </summary>
		[JsonProperty, Column(Name = "childrenTrayMode", DbType = "int unsigned")]
		public uint? ChildrenTrayMode { get; set; }

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
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 80)]
		public string Remark { get; set; }

		/// <summary>
		/// 码垛区工位，实际工作点
		/// </summary>
		[JsonProperty, Column(Name = "stackingWorkshopId", DbType = "int")]
		public int? StackingWorkshopId { get; set; }

		/// <summary>
		/// 使用状态：0.未使用；1.已使用
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
