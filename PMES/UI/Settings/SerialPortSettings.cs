using System.IO.Ports;
using DevExpress.XtraEditors;
using PMES.Core;
using PMES.Core.Managers;
using Serilog;
using SICD_Automatic.Core;

namespace PMES.UI.Settings;

public partial class SerialPortSettings : XtraForm
{
    private readonly ILogger _logger;

    public SerialPortSettings()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
        _logger = SerilogManager.GetOrCreateLogger();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        var portNames = SerialPort.GetPortNames();
        if (portNames == null) return;
        cbxCom.Properties.Items.Clear();
        cbxCom.Properties.Items.AddRange(portNames);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(cbxCom.Text) || string.IsNullOrEmpty(cbxBa.Text))
        {
            XtraMessageBox.Show("请先输入端口号或波特率！", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            GlobalVar.WeighingMachine?.Close();
            GlobalVar.WeighingMachine = new WeighingMachine(_logger);
            GlobalVar.WeighingMachine.Open(cbxCom.Text, int.Parse(cbxBa.Text));
            XtraMessageBox.Show("称初始化成功！", "Info:", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        catch (Exception exception)
        {
            _logger.Error($"称初始化失败：{exception.Message}");
            XtraMessageBox.Show("称初始化失败！", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
       
    }

    private void SerialPortSettings_Load(object sender, EventArgs e)
    {
        var portNames = SerialPort.GetPortNames();
        if (portNames == null) return;
        cbxCom.Properties.Items.Clear();
        cbxCom.Properties.Items.AddRange(portNames);
    }
}