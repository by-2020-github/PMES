using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentModbus;
using PMES_Respository.DataStruct;
using S7.Net.Types;
using S7.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using PMES_Common;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class DebugViewModel : ObservableObject
    {
        public string Ip { get; set; } = PMESConfig.Default.PlcIp;
        public short Rack { get; set; } = (short)PMESConfig.Default.PlcRack;
        public short Slot { get; set; } = (short)PMESConfig.Default.PlcSlot;

        public string IpJieDa { get; set; } = PMESConfig.Default.PlcXinJieIp;
        public int Port { get; set; } = PMESConfig.Default.PlcXinJiePort;


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
        private S7.Net.Plc plc;


        public DebugViewModel()
        {
            var types = typeof(PmesCmdUnStacking).Assembly.GetTypes();
            CmdStruct = types.Where(s => s.Namespace == "PMES_Respository.DataStruct" && s.Name != "PmesDataItemList")
                .Select(s => s.Name).ToList();
            CmdStrStruct = typeof(PmesDataItemList).GetProperties().Select(s => s.Name).ToList();
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
                _modbusTcpClient.Connect(new IPEndPoint(IPAddress.Parse(IpJieDa), int.Parse(Port.ToString())));
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
            plc = new Plc(S7.Net.CpuType.S71200, Ip, Rack, Slot);
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
            plc.Write(StrCmdDataItems.ToArray());
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

        private ModbusTcpClient _modbusTcpClient;

        [RelayCommand]
        private void SendXinJieCmd()
        {
            foreach (var modbusCmd in ModbusCmds)
            {
                switch (modbusCmd.Method)
                {
                    case ModbusMethods.ReadCoils:
                        _modbusTcpClient.WriteSingleCoil(modbusCmd.DeviceId, modbusCmd.Address,
                            modbusCmd.Value == 0);

                        break;
                    case ModbusMethods.ReadHoldingRegisters:
                        _modbusTcpClient.WriteSingleRegister(modbusCmd.DeviceId, modbusCmd.Address,
                            (short)modbusCmd.Value);

                        break;
                    case ModbusMethods.ReadInputRegisters:
                        _modbusTcpClient.WriteSingleRegister(modbusCmd.DeviceId, modbusCmd.Address,
                            (short)modbusCmd.Value);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Thread.Sleep(50);
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
                        var ret1 = _modbusTcpClient.ReadCoils(modbusCmd.DeviceId, modbusCmd.Address, 1).ToArray();
                        modbusCmd.Value = ret1[0];
                        break;
                    case ModbusMethods.ReadHoldingRegisters:
                        var ret2 = _modbusTcpClient.ReadHoldingRegisters<short>(modbusCmd.DeviceId, modbusCmd.Address,
                            1).ToArray();
                        modbusCmd.Value = ret2[0];
                        break;
                    case ModbusMethods.ReadInputRegisters:
                        var ret3 = _modbusTcpClient.ReadInputRegisters<short>(modbusCmd.DeviceId, modbusCmd.Address,
                            1).ToArray();
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
        public int Address { get; set; }
        public ModbusMethods Method { get; set; }
        public int Value { get; set; }
    }

    public enum ModbusMethods
    {
        ReadCoils,
        ReadHoldingRegisters,
        ReadInputRegisters,
    }
}