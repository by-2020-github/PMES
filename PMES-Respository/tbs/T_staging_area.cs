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
	/// 待包装成品物料管理
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_staging_area", DisableSyncStructure = true)]
	public partial class T_staging_area {

		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 记录建立时间
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 是否空虚：1.未完成；2.已完成
		/// </summary>
		[JsonProperty, Column(Name = "isFinished", DbType = "int unsigned")]
		public uint? IsFinished { get; set; } = 1;

		/// <summary>
		/// 盘上的人工条码
		/// </summary>
		[JsonProperty, Column(Name = "operatorU8", StringLength = 40)]
		public string OperatorU8 { get; set; }

		/// <summary>
		/// 调度AGV时的任务编号
		/// </summary>
		[JsonProperty, Column(Name = "pmesAgvTaskCode", StringLength = 100)]
		public string PmesAgvTaskCode { get; set; }

		/// <summary>
		/// 托盘上线盘个数
		/// </summary>
		[JsonProperty, Column(Name = "reelNum", DbType = "int")]
		public int? ReelNum { get; set; }

		/// <summary>
		/// 线盘类型
		/// </summary>
		[JsonProperty, Column(Name = "reelSpecification", StringLength = 40)]
		public string ReelSpecification { get; set; }

		/// <summary>
		/// 记录更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 扩展使用--虚拟母托盘编号：生成规则：VTY开头+日期+序列号（主键ID）
		/// </summary>
		[JsonProperty, Column(Name = "virtualMotherStayCode", StringLength = 40)]
		public string VirtualMotherStayCode { get; set; }

		/// <summary>
		/// 接驳位站台编号
		/// </summary>
		[JsonProperty, Column(Name = "workshopId", StringLength = 40)]
		public string WorkshopId { get; set; }

	}

}
