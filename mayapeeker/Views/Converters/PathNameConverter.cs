using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace mayapeeker.Views.Converters
{
    public class PathNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            return ((FileSystemInfo)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
