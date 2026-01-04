using Payment.Bank.Modulos;
using Payment.Bank.Controles;
using System;
using System.Windows;
using Payment.Bank.DAL;
using System.Linq;
using Payment.Bank.Core.Common;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmLogIn.xaml
    /// </summary>
    public partial class frmLogIn : Window
    {
        Core.Manager core = new Core.Manager();
        string internalKey = "http://softarch.ddns.net";
        public frmLogIn()
        {
            InitializeComponent();
            Loaded += FrmLogin_Loaded;

            btnSalir.Click += BtnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            clsLenguajeBO.Load(gridLogin);
            Title = clsLenguajeBO.Find(Title.ToString());

            txtCorreo.GotFocus += txtCorreo_GotFocus;
            txtClave.GotFocus += txtClave_GotFocus;
            
        }

        private void txtClave_GotFocus(object sender, RoutedEventArgs e)
        {
            lblMensaje.Text = string.Empty;
        }

        private void txtCorreo_GotFocus(object sender, RoutedEventArgs e)
        {
            lblMensaje.Text = string.Empty;
        }

        private async void FrmLogin_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {                                           
                txtCorreo.Text = clsCookiesBO.CorreosGet();
                if (!string.IsNullOrEmpty(txtCorreo.Text))
                {
                    txtClave.Focus();
                }
                else
                {
                    txtCorreo.Focus();
                    lblMensaje.Text = string.Empty;
                }
               //txtCorreo.Text = Payment.Bank.Common.Generic.Encryption(""); //Payment.Bank.Core.Common.Security.GetHash("6ED1-9BA3-604B-8113-1F66-8E8B-7089-9080" + internalKey, Payment.Bank.Core.Common.HashType.MD5);

            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //txtCorreo.Text = Common.Generic.Decoding(txtClave.Password);
                //clsMessage.InfoMessage(Common.Generic.Decoding("cGVndWVybzI1Mzc="), "");
                //RadBusyIndicator.IsActive = true;
                var BE = core.LogIn(txtCorreo.Text, txtClave.Password);
                if (BE != null)
                {
                    if (BE.EstadoID == true)
                    {
                        clsVariablesBO.UsuariosBE = BE;

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SetLogo)
                        {
                            clsVariablesBO.Host.SetLogo();
                        }
                        //clsVariablesBO.Host.gridMenu.Children.Add(clsMenuBO.GenerarTabMenu());
                        if (!string.IsNullOrEmpty(clsVariablesBO.UsuariosBE.Documento))
                        {
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                            var file = System.IO.Path.Combine(path, "Lenguaje.txt");

                            if (System.IO.File.Exists(file))
                            {
                                System.IO.File.Delete(file);
                            }
                            clsCookiesBO.LenguajeID(BE.LenguajeID);
                            GotoPrincipal();
                        }
                    }
                    else
                    {
                        lblMensaje.Text = "Este usuario no tiene acceso.";
                    }
                }
                else
                {
                    lblMensaje.Text = "Usuario o Contraseña incorrectos";
                    //RadBusyIndicator.IsActive = false;
                }
            }
            catch(Exception ex) { /*RadBusyIndicator.IsActive = false;*/
                Payment.Bank.Core.Common.clsMisc ms = new Core.Common.clsMisc();

                clsMessage.ErrorMessage(ex.Message, clsLenguajeBO.Find("msgTitle"));

                //ms.SMTPHOST = Settings.Default.SERVIDOR;
                //ms.SENDER_MAIL = Settings.Default.Correo;
                //ms.SENDER_PSW = Settings.Default.Password;
                //ms.SMTPORT = Settings.Default.Puerto;
                //ms.SMTPSSL = Settings.Default.SSL;

                //ms.SendMail("SERVIDOR_ventura@outlook.com", ex.Message + " | " + ex.InnerException.Message.ToString() , ex.Message);

            }
        }

        private void GotoPrincipal()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = System.IO.Path.Combine(path, "Correo.txt");
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
                clsCookiesBO.CorreosCreate(txtCorreo.Text);                
                Close();
            }
            catch { }
        }


        private void BtnSalir_Click(object sender, RoutedEventArgs e)
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

    }
}
