using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMES.Model
{
    public class Certificate
    {
        public string Title { get; set; } = "先登高科产品合格证";
        public string MaterialNo { get; set; } = "152131054";

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; } = "1PEW/155";


        /// <summary>
        ///     规格 F3
        /// </summary>
        public string Specifications { get; set; } = "1.050";

        public string GrossWeight { get; set; } = "51.28 kg";

        /// <summary>
        /// F2
        /// </summary>
        public string NetWeight { get; set; } = "49.03 kg";

        public string BatchNum { get; set; } = "G231006611 15A";

        public string No { get; set; } = "11-57001";
        public string DateTime { get; set; } = System.DateTime.Now.ToString("YY-MM-dd");
    }
}