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
	/// Agv表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_agv", DisableSyncStructure = true)]
	public partial class T_agv {

		/// <summary>
		/// agv表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 出发地
		/// </summary>
		[JsonProperty, Column(Name = "from", DbType = "int unsigned")]
		public uint? From { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[JsonProperty, Column(Name = "name", StringLength = 20)]
		public string Name { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 80)]
		public string Remark { get; set; }

		/// <summary>
		/// 目的地
		/// </summary>
		[JsonProperty, Column(Name = "to", DbType = "int")]
		public int? To { get; set; } = 1;

		/// <summary>
		/// 工段条码标签， 可用于操作工扫码条码识别工段的功能
		/// </summary>
		[JsonProperty, Column(Name = "workshopBarCode", StringLength = 50)]
		public string WorkshopBarCode { get; set; }

	}

}
