using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.Controles;
using Payment.Bank.View.Informes;
using Payment.Bank.Core.Entity;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasSolicitudes : MetroWindow
    {
        Solicitudes BE = new Solicitudes();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        public bool IsQuery;
        public int _SolicitudID = 0;
        public int _EstadoID;
        public frmConsultasSolicitudes()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                LoadCombox();
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                cmbEstadoSolicitudes.SelectionChanged += cmbEstadoSolicitudes_SelectionChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtBuscar.Focus();
            }
            catch{}
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           try
            {
                frmPrintSolicitudesGetBySolicitudID Solicitud = new frmPrintSolicitudesGetBySolicitudID();
                Solicitud.OnInit(BE.SolicitudID);
                Solicitud.ShowDialog();

                if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                {
                    frmPrintReglasSolicitudesGetBySolicitudID reglas = new frmPrintReglasSolicitudesGetBySolicitudID();
                    reglas.OnInit(BE.SolicitudID);
                    reglas.ShowDialog();
                }
            }
            catch { }
        }

        private void cmbEstadoSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                Find();
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Estados Solicitudes
            List<clsEstadoSolicitudesBE> EstadoSolicitudes = new List<clsEstadoSolicitudesBE>();
            EstadoSolicitudes = db.EstadoSolicitudesGet(null).ToList();
            EstadoSolicitudes.Add(new clsEstadoSolicitudesBE { EstadoID = -1, Estado = clsLenguajeBO.Find("itemSelect") });
            cmbEstadoSolicitudes.ItemsSource = EstadoSolicitudes;
            cmbEstadoSolicitudes.SelectedValuePath = "EstadoID";
            cmbEstadoSolicitudes.DisplayMemberPath = "Estado";
            if (EstadoSolicitudes.Count() > 1)
            {
                if (IsQuery == false)
                {
                    cmbEstadoSolicitudes.SelectedValue = EstadoSolicitudes.Where(x => x.IsDefault == true).FirstOrDefault().EstadoID;
                }
                else
                {
                    cmbEstadoSolicitudes.SelectedValue = _EstadoID;
                }
            }
            else
            {
                cmbEstadoSolicitudes.SelectedValue = -1;
            }

            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.SolicitudID > 0)
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
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionRequestModify"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), BE.SolicitudID, BE.Completo, Code);

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
                                            string Body = string.Format(template, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion,clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos, Mensaje, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, DateTime.Now.Year, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Site);

                                            Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                            ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Solicitud #{0} | {1}", BE.SolicitudID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
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

        void ModificacionAutorizada(bool PermissionAccess = false)
        {
            try
            {
                VISTA = 2;
                frmSolicitudes Search = new frmSolicitudes();
                Search.OnInit(BE.SolicitudID, 2);
                Visibility = Visibility.Hidden;
                Search.PermissionAccess = PermissionAccess;
                Search.Closed += (obj, args) =>
                {
                    BE = new Solicitudes(); dataGrid1.SelectedIndex = -1;
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
                    VISTA = 1;
                    frmSolicitudes Search = new frmSolicitudes();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new Solicitudes(); dataGrid1.SelectedIndex = -1;
                        Find();
                        Visibility = Visibility.Visible;

                    };
                    Search.Owner = this;
                    Search.ShowDialog();
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
                if (BE.SolicitudID > 0)
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
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionRequestDelete"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), BE.SolicitudID, BE.Completo, Code);

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
                                                    EliminarAutorizado();
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
                                            ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Solicitud #{0} | {1}", BE.SolicitudID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
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
                                                        EliminarAutorizado();
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

        private void EliminarAutorizado()
        {
            try {
                frmMessageBox Mensaje = new frmMessageBox();
                Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                Mensaje.btnAceptar.Click += (obj, args) =>
                {
                    OperationResult result = db.SolicitudesDelete(BE.SolicitudID);
                    if (result.ResponseCode == "00")
                    {
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgDelete"), clsLenguajeBO.Find("msgTitle"));
                    }
                    else
                    {
                        clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                    }
                    Find();
                    Mensaje.Close();
                };

                Mensaje.btnSalir.Click += (obj, args) =>
                {
                    Mensaje.Close();
                };
                Mensaje.ShowDialog();
            } catch { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (Solicitudes)dataGrid1.SelectedItem;
                if (IsQuery == true)
                {
                    _SolicitudID = BE.SolicitudID;
                    Close();
                }
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
                if (IsQuery == true)
                {
                    cmbEstadoSolicitudes.IsEnabled = false;
                    cmbEstadoSolicitudes.SelectedValue = _EstadoID;
                }

                var Result = db.SolicitudesGet(txtBuscar.Text, (int)cmbEstadoSolicitudes.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
                float monto = Result.Sum(x => x.Monto);
                lblMonto.Text = String.Format("{0:N2}", monto);
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }


}

