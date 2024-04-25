using DevExpress.XtraBars.Docking;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
namespace PMES.Core
{
    public class WeighingMachine
    {
        private SerialPort _serialPort;

        private int _port;

        private string _portName;

        private ILogger _logger;

        public WeighingMachine(ILogger logger)
        {
            _logger = logger;
        }

        public void Open(
            string portName,
            int baudRate = 9600,
            Parity parity = Parity.None,
            int dataBits = 8,
            StopBits stopBits = StopBits.One)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _serialPort.NewLine = "\r\n";
            _serialPort.ReadTimeout = 3000;
            _serialPort.WriteTimeout = 3000;
        }

        public void Close()
        {
            _serialPort?.Close();
        }

        public void Start()
        {
            _serialPort.DataReceived += SerialPortOnDataReceived;
        }

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var s = sender as SerialPort;
            var msg = s?.ReadLine();
            _logger?.Verbose($"收到数据：{msg}");  
        }
    }
}