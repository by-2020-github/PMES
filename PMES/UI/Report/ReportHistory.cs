using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMES.UI.Report
{
    public partial class ReportHistory : DevExpress.XtraEditors.XtraForm
    {
        public ReportHistory()
        {
            InitializeComponent();
        }

        private void ReportHistory_Load(object sender, EventArgs e)
        {
            gridViewReport.Columns.ForEach(s=>s.AppearanceHeader.Font = new Font(FontFamily.GenericMonospace,10,FontStyle.Bold) );
        }
    }
}