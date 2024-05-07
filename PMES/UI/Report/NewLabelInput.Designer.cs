namespace PMES.UI.Report
{
    partial class NewLabelInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            btnOk = new DevExpress.XtraEditors.SimpleButton();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            cbxPreCode = new DevExpress.XtraEditors.ComboBoxEdit();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            lb6 = new DevExpress.XtraEditors.LabelControl();
            lb2 = new DevExpress.XtraEditors.LabelControl();
            lb3 = new DevExpress.XtraEditors.LabelControl();
            lb4 = new DevExpress.XtraEditors.LabelControl();
            lb5 = new DevExpress.XtraEditors.LabelControl();
            lb7 = new DevExpress.XtraEditors.LabelControl();
            cbxProductCode = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxStandCode = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxCustomerCode = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxRemark = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxType = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxIsDefault = new DevExpress.XtraEditors.ComboBoxEdit();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            cbxLabelName = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)tablePanel1).BeginInit();
            tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)stackPanel1).BeginInit();
            stackPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbxPreCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxProductCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxStandCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxCustomerCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxRemark.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxType.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxIsDefault.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxLabelName.Properties).BeginInit();
            SuspendLayout();
            // 
            // tablePanel1
            // 
            tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] { new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 120F), new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F) });
            tablePanel1.Controls.Add(stackPanel1);
            tablePanel1.Controls.Add(cbxPreCode);
            tablePanel1.Controls.Add(labelControl1);
            tablePanel1.Controls.Add(lb6);
            tablePanel1.Controls.Add(lb2);
            tablePanel1.Controls.Add(lb3);
            tablePanel1.Controls.Add(lb4);
            tablePanel1.Controls.Add(lb5);
            tablePanel1.Controls.Add(lb7);
            tablePanel1.Controls.Add(cbxProductCode);
            tablePanel1.Controls.Add(cbxStandCode);
            tablePanel1.Controls.Add(cbxCustomerCode);
            tablePanel1.Controls.Add(cbxRemark);
            tablePanel1.Controls.Add(cbxType);
            tablePanel1.Controls.Add(cbxIsDefault);
            tablePanel1.Controls.Add(labelControl2);
            tablePanel1.Controls.Add(cbxLabelName);
            tablePanel1.Dock = DockStyle.Fill;
            tablePanel1.Location = new Point(0, 0);
            tablePanel1.Name = "tablePanel1";
            tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] { new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 15F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 80F) });
            tablePanel1.Size = new Size(400, 450);
            tablePanel1.TabIndex = 0;
            // 
            // stackPanel1
            // 
            tablePanel1.SetColumn(stackPanel1, 0);
            tablePanel1.SetColumnSpan(stackPanel1, 2);
            stackPanel1.Controls.Add(btnOk);
            stackPanel1.Controls.Add(btnCancel);
            stackPanel1.Dock = DockStyle.Fill;
            stackPanel1.LayoutDirection = DevExpress.Utils.Layout.StackPanelLayoutDirection.RightToLeft;
            stackPanel1.Location = new Point(3, 370);
            stackPanel1.Name = "stackPanel1";
            tablePanel1.SetRow(stackPanel1, 9);
            stackPanel1.Size = new Size(394, 77);
            stackPanel1.TabIndex = 2;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(274, 21);
            btnOk.Margin = new Padding(3, 3, 20, 3);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 35);
            btnOk.TabIndex = 0;
            btnOk.Text = "确认";
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(151, 21);
            btnCancel.Margin = new Padding(3, 3, 20, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "取消";
            // 
            // cbxPreCode
            // 
            tablePanel1.SetColumn(cbxPreCode, 1);
            cbxPreCode.Location = new Point(130, 67);
            cbxPreCode.Margin = new Padding(10, 3, 15, 3);
            cbxPreCode.Name = "cbxPreCode";
            cbxPreCode.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxPreCode.Properties.Appearance.Options.UseFont = true;
            cbxPreCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            tablePanel1.SetRow(cbxPreCode, 2);
            cbxPreCode.Size = new Size(255, 28);
            cbxPreCode.TabIndex = 1;
            // 
            // labelControl1
            // 
            labelControl1.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl1.Appearance.Options.UseFont = true;
            labelControl1.Appearance.Options.UseTextOptions = true;
            labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(labelControl1, 0);
            labelControl1.Dock = DockStyle.Fill;
            labelControl1.Location = new Point(3, 62);
            labelControl1.Margin = new Padding(3, 3, 10, 3);
            labelControl1.Name = "labelControl1";
            tablePanel1.SetRow(labelControl1, 2);
            labelControl1.Size = new Size(107, 38);
            labelControl1.TabIndex = 0;
            labelControl1.Text = "线盘代码";
            // 
            // lb6
            // 
            lb6.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lb6.Appearance.Options.UseFont = true;
            lb6.Appearance.Options.UseTextOptions = true;
            lb6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lb6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(lb6, 0);
            lb6.Dock = DockStyle.Fill;
            lb6.Location = new Point(3, 282);
            lb6.Margin = new Padding(3, 3, 10, 3);
            lb6.Name = "lb6";
            tablePanel1.SetRow(lb6, 7);
            lb6.Size = new Size(107, 38);
            lb6.TabIndex = 0;
            lb6.Text = "标签类型";
            lb6.ToolTip = "标签类型：0. 盘标签；1. 箱标签；3. 发货标签";
            // 
            // lb2
            // 
            lb2.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lb2.Appearance.Options.UseFont = true;
            lb2.Appearance.Options.UseTextOptions = true;
            lb2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lb2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(lb2, 0);
            lb2.Dock = DockStyle.Fill;
            lb2.Location = new Point(3, 106);
            lb2.Margin = new Padding(3, 3, 10, 3);
            lb2.Name = "lb2";
            tablePanel1.SetRow(lb2, 3);
            lb2.Size = new Size(107, 38);
            lb2.TabIndex = 0;
            lb2.Text = "产品代码";
            // 
            // lb3
            // 
            lb3.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lb3.Appearance.Options.UseFont = true;
            lb3.Appearance.Options.UseTextOptions = true;
            lb3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lb3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(lb3, 0);
            lb3.Dock = DockStyle.Fill;
            lb3.Location = new Point(3, 150);
            lb3.Margin = new Padding(3, 3, 10, 3);
            lb3.Name = "lb3";
            tablePanel1.SetRow(lb3, 4);
            lb3.Size = new Size(107, 38);
            lb3.TabIndex = 0;
            lb3.Text = "技术标准代码";
            // 
            // lb4
            // 
            lb4.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lb4.Appearance.Options.UseFont = true;
            lb4.Appearance.Options.UseTextOptions = true;
            lb4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lb4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(lb4, 0);
            lb4.Dock = DockStyle.Fill;
            lb4.Location = new Point(3, 194);
            lb4.Margin = new Padding(3, 3, 10, 3);
            lb4.Name = "lb4";
            tablePanel1.SetRow(lb4, 5);
            lb4.Size = new Size(107, 38);
            lb4.TabIndex = 0;
            lb4.Text = "客户代码";
            // 
            // lb5
            // 
            lb5.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lb5.Appearance.Options.UseFont = true;
            lb5.Appearance.Options.UseTextOptions = true;
            lb5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lb5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(lb5, 0);
            lb5.Dock = DockStyle.Fill;
            lb5.Location = new Point(3, 238);
            lb5.Margin = new Padding(3, 3, 10, 3);
            lb5.Name = "lb5";
            tablePanel1.SetRow(lb5, 6);
            lb5.Size = new Size(107, 38);
            lb5.TabIndex = 0;
            lb5.Text = "备注";
            // 
            // lb7
            // 
            lb7.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lb7.Appearance.Options.UseFont = true;
            lb7.Appearance.Options.UseTextOptions = true;
            lb7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lb7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(lb7, 0);
            lb7.Dock = DockStyle.Fill;
            lb7.Location = new Point(3, 326);
            lb7.Margin = new Padding(3, 3, 10, 3);
            lb7.Name = "lb7";
            tablePanel1.SetRow(lb7, 8);
            lb7.Size = new Size(107, 38);
            lb7.TabIndex = 0;
            lb7.Text = "是否系统默认";
            lb7.ToolTip = "1.系统默认；2.客户指定";
            // 
            // cbxProductCode
            // 
            tablePanel1.SetColumn(cbxProductCode, 1);
            cbxProductCode.Location = new Point(130, 111);
            cbxProductCode.Margin = new Padding(10, 3, 15, 3);
            cbxProductCode.Name = "cbxProductCode";
            cbxProductCode.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxProductCode.Properties.Appearance.Options.UseFont = true;
            cbxProductCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            tablePanel1.SetRow(cbxProductCode, 3);
            cbxProductCode.Size = new Size(255, 28);
            cbxProductCode.TabIndex = 1;
            // 
            // cbxStandCode
            // 
            tablePanel1.SetColumn(cbxStandCode, 1);
            cbxStandCode.Location = new Point(130, 155);
            cbxStandCode.Margin = new Padding(10, 3, 15, 3);
            cbxStandCode.Name = "cbxStandCode";
            cbxStandCode.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxStandCode.Properties.Appearance.Options.UseFont = true;
            cbxStandCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            tablePanel1.SetRow(cbxStandCode, 4);
            cbxStandCode.Size = new Size(255, 28);
            cbxStandCode.TabIndex = 1;
            // 
            // cbxCustomerCode
            // 
            tablePanel1.SetColumn(cbxCustomerCode, 1);
            cbxCustomerCode.Location = new Point(130, 199);
            cbxCustomerCode.Margin = new Padding(10, 3, 15, 3);
            cbxCustomerCode.Name = "cbxCustomerCode";
            cbxCustomerCode.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxCustomerCode.Properties.Appearance.Options.UseFont = true;
            cbxCustomerCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            tablePanel1.SetRow(cbxCustomerCode, 5);
            cbxCustomerCode.Size = new Size(255, 28);
            cbxCustomerCode.TabIndex = 1;
            // 
            // cbxRemark
            // 
            tablePanel1.SetColumn(cbxRemark, 1);
            cbxRemark.Location = new Point(130, 243);
            cbxRemark.Margin = new Padding(10, 3, 15, 3);
            cbxRemark.Name = "cbxRemark";
            cbxRemark.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxRemark.Properties.Appearance.Options.UseFont = true;
            cbxRemark.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            tablePanel1.SetRow(cbxRemark, 6);
            cbxRemark.Size = new Size(255, 28);
            cbxRemark.TabIndex = 1;
            // 
            // cbxType
            // 
            tablePanel1.SetColumn(cbxType, 1);
            cbxType.EditValue = "0 盘标签";
            cbxType.Location = new Point(130, 287);
            cbxType.Margin = new Padding(10, 3, 15, 3);
            cbxType.Name = "cbxType";
            cbxType.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxType.Properties.Appearance.Options.UseFont = true;
            cbxType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxType.Properties.Items.AddRange(new object[] { "0 盘标签", "1 箱标签", "2 发货标签" });
            tablePanel1.SetRow(cbxType, 7);
            cbxType.Size = new Size(255, 28);
            cbxType.TabIndex = 1;
            // 
            // cbxIsDefault
            // 
            tablePanel1.SetColumn(cbxIsDefault, 1);
            cbxIsDefault.EditValue = "1 系统默认";
            cbxIsDefault.Location = new Point(130, 331);
            cbxIsDefault.Margin = new Padding(10, 3, 15, 3);
            cbxIsDefault.Name = "cbxIsDefault";
            cbxIsDefault.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxIsDefault.Properties.Appearance.Options.UseFont = true;
            cbxIsDefault.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxIsDefault.Properties.Items.AddRange(new object[] { "1 系统默认", "2 客户指定" });
            tablePanel1.SetRow(cbxIsDefault, 8);
            cbxIsDefault.Size = new Size(255, 28);
            cbxIsDefault.TabIndex = 1;
            // 
            // labelControl2
            // 
            labelControl2.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl2.Appearance.Options.UseFont = true;
            labelControl2.Appearance.Options.UseTextOptions = true;
            labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            tablePanel1.SetColumn(labelControl2, 0);
            labelControl2.Dock = DockStyle.Fill;
            labelControl2.Location = new Point(3, 18);
            labelControl2.Margin = new Padding(3, 3, 10, 3);
            labelControl2.Name = "labelControl2";
            tablePanel1.SetRow(labelControl2, 1);
            labelControl2.Size = new Size(107, 38);
            labelControl2.TabIndex = 0;
            labelControl2.Text = "标签名字";
            // 
            // cbxLabelName
            // 
            tablePanel1.SetColumn(cbxLabelName, 1);
            cbxLabelName.Location = new Point(130, 23);
            cbxLabelName.Margin = new Padding(10, 3, 15, 3);
            cbxLabelName.Name = "cbxLabelName";
            cbxLabelName.Properties.Appearance.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbxLabelName.Properties.Appearance.Options.UseFont = true;
            cbxLabelName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            tablePanel1.SetRow(cbxLabelName, 1);
            cbxLabelName.Size = new Size(255, 28);
            cbxLabelName.TabIndex = 1;
            // 
            // NewLabelInput
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(400, 450);
            Controls.Add(tablePanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "NewLabelInput";
            StartPosition = FormStartPosition.CenterParent;
            Text = "NewLabelInput";
            Load += NewLabelInput_Load;
            ((System.ComponentModel.ISupportInitialize)tablePanel1).EndInit();
            tablePanel1.ResumeLayout(false);
            tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)stackPanel1).EndInit();
            stackPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cbxPreCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxProductCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxStandCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxCustomerCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxRemark.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxType.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxIsDefault.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxLabelName.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lb6;
        private DevExpress.XtraEditors.LabelControl lb2;
        private DevExpress.XtraEditors.LabelControl lb3;
        private DevExpress.XtraEditors.LabelControl lb4;
        private DevExpress.XtraEditors.LabelControl lb5;
        private DevExpress.XtraEditors.LabelControl lb7;
        private DevExpress.XtraEditors.ComboBoxEdit cbxPreCode;
        private DevExpress.XtraEditors.ComboBoxEdit cbxProductCode;
        private DevExpress.XtraEditors.ComboBoxEdit cbxStandCode;
        private DevExpress.XtraEditors.ComboBoxEdit cbxCustomerCode;
        private DevExpress.XtraEditors.ComboBoxEdit cbxRemark;
        private DevExpress.XtraEditors.ComboBoxEdit cbxType;
        private DevExpress.XtraEditors.ComboBoxEdit cbxIsDefault;
        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cbxLabelName;
    }
}