using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	/// <summary>
	/// 工位
	/// </summary>
	[Table(Name = "t_station", DisableSyncStructure = true)]
	public partial class T_station {

        [Column(IsIdentity = true, IsPrimary = true)] public int Id { get; set; }

	}

}
