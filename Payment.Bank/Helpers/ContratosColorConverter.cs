using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace  Payment.Bank.Helpers
{
    public class ContratosColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime Fecha = (DateTime)values[0];
            TimeSpan ts = DateTime.Now - Fecha;
            
            if (ts.Days <= 0)
            {
                return new SolidColorBrush(Colors.Black);
            }
            else
            {
                switch(values[1])
                {
                    case 1:
                        { return new SolidColorBrush(Colors.Red); }
                    case 2:
                        { return new SolidColorBrush(Colors.Black); }
                    case 3:
                        { return new SolidColorBrush(Colors.Blue); }
                    default:
                        {
                            return new SolidColorBrush(Colors.Black);                            
                        }
                }
               
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
