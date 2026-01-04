using Payment.Bank.Core;
using Payment.Bank.Core.Common;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using Payment.Bank.CxC.Common;
using System.Collections.Generic;
using Payment.Bank.Entity;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Net.Http;

namespace Payment.Bank.CxC
{
    class Program
    {

        private static Mutex mutex = null;

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        public static DateTime _Date = DateTime.Now;
        public static bool authorized = true;

        static async Task Main(string[] args)
        {
            try
            {

                IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
                ShowWindow(handle, 6);

                Console.Title = "i-Task";
                const string appName = "i-Task";
                bool createdNew;

                mutex = new Mutex(true, appName, out createdNew);

                if (!createdNew)
                {
                    Console.WriteLine(appName + " is already running! Exiting the application.");
                    return;
                }

                await IsAutorized();
                await getCheckUpdate();

                Manager db = new Manager();
                Common.clsGeneric Generic = new Common.clsGeneric();

                var GetCheckStatusDate = clsGeneric.GetCheckStatusDate();
                if (GetCheckStatusDate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                {
                    db.CheckStatus();
                    clsGeneric.CheckStatusDate($"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}");
                }

                //var GetHostHistoryDataDate = clsGeneric.GetHostHistoryDataDate();
                //if (GetHostHistoryDataDate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                //{
                db.HostHistoryData(DateTime.Today, -1);
                //    clsGeneric.HostHistoryDataDate($"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}");
                //}

                await db.ValidarRecibosGet();
                db.HostUpdateData();
                db.HostUpdate();
               
                Console.Clear();
                List<clsFotografiasBE> photos = new List<clsFotografiasBE>();
                photos = db.FotografiasGet().ToList();

                var result = db.TareasGet(null).OrderBy(x => x.TipoTareas.TipoTareaID);
                foreach (var Task in db.TareasGet(null).OrderBy(x => x.TipoTareas.TipoTareaID))
                {
                    switch (Task.TipoTareas.Codigo)
                    {
                        case "DOWNLOADPHOTO": //Descargar Fotografias
                            {
                                try
                                {
                                    if (authorized)
                                    {
                                        Console.WriteLine("");
                                        Console.WriteLine("*********** Descargando Fotografias ***********", _Date);
                                        Console.WriteLine("");
                                        foreach (var p in db.PersonasGet().Where(x => x.Documento.Length == 13 && !x.Documento.Contains("--")))
                                        {
                                            if (Common.clsGeneric.ValidarDocumento(p.Documento))
                                            {
                                                if (photos.Where(x => x.Documento == p.Documento).Count() == 0)
                                                {
                                                    var pic = db.FotografiasGetByDocumentoCloud(p.Documento);
                                                    if (pic.ResponseCode == "00")
                                                    {
                                                        Console.WriteLine($"Descargando fotografia {p.Documento}");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"{pic.ResponseMessage} {p.Documento}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Console.Clear();

                                    Host.DAL.Context host = new Host.DAL.Context();
                                    var empresa = host.clsEmpresasBE.Where(x => x.ClientAccess == true && x.EmpresaID == Task.EmpresaID);

                                    if (empresa.Count() > 0)
                                    {
                                        Console.WriteLine("");
                                        Console.WriteLine("*********** Envando invitacion i-Bank Client {0} ***********", _Date);
                                        Console.WriteLine("");

                                        var personas = (from U in host.clsUsuariosBE select U.Documento);
                                        foreach (var item in db.ClientesGet(null).Where(x => !personas.Contains(x.Documento)))
                                        {
                                            if (Regex.IsMatch(item.Personas.Correo, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                                            {
                                                var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Correos/AppDownload.html");
                                                string Body = string.Format(message, item.Personas.Nombres.ToUpper() + " " + item.Personas.Apellidos.ToUpper(), item.Personas.Clientes.FirstOrDefault().Sucursales.Gerentes.Personas.Nombres + " " + item.Sucursales.Gerentes.Personas.Apellidos, item.Sucursales.Empresas.Empresa, item.Sucursales.Direccion, item.Sucursales.Telefonos, item.Sucursales.Empresas.Rnc, DateTime.Now.Year, item.Sucursales.Empresas.Empresa);

                                                clsMisc ms = new clsMisc();
                                                var response = ms.SendMail(item.Personas.Correo, "Alerta i-Bank Client | " + item.Sucursales.Empresas.Empresa, Body);
                                                if (response == "SUCCESS")
                                                {
                                                    Console.WriteLine($"Enviado a {item.Personas.Correo}");
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }

                                break;
                            }
                        case "BACKUP": //BackUp
                            {
                                Console.WriteLine("");
                                Console.WriteLine("*********** Copia de Seguridad al {0} ***********", _Date);
                                Console.WriteLine("");
                                await db.SetBackUpMySQL(Task.Empresas.Copias, true);
                                await CopiasDeleteGet(Task.EmpresaID);
                            }
                            break;
                        case "BIRTHDAY": //Cumpleaños
                            {
                                Generic.EnviarCorreoCumpleaños(DateTime.Today.AddDays(Task.Dia));
                            }
                            break;
                        case "DEFERREDDUES": //Cuotas Vencidas
                            {
                                Generic.NotificarCuotasVencidasGetByFecha(DateTime.Now.AddDays(Task.Dia));
                            }
                            break;

                        case "FEESFORWINNING": //Cuotas a Vencer
                            {
                                _ = Generic.AVencerSMS(DateTime.Now.AddDays(Task.Dia));
                            }
                            break;

                        case "CALCULATEINTERESTS": //Calcular Intereses
                            {
                                var GetInteresCalculateDate = clsGeneric.GetInteresCalculateDate();
                                if (GetInteresCalculateDate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                                {
                                    Generic.CalcularInteres(DateTime.Today.AddDays(Task.Dia));
                                    clsGeneric.InteresCalculateDate($"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}");
                                }

                            }
                            break;
                        case "CALCULATELATEFEES": //Calcular Moras
                            {
                                var GetMoraCalculateDate = clsGeneric.GetMoraCalculateDate();
                                if (GetMoraCalculateDate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                                {
                                    Generic.CalcularMoras(DateTime.Today.AddDays(Task.Dia));
                                    clsGeneric.MoraCalculateDate($"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}");
                                }
                            }
                            break;
                        case "CALCULATELEGAL": //Calcular Legal
                            {

                                var GetLegalCalculateDate = clsGeneric.GetLegalCalculateDate();
                                if (GetLegalCalculateDate != $"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}")
                                {
                                    Generic.CalcularLegal(DateTime.Today.AddDays(Task.Dia));
                                    clsGeneric.LegalCalculateDate($"{string.Format("{0:00}", _Date.Day)}{string.Format("{0:00}", _Date.Month)}{string.Format("{0:0000}", _Date.Year)}");
                                }
                            }
                            break;
                        case "CALCULATERENT": //Calcular Renta
                            {
                                Generic.CalcularRenta(DateTime.Today.AddDays(Task.Dia).Day);
                            }
                            break;

                        case "PAYMENTCARD": //Pagos Automaticos
                            {
                                Generic.PagosAutomaticos(DateTime.Today.AddDays(Task.Dia));
                            }
                            break;
                    }
                }


                //Generic.CalcularCertificados(DateTime.Today);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Task<bool> CopiasDeleteGet(string EmpresaID)
        {
            try {

                DAL.Context db = new DAL.Context();
                System.IO.DirectoryInfo di = new DirectoryInfo(db.clsCopiasBE.Where(x => x.EmpresaID == EmpresaID).FirstOrDefault().Url);

                foreach (FileInfo file in di.GetFiles())
                {
                    TimeSpan ts = DateTime.Now - file.LastWriteTime;
                    if (ts.Days > 30)
                    {
                        file.Delete();
                    }
                }


                return Task.Run(() =>
                {
                    return true;
                });

            } catch {
                return Task.Run(() =>
                {
                    return false;
                });
            }
        }

        private static Task<bool> getCheckUpdate()
        {
            try
            {
                WebClient webClient = new WebClient();
                var client = new WebClient();
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                if (!webClient.DownloadString("http://softarch.ddns.net/i-bank-update/Version.txt").Contains(assembly.GetName().Version.ToString()))
                {
                    Manager db = new Manager();
                    string payment = db.EmpresasGet(null).FirstOrDefault().Payment;
                    var rowXML = Common.clsGeneric.Decoding(payment == null ? "" : payment);
                    bool CanUpdate = false;
                    if (!string.IsNullOrEmpty(rowXML))
                    {
                        String xml = rowXML;
                        XDocument doc = XDocument.Parse(xml);
                        XElement Nodo = doc.Root.Element("Payment");
                        CanUpdate = Convert.ToBoolean(Nodo.Element("CanUpdate").Value);
                    }

                    if (CanUpdate)
                    {
                       _= getDownloadAsync();
                    }
                }
                return Task.Run(() =>
                {
                    return true;
                });
            }
            catch
            {
                return Task.Run(() =>
                {
                    return false;
                });
            }
        }

        static async Task getDownloadAsync()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                using (var httpClient = new HttpClient())
                {
                    var fileUrl = "http://softarch.ddns.net/i-bank-update/i-task.zip";
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var filePath = System.IO.Path.Combine(path, "i-task.zip");
                        using (var ms = await response.Content.ReadAsStreamAsync())
                        using (var fs = File.Create(filePath))
                        {
                            await ms.CopyToAsync(fs);
                            fs.Flush();
                        }
                    }
                }

                //if (File.Exists(@".\i-bank.zip")) { File.Delete(@".\i-bank.zip"); }
                if (File.Exists($@"{path}\i-task.msi")) { File.Delete($@"{path}\i-task.msi"); }

                string zipPath = $@"{path}\i-task.zip";
                string extractPath = $@"{path}\";
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                Process process = new Process();
                process.StartInfo.FileName = "msiexec.exe";
                process.StartInfo.Arguments = string.Format($@"/i {path}/i-task.msi");

                process.Start();
                Environment.Exit(0);
                System.Environment.Exit(0);

            }
            catch
            {
            }
        }

        private static Task<bool> IsAutorized()
        {
            try
            {
                Core.Manager db = new Core.Manager();
                string EmpresaID = db.EmpresasGet(null).FirstOrDefault().EmpresaID;
                string payment = db.EmpresasGetByEmpresaID(EmpresaID).Payment;
                var rowXML = Common.clsGeneric.Decoding(payment == null ? "" : payment);

                if (Common.clsGeneric.InternetAccess())
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    Stream ms = client.OpenRead(new Uri(string.Format("http://softarch.ddns.net/Authorized/{0}.xml", EmpresaID), UriKind.RelativeOrAbsolute));
                    StreamReader sr = new StreamReader(ms);
                    var xml = sr.ReadToEnd();

                    db.EmpresasUpdatePaymentGetByEmpresaID(EmpresaID, xml);
                    ms.Close();


                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");
                    var ResponseCode = Nodo.Element("ResponseCode").Value;
                    var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                    var Amount = Nodo.Element("Amount").Value;
                    var ResponseAction = Nodo.Element("ResponseAction").Value;
                    var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                    var MovilAccess = Convert.ToBoolean(Nodo.Element("MovilAccess").Value);
                    DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                    int Gracia = int.Parse(Nodo.Element("Gracia").Value);
                    int PlanID = int.Parse(Nodo.Element("PlanID").Value);
                    int PaqueteID = int.Parse(Nodo.Element("PaqueteID").Value);
                    var ftp = Nodo.Element("ftp").Value;
                    var User = Nodo.Element("User").Value;
                    var pwd = Nodo.Element("Pwd").Value;

                    ActionGet(EmpresaID, ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha, MovilAccess, PlanID, PaqueteID, Gracia);
                    ms.Close();

                }

                return Task.Run(() =>
                {
                    return true;
                });
            }
            catch
            {
                return Task.Run(() =>
                {
                    return false;
                });
            }
        }


        static void ActionGet(string EmpresaID, string ResponseCode, string ResponseMessage, string Amount, string ResponseAction, string ResponseArgument, DateTime Fecha, bool movilAccess, int PlanID, int PaqueteID, int Gracia)
        {
            try
            {
                Core.Manager core = new Core.Manager();
                switch (ResponseCode)
                {
                    case "00":
                        {
                            core.EmpresasUpdateMovilAccess(EmpresaID, movilAccess, Environment.MachineName);
                        }
                        break;
                    case "01":
                        {
                            var date = core.HostDateTime(EmpresaID, Common.FingerPrint.GetKey());
                            TimeSpan ts = Fecha.AddDays(Gracia) - date;
                            if (ts.Days <= 0)
                            {
                                authorized = false;
                                core.EmpresasUpdateMovilAccess(EmpresaID, movilAccess, Environment.MachineName);

                                if (!string.IsNullOrEmpty(ResponseAction))
                                {
                                    System.Diagnostics.Process.Start(ResponseAction, ResponseArgument);
                                }

                                Process[] processes = Process.GetProcesses();
                                foreach (Process process in processes)
                                {
                                   if(process.ProcessName.Contains("i-Bank"))
                                    {
                                        process.Kill();
                                        process.WaitForExit();
                                    }
                                }
                            }
                            else
                            {
                                core.EmpresasUpdateMovilAccess(EmpresaID, movilAccess, Environment.MachineName);
                            }
                        }
                        break;
                    case "02":
                        {
                            var date = core.HostDateTime(EmpresaID, Common.FingerPrint.GetKey());
                            TimeSpan ts = Fecha.AddDays(Gracia) - date;

                            if (ts.Days <= 0)
                            {
                                PaqueteID = 0;
                                movilAccess = false;
                                authorized = false;
                            }


                            core.EmpresasUpdateMovilAccess(EmpresaID, movilAccess, Environment.MachineName);
                        }
                        break;
                    case "03":
                        {
                            var date = core.HostDateTime(EmpresaID, Common.FingerPrint.GetKey());
                            TimeSpan ts = Fecha.AddDays(Gracia) - date;
                            if (ts.Days <= 0)
                            {
                                PaqueteID = 0;
                                movilAccess = false;
                                authorized = false;
                            }

                            core.EmpresasUpdateMovilAccess(EmpresaID, movilAccess, Environment.MachineName);
                        }
                        break;

                }
                core.EmpresasUpdatePlanID(EmpresaID, PlanID, PaqueteID, Environment.MachineName);

            }
            catch{}
        }



    }
}