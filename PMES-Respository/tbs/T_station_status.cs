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
	/// 工位管理表，用于【拆盘工位】、【码垛工位】等位置的工位
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_station_status", DisableSyncStructure = true)]
	public partial class T_station_status {

		/// <summary>
		/// 工位编号表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 附加消息json格式：1.码垛消息：码垛层数，每层最大数量；当前数量
		/// </summary>
		[JsonProperty, Column(Name = "attachInfo", StringLength = -1)]
		public string AttachInfo { get; set; }

		/// <summary>
		/// 工位编号的条形码标签
		/// </summary>
		[JsonProperty, Column(Name = "barCode", StringLength = 50)]
		public string BarCode { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 推送是否成功：1.成功；0.失败
		/// </summary>
		[JsonProperty, Column(Name = "isNotifySuccess", DbType = "int")]
		public int? IsNotifySuccess { get; set; }

		/// <summary>
		/// 所属产线
		/// </summary>
		[JsonProperty, Column(Name = "lineId", DbType = "int unsigned")]
		public uint? LineId { get; set; }

		/// <summary>
		/// 备注:
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 60)]
		public string Remark { get; set; }

		/// <summary>
		/// 工位状态：0. 可用; 1.物料占用；2.空闲; 3.锁定；4.维护；5.暂停关闭
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 1;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 所属工段ID
		/// </summary>
		[JsonProperty, Column(Name = "worksection", DbType = "int")]
		public int? Worksection { get; set; }

		/// <summary>
		/// 工位编号
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", StringLength = 50)]
		public string WorkshopId { get; set; }

	}

}
