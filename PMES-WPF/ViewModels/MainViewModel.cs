
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PMES_WPF.Models.Ui;

namespace PMES_WPF.ViewModels
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
