
using Payment.Host.DAL;
using System;
using System.Net;
using System.Text;


namespace Payment.Host.Common
{
    class Generic
    {
        //public static bool HasAccess()
        //{
        //    bool result = false;
        //    try
        //    {
        //        Context cx = new Context();
        //        var request = WebRequest.Create(cx.Database.Connection.DataSource);
        //        using (var response = request.GetResponse())
        //        {
        //            using (var responseStream = response.GetResponseStream())
        //            {
        //                result = true;
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Status == WebExceptionStatus.ProtocolError &&
        //            ex.Response != null)
        //        {
        //            var resp = (HttpWebResponse)ex.Response;
        //            if (resp.StatusCode == HttpStatusCode.NotFound)
        //            {                        
        //                result = false;
        //                // Si no se encuentra la pagina, el famoso Error 404
        //            }
        //            else
        //            {
        //                result = false;
        //                // Si fue otro error...
        //            }
        //        }
        //        else
        //        {
        //            result = false;
        //            // Si fue otro error...
        //        }
        //    }
        //       return result;
        //}

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

