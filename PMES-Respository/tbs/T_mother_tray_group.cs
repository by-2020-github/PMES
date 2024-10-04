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
	/// 母托盘组表 - 来自【从wms申请过来】放到【母托盘组缓存位】的物料
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_mother_tray_group", DisableSyncStructure = true)]
	public partial class T_mother_tray_group {

		/// <summary>
		/// 主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 母托盘组-最下面的托盘编号
		/// </summary>
		[JsonProperty, Column(Name = "motherTrayBarcode", StringLength = 20)]
		public string MotherTrayBarcode { get; set; }

		/// <summary>
		/// 托盘个数
		/// </summary>
		[JsonProperty, Column(Name = "num", StringLength = 80)]
		public string Num { get; set; }

		/// <summary>
		/// 状态：0.待用；1.已用
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 0;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 母托盘组-- 存放的缓存工位
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", StringLength = 60)]
		public string WorkshopId { get; set; }

	}

}
