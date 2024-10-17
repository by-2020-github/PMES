using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver {

	[Table(Name = "t_wms_log", DisableSyncStructure = true)]
	public partial class T_wms_log {

        /// <summary>
        /// wms接口调用日志
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// pmes日志
		/// </summary>
		public string Pmes { get; set; }

		/// <summary>
		/// 日志内容
		/// </summary>
		public string PmesContext { get; set; }

		/// <summary>
		/// 调用状态：1.pmes下发成功；2.wms反馈成功
		/// </summary>
		public int? Status { get; set; }

		/// <summary>
		/// 接口类型：1.申请入库；2.保存入库；3.辅料入库
		/// </summary>
		public int? Type { get; set; }

		/// <summary>
		/// wms调用
		/// </summary>
		public string Wms { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		public string WmsContext { get; set; }

	}

}
