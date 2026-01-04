using Payment.Bank.Controles;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Notifications.Wpf;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Media;
using System.Net.Http;
using Payment.Bank.Core.Common;
using Payment.Bank.Entity;
using Newtonsoft.Json;
using System.Text;
using Payment.Bank.View.Informes;
using System.Collections.Generic;
using Payment.Bank.Model;
using System.Xml;
using Payment.Bank.Properties;
using System.IO.Compression;
using System.Diagnostics;
using Payment.Bank.DAL;
using System.Windows.Media.Imaging;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmPrincipal.xaml
    /// </summary>
    public partial class frmPrincipalView : Window
    {
        //DAL.Context db = new Context();
        Core.Manager core = new Core.Manager();
        private DateTime fechaPago;
        HubConnection hubConnection;
        string internalKey = "http://softarch.ddns.net";
        public static string api_rest = "/i-bank/api/v1.0";

        public frmPrincipalView()
        {
            InitializeComponent();

            this.Width = System.Windows.SystemParameters.WorkArea.Width * 0.95;
            this.Height = System.Windows.SystemParameters.WorkArea.Height * 0.95;
            CenterWindowOnScreen();

            clsCookiesBO.Key(Common.FingerPrint.GetKey());
            clsVariablesBO.UsuariosBE = new Entity.clsUsuariosBE();
            clsVariablesBO.LenguajeID = clsCookiesBO.LenguajeID();
            CultureInfo culture = new CultureInfo("es-DO");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            btnAbout.Click += btnAbout_Click;
            btnPassword.Click += btnPassword_Click;
              

            hubConnection = new HubConnectionBuilder()
            .WithUrl($"https://i-bank.azurewebsites.net/chatHub")
            .Build();


            clsVariablesBO.Host = this;


            Loaded += FrmPrincipal_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnMinimizar.Click += btnMinimizar_Click;


            if (Environment.MachineName == "SERVIDOR-PC")
            {
                try
                {
                    clsSchedulerTask.Create("i-Bank");
                }
                catch(Exception ex)
                { }
            }

            try
            {
              _ =  Init();
            }
            catch { }
        }

        public void SetLogo()
        {
            try
            {

                var db = new DAL.Context();
                var row = db.clsEmpresasBE.FirstOrDefault();
                if (row.Logo.Count() > 0)
                {
                    imgLogo.Source = Common.Generic.ByteArrayToImageSource(row.Logo, 1024, 1024);
                    RenderOptions.SetBitmapScalingMode(imgLogo, BitmapScalingMode.HighQuality);
                }
                else
                {
                    imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Images/logo/logo.png"));

                }
                imgLogo.Visibility = Visibility.Visible;
            }
            catch 
            {
                //return Task.Run(async () =>
                //{
                    imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Images/logo/logo.png"));

                //});
            }
        }

        private void btnPassword_Click(object sender, RoutedEventArgs e)
        {
            try {
                frmPasswordChange change = new frmPasswordChange();
                change.Owner = this;
                change.ShowDialog();
            }
            catch { }
        }

        async Task Init()
        {
            try
            {

                Mutex mutex = null;
                const string appName = "i-Bank";
                bool createdNew;

                mutex = new Mutex(true, appName, out createdNew);

                if (!createdNew)
                {
                    Environment.Exit(0);
                    return;
                }

                await hubConnection.StartAsync();
                await Task.Delay(200);

                hubConnection.On<string>("Joined", async (user) =>
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        if (clsVariablesBO.UsuariosBE.Personas.Nombres != user)
                        {
                            try
                            {
                                var notificationManager = new NotificationManager();
                                notificationManager.Show(new NotificationContent
                                {
                                    Title = "Notificación",
                                    Message = $"{user} ha iniciado sesión",
                                    Type = NotificationType.Information
                                }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(10));
                            }
                            catch { }
                        }
                    }
                });

                hubConnection.On<string, string>("SolicitudReceived", async (user, Monto) =>
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Notificación",
                            Message = $"Has recibido una nueva solicitud de préstamos desde la aplicación movil del señor(a) {user.ToUpper()}, por un monto de RD${Monto}.",
                            Type = NotificationType.Information
                        }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(20));
                    }
                });


                hubConnection.On<string, string, string, string, string, string>("RequestDeleteMovilReceipt", async (empresaID, documento, contratoID, monto, tokenID, codigo) =>
                {
                    try
                    {
                        if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == empresaID && clsVariablesBO.UsuariosBE.Roles.IsAdmin)
                        {
                            Core.Manager db = new Core.Manager();
                            var Contrato = db.ContratosGetByContratoID(int.Parse(contratoID));

                            var notificationManager = new NotificationManager();
                            var Usuario = db.PersonasGetByDocumento(documento);
                            notificationManager.Show(new NotificationContent
                            {
                                Title = "Notificación",
                                Message = $"Has recibido del señor(a) {Usuario.Nombres.ToUpper()} {Usuario.Apellidos.ToUpper()}, una solicitud para eliminar un recibo realizado en el dispositivo movil a nombre de {Contrato.Solicitudes.Clientes.Personas.Nombres.ToUpper()} {Contrato.Solicitudes.Clientes.Personas.Apellidos.ToUpper()}, contrato numero {Contrato.ContratoID} por un monto de RD${string.Format("{0:N2}", monto)}. El código de autorización es: {codigo}",
                                Type = NotificationType.Error
                            }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(60), onClick: ()=> 
                            {
                                frmMessageBox msgBox = new frmMessageBox();
                                msgBox.OnInit("¿Deseas autorizar sea eliminado este recibo?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                                msgBox.btnAceptar.Click += async (arg, obj) =>
                                {
                                    await hubConnection.InvokeAsync("AuthorizationDeleteReceipt", empresaID, documento, contratoID, monto, tokenID, codigo);
                                    msgBox.Close();
                                };
                                msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                                msgBox.Owner = this;
                                msgBox.ShowDialog();

                            });
                        }
                    }
                    catch(Exception ex)
                    { }

                });


                hubConnection.On<string, string>("RecibosSyncReceived", async (user, cantidad) =>
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Notificación",
                            Message = $"{user}, ha sincronizado {cantidad} recibo(s) de su jornada laboral desde la aplicación movil.",
                            Type = NotificationType.Information
                        }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(20));
                    }
                });

                hubConnection.On<string, string>("ContratosSyncReceived", async (user, cantidad) =>
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Notificación",
                            Message = $"{user}, ha sincronizado {cantidad} contrato(s) de su jornada laboral desde la aplicación movil.",
                            Type = NotificationType.Information
                        }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(20));
                    }
                });

                if (Environment.MachineName.ToUpper() == "SERVIDOR-PC")
                {
                    Core.Manager db = new Core.Manager();
                    var Empresa = db.SucursalesGet(null).FirstOrDefault().Empresas;

                    hubConnection.On<string, string>("sendRutasGetByOficialID", async (empresaID, documento) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                List<clsRutasBE> result = new List<clsRutasBE>();
                                var A = db.OficialesGet(null).Where(b => b.Personas.Documento == clsValidacionesBO.DocumentFormat(documento));
                                var P = db.PersonasGetByDocumento(clsValidacionesBO.DocumentFormat(documento));


                                var x = P.Usuarios.Roles.IsAdmin == true ? db.RutasGet(null) : A.Count() == 0 ? new List<Entity.clsRutasBE>() : db.RutasGetByOficialID(A.FirstOrDefault().OficialID);
                                if (x != null && P.Usuarios.EstadoID == true)
                                {
                                    List<clsZonasBE> zonas = new List<clsZonasBE>();
                                    foreach (var item in x.ToList())
                                    {
                                        zonas = new List<clsZonasBE>();
                                        foreach (var zona in item.Zonas)
                                        {
                                            zonas.Add(new clsZonasBE { ZonaID = zona.ZonaID, Fecha = zona.Fecha, Zona = zona.Zona, RutaID = zona.RutaID, Usuario = zona.Usuario, ModificadoPor = zona.ModificadoPor, FechaModificacion = zona.FechaModificacion });
                                        }
                                        result.Add(new clsRutasBE { RutaID = item.RutaID, Fecha = item.Fecha, Ruta = item.Ruta, Usuario = item.Usuario, ModificadoPor = item.ModificadoPor, FechaModificacion = item.FechaModificacion, Zonas = zonas });
                                    }
                                }
                                else
                                {
                                    result = new System.Collections.Generic.List<clsRutasBE>();
                                }
                                await hubConnection.InvokeAsync("sendDataRutasGetByOficialID", Empresa.EmpresaID, documento, JsonConvert.SerializeObject(result));
                            }
                        }
                        catch { }
                    });


                    hubConnection.On<string, string>("sendJornadasGetByDispositivoID", async (empresaID, DispositivoID) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                clsJornadasBE data = new clsJornadasBE();
                                var result = db.JornadasGetByDispositivoID(DispositivoID);
                                List<clsDetalleJornadasBE> detalle = new List<clsDetalleJornadasBE>();

                                foreach (var item in result.DetalleJornadas)
                                {
                                    detalle.Add(new clsDetalleJornadasBE { DetalleJornadaID = item.DetalleJornadaID, Fecha = item.Fecha, JornadaID = item.JornadaID, Desde = item.Desde, Hasta = item.Hasta, DiaID = item.DiaID, Usuario = item.Usuario, ModificadoPor = item.ModificadoPor, FechaModificacion = item.FechaModificacion });
                                }

                                data.JornadaID = result.JornadaID;
                                data.Fecha = result.Fecha;
                                data.Jornada = result.Jornada;
                                data.Usuario = result.Usuario;
                                data.ModificadoPor = result.ModificadoPor;
                                data.FechaModificacion = result.FechaModificacion;
                                data.DetalleJornadas = detalle;


                                await hubConnection.InvokeAsync("sendDataJornadasGetByDispositivoID", Empresa.EmpresaID, DispositivoID, JsonConvert.SerializeObject(data));
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendRecibosSync", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync($"{api_rest}/RecibosSync?data={data}");
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataRecibosSync", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendClientesSync", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync($"{api_rest}/ClientesSync?data={data}");
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataClientesSync", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendPersonasCreate", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync(data);
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataPersonasCreate", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendSolicitudesCreate", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync(data);
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataSolicitudesCreate", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendContratosCreate", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync(data);
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataContratosCreate", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendFirmasCreate", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");

                                var content = new StringContent(data, Encoding.UTF8, "application/json");
                                var url = $"{api_rest}/FirmasCreate";
                                var result = await client.PostAsync(url, content);

                                await hubConnection.InvokeAsync("sendDataFirmasCreate", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendFotografiasCreate", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");

                                var content = new StringContent(data, Encoding.UTF8, "application/json");
                                var url = $"{api_rest}/FotografiasCreate";
                                var result = await client.PostAsync(url, content);

                                await hubConnection.InvokeAsync("sendDataFotografiasCreate", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendListadoContratosGetByRutaID", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync(data);
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataListadoContratosGetByRutaID", Empresa.EmpresaID, documento, result);

                                //foreach (var item in JsonConvert.DeserializeObject<List<QueryBussiness>>(result))
                                //{
                                //    var serialized = JsonConvert.SerializeObject(item);
                                //    await hubConnection.InvokeAsync("sendDataListadoContratosGetByRutaID", Empresa.EmpresaID, documento, serialized);
                                //}

                            }
                        }
                        catch(Exception ex)
                        { }
                    });

                    hubConnection.On<string, string, string>("sendCuotasGetByRutaID", async (empresaID, documento, data) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync(data);
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataCuotasGetByRutaID", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    hubConnection.On<string, string, string>("sendLogin", async (empresaID, documento, pin) =>
                    {
                        try
                        {
                            if (Empresa.EmpresaID == empresaID)
                            {
                                var client = new HttpClient();
                                client.Timeout = new TimeSpan(0, 2, 0);
                                client.DefaultRequestHeaders.Add("Appkey", string.Empty);
                                client.DefaultRequestHeaders.Add("MessageHash", Security.GetHash(internalKey, HashType.MD5));
                                client.BaseAddress = new Uri("http://LocalHost/");
                                var response = await client.GetAsync($"{api_rest}/Login?Documento={documento}&Pin={pin}");
                                var result = await response.Content.ReadAsStringAsync();
                                await hubConnection.InvokeAsync("sendDataLogin", Empresa.EmpresaID, documento, result);
                            }
                        }
                        catch { }
                    });

                    await hubConnection.InvokeAsync("JoinChat", Empresa.EmpresaID, "Linked Service");
                }


            }
            catch (Exception ex)
            { }
        }

  

        private void YesAction()
        {
            //frmMessageBox msgBox = new frmMessageBox();
            //msgBox.OnInit("¿Deseas autorizar sea eliminado este recibo?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
            //msgBox.btnAceptar.Click += (arg, obj) =>
            //{ 
            //    hubConnection.InvokeAsync("AuthorizationDeleteReceipt", Empresa.EmpresaID, )
            //};
            //msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
            //msgBox.Owner = this;
            //msgBox.ShowDialog();
        }



        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            frmAbout About = new frmAbout();
            About.Owner = this;
            About.ShowDialog();
        }

        void ActionGet(string ResponseCode, string ResponseMessage, string Amount, string ResponseAction, string ResponseArgument, DateTime Fecha, bool movilAccess, int PlanID, int PaqueteID, int Gracia)
        {
            try
            {
                switch (ResponseCode)
                {
                    case "00":
                        {
                            core.EmpresasUpdateMovilAccess(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, movilAccess, clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    case "01":
                        {
                            var date = core.HostDateTime(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                            TimeSpan ts = Fecha.AddDays(Gracia) - date;
                            if (ts.Days <= 0)
                            {
                                frmAviso apagado = new frmAviso();
                                apagado.btnAceptar.Visibility = Visibility.Collapsed;
                                apagado.dias = 0;
                                apagado.Owner = this;
                                apagado.ShowDialog();
                                core.EmpresasUpdateMovilAccess(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, movilAccess, clsVariablesBO.UsuariosBE.Documento);

                                if (!string.IsNullOrEmpty(ResponseAction))
                                {
                                    System.Diagnostics.Process.Start(ResponseAction, ResponseArgument);
                                }
                            }
                            else
                            {
                                if (ts.Days <= 8)
                                {
                                    lblMensaje.Content = string.Format("REALICE SU PAGO DEL MES EN CURSO ANTES DEL {0}. EVITE SUSPENSIÓN DEL SERVICIO.", string.Format("{0:dd/MM/yyyy}", fechaPago));
                                }
                                core.EmpresasUpdateMovilAccess(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, movilAccess, clsVariablesBO.UsuariosBE.Documento);

                                if (ts.Days <= 3)
                                {
                                    frmAviso Aviso = new frmAviso();
                                    Aviso.Owner = this;
                                    Aviso.dias = ts.Days;
                                    Aviso.ShowDialog();
                                }
                            }

                        }
                        break;
                    case "02":
                        {
                            var date = core.HostDateTime(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                            TimeSpan ts = Fecha.AddDays(Gracia) - date;

                            if (ts.Days <= 8)
                            {
                                lblMensaje.Content = string.Format("REALICE SU PAGO DEL MES EN CURSO ANTES DEL {0}. EVITE SUSPENSIÓN DEL SERVICIO.", string.Format("{0:dd/MM/yyyy}", fechaPago));
                            }
                            core.EmpresasUpdateMovilAccess(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, movilAccess, clsVariablesBO.UsuariosBE.Documento);

                            if (ts.Days <= 3)
                            {
                                frmAviso Aviso = new frmAviso();
                                Aviso.Owner = this;
                                Aviso.dias = ts.Days;
                                Aviso.ShowDialog();

                                clsVariablesBO.IsRemoteQuery = false;
                                PaqueteID = 0;
                                movilAccess = false;
                            }

                            if (ts.Days <= 0)
                            {
                                clsVariablesBO.IsRemoteQuery = false;
                                PaqueteID = 0;
                                movilAccess = false;
                            }

                        }
                        break;
                    case "03":
                        {
                            var date = core.HostDateTime(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                            TimeSpan ts = Fecha.AddDays(Gracia) - date;

                            if (ts.Days <= 8)
                            {
                                lblMensaje.Content = string.Format("REALICE SU PAGO DEL MES EN CURSO ANTES DEL {0}. EVITE SUSPENSIÓN DEL SERVICIO.", string.Format("{0:dd/MM/yyyy}", fechaPago.AddDays(-3)));
                            }

                            if (ts.Days <= 0)
                            {
                                clsVariablesBO.IsRemoteQuery = false;
                                PaqueteID = 0;
                                movilAccess = false;
                            }

                            core.EmpresasUpdateMovilAccess(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, movilAccess, clsVariablesBO.UsuariosBE.Documento);

                        }
                        break;

                }
                
                core.EmpresasUpdatePlanID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, PlanID, PaqueteID, clsVariablesBO.UsuariosBE.Documento);

            }
            catch (Exception ex) 
            { 
                clsMessage.ErrorMessage(ex.Message, "Mensaje"); }
        }


        private void IsAutorized()
        {
            try
            {
                Core.Manager db = new Core.Manager();
                string payment = db.EmpresasGetByEmpresaID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID).Payment;
                var rowXML = Common.Generic.Decoding(payment == null? "": payment);

                if (HasConnection())
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    Stream ms = client.OpenRead(new Uri(string.Format("http://softarch.ddns.net/Authorized/{0}.xml", clsVariablesBO.UsuariosBE.Sucursales.EmpresaID), UriKind.RelativeOrAbsolute));
                    StreamReader sr = new StreamReader(ms);
                    var xml = sr.ReadToEnd();

                    db.EmpresasUpdatePaymentGetByEmpresaID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, xml);

                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");
                    var ResponseCode = Nodo.Element("ResponseCode").Value;
                    var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                    var Amount = Nodo.Element("Amount").Value;
                    var ResponseAction = Nodo.Element("ResponseAction").Value;
                    var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                    clsVariablesBO.IsRemoteQuery = Convert.ToBoolean(Nodo.Element("IsRemoteQuery").Value);
                    var MovilAccess = Convert.ToBoolean(Nodo.Element("MovilAccess").Value);
                    DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                    int Gracia = int.Parse(Nodo.Element("Gracia").Value);
                    int PlanID = int.Parse(Nodo.Element("PlanID").Value);
                    int PaqueteID = int.Parse(Nodo.Element("PaqueteID").Value);
                    clsVariablesBO.IsCheckPrevious = Convert.ToBoolean(Nodo.Element("IsCheckPrevious").Value);
                    clsVariablesBO.GatewaySMS = Convert.ToBoolean(Nodo.Element("GatewaySMS").Value);
                    clsVariablesBO.CanUpdate = Convert.ToBoolean(Nodo.Element("CanUpdate").Value);
                    clsVariablesBO.UpdateAutomatically = Convert.ToBoolean(Nodo.Element("UpdateAutomatically").Value);
                    var ftp = Nodo.Element("ftp").Value;
                    var User = Nodo.Element("User").Value;
                    var pwd = Nodo.Element("Pwd").Value;

                    fechaPago = Fecha;
                    ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha, MovilAccess, PlanID, PaqueteID, Gracia);
                    ms.Close();
                }
                else
                {
                    if (!string.IsNullOrEmpty(rowXML))
                    {
                        String xml = rowXML;
                        XDocument doc = XDocument.Parse(xml);
                        XElement Nodo = doc.Root.Element("Payment");
                        var ResponseCode = Nodo.Element("ResponseCode").Value;
                        var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                        var Amount = Nodo.Element("Amount").Value;
                        var ResponseAction = Nodo.Element("ResponseAction").Value;
                        var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                        DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                        clsVariablesBO.IsRemoteQuery = Convert.ToBoolean(Nodo.Element("IsRemoteQuery").Value);
                        var MovilAccess = Convert.ToBoolean(Nodo.Element("MovilAccess").Value);
                        int Gracia = int.Parse(Nodo.Element("Gracia").Value);
                        int PlanID = int.Parse(Nodo.Element("PlanID").Value);
                        int PaqueteID = int.Parse(Nodo.Element("PaqueteID").Value);
                        clsVariablesBO.IsCheckPrevious = Convert.ToBoolean(Nodo.Element("IsCheckPrevious").Value);
                        clsVariablesBO.GatewaySMS = Convert.ToBoolean(Nodo.Element("GatewaySMS").Value);
                        clsVariablesBO.CanUpdate = Convert.ToBoolean(Nodo.Element("CanUpdate").Value);
                        clsVariablesBO.UpdateAutomatically = Convert.ToBoolean(Nodo.Element("UpdateAutomatically").Value);
                        var ftp = Nodo.Element("ftp").Value;
                        var User = Nodo.Element("User").Value;
                        var pwd = Nodo.Element("Pwd").Value;
                        fechaPago = Fecha;
                        ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha, MovilAccess, PlanID, PaqueteID, Gracia);

                    }
                    else
                    {
                        CreateFile();
                    }
                }

            }
            catch(Exception ex)
            {
                CreateFile();
            }
        }

        private void CreateFile()
        {
            try
            {
                Core.Manager db = new Core.Manager();
                var rowXML = Common.Generic.Decoding(db.EmpresasGetByEmpresaID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID).Payment == null ? "" : db.EmpresasGetByEmpresaID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID).Payment);

                if (!string.IsNullOrEmpty(rowXML))
                {
                    String xml = rowXML;
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");
                    var ResponseCode = Nodo.Element("ResponseCode").Value;
                    var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                    var Amount = Nodo.Element("Amount").Value;
                    var ResponseAction = Nodo.Element("ResponseAction").Value;
                    var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                    DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                    clsVariablesBO.IsRemoteQuery = Convert.ToBoolean(Nodo.Element("IsRemoteQuery").Value);
                    var MovilAccess = Convert.ToBoolean(Nodo.Element("MovilAccess").Value);
                    int Gracia = int.Parse(Nodo.Element("Gracia").Value);
                    int PlanID = int.Parse(Nodo.Element("PlanID").Value);
                    int PaqueteID = int.Parse(Nodo.Element("PaqueteID").Value);
                    clsVariablesBO.IsCheckPrevious = Convert.ToBoolean(Nodo.Element("IsCheckPrevious").Value);
                    clsVariablesBO.GatewaySMS = Convert.ToBoolean(Nodo.Element("GatewaySMS").Value);
                    clsVariablesBO.CanUpdate = Convert.ToBoolean(Nodo.Element("CanUpdate").Value);
                    clsVariablesBO.UpdateAutomatically = Convert.ToBoolean(Nodo.Element("UpdateAutomatically").Value);
                    var ftp = Nodo.Element("ftp").Value;
                    var User = Nodo.Element("User").Value;
                    var pwd = Nodo.Element("Pwd").Value;
                    fechaPago = Fecha;
                    ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha, MovilAccess, PlanID, PaqueteID, Gracia);
                }
                else
                {
                    DateTime date = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), DateTime.Today.Month, DateTime.Today.Year));

                    XDocument document = new XDocument(
                        new XDeclaration("0.1", "utf-8", "yes"),
                        new XElement("Root",
                            new XElement("Payment",
                                new XElement("ResponseCode", "01"),
                                new XElement("ResponseMessage", ""),
                                new XElement("Amount", "0"),
                                new XElement("ResponseAction", "shutdown.exe"),
                                new XElement("ResponseArgument", "-r -t 60"),
                                new XElement("IsRemoteQuery", "false"),
                                new XElement("MovilAccess", "false"),
                                new XElement("PlanID", "0"),
                                new XElement("IsCheckPrevious", "false"),
                                new XElement("GatewaySMS", "false"),
                                new XElement("CanUpdate", "true"),
                                new XElement("UpdateAutomatically", "true"),
                                new XElement("Fecha", date.AddDays(3)),
                                new XElement("Gracia", "2"),
                                new XElement("PaqueteID", "0"),
                                new XElement("ftp", ""),
                                new XElement("User", ""),
                                new XElement("Pwd", ""))));

                    db.EmpresasUpdatePaymentGetByEmpresaID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, document.ToString());

                    String xml = document.ToString();
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");
                    var ResponseCode = Nodo.Element("ResponseCode").Value;
                    var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                    var Amount = Nodo.Element("Amount").Value;
                    var ResponseAction = Nodo.Element("ResponseAction").Value;
                    var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                    DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                    clsVariablesBO.IsRemoteQuery = Convert.ToBoolean(Nodo.Element("IsRemoteQuery").Value);
                    var MovilAccess = Convert.ToBoolean(Nodo.Element("MovilAccess").Value);
                    int Gracia = int.Parse(Nodo.Element("Gracia").Value);
                    int PlanID = int.Parse(Nodo.Element("PlanID").Value);
                    int PaqueteID = int.Parse(Nodo.Element("PaqueteID").Value);
                    clsVariablesBO.IsCheckPrevious = Convert.ToBoolean(Nodo.Element("IsCheckPrevious").Value);
                    clsVariablesBO.GatewaySMS = Convert.ToBoolean(Nodo.Element("GatewaySMS").Value);
                    clsVariablesBO.CanUpdate = Convert.ToBoolean(Nodo.Element("CanUpdate").Value);
                    clsVariablesBO.UpdateAutomatically = Convert.ToBoolean(Nodo.Element("UpdateAutomatically").Value);
                    var ftp = Nodo.Element("ftp").Value;
                    var User = Nodo.Element("User").Value;
                    var pwd = Nodo.Element("Pwd").Value;
                    fechaPago = Fecha;
                    ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha, MovilAccess, PlanID, PaqueteID, Gracia);
                }
            }
            catch { }
        }

        private bool HasConnection()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com");
                request.Timeout = 1000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
         try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch { }

        }

        private void GetMenu()
        {
            try
            {
                IsAutorized();
                frmProgress progress = new frmProgress();
                progress.Owner = this;
                progress.Loaded += (obj, arg) => {
                    gridMenu.Visibility = Visibility.Collapsed;
                    DataContext = clsVariablesBO.UsuariosBE;
                    border.BorderBrush = new SolidColorBrush(Colors.White);
                    gridMenu.Children.Add(clsMenuBO.GenerarMenu());
                    clsVariablesBO.Host = this;
                    lblWelcome.Text = clsLenguajeBO.Find(core.Bienvenida());
                };
                progress.Closing += async (obj, arg) => { gridMenu.Visibility = Visibility.Visible;
                    try
                    { 
                        lblPlan.Visibility = Visibility.Visible;
                        btnPassword.Visibility = Visibility.Visible;
                        //await hubConnection.StartAsync();
                        //await Task.Delay(200);
                        await hubConnection.InvokeAsync("JoinChat", clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, $"{clsVariablesBO.UsuariosBE.Personas.Nombres}");
                        
                        getNotification();
                       
                    }
                    catch(Exception ex)
                    { }
                };
                progress.ShowDialog();
            }
            catch { }
        }


        void getUpdate()
        {
            WebClient webClient = new WebClient();
            var client = new WebClient();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (!webClient.DownloadString("http://softarch.ddns.net/i-bank-update/Version.txt").Contains(assembly.GetName().Version.ToString()))
            {
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "¡Actualización Disponible!",
                    Message = @"Hay una nueva versión de i-Bank disponible para ser instalada. ¿Deseas instalarla ahora?",
                    Type = NotificationType.Warning,
                    
                }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(30), onClick: ()=> 
                {
                   _= getDownloadAsync();
                }, onClose: ()=> 
                { 
                    if(clsVariablesBO.CanUpdate && clsVariablesBO.UpdateAutomatically)
                    {
                       _= getDownloadAsync();
                    }
                });



            }
        }

        async Task getDownloadAsync()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                using (var httpClient = new HttpClient())
                {
                    var fileUrl = "http://softarch.ddns.net/i-bank-update/i-bank.zip";
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var filePath = System.IO.Path.Combine(path, "i-bank.zip");
                        using (var ms = await response.Content.ReadAsStreamAsync())
                        using (var fs = File.Create(filePath))
                        {
                            await ms.CopyToAsync(fs);
                            fs.Flush();
                        }
                    }
                }

                //if (File.Exists(@".\i-bank.zip")) { File.Delete(@".\i-bank.zip"); }
                if (File.Exists($@"{path}\i-bank.msi")) { File.Delete($@"{path}\i-bank.msi"); }

                string zipPath = $@"{path}\i-bank.zip";
                string extractPath = $@"{path}\";
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                Process process = new Process();
                process.StartInfo.FileName = "msiexec.exe";
                process.StartInfo.Arguments = string.Format($@"/i {path}\i-bank.msi");

                process.Start();
                Environment.Exit(0);
            }
            catch(Exception ex)
            {
                //clsMessage.ErrorMessage(ex.Message, "Mensaje");
            }
        }

        private void getNotification()
        {
            try
            {
                var db = new Core.Manager();

                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.LocalNotification)
                {
                    var result = db.SolicitudesGet(null, 1, clsVariablesBO.UsuariosBE.Documento).Count();
                    if (result > 0)
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Notificación",
                            Message = $"Hay {result} solicitudes con estado de Revision Pendiente.",
                            Type = NotificationType.Information
                        }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(10));
                    }

                    var result1 = db.ContratosGet(null, 0, clsVariablesBO.UsuariosBE.Documento).Count();
                    if (result1 > 0)
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Notificación",
                            Message = $"Hay {result1} contratos con estado de Revision Pendiente.",
                            Type = NotificationType.Information
                        }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(10));
                    }

                    try
                    {
                        var result2 = db.PrintCuotasAVencerGetByFecha(DateTime.Today, DateTime.Today, clsVariablesBO.UsuariosBE.SucursalID, -1).Tables[1].Rows.Count;
                        if (result2 > 0)
                        {
                            var notificationManager = new NotificationManager();
                            notificationManager.Show(new NotificationContent
                            {
                                Title = "Notificación",
                                Message = $"Hay {result2} cuotas que vencen este dia.",
                                Type = NotificationType.Information
                            }, areaName: "WindowArea", onClick: () => OpenCuotasAVencer(), expirationTime: TimeSpan.FromSeconds(60));

                        }
                    }
                    catch { }

                    var result3 = db.PrintListadoLlamadasGetByFecha(DateTime.Today, DateTime.Today, clsVariablesBO.UsuariosBE.SucursalID, true).Tables[1].Rows.Count;
                    if (result3 > 0)
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Notificación",
                            Message = $"Hay {result3} clientes que tienen compromisos que vencen este dia.",
                            Type = NotificationType.Information
                        }, areaName: "WindowArea", onClick: () => OpenLlamadas(), expirationTime: TimeSpan.FromSeconds(60));

                    }

                    //var result4 = db.PrintCuotasVencidasGetByFecha(DateTime.Today, clsVariablesBO.UsuariosBE.SucursalID, -1).Tables[0].Rows.Count;
                    //if (result4 > 0)
                    //{
                    //    var notificationManager = new NotificationManager();
                    //    notificationManager.Show(new NotificationContent
                    //    {
                    //        Title = "Notificación",
                    //        Message = $"Hay {result4} contratos que tienen cuotas vencidas.",
                    //        Type = NotificationType.Information
                    //    }, areaName: "WindowArea", onClick: () => OpenCuotasVencidas(), expirationTime: TimeSpan.FromSeconds(60));

                    //}

                }

                if (clsVariablesBO.CanUpdate)
                {
                    getUpdate();
                }

            }
            catch { }
        }


        void OpenCuotasVencidas()
        {
            try
            {
                frmPrintCuotasVencidasGetByFecha cuotas = new frmPrintCuotasVencidasGetByFecha();
                cuotas.Owner = this;
                cuotas.OnInit(DateTime.Today, clsVariablesBO.UsuariosBE.SucursalID, -1, 0);
                cuotas.ShowDialog();

            }
            catch
            {

            }
        }

        void OpenCuotasAVencer()
        {
            try {
                frmPrintCuotasAVencerGetByFecha cuotas = new frmPrintCuotasAVencerGetByFecha();
                cuotas.Owner = this;
                cuotas.OnInit(DateTime.Today, DateTime.Today, clsVariablesBO.UsuariosBE.SucursalID, -1);
                cuotas.ShowDialog();

            }
            catch
            {

            }
        }

        void OpenLlamadas()
        {
            try
            {
                frmPrintListadoLlamadasGetByFecha cuotas = new frmPrintListadoLlamadasGetByFecha();
                cuotas.Owner = this;
                cuotas.OnInit(DateTime.Today, DateTime.Today, clsVariablesBO.UsuariosBE.SucursalID, true);
                cuotas.ShowDialog();

            }
            catch
            {

            }
        }

        public void Load()
        {
            DataContext = clsVariablesBO.UsuariosBE;
            gridMenu.Children.Add(clsMenuBO.GenerarMenu());
            clsVariablesBO.Host = this;
            lblWelcome.Text = clsLenguajeBO.Find(core.Bienvenida());
        }

        private bool GetKey()
        {
           bool result = core.GetKey(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, clsCookiesBO.GetKey());
            return result;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmMessageBox msgBox = new frmMessageBox();
                msgBox.OnInit(clsLenguajeBO.Find("msgClose"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                msgBox.btnAceptar.Click += async (arg, obj) => 
                {
                    this.WindowState = WindowState.Minimized;
                    Application.Current.Shutdown(0);
                    if (Environment.MachineName.ToUpper() == "SERVIDOR-PC")
                    {
                        var BE = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Copias;
                        var result = await core.SetBackUpMySQL(BE, true);
                    }
                    else
                    {
                        var BE = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Copias;
                        var result = await core.SetRemoteBackUpMySQL(BE);
                    }
                };
                msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                msgBox.Owner = this;
                msgBox.ShowDialog();
            }
            catch { }
        }


        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void FrmPrincipal_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                clsVariablesBO.LenguajeID = clsCookiesBO.LenguajeID();
                clsVariablesBO.Etiquetas = core.EtiquetasGet();

                frmLogIn Login = new frmLogIn();
                Login.Owner = this;
                Login.Unloaded += (arg, obj) =>
                {
                    if (!string.IsNullOrEmpty(clsVariablesBO.UsuariosBE.Documento))
                    {
                        
                        if (GetKey())
                        {
                            GetMenu();
                        }
                        else
                        {
                            frmShutdown shut = new frmShutdown();
                            shut.Owner = this;
                            shut.Closed += (obj1, arg1) => { if (shut.IsValid == true) { GetMenu(); } };
                            shut.ShowDialog();
                        }
                    }
                };

                Login.ShowDialog();



                DispatcherTimer Timer = new DispatcherTimer();
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Tick += (s, a) =>
                {
                    try
                    {
                        txtTime.Text = String.Format("{0:00}:{1:00}:{2:00}", +DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        lblVersion.Text = "V " + assembly.GetName().Version;
                        if (System.DateTime.Now.Minute == 00 && System.DateTime.Now.Second == 00)
                        {
                            lblWelcome.Text = clsLenguajeBO.Find(core.Bienvenida());
                            IsAutorized();
                            getNotification();
                        }

                        if(this.hubConnection.State == HubConnectionState.Disconnected)
                        {
                            this.hubConnection.StartAsync();
                        }
                    }
                    catch { }
                };
                Timer.Start();
            }
            catch (Exception ex) { clsMessage.ErrorMessage(ex.Message, "Mensaje"); }
        }
    }
}
