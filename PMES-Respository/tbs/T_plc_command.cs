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
	/// Plc命令字
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_plc_command", DisableSyncStructure = true)]
	public partial class T_plc_command {

		/// <summary>
		/// agv表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "attInfo", StringLength = 80)]
		public string AttInfo { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// json格式的命令内容
		/// </summary>
		[JsonProperty, Column(Name = "plcComandContent")]
		public string PlcComandContent { get; set; }

		/// <summary>
		/// 命令类型：1.拆垛；2.码垛、3.组合子母托
		/// </summary>
		[JsonProperty, Column(Name = "plcComandType", DbType = "int")]
		public int? PlcComandType { get; set; }

		/// <summary>
		/// 是否执行完成0：未执行，1：执行完成 ， 2：执行中，3：暂停
		/// </summary>
		[JsonProperty, Column(Name = "status", DbType = "int")]
		public int? Status { get; set; } = 1;

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
