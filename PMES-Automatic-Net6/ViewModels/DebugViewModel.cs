using System.CodeDom.Compiler;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentModbus;
using PMES_Respository.DataStruct;
using S7.Net.Types;
using S7.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Net;
using System.Reflection;
using PMES_Common;
using System.Printing;
using System.IO;
using HslCommunication.ModBus;
using PMES.UC.reports;
using PMES_Automatic_Net6.Core.Managers;


namespace PMES_Automatic_Net6.ViewModels
{
    public partial class DebugViewModel : ObservableObject
    {
        public Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger();
        public string Ip { get; set; } = PMESConfig.Default.PlcIp;
        public short Rack { get; set; } = (short)PMESConfig.Default.PlcRack;
        public short Slot { get; set; } = (short)PMESConfig.Default.PlcSlot;

        public string IpJieDa { get; set; } = PMESConfig.Default.PlcXinJieIp;
        public int Port { get; set; } = PMESConfig.Default.PlcXinJiePort;

        #region 打印机

        [ObservableProperty] private ObservableCollection<string> _printIps = new ObservableCollection<string>()
        {
            PMESConfig.Default.PrinterName1,
            PMESConfig.Default.PrinterName2,
            PMESConfig.Default.PrinterName3,
            PMESConfig.Default.PrinterName4,
            PMESConfig.Default.PrinterName5,
            PMESConfig.Default.PrinterName6,
        };

        [ObservableProperty] private ObservableCollection<string> _templateLabels = new ObservableCollection<string>()
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
        };

        [RelayCommand]
        private void PrintLabel(object parameters)
        {
            var param = (object[])parameters;
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            var sharedPrinterName = param[0].ToString() ?? ""; // 替换为实际打印机的共享名
            if (!printers.Contains(sharedPrinterName))
            {
                return;
            }

            var label = param[0].ToString();
            var templateBox = new TemplateBox();
            templateBox.Print(sharedPrinterName);
        }

        #endregion


        [ObservableProperty] private bool _isOpen;
        [ObservableProperty] private bool _isOpenJieDa;

        #region 正常不带字符串指令

        [ObservableProperty] private List<string> _cmdStruct;
        [ObservableProperty] private object? _writeObject;
        [ObservableProperty] private object? _readObject;
        [ObservableProperty] private string? _selectedCmdItem;
        [ObservableProperty] private int? _dbBlock;

        #endregion


        #region 带字符串的指令

        private PmesDataItemList _dataItemList = new PmesDataItemList();
        [ObservableProperty] private List<string> _cmdStrStruct;
        [ObservableProperty] private string? _selectedCmdStrItem;
        [ObservableProperty] private ObservableCollection<DataItem> _strCmdDataItems;

        #endregion


        //以下代码创建了一个连接到本地主机（IP地址为127.0.0.1）上机架0插槽2的S7-1200 PLC的实例
        private S7.Net.Plc plc => HardwareManager.Instance.Plc;


        public DebugViewModel()
        {
            var types = typeof(PmesCmdUnStacking).Assembly.GetTypes();
            CmdStruct = types.Where(s => s.Namespace == "PMES_Respository.DataStruct" && s.Name != "PmesDataItemList")
                .Select(s => s.Name).ToList();
            CmdStrStruct = typeof(PmesDataItemList).GetProperties().Select(s => s.Name).ToList();
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            PrintIps.Clear();
            printers.ForEach(s => { PrintIps.Add(s); });
        }

        public Plc GetPlc => plc;

        [RelayCommand]
        private void OpenModbus()
        {
        }

        [RelayCommand]
        private void CloseModbus()
        {
        }

        [RelayCommand]
        private void Open()
        {
            return;
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

        /// <summary>
        ///     读取不含字符串的指令
        /// </summary>
        [RelayCommand]
        private void ReadCmd()
        {
            if (!plc.IsConnected)
                return;
            if (ReadObject == null)
                return;
            plc.ReadClass(ReadObject, DbBlock ?? 0, 0);
        }

        /// <summary>
        ///     读取含字符串的指令
        /// </summary>
        [RelayCommand]
        private void ReadCmdWithStr()
        {
            if (!plc.IsConnected)
                return;
            plc.ReadMultipleVars(StrCmdDataItems.ToList());
            Logger?.Information($"读取到内容\n:{HardwareManager.PrintDataItems(StrCmdDataItems.ToList())}");
        }

        /// <summary>
        ///     发送不含字符串的指令
        /// </summary>
        [RelayCommand]
        private void SendCmd()
        {
            if (!plc.IsConnected)
                return;
            if (WriteObject == null)
                return;

            plc.WriteClass(WriteObject, DbBlock ?? 0, 0);
        }

        /// <summary>
        ///     发送含字符串的指令
        /// </summary>
        [RelayCommand]
        private void SendCmdWithStr()
        {
            if (!plc.IsConnected)
                return;
            if (StrCmdDataItems.Count == 0)
                return;
            try
            {
                StrCmdDataItems[^3].Value = byte.Parse(StrCmdDataItems[^3].Value.ToString());
                StrCmdDataItems[^1].Value = ushort.Parse(StrCmdDataItems[^1].Value.ToString());
                StrCmdDataItems[^2].Value = ushort.Parse(StrCmdDataItems[^2].Value.ToString());

                plc.Write(StrCmdDataItems.TakeLast(3).ToArray());
                Logger?.Information($"发送指令\n:{HardwareManager.PrintDataItems(StrCmdDataItems.ToList())}");
            }
            catch (Exception e)
            {
                Logger?.Error($"发送指令错误:\n{e}");
            }

          
        }


        /// <summary>
        ///     一般不带字符串的指令下拉列表
        /// </summary>
        [RelayCommand]
        private void CmdComBoxChanged()
        {
            if (string.IsNullOrEmpty(SelectedCmdItem))
                return;
            var types = typeof(PmesCmdUnStacking).Assembly.GetTypes();
            var type = types.FirstOrDefault(s => s.FullName != null && s.FullName.Contains(SelectedCmdItem));
            if (type == null)
                return;
            WriteObject = Activator.CreateInstance(type);
            var plcCmdAttribute = WriteObject!.GetType().GetCustomAttribute<PlcCmdAttribute>();
            if (plcCmdAttribute != null)
                DbBlock = plcCmdAttribute.DbBlock;
            ReadObject = Activator.CreateInstance(type);
        }

        /// <summary>
        ///     带字符串的指令下拉列表
        /// </summary>
        [RelayCommand]
        private void CmdComBoxStrChanged()
        {
            if (string.IsNullOrEmpty(SelectedCmdStrItem))
                return;
            var props = typeof(PmesDataItemList).GetProperties().ToList();
            var prop = props.FirstOrDefault(s => s.Name == SelectedCmdStrItem);
            if (prop == null)
                return;
            if (prop.GetValue(_dataItemList) is ObservableCollection<DataItem> dataItems)
            {
                StrCmdDataItems = dataItems;
            }
        }

        #region 信捷PLC

        [ObservableProperty] private ObservableCollection<ModbusCmd> _modbusCmds = new ObservableCollection<ModbusCmd>()
        {
            new()
            {
                DeviceId = 1,
                Address = 1000,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new()
            {
                DeviceId = 1,
                Address = 1002,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new()
            {
                DeviceId = 1,
                Address = 1004,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new()
            {
                DeviceId = 1,
                Address = 1006,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new()
            {
                DeviceId = 1,
                Address = 1008,
                Method = ModbusMethods.ReadHoldingRegisters,
                Value = 0
            },
            new()
            {
                DeviceId = 1,
                Address = 800,
                Method = ModbusMethods.ReadCoils,
                Value = 0
            },
        };

        private ModbusTcpNet _modbusTcpClient => HardwareManager.Instance.PlcXj;

        [RelayCommand]
        private void SendXinJieCmd()
        {
            Logger?.Information($"信捷PLC写指令，长度：{ModbusCmds.Count}");
            foreach (var modbusCmd in ModbusCmds)
            {
                try
                {
                    var isSuccess = _modbusTcpClient.WriteOneRegister(modbusCmd.Address.ToString(), modbusCmd.Value)
                        .IsSuccess;
                    Logger?.Information(
                        $"信捷PLC写指令,address = {modbusCmd.Address},value = {modbusCmd.Value}执行结果:{isSuccess}");


                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    Logger?.Error($"信捷写入错误:{e.Message}");
                }
            }
        }

        [RelayCommand]
        private void ReadXinJieCmd()
        {
            foreach (var modbusCmd in ModbusCmds)
            {
                switch (modbusCmd.Method)
                {
                    case ModbusMethods.ReadCoils:
                        var ret1 = _modbusTcpClient.ReadCoil(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = (short)(ret1[0] ? 1 : 0);
                        break;
                    case ModbusMethods.ReadHoldingRegisters:
                        var ret2 = _modbusTcpClient.Read(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = ret2[0];
                        break;
                    case ModbusMethods.ReadInputRegisters:
                        var ret3 = _modbusTcpClient.Read(modbusCmd.Address.ToString(), 1).Content.ToArray();
                        modbusCmd.Value = ret3[0];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Thread.Sleep(50);
            }
        }

        #endregion
    }

    public class ModbusCmd
    {
        public int DeviceId { get; set; }
        public short Address { get; set; }
        public ModbusMethods Method { get; set; }
        public short Value { get; set; }
    }

    public enum ModbusMethods
    {
        ReadCoils,
        ReadHoldingRegisters,
        ReadInputRegisters,
    }

    public class PrintService
    {
        private PrintQueue _queue;

        public PrintService(string printerName)
        {
            _queue = new PrintQueue(new PrintServer(), printerName);
        }

        public async Task PrintDocumentAsync(byte[] documentData, string documentName)
        {
        }
    }
}