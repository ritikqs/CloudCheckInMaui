using Microsoft.Maui.Controls;
using System;
using System.Globalization;
using CloudCheckInMaui.Models;

namespace CloudCheckInMaui.Converter
{
    public class LeaveStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            
            if (value is string strValue)
            {
                if (Enum.TryParse<HolidayStatus>(strValue, true, out var status))
                {
                    return status switch
                    {
                        HolidayStatus.Pending => "Pending",
                        HolidayStatus.Approved => "Approved",
                        HolidayStatus.Rejected => "Rejected",
                        _ => "Unknown",
                    };
                }
            }
            else if (value is HolidayStatus status)
            {
                return status switch
                {
                    HolidayStatus.Pending => "Pending",
                    HolidayStatus.Approved => "Approved",
                    HolidayStatus.Rejected => "Rejected",
                    _ => "Unknown",
                };
            }
            
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 