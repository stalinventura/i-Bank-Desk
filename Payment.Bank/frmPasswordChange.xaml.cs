using Payment.Bank.Modulos;
using Payment.Bank.Controles;
using System;
using System.Windows;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmLogIn.xaml
    /// </summary>
    public partial class frmPasswordChange : Window
    {
        Core.Manager core = new Core.Manager();
        string internalKey = "http://softarch.ddns.net";
        public frmPasswordChange()
        {
            InitializeComponent();
            Loaded += frmPasswordChange_Loaded; ;

            btnSalir.Click += BtnSalir_Click;
            btnAceptar.Click += btnAceptar_Click; ;
            clsLenguajeBO.Load(gridLogin);
            Title = clsLenguajeBO.Find(Title.ToString());

           
            
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
           if(clsValidacionesBO.Validar(gridLogin))
            {
                string pwdEncoded = Core.Common.Security.GetHash(clsVariablesBO.UsuariosBE.Documento + txtClave.Password, Core.Common.HashType.SHA256);
                pwdEncoded = Common.FingerPrint.GetHash(pwdEncoded);

                if (pwdEncoded != clsVariablesBO.UsuariosBE.Contraseña)
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgInvalicPassword"), clsLenguajeBO.Find("msgTitle"));
                    txtClave.Clear();
                    txtClave.Focus();
                    return;
                }

                if(txtNewClave.Password != txtConfirmarClave.Password)
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgIncorrectConfirmation"), clsLenguajeBO.Find("msgTitle"));
                    txtNewClave.Clear();
                 
                    txtConfirmarClave.Clear();
                    txtNewClave.Focus();
                    return;
                }

                Core.Manager db = new Core.Manager();
                var result = db.PasswordUpdateGetByDocumento(clsVariablesBO.UsuariosBE.Documento, txtNewClave.Password, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    clsMessage.InfoMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                    
                    Application.Current.Shutdown();
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
            }
        }

        private void frmPasswordChange_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch { }
        }

    }
}
