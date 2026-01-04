using Payment.Bank.Modulos;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.PointOfService;
using System.Drawing.Imaging;

namespace Payment.Bank.Common
{
    class Generic
    {


        public static byte[] RedimensionarImagen(string filePath, int maxWidth, int maxHeight)
        {
            using (var original = new Bitmap(filePath))
            {
                // Calculamos la relación de aspecto
                double ratioX = (double)maxWidth / original.Width;
                double ratioY = (double)maxHeight / original.Height;
                double ratio = Math.Min(ratioX, ratioY);

                int newWidth = (int)(original.Width * ratio);
                int newHeight = (int)(original.Height * ratio);

                using (var resized = new Bitmap(newWidth, newHeight))
                {
                    using (var g = Graphics.FromImage(resized))
                    {
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        g.DrawImage(original, 0, 0, newWidth, newHeight);
                    }

                    using (var ms = new MemoryStream())
                    {
                        resized.Save(ms, ImageFormat.Png); // o ImageFormat.Jpeg
                        return ms.ToArray();
                    }
                }
            }
        }


        public static ImageSource ByteArrayToImageSource(byte[] imageData, int decodeWidth = 0, int decodeHeight = 0)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;

                    if (decodeWidth > 0)
                        bitmap.DecodePixelWidth = decodeWidth;
                    if (decodeHeight > 0)
                        bitmap.DecodePixelHeight = decodeHeight;

                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }


        public static string Encrypt(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";
                string publickey = "1234.abcd";
                string secretkey = "abcd.1234";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        public static string Decrypt(string textToDecrypt)
        {
            try
            {
                string ToReturn = "";
                string publickey = "1234.abcd";
                string secretkey = "abcd.1234";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                return string.Empty;
            }
        }

        public static string CardNumber(String cardNumber)
        {
            var result = Regex.Replace(cardNumber.Replace(" ", "").Replace("-", ""), @"(\w{4})(\w{4})(\w{4})(\w{4})", @"$1 $2 $3 $4");
            return result;
        }

        public static bool HasAccess()
        {
            bool result = false;
            try
            {
                var request = WebRequest.Create(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost);
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        result = true;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {                        
                        result = false;
                        // Si no se encuentra la pagina, el famoso Error 404
                    }
                    else
                    {
                        result = false;
                        // Si fue otro error...
                    }
                }
                else
                {
                    result = false;
                    // Si fue otro error...
                }
            }
            return result;
        }

        public static bool InternetAccess()
        {
            bool result = false;
            try
            {
                var request = WebRequest.Create("https://twitter.com/");
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        result = true;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        result = false;
                        // Si no se encuentra la pagina, el famoso Error 404
                    }
                    else
                    {
                        result = false;
                        // Si fue otro error...
                    }
                }
                else
                {
                    result = false;
                    // Si fue otro error...
                }
            }
            return result;
        }

        public static CultureInfo getCulture(string CultureKey)
        {
            CultureInfo CurrentCulture = new CultureInfo(CultureKey);
            try
            {
                return CurrentCulture;
            }
            catch { }
            return CurrentCulture;
        }

        public static byte[] ImageFromUrl(string ID)
        {
            string barCode = ID.ToString();
            using (Bitmap bitMap = new Bitmap(barCode.Length, 120))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    Convert.ToBase64String(byteImage);
                    return byteImage;
                }
            }
        }

        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }


        public static string ShortName( string Name)
        {
            try
            {               
                string Short = string.Empty;
                foreach (char x in Name.ToCharArray())
                {
                    if (x.ToString() == " ")
                    {
                        break;
                    }
                    Short += x;
                }
                return Short;
            }
            catch { return string.Empty; }
        }

        public static byte[] ImageFromText(string ID)
        {
            string barCode = ID.ToString();
            using (Bitmap bitMap = new Bitmap(barCode.Length * 30, 60))
            {
                using (Graphics graphics = Graphics.FromImage(bitMap))
               {
                    var myFonts = new System.Drawing.Text.PrivateFontCollection();
                    myFonts.AddFontFile(@".\Font\IDAutomationHC39M.TTF");
                    var oFont = new System.Drawing.Font(myFonts.Families[0], 16);

                    //Font oFont = new Font("/Payment.Bank;component/Font/#IDAutomationHC39M", 16);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush blackBrush = new SolidBrush(System.Drawing.Color.Black);
                    SolidBrush whiteBrush = new SolidBrush(System.Drawing.Color.White);
                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    Convert.ToBase64String(byteImage);
                    return byteImage;
                }
            }
        }
    
        public static string GenerarCodigo(int Length)
        {
            Random rnd = new Random();
            int x = 1;
            String Codigo = String.Empty;
            while (x <= Length)
            {
                String tmp = Convert.ToChar(Convert.ToInt32(rnd.Next(47, 254))).ToString();
                if (Regex.IsMatch(tmp.ToString(), @"^[0-9]*$"))
                {
                    Codigo += tmp;
                    x++;
                }
            }
            return Codigo.ToString();
        }

        public static bool ValidarDocumento(string Documento) 
        {
            if (Payment.Bank.Modulos.clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.ValidarDocumento == true)
            {
                string c = Documento.Replace("-", "");
                string Cedula = c.Substring(0, c.Length - 1);
                string Verificador = c.Substring(c.Length - 1, 1);
                float suma = 0;

                int mod, dig, res;
                res = 0;

                if ((Documento.Length < 13) || (Documento.Length > 13))
                {
                    return false;
                }
                for (int i = 0; i < Cedula.Length; i++)
                {
                    mod = 0;
                    if ((i % 2) == 0) mod = 1;
                    else mod = 2;
                    if (int.TryParse(Cedula.Substring(i, 1), out dig))
                    {
                        res = dig * mod;
                    }
                    else
                    {
                        return false;
                    }
                    if (res > 9)
                    {
                        res = Convert.ToInt32(res.ToString().Substring(0, 1)) +
                        Convert.ToInt32(res.ToString().Substring(1, 1));
                    }
                    suma += res;

                }
                float el_numero = (10 - (suma % 10)) % 10;
                if ((el_numero.ToString() == Verificador) && (Cedula.Substring(0, 3) != "000"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static string Encryption(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public static string Decoding(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            string Texto = Encoding.UTF8.GetString(bytes);
            return Texto;
        }
        
    }
}

