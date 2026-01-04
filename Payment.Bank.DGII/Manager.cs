using Payment.Bank.DGII.Interfaces;
using Payment.Bank.DGII.Models;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Payment.Bank.DGII
{
    public class Manager : IServicios
    {
        private const string RequestUrlConsultaRnc = "https://dgii.gov.do/app/WebApps/ConsultasWeb2/ConsultasWeb/consultas/rnc.aspx";
        private const string RequestUrlConsultaCiudadanos = "https://dgii.gov.do/app/WebApps/ConsultasWeb2/ConsultasWeb/consultas/ciudadanos.aspx";
        private const string RequestUrlConsultaNcf = "https://dgii.gov.do/app/WebApps/ConsultasWeb2/ConsultasWeb/consultas/ncf.aspx";

        public OperationResult ConsultarRnc(string Rnc)
        {
            CookieContainer cookieContainer = new CookieContainer();

            var htmlDocument = LoadPage(RequestUrlConsultaRnc, cookieContainer);

            string __VIEWSTATE = htmlDocument.DocumentNode.SelectNodes("//input[@name='__VIEWSTATE']").First().Attributes["value"].Value;
            string __EVENTVALIDATION = htmlDocument.DocumentNode.SelectNodes("//input[@name='__EVENTVALIDATION']").First().Attributes["value"].Value;
            string __VIEWSTATEGENERATOR = htmlDocument.DocumentNode.SelectNodes("//input[@name='__VIEWSTATEGENERATOR']").First().Attributes["value"].Value;

            string formData = "";
            formData += "ctl00$smMain=ctl00$cphMain$upBusqueda|ctl00$cphMain$btnBuscarPorRNC&";
            formData += "ctl00$cphMain$txtRNCCedula=" + Rnc + "&";
            formData += "ctl00$cphMain$txtRazonSocial=&";
            formData += "__EVENTTARGET=&";
            formData += "__EVENTARGUMENT=&";
            formData += "__VIEWSTATEGENERATOR=" + __VIEWSTATEGENERATOR + "&";

            formData += "__VIEWSTATE=" + HttpUtility.UrlEncode(__VIEWSTATE) + "&";
            formData += "__EVENTVALIDATION=" + HttpUtility.UrlEncode(__EVENTVALIDATION) + "&";
            formData += "__ASYNCPOST=true&";
            formData += "ctl00$cphMain$btnBuscarPorRNC=BUSCAR";

            var responseText = PostBack(RequestUrlConsultaRnc, formData, cookieContainer);
            var xDocument = ParseToXDocument(responseText);

            var response = new OperationResult();
            var datosContribuyenteElement = xDocument.XPathSelectElement("//*[@id='cphMain_dvDatosContribuyentes']");
            if (datosContribuyenteElement != null && datosContribuyenteElement.Elements().Count() > 0)
            {
                response.Rnc = xDocument.XPathSelectElement("//tr[1]/td[2]")?.Value;
                response.Empresa = xDocument.XPathSelectElement("//tr[2]/td[2]")?.Value;
                response.Siglas = xDocument.XPathSelectElement("//tr[3]/td[2]")?.Value;
                response.Categoría = xDocument.XPathSelectElement("//tr[5]/td[4]")?.Value;
                response.RegimenDePagos = xDocument.XPathSelectElement("//tr[5]/td[2]")?.Value;
                response.Estado = xDocument.XPathSelectElement("//tr[6]/td[2]")?.Value;
                response.Actividad = xDocument.XPathSelectElement("//tr[7]/td[2]")?.Value;
                response.Direccion = xDocument.XPathSelectElement("//tr[8]/td[2]")?.Value;
                response.Success = true;
            }
            else
            {
                response.Message = xDocument.XPathSelectElement("//*[@id='cphMain_lblInformacion']")?.Value;
            }

            return response;
        }

        private static XDocument ParseToXDocument(string content)
        {
            int startIndex = content.IndexOf("<");
            int finalIndex = content.LastIndexOf(">");
            content = content.Substring(startIndex, finalIndex - startIndex + 1);

            // Extraemos los valores del la tabla HTML
            return XDocument.Parse($"<!DOCTYPE doctypeName [" +
                $"<!ENTITY nbsp \"&#160;\">" +
                $"<!ENTITY aacute \"&#225;\">" +
                $"<!ENTITY eacute \"&#233;\">" +
                $"<!ENTITY iacute \"&#237;\">" +
                $"<!ENTITY oacute \"&#243;\">" +
                $"<!ENTITY uacute \"&#250;\">" +
                $"]> " + content);
        }

        private static HtmlDocument LoadPage(string requestUrl, CookieContainer container)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var httpRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            httpRequest.CookieContainer = container;

            var httpResponse = httpRequest.GetResponse();
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(httpResponse.GetResponseStream());
            httpResponse.Close();

            return htmlDocument;
        }

        private static string PostBack(string requestUriString, string postData, CookieContainer container)
        {
            var requestData = Encoding.UTF8.GetBytes(postData);

            var request = WebRequest.Create(requestUriString) as HttpWebRequest;

            request.Referer = requestUriString;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.182 Safari/537.36 Edg/88.0.705.74";
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Origin", "https://dgii.gov.do");

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = requestData.Length;
            request.CookieContainer = container;
            request.Timeout = System.Threading.Timeout.Infinite;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestData, 0, requestData.Length);
                requestStream.Close();
            }

            string responseText = "";
            using (var response = request.GetResponse())
            using (var responseStreamReader = new StreamReader(response.GetResponseStream()))
            {
                responseText = responseStreamReader.ReadToEnd();
                responseStreamReader.Close();
                response.Close();
            }

            return responseText;
        }
    }
}
