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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.XtraReports.Native;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Automatic_Net6.ViewModels;

namespace PMES_Automatic_Net6.Views
{
    /// <summary>
    /// TopPage.xaml 的交互逻辑
    /// </summary>
    public partial class TopPage : UserControl
    {
        public TopPage()
        {
            InitializeComponent();
            LogGrid.Children.Add(SerilogManager.LogViewTextBox);
        }
    }
}