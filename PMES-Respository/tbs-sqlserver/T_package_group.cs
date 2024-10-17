﻿using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_package_group", DisableSyncStructure = true)]
	public partial class T_package_group {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public string Code { get; set; }

		public DateTime? CreateTime { get; set; }

		public string Name { get; set; }

		public string Remark { get; set; }

		public int? Status { get; set; } = 0;

		public DateTime? UpdateTime { get; set; }

	}

}
