using System.Windows;
using PMES_Automatic_Net6.Core;
using PMES_Automatic_Net6.Core.Managers;
using PMES_Automatic_Net6.ViewModels;
using PMES_Respository.DataStruct;
using S7.Net;

namespace PMES_Automatic_Net6.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainView : Window
    {
        public Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger();
        private MainViewModel viewModel;
        private DebugViewModel _debugViewModel;
        private TopPageViewModel _topPageViewModel;
        private SlidePageViewModel _slidePageViewModel;

        public MainView()
        {
            InitializeComponent();

            //var pmesDataItemList = new PmesDataItemList();

            //if (!HardwareManager.Instance. ComparerDataList(pmesDataItemList.PmesWeightAndBarCode.ToList(),
            //        GlobalVar.PmesDataItems.PmesWeightAndBarCode.ToList()))
            //{
            //    GlobalVar.PmesDataItems.PmesWeightAndBarCode = pmesDataItemList.PmesWeightAndBarCode;
            //}

            //pmesDataItemList.PmesWeightAndBarCode[0].Value = 1;
            //if (!HardwareManager.Instance.ComparerDataList(pmesDataItemList.PmesWeightAndBarCode.ToList(),
            //        GlobalVar.PmesDataItems.PmesWeightAndBarCode.ToList()))
            //{
            //    GlobalVar.PmesDataItems.PmesWeightAndBarCode = pmesDataItemList.PmesWeightAndBarCode;
            //}

            //if (!HardwareManager.Instance.ComparerDataList(pmesDataItemList.PmesWeightAndBarCode.ToList(),
            //        GlobalVar.PmesDataItems.PmesWeightAndBarCode.ToList()))
            //{
            //    GlobalVar.PmesDataItems.PmesWeightAndBarCode = pmesDataItemList.PmesWeightAndBarCode;
            //}


            if (!HardwareManager.Instance.InitPlc())
            {
                MessageBox.Show("打开PLC失败，请检查配置文件中的PLC配置，即将退出！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            else
            {
                //MessageBox.Show("打开PLC成功！", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            viewModel = new MainViewModel();
            _debugViewModel = new DebugViewModel();
            _topPageViewModel = new TopPageViewModel();
            _slidePageViewModel = new SlidePageViewModel();
            this.DataContext = viewModel;
            this.TopPage.DataContext = _topPageViewModel;
            SlidePage.DataContext = _slidePageViewModel;
            DebugView.DataContext = _debugViewModel;
            Logger.Information("启动成功！");
        }
    }
}