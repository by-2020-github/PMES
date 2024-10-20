using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreeSql.DataAnnotations;
using K4os.Hash.xxHash;
using PMES.Manual.Net6.Core;
using PMES.Manual.Net6.Core.Managers;
using PMES.Manual.Net6.Model;
using Serilog;

namespace PMES.Manual.Net6.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private string _portSelect;

        [ObservableProperty] private List<string> _portSource;


        [ObservableProperty] private int _baudSelect = 19200;

        [ObservableProperty] private List<int> _baudSource = new List<int>() { 9600, 19200, 38400, 115200 };


        [ObservableProperty] private Parity _paritySelect = Parity.None;
        public List<Parity> ParitySource => Enum.GetValues<Parity>().ToList();

        [ObservableProperty] private int _dataBitsSelect = 8;

        [ObservableProperty] private List<int> _dataBitsSource = new List<int>() { 7, 8 };

        [ObservableProperty] private StopBits _stopBitsSelect = StopBits.One;

        public List<StopBits> StopBitstSource => Enum.GetValues<StopBits>().ToList();

        [RelayCommand]
        private void Refresh()
        {
            PortSource = SerialPort.GetPortNames().ToList();
            BaudSelect = PMES.Default.BaudRate;
            ParitySelect = (Parity)PMES.Default.Parity;
            DataBitsSelect = PMES.Default.DataBits;
            StopBitsSelect = (StopBits)PMES.Default.StopBits;
        }
    }
}