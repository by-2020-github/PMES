using System.Windows;
using PMES_Automatic_Net6.ViewModels;

namespace PMES_Automatic_Net6.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel viewModel;
        private DebugViewModel _debugViewModel;
        private TopPageViewModel _topPageViewModel;
        private SlidePageViewModel _slidePageViewModel;

        public MainView()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            _debugViewModel = new DebugViewModel();
            _topPageViewModel = new TopPageViewModel();
            _slidePageViewModel = new SlidePageViewModel();
            this.DataContext = viewModel;
            this.TopPage.DataContext = _topPageViewModel;
            SlidePage.DataContext = _slidePageViewModel;
            DebugView.DataContext = _debugViewModel;
        }
    }
}