using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SimplyGradients.Mvvm.Converters
{
    public class IgnoreAlphaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color= (Color)value;
            color.A = 255;
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
