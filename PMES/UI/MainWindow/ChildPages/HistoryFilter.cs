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
                NetWeightMax = (double)spMaxNetWeight.Value,
                NetWeightMin = (double)spMinNetWeight.Value,
                PackingNumber = cbx_packingCode.Text ?? "0",
                PreheaterCode = cbx_preheaterCode.Text ?? "",
                PreheaterSpec = cbx_preheaterSpec.Text ?? "",
                ProductCode = cbx_productCode.Text ?? "",
                ProductSpec = cbx_productSpec.Text ?? "",
                ProductionBarCode = cbx_productCode.Text ?? "",
                ProductionBatchNo = cbx_productionBatchNo.Text ?? "",
                UserStandardCode = cbx_userStandardCode.Text ?? ""
            };
        }
    }
}