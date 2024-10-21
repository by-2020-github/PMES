using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using PMES.Manual.Net6.Core.Managers;
using Serilog;
using System.Security;
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Model.users;
using PMES.Manual.Net6.Views;

namespace PMES.Manual.Net6.ViewModels
{
    partial class LoginViewMode : ObservableObject
    {
        private ILogger Logger => SerilogManager.GetOrCreateLogger("running");
        private IFreeSql _fsql;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string? _username;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string? _password;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async void Login()
        {
            try
            {
                var param = new Dictionary<string, string>
                {
                    { "password", $"{Username}" },
                    { "username", $"{Password}" }
                };
                var userInfo = await WebService.Instance.Post<UserInfo>(param, ApiUrls.Login);
                if (userInfo == null)
                {
                    MessageBox.Show("登录失败，请检查用户名或密码！", "Error:", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                GlobalVar.CurrentUserInfo = userInfo;
                GlobalVar.LoginView.Hide();
                var mainView = new MainView();
                mainView.ShowDialog();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }


            if (Equals(GlobalVar.CurrentUserInfo, null))
            {
                MessageBox.Show("登录失败，请检查用户名或密码！", "Error:", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool CanLogin()
        {
            var canDo = string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password);
            return !canDo;
        }

        [RelayCommand]
        private void Close()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void PassWordChanged(string word)
        {
            Password = word;
            //LoginCommand.NotifyCanExecuteChanged();
        }
    }
}