using System;
using System.Globalization;
using System.Windows.Data;

namespace Chain
{
	public class DoubleTrimmingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var stringParam = parameter?.ToString();
			int.TryParse(stringParam, out var b);
			return !(value is double a) ? (object)null : Math.Round((double)a, b);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}