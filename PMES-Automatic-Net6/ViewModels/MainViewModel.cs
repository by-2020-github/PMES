using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
       
    }

    public enum Status
    {
        Unknown,
        Running,
        Pause,
        Stop,
        Error
    }
}