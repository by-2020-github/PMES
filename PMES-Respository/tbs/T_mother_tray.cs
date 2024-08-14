﻿using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES.Model.tbs {

	/// <summary>
	/// 空托盘组表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_mother_tray", DisableSyncStructure = true)]
	public partial class T_mother_tray {

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
		/// 是否是子托盘：1.是；2.是母托盘
		/// </summary>
		[JsonProperty, Column(Name = "isChildOrMotherTray", DbType = "int")]
		public int? IsChildOrMotherTray { get; set; }

		/// <summary>
		/// 托盘个数
		/// </summary>
		[JsonProperty, Column(Name = "num", StringLength = 80)]
		public string Num { get; set; }

		/// <summary>
		/// 合托状态：1.未开始；2.组托中；3.组托完成
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 1;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 母托盘存放的缓存工位
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", StringLength = 60)]
		public string WorkshopId { get; set; }

	}

}