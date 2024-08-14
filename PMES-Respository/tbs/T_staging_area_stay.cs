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
	/// 暂存区 - 信息组托盘表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_staging_area_stay", DisableSyncStructure = true)]
	public partial class T_staging_area_stay {

		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 产线编号
		/// </summary>
		[JsonProperty, Column(Name = "lineNo", DbType = "int")]
		public int? LineNo { get; set; }

		/// <summary>
		/// 每托盘线盘数量
		/// </summary>
		[JsonProperty, Column(Name = "preaheateNumOfPerStay", DbType = "int")]
		public int? PreaheateNumOfPerStay { get; set; } = 0;

		/// <summary>
		/// 台架编号
		/// </summary>
		[JsonProperty, Column(Name = "rackNo", StringLength = 40)]
		public string RackNo { get; set; }

		/// <summary>
		/// 垛位备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 20)]
		public string Remark { get; set; }

		/// <summary>
		/// 是否已搬运：0.入库；1.出库；2.已搬运至来料区的
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 0;

		/// <summary>
		/// 暂存区-母托盘编号
		/// </summary>
		[JsonProperty, Column(Name = "stayCode", StringLength = 40)]
		public string StayCode { get; set; }

	}

}
