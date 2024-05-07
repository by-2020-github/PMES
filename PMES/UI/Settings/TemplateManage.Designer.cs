namespace PMES.UI.Settings
{
    partial class TemplateManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateManage));
            gridControlTemplates = new DevExpress.XtraGrid.GridControl();
            gridViewTemplates = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)gridControlTemplates).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewTemplates).BeginInit();
            ((System.ComponentModel.ISupportInitialize)stackPanel1).BeginInit();
            stackPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // gridControlTemplates
            // 
            gridControlTemplates.Dock = DockStyle.Fill;
            gridControlTemplates.Location = new Point(0, 0);
            gridControlTemplates.MainView = gridViewTemplates;
            gridControlTemplates.Name = "gridControlTemplates";
            gridControlTemplates.Size = new Size(798, 568);
            gridControlTemplates.TabIndex = 0;
            gridControlTemplates.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewTemplates });
            // 
            // gridViewTemplates
            // 
            gridViewTemplates.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumn1, gridColumn2 });
            gridViewTemplates.GridControl = gridControlTemplates;
            gridViewTemplates.Name = "gridViewTemplates";
            gridViewTemplates.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            gridViewTemplates.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            gridViewTemplates.FocusedRowChanged += gridViewTemplates_FocusedRowChanged;
            // 
            // gridColumn1
            // 
            gridColumn1.AppearanceCell.Font = new Font("微软雅黑", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gridColumn1.AppearanceCell.Options.UseFont = true;
            gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridColumn1.AppearanceHeader.Font = new Font("微软雅黑", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gridColumn1.AppearanceHeader.Options.UseFont = true;
            gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridColumn1.Caption = "标签名称";
            gridColumn1.FieldName = "Name";
            gridColumn1.Name = "gridColumn1";
            gridColumn1.OptionsColumn.AllowEdit = false;
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            gridColumn2.AppearanceCell.Font = new Font("微软雅黑", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gridColumn2.AppearanceCell.Options.UseFont = true;
            gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridColumn2.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            gridColumn2.AppearanceHeader.Font = new Font("微软雅黑", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gridColumn2.AppearanceHeader.Options.UseFont = true;
            gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridColumn2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            gridColumn2.Caption = "是否启用";
            gridColumn2.FieldName = "IsCurrent";
            gridColumn2.Name = "gridColumn2";
            gridColumn2.OptionsColumn.AllowEdit = false;
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 1;
            // 
            // stackPanel1
            // 
            stackPanel1.Controls.Add(btnConfirm);
            stackPanel1.Controls.Add(btnCancel);
            stackPanel1.Dock = DockStyle.Bottom;
            stackPanel1.LayoutDirection = DevExpress.Utils.Layout.StackPanelLayoutDirection.RightToLeft;
            stackPanel1.Location = new Point(0, 508);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new Size(798, 60);
            stackPanel1.TabIndex = 1;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(688, 10);
            btnConfirm.Margin = new Padding(10, 3, 20, 3);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(90, 40);
            btnConfirm.TabIndex = 0;
            btnConfirm.Text = "确认";
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(578, 10);
            btnCancel.Margin = new Padding(10, 3, 10, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 40);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "取消";
            btnCancel.Click += btnCancel_Click;
            // 
            // TemplateManage
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 568);
            Controls.Add(stackPanel1);
            Controls.Add(gridControlTemplates);
            FormBorderStyle = FormBorderStyle.None;
            IconOptions.Image = (Image)resources.GetObject("TemplateManage.IconOptions.Image");
            Name = "TemplateManage";
            StartPosition = FormStartPosition.CenterParent;
            Text = "TemplateManage";
            Load += TemplateManage_Load;
            ((System.ComponentModel.ISupportInitialize)gridControlTemplates).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewTemplates).EndInit();
            ((System.ComponentModel.ISupportInitialize)stackPanel1).EndInit();
            stackPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlTemplates;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTemplates;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}