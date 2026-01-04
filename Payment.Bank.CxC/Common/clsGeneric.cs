//using Microsoft.SqlServer.Management.Common;
//using Microsoft.SqlServer.Management.Smo;
using Payment.Bank.Core;
using Payment.Bank.Core.Common;
using Payment.Bank.Core.Model;
using Payment.Bank.Entity;
using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.Xml.Linq;
using System.Net;

namespace Payment.Bank.CxC.Common
{

    class clsGeneric
    {
        clsEmpresasBE BE = new clsEmpresasBE();
        string result = string.Empty;
        string CurrentTable;
        int Current = 0;

        public static String GetCheckStatusDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "CheckStatusDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void CheckStatusDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "CheckStatusDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetHostHistoryDataDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "HostHistoryDataDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void HostHistoryDataDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "HostHistoryDataDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetInteresCalculateDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "InteresCalculateDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void InteresCalculateDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "InteresCalculateDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetMoraCalculateDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "MoraCalculateDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void MoraCalculateDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "MoraCalculateDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetLegalCalculateDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "LegalCalculateDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void LegalCalculateDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "LegalCalculateDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public void CalcularInteres(DateTime _Date)
        {
            try
            {
                Manager db = new Manager();
                string Usuario = Environment.MachineName.ToString();
                Console.WriteLine("*********** Calculando Intereses al {0} ***********", _Date);
                Console.WriteLine("");

                db.CalcularInteres(_Date, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async System.Threading.Tasks.Task AVencerSMS(DateTime _Date)
        {
            try
            {
                Manager db = new Manager();

                Console.WriteLine("*********** Notificando cuotas a vencer Electronicamente al {0} ***********", DateTime.Now);
                string msg = db.EtiquetasGet().Where(x => x.Nombre == "msgNotifyCuotas").FirstOrDefault().Texto;

                bool SendSMS = false;
                bool SendMail = false;

                DateTime Desde = new DateTime(_Date.Year, _Date.Month, _Date.Day).Add(new TimeSpan(0, 0, 0));
                DateTime Hasta = new DateTime(_Date.Year, _Date.Month, _Date.Day).Add(new TimeSpan(23, 59, 59));

                foreach (var row in db.ContratosGet())
                {
                    clsCuotasView Cuotas = new clsCuotasView();
                    Cuotas.SetDataSource(row.ContratoID);
                    var result = Cuotas.GetGroup().OrderBy(x => x.Numero).ToList();

                    string payment = db.EmpresasGetByEmpresaID(row.Solicitudes.Clientes.Sucursales.EmpresaID).Payment;
                    var rowXML = Decoding(payment == null ? "" : payment);
                    bool GatewaySMS = false;
                    if (!string.IsNullOrEmpty(rowXML))
                    {
                        String xml = rowXML;
                        XDocument doc = XDocument.Parse(xml);
                        XElement Nodo = doc.Root.Element("Payment");
                        GatewaySMS = Convert.ToBoolean(Nodo.Element("GatewaySMS").Value);
                    }

                    if (row.Solicitudes.Clientes.Sucursales.Configuraciones.HasNetworkAccess == true )
                    {
                        foreach (var x in result.Where(x => x.Vence >= Desde && x.Vence<= Hasta && x.Balance > row.Solicitudes.Clientes.Sucursales.Configuraciones.MinimumAmount))
                        {
                            try
                            {
                                if (row.Solicitudes.Clientes.Sucursales.Configuraciones.RemoteNotification)
                                {
                                    string Mensaje = string.Format(msg, ShortName(row.Solicitudes.Clientes.Personas.Nombres), x.Numero, string.Format("{0:dd/MM/yyyy}", x.Vence), string.Format("{0:N2}", x.Balance), row.Solicitudes.Clientes.Sucursales.Empresas.Empresa);
                                    string[] userID = new string[1];
                                    userID[0] = row.Solicitudes.Clientes.Documento;
                                    await db.NotificacionesSent(row.Solicitudes.Clientes.Sucursales.Empresas.Empresa, Mensaje, userID, Environment.MachineName);
                                }
                            }
                            catch { }

                            try
                            {
                                if (row.Solicitudes.Clientes.Sucursales.Configuraciones.SmsNotification && row.Solicitudes.Condiciones.SendSMS && GatewaySMS == true)
                                {
                                    var smsdate = string.IsNullOrEmpty(GetSmsDate()) ? 20240101 : int.Parse(GetSmsDate());
                                    if (smsdate < int.Parse( $"{string.Format("{0:0000}", _Date.Year)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:00}", _Date.Day)}"))
                                    {
                                        if (_Date.Hour >= 8 && _Date.Hour <= 9 && _Date.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            if (row.Solicitudes.Clientes.Personas.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "").Length == 10)
                                            {
                                                if (db.CanSendSmS(row.Solicitudes.Clientes.Sucursales.EmpresaID))
                                                {
                                                    var phone = row.Solicitudes.Clientes.Personas.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "");
                                                    if (phone != "0000000000")
                                                    {
                                                        Console.WriteLine("Enviando sms al movil {0}", row.Solicitudes.Clientes.Personas.Celular);
                                                        string Mensaje = string.Format(msg, ShortName(row.Solicitudes.Clientes.Personas.Nombres), x.Numero, string.Format("{0:dd/MM/yyyy}", x.Vence), string.Format("{0:N2}", x.Balance), row.Solicitudes.Clientes.Sucursales.Empresas.Empresa);

                                                        var smsResult = db.EnviarSMS(row.Solicitudes.Clientes.Sucursales.Configuraciones.smsHost, row.Solicitudes.Clientes.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), row.Solicitudes.Clientes.Personas.Celular, Mensaje, row.Solicitudes.Clientes.Personas.OperadorID);
                                                        if (smsResult.ResponseCode == "00")
                                                        {
                                                            db.SmsCreate(row.Solicitudes.Clientes.Personas.Celular, Mensaje, row.ContratoID, Environment.MachineName);
                                                            SendSMS = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }

                            try
                            {
                                if (row.Solicitudes.Clientes.Sucursales.Configuraciones.EmailNotification)
                                {
                                    var maildate = GetMailDate();
                                    if (maildate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                                    {
                                        Console.WriteLine("Enviando correo al cliente {0}", row.Solicitudes.Clientes.Personas.Correo);
                                        try
                                        {
                                            if (Regex.IsMatch(row.Solicitudes.Clientes.Personas.Correo, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                                            {
                                                if (x.Balance > 0)
                                                {
                                                    var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Correos/CuotasAVencer.html");
                                                    string Body = string.Format(message, row.Solicitudes.Clientes.Sucursales.Empresas.Empresa, row.Solicitudes.Clientes.Sucursales.Direccion, row.Solicitudes.Clientes.Sucursales.Telefonos, row.Solicitudes.Clientes.Sucursales.Empresas.Rnc, row.Solicitudes.Clientes.Personas.Nombres + " " + row.Solicitudes.Clientes.Personas.Apellidos, string.Format("{0:000}", x.Numero), row.Solicitudes.TipoSolicitudes.TipoSolicitud, string.Format("{0:000000}", row.ContratoID), string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", x.Vence), string.Format("{0:N2}", x.Balance), row.Solicitudes.Clientes.Sucursales.Empresas.Empresa, row.Solicitudes.Clientes.Sucursales.Direccion, row.Solicitudes.Clientes.Sucursales.Telefonos, row.Solicitudes.Clientes.Sucursales.Empresas.Rnc, DateTime.Now.Year, row.Solicitudes.Clientes.Sucursales.Empresas.Empresa, row.Solicitudes.Clientes.Sucursales.Empresas.Site); // Row["Empresa"], Row["Direccion"], Row["Telefonos"], Row["Rnc"], DateTime.Now.Year, Row["Empresa"], Sucursales.Empresas.Site);

                                                    Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                                    var response = ms.SendMail(row.Solicitudes.Clientes.Personas.Correo, string.Format("Cuotas a Vencer #{0} | {1}", string.Format("{0:dd/MM/yyyy}", x.Vence), row.Solicitudes.Clientes.Sucursales.Empresas.Empresa), Body);
                                                    SendMail = true;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }

                if (SendSMS)
                {
                    SmsDate($"{string.Format("{0:0000}", _Date.Year)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:00}", _Date.Day)}");
                }

                if (SendMail)
                {
                    MailDate($"{string.Format("{0:0000}", _Date.Year)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:00}", _Date.Day)}");
                }

                Console.Clear();
            }
            catch { }
        }

        public static void SmsDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "smsDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetSmsDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "smsDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void SmsVencidosDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "smsVencidosDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetSmsVencidosDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "smsVencidosDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void MailDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "mailDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetMailDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "mailDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void MailVencidosDate(String date)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "mailVencidosDate.txt");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(date);
                }
            }
            catch { }
        }

        public static String GetMailVencidosDate()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "mailVencidosDate.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public void CalcularLegal(DateTime _Date)
        {
            Manager db = new Manager();
            Console.WriteLine("");
            Console.WriteLine("*********** Calculando Cartas al {0} ***********", DateTime.Now);
            Console.WriteLine("");
            db.CalcularLegal(_Date);
        }


        public void PagosAutomaticos(DateTime _Date)
        {
            Manager db = new Manager();
            Console.WriteLine("");
            Console.WriteLine("*********** Pagos Automaticos al {0} ***********", _Date);
            Console.WriteLine("");
          _=  db.PagosAutomaticosAsync(_Date);
        }

        public void CalcularRenta(int Day)
        {
            Manager db = new Manager();
            Console.WriteLine("");
            Console.WriteLine("*********** Calculando Rentas al {0} ***********", DateTime.Now);
            Console.WriteLine("");
            db.CalcularRentas(DateTime.Now.AddDays(Day));
        }

        public void CalcularCertificados(DateTime Fecha)
        {
            try {

                Manager db = new Manager();
                Console.WriteLine("");
                Console.WriteLine("*********** Calculando Certificados al {0} ***********", DateTime.Now);
                Console.WriteLine("");
                db.CalcularCertificados(Fecha);
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void NotificarCuotasVencidasGetByFecha(DateTime _Date)
        {
            try
            {
                bool SendSMS = false;
                bool SendMail = false;

                Manager db = new Manager();
                Console.WriteLine("*********** Notificando electronicamente atrasos al {0} ***********", _Date);

                foreach (clsSucursalesBE Sucursales in db.SucursalesGet(null))
                {
                    foreach (DataRow Row in db.PrintCuotasVencidasGetByFecha(_Date, Sucursales.SucursalID, -1).Tables["Contratos"].Rows)
                    {
                        string payment = db.EmpresasGetByEmpresaID(Sucursales.EmpresaID).Payment;
                        var rowXML = Decoding(payment == null ? "" : payment);
                        bool GatewaySMS = false;
                        if (!string.IsNullOrEmpty(rowXML))
                        {
                            String xml = rowXML;
                            XDocument doc = XDocument.Parse(xml);
                            XElement Nodo = doc.Root.Element("Payment");
                            GatewaySMS = Convert.ToBoolean(Nodo.Element("GatewaySMS").Value);
                        }

                        var c = db.ContratosGetByContratoID(int.Parse(Row["ContratoID"].ToString()));

                        if (Sucursales.Configuraciones.HasNetworkAccess && c.Solicitudes.Condiciones.SendSMS)
                        {
                            if (Sucursales.Configuraciones.RemoteNotification)
                            {
                                var message = "Señor(a) {0} Cortésmente, nos permitimos recordarle que en {1} su préstamo {2} No. {3} otorgado en fecha {4}, se encuentra en atraso por un monto de RD${5}.";
                                string Body = string.Format(message, Row["Apellidos"], Row["Empresa"], Row["TipoSolicitud"], Row["ContratoID"], string.Format("{0:dd/MM/yyyy}", Row["Fecha"]), string.Format("{0:N2}", Row["Atraso"]));
                                string[] userID = new string[1];
                                userID[0] = Row["Documento"].ToString();
                                var result = db.NotificacionesSent(Sucursales.Empresas.Empresa, Body, userID, Environment.MachineName);
                            }

                            if (Sucursales.Configuraciones.SmsNotification && GatewaySMS == true && c.Solicitudes.Condiciones.SendSMS && HasAccess(Sucursales.Configuraciones.smsHost))
                            { 
                                var smsvencidosdate = GetSmsVencidosDate() == null ? "2024-01-01": GetSmsVencidosDate();
                                DateTime tmp = DateTime.Parse(smsvencidosdate);
                                TimeSpan ts = _Date - tmp;
                                if (_Date.Hour >= 8 && _Date.Hour <= 9  && ts.Days == 28 && _Date.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    if (c.Solicitudes.Clientes.Personas.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "").Length == 10)
                                    {
                                        if (smsvencidosdate != $"{string.Format("{0:0000}", _Date.Year)}-{string.Format("{0:00}", _Date.Month)}-{string.Format("{0:0000}", _Date.Day)}")
                                        {
                                            if (db.CanSendSmS(c.Solicitudes.Clientes.Sucursales.EmpresaID))
                                            {
                                                var phone = c.Solicitudes.Clientes.Personas.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "");
                                                if (phone != "0000000000")
                                                {
                                                    Console.WriteLine("Enviando sms al movil {0}", Row["Celular"].ToString());

                                                    var message = "Sr(a) {0}, en {1} su prestamo No. {2} otorgado en fecha {3}, esta en atraso por un monto de RD{4}.";
                                                    string Body = string.Format(message, Row["Apellidos"], Row["Empresa"], Row["ContratoID"], string.Format("{0:dd/MM/yyyy}", Row["Fecha"]), string.Format("{0:N2}", Row["Atraso"]));

                                                    try
                                                    {

                                                        var smsResult = db.EnviarSMS(Sucursales.Configuraciones.smsHost, Sucursales.EmpresaID, Common.FingerPrint.GetKey(), Row["Celular"].ToString(), Body, (int)Row["OperadorID"]);
                                                        if (smsResult.ResponseCode == "00")
                                                        {
                                                            SendSMS = true;
                                                            db.SmsCreate(Row["Celular"].ToString(), Body, int.Parse(Row["ContratoID"].ToString()), Environment.MachineName);
                                                        }
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (Sucursales.Configuraciones.EmailNotification && InternetAccess())
                            {

                                if (Regex.IsMatch(Row["Correo"].ToString().Trim(), @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                                {
                                    var mailvencidosdate = GetMailVencidosDate();
                                    if (mailvencidosdate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                                    {
                                        Console.WriteLine("Enviando correo al cliente {0}", Row["Correo"].ToString());


                                        var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Correos/Atrasos.html");
                                        string Body = string.Format(message, Row["Empresa"], Row["Direccion"], Row["Telefonos"], Row["Rnc"], Row["Nombres"] + " " + Row["Apellidos"], Row["TipoSolicitud"], Row["ContratoID"], string.Format("{0:dd/MM/yyyy}", Row["Fecha"]), string.Format("{0:N2}", Row["Atraso"]), Row["Cantidad"], Row["Empresa"], Row["Direccion"], Row["Telefonos"], Row["Rnc"], DateTime.Now.Year, Row["Empresa"], Sucursales.Empresas.Site);

                                        clsMisc ms = new clsMisc();
                                        try
                                        {
                                            ms.SendMail(Row["Correo"].ToString(), "Departamento de Cobros | " + Row["Empresa"].ToString(), Body);
                                            Console.WriteLine("Enviando a {0}", Row["Correo"].ToString());
                                            SendMail = true;
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                }

                if (SendSMS)
                {
                    SmsVencidosDate($"{string.Format("{0:0000}", _Date.Year)}-{string.Format("{0:00}", _Date.Month)}-{string.Format("{0:00}", _Date.Day)}");
                }

                if (SendMail)
                {
                    MailVencidosDate($"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}");
                }

                Console.Clear();
            }
            catch(Exception ex)
            { }

        }

        public async void EnviarCorreoCumpleaños(DateTime _Date)
        {
            try
            {
                Manager db = new Manager();

                foreach (clsSucursalesBE Sucursales in db.SucursalesGet(null))
                {
                    foreach (clsClientesBE Row in db.ClientesGet(null))
                    {
                        if (Row.Personas.FechaNacimiento.Month == _Date.Month && Row.Personas.FechaNacimiento.Day == _Date.Day)
                        {
                            if (Regex.IsMatch(Row.Personas.Correo, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                            {
                                var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Correos/Cumpleaños.html");
                                string Body = string.Format(message, Row.Personas.Nombres, Row.Sucursales.Empresas.Empresa, Row.Sucursales.Direccion, Row.Sucursales.Telefonos, Row.Sucursales.Empresas.Rnc, Row.Sucursales.Empresas.Site, DateTime.Now.Year, Row.Sucursales.Empresas.Empresa);

                                clsMisc ms = new clsMisc();
                                ms.SendMail(Row.Personas.Correo, "Feliz Cumpleaños | " + Row.Sucursales.Empresas.Empresa, Body);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) { }

        }

        public async void CalcularMoras(DateTime _Date)
        {
            try
            {
                Manager db = new Manager();
                string Usuario = Environment.MachineName.ToString();
                Console.WriteLine("");
                Console.WriteLine("*********** Calculando Moras al {0} ***********",_Date);
                Console.WriteLine("");

                await db.CalcularMoras(_Date, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string ShortName(string Name)
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

        public static string Decoding(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            string Texto = Encoding.UTF8.GetString(bytes);
            return Texto;
        }

        public static bool ValidarDocumento(string Documento)
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

        public static bool HasAccess(string url)
        {
            bool result = false;
            try
            {
                var request = WebRequest.Create(url);
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

    }
}
