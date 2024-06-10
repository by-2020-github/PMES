using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using PMES.Model.report;
using PMES.Core.Managers;
using PMES.Core;
using PMES_Respository.tbs;

namespace PMES.UI.Settings
{
    public partial class TemplateManage : DevExpress.XtraEditors.XtraForm
    {
        private readonly ILogger _logger;
        private readonly IFreeSql _freeSql = FreeSqlManager.FSql;
        private List<string> _boxList = new List<string>();
        private List<T_label> _tLabels = new List<T_label>();

        public TemplateManage(ILogger logger)
        {
            this._logger = logger;
            InitializeComponent();
        }

        private void TemplateManage_Load(object sender, EventArgs e)
        {
            gridViewTemplates.FocusedRowChanged -= gridViewTemplates_FocusedRowChanged;
            _tLabels = _freeSql.Select<T_label>().ToList();
            gridControlTemplates.DataSource = _tLabels;
            gridViewTemplates.FocusedRowChanged += gridViewTemplates_FocusedRowChanged;
        }

        private void gridViewTemplates_FocusedRowChanged(object sender,
            DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var row = e.FocusedRowHandle;
            if (row < 0)
            {
                return;
            }

            _tLabels.ForEach(s => s.IsCurrent = false);
            _tLabels[row].IsCurrent = true;
            gridViewTemplates.FocusedRowChanged -= gridViewTemplates_FocusedRowChanged;
            gridControlTemplates.DataSource = null;
            gridControlTemplates.DataSource = _tLabels;
            gridViewTemplates.FocusedRowHandle = row;
            gridViewTemplates.FocusedRowChanged += gridViewTemplates_FocusedRowChanged;
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (await _freeSql.Update<T_label>().SetSource(_tLabels).ExecuteAffrowsAsync() > 0)
            {
                XtraMessageBox.Show("修改成功");
            }

            GlobalVar.CurrentTemplate = $"{GlobalVar.TemplatePath}\\{_tLabels.First(s => (bool)s.IsCurrent).Name}";
            Trace.WriteLine($"current:{GlobalVar.CurrentTemplate}");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}