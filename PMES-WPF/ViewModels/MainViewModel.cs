using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PMES_WPF.Models.Ui;

namespace PMES_WPF.ViewModels
{
    [ObservableObject]
    public partial class MainViewModel
    {
        [ObservableProperty] private ObservableCollection<MRecord> _mRecordsCurrent;

        [ObservableProperty] private ObservableCollection<MRecord> _mRecordsHistory;

        public string PreScanCode { get; set; } = "s";
        [ObservableProperty]
        private string? name;

        [RelayCommand]
        private void SayHello()
        {
            Console.WriteLine("Hello");
        }
    }
}