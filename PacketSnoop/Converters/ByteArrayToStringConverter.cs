using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PacketSnoop.Converters
{
    public class ByteArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bytes = value as byte[];
            if (bytes == null)
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(bytes);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
