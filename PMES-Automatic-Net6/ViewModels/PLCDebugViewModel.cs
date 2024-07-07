using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PMES_Respository.DataStruct;
using S7.Net;
using S7.Net.Protocol;
using S7.Net.Types;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class PLCDebugViewModel : ObservableObject
    {
        [ObservableProperty]private List<string> _cmdStruct;
        public string Ip { get; set; }
        public int Rack { get; set; }
        public int Slot { get; set; }

        [ObservableProperty] private bool _isOpen;

        //以下代码创建了一个连接到本地主机（IP地址为127.0.0.1）上机架0插槽2的S7-1200 PLC的实例
        private S7.Net.Plc plc;

        public PLCDebugViewModel()
        {
            CmdStruct = Enum.GetNames(typeof(CmdStructEnum)).ToList();
        }

        public Plc GetPlc => plc;

        [RelayCommand]
        private void SelectStructChanged()
        {
          
        }

        [RelayCommand]
        private void SendMsg1(object classValue)
        {

        }

        [RelayCommand]
        private void ReadStruct()
        {


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
    }

 
}