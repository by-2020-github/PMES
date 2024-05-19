namespace PMES.UI.Settings
{
    partial class SerialPortSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialPortSettings));
            panel1 = new Panel();
            btnClose = new DevExpress.XtraEditors.SimpleButton();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            tableLayoutPanel1 = new TableLayoutPanel();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            labelControl5 = new DevExpress.XtraEditors.LabelControl();
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            labelControl7 = new DevExpress.XtraEditors.LabelControl();
            cbxCom = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxBa = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxData = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxPr = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxStop = new DevExpress.XtraEditors.ComboBoxEdit();
            cbxType = new DevExpress.XtraEditors.ComboBoxEdit();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            radioGroup = new DevExpress.XtraEditors.RadioGroup();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbxCom.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxBa.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxData.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxPr.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxStop.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbxType.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radioGroup.Properties).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(2, 167, 240);
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(labelControl1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(433, 27);
            panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Dock = DockStyle.Right;
            btnClose.ImageOptions.Image = (Image)resources.GetObject("btnClose.ImageOptions.Image");
            btnClose.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            btnClose.Location = new Point(404, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(29, 27);
            btnClose.TabIndex = 1;
            btnClose.Click += btnClose_Click;
            // 
            // labelControl1
            // 
            labelControl1.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelControl1.Appearance.Options.UseFont = true;
            labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            labelControl1.Dock = DockStyle.Left;
            labelControl1.Location = new Point(0, 0);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(70, 27);
            labelControl1.TabIndex = 0;
            labelControl1.Text = "   参数设置";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38.4615364F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.0769253F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38.46154F));
            tableLayoutPanel1.Controls.Add(labelControl2, 0, 0);
            tableLayoutPanel1.Controls.Add(labelControl3, 0, 1);
            tableLayoutPanel1.Controls.Add(labelControl4, 0, 2);
            tableLayoutPanel1.Controls.Add(labelControl5, 0, 3);
            tableLayoutPanel1.Controls.Add(labelControl6, 0, 4);
            tableLayoutPanel1.Controls.Add(labelControl7, 0, 5);
            tableLayoutPanel1.Controls.Add(cbxCom, 1, 0);
            tableLayoutPanel1.Controls.Add(cbxBa, 1, 1);
            tableLayoutPanel1.Controls.Add(cbxData, 1, 2);
            tableLayoutPanel1.Controls.Add(cbxPr, 1, 3);
            tableLayoutPanel1.Controls.Add(cbxStop, 1, 4);
            tableLayoutPanel1.Controls.Add(cbxType, 1, 5);
            tableLayoutPanel1.Controls.Add(btnSave, 3, 5);
            tableLayoutPanel1.Controls.Add(btnRefresh, 3, 4);
            tableLayoutPanel1.Controls.Add(radioGroup, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 27);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.Size = new Size(433, 227);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // labelControl2
            // 
            labelControl2.Appearance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl2.Appearance.Options.UseFont = true;
            labelControl2.Appearance.Options.UseTextOptions = true;
            labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl2.Dock = DockStyle.Fill;
            labelControl2.Location = new Point(3, 3);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new Size(74, 27);
            labelControl2.TabIndex = 0;
            labelControl2.Text = "端口：";
            // 
            // labelControl3
            // 
            labelControl3.Appearance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl3.Appearance.Options.UseFont = true;
            labelControl3.Appearance.Options.UseTextOptions = true;
            labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl3.Dock = DockStyle.Fill;
            labelControl3.Location = new Point(3, 36);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new Size(74, 27);
            labelControl3.TabIndex = 0;
            labelControl3.Text = "波特率：";
            // 
            // labelControl4
            // 
            labelControl4.Appearance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl4.Appearance.Options.UseFont = true;
            labelControl4.Appearance.Options.UseTextOptions = true;
            labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl4.Dock = DockStyle.Fill;
            labelControl4.Location = new Point(3, 69);
            labelControl4.Name = "labelControl4";
            labelControl4.Size = new Size(74, 27);
            labelControl4.TabIndex = 0;
            labelControl4.Text = "数据位：";
            // 
            // labelControl5
            // 
            labelControl5.Appearance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl5.Appearance.Options.UseFont = true;
            labelControl5.Appearance.Options.UseTextOptions = true;
            labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl5.Dock = DockStyle.Fill;
            labelControl5.Location = new Point(3, 102);
            labelControl5.Name = "labelControl5";
            labelControl5.Size = new Size(74, 27);
            labelControl5.TabIndex = 0;
            labelControl5.Text = "奇偶校验：";
            // 
            // labelControl6
            // 
            labelControl6.Appearance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl6.Appearance.Options.UseFont = true;
            labelControl6.Appearance.Options.UseTextOptions = true;
            labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl6.Dock = DockStyle.Fill;
            labelControl6.Location = new Point(3, 135);
            labelControl6.Name = "labelControl6";
            labelControl6.Size = new Size(74, 27);
            labelControl6.TabIndex = 0;
            labelControl6.Text = "停止位：";
            // 
            // labelControl7
            // 
            labelControl7.Appearance.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelControl7.Appearance.Options.UseFont = true;
            labelControl7.Appearance.Options.UseTextOptions = true;
            labelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            labelControl7.Dock = DockStyle.Fill;
            labelControl7.Location = new Point(3, 168);
            labelControl7.Name = "labelControl7";
            labelControl7.Size = new Size(74, 56);
            labelControl7.TabIndex = 0;
            labelControl7.Text = "仪表类型：";
            // 
            // cbxCom
            // 
            cbxCom.Dock = DockStyle.Fill;
            cbxCom.Location = new Point(83, 3);
            cbxCom.Name = "cbxCom";
            cbxCom.Properties.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxCom.Properties.Appearance.Options.UseFont = true;
            cbxCom.Properties.Appearance.Options.UseTextOptions = true;
            cbxCom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            cbxCom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            cbxCom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxCom.Size = new Size(129, 24);
            cbxCom.TabIndex = 1;
            // 
            // cbxBa
            // 
            cbxBa.Dock = DockStyle.Fill;
            cbxBa.EditValue = "9600";
            cbxBa.Location = new Point(83, 36);
            cbxBa.Name = "cbxBa";
            cbxBa.Properties.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxBa.Properties.Appearance.Options.UseFont = true;
            cbxBa.Properties.Appearance.Options.UseTextOptions = true;
            cbxBa.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            cbxBa.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            cbxBa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxBa.Properties.Items.AddRange(new object[] { "4800", "9600", "19200" });
            cbxBa.Size = new Size(129, 24);
            cbxBa.TabIndex = 1;
            // 
            // cbxData
            // 
            cbxData.Dock = DockStyle.Fill;
            cbxData.EditValue = "8";
            cbxData.Location = new Point(83, 69);
            cbxData.Name = "cbxData";
            cbxData.Properties.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxData.Properties.Appearance.Options.UseFont = true;
            cbxData.Properties.Appearance.Options.UseTextOptions = true;
            cbxData.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            cbxData.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            cbxData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxData.Properties.Items.AddRange(new object[] { "8", "7" });
            cbxData.Size = new Size(129, 24);
            cbxData.TabIndex = 1;
            // 
            // cbxPr
            // 
            cbxPr.Dock = DockStyle.Fill;
            cbxPr.EditValue = "无";
            cbxPr.Location = new Point(83, 102);
            cbxPr.Name = "cbxPr";
            cbxPr.Properties.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxPr.Properties.Appearance.Options.UseFont = true;
            cbxPr.Properties.Appearance.Options.UseTextOptions = true;
            cbxPr.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            cbxPr.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            cbxPr.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxPr.Properties.Items.AddRange(new object[] { "无", "奇", "偶" });
            cbxPr.Size = new Size(129, 24);
            cbxPr.TabIndex = 1;
            // 
            // cbxStop
            // 
            cbxStop.Dock = DockStyle.Fill;
            cbxStop.EditValue = "1";
            cbxStop.Location = new Point(83, 135);
            cbxStop.Name = "cbxStop";
            cbxStop.Properties.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxStop.Properties.Appearance.Options.UseFont = true;
            cbxStop.Properties.Appearance.Options.UseTextOptions = true;
            cbxStop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            cbxStop.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            cbxStop.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxStop.Properties.Items.AddRange(new object[] { "1", "1.5" });
            cbxStop.Size = new Size(129, 24);
            cbxStop.TabIndex = 1;
            // 
            // cbxType
            // 
            cbxType.Anchor = AnchorStyles.None;
            tableLayoutPanel1.SetColumnSpan(cbxType, 2);
            cbxType.EditValue = "TLD-TD231_多行";
            cbxType.Location = new Point(83, 184);
            cbxType.Name = "cbxType";
            cbxType.Properties.Appearance.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxType.Properties.Appearance.Options.UseFont = true;
            cbxType.Properties.Appearance.Options.UseTextOptions = true;
            cbxType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            cbxType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            cbxType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbxType.Properties.Items.AddRange(new object[] { "TLD-TD231_多行" });
            cbxType.Size = new Size(210, 24);
            cbxType.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.None;
            btnSave.Location = new Point(334, 181);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(60, 30);
            btnSave.TabIndex = 2;
            btnSave.Text = "确定";
            btnSave.Click += btnSave_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.None;
            btnRefresh.Location = new Point(314, 135);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 27);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "获取串口信息";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // radioGroup
            // 
            radioGroup.Dock = DockStyle.Fill;
            radioGroup.Location = new Point(299, 3);
            radioGroup.Name = "radioGroup";
            radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] { new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Timer计时器"), new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "OnComm事件") });
            tableLayoutPanel1.SetRowSpan(radioGroup, 3);
            radioGroup.Size = new Size(131, 93);
            radioGroup.TabIndex = 3;
            // 
            // SerialPortSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(433, 254);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(435, 286);
            MinimumSize = new Size(433, 254);
            Name = "SerialPortSettings";
            Text = "SerialPortSettings";
            Load += SerialPortSettings_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cbxCom.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxBa.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxData.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxPr.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxStop.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbxType.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)radioGroup.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit cbxCom;
        private DevExpress.XtraEditors.ComboBoxEdit cbxBa;
        private DevExpress.XtraEditors.ComboBoxEdit cbxData;
        private DevExpress.XtraEditors.ComboBoxEdit cbxPr;
        private DevExpress.XtraEditors.ComboBoxEdit cbxStop;
        private DevExpress.XtraEditors.ComboBoxEdit cbxType;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
    }
}