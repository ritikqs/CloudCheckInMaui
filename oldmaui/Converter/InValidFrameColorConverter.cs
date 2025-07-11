using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CloudCheckInMaui.Converter
{
    public class InValidFrameColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // In MAUI, handle validation error styling
            if (value is bool isValid && isValid)
            {
                return Colors.Red;
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 