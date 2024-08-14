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
	/// 工位管理
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_station", DisableSyncStructure = true)]
	public partial class T_station {

		/// <summary>
		/// 工位编号表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

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
		/// 设备条码标签
		/// </summary>
		[JsonProperty, Column(Name = "deviceBarcode", StringLength = 40)]
		public string DeviceBarcode { get; set; }

		/// <summary>
		/// 设备实际编号（工艺平面图上的编号）
		/// </summary>
		[JsonProperty, Column(Name = "deviceCode", StringLength = 20)]
		public string DeviceCode { get; set; }

		/// <summary>
		/// 对应的硬件设备名称
		/// </summary>
		[JsonProperty, Column(Name = "deviceName", StringLength = 20)]
		public string DeviceName { get; set; }

		/// <summary>
		/// 设备虚拟编号
		/// </summary>
		[JsonProperty, Column(Name = "deviceVirtualNumber", StringLength = 40)]
		public string DeviceVirtualNumber { get; set; }

		/// <summary>
		/// 工位作用指定
		/// </summary>
		[JsonProperty, Column(Name = "function", StringLength = 80)]
		public string Function { get; set; }

		/// <summary>
		/// 工作状态：1.占用；2.空闲; 0.其他
		/// </summary>
		[JsonProperty, Column(Name = "isWorking", DbType = "int")]
		public int? IsWorking { get; set; } = 1;

		/// <summary>
		/// 所属产线
		/// </summary>
		[JsonProperty, Column(Name = "lineId", DbType = "int unsigned")]
		public uint? LineId { get; set; }

		/// <summary>
		/// 工位名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 20)]
		public string Name { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 60)]
		public string Remark { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 所属工段ID
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", DbType = "int")]
		public int? WorkshopId { get; set; }

	}

}
