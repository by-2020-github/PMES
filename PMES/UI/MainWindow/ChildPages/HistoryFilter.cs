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
using PMES.Core.Managers;
using Serilog;
using PMES.Core;
using PMES_Respository.tbs;

namespace PMES.UI.MainWindow.ChildPages
{
    public partial class HistoryFilter : DevExpress.XtraEditors.XtraForm
    {
        private readonly ILogger _logger;
        private readonly IFreeSql _freeSql = FreeSqlManager.FSql;

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
                PackingBoxCode = cbx_packingBoxCode.Text ?? "0",
                PreheaterCode = cbx_preheaterCode.Text ?? "",
                PreheaterSpec = cbx_preheaterSpec.Text ?? "",
                ProductCode = cbx_productCode.Text ?? "",
                ProductSpec = cbx_productSpec.Text ?? "",
                ProductBatchNo = cbx_productBatchNo.Text ?? "",
                UserStandardCode = cbx_userStandardCode.Text ?? ""
            };
        }

        private void HistoryFilter_Load(object sender, EventArgs e)
        {
            var codes = _freeSql.Select<T_preheater_code>().Where(s => s.ProductDate >= DateTime.Today).ToList();
            cbx_preheaterCode.Properties.Items.Clear();
            cbx_preheaterCode.Properties.Items.AddRange(codes.Select(s => s.PreheaterCode)
                .Where(s => !string.IsNullOrEmpty(s)).ToList());

            cbx_preheaterSpec.Properties.Items.Clear();
            cbx_preheaterSpec.Properties.Items.AddRange(codes.Select(s => s.PreheaterSpec)
                .Where(s => !string.IsNullOrEmpty(s)).ToList());

            cbx_productCode.Properties.Items.Clear();
            cbx_productCode.Properties.Items.AddRange(codes.Select(s => s.ProductCode)
                .Where(s => !string.IsNullOrEmpty(s)).ToList());

            cbx_productSpec.Properties.Items.Clear();
            cbx_productSpec.Properties.Items.AddRange(codes.Select(s => s.ProductSpec)
                .Where(s => !string.IsNullOrEmpty(s)).ToList());

            cbx_productBatchNo.Properties.Items.Clear();
            cbx_productBatchNo.Properties.Items.AddRange(codes.Select(s => s.BatchNO)
                .Where(s => !string.IsNullOrEmpty(s)).ToList());

            cbx_userStandardCode.Properties.Items.Clear();
            cbx_userStandardCode.Properties.Items.AddRange(codes.Select(s => s.UserStandardCode)
                .Where(s => !string.IsNullOrEmpty(s)).ToList());
        }
    }
}