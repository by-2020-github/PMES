using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMES_Automatic_Net6.Model
{
    public class TopGridModel : ObservableObject
    {
        public string? BarCode { get; set; }
        public string? ProductCode { get; set; }
        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public double NetWeight { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string Result { get; set; } = "";
        public string Reason { get; set; } = "";
        public MyProductTaskInfo? MyProductTaskInfo { get; set; }  
    }
}
