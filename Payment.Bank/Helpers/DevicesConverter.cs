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
    public class DevicesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string device = value.ToString();
                for (int i = 0; i <= 1000; i++)
                {
                    if (Common.FingerPrint.GetHash(i.ToString()) == device)
                    {
                        return i;
                    }
                }
                return 0;
            }
            catch { return 0; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
