using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMES.Model.report
{
    public class ReportFilters
    {
        public double NetWeightMin { get; set; } = 30;
        public double NetWeightMax { get; set; } = 52;

        /// <summary>
        ///     装箱编号
        /// </summary>
        public string PackingBoxCode { get; set; } = "";

        public string PreheaterCode { get; set; } = "";
        public string PreheaterSpec { get; set; } = "";
        public string ProductCode { get; set; } = "";
        public string ProductSpec { get; set; } = "";
        public string ProductBatchNo { get; set; } = "";
        public string UserStandardCode { get; set; } = "";
    }
}