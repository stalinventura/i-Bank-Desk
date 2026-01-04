using Payment.Bank.Modulos;
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
    public class TipoSolicitudesConverter : IMultiValueConverter
{
        // Este método convierte los valores a un solo valor (en este caso, un texto).
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return null;

            var Completo = values[0] as int?;
            int TipoSolicitudID = (int)values[1];
            if (Completo.HasValue && TipoSolicitudID==4)
            {
                DAL.Context db = new DAL.Context();

                return Completo;
            }

            return Completo;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
