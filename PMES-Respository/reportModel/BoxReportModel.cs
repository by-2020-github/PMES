﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMES_Respository.reportModel
{
    public class BoxReportModel
    {
        public string MaterialNo { get; set; } = "152131054";

        /// <summary>
        ///     型号
        /// </summary>
        public string Model { get; set; } = "1PEW/155";

        /// <summary>
        ///     规格
        /// </summary>
        public string Specifications { get; set; } = "1PEW/155";


        public string NetWeight { get; set; } = "49.03 kg";

        /// <summary>
        ///     批号
        /// </summary>
        public string BatchNum { get; set; } = "G23100061-15A";

        /// <summary>
        ///     编号
        /// </summary>
        public string No { get; set; } = "57001 - W03";

        /// <summary>
        ///     执行标准
        /// </summary>
        public string Standard { get; set; } = "GB/T6109.2-2008";

        /// <summary>
        ///     产品代码：21.T.Y.41.2.1050 --> TY4121050
        /// </summary>
        public string ProductNo { get; set; } = "TY4121050";

        /// <summary>
        ///     箱码
        /// </summary>
        public string BoxCode { get; set; } = "";

        public string DateTime { get; set; } = System.DateTime.Now.ToString("YY-MM-dd");


        public string GrossWeight { get; set; }

        public string WaterMark { get; set; }
    }
}
