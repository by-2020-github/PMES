using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_order_exchange", DisableSyncStructure = true)]
	public partial class T_order_exchange {

        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		public string Newcode { get; set; }

		public string Oldcode { get; set; }

		public int? WeightUserId { get; set; }

	}

}
