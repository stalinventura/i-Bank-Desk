using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Payment.Bank.Helpers
{
    public class CuotasColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime Fecha = (DateTime)values[0];
            TimeSpan ts = DateTime.Now - Fecha;
            var balance = (float)values[1];

            if (ts.Days > 0 && balance > 1)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
