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
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xff,0xff,0xf9),
                        Offset = 0
                    });
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xDA, 0xD4, 0xC8),
                        Offset = 1
                    });
                    break;
                case Status.Running:
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xff, 0xff, 0xf9),
                        Offset = 0
                    });
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0x00, 0xFF, 0x00),
                        Offset = 1
                    });
                    break;
                case Status.Pause:
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xff, 0xff, 0xf9),
                        Offset = 0
                    });
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xFF, 0xFF, 0x00),
                        Offset = 1
                    });
                    break;
                case Status.Stop:
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xff, 0xff, 0xf9),
                        Offset = 0
                    });
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xFF, 0x00, 0x00),
                        Offset = 1
                    });
                    break;
                case Status.Error:
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xff, 0xff, 0xf9),
                        Offset = 0
                    });
                    radialBrush.GradientStops.Add(new GradientStop
                    {
                        Color = Color.FromRgb(0xFF, 0x00, 0x00),
                        Offset = 1
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return radialBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}