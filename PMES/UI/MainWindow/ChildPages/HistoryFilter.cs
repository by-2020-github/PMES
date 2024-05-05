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
using PMES.Model.report;
using SICD_Automatic.Core;

namespace PMES.UI.MainWindow.ChildPages
{
    public partial class HistoryFilter : DevExpress.XtraEditors.XtraForm
    {
        public HistoryFilter()
        {
            InitializeComponent();
            btnQuery.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            this.AcceptButton = btnQuery;
            this.CancelButton = btnCancel;
        }

        private void Cancel(object sender, EventArgs e)
        {
        }

        private void Query(object sender, EventArgs e)
        {
            GlobalVar.ReportFilters = new ReportFilters
            {
                netWeight = cbx_minNetWeight.Text ?? "",
                packingNumber = cbx_packingNumber.Text ?? "",
                preheaterCode = cbx_preheaterCode.Text ?? "",
                preheaterSpec = cbx_preheaterSpec.Text ?? "",
                productCode = cbx_productCode.Text ?? "",
                productSpec = cbx_productSpec.Text ?? "",
                productionBarCode = cbx_productCode.Text ?? "",
                productionBatchNo = cbx_productionBatchNo.Text ?? "",
                productionDate = DateTime.Now.ToString("yyyy-MM-dd"),
                userStandardCode = cbx_userStandardCode.Text ?? ""
            };
        }
    }
}