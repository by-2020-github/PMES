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
	/// 工段管理
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_work_section", DisableSyncStructure = true)]
	public partial class T_work_section {

		/// <summary>
		/// 工段表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 是否空闲：1.空闲；2.占用
		/// </summary>
		[JsonProperty, Column(Name = "isWorking", DbType = "int")]
		public int? IsWorking { get; set; } = 1;

		/// <summary>
		/// 所属产线
		/// </summary>
		[JsonProperty, Column(Name = "lineId", DbType = "int unsigned")]
		public uint? LineId { get; set; }

		/// <summary>
		/// 工段名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 20)]
		public string Name { get; set; }

		/// <summary>
		/// 工段备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 80)]
		public string Remark { get; set; }

		/// <summary>
		/// 工段条码标签， 可用于操作工扫码条码识别工段的功能
		/// </summary>
		[JsonProperty, Column(Name = "workshopBarCode", StringLength = 50)]
		public string WorkshopBarCode { get; set; }

	}

}
