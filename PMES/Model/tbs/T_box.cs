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
	/// 箱码表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_box", DisableSyncStructure = true)]
	public partial class T_box {

		/// <summary>
		/// 箱码表主键ID
		/// </summary>
		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 包装日期;取写入至数据库的日期和时间，精确到秒
		/// </summary>
		[JsonProperty, Column(Name = "createTime", DbType = "datetime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 是否删除：0未删除；1.删除
		/// </summary>
		[JsonProperty, Column(Name = "isDel", DbType = "int")]
		public int? IsDel { get; set; } = 0;

		/// <summary>
		/// 标签ID
		/// </summary>
		[JsonProperty, Column(Name = "labelId", DbType = "int")]
		public int? LabelId { get; set; }

		/// <summary>
		/// 标签名称
		/// </summary>
		[JsonProperty, Column(Name = "labelName", StringLength = 20)]
		public string LabelName { get; set; }

		/// <summary>
		/// 包装组编号：PMES系统自定义---几号包装线或人工包装线
		/// </summary>
		[JsonProperty, Column(Name = "packagingCode", StringLength = 50)]
		public string PackagingCode { get; set; }

		/// <summary>
		/// 包装箱编号：PMES系统自定义：包装组编号+四位流水线号
		/// </summary>
		[JsonProperty, Column(Name = "packagingSN", StringLength = 50)]
		public string PackagingSN { get; set; }

		/// <summary>
		/// 包装组名称，PMES系统自定义
		/// </summary>
		[JsonProperty, Column(Name = "packagingWorker", StringLength = 50)]
		public string PackagingWorker { get; set; }

		/// <summary>
		/// 包装条码；产品助记码+线盘分组代码+用户标准代码+包装组编号+年月+4位流水号+装箱净重，如TY4121050-14-BZ001-B12310001-04903
		/// </summary>
		[JsonProperty, Column(Name = "packingBarCode", StringLength = 50)]
		public string PackingBarCode { get; set; }

		/// <summary>
		/// 装箱件数
		/// </summary>
		[JsonProperty, Column(Name = "packingQty", StringLength = 50)]
		public string PackingQty { get; set; }

		/// <summary>
		/// 装箱净重
		/// </summary>
		[JsonProperty, Column(Name = "packingWeight", DbType = "decimal(18,2)")]
		public decimal? PackingWeight { get; set; }

		/// <summary>
		/// 母托盘条码
		/// </summary>
		[JsonProperty, Column(Name = "trayBarcode", StringLength = 50)]
		public string TrayBarcode { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		[JsonProperty, Column(Name = "updateTime", DbType = "datetime")]
		public DateTime? UpdateTime { get; set; }

	}

}
