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
    public class RiesgoColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Riesgo = (double)value;
            if (Riesgo >=90 && Riesgo <=100)
            {
                return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF0000"));
            }
            else
            {
                if (Riesgo >= 80 && Riesgo <= 89)
                {
                    return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF6600"));
                }
                else
                {
                    if (Riesgo >= 70 && Riesgo <= 79)
                    {
                        return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF9900"));
                    }
                    else
                    {
                        if (Riesgo >= 60 && Riesgo <= 69)
                        {
                            return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFCC00"));
                        }
                        else
                        {
                            if (Riesgo >= 50 && Riesgo <= 59)
                            {
                                return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFF00"));
                            }
                            else
                            {
                                if (Riesgo >= 40 && Riesgo <= 49)
                                {
                                    return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#CCFF00"));
                                }
                                else
                                {
                                    if (Riesgo >= 30 && Riesgo <= 39)
                                    {
                                        return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#99FF00"));
                                    }
                                    else
                                    {
                                        if (Riesgo >= 20 && Riesgo <= 29)
                                        {
                                            return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#66FF00"));
                                        }
                                        else
                                        {
                                            if (Riesgo >= 10 && Riesgo <= 19)
                                            {
                                                return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#33FF00"));
                                            }
                                            else
                                            {
                                                return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#008f39"));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
