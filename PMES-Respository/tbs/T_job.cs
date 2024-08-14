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
	/// 作业进度表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_job", DisableSyncStructure = true)]
	public partial class T_job {

		/// <summary>
		/// 表主键id
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 起点位置编号, 此字段可不用，因为下任务者，不知道由哪个工位进行搬运..., 可为0，则表示-来自人工下命令，机器人搬运出发点，可由agv调度操作者自行负责
		/// </summary>
		[JsonProperty, Column(Name = "fromCode", StringLength = 20)]
		public string FromCode { get; set; }

		/// <summary>
		/// 任务发起者操作工Id, 为0，则为自动线的agv驱动调度
		/// </summary>
		[JsonProperty, Column(Name = "fromUserId", DbType = "int")]
		public int? FromUserId { get; set; }

		/// <summary>
		/// 任务发起工段ID
		/// </summary>
		[JsonProperty, Column(Name = "fromWorkstationId", DbType = "int")]
		public int? FromWorkstationId { get; set; }

		/// <summary>
		/// 工作是否完成：false-未完成；true-已完成
		/// </summary>
		[JsonProperty, Column(Name = "isFinished", DbType = "tinyint(1)")]
		public sbyte? IsFinished { get; set; }

		/// <summary>
		/// 作业ID
		/// </summary>
		[JsonProperty, Column(Name = "jobId", DbType = "int")]
		public int? JobId { get; set; }

		/// <summary>
		/// 作业类型：1. 待包装区任务；2.来料区任务
		/// </summary>
		[JsonProperty, Column(Name = "jobType", DbType = "int")]
		public int? JobType { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 80)]
		public string Remark { get; set; }

		/// <summary>
		/// 作业发起时间
		/// </summary>
		[JsonProperty, Column(Name = "requestTime", DbType = "datetime")]
		public DateTime? RequestTime { get; set; }

		/// <summary>
		/// 使用终端：1.自动线上位机; 2.人工线pda;3.pc后台管理系统
		/// </summary>
		[JsonProperty, Column(Name = "source", DbType = "int")]
		public int? Source { get; set; }

		/// <summary>
		/// 目的地位置编号
		/// </summary>
		[JsonProperty, Column(Name = "toCode", StringLength = 20)]
		public string ToCode { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 作业所属工段
		/// </summary>
		[JsonProperty, Column(Name = "workstationId", DbType = "int")]
		public int? WorkstationId { get; set; }

	}

}
