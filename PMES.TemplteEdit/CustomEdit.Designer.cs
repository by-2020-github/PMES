namespace PMES.TemplteEdit
{
    partial class CustomEdit
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
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.lbLabelTypeInfo = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cbxCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbxType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lbPakageName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 120F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 2F)});
            this.tablePanel1.Controls.Add(this.lbLabelTypeInfo);
            this.tablePanel1.Controls.Add(this.simpleButton1);
            this.tablePanel1.Controls.Add(this.cbxCode);
            this.tablePanel1.Controls.Add(this.labelControl1);
            this.tablePanel1.Controls.Add(this.labelControl2);
            this.tablePanel1.Controls.Add(this.cbxType);
            this.tablePanel1.Controls.Add(this.lbPakageName);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 80F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 80F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 45F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 80F)});
            this.tablePanel1.Size = new System.Drawing.Size(876, 468);
            this.tablePanel1.TabIndex = 0;
            // 
            // lbLabelTypeInfo
            // 
            this.tablePanel1.SetColumn(this.lbLabelTypeInfo, 2);
            this.lbLabelTypeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLabelTypeInfo.Location = new System.Drawing.Point(441, 149);
            this.lbLabelTypeInfo.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.lbLabelTypeInfo.Name = "lbLabelTypeInfo";
            this.tablePanel1.SetRow(this.lbLabelTypeInfo, 1);
            this.lbLabelTypeInfo.Size = new System.Drawing.Size(426, 122);
            this.lbLabelTypeInfo.TabIndex = 4;
            // 
            // simpleButton1
            // 
            this.tablePanel1.SetColumn(this.simpleButton1, 2);
            this.simpleButton1.Location = new System.Drawing.Point(607, 378);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(175, 9, 175, 9);
            this.simpleButton1.Name = "simpleButton1";
            this.tablePanel1.SetRow(this.simpleButton1, 4);
            this.simpleButton1.Size = new System.Drawing.Size(94, 70);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "保存";
            this.simpleButton1.Click += new System.EventHandler(this.Save);
            // 
            // cbxCode
            // 
            this.tablePanel1.SetColumn(this.cbxCode, 1);
            this.cbxCode.Location = new System.Drawing.Point(219, 50);
            this.cbxCode.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.cbxCode.Name = "cbxCode";
            this.cbxCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.tablePanel1.SetRow(this.cbxCode, 0);
            this.cbxCode.Size = new System.Drawing.Size(204, 40);
            this.cbxCode.TabIndex = 1;
            this.cbxCode.SelectedIndexChanged += new System.EventHandler(this.PackageCodeChanged);
            // 
            // labelControl1
            // 
            this.tablePanel1.SetColumn(this.labelControl1, 0);
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(16, 156);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.labelControl1.Name = "labelControl1";
            this.tablePanel1.SetRow(this.labelControl1, 1);
            this.labelControl1.Size = new System.Drawing.Size(178, 108);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "标签类型：";
            // 
            // labelControl2
            // 
            this.tablePanel1.SetColumn(this.labelControl2, 0);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl2.Location = new System.Drawing.Point(28, 28);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(28, 28, 28, 28);
            this.labelControl2.Name = "labelControl2";
            this.tablePanel1.SetRow(this.labelControl2, 0);
            this.labelControl2.Size = new System.Drawing.Size(154, 84);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "包装代码：";
            // 
            // cbxType
            // 
            this.tablePanel1.SetColumn(this.cbxType, 1);
            this.cbxType.Location = new System.Drawing.Point(226, 190);
            this.cbxType.Margin = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.cbxType.Name = "cbxType";
            this.cbxType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.tablePanel1.SetRow(this.cbxType, 1);
            this.cbxType.Size = new System.Drawing.Size(190, 40);
            this.cbxType.TabIndex = 1;
            this.cbxType.SelectedIndexChanged += new System.EventHandler(this.LabelTypeChanged);
            // 
            // lbPakageName
            // 
            this.tablePanel1.SetColumn(this.lbPakageName, 2);
            this.lbPakageName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPakageName.Location = new System.Drawing.Point(448, 16);
            this.lbPakageName.Margin = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.lbPakageName.Name = "lbPakageName";
            this.tablePanel1.SetRow(this.lbPakageName, 0);
            this.lbPakageName.Size = new System.Drawing.Size(412, 108);
            this.lbPakageName.TabIndex = 4;
            // 
            // CustomEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 468);
            this.Controls.Add(this.tablePanel1);
            this.Name = "CustomEdit";
            this.Text = "CustomEdit";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit cbxCode;
        private DevExpress.XtraEditors.ComboBoxEdit cbxType;
        private DevExpress.XtraEditors.LabelControl lbLabelTypeInfo;
        private DevExpress.XtraEditors.LabelControl lbPakageName;
    }
}