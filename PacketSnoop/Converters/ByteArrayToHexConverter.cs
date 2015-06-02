using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PacketSnoop.Converters
{
    public class ByteArrayToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bytes = value as byte[];
            if (bytes == null)
            {
                return string.Empty;
            }

            return string.Join(" ", bytes.Select(b => b.ToString("X")));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
