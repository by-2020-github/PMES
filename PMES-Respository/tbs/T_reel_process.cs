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
	/// 线盘包装处理进程表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_reel_process", DisableSyncStructure = true)]
	public partial class T_reel_process {

		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 人工标签
		/// </summary>
		[JsonProperty, Column(Name = "barcode", StringLength = 80)]
		public string Barcode { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 当前设备命令值
		/// </summary>
		[JsonProperty, Column(Name = "deviceCmdContent")]
		public string DeviceCmdContent { get; set; }

		/// <summary>
		/// 处理到的当前设备号
		/// </summary>
		[JsonProperty, Column(Name = "deviceNo", StringLength = 10)]
		public string DeviceNo { get; set; }

		/// <summary>
		/// 当前设备命令值
		/// </summary>
		[JsonProperty, Column(Name = "plcCmdContent")]
		public string PlcCmdContent { get; set; }

		/// <summary>
		/// 线盘类型
		/// </summary>
		[JsonProperty, Column(Name = "reelType", StringLength = 10)]
		public string ReelType { get; set; }

		/// <summary>
		/// 状态：合格；不合格
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; }

	}

}
