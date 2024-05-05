using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PMES_WPF_Automatic_Slide.Models.Ui;

namespace PMES_WPF_Automatic_Slide.ViewModels
{

    [ObservableObject]
    public partial class MainViewModel
    {
        [ObservableProperty]
        private ObservableCollection<MRecord> mRecordsCurrent;

        [ObservableProperty]
        private ObservableCollection<MRecord> mRecordsHistory;
    }
}
