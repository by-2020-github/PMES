using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMES.Model.report
{
    public class ReportFilters
    {
        public string netWeight { get; set; } = "52";
        public string packingNumber { get; set; } = "";
        public string preheaterCode { get; set; } = "";
        public string preheaterSpec { get; set; } = "";
        public string productCode { get; set; } = "";
        public string productSpec { get; set; } = "";
        public string productionBarCode { get; set; } = "";
        public string productionBatchNo { get; set; } = "";
        public string productionDate { get; set; } = "";
        public string userStandardCode { get; set; } = "";
    }
}
