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

        [ObservableProperty] private string _printerName = "";
        [ObservableProperty] private ObservableCollection<string> _printerNames = new ObservableCollection<string>();
        [ObservableProperty] private int _printerDirection;
        [ObservableProperty] private string _printerLabel;

        [RelayCommand]
        private void RefreshPorts()
        {
            PortSource = SerialPort.GetPortNames().ToList();
        }

        [RelayCommand]
        private void RefreshPrinters()
        {
            PrinterNames.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterNames.Add(printer);
            }
        }

        private void LoadSettings()
        {
            PortSelect = PMES.Default.Port;
            BaudSelect = PMES.Default.BaudRate;
            ParitySelect = (Parity)PMES.Default.Parity;
            DataBitsSelect = PMES.Default.DataBits;
            StopBitsSelect = (StopBits)PMES.Default.StopBits;

            PrinterName = PMES.Default.PrinterName1;
            PrinterDirection = PMES.Default.PrinterDirection;
            PrinterLabel = PMES.Default.DefaultLabel;
        }

        [RelayCommand]
        private void SavePortSettings()
        {
            PMES.Default.Port = PortSelect;
            PMES.Default.BaudRate = BaudSelect;
            PMES.Default.Parity = (int)ParitySelect;
            PMES.Default.DataBits = DataBitsSelect;
            PMES.Default.StopBits = (int)StopBitsSelect;
            PMES.Default.Save();
        }

        [RelayCommand]
        private void SavePrinterSettings()
        {
            PMES.Default.PrinterName1 = PrinterName;
            PMES.Default.PrinterDirection = PrinterDirection;
            PMES.Default.DefaultLabel = PrinterLabel;
            PMES.Default.Save();
        }
    }
}