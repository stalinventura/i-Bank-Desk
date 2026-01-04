
using System;
using System.Net;
using System.Text;

namespace Payment.Bank.Core.Common
{
    class Generic
    {


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

        public static String DocumentFormat(string Documento)
        {
            String DocumentFormat = "";
            try
            {
                Documento = Documento.Replace("-", "");
                if (Documento.Length < 10)
                {
                    DocumentFormat = Documento;
                }
                else
                {
                    if (Documento.Length == 11)
                    {
                        DocumentFormat = Documento.Substring(0, 3) + "-" + Documento.Substring(3, 7) + "-" + Documento.Substring(10, 1);
                    }
                    else
                    {
                        DocumentFormat = Documento.Substring(0, 3) + "-" + Documento.Substring(3, 7) + "-" + Documento.Substring(10, 1);
                    }
                }
                return DocumentFormat;
            }
            catch
            {
                return "";
            }
        }

        public static bool ValidarDocumento(string Documento)
        {
            try
            {

                string c = Documento.Replace("-", "");
                string Cedula = c.Substring(0, c.Length - 1);
                string Verificador = c.Substring(c.Length - 1, 1);
                decimal suma = 0;

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
                decimal el_numero = (10 - (suma % 10)) % 10;
                if ((el_numero.ToString() == Verificador) && (Cedula.Substring(0, 3) != "000"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
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

