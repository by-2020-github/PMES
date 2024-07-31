using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PMES_Automatic_Net6.Core;
using S7.Net;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Respository.DataStruct;
using S7.Net.Types;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class TopPageViewModel : ObservableObject
    {
        private Plc Plc => HardwareManager.Instance.Plc;


        public TopPageViewModel()
        {
            HardwareManager.Instance.OnReceive += Handler;
            HardwareManager.Instance.OnWeightAndCodeChanged += WeightAndCodeChanged;
            HardwareManager.Instance.OnReelCodeChanged += ReelCodeChanged;
            HardwareManager.Instance.OnBoxBarCodeChanged += BoxBarCodeChanged;
        }

        #region PLC状态改变处理事件

        private System.Collections.Concurrent.ConcurrentQueue<ProductInfo> _productInfoQueue = new();

        /// <summary>
        ///     PLC状态监测
        /// </summary>
        /// <param name="obj"></param>
        private void Handler(object obj)
        {
            if (obj is PmesCmdUnStacking pmesCmdUnStacking)
            {
                if (pmesCmdUnStacking.PmesAndPlcReadWriteFlag == 1) //PLC读走指令后置1 表示空闲
                {
                }
            }
        }

        /// <summary>
        ///     读取重量和条码
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void WeightAndCodeChanged(List<DataItem> obj)
        {
            Weight1 = ((double)obj[2].Value / 100d).ToString("f2");
            Weight2 = ((double)obj[3].Value / 100d).ToString("f2");

            _productInfoQueue.Enqueue(new ProductInfo
            {
                Weight1 = (double)obj[2].Value,
                Weight2 = (double)obj[3].Value,
                BarCode = obj[1].Value.ToString(),
                WorkShop = EnumWorkShop.Weight
            });
        }

        private void BoxBarCodeChanged(List<DataItem> obj)
        {
            PreScanCode2 = obj[1].ToString();
            PreScanCode2 = obj[2].ToString();
            var info = _productInfoQueue.First(s => s.WorkShop == EnumWorkShop.Weight);
            info.ReelCode1 = PreScanCode1;
            info.ReelCode2 = PreScanCode2;
            info.WorkShop = EnumWorkShop.ReelCodeCheck;
        }

        private void ReelCodeChanged(List<DataItem> obj)
        {
            PreScanCode1 = obj[1].ToString();
            PreScanCode2 = obj[2].ToString();
            var info = _productInfoQueue.First(s => s.WorkShop == EnumWorkShop.Weight);
            info.ReelCode1 = PreScanCode1;
            info.ReelCode2 = PreScanCode2;
            info.WorkShop = EnumWorkShop.ReelCodeCheck;
        }

        #endregion

        [ObservableProperty] private string? _preScanCode1 = "s1";
        [ObservableProperty] private string? _preScanCode2 = "s2";
        [ObservableProperty] private string? _weight1 = "2.22d";
        [ObservableProperty] private string? _weight2 = "3.55d";

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
            //Application.Current.Shutdown();
        }

        #endregion
    }

    public class ProductInfo
    {
        public EnumWorkShop WorkShop { get; set; }

        #region 称重条码数据

        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public string? BarCode { get; set; }

        #endregion

        #region reel code 复核

        public string? ReelCode1 { get; set; }
        public string? ReelCode2 { get; set; }

        #endregion
    }

    public enum EnumWorkShop
    {
        None = 0,
        UnStacking,
        Weight,
        ReelCodeCheck,
        PackingBoxCode,
        Stacking
    }
}