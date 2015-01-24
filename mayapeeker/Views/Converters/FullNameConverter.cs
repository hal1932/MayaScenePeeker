using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace mayapeeker.Views.Converters
{
    public class FullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            return ((FileSystemInfo)value).FullName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var path = (string)value;
            if (File.Exists(path))
            {
                return new FileInfo(path);
            }
            else if (Directory.Exists(path))
            {
                return new DirectoryInfo(path);
            }
            throw new ArgumentException(string.Format("path ({0}) is not found", path));
        }
    }
}
