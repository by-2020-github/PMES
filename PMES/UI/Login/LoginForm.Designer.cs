namespace PMES.UI.Login
{
    partial class LoginForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            txtUser = new DevExpress.XtraEditors.TextEdit();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            btnLogin = new DevExpress.XtraEditors.SimpleButton();
            txtPassword = new DevExpress.XtraEditors.TextEdit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtUser.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtPassword.Properties).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(txtUser, 0, 2);
            tableLayoutPanel1.Controls.Add(labelControl2, 0, 1);
            tableLayoutPanel1.Controls.Add(btnLogin, 0, 4);
            tableLayoutPanel1.Controls.Add(txtPassword, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(591, 334);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // txtUser
            // 
            txtUser.Anchor = AnchorStyles.None;
            txtUser.Location = new Point(145, 117);
            txtUser.Name = "txtUser";
            txtUser.Properties.Appearance.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUser.Properties.Appearance.Options.UseFont = true;
            txtUser.Properties.Appearance.Options.UseTextOptions = true;
            txtUser.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            txtUser.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            txtUser.Properties.AutoHeight = false;
            txtUser.Properties.NullText = "员工号";
            txtUser.Size = new Size(300, 40);
            txtUser.TabIndex = 1;
            // 
            // labelControl2
            // 
            labelControl2.Anchor = AnchorStyles.None;
            labelControl2.Appearance.Font = new Font("微软雅黑", 16F, FontStyle.Bold, GraphicsUnit.Point);
            labelControl2.Appearance.Options.UseFont = true;
            labelControl2.Appearance.Options.UseTextOptions = true;
            labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            labelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            labelControl2.AppearancePressed.Options.UseTextOptions = true;
            labelControl2.AppearancePressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            labelControl2.Location = new Point(155, 29);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new Size(280, 60);
            labelControl2.TabIndex = 2;
            labelControl2.Text = "先登高科电气股份有限公司成品智能包装系统";
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.None;
            btnLogin.Appearance.BackColor = Color.FromArgb(22, 155, 213);
            btnLogin.Appearance.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnLogin.Appearance.Options.UseBackColor = true;
            btnLogin.Appearance.Options.UseFont = true;
            btnLogin.Location = new Point(225, 274);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(140, 40);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "登录";
            btnLogin.Click += Login;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.None;
            txtPassword.Location = new Point(145, 195);
            txtPassword.Name = "txtPassword";
            txtPassword.Properties.Appearance.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.Properties.Appearance.Options.UseFont = true;
            txtPassword.Properties.Appearance.Options.UseTextOptions = true;
            txtPassword.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            txtPassword.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            txtPassword.Properties.AutoHeight = false;
            txtPassword.Properties.NullText = "密码";
            txtPassword.Size = new Size(300, 40);
            txtPassword.TabIndex = 1;
            txtPassword.KeyDown += TryLogin;
            // 
            // LoginForm
            // 
            Appearance.BackColor = Color.FromArgb(129, 211, 248);
            Appearance.Options.UseBackColor = true;
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(591, 334);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            Text = "LoginForm";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtUser.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtPassword.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.TextEdit txtUser;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtPassword;
    }
}