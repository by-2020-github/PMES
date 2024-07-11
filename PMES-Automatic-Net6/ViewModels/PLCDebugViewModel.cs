using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentModbus;
using PMES_Respository.DataStruct;
using S7.Net;
using S7.Net.Protocol;
using S7.Net.Types;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class PLCDebugViewModel : ObservableObject
    {
        [ObservableProperty] private List<string> _cmdStruct;
        public string Ip { get; set; }
        public int Rack { get; set; }
        public int Slot { get; set; }

        public string IpJieDa { get; set; }
        public int Port { get; set; }


        [ObservableProperty] private bool _isOpen;
        [ObservableProperty] private bool _isOpenJieDa;
        [ObservableProperty] private ObservableCollection<DataItem> _weightVar;
        [ObservableProperty] private ObservableCollection<DataItem> _barCodeVar;
        [ObservableProperty] private ObservableCollection<DataItem> _packingBoxVar;


        //以下代码创建了一个连接到本地主机（IP地址为127.0.0.1）上机架0插槽2的S7-1200 PLC的实例
        private S7.Net.Plc plc;
        //信捷PLC
        private ModbusTcpClient _modbusTcpClient;

        public PLCDebugViewModel()
        {
            CmdStruct = Enum.GetNames(typeof(CmdStructEnum)).ToList();
            WeightVar = PmesDataItemList.PmesWeightAndBarCode;
            BarCodeVar = PmesDataItemList.PmesReelCodeCheck;
            PackingBoxVar = PmesDataItemList.PmesPackingBox;
        }

        public Plc GetPlc => plc;

        [RelayCommand]
        private void OpenModbus()
        {
            _modbusTcpClient ??= new ModbusTcpClient();
            if (_modbusTcpClient.IsConnected)
            {
                return;
            }
            try
            {
                _modbusTcpClient.Connect(new IPEndPoint(IPAddress.Parse(IpJieDa),int.Parse(Port.ToString()) ));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                IsOpenJieDa = false;
            }
        }

        [RelayCommand]
        private void CloseModbus()
        {
            try
            {
                _modbusTcpClient?.Disconnect();
                IsOpenJieDa = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                IsOpenJieDa = false;
            }
        }

        [RelayCommand]
        private void Open()
        {
            plc = new Plc(S7.Net.CpuType.S71200, "127.0.0.1", 0, 2);
            try
            {
                plc.Open();
                IsOpen = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                IsOpen = false;
            }
        }

        [RelayCommand]
        private void Close()
        {
            try
            {
                plc?.Close();
                IsOpen = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                IsOpen = false;
            }
        }

        [RelayCommand]
        private void SendWeight()
        {
            if (!plc.IsConnected) return;
            plc.Write(WeightVar.ToArray());
        }

        [RelayCommand]
        private void ReadWeight()
        {
            if (!plc.IsConnected) return;
            plc.ReadMultipleVars(WeightVar.ToList());
        }

        [RelayCommand]
        private void SendBarCode()
        {
            if (!plc.IsConnected) return;
            plc.Write(BarCodeVar.ToArray());
        }

        [RelayCommand]
        private void ReadBarCode()
        {
            if (!plc.IsConnected) return;
            plc.ReadMultipleVars(BarCodeVar.ToList());
        }

        [RelayCommand]
        private void SendBoxCode()
        {
            if (!plc.IsConnected) return;
            plc.Write(PackingBoxVar.ToArray());
        }

        [RelayCommand]
        private void ReadBoxCode()
        {
            if (!plc.IsConnected) return;
            plc.ReadMultipleVars(PackingBoxVar.ToList());
        }
    }


}