using System.IO.Ports;
using DevExpress.XtraEditors;
using PMES.Core;
using PMES.Core.Managers;
using PMES.Model.settings;
using PMES.Properties;
using Serilog;

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
        RefreshSerialPorts();
    }

    private void RefreshSerialPorts()
    {
        var portNames = SerialPort.GetPortNames().Where(s => !string.IsNullOrEmpty(s)).ToList();
        if (portNames.Count == 0) return;
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

            if (GlobalVar.WeighingMachine.Open(cbxCom.Text, int.Parse(cbxBa.Text)))
            {
                PMES_Settings.Default.COM = cbxCom.Text;
                PMES_Settings.Default.BaudRate = int.Parse(cbxBa.Text);
                PMES_Settings.Default.Save();

                XtraMessageBox.Show("称初始化成功！", "Info:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                _logger.Error($"称初始化失败!");
                XtraMessageBox.Show("称初始化失败！", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
        }
        catch (Exception exception)
        {
            _logger.Error($"称初始化失败：{exception.Message}");
            XtraMessageBox.Show("称初始化失败！", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
       
    }

    private void SerialPortSettings_Load(object sender, EventArgs e)
    {
        RefreshSerialPorts();
    }


}