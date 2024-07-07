using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PMES_Automatic_Net6.ViewModels;
using PMES_Respository.DataStruct;
using S7.Net;

namespace PMES_Automatic_Net6.Views
{
    /// <summary>
    /// PLCDebugView.xaml 的交互逻辑
    /// </summary>
    public partial class PLCDebugView : Window
    {
        private Plc _plc;

        public PLCDebugView()
        {
            InitializeComponent();
            var plcDebugViewModel = new PLCDebugViewModel();
            this.DataContext = plcDebugViewModel;
            _plc = plcDebugViewModel.GetPlc;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CbxClassType?.SelectedValue.ToString())
            {
                case "PmesCmdUnStacking":
                    PropCmdSend.SelectedObject = new PmesCmdUnStacking();
                    break;
                case "PlcCmdUnStacking":
                    PropCmdSend.SelectedObject = new PlcCmdUnStacking();
                    break;
                case "PmesStacking":
                    PropCmdSend.SelectedObject = new PmesStacking();
                    break;
                default:
                    PropCmdSend.SelectedObject = new object();
                    break;
            }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_plc.IsConnected)
            {
                MessageBox.Show("先连接Plc");
                return;
            }

            var db = 500;
            //todo: 补全switch
            switch (CbxClassType.Text)
            {
                case "PmesCmdUnStacking":
                    db = 501;
                    break;
                case "PlcCmdUnStacking":
                    db = 502;
                    if (PropCmdSend.SelectedObject is PlcCmdUnStacking plcCmd)
                    {
                        db += plcCmd.DeviceId - 202;
                    }

                    break;
                case "PmesStacking":
                    db = 540;
                    break;
            }

            _plc.WriteClass(PropCmdSend.SelectedObject, db);
        }

        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_plc.IsConnected)
            {
                MessageBox.Show("先连接Plc");
                return;
            }

            var db = 500;
            //todo: 补全switch
            switch (CbxClassType.Text)
            {
                case "PmesCmdUnStacking":
                    db = 501;
                    break;

                case "PlcCmdUnStacking":
                    db = 502;
                    if (PropCmdSend.SelectedObject is PlcCmdUnStacking plcCmd)
                    {
                        db += plcCmd.DeviceId - 202;
                    }

                    break;
                case "PmesStacking":
                    db = 540;
                    break;
            }

            _plc.ReadClass(PropCmdRead.SelectedObject, db);
        }
    }
}