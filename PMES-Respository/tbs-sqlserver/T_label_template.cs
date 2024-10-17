using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_label_template", DisableSyncStructure = true)]
	public partial class T_label_template {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? DefaultType { get; set; }

		public int? IsAutoLine { get; set; }

		public int? IsDel { get; set; }

		public int? LabelId { get; set; }

		public string PackageCode { get; set; }

		public string PackageName { get; set; }

		public string Postion { get; set; }

		public int? PrintLabelTypeId { get; set; }

		public int? Radus { get; set; }

		public string Remark { get; set; }

		public string SizeInfo { get; set; }

		public byte[] TemplateFile { get; set; }

		public string TemplateFileName { get; set; }

		public byte[] TemplatePicture { get; set; }

		public DateTime? UpdateTime { get; set; }

	}

}
