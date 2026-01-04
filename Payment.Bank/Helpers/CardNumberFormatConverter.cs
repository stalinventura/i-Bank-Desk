using Payment.Bank.Modulos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace  Payment.Bank.Helpers
{
    public class CardNumberFormatConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string text =(string)value;
                if ( text == null) return "0000 0000 0000 0000";
                var result = Regex.Replace(text.ToString().Replace(" ", "").Replace("-", ""), @"(\w{4})(\w{4})(\w{4})(\w{4})", @"$1 $2 $3 $4");

                return result;
            }
            catch { return "0000 0000 0000 0000"; }
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
