using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.View.Informes;
using Payment.Bank.Core.Model;
using Payment.Bank.Core.Entity;
using Payment.Bank.Controles;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasRecibos : MetroWindow
    {
        Recibos BE = new Recibos();
        clsRecibosBE _BE = new clsRecibosBE();

        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasRecibos()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
     
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtDesde.SelectedDateChanged += txtDesde_SelectedDateChanged;
                txtHasta.SelectedDateChanged += txtHasta_SelectedDateChanged;
                Loaded += frmConsultasRecibos_Loaded;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                txtBuscar.Focus();
                
                if(clsVariablesBO.UsuariosBE.Roles.CanQuery == true)
                {
                    txtDesde.IsEnabled = true;
                    txtHasta.IsEnabled = true;
                }
                else
                {
                    txtDesde.IsEnabled = false;
                    txtHasta.IsEnabled = false;
                }
 
            }
            catch{}
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                Recibos.Owner = this;
                Recibos.OnInit(BE.ReciboID);
                Recibos.ShowDialog();

                clsCuotasView Cuotas = new clsCuotasView();
                Cuotas.SetDataSource(BE.ContratoID, BE.ReciboID);
                var R = Cuotas.GetGroup();
                var balance = (Cuotas.BalanceGetByReciboID() < 0 ? 0 : Cuotas.BalanceGetByReciboID());

                if (balance < (float)clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.MinimumAmount)
                {
                    frmPrintCartaSaldo Saldo = new frmPrintCartaSaldo();
                    Saldo.Owner = this;
                    Saldo.OnInit(BE.ReciboID);
                    Saldo.ShowDialog();

                    var request = db.RecibosGetByReciboID(BE.ReciboID);
                    switch (request.Contratos.Solicitudes.TipoSolicitudID)
                    {
                        case 2:
                            {
                                frmPrintCertificacionDocumentos doc = new frmPrintCertificacionDocumentos();
                                doc.Owner = this;
                                doc.OnInit(BE.ReciboID);
                                doc.ShowDialog();
                            }
                            break;
                        case 3:
                            {
                                frmPrintCertificacionDocumentos doc = new frmPrintCertificacionDocumentos();
                                doc.Owner = this;
                                doc.OnInit(BE.ReciboID);
                                doc.ShowDialog();
                            }
                            break;
                    }
                }

            }
            catch { }
        }

        private void txtHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Find();
            }
            catch { }
        }

        private void txtDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                Find();
            }
            catch { }
        }

        private void frmConsultasRecibos_Loaded(object sender, RoutedEventArgs e)
        {
           try
            {
                txtDesde.SelectedDate = DateTime.Today;
                txtHasta.SelectedDate = DateTime.Today;
            }
            catch { }
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ReciboID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        ModificacionAutorizada();
                    }
                    else
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit(clsLenguajeBO.Find("msgRequestAccess"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += async (arg, obj) =>
                        {
                            if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true)
                            {
                                if (Common.Generic.HasAccess() == true)
                                {
                                    msgBox.Close();
                                    var p = db.GerentesGetByDocumento(clsVariablesBO.UsuariosBE.Sucursales.Documento).Personas;
                                    string Code = Common.Generic.GenerarCodigo(4);
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionReceiptModify"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), BE.ContratoID, BE.Completo, Code);

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                                    {
                                        string[] userID = new string[1];
                                        userID[0] = clsVariablesBO.UsuariosBE.Documento;
                                        var response = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                                        if (response.ResponseCode == "00")
                                        {
                                            frmPin Pin = new frmPin();
                                            Pin.OnInit(Code);
                                            Pin.Closed += (a, b) =>
                                            {
                                                if (Pin.Confirmado == true)
                                                {
                                                    Pin.Close();
                                                    ModificacionAutorizada(true);
                                                }
                                            };
                                            Pin.ShowDialog();
                                        }
                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                                    {
                                        try
                                        {
                                            var template = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/PermissionRequest.html");
                                            string Body = string.Format(template, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos, Mensaje, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, DateTime.Now.Year, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Site);

                                            Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                            ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Recibos #{0} | {1}", BE.ReciboID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
                                        }
                                        catch { }
                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                                    {
                                        if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                                        {
                                            var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Celular, Mensaje, p.OperadorID);
                                            if (smsResult.ResponseCode == "00")
                                            {
                                                frmPin Pin = new frmPin();
                                                Pin.OnInit(Code);
                                                Pin.Closed += (a, b) =>
                                                {
                                                    if (Pin.Confirmado == true)
                                                    {
                                                        Pin.Close();
                                                        ModificacionAutorizada(true);
                                                    }
                                                };
                                                Pin.ShowDialog();
                                            }
                                            else
                                            {
                                                clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgMessagingNetworkNotAvailable"), clsLenguajeBO.Find("msgTitle"));
                                }
                            }
                            else
                            {
                                clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                            }
                        };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                        msgBox.Owner = this;
                        msgBox.ShowDialog();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void ModificacionAutorizada(bool PermissionAccess = false)
        {
          try
            {
                VISTA = 2;
                frmRecibos Search = new frmRecibos();
                Search.OnInit(_BE, 2);
                Visibility = Visibility.Hidden;
                Search.PermissionAccess = PermissionAccess;
                Search.Closed += (obj, args) =>
                {
                    BE = new Recibos(); dataGrid1.SelectedIndex = -1;
                    Find();
                    Visibility = Visibility.Visible;
                };
                Search.Owner = this;
                Search.ShowDialog();
            }
            catch { }
        }

        void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.Permiso.Agregar == true)
                {
                    if (!db.IsOpenBox(clsVariablesBO.UsuariosBE.Documento) && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsOpenCloseBox==true)
                    {
                        var row = db.OpenBoxGetByDocumento(clsVariablesBO.UsuariosBE.Documento);
                        if (row == null)
                        {
                            frmTurnos turnos = new frmTurnos();
                            turnos.Owner = this;
                            turnos.btnSalir.Click += (obj, arg) =>
                              {
                                  if (!db.IsOpenBox(clsVariablesBO.UsuariosBE.Documento))
                                  {
                                      return;
                                  }
                              };
                            turnos.ShowDialog();
                        }
                        else 
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgOpenBox"), clsLenguajeBO.Find("msgTitle"));
                        }
                    }
                    else
                    {
                        VISTA = 1;
                        frmRecibos Search = new frmRecibos();
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new Recibos(); dataGrid1.SelectedIndex = -1;
                            Find();
                            Visibility = Visibility.Visible;
                        };
                        Search.Owner = this;
                        Search.ShowDialog();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }
        
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ReciboID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        EliminarAutorizado();
                    }
                    else
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit(clsLenguajeBO.Find("msgRequestAccess"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += async (arg, obj) =>
                        {
                            if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true)
                            {
                                if (Common.Generic.HasAccess() == true)
                                {
                                    msgBox.Close();
                                    var p = db.GerentesGetByDocumento(clsVariablesBO.UsuariosBE.Sucursales.Documento).Personas;
                                    string Code = Common.Generic.GenerarCodigo(4);
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionReceiptDelete"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), BE.ContratoID, BE.Completo, Code);

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                                    {
                                        string[] userID = new string[1];
                                        userID[0] = clsVariablesBO.UsuariosBE.Documento;
                                        var response = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                                        if (response.ResponseCode == "00")
                                        {
                                            frmPin Pin = new frmPin();
                                            Pin.OnInit(Code);
                                            Pin.Closed += (a, b) =>
                                            {
                                                if (Pin.Confirmado == true)
                                                {
                                                    Pin.Close();
                                                    EliminarAutorizado(true);
                                                }
                                            };
                                            Pin.ShowDialog();
                                        }
                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                                    {
                                        try
                                        {
                                            var template = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/PermissionRequest.html");
                                            string Body = string.Format(template, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos, Mensaje, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, DateTime.Now.Year, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Site);

                                            Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                            ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Recibos #{0} | {1}", BE.ReciboID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
                                        }
                                        catch { }
                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                                    {
                                        if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                                        {
                                            var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Celular, Mensaje, p.OperadorID);
                                            if (smsResult.ResponseCode == "00")
                                            {
                                                frmPin Pin = new frmPin();
                                                Pin.OnInit(Code);
                                                Pin.Closed += (a, b) =>
                                                {
                                                    if (Pin.Confirmado == true)
                                                    {
                                                        Pin.Close();
                                                        EliminarAutorizado(true);
                                                    }
                                                };
                                                Pin.ShowDialog();
                                            }
                                            else
                                            {
                                                clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgMessagingNetworkNotAvailable"), clsLenguajeBO.Find("msgTitle"));
                                }
                            }
                            else
                            {
                                clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                            }
                        };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                        msgBox.Owner = this;
                        msgBox.ShowDialog();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { 
                //clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
            }
        }

        private void EliminarAutorizado(bool PermissionAccess = false)
        {
            try
            {
                if (BE.EstadoID == true)
                {
                    frmRecibosDelete Suspender = new frmRecibosDelete();
                    Suspender.OnInit(_BE);
                    Suspender.Owner = this;
                    Suspender.PermissionAccess = PermissionAccess;
                    Suspender.Closed += (obj, arg) => { Find(); };
                    Suspender.ShowDialog();
                }
            }
            catch { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                 BE = (Recibos)dataGrid1.SelectedItem;
                _BE = db.RecibosGetByReciboID(BE.ReciboID);
            }
            catch { }
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Find();
                }
            }
            catch { }
        }

        public void Find()
        {
            try
            {
                if (txtDesde.SelectedDate != null && txtHasta.SelectedDate != null)
                {
                    var Result = db.RecibosGet((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, txtBuscar.Text, clsVariablesBO.UsuariosBE.Documento);
                    dataGrid1.ItemsSource = Result.ToList();
                    lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
                }
            
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }


    }


}

