using PMES.Core.Managers;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PMES.Model.tbs;
using SICD_Automatic.Core;

namespace PMES.UI.Report
{
    public partial class NewLabelInput : Form
    {
        private readonly ILogger _logger;
        private readonly IFreeSql _freeSql = FreeSqlManager.FSql;

        public NewLabelInput()
        {
            InitializeComponent();
            //btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            //this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void NewLabelInput_Load(object sender, EventArgs e)
        {
            var preCode = _freeSql.Select<T_preheater_code>().Where(s => s.IsDel != 1).ToList(a => a.PreheaterCode)
                .Where(s => !string.IsNullOrEmpty(s)).ToList();
            var productCode = _freeSql.Select<T_preheater_code>().Where(s => s.IsDel != 1).ToList(a => a.ProductCode)
                .Where(s => !string.IsNullOrEmpty(s)).ToList();
            var userStandardCode = _freeSql.Select<T_preheater_code>().Where(s => s.IsDel != 1)
                .ToList(a => a.UserStandardCode).Where(s => !string.IsNullOrEmpty(s)).ToList();
            var customerCode = _freeSql.Select<T_preheater_code>().Where(s => s.IsDel != 1).ToList(a => a.CustomerCode)
                .Where(s => !string.IsNullOrEmpty(s)).ToList();

            var labelNames = _freeSql.Select<T_label>().Where(s => s.IsDel != 1).ToList(a => a.Name)
                .Where(s => !string.IsNullOrEmpty(s)).ToList();
            cbxPreCode.Properties.Items.AddRange(preCode);
            cbxProductCode.Properties.Items.AddRange(productCode);
            cbxStandCode.Properties.Items.AddRange(userStandardCode);
            cbxCustomerCode.Properties.Items.AddRange(customerCode);
            cbxLabelName.Properties.Items.AddRange(labelNames);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbxLabelName.Text))
            {
                XtraMessageBox.Show("请先输入标签的名字！","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            var label = _freeSql.Select<T_label>().Where(s => s.Name == cbxLabelName.Text).First();
            var id = 0;
            if (label == null)
            {
                label = new T_label
                {
                    CreateTime = DateTime.Now,
                    IsCurrent = false,
                    IsDel = 0,
                    Name = cbxLabelName.Text,
                    NumOfPackedItems = "1",
                    Remark = "",
                    UpdateTime = DateTime.Now
                };
                id = (int)_freeSql.Insert<T_label>(label).ExecuteIdentity();
            }
            else
            {
                id = (int)label.Id;
            }
            GlobalVar.NewLabelTemplate = new T_label_template
            {
                CreateTime = DateTime.Now,
                CustormerCode = cbxCustomerCode.Text,
                DefaultType = cbxIsDefault.SelectedIndex == 1 ? 2 : 1,
                IsDel = 0,
                PreheaterCode = cbxPreCode.Text,
                PrintLabelType = cbxType.SelectedIndex < 0 ? 0 : cbxType.SelectedIndex,
                ProductCode = cbxProductCode.Text,
                Remark = cbxRemark.Text,
                UpdateTime = DateTime.Now,
                UserStandardCode = cbxStandCode.Text,
                LabelId = id
            };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}