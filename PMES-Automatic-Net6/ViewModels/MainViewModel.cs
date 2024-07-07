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
        [ObservableProperty] private string? _preScanCode1 = "ssssssssssssss";
        [ObservableProperty] private string? _preScanCode2 = "ssssssssssssss";
        [ObservableProperty] private string ? _weight1 = "2.22d";
        [ObservableProperty] private string? _weight2= "3.5555d";

        #region 表示状态

        /// <summary>
        ///     第一次扫码
        /// </summary>
        [ObservableProperty] private Status? _barCodeScannerStatus1 = Status.Unknown;

        /// <summary>
        ///     扫描校验
        /// </summary>
        [ObservableProperty] private Status? _barCodeScannerStatus2 = Status.Unknown;

        /// <summary>
        ///     第一次称重
        /// </summary>
        [ObservableProperty] private Status? _weighingStatus1 = Status.Unknown;

        /// <summary>
        ///     称重校验
        /// </summary>
        [ObservableProperty] private Status? _weighingStatus2 = Status.Unknown;

        /// <summary>
        ///     打印机
        /// </summary>
        [ObservableProperty] private Status? _printerStatus = Status.Unknown;

        /// <summary>
        ///     条码校验结果
        /// </summary>
        [ObservableProperty] private bool? _barCodeCheck = false;

        /// <summary>
        ///     PLC状态
        /// </summary>
        [ObservableProperty] private Status? _plcStatus = Status.Unknown;

        /// <summary>
        ///     服务器状态
        /// </summary>
        [ObservableProperty] private Status? _serviceStatus = Status.Unknown;

        /// <summary>
        ///     所有设备（工作指示灯）
        /// </summary>
        [ObservableProperty] private Status? _allStatus = Status.Unknown;

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