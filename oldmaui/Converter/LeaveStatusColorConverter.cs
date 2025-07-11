using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CloudCheckInMaui.Converter
{
    public class LeaveStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Colors.Gray;
            
            int status = System.Convert.ToInt32(value);
            return status switch
            {
                1 => Colors.Orange,  // Pending
                2 => Colors.Green,   // Approved
                3 => Colors.Red,     // Rejected
                _ => Colors.Gray,    // Unknown
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 