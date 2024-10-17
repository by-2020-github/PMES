using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(DisableSyncStructure = true)]
	public partial class P_AutoInput {

		public string Execresult { get; set; }

		public string LinkStacklabel { get; set; }

	}

}
