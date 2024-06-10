using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Data.XtraReports.ReportGeneration;
using DevExpress.Xpf.Printing;
using PMES_WPF.ViewModels;

namespace PMES_WPF.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //var report = new XtraReport1();
            //report.DataSource = new List<object>()
            //{
              
            //};
            //PrintHelper.ShowPrintPreview(this,report);

            Application.Current.Shutdown();
        }
    }
}
