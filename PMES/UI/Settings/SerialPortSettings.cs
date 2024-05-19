using System.IO.Ports;
using DevExpress.XtraEditors;
using PMES.Core;
using PMES.Core.Managers;
using PMES.Model.settings;
using Serilog;
using SICD_Automatic.Core;

namespace PMES.UI.Settings;

public partial class SerialPortSettings : XtraForm
{
    private readonly ILogger _logger;
    private readonly IFreeSql _freeSql = FreeSqlManager.FSql;

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
            _freeSql.InsertOrUpdate<SystemSettings>().SetSource(new SystemSettings
            {
                Id = 1,
                SerialPort = cbxCom.Text,
                BaudRate = int.Parse(cbxBa.Text)
            }).ExecuteAffrows();
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
        var portNames = SerialPort.GetPortNames().Where(s=>!string.IsNullOrEmpty(s)).ToList();
        if (portNames.Count == 0) return;
        cbxCom.Properties.Items.Clear();
        cbxCom.Properties.Items.AddRange(portNames);
    }
}