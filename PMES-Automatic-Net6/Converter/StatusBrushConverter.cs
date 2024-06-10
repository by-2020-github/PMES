using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using PMES_Automatic_Net6.ViewModels;

namespace PMES_Automatic_Net6.Converter
{
    public class StatusBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var radialBrush = new RadialGradientBrush()
            {
                GradientOrigin = new Point(0.5, 0.5),
                RadiusX = 0.5,
                RadiusY = 0.5,
            };
            if (value is not Status status) return radialBrush;
            switch (status)
            {
                case Status.Unknown:
                    break;
                case Status.Running:
                    break;
                case Status.Pause:
                    break;
                case Status.Stop:
                    break;
                case Status.Error:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();0
            }

            return radialBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}