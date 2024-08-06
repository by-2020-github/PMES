using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMES_Automatic_Net6.Core.Managers;

namespace PMES_Automatic_Net6.ViewModels
{
    public partial class SlidePageViewModel : ObservableObject
    {
        public Serilog.ILogger Logger => SerilogManager.GetOrCreateLogger();
    }
}