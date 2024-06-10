using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private string? _preScanCode;
        [ObservableProperty] private double? _weight1;
        [ObservableProperty] private double? _weight2;


        #region 表示状态

        /// <summary>
        ///     第一次扫码
        /// </summary>
        [ObservableProperty] private Status? _barCodeScanner1;

        /// <summary>
        ///     扫描校验
        /// </summary>
        [ObservableProperty] private Status? _barCodeScanner2;

        /// <summary>
        ///     第一次称重
        /// </summary>
        [ObservableProperty] private Status? _weighing1;

        /// <summary>
        ///     称重校验
        /// </summary>
        [ObservableProperty] private Status? _weighing2;

        /// <summary>
        ///     打印机
        /// </summary>
        [ObservableProperty] private Status? _printer;

        /// <summary>
        ///     条码校验结果
        /// </summary>
        [ObservableProperty] private bool? _barCodeCheck;

        /// <summary>
        ///     PLC状态
        /// </summary>
        [ObservableProperty] private Status? _plcStatus;

        /// <summary>
        ///     服务器状态
        /// </summary>
        [ObservableProperty] private Status? _serviceStatus;

        /// <summary>
        ///     所有设备（工作指示灯）
        /// </summary>
        [ObservableProperty] private Status? _allStatus;

        #endregion

        #region cmds

        [RelayCommand]
        private void Close()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }

    public enum Status
    {
        Unknown,
        Running,
        Pause,
        Stop,
        Error
    }
}