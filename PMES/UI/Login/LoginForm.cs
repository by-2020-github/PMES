using DevExpress.XtraEditors;
using Newtonsoft.Json;
using PMES.Core;
using PMES.Core.Managers;
using PMES.Model.tbs;
using PMES.Model.users;
using PMES.UI.MainWindow;
using Serilog;
using SICD_Automatic.Core;

namespace PMES.UI.Login;

public partial class LoginForm : XtraForm
{
    private readonly IFreeSql _freeSql = FreeSqlManager.FSql;
    private readonly ILogger _logger = SerilogManager.GetOrCreateLogger();

    public LoginForm()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
    }

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Login(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            ShowMsg("用户名或密码不能为空.");
            return;
        }

        if (txtUser.Text == @"员工号" || txtPassword.Text == @"密码")
        {
            ShowMsg("用户名或密码不能为空.");
            return;
        }

        //var userInfo = _freeSql.Select<T_admin>().Where(s => s.LoginName == txtUser.Text).First();
        //if (userInfo == null)
        //{
        //    ShowMsg("用户不存在.");
        //    return;
        //}
        var param = new Dictionary<string, string>();
        param.Add("password", $"{txtPassword.Text}");
        param.Add("username", $"{txtUser.Text}");
        var data = "";
        try
        {
            var userInfo = await WebService.Instance.Post<UserInfo>(param, ApiUrls.Login);
            GlobalVar.CurrentUserInfo = userInfo;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }


        if (Equals(GlobalVar.CurrentUserInfo, null))
        {
            XtraMessageBox.Show("登录失败！", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        this.Hide();
        var mainForm = new MainForm();
        mainForm.Show();
    }


    private void ShowMsg(string msg, MessageBoxIcon icon = MessageBoxIcon.Error)
    {
        switch (icon)
        {
            case MessageBoxIcon.Asterisk:
                _logger.Warning(msg);
                XtraMessageBox.Show(msg, "Alert:", MessageBoxButtons.OK, icon);
                break;
            case MessageBoxIcon.Error:
                _logger.Error(msg);
                XtraMessageBox.Show(msg, "Error:", MessageBoxButtons.OK, icon);
                break;
            default:
                XtraMessageBox.Show(msg, "Info:", MessageBoxButtons.OK, icon);
                break;
        }
    }

    private void TryLogin(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) Login(null, null);
    }
}