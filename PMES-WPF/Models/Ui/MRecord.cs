using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.Mouse;

namespace PMES_WPF.Models.Ui
{
    public class MRecord
    {
        public string Code { get; set; }
        public string ProductCode { get; set; }
        public string GrossWeight1 { get; set; }
        public string GrossWeight2 { get; set; }
        public string AvgWeight { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}