using System;
using System.Globalization;
using System.Windows.Data;

namespace Chain
{
	public class BoolToOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
				return value == null ? 1 : 0;

			var x = (bool)value;
			return x ? 1 : 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}