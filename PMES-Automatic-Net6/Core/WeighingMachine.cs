using System.IO.Ports;
using Serilog;

namespace PMES_Automatic_Net6.Core;

public class WeighingMachine
{
    private readonly ILogger _logger;

    private int _port;

    private string _portName;
    private SerialPort? _serialPort;

    public Action<double> OnGetWeight { get; set; }
    private List<double> _buffer = new List<double>();
    public bool IsOpen  => _serialPort?.IsOpen ?? false;

    public WeighingMachine(ILogger logger)
    {
        _logger = logger;
        _logger?.Verbose("WeighingMachine 对象已加载...");
    }

    public bool Open(
        string portName,
        int baudRate = 19200,
        Parity parity = Parity.None,
        int dataBits = 8,
        StopBits stopBits = StopBits.One)
    {
        _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        _serialPort.NewLine = "\r\n";
        _serialPort.ReadTimeout = 3000;
        _serialPort.WriteTimeout = 3000;
        try
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            return true;
        }
        catch (Exception e)
        {
            _logger?.Error($"电子秤串口打开失败！{e}");
            return false;
        }
    }


    public bool Close()
    {
        if (_serialPort == null)
        {
            return true;
        }

        if (!_serialPort.IsOpen) return true;
        try
        {
            _serialPort.Close();
            return true;
        }
        catch (Exception e)
        {
            _logger?.Error($"电子秤串口关闭失败！{e}");
            return false;
        }
    }

    public void AddEvent()
    {
        if (_serialPort != null) _serialPort.DataReceived += SerialPortOnDataReceived;
    }

    public void RemoveEvent()
    {
        if (_serialPort != null) _serialPort.DataReceived -= SerialPortOnDataReceived;
    }

    public async Task<double> GetWeight()
    {
        return await ReadWeight();

        var start = DateTime.Now;
        var buffer = new List<double>();
        while ((DateTime.Now - start).TotalSeconds < 5)
        {
            await Task.Delay(200);
            var ret = await ReadWeight();
            _logger?.Verbose($"读取到重量:{ret}");
            if (ret <= 0)
            {
                continue;
            }

            if (_buffer.Count < 3)
            {
                buffer.Add(ret);
                continue;
            }

            if (buffer.Max() - buffer.Min() < 0.5)
            {
                return buffer.Average();
            }

            buffer.RemoveAt(0);
            buffer.Add(ret);
        }

        return -1;
    }

    private async Task<double> ReadWeight()
    {
        return await Task.Run(async () =>
        {
            //查询稳态重量 如果超过三秒没有稳定则放弃
            try
            {
                if (_serialPort == null) return 0;
                _serialPort.WriteLine("S ");
                var readLine = _serialPort.ReadLine();
                if (string.IsNullOrEmpty(readLine))
                {
                    return 0;
                }

                _logger?.Verbose($"收到消息:{readLine}");
                var trim = readLine.Replace(" ", "");
                if (trim.StartsWith("SS")) //稳定正常返回
                {
                    var replace = trim.Replace("SS", "");
                    var unit = replace.Substring(replace.Length - 2, 2);
                    var substring = replace.Substring(0, replace.Length - 2);
                    if (double.TryParse(substring, out double w))
                    {
                        return w;
                    }
                }
                else if (trim.StartsWith("SI")) //收到了命令 但是由于不稳定不能执行
                {
                    return 0;
                }
                else if (trim.StartsWith("S+")) //设备过载
                {
                    return 0;
                }
                else if (trim.StartsWith("S-")) //设备欠载
                {
                    return 0;
                }

                return 0;
            }
            catch (Exception e)
            {
                return 0.00;
            }
        });
    }

    private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var s = sender as SerialPort;
        var msg = s?.ReadLine();
        if (msg != null)
        {
            if (_buffer.Count < 3)
            {
                _buffer.Add(new Random().NextDouble() * 100);
                return;
            }

            if (_buffer.Max() - _buffer.Min() < 0.5)
            {
                OnGetWeight?.Invoke(_buffer.Average());
                _buffer.Clear();
            }
            else
            {
                _buffer.RemoveAt(0);
            }
        }

        _logger?.Verbose($"收到数据：{msg}");
    }
}