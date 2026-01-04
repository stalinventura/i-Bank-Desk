using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;


using Payment.Bank.Modulos;
using Payment.Bank.Core;
using System.ServiceModel;
using Payment.Bank.Controles;
using Microsoft.Win32;
using System.IO;
using System.Web;
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes;
using Payment.Bank.Properties;
using Payment.Bank.Core.Model;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmAlquileres.xaml
    /// </summary>
    public partial class frmAlquileres : MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        string _DocumentoGarante = string.Empty;

        //Valiables Temporales       
        int _OcupacionID;
        int _HorarioID;
        int _IngresoID;
        int _ClienteID;
        int _AlquilerID;
        int _TipoAlquilerID = 0;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>(); 
        Core.Manager db = new Core.Manager();
        public frmAlquileres()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridSolicitudes);
            clsLenguajeBO.Load(gridDatosPersonales);
            clsLenguajeBO.Load(gridDatosEconomicos);
            clsLenguajeBO.Load(gridContactoEmergencia);
            clsLenguajeBO.Load(gridDatosGarantiaHipotecario);
            clsLenguajeBO.Load(gridDatosGarantiaPersonal);
            clsLenguajeBO.Load(gridDatosGarantiaVehiculo);
            sectionE.Text = clsLenguajeBO.Find(sectionE.Text);

            if (VISTA == 1)
            {
                gridSolicitudes.DataContext = new clsAlquileresBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                gridDatosGarantiaHipotecario.DataContext = new clsGarantiaHipotecariaBE();
                gridDatosGarantiaVehiculo.DataContext = new clsGarantiaVehiculosBE();
            }

            btnSalir.Click += btnSalir_Click;
            cmbTipoAlquileres.SelectionChanged += cmbTipoAlquileres_SelectionChanged;
            Loaded += frmSolicitudes_Loaded;
            rbSi.Checked += rbSi_Checked;
            rbNo.Checked += rbNo_Checked;
            txtDocumento.LostFocus += txtDocumento_LostFocus;
            txtDocumento.KeyDown += txtDocumento_KeyDown;
            txtDocumento.TextChanged += txtDocumento_TextChanged;
            txtTelefono.LostFocus += txtTelefono_LostFocus;
            txtTelefono.KeyDown += txtTelefono_KeyDown;
            txtCelular.LostFocus += txtCelular_LostFocus;
            txtCelular.KeyDown += txtCelular_KeyDown;
            txtTelefonoTrabajo.LostFocus += txtTelefonoTrabajo_LostFocus;
            txtTelefonoTrabajo.KeyDown += txtTelefonoTrabajo_KeyDown;
            txtTiempo.KeyDown += txtTiempo_KeyDown;
            txtMonto.KeyDown += txtMonto_KeyDown;

            btnAceptar.Click += txtnAceptar_Click;
            btnAdd.Click += btnAdd_Click;
            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;
            btnAdd_srcPhotoGarante.Click += btnAdd_srcPhotoGarante_Click;

            //Garantias
            txtDocumentoGarante.LostFocus += txtDocumentoGarante_LostFocus;
            txtDocumentoGarante.KeyDown += txtDocumentoGarante_KeyDown; ;
            txtDocumentoGarante.TextChanged += txtDocumentoGarante_TextChanged;
            txtTelefonoGarante.LostFocus += txtTelefonoGarante_LostFocus;
            txtTelefonoGarante.KeyDown += txtTelefonoGarante_KeyDown; ;
            txtCelularGarante.LostFocus += txtCelularGarante_LostFocus; ;
            txtCelularGarante.KeyDown += txtCelularGarante_KeyDown;


            cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;



            btnAddBusiness.Click += btnAddBusiness_Click;
        }

        private void txtDocumentoGarante_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDocumentoGarante.Text))
                {
                    _DocumentoGarante = string.Empty;
                    gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                }
            }
            catch { }
        }

        private void txtDocumento_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDocumento.Text))
                {
                    _Documento = string.Empty;
                    gridDatosPersonales.DataContext = new clsPersonasBE();
                    gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                    _Referencias = new List<clsReferenciasBE>();
                    dtgContactoEmergencias.ItemsSource = _Referencias;
                }
            }
            catch { }
        }

        private void btnAddBusiness_Click(object sender, RoutedEventArgs e)
        {
            try {
                frmInstituciones f = new frmInstituciones();
                f.Owner = this;
                f.Closed += (obj, arg) => { InstitucionesGet(); };
                f.ShowDialog();
            }
            catch { }

        }

        private void cmbInstituciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((int)cmbInstituciones.SelectedValue != -1)
                {
                    txtDireccionTrabajo.Text = (cmbInstituciones.SelectedItem as clsInstitucionesBE).Direccion;
                    txtTelefonoTrabajo.Text = (cmbInstituciones.SelectedItem as clsInstitucionesBE).Telefono;
                }
            }
            catch { }
        }

        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key|| e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtCelularGarante_KeyDown(object sender, KeyEventArgs e)
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

        private void txtCelularGarante_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtCelularGarante.Text;
                txtCelularGarante.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void txtTelefonoGarante_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTelefonoGarante_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtTelefonoGarante.Text;
                txtTelefonoGarante.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void txtDocumentoGarante_KeyDown(object sender, KeyEventArgs e)
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

        private void btnAdd_srcPhotoGarante_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (openFileDialog.ShowDialog() == true)
                {
                    srcPhotoGarante.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                }
            }
            catch { }
        }

        private void btnAdd_srcPhoto_Click(object sender, RoutedEventArgs e)
        {
            try {
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (openFileDialog.ShowDialog() == true)
                {                    
                    srcPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                }
            }
            catch { }
        }

        private void txtDocumentoGarante_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Documento = txtDocumentoGarante.Text;
                txtDocumentoGarante.Text = clsValidacionesBO.DocumentFormat(Documento);

                if (Common.Generic.ValidarDocumento(txtDocumentoGarante.Text) == true)
                {
                    if (!String.IsNullOrEmpty(txtDocumentoGarante.Text))
                    {
                        var Persona = db.PersonasGetByDocumento(txtDocumentoGarante.Text, clsVariablesBO.IsRemoteQuery, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                        if (Persona != null)
                        {
                            _DocumentoGarante = Persona.Documento;
                            gridDatosGarantiaPersonal.DataContext = Persona;                          
                        }
                        else
                        {
                            _DocumentoGarante = string.Empty;
                            //clsValidacionesBO.Limpiar(gridDatosGarantiaPersonal);
                            txtDocumentoGarante.Text = clsValidacionesBO.DocumentFormat(Documento);
                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage("Numero de documento incorrecto.", clsLenguajeBO.Find("msgTitle"));
                    txtDocumentoGarante.Text = "";
                }
            }
            catch { }

        }

        private void cmbTipoAlquileres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TipoSolicitudChange();
        }

        private void TipoSolicitudChange()
        {
            try
            {
                switch ((int)cmbTipoAlquileres.SelectedValue)
                {
                    case 1:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonal.Visibility = Visibility.Visible;
                            //gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                            wpGarantias.Visibility = Visibility.Visible;

                        }
                        break;      
                    default:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonal.Visibility = Visibility.Collapsed;
                            wpGarantias.Visibility = Visibility.Collapsed;
                        }
                        break;
                }
            }
            catch { }
        }

        public void OnInit(int AlquilerID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                clsAlquileresBE row = db.AlquileresGetByAlquilerID(AlquilerID);
                _AlquilerID = AlquilerID;
                _Documento = row.Documento;
                //_ClienteID = row.Clientes.ClienteID;

                cmbApartamentos.ItemsSource = db.ApartamentosGet(null).Where(x=>x.ApartamentoID == row.ApartamentoID);
                srcPhoto.DataContext = row.Personas.Fotografias;
               //Solicitud
                gridSolicitudes.DataContext = row;
                gridDatosPersonales.DataContext = row.Personas;


                if (row.Personas.DatosEconomicos.Trabaja == true)
                {
                    rbSi.IsChecked = true;
                }
                else
                {
                    rbNo.IsChecked = true;
                }

                gridDatosEconomicos.DataContext = row.Personas.DatosEconomicos;
                dtgContactoEmergencias.ItemsSource = row.Personas.Referencias;
                _Referencias = row.Personas.Referencias.ToList();
                gridDatosGarantiaPersonal.DataContext = row.GarantiaAlquileres.Personas;
                srcPhotoGarante.DataContext = row.Personas.Fotografias;

            }
            catch { }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
          try
            {
                frmReferencias Contactos = new frmReferencias();
                Contactos.Referencias = _Referencias;
                Contactos.Closed += (obj, arg) => { dtgContactoEmergencias.ItemsSource = new List<clsReferenciasBE>(); dtgContactoEmergencias.ItemsSource = Contactos.Referencias; _Referencias = Contactos.Referencias; };
                Contactos.ShowDialog();

            }
            catch (Exception ex){ clsMessage.ErrorMessage(ex.Message, "-"); }
        }

        void SavePhoto(string numero, byte[] data)
        {
            try
            {
                if (data.Length > 0)
                {
                    db.FotografiasCreate(numero, data, clsVariablesBO.UsuariosBE.Documento);
                }
            }
            catch { }
        }

        private void PersonasCreate()
        {
            try
            {
               OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, null, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
               if(result.ResponseCode == "00")
                {
                    ClientesCreate();
                    if ((gridDatosPersonales.DataContext as clsPersonasBE).Fotografias != null)
                    {
                        byte[] data = (gridDatosPersonales.DataContext as clsPersonasBE).Fotografias.Foto;
                        SavePhoto(txtDocumento.Text, data);
                    }
                }
               else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void PersonasUpdate()
        {
            try
            {              
                OperationResult result = db.PersonasUpdate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, openFileDialog.FileName, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    if (VISTA == 1)
                    {
                        ClientesCreate();
                        if ((gridDatosPersonales.DataContext as clsPersonasBE).Fotografias != null)
                        {
                            byte[] data = (gridDatosPersonales.DataContext as clsPersonasBE).Fotografias.Foto;
                            SavePhoto(txtDocumento.Text, data);
                        }
                    }
                    else
                    {
                        DatosEconomicosUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void ClientesCreate()
        {
            try
            {
                OperationResult result = db.ClientesCreate(txtDocumento.Text, clsVariablesBO.UsuariosBE.SucursalID,0,0, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    _ClienteID = Convert.ToInt32(result.ResponseMessage);
                    if (VISTA == 1)
                    {
                        DatosEconomicosCreate();
                    }
                    else
                    {
                        DatosEconomicosUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void DatosEconomicosCreate()
        {
            try
            {
                bool trabaja = ((bool)rbSi.IsChecked == true? true : false);
                OperationResult result = db.DatosEconomicosCreate(txtDocumento.Text,trabaja, (int)cmbInstituciones.SelectedValue, (int)cmbOcupaciones.SelectedValue, (int)cmbHorarios.SelectedValue, (int)cmbIngresos.SelectedValue,  clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    if (_Referencias != null)
                    {
                        ReferenciasCreate();
                    }
                    else
                    {
                        SolicitudesCreate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void DatosEconomicosUpdate()
        {
            try
            {
                bool trabaja = ((bool)rbSi.IsChecked == true ? true : false);
                OperationResult result = db.DatosEconomicosUpdate(txtDocumento.Text, trabaja, (int)cmbInstituciones.SelectedValue, (int)cmbOcupaciones.SelectedValue, (int)cmbHorarios.SelectedValue, (int)cmbIngresos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    List<clsReferenciasBE> List = new List<clsReferenciasBE>();
                    if (_Referencias != null)
                    {
                        ReferenciasCreate();
                    }
                    else
                    {
                        SolicitudesUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void SolicitudesCreate()
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarAlquiler == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                        {
                            string Code = Common.Generic.GenerarCodigo(4);
                            string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarAlquiler), Common.Generic.ShortName(txtNombres.Text), (cmbTipoAlquileres.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtMonto.Text, Code);

                            var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsUrl, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, Mensaje, (int)cmbOperadores.SelectedValue);

                            if (smsResult.ResponseCode == "00")
                            {
                                frmPin Pin = new frmPin();
                                Pin.OnInit(Code);
                                Pin.Closed += (a, b) =>
                                {
                                    if (Pin.Confirmado == true)
                                    {
                                        SaveAlquileresCreate();
                                        Pin.Close();
                                    }
                                    RadBusyIndicator.IsActive = false;
                                };
                                Pin.ShowDialog();
                            }
                            else
                            {
                                clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                RadBusyIndicator.IsActive = false;
                            }
                        }
                        else
                        {
                            if (VISTA == 1) { SaveAlquileresCreate(); } else { SaveAlquileresUpdate(); }
                        }
                    }
                    else
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit("La red de mensajería no esta disponible en estos momentos. ¿Deseas continuar y almacenar los datos de la solicitud ?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += (arg, obj) => { if (VISTA == 1) { SaveAlquileresCreate(); } else { SaveAlquileresUpdate(); } };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); RadBusyIndicator.IsActive = false; };
                        msgBox.ShowDialog();
                    }
                }
               else
                {
                    if (VISTA == 1) { SaveAlquileresCreate(); } else { SaveAlquileresUpdate(); }
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); RadBusyIndicator.IsActive = false; }
        }

        private void GarantiasCreate(int ID)
        {
            try
            {
                switch((int)cmbTipoAlquileres.SelectedValue)
                {
                    case 1: //Con Garantia
                        {
                            GarantesCreate(ID);                            
                        }
                        break;
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void GarantesCreate(int ID)
        {
            try
            {
                OperationResult result = db.PersonasCreate(1, txtDocumentoGarante.Text, (DateTime)txtFechaNacimientoGarante.SelectedDate, txtNombresGarante.Text, txtApellidosGarante.Text, txtApodoGarante.Text, openFileDialog.FileName, (int)cmbCiudadesGarante.SelectedValue, txtDireccionGarante.Text, txtCorreoElectronicoGarante.Text, txtTelefonoGarante.Text, (int)cmbOperadoresGarante.SelectedValue, txtCelularGarante.Text, (int)cmbSexosGarante.SelectedValue, (int)cmbEstadosCivilesGarante.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    //db.GarantesCreate(txtDocumento.Text, clsVariablesBO.UsuariosBE.Documento);
                    db.GarantiaAlquileresCreate(ID, _DocumentoGarante, clsVariablesBO.UsuariosBE.Documento);
                    byte[] data = (gridDatosGarantiaPersonal.DataContext as clsPersonasBE).Fotografias.Foto;
                    SavePhoto(txtDocumentoGarante.Text, data);
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

       private void gotoPrint(int ID)
        {
            try
            {
                frmPrintAlquileresGetByAlquilerID Print = new frmPrintAlquileresGetByAlquilerID();
                Print.OnInit(ID);
                Print.ShowDialog();
            }
            catch { }
        }


        private void SaveAlquileresCreate()
        {
            try
            {
                DateTime date = (DateTime)txtFecha.SelectedDate;
                OperationResult result = db.AlquileresCreate(date, (int)cmbTipoAlquileres.SelectedValue, (int)cmbApartamentos.SelectedValue, txtDocumento.Text, Convert.ToInt32(txtTiempo.Text), (int)cmbNotarioPublico.SelectedValue, float.Parse(txtMonto.Text),  clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    float Monto = 0;
                    int dia = DateTime.DaysInMonth(date.Year, date.Month);
                    if(dia == 31)
                    {
                        dia = 30;
                    }
                    DateTime fecha = Convert.ToDateTime(dia.ToString() + "/" + date.Month.ToString() + "/" + date.Year.ToString());
                    TimeSpan ts = fecha - DateTime.Today;
                    Monto = float.Parse(txtMonto.Text); //(float.Parse(txtMonto.Text) / dia) * ts.Days;
                    if (ts.Days < 5)
                    {
                        Monto = 0;
                    }

                    float Desposito = float.Parse(txtMonto.Text) * float.Parse(txtTiempo.Text);
                    db.RentasCreate(DateTime.Now, int.Parse(result.ResponseMessage), date.Month, date.Year, Desposito, true, clsVariablesBO.UsuariosBE.Documento);

                    db.RentasCreate(fecha, int.Parse(result.ResponseMessage), date.Month, date.Year, Monto, false, clsVariablesBO.UsuariosBE.Documento);
                    GarantiasCreate(int.Parse(result.ResponseMessage));
                    gotoPrint(int.Parse(result.ResponseMessage));
                   
                    ClearAll();
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                RadBusyIndicator.IsActive = false;
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        void ClearAll()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridSolicitudes);
                clsValidacionesBO.Limpiar(gridDatosPersonales);
                clsValidacionesBO.Limpiar(gridDatosEconomicos);
                clsValidacionesBO.Limpiar(gridDatosGarantiaHipotecario);
                clsValidacionesBO.Limpiar(gridDatosGarantiaPersonal);
                clsValidacionesBO.Limpiar(gridDatosGarantiaVehiculo);

                gridSolicitudes.DataContext = new clsSolicitudesBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosGarantiaHipotecario.DataContext = new clsGarantiaHipotecariaBE();
                gridDatosGarantiaVehiculo.DataContext = new clsGarantiaVehiculosBE();
                gridDatosGarantiaPersonal.DataContext = new clsGarantiaPersonalBE();
                _Referencias = new List<clsReferenciasBE>();

                dtgContactoEmergencias.ItemsSource = _Referencias;

                _AlquilerID = 0;
                _OcupacionID = 0;
                _HorarioID = 0;
                _IngresoID = 0;
                _AlquilerID = 0;
                _Documento = string.Empty;
                _ClienteID = 0;                            
                VISTA = 1;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
                cmbInstituciones.SelectedValue = 100001;
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;

                cmbCiudades.SelectedValue = -1;
                cmbCiudadesGarante.SelectedValue = -1;
                cmbColores.SelectedValue = -1;
                cmbModelos.SelectedValue = -1;
                cmbTipoVehiculos.SelectedValue = -1;
                
                cmbSexos.SelectedValue = -1;
                cmbSexosGarante.SelectedValue = -1;
                cmbEstadosCiviles.SelectedValue = -1;
                cmbEstadosCivilesGarante.SelectedValue = -1;
                cmbOperadores.SelectedValue = -1;
                cmbOperadoresGarante.SelectedValue = -1;

                txtTiempo.Text = string.Empty;
                LoadApartamentos();
            }
            catch { }
        }

        private void SolicitudesUpdate()
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarAlquiler == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                        {
                            string Code = Common.Generic.GenerarCodigo(4);
                            string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarAlquiler), Common.Generic.ShortName(txtNombres.Text), (cmbTipoAlquileres.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtMonto.Text, Code);

                            var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsUrl, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, Mensaje, (int)cmbOperadores.SelectedValue);

                            if (smsResult.ResponseCode == "00")
                            {
                                frmPin Pin = new frmPin();
                                Pin.OnInit(Code);
                                Pin.Closed += (a, b) =>
                                {
                                    if (Pin.Confirmado == true)
                                    {
                                        SaveAlquileresUpdate();
                                        Pin.Close();
                                    }
                                    RadBusyIndicator.IsActive = false;
                                };
                                Pin.ShowDialog();
                            }
                            else
                            {
                                clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                RadBusyIndicator.IsActive = false;
                            }
                        }
                        else
                        {
                            if (VISTA == 1) { SaveAlquileresCreate(); } else { SaveAlquileresUpdate(); }
                        }
                    }
                    else
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit("La red de mensajería no esta disponible en estos momentos. ¿Deseas continuar y almacenar los datos de la solicitud ?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += (arg, obj) => { if (VISTA == 1) { SaveAlquileresCreate(); } else { SaveAlquileresUpdate(); } };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); RadBusyIndicator.IsActive = false; };
                        msgBox.ShowDialog();
                    }
                }
                else
                {
                    if (VISTA == 1) { SaveAlquileresCreate(); } else { SaveAlquileresUpdate(); }
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); RadBusyIndicator.IsActive = false; }
        }

        private void SaveAlquileresUpdate()
        {
            try
            {
                OperationResult result = db.AlquileresUpdate(_AlquilerID, (DateTime)txtFecha.SelectedDate, (int)cmbTipoAlquileres.SelectedValue, (int)cmbApartamentos.SelectedValue, txtDocumento.Text, Convert.ToInt32(txtTiempo.Text), (int)cmbNotarioPublico.SelectedValue, float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    gotoPrint(_AlquilerID);
                    //Aqui va el mensaje final
                    //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSolicitudSuccess"), clsLenguajeBO.Find("msgTitle"));
                    ClearAll();
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                RadBusyIndicator.IsActive = false;
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void ReferenciasCreate()
        {
            try
            {
                OperationResult result = db.ReferenciasDeleteGetByDocumento(txtDocumento.Text);
                if (result.ResponseCode == "00")
                {
                    foreach (clsReferenciasBE row in _Referencias)
                    {
                        db.ReferenciasCreate(row.TipoReferenciaID, txtDocumento.Text, row.Referencia, row.Direccion, row.Telefono, clsVariablesBO.UsuariosBE.Documento);
                    }
                    if (VISTA == 1)
                    {
                        SolicitudesCreate();
                    }
                    else
                    {
                        SolicitudesUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }

        private void txtTiempo_KeyDown(object sender, KeyEventArgs e)
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

        private void txtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                                                                                                                                                               // color = (n.Difference1 < 0) ? Negativo : (n.Difference1 > 0) ? Positivo : Balanceado,                                                            
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true && (int)cmbTipoAlquileres.SelectedValue == 2? clsValidacionesBO.Validar(gridDatosGarantiaHipotecario): (int)cmbTipoAlquileres.SelectedValue == 3 ? clsValidacionesBO.Validar(gridDatosGarantiaVehiculo) : (int)cmbTipoAlquileres.SelectedValue == 1 ? clsValidacionesBO.Validar(gridDatosGarantiaPersonal) == true : true)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        RadBusyIndicator.IsActive = true;
                        if (_Documento == string.Empty)
                        {
                            PersonasCreate();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_Documento))
                            {
                                PersonasUpdate();
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true && (int)cmbTipoAlquileres.SelectedValue == 2 ? clsValidacionesBO.Validar(gridDatosGarantiaHipotecario) : (int)cmbTipoAlquileres.SelectedValue == 3 ? clsValidacionesBO.Validar(gridDatosGarantiaVehiculo) : (int)cmbTipoAlquileres.SelectedValue == 1 ? clsValidacionesBO.Validar(gridDatosGarantiaPersonal) == true: true )
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            RadBusyIndicator.IsActive = true;
                            if (!string.IsNullOrEmpty(_Documento))
                            {
                                PersonasUpdate();
                            }
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                        }
                    }
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void txtTelefonoTrabajo_KeyDown(object sender, KeyEventArgs e)
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

        private void txtCelular_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTelefono_KeyDown(object sender, KeyEventArgs e)
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

        private void txtDocumento_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTelefonoTrabajo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtTelefonoTrabajo.Text;
                txtTelefonoTrabajo.Text = clsValidacionesBO.PhoneFormat(Phone);
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

        private void txtTelefono_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtTelefono.Text;
                txtTelefono.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void txtDocumento_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Documento = txtDocumento.Text;
                txtDocumento.Text = clsValidacionesBO.DocumentFormat(Documento);

                if (Common.Generic.ValidarDocumento(txtDocumento.Text) == true)
                {
                    if (!String.IsNullOrEmpty(txtDocumento.Text))
                    {
                        var Persona = db.PersonasGetByDocumento(txtDocumento.Text, clsVariablesBO.IsRemoteQuery, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                        if (Persona != null)
                        {
                            _Documento = Persona.Documento;
                            gridDatosPersonales.DataContext = Persona;
                            if (Persona.DatosEconomicos != null)
                            {
                                if (Persona.DatosEconomicos.Trabaja == true)
                                {
                                    rbSi.IsChecked = true;
                                }
                                else
                                {
                                    rbNo.IsChecked = true;
                                }
                                gridDatosEconomicos.DataContext = Persona.DatosEconomicos;
                            }

                            if (Persona.Referencias.Count > 0)
                            {

                                dtgContactoEmergencias.ItemsSource = Persona.Referencias;
                                _Referencias = Persona.Referencias.ToList();
                            }

                            //VerificarDeudas();
                        }
                        else
                        {
                            string x = txtDocumento.Text;
                            clsValidacionesBO.Limpiar(gridDatosPersonales);
                            clsValidacionesBO.Limpiar(gridDatosEconomicos);
                            _Referencias = new List<clsReferenciasBE>();
                            dtgContactoEmergencias.ItemsSource = _Referencias;
                            rbNo.IsChecked = true;
                            txtDocumento.Text = x;
                            txtFechaNacimiento.SelectedDate = DateTime.Now;
                            //_Documento = string.Empty;
                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage("Numero de documento incorrecto.", clsLenguajeBO.Find("msgTitle"));
                    txtDocumento.Text = "";
                }
            }
            catch { }
        }

        private void VerificarDeudas()
        {
            try
            {
                foreach(clsContratosBE C in db.ContratosGet().Where(x=>x.Solicitudes.Clientes.Documento == txtDocumento.Text))
                {
                    clsCuotasView Cuotas = new clsCuotasView();
                    Cuotas.SetDataSource(C.ContratoID,0);                    
                    var balance = (Cuotas.BalanceGetByReciboID() < 0 ? 0 : Cuotas.BalanceGetByReciboID());
                    if (balance > 0)
                    {
                        clsMessage.ErrorMessage(string.Format(clsLenguajeBO.Find("msgBalancePending"), C.Solicitudes.Clientes.Personas.Nombres + " " + C.Solicitudes.Clientes.Personas.Apellidos, C.ContratoID, string.Format("{0:N2}",balance)), clsLenguajeBO.Find("msgTitle"));
                    }
                    return;
                }
            }
            catch { }
        }

        private void rbNo_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFechaIngreso.IsEnabled = false;
                cmbInstituciones.IsEnabled = false;
                txtDireccionTrabajo.IsEnabled = false;
                txtTelefonoTrabajo.IsEnabled = false;
                cmbOcupaciones.IsEnabled = false;
                cmbHorarios.IsEnabled = false;
                cmbIngresos.IsEnabled = false;

                cmbInstituciones.SelectedValue = 100001;
                _OcupacionID = (int)cmbOcupaciones.SelectedValue;
                _HorarioID = (int)cmbHorarios.SelectedValue;
                _IngresoID = (int)cmbIngresos.SelectedValue;

            
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
            }
            catch { }
        }

        private void rbSi_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFechaIngreso.IsEnabled = true;
                cmbInstituciones.IsEnabled = true;
                //txtDireccionTrabajo.IsEnabled = true;
                //txtTelefonoTrabajo.IsEnabled = true;
                cmbOcupaciones.IsEnabled = true;
                cmbHorarios.IsEnabled = true;
                cmbIngresos.IsEnabled = true;


                cmbOcupaciones.SelectedValue = _OcupacionID;
                cmbHorarios.SelectedValue = _HorarioID;
                cmbIngresos.SelectedValue = _IngresoID;
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Tipo de Alquileres
            List<clsTipoAlquileresBE> TipoAlquileres = new List<clsTipoAlquileresBE>();
            TipoAlquileres = db.TipoAlquileresGet(null).ToList();
            TipoAlquileres.Add(new clsTipoAlquileresBE { TipoAlquilerID = -1, TipoAlquiler = clsLenguajeBO.Find("itemSelect") });
            cmbTipoAlquileres.ItemsSource = TipoAlquileres;
            cmbTipoAlquileres.SelectedValuePath = "TipoAlquilerID";
            cmbTipoAlquileres.DisplayMemberPath = "TipoAlquiler";
            if (TipoAlquileres.Count() > 1)
            {
                _TipoAlquilerID = TipoAlquileres.Where(x => x.IsDefault == true).FirstOrDefault().TipoAlquilerID;
                cmbTipoAlquileres.SelectedValue = _TipoAlquilerID;
            }
            else
            {
                cmbTipoAlquileres.SelectedValue =  -1;
            }

            LoadApartamentos();




            //Notarios Publico
            List<clsNotariosPublicoBE> Notarios = new List<clsNotariosPublicoBE>();
            Notarios = db.NotariosPublicoGet(null).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID).ToList();
            Notarios.Add(new clsNotariosPublicoBE { NotarioPublicoID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemSelect") } });

            List<clsNotariosPublicoBE> List = new List<clsNotariosPublicoBE>();
            foreach (var row in Notarios)
            {
                List.Add(new clsNotariosPublicoBE { NotarioPublicoID = row.NotarioPublicoID, SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
            }

            cmbNotarioPublico.ItemsSource = List;
            cmbNotarioPublico.SelectedValuePath = "NotarioPublicoID";
            cmbNotarioPublico.DisplayMemberPath = "Personas.Nombres";
            cmbNotarioPublico.SelectedValue = -1;


            //Ciudades
            List<clsCiudadesBE> Ciudades = new List<clsCiudadesBE>();
            Ciudades = db.CiudadesGet(null).ToList();
            Ciudades.Add(new clsCiudadesBE { CiudadID = -1, Ciudad = clsLenguajeBO.Find("itemSelect") });
            cmbCiudades.ItemsSource = Ciudades;
            cmbCiudades.SelectedValuePath = "CiudadID";
            cmbCiudades.DisplayMemberPath = "Ciudad";
            cmbCiudades.SelectedValue = -1;//Ciudades.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            //Garantias
            cmbCiudadesGarante.ItemsSource = Ciudades;
            cmbCiudadesGarante.SelectedValuePath = "CiudadID";
            cmbCiudadesGarante.DisplayMemberPath = "Ciudad";
            cmbCiudadesGarante.SelectedValue = -1;


            //Operadores
            List<clsOperadoresBE> Operadores = new List<clsOperadoresBE>();
            Operadores = db.OperadoresGet(null).ToList();
            Operadores.Add(new clsOperadoresBE { OperadorID = -1, Operador = clsLenguajeBO.Find("itemSelect") });
            cmbOperadores.ItemsSource = Operadores;
            cmbOperadores.SelectedValuePath = "OperadorID";
            cmbOperadores.DisplayMemberPath = "Operador";
            cmbOperadores.SelectedValue = -1;//Operadores.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID

            //Garantias
            cmbOperadoresGarante.ItemsSource = Operadores;
            cmbOperadoresGarante.SelectedValuePath = "OperadorID";
            cmbOperadoresGarante.DisplayMemberPath = "Operador";
            cmbOperadoresGarante.SelectedValue = -1;


            //Sexos
            List<clsSexosBE> Sexos = new List<clsSexosBE>();
            Sexos = db.SexosGet(null).ToList();
            Sexos.Add(new clsSexosBE { SexoID = -1, Sexo = clsLenguajeBO.Find("itemSelect") });
            cmbSexos.ItemsSource = Sexos;
            cmbSexos.SelectedValuePath = "SexoID";
            cmbSexos.DisplayMemberPath = "Sexo";
            cmbSexos.SelectedValue = -1;//Sexos.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            //Garantias
            cmbSexosGarante.ItemsSource = Sexos;
            cmbSexosGarante.SelectedValuePath = "SexoID";
            cmbSexosGarante.DisplayMemberPath = "Sexo";
            cmbSexosGarante.SelectedValue = -1;


            //Estados Civiles
            List<clsEstadosCivilesBE> EstadosCiviles = new List<clsEstadosCivilesBE>();
            EstadosCiviles = db.EstadosCivilesGet(null).ToList();
            EstadosCiviles.Add(new clsEstadosCivilesBE { EstadoCivilID = -1, EstadoCivil = clsLenguajeBO.Find("itemSelect") });
            cmbEstadosCiviles.ItemsSource = EstadosCiviles;
            cmbEstadosCiviles.SelectedValuePath = "EstadoCivilID";
            cmbEstadosCiviles.DisplayMemberPath = "EstadoCivil";
            cmbEstadosCiviles.SelectedValue = -1;//EstadosCiviles.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            //Garantias
            cmbEstadosCivilesGarante.ItemsSource = EstadosCiviles;
            cmbEstadosCivilesGarante.SelectedValuePath = "EstadoCivilID";
            cmbEstadosCivilesGarante.DisplayMemberPath = "EstadoCivil";
            cmbEstadosCivilesGarante.SelectedValue = -1;


            InstitucionesGet();

            cmbInstituciones.SelectedValue = 100001;
            //Ocupaciones
            List<clsOcupacionesBE> Ocupaciones = new List<clsOcupacionesBE>();
            Ocupaciones = db.OcupacionesGet(null).ToList();
            Ocupaciones.Add(new clsOcupacionesBE { OcupacionID = -1, Ocupacion = clsLenguajeBO.Find("itemSelect") });
            cmbOcupaciones.ItemsSource = Ocupaciones;
            cmbOcupaciones.SelectedValuePath = "OcupacionID";
            cmbOcupaciones.DisplayMemberPath = "Ocupacion";
            cmbOcupaciones.SelectedValue = 1;//Ocupaciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
        
            //Horarios
            List<clsHorariosBE> Horarios = new List<clsHorariosBE>();
            Horarios = db.HorariosGet(null).ToList();
            Horarios.Add(new clsHorariosBE { HorarioID = -1, Horario = clsLenguajeBO.Find("itemSelect") });
            cmbHorarios.ItemsSource = Horarios;
            cmbHorarios.SelectedValuePath = "HorarioID";
            cmbHorarios.DisplayMemberPath = "Horario";
            cmbHorarios.SelectedValue = 1;//Horarios.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            //Ingresos
            List<clsIngresosBE> Ingresos = new List<clsIngresosBE>();
            Ingresos = db.IngresosGet(null).ToList();
            Ingresos.Add(new clsIngresosBE { IngresoID = -1, Ingreso = clsLenguajeBO.Find("itemSelect") });
            cmbIngresos.ItemsSource = Ingresos;
            cmbIngresos.SelectedValuePath = "IngresoID";
            cmbIngresos.DisplayMemberPath = "Ingreso";
            cmbIngresos.SelectedValue = 1;//Ingresos.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;


            //Tipo de Vehiculos
            List<clsTipoVehiculosBE> TipoVehiculo = new List<clsTipoVehiculosBE>();
            TipoVehiculo = db.TipoVehiculosGet(null).ToList();
            TipoVehiculo.Add(new clsTipoVehiculosBE { TipoVehiculoID = -1, TipoVehiculo = clsLenguajeBO.Find("itemSelect") });
            cmbTipoVehiculos.ItemsSource = TipoVehiculo;
            cmbTipoVehiculos.SelectedValuePath = "TipoVehiculoID";
            cmbTipoVehiculos.DisplayMemberPath = "TipoVehiculo";
            cmbTipoVehiculos.SelectedValue = -1;

            //Modelos
            List<clsModelosBE> Modelos = new List<clsModelosBE>();
            Modelos = db.ModelosGet(null).ToList();
            List<clsModelosBE> list = new List<clsModelosBE>();
            foreach(var row in Modelos)
            {
                list.Add(new clsModelosBE { ModeloID = row.ModeloID, Modelo = row.Marcas.Marca + " / " + row.Modelo });
            }
            list.Add(new clsModelosBE { ModeloID = -1, Modelo = clsLenguajeBO.Find("itemSelect") });
            cmbModelos.ItemsSource = list.OrderBy(x => x.Modelo);
            cmbModelos.SelectedValuePath = "ModeloID";
            cmbModelos.DisplayMemberPath = "Modelo";
            cmbModelos.SelectedValue = -1;

            //Colores
            List<clsColoresBE> Colores = new List<clsColoresBE>();
            Colores = db.ColoresGet(null).ToList();
            Colores.Add(new clsColoresBE { ColorID = -1, Color = clsLenguajeBO.Find("itemSelect") });
            cmbColores.ItemsSource = Colores;
            cmbColores.SelectedValuePath = "ColorID";
            cmbColores.DisplayMemberPath = "Color";
            cmbColores.SelectedValue = -1;

           
        }

        private void LoadApartamentos()
        {
            try
            {
                //Apartamentos
                List<clsApartamentosBE> Apartamentos = new List<clsApartamentosBE>();
                Apartamentos = db.ApartamentosDisponiblesGet();

                List<clsApartamentosBE> _list = new List<clsApartamentosBE>();
                foreach (var row in Apartamentos)
                {
                    _list.Add(new clsApartamentosBE { ApartamentoID = row.ApartamentoID, Apartamento = row.Edificios.Edificio + " / " + row.Pisos.Piso + " / " + row.Apartamento });
                }

                _list.Add(new clsApartamentosBE { ApartamentoID = -1, Apartamento = clsLenguajeBO.Find("itemSelect") });
                cmbApartamentos.ItemsSource = _list.OrderBy(x => x.Apartamento);
                cmbApartamentos.SelectedValuePath = "ApartamentoID";
                cmbApartamentos.DisplayMemberPath = "Apartamento";
                cmbApartamentos.SelectedValue = -1;

            }
            catch { }
        }

        private void InstitucionesGet()
        {
            //Instituciones
            List<clsInstitucionesBE> Instituciones = new List<clsInstitucionesBE>();
            Instituciones = db.InstitucionesGet(null).ToList();
            Instituciones.Add(new clsInstitucionesBE { InstitucionID = -1, Institucion = clsLenguajeBO.Find("itemSelect") });
            cmbInstituciones.ItemsSource = Instituciones;
            cmbInstituciones.SelectedValuePath = "InstitucionID";
            cmbInstituciones.DisplayMemberPath = "Institucion";
            cmbInstituciones.SelectedValue = -1;//Instituciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
        }

        private void frmSolicitudes_Loaded(object sender, RoutedEventArgs e)
        {

            if (VISTA == 1)
            {
                txtFecha.SelectedDate = DateTime.Now;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
                txtFechaNacimientoGarante.SelectedDate = DateTime.Now;
                txtFechaExpedicion.SelectedDate = DateTime.Now;
                LoadCombox();
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
            }
            TipoSolicitudChange();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
