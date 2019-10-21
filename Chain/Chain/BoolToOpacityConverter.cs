using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chain
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool x = (bool)value;

                return x ? 1 : 0;

            }
            else
            {

                return value == null ? 1 : 0;

            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}