using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {
	/// <summary>
	///		通过包装代码+标签类型唯一索引到对应的标签和打印机，调用打印机去打印
	/// </summary>
	[Table(Name = "t_label_template", DisableSyncStructure = true)]
	public partial class T_label_template {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public DateTime? CreateTime { get; set; } = DateTime.Now;

        public int? IsDel { get; set; } = 0;

        /// <summary>
        ///		标签类型--->对应唯一的打印机
        /// 标签类型：
        /// 1 自动线2-&gt;盘码1
        /// 2 自动线2-&gt;盘码2
        /// 3 自动线2-&gt;盘码3
        /// 4 自动线2-&gt;顶贴
        /// 5 自动线2-&gt;侧贴
        /// 
        /// 标签类型：
        /// 21 自动线1-&gt;盘码1
        /// 22 自动线1-&gt;盘码2
        /// 23 自动线1-&gt;盘码3
        /// 24 自动线1-&gt;顶贴
        /// 25 自动线1-&gt;侧贴
        /// 
        /// 标签类型：
        /// 101 人工1-&gt;统一标签
        /// </summary>
        public int LabelType { get; set; }

		/// <summary>
		///		包装代码
		/// </summary>
        public string PackageCode { get; set; }

		public string PackageName { get; set; }

		public string Remark { get; set; }

		public byte[] TemplateFile { get; set; }

		public string TemplateFileName { get; set; }

		public byte[] TemplatePicture { get; set; }

		public DateTime? UpdateTime { get; set; } = DateTime.Now;

	}

}
