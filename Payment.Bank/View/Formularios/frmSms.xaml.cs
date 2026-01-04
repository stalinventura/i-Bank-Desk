using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;


using Payment.Bank.Entity;
using Payment.Bank.Modulos;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmSms : MetroWindow
    {
        int VISTA = 1;
        clsSmsBE BE = new clsSmsBE();
        Core.Manager db = new Core.Manager();
        public int ContratoID = 0;
        clsContratosBE row = new clsContratosBE();

        public frmSms()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridSms);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            DataContext = new clsSmsBE();
            Loaded += frmSms_Loaded;
            txtCelular.LostFocus += txtCelular_LostFocus;
            txtCelular.KeyDown += TxtCelular_KeyDown;
        }

        private void TxtCelular_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtCelular_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtCelular.Text;
                txtCelular.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void frmSms_Loaded(object sender, RoutedEventArgs e)
        {
            row = db.ContratosGetByContratoID(ContratoID);
            txtCelular.Text = row.Solicitudes.Clientes.Personas.Celular;
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

        private async void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && row.Solicitudes.Condiciones.SendSMS == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        if (VISTA == 1 && clsValidacionesBO.Validar(gridSms))
                        {
                            bool save = false;
                            var result = new OperationResult();
                            if (clsVariablesBO.Permiso.Agregar == true)
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                                {
                                    string[] userID = new string[1];
                                    userID[0] = row.Solicitudes.Clientes.Documento;
                                    result = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtMensaje.Text, userID, clsVariablesBO.UsuariosBE.Documento);
                                    if(result.ResponseCode == "00")
                                    {
                                        save = true;
                                    }
                                }

                                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                                {

                                }

                                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                                {
                                    if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                                    {
                                        var response = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, txtMensaje.Text, row.Solicitudes.Clientes.Personas.OperadorID);
                                        if (response.ResponseCode == "00")
                                        {
                                            save = true;
                                        }
                                        else
                                        {
                                            clsMessage.ErrorMessage(response.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                        }
                                    }
                                }

                                if(save)
                                {
                                    var request = db.SmsCreate(txtCelular.Text, txtMensaje.Text, ContratoID, clsVariablesBO.UsuariosBE.Documento);
                                    if (request.ResponseCode == "00")
                                    {
                                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                        Limpiar();
                                    }
                                    else
                                    {
                                        clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                    }
                                }
                            }
                            else
                            {
                                clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                            }
                        }
                        else
                        {
                            if (VISTA == 2 && clsValidacionesBO.Validar(gridSms))
                            {
                                if (clsVariablesBO.Permiso.Modificar == true)
                                {
                                    bool save = false;
                                    var result = new OperationResult();
                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                                    {
                                        string[] userID = new string[1];
                                        userID[0] = row.Solicitudes.Clientes.Documento;
                                        await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtMensaje.Text, userID, clsVariablesBO.UsuariosBE.Documento);
                                        if (result.ResponseCode == "00")
                                        {
                                            save = true;
                                        }
                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                                    {

                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                                    {
                                        if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                                        {
                                            var response = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsUrl, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, txtMensaje.Text, BE.Contratos.Solicitudes.Clientes.Personas.OperadorID);
                                            if (response.ResponseCode == "00")
                                            {
                                                save = true;
                                            }
                                            else
                                            {
                                                clsMessage.ErrorMessage(response.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                            }
                                        }
                                    }

                                    if(save)
                                    {
                                        result = db.SmsUpdate(BE.smsID, txtCelular.Text, txtMensaje.Text, ContratoID, clsVariablesBO.UsuariosBE.Documento);
                                        if (result.ResponseCode == "00")
                                        {
                                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                            Limpiar();
                                        }
                                        else
                                        {
                                            clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                        }
                                    }

                                }
                                else
                                {
                                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void Limpiar()
        {
            try
            {
                txtMensaje.Text = string.Empty;
                txtMensaje.Focus();
                VISTA = 1;
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsSmsBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                //if (VISTA == 2)
                //{
                    BE = be;
                    DataContext = BE;
                //}
            }
            catch { }
        }     
    }
}
