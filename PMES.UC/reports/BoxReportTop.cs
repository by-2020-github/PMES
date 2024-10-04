using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace PMES.UC.reports
{
    public partial class BoxReportTop : DevExpress.XtraReports.UI.XtraReport
    {
        public BoxReportTop()
        {
            InitializeComponent();
        }
        public void SetQrCode(string code) {
         this.xrBarCode1.Text = code;
        }
    }
}
