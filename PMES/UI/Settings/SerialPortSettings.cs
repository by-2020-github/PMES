using System.IO.Ports;
using DevExpress.XtraEditors;

namespace PMES.UI.Settings;

public partial class SerialPortSettings : XtraForm
{
    public SerialPortSettings()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
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
    }
}