using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace PMES.UC.reports
{
    public partial class ReelReportAuto : DevExpress.XtraReports.UI.XtraReport
    {
        public ReelReportAuto()
        {
            InitializeComponent();
        }

        public void SetQCR(bool vis) {
            xrBarCode1.Visible = vis;
            xrTableInfo.Visible = vis;
            xrLabelDate.Visible = !vis;
        }

    }
}
