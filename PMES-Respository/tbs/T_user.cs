using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES.Model.tbs {

	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_user", DisableSyncStructure = true)]
	public partial class T_user {

		/// <summary>
		/// 员工id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 操作工编号
		/// </summary>
		[JsonProperty, Column(Name = "code", StringLength = 20)]
		public string Code { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 是否删除 1是0否
		/// </summary>
		[JsonProperty, Column(Name = "isDel", DbType = "int")]
		public int? IsDel { get; set; } = 1;

		/// <summary>
		/// 是否运行登录自动线：1-可；2-不可
		/// </summary>
		[JsonProperty, Column(Name = "isLoginAutoLine", DbType = "int")]
		public int? IsLoginAutoLine { get; set; }

		/// <summary>
		/// 操作工登录账号
		/// </summary>
		[JsonProperty, Column(Name = "loginUsername", StringLength = 11)]
		public string LoginUsername { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[JsonProperty, Column(Name = "password", StringLength = 20)]
		public string Password { get; set; }

		/// <summary>
		/// 状态 1有效0无效
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 1;

		/// <summary>
		/// 操作工的二维码条码
		/// </summary>
		[JsonProperty, Column(Name = "u8", StringLength = 60)]
		public string U8 { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 操作工姓名
		/// </summary>
		[JsonProperty, Column(Name = "username", StringLength = 11)]
		public string Username { get; set; }

		/// <summary>
		/// 操作工工号
		/// </summary>
		[JsonProperty, Column(Name = "workNo", StringLength = 20)]
		public string WorkNo { get; set; }

		/// <summary>
		/// 所属工段id组，id以逗号隔开
		/// </summary>
		[JsonProperty, Column(Name = "workshopIds", StringLength = 8)]
		public string WorkshopIds { get; set; }

	}

}
