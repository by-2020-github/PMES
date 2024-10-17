using System;
using FreeSql.DataAnnotations;

namespace PMES_Respository.tbs_sqlserver
{
    [Table(Name = "t_preheater_code", DisableSyncStructure = true)]
    public partial class T_preheater_code
    {
        [Column(IsIdentity = true, IsPrimary = true)] public long Id { get; set; }

        public string BatchNO { get; set; }

        public int BoxId { get; set; }

        public DateTime? CreateTime { get; set; }

        public string CustomerCode { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerMaterialCode { get; set; }

        public string CustomerMaterialName { get; set; }

        public string CustomerMaterialSpec { get; set; }

        public string CustomerName { get; set; }

        public double GrossWeight { get; set; }

        public string ICMOBillNO { get; set; }

        public int? IsDel { get; set; }

        public bool IsQualified { get; set; }

        public string Jsbz_short_name { get; set; }

        public int? LabelTemplateId { get; set; }

        public string MachineCode { get; set; }

        public int? MachineId { get; set; }

        public string MachineName { get; set; }

        public string Material_spec { get; set; }

        public string Material_themal_grade { get; set; }

        public double NetWeight { get; set; }

        public string NoQualifiedReason { get; set; }

        public string OperatorCode { get; set; }

        public string OperatorName { get; set; }

        public string PreheaterCode { get; set; }

        public int PreheaterId { get; set; }

        public string PreheaterName { get; set; }

        public string PreheaterSpec { get; set; }

        public double PreheaterWeight { get; set; }

        public string ProductCode { get; set; }

        public DateTime ProductDate { get; set; }

        public string ProductGBName { get; set; }

        public int? ProductId { get; set; }

        public string ProductionBarcode { get; set; }

        public string ProductionOrgNO { get; set; }

        public string ProductMnemonicCode { get; set; }

        public string ProductName { get; set; }

        public string ProductSpec { get; set; }

        public string ProductStandardName { get; set; }

        public string PSN { get; set; }

        public long Status { get; set; }

        public string StockCode { get; set; }

        public int? StockId { get; set; }

        public string StockName { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UserStandardCode { get; set; }

        public int? UserStandardId { get; set; }

        public string UserStandardName { get; set; }

        public double Weight1 { get; set; }

        public double Weight2 { get; set; }

        public int? WeightUserId { get; set; }

        [Column(IsIgnore = true)] public string VirtualBoxCode { get; set; }
    }
}