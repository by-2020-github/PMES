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
using SICD_Automatic.Core;
using PMES.Model.report;

namespace PMES.UI.Settings
{
    public partial class TemplateManage : DevExpress.XtraEditors.XtraForm
    {
        private Serilog.ILogger logger;
        private List<string> _boxList = new List<string>();
        private List<TemplateManagement> _templateManages = new List<TemplateManagement>();
        public TemplateManage(ILogger logger)
        {
            this.logger = logger;
            InitializeComponent();
        }

        private void TemplateManage_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(GlobalVar.TemplatePath);
            foreach (var file in files)
            {
                if (!file.EndsWith(".repx") || file.Contains("box")) continue;
                _boxList.Add(file);
                _templateManages.Add(new TemplateManagement()
                {
                    Name = Path.GetFileName(file)
                });
            }
    
            gridControlTemplates.DataSource = _templateManages;

        }

        private void gridViewTemplates_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var row = e.FocusedRowHandle;
            if (row < 0)
            {
                return;
            }
            _templateManages.ForEach(s => s.Enable = false);
            _templateManages[row].Enable = true;
            gridViewTemplates.FocusedRowChanged -= gridViewTemplates_FocusedRowChanged;
            gridControlTemplates.DataSource = null;
            gridControlTemplates.DataSource = _templateManages;
            gridViewTemplates.FocusedRowHandle = row;
            gridViewTemplates.FocusedRowChanged += gridViewTemplates_FocusedRowChanged;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            GlobalVar.CurrentTemplate = $"{GlobalVar.TemplatePath}\\{_templateManages.First(s=>s.Enable).Name}";
            Trace.WriteLine($"current:{GlobalVar.CurrentTemplate}");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}