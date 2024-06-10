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
        public MainView()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            
            this.DataContext = viewModel;
        }
    }
}