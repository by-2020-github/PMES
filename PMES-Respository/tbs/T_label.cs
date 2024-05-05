using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace PMES.Model.tbs {

	[JsonObject(MemberSerialization.OptIn), Table(Name = "t_label", DisableSyncStructure = true)]
	public partial class T_label {

		[JsonProperty, Column(Name = "id", DbType = "int unsigned", IsPrimary = true, IsIdentity = true)]
		public uint Id { get; set; }

		/// <summary>
		/// 包装代码
		/// </summary>
		[JsonProperty, Column(Name = "labelInCode", StringLength = 20)]
		public string LabelInCode { get; set; }

		/// <summary>
		/// 内包装物料名称
		/// </summary>
		[JsonProperty, Column(Name = "materialName", StringLength = 40)]
		public string MaterialName { get; set; }

		/// <summary>
		/// 内包装物料规格
		/// </summary>
		[JsonProperty, Column(Name = "materiaSku", StringLength = 40)]
		public string MateriaSku { get; set; }

		/// <summary>
		/// 内包装物料示意图
		/// </summary>
		[JsonProperty, Column(Name = "picture", StringLength = 200)]
		public string Picture { get; set; }

		/// <summary>
		/// 要求描述
		/// </summary>
		[JsonProperty, Column(Name = "remark", StringLength = 100)]
		public string Remark { get; set; }

	}

}
