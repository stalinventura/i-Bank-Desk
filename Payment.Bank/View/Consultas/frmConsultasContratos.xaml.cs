using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasContratos : MetroWindow
    {
        Contratos BE = new Contratos();
        clsContratosBE _BE = new clsContratosBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        public bool IsQueryPay = false;
        public int _ContratoID =0;
        Window wnds;
        public frmConsultasContratos()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                LoadCombox();
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                cmbEstadoContratos.SelectionChanged += cmbEstadoContratos_SelectionChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                btnCuotas.Click += btnCuotas_Click;
                btnCartas.Click += btnCartas_Click;
                btnBlackList.Click += btnBlackList_Click;
                btnSms.Click += btnSms_Click;
                btnTarjetas.Click += btnTarjetas_Click;
                btnEstadoCuentas.Click += btnEstadoCuentas_Click;
                btnContratos.Click += btnContratos_Click;
                txtBuscar.Focus();
                Loaded += frmConsultasContratos_Loaded;
                btnLocation.Click += btnLocation_Click;
                btnBitacora.Click += btnBitacora_Click;

                this.KeyDown += frmConsultasContratos_KeyDown;
                this.KeyUp+= frmConsultasContratos_KeyDown;
            }
            catch{}
        }

        private void frmConsultasContratos_KeyDown(object sender, KeyEventArgs e)
        {
            try
            { 
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    switch(e.Key)
                    {
                        case Key.N:
                            {
                                Agregar();
                            }break;
                        case Key.M:
                            {
                                Editar();
                            }
                            break;
                        case Key.E:
                            {
                                Delete();
                            }
                            break;
                    }
                }

                if(e.Key == Key.Escape)
                {
                    this.Close();
                }
            }
            catch { }
        }

        private void btnBitacora_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    frmConsultasBitacoras bitacoras = new frmConsultasBitacoras();
                    bitacoras.ContratoID = BE.ContratoID;
                    bitacoras.Owner = this;
                    bitacoras.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnTarjetas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    frmPrintTarjetasGetByContratoID Cuotas = new frmPrintTarjetasGetByContratoID();
                    Cuotas.OnInit(_BE.ContratoID);
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    var result = db.ContratosGetByContratoID(BE.ContratoID);

                    StringBuilder queryAddress = new StringBuilder();
                    queryAddress.Append("http://maps.google.com/maps?q=");

                    if (result.Solicitudes.Clientes.Latitude == 0 && result.Solicitudes.Clientes.Longitude == 0)
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgLocationNotAvailable"), clsLenguajeBO.Find("msgTitle"));
                        return;
                    }

                    if (result.Solicitudes.Clientes.Latitude != 0)
                    {
                        queryAddress.Append(result.Solicitudes.Clientes.Latitude + "%2C");
                    }

                    if (result.Solicitudes.Clientes.Longitude != 0)
                    {
                        queryAddress.Append(result.Solicitudes.Clientes.Longitude);
                    }

                    var url = new Uri(queryAddress.ToString());
                    Process.Start(url.ToString());

                    //}
                    //else
                    //{
                    //    clsme.Show("Ubicacion no disponible!");
                    //}
                }
            }
            catch
            {

            }
        }

        private void btnBlackList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    frmBlackList Suspender = new frmBlackList();
                    Suspender.OnInit(_BE);
                    Suspender.Owner = this;
                    Suspender.btnSalir.Click += (obj, arg) => { Find(); };
                    Suspender.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnCartas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        frmConsultasHistorialCartasGetByContratoID Cartas = new frmConsultasHistorialCartasGetByContratoID();
                        Cartas.Owner = this;
                        Cartas.ContratoID = BE.ContratoID;
                        Cartas.ShowDialog();
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSms_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        frmConsultasSms sms = new frmConsultasSms();
                        sms.Owner = this;
                        sms.ContratoID = BE.ContratoID;
                        sms.ShowDialog();
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void frmConsultasContratos_Loaded(object sender, RoutedEventArgs e)
        {
            if(IsQueryPay == true)
            {
                cmbEstadoContratos.IsEnabled = false;
            }
            wnds = Window.GetWindow(this);
        }

        private void btnContratos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    switch (BE.TipoSolicitudID)
                    {

                        case 2:
                            {
                                frmPrintContratosHipotecariosGetByContratoID Hipoteca = new frmPrintContratosHipotecariosGetByContratoID();
                                Hipoteca.OnInit(_BE);
                                Hipoteca.Owner = this;
                                Hipoteca.ShowDialog();

                                frmPrintContratosVentaHipotecariosGetByContratoID Venta = new frmPrintContratosVentaHipotecariosGetByContratoID();
                                Venta.OnInit(_BE);
                                Venta.Owner = this;
                                Venta.ShowDialog();

                                frmPrintActoEntregaVoluntariaGetByContratoID entrega = new frmPrintActoEntregaVoluntariaGetByContratoID();
                                entrega.OnInit(_BE);
                                entrega.Owner = this;
                                entrega.ShowDialog();
                            }
                            break;
                        case 3:
                            {
                                frmPrintContratosVehiculosGetByContratoID Vehiculos = new frmPrintContratosVehiculosGetByContratoID();
                                Vehiculos.OnInit(_BE);
                                Vehiculos.Owner = this;
                                Vehiculos.ShowDialog();

                                frmPrintActoEntregaVoluntariaGetByContratoID entrega = new frmPrintActoEntregaVoluntariaGetByContratoID();
                                entrega.OnInit(_BE);
                                entrega.Owner = this;
                                entrega.ShowDialog();


                                frmPrintCartaOposicionVehiculosGetByContratoID oposicion = new frmPrintCartaOposicionVehiculosGetByContratoID();
                                oposicion.OnInit(_BE);
                                oposicion.Owner = this;
                                oposicion.ShowDialog();
                            }
                            break;
                        case 6:
                            {
                                frmPrintActoEntregaVoluntariaGetByContratoID entrega = new frmPrintActoEntregaVoluntariaGetByContratoID();
                                entrega.OnInit(_BE);
                                entrega.Owner = this;
                                entrega.ShowDialog();
                            }
                            break;
                    }

                    frmPrintContratosGetByContratoID Cuotas = new frmPrintContratosGetByContratoID();
                    Cuotas.OnInit(_BE, false);
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();

                    frmPrintContratosAutenticoGetByContratoID Acto = new frmPrintContratosAutenticoGetByContratoID();
                    Acto.OnInit(_BE, true);
                    Acto.Owner = this;
                    Acto.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnEstadoCuentas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    frmPrintEstadoCuentasGetByContratoID Cuotas = new frmPrintEstadoCuentasGetByContratoID();
                    Cuotas.OnInit(BE.ContratoID);
                    Cuotas.Owner = wnds;
                    //Cuotas.Closed += (arg, obj)=> { OpcionesDesactivar(); BE = new clsContratosBE(); };
                    //Cuotas.Owner = Window.GetWindow(this);
                    Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch(Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnCuotas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.ContratoID > 0)
                {
                    frmConsultasCuotas Cuotas = new frmConsultasCuotas();
                    Cuotas.OnInit(_BE, DateTime.Now);
                    //Cuotas.Closed += (arg, obj)=> { OpcionesDesactivar(); BE = new clsContratosBE(); };
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }        

        private void cmbEstadoContratos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                RadBusyIndicator.IsActive = true;
                Find();
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Estados Contratos
            List<clsEstadoContratosBE> EstadoContratos = new List<clsEstadoContratosBE>();
            EstadoContratos = db.EstadoContratosGet(null).ToList();
            EstadoContratos.Add(new clsEstadoContratosBE { EstadoID = -1, Estado = clsLenguajeBO.Find("itemSelect") });
            cmbEstadoContratos.ItemsSource = EstadoContratos;
            cmbEstadoContratos.SelectedValuePath = "EstadoID";
            cmbEstadoContratos.DisplayMemberPath = "Estado";
            if (EstadoContratos.Count() > 1)
            {
                cmbEstadoContratos.SelectedValue = EstadoContratos.Where(x => x.IsDefault == true).FirstOrDefault().EstadoID;
            }
            else
            {
                cmbEstadoContratos.SelectedValue = -1;
            }
            Find();
        }

        private void OpcionesActivar()
        {
            btnEstadoCuentas.Visibility = Visibility.Visible;
            btnCuotas.Visibility = Visibility.Visible;
            btnTarjetas.Visibility = Visibility.Visible;
            btnSms.Visibility = Visibility.Visible;
            btnContratos.Visibility = Visibility.Visible;
            btnCartas.Visibility = Visibility.Visible;
            btnBlackList.Visibility = Visibility.Visible;
            btnLocation.Visibility = Visibility.Visible;
            btnBitacora.Visibility = Visibility.Visible;
        }

        private void OpcionesDesactivar()
        {
            btnEstadoCuentas.Visibility = Visibility.Collapsed;
            btnCuotas.Visibility = Visibility.Collapsed;
            btnTarjetas.Visibility = Visibility.Collapsed;
            btnSms.Visibility = Visibility.Collapsed;
            btnContratos.Visibility = Visibility.Collapsed;
            btnCartas.Visibility = Visibility.Collapsed;
            btnBlackList.Visibility = Visibility.Collapsed;
            btnLocation.Visibility = Visibility.Collapsed;
            btnBitacora.Visibility = Visibility.Collapsed;
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            Editar();
        }

        private void Editar()
        {
            try
            {
                if (BE.ContratoID > 0)
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
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionContractModify"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), BE.ContratoID, BE.Completo, Code);

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
                                            ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Contrato #{0} | {1}", BE.ContratoID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
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
            catch (Exception ex)
            { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        void ModificacionAutorizada(bool PermissionAccess = false)
        {
            try
            {
                VISTA = 2;
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsEasyContract)
                {
                    frmPrestamos Search = new frmPrestamos();
                    Search.OnInit(BE.ContratoID, 2);
                    Visibility = Visibility.Hidden;
                    Search.PermissionAccess = PermissionAccess;
                    Search.Closed += (obj, args) =>
                            {
                                BE = new Contratos(); dataGrid1.SelectedIndex = -1;
                                Find();
                                Visibility = Visibility.Visible;
                            };
                    Search.Owner = this;
                    Search.ShowDialog();
                }
                else
                {
                    frmContratos Search = new frmContratos();
                    Search.OnInit(BE.ContratoID, 2);
                    Visibility = Visibility.Hidden;
                    Search.PermissionAccess = PermissionAccess;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new Contratos(); dataGrid1.SelectedIndex = -1;
                        Find();
                        Visibility = Visibility.Visible;
                    };
                    Search.Owner = this;
                    Search.ShowDialog();
                }
            }
            catch { }
        }

        void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            Agregar();
        }

        private void Agregar()
        {
            try
            {
                if (clsVariablesBO.Permiso.Agregar == true)
                {
                    var count = db.ContratosGet(null, 1, clsVariablesBO.UsuariosBE.Documento).Count();
                    if (count <= clsVariablesBO.UsuariosBE.Sucursales.Empresas.Planes.Hasta)
                    {
                        VISTA = 1;
                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsEasyContract)
                        {
                            frmPrestamos Search = new frmPrestamos();
                            Visibility = Visibility.Hidden;
                            Search.Closed += (obj, args) =>
                            {
                                BE = new Contratos(); dataGrid1.SelectedIndex = -1;
                                Find();
                                Visibility = Visibility.Visible;
                            };
                            Search.Owner = this;
                            Search.ShowDialog();
                        }
                        else
                        {
                            frmContratos Search = new frmContratos();
                            Visibility = Visibility.Hidden;
                            Search.Closed += (obj, args) =>
                            {
                                BE = new Contratos(); dataGrid1.SelectedIndex = -1;
                                Find();
                                Visibility = Visibility.Visible;
                            };
                            Search.Owner = this;
                            Search.ShowDialog();
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage(string.Format(clsLenguajeBO.Find("msgLimitPlan"), clsVariablesBO.UsuariosBE.Sucursales.Empresas.Planes.Hasta, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Planes.Plan), clsLenguajeBO.Find("msgTitle"));
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
            Delete();
        }

        private void Delete()
        {
            try
            {
                if (BE.ContratoID > 0)
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
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionContractDelete"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), BE.ContratoID, BE.Completo, Code);

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
                                            ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Contrato #{0} | {1}", BE.ContratoID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
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
            catch
            {
                //clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); 
            }
        }

        private void EliminarAutorizado(bool PermissionAccess = false)
        {
           try
            {
                switch (BE.EstadoID)
                {
                    case 1:
                        {
                            frmSuspenderContratos Suspender = new frmSuspenderContratos();
                            Suspender.OnInit(_BE);
                            Suspender.Owner = this;
                            Suspender.PermissionAccess = PermissionAccess;
                            Suspender.Closed += (obj, arg) => { Find(); };
                            Suspender.ShowDialog();
                        }
                        break;
                    case 2:
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgCancelContract"), clsLenguajeBO.Find("msgTitle"));
                        }
                        break;
                    default:
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgSuspendContract"), clsLenguajeBO.Find("msgTitle"));
                        }
                        break;
                }
            }
            catch { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (Contratos)dataGrid1.SelectedItem;
                _BE = db.ContratosGetByContratoID(BE.ContratoID);
                if (IsQueryPay == false)
                {
                    OpcionesActivar();
                }
                else
                {
                    _ContratoID = BE.ContratoID;
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
                    RadBusyIndicator.IsActive = true;
                    Find();
                }
            }
            catch { }
        }
              
        public void Find()
        {
            try
            {
                RadBusyIndicator.IsActive = true;
                OpcionesDesactivar();
                var Result = db.ContratosGet(txtBuscar.Text, (int)cmbEstadoContratos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                dataGrid1.ItemsSource = Result;
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
                float monto = Result.Sum(x => x.Monto);
                lblMonto.Text = String.Format("{0:N2}", monto);
                RadBusyIndicator.IsActive = false;

                if (!String.IsNullOrEmpty(txtBuscar.Text))
                {
                    if (!IsQueryPay)
                    {
                        dataGrid1.SelectedItem = Result.FirstOrDefault();
                    }
                }
            }
            catch (Exception Ex) { RadBusyIndicator.IsActive = false; clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }


    }


}

