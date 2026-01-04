using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;


using Payment.Bank.Modulos;
using Payment.Bank.Controles;
using Microsoft.Win32;
using Payment.Bank.View.Informes;
using Payment.Bank.Core.Model;
using System.IO;
using Payment.Bank.Model.Request;
using Newtonsoft.Json;
using Payment.Bank.View.Consultas;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmSolicitudes : MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        string _Rnc = string.Empty;
        string _DocumentoGarante = string.Empty;
        string photo = string.Empty;
        string photoGarante = string.Empty;
        public bool PermissionAccess = false;

        //Valiables Temporales       
        int _OcupacionID =-1;
        int _HorarioID =-1;
        int _IngresoID=-1;
        int _ClienteID;
        int _SolicitudID;
        int _EstadoID;
        int _CondicionID = 0;
        int _TipoSolicitudID = 0;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>();
        clsReferenciasBE _referencia = new clsReferenciasBE();
        List<clsDependientesBE> _Dependientes = new List<clsDependientesBE>();
        clsDependientesBE _dependiente = new clsDependientesBE();

        Core.Manager db = new Core.Manager();

        public frmSolicitudes()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height * 0.95;
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridSolicitudes);
            clsLenguajeBO.Load(gridDatosPersonales);
            clsLenguajeBO.Load(gridDatosEconomicos);
            clsLenguajeBO.Load(gridContactoEmergencia);
            clsLenguajeBO.Load(gridDependientes);
            clsLenguajeBO.Load(gridDatosGarantiaHipotecario);
            clsLenguajeBO.Load(gridDatosGarantiaPersonal);
            clsLenguajeBO.Load(gridDatosGarantiaVehiculo);
            clsLenguajeBO.Load(gridDatosEconomicosGarantes);
            clsLenguajeBO.Load(gridTarjetas);
            clsLenguajeBO.Load(gridDatosGarantiaComercial);

            sectionE.Text = clsLenguajeBO.Find(sectionE.Text);
            chkDocument.Content = clsLenguajeBO.Find(chkDocument.Content.ToString());

            if (VISTA == 1)
            {
                gridSolicitudes.DataContext = new clsSolicitudesBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                gridDatosGarantiaHipotecario.DataContext = new clsGarantiaHipotecariaBE();
                gridDatosGarantiaVehiculo.DataContext = new clsGarantiaVehiculosBE();
                gridDatosEconomicosGarantes.DataContext = new clsDatosEconomicosBE() { Fecha = DateTime.Now};
                gridDatosGarantiaTarjetas.DataContext = new clsGarantiaTarjetasBE();
                gridDatosGarantiaComercial.DataContext = new clsComerciosBE();
            }

            btnSalir.Click += btnSalir_Click;
            cmbTipoSolicitudes.SelectionChanged += cmbTipoSolicitudes_SelectionChanged;
            cmbCondicionSolicitudes.SelectionChanged += cmbCondicionSolicitudes_SelectionChanged;
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
            txtNumero.KeyDown += txtNumero_KeyDown;
            txtCvv.KeyDown += txtCvv_KeyDown;

            btnAceptar.Click += btnAceptar_Click;

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            dtgContactoEmergencias.SelectionChanged += dtgContactoEmergencias_SelectionChanged;

            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;
            btnAdd_srcPhotoGarante.Click += btnAdd_srcPhotoGarante_Click;

            btnAdd_srcCamera.Click += btnAdd_srcCamera_Click;
            btnAdd_srcCameraGarante.Click += btnAdd_srcCameraGarante_Click;

            dtgDependientes.SelectionChanged += dtgDependientes_SelectionChanged;
            btnAddDependientes.Click += btnAddDependientes_Click;
            btnEditDependientes.Click += btnEditDependientes_Click;
            btnDeleteDependientes.Click += BtnDeleteDependientes_Click;


            //Garantias
            txtDocumentoGarante.LostFocus += txtDocumentoGarante_LostFocus;
            txtDocumentoGarante.KeyDown += txtDocumentoGarante_KeyDown; ;
            txtDocumentoGarante.TextChanged += txtDocumentoGarante_TextChanged;
            txtTelefonoGarante.LostFocus += txtTelefonoGarante_LostFocus;
            txtTelefonoGarante.KeyDown += txtTelefonoGarante_KeyDown; ;
            txtCelularGarante.LostFocus += txtCelularGarante_LostFocus; ;
            txtCelularGarante.KeyDown += txtCelularGarante_KeyDown;
            rbGaranteSi.Checked += rbGaranteSi_Checked;
            rbGaranteNo.Checked += rbGaranteNo_Checked;

            cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;

            //Permisos
            if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                cmbEstados.IsEnabled = true;
            }
            else
            {
                cmbEstados.IsEnabled = false;
            }

            btnAddBusiness.Click += btnAddBusiness_Click;
            btnAddOcupation.Click += btnAddOcupation_Click;
            btnAddBusinessGarantes.Click += btnAddBusinessGarantes_Click;
            btnAddOcupationGarantes.Click += btnAddOcupationGarantes_Click;

            //Vehiculos
            btnAddColor.Click += btnAddColor_Click;
            btnAddTypeVehicle.Click += btnAddTypeVehicle_Click;
            btnAddModel.Click += btnAddModel_Click;


            chkDocument.Checked += chkDocument_Checked;
            chkDocument.Unchecked += chkDocument_Unchecked;


            //Comercios
            txtRnc.LostFocus += txtRnc_LostFocus;
            txtRnc.TextChanged += txtRnc_TextChanged;
            txtRnc.KeyDown += txtRnc_KeyDown;
            txtTelefonoComercio.LostFocus += txtTelefonoComercio_LostFocus;
            txtTelefonoComercio.KeyDown += txtTelefonoComercio_KeyDown;
        }

     

        private void txtTelefonoComercio_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTelefonoComercio_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtTelefonoComercio.Text;
                txtTelefonoComercio.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void txtRnc_KeyDown(object sender, KeyEventArgs e)
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


        private void txtRnc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtRnc.Text))
            {
                clsValidacionesBO.Limpiar(gridDatosGarantiaComercial);
                gridDatosGarantiaComercial.DataContext = new clsComerciosBE();
            }
        }

        private void txtRnc_LostFocus(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (!string.IsNullOrEmpty(txtRnc.Text))
                {
                    var response = db.ComerciosGetByRnc(txtRnc.Text);
                    if (!string.IsNullOrEmpty(response.Rnc))
                    {
                        _Rnc = response.Rnc;
                        txtRnc.Text = response.Rnc;
                        txtComercio.Text = response.Comercio;
                        txtDireccion.Text = response.Direccion;
                        txtTelefono.Text = response.Telefono;
                    }
                    else
                    {
                        _Rnc = string.Empty;
                        gridDatosGarantiaComercial.DataContext = new clsComerciosBE();
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgRncNotFound"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
            }
            catch { }
        }

        private void chkDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = (CheckBox)sender;
                clsCookiesBO.PrintApplicacion((bool)obj.IsChecked);
            }
            catch { }
        }

        private void chkDocument_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = (CheckBox)sender;
                clsCookiesBO.PrintApplicacion((bool)obj.IsChecked);

            }
            catch { }
        }

        private void txtCvv_KeyDown(object sender, KeyEventArgs e)
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

        private void txtNumero_KeyDown(object sender, KeyEventArgs e)
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

        private void dtgContactoEmergencias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _referencia = (clsReferenciasBE)dtgContactoEmergencias.SelectedItem;
            }
            catch { }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_referencia != null)
                {
                    foreach (var item in _Referencias)
                    {
                        if (item.ReferenciaID == _referencia.ReferenciaID)
                        {
                            _Referencias.Remove(item);
                            dtgContactoEmergencias.ItemsSource = new List<clsReferenciasBE>();
                            dtgContactoEmergencias.ItemsSource = _Referencias;
                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }
        private void BtnDeleteDependientes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dependiente != null)
                {
                    foreach (var item in _Dependientes)
                    {
                        if (item.DependienteID == _dependiente.DependienteID)
                        {
                            _Dependientes.Remove(item);
                            dtgDependientes.ItemsSource = new List<clsDependientesBE>();
                            dtgDependientes.ItemsSource = _Dependientes;
                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }
        private void dtgDependientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _dependiente = (clsDependientesBE)dtgDependientes.SelectedItem;
            }
            catch { }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_referencia != null)
                {
                    frmReferencias Contactos = new frmReferencias();
                    Contactos.Referencias = _Referencias;
                    Contactos.OnInit(_referencia, 2);
                    Contactos.Closed += (obj, arg) =>
                    {
                        dtgContactoEmergencias.ItemsSource = new List<clsReferenciasBE>();
                        dtgContactoEmergencias.ItemsSource = Contactos.Referencias;
                        _Referencias = Contactos.Referencias;
                    };
                    Contactos.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }
        private void btnEditDependientes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dependiente != null)
                {
                    frmDependientes Contactos = new frmDependientes();
                    Contactos.Dependientes = _Dependientes;
                    Contactos._VISTA = 2;
                    Contactos.OnInit(_dependiente, 2);
                    Contactos.Closed += (obj, arg) =>
                    {
                        dtgDependientes.ItemsSource = new List<clsDependientesBE>();
                        dtgDependientes.ItemsSource = Contactos.Dependientes;
                        _Dependientes = Contactos.Dependientes;
                    };
                    Contactos.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }
        private void btnAddDependientes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmDependientes Contactos = new frmDependientes();
                Contactos.Dependientes = _Dependientes;
                Contactos.Closed += (obj, arg) => { dtgDependientes.ItemsSource = new List<clsDependientesBE>(); dtgDependientes.ItemsSource = Contactos.Dependientes; _Dependientes = Contactos.Dependientes; };
                Contactos.ShowDialog();

            }
            catch (Exception ex) { clsMessage.ErrorMessage(ex.Message, "-"); }
        }

        private void cmbCondicionSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtTiempo.Text = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Tiempo.ToString();
            }
            catch { }
        }

        private void btnAddOcupation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmOcupaciones f = new frmOcupaciones();
                f.Owner = this;
                f.Closed += (obj, arg) => { OcupacionesGet(); };
                f.ShowDialog();
            }
            catch { }
        }

        private void btnAddOcupationGarantes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmOcupaciones f = new frmOcupaciones();
                f.Owner = this;
                f.Closed += (obj, arg) => { OcupacionesGarantesGet(); };
                f.ShowDialog();
            }
            catch { }
        }



        private void btnAddModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmModelos ruta = new frmModelos();
                ruta.Owner = this;
                ruta.Closed += (obj, arg) => { ModelosGet(); };
                ruta.ShowDialog();
            }
            catch { }
        }

        private void btnAddTypeVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmTipoVehiculos ruta = new frmTipoVehiculos();
                ruta.Owner = this;
                ruta.Closed += (obj, arg) => { TipoVehiculosGet(); };
                ruta.ShowDialog();
            }
            catch { }
        }

        private void btnAddColor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmColores ruta = new frmColores();
                ruta.Owner = this;
                ruta.Closed += (obj, arg) => { ColoresGet(); };
                ruta.ShowDialog();
            }
            catch { }
        }


        private void OcupacionesGarantesGet()
        {
            try
            {
                List<clsOcupacionesBE> Ocupaciones = new List<clsOcupacionesBE>();
                Ocupaciones = db.OcupacionesGet(null).ToList();
                Ocupaciones.Add(new clsOcupacionesBE { OcupacionID = -1, Ocupacion = clsLenguajeBO.Find("itemSelect") });

                cmbOcupacionesGarantes.ItemsSource = Ocupaciones;
                cmbOcupacionesGarantes.SelectedValuePath = "OcupacionID";
                cmbOcupacionesGarantes.DisplayMemberPath = "Ocupacion";
                cmbOcupacionesGarantes.SelectedValue = 1;//Ocupaciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
            }
            catch { }
        }

        private void OcupacionesGet()
        {
            try
            {
                List<clsOcupacionesBE> Ocupaciones = new List<clsOcupacionesBE>();
                Ocupaciones = db.OcupacionesGet(null).ToList();
                Ocupaciones.Add(new clsOcupacionesBE { OcupacionID = -1, Ocupacion = clsLenguajeBO.Find("itemSelect") });
                cmbOcupaciones.ItemsSource = Ocupaciones;
                cmbOcupaciones.SelectedValuePath = "OcupacionID";
                cmbOcupaciones.DisplayMemberPath = "Ocupacion";
                cmbOcupaciones.SelectedValue = 1;//Ocupaciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            }
            catch { }
        }

        private void ModelosGet()
        {
            try
            {
                //Modelos
                List<clsModelosBE> Modelos = new List<clsModelosBE>();
                Modelos = db.ModelosGet(null).ToList();
                List<clsModelosBE> list = new List<clsModelosBE>();
                foreach (var row in Modelos)
                {
                    list.Add(new clsModelosBE { ModeloID = row.ModeloID, Modelo = row.Marcas.Marca + " / " + row.Modelo });
                }
                list.Add(new clsModelosBE { ModeloID = -1, Modelo = clsLenguajeBO.Find("itemSelect") });
                cmbModelos.ItemsSource = list.OrderBy(x => x.Modelo);
                cmbModelos.SelectedValuePath = "ModeloID";
                cmbModelos.DisplayMemberPath = "Modelo";
                cmbModelos.SelectedValue = -1;
            }
            catch { }
        }

        private void TipoVehiculosGet()
        {
            try
            {
                //TipoVehiculos
                List<clsTipoVehiculosBE> TipoVehiculos = new List<clsTipoVehiculosBE>();
                TipoVehiculos = db.TipoVehiculosGet(null).ToList();
                TipoVehiculos.Add(new clsTipoVehiculosBE { TipoVehiculoID = -1, TipoVehiculo = clsLenguajeBO.Find("itemSelect") });
                cmbTipoVehiculos.ItemsSource = TipoVehiculos;
                cmbTipoVehiculos.SelectedValuePath = "TipoVehiculoID";
                cmbTipoVehiculos.DisplayMemberPath = "TipoVehiculo";
                cmbTipoVehiculos.SelectedValue = -1;
            }
            catch { }
        }

        private void ColoresGet()
        {
            try
            {
                //Colores
                List<clsColoresBE> Colores = new List<clsColoresBE>();
                Colores = db.ColoresGet(null).ToList();
                Colores.Add(new clsColoresBE { ColorID = -1, Color = clsLenguajeBO.Find("itemSelect") });
                cmbColores.ItemsSource = Colores;
                cmbColores.SelectedValuePath = "ColorID";
                cmbColores.DisplayMemberPath = "Color";
                cmbColores.SelectedValue = -1;
            }
            catch { }
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
                    clsValidacionesBO.Limpiar(gridDatosPersonales);
                    clsValidacionesBO.Limpiar(gridDatosEconomicos);

                    gridDatosPersonales.DataContext = new clsPersonasBE();
                    gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                    _Referencias = new List<clsReferenciasBE>();

                    dtgContactoEmergencias.ItemsSource = _Referencias;

                    photo = string.Empty;
                    
                    txtFechaNacimiento.SelectedDate = DateTime.Now;
                    txtFechaIngreso.SelectedDate = DateTime.Now;
                    cmbInstituciones.SelectedValue = 100001;
                    txtDireccionTrabajo.Text = "-";
                    txtTelefonoTrabajo.Text = "-";

                    cmbCiudades.SelectedValue = -1;

                    cmbSexos.SelectedValue = -1;
                    cmbEstadosCiviles.SelectedValue = -1;
                    cmbOperadores.SelectedValue = -1;


                    cmbInstituciones.SelectedValue = 100001;
                    cmbOcupaciones.SelectedValue = 1;
                    cmbHorarios.SelectedValue = 1;
                    cmbIngresos.SelectedValue = 1;

                    cmbSucursales.SelectedValue = 1;

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

        private void btnAddBusinessGarantes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmInstituciones f = new frmInstituciones();
                f.Owner = this;
                f.Closed += (obj, arg) => { InstitucionesGarantesGet(); };
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

        private void btnAdd_srcCameraGarante_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmCamera camera = new frmCamera();
                camera.Owner = this;
                camera.photo = "garante.jpg";
                camera.Closed += (obj, arg) =>
                {
                    if (camera.TakePhoto)
                    {
                        string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), camera.photo);
                        srcPhotoGarante.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));
                        photoGarante = filename;
                    }
                };
                camera.ShowDialog();
            }
            catch { }
        }

        private void btnAdd_srcCamera_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmCamera camera = new frmCamera();
                camera.Owner = this;
                photo = "cliente.jpg";
                camera.Closed += (obj, arg) => 
                {
                    if(camera.TakePhoto)
                    {                        
                        string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), camera.photo);
                        srcPhoto.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));
                        photo = filename;
                    }                
                };
                camera.ShowDialog();
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
                    photo = openFileDialog.FileName;
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


                            if (Persona.DatosEconomicos.Trabaja == true)
                            {
                                rbGaranteSi.IsChecked = true;
                            }
                            else
                            {
                                rbGaranteNo.IsChecked = true;
                            }

                            gridDatosEconomicosGarantes.DataContext = Persona.DatosEconomicos;
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

        private void cmbTipoSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TipoSolicitudChange();
        }

        private void TipoSolicitudChange()
        {
            try
            {
                switch ((int)cmbTipoSolicitudes.SelectedValue)
                {
                    case 1:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonalHost.Visibility = Visibility.Visible;
                            gridDatosGarantiaTarjetas.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaComercial.Visibility = Visibility.Collapsed;
                            wpGarantias.Visibility = Visibility.Visible;
                        }
                        break;
                    case 2:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Visible;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonalHost.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaTarjetas.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaComercial.Visibility = Visibility.Collapsed;
                            wpGarantias.Visibility = Visibility.Visible;
                        }
                        break;
                    case 3:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Visible;
                            gridDatosGarantiaPersonalHost.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaTarjetas.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaComercial.Visibility = Visibility.Collapsed;
                            wpGarantias.Visibility = Visibility.Visible;
                        }
                        break;
                    case 4:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonalHost.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaTarjetas.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaComercial.Visibility = Visibility.Visible;
                            wpGarantias.Visibility = Visibility.Visible;
                        }
                        break;
                    case 6:
                        {
                            if (db.PasarelasGet(null).Count > 0)
                            {
                                gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                                gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                                gridDatosGarantiaPersonalHost.Visibility = Visibility.Collapsed;
                                gridDatosGarantiaTarjetas.Visibility = Visibility.Visible;
                                gridDatosGarantiaComercial.Visibility = Visibility.Collapsed;
                                wpGarantias.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                
                                if ((int)cmbTipoSolicitudes.SelectedValue == 6)
                                {
                                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPaymentGatewayRequired"), clsLenguajeBO.Find("msgTitle"));
                                    cmbTipoSolicitudes.SelectedValue = _TipoSolicitudID;
                                    break;
                                }
                                goto default;

                            }
                        }
                        break;

                    default:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonalHost.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaTarjetas.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaComercial.Visibility = Visibility.Collapsed;
                            wpGarantias.Visibility = Visibility.Collapsed;
                        }
                        break;
                }
            }
            catch { }
        }

        public void OnInit(int SolicitudID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                clsSolicitudesBE row = db.SolicitudesGetBySolicitudID(SolicitudID);
                _SolicitudID = SolicitudID;
                _Documento = row.Clientes.Documento;
                _ClienteID = row.Clientes.ClienteID;

               //Solicitud
                gridSolicitudes.DataContext = row;
                gridDatosPersonales.DataContext = row.Clientes.Personas;
                cmbSucursales.SelectedValue = row.Clientes.SucursalID;

                if (row.Clientes.Personas.DatosEconomicos.Trabaja == true)
                {
                    rbSi.IsChecked = true;
                }
                else
                {
                    rbNo.IsChecked = true;
                }

                gridDatosEconomicos.DataContext = row.Clientes.Personas.DatosEconomicos;
                dtgContactoEmergencias.ItemsSource = row.Clientes.Personas.Referencias;
                _Referencias = row.Clientes.Personas.Referencias.ToList();

                dtgDependientes.ItemsSource = row.Clientes.Personas.Dependientes;
                _Dependientes = row.Clientes.Personas.Dependientes.ToList();

                switch (row.TipoSolicitudID)
                {
                    case 1:
                        {
                            _DocumentoGarante = row.GarantiaPersonal.Documento;
                            gridDatosGarantiaPersonal.DataContext = row.GarantiaPersonal.Garantes.Personas;

                            gridDatosEconomicosGarantes.DataContext = row.GarantiaPersonal.Garantes.Personas.DatosEconomicos;
                            if (row.GarantiaPersonal.Garantes.Personas.DatosEconomicos.Trabaja == true)
                            {
                                rbGaranteSi.IsChecked = true;
                            }
                            else
                            {
                                rbGaranteNo.IsChecked = true;
                            }
                        } break;
                    case 2:
                        {
                            gridDatosGarantiaHipotecario.DataContext = row.GarantiaHipotecaria;
                            txtDescripcion.Text = row.GarantiaHipotecaria.Descripcion;
                            txtMontoHipoteca.Text = string.Format("{0:N2}", row.GarantiaHipotecaria.Monto);
                        }
                        break;
                    case 3:
                        {
                            gridDatosGarantiaVehiculo.DataContext = row.GarantiaVehiculos;
                        } break;
                    case 4:
                        {
                            _Rnc = row.GarantiaComerciales.Rnc;
                            gridDatosGarantiaComercial.DataContext = row.GarantiaComerciales.Comercios;
                        }
                        break;
                    case 6:
                        {
                            //clsGarantiaTarjetasBE tc = new clsGarantiaTarjetasBE
                            //{
                            //    SolicitudID = row.GarantiaTarjetas.SolicitudID,
                            //    Fecha = row.GarantiaTarjetas.Fecha,
                            //    Holder = row.GarantiaTarjetas.Holder,
                            //    Numero = Common.Generic.Decrypt(row.GarantiaTarjetas.Numero),
                            //    Ano = row.GarantiaTarjetas.Ano,
                            //    Mes = row.GarantiaTarjetas.Mes,
                            //    Cvv = row.GarantiaTarjetas.Cvv,
                            //    Usuario = row.GarantiaTarjetas.Usuario,
                            //    ModificadoPor = row.GarantiaTarjetas.ModificadoPor,
                            //    FechaModificacion = row.GarantiaTarjetas.FechaModificacion
                            //};
                            gridTarjetas.DataContext = row.GarantiaTarjetas;
                            gridDatosGarantiaTarjetas.DataContext = row.GarantiaTarjetas;
                        }
                        break;
                }
            }
            catch(Exception ex) 
            { }
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

        private void PersonasCreate()
        {
            try
            {
               OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, photo, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
               if(result.ResponseCode == "00")
                {
                    ClientesCreate();

                    if ((gridDatosPersonales.DataContext as clsPersonasBE).Fotografias != null)
                    {
                        byte[] data = (gridDatosPersonales.DataContext as clsPersonasBE).Fotografias.Foto;
                        if (data.Length > 0)
                        {
                            SavePhoto(txtDocumento.Text, data);
                        }
                    }
                }
               else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }


            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
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

        private void PersonasUpdate()
        {
            try
            {
                OperationResult result = db.PersonasUpdate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, photo, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    if ((gridDatosPersonales.DataContext as clsPersonasBE).Fotografias != null)
                    {
                        byte[] data = (gridDatosPersonales.DataContext as clsPersonasBE).Fotografias.Foto;
                        if (data.Length > 0)
                        {
                            SavePhoto(txtDocumento.Text, data);
                        }
                    }

                    if (VISTA == 1)
                    {
                        ClientesCreate();
                    }
                    else
                    {
                        ClientesUpdate();
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
                OperationResult result = db.ClientesCreate(txtDocumento.Text, (int)cmbSucursales.SelectedValue, 0, 0, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    _ClienteID = Convert.ToInt32(result.ResponseMessage);

                    if (clsValidacionesBO.Validar(gridDatosEconomicos))
                    {
                        if (VISTA == 1)
                        {
                            DatosEconomicosCreate();
                        }
                        else
                        {
                            DatosEconomicosUpdate();
                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void ClientesUpdate()
        {
            try
            {
                OperationResult result = db.ClientesUpdate(_ClienteID,txtDocumento.Text, (int)cmbSucursales.SelectedValue, 0, 0, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    if (clsValidacionesBO.Validar(gridDatosEconomicos))
                    {
                        if (VISTA == 1)
                        {
                            DatosEconomicosCreate();
                        }
                        else
                        {
                            DatosEconomicosUpdate();
                        }
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
                if (clsValidacionesBO.Validar(gridDatosEconomicos))
                {

                    OperationResult result = db.DatosEconomicosCreate(txtDocumento.Text, trabaja, (int)cmbInstituciones.SelectedValue, (int)cmbOcupaciones.SelectedValue, (int)cmbHorarios.SelectedValue, (int)cmbIngresos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                    if (result.ResponseCode == "00")
                    {

                        if (_Dependientes != null)
                        {
                            DependientesCreate();
                        }

                        if (_Referencias != null)
                        {
                            ReferenciasCreate();
                        }
                        else
                        {
                            if (clsValidacionesBO.Validar(gridSolicitudes))
                            {
                                if (float.Parse(txtMonto.Text) <= 0)
                                {
                                    txtMonto.Focus();
                                    return;
                                }

                                if (float.Parse(txtTiempo.Text) <= 0)
                                {
                                    txtTiempo.Focus();
                                    return;
                                }

                                SolicitudesCreate();
                            }
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                    }
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
                    if (_Dependientes != null)
                    {
                        DependientesCreate();
                    }

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

        private async void SolicitudesCreate()
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarSolicitud == true && (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).SendSMS == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        string Code = Common.Generic.GenerarCodigo(4);
                        string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarCelular), Common.Generic.ShortName(txtNombres.Text), clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtMonto.Text, Code);

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                        {
                            string[] userID = new string[1];
                            userID[0] = txtDocumento.Text;
                            var response = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                            if (response.ResponseCode == "00")
                            {
                                frmPin Pin = new frmPin();
                                Pin.OnInit(Code);
                                Pin.Closed += (a, b) =>
                                {
                                    if (Pin.Confirmado == true)
                                    {
                                        SaveSolicitudesCreate();
                                        Pin.Close();
                                    }
                                };
                                Pin.ShowDialog();
                            }
                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                        {

                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                        {
                            if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                            {
                                var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, Mensaje, (int)cmbOperadores.SelectedValue);
                                if (smsResult.ResponseCode == "00")
                                {
                                    frmPin Pin = new frmPin();
                                    Pin.OnInit(Code);
                                    Pin.Closed += (a, b) =>
                                    {
                                        if (Pin.Confirmado == true)
                                        {
                                            SaveSolicitudesCreate();
                                            Pin.Close();
                                        }
                                    };
                                    Pin.ShowDialog();
                                }
                                else
                                {
                                    clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                }
                            }
                            else 
                            {
                                if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
                            }
                        }
                    }
                    else
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit("La red de mensajería no esta disponible en estos momentos. ¿Deseas continuar y almacenar los datos de la solicitud ?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += (arg, obj) => { if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); } };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                        msgBox.ShowDialog();
                    }
                }
                else
                {
                    if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle"));  }
        }

        private void GarantiasCreate(int ID)
        {
            try
            {
                switch((int)cmbTipoSolicitudes.SelectedValue)
                {
                    case 1: //Personales
                        {
                            GarantesCreate();
                            db.GarantiaPersonalCreate(ID, _DocumentoGarante,clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    case 2: //Hipotecarios
                        {
                            db.GarantiaHipotecariaCreate(ID, txtDescripcion.Text, float.Parse(txtMontoHipoteca.Text), clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    case 3: //Vehiculos
                        {
                            db.GarantiaVehiculosCreate(ID, (int)cmbTipoVehiculos.SelectedValue, (int)cmbModelos.SelectedValue, (int)cmbColores.SelectedValue, txtChassis.Text, txtPlaca.Text, txtRegistro.Text, (DateTime)txtFechaExpedicion.SelectedDate, int.Parse(txtAno.Text), clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;

                    case 4: //Comercios
                        {
                            db.GarantiaComercialesCreate(ID, _Rnc, txtComercio.Text, txtDireccionComercio.Text, txtTelefonoComercio.Text, clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;

                    case 6: //Tarjetas
                        {
                            db.GarantiaTarjetasCreate(ID, txtHolder.Text, txtNumero.Text, (int)cmbExpirationMonth.SelectedValue, (int)cmbExpirationYear.SelectedValue, int.Parse(txtCvv.Text), clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    default: //Otros
                        {
                            //GarantesCreate();
                            //db.GarantiaPersonalCreate(ID, _DocumentoGarante, clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void GarantesCreate()
        {
            try
            {
                OperationResult result = db.PersonasCreate(1, txtDocumentoGarante.Text, (DateTime)txtFechaNacimientoGarante.SelectedDate, txtNombresGarante.Text, txtApellidosGarante.Text, txtApodoGarante.Text, photoGarante, (int)cmbCiudadesGarante.SelectedValue, txtDireccionGarante.Text, txtCorreoElectronicoGarante.Text, txtTelefonoGarante.Text, (int)cmbOperadoresGarante.SelectedValue, txtCelularGarante.Text, (int)cmbSexosGarante.SelectedValue, (int)cmbEstadosCivilesGarante.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    db.GarantesCreate(txtDocumentoGarante.Text, clsVariablesBO.UsuariosBE.Documento);
                    if ((srcPhotoGarante.DataContext as clsPersonasBE).Fotografias != null)
                    {
                        byte[] data = (srcPhotoGarante.DataContext as clsPersonasBE).Fotografias.Foto;
                        SavePhoto(txtDocumentoGarante.Text, data);
                    }

                    bool trabaja = ((bool)rbGaranteSi.IsChecked == true ? true : false);
                    db.DatosEconomicosCreate(txtDocumentoGarante.Text, trabaja, (int)cmbInstitucionesGarantes.SelectedValue, (int)cmbOcupacionesGarantes.SelectedValue, (int)cmbHorariosGarantes.SelectedValue, (int)cmbIngresosGarantes.SelectedValue, clsVariablesBO.UsuariosBE.Documento);

                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

       private void gotoPrint(int ID)
        {
            try
            {

                if ((bool)chkDocument.IsChecked)
                {
                    frmPrintSolicitudesGetBySolicitudID Print = new frmPrintSolicitudesGetBySolicitudID();
                    Print.OnInit(ID);
                    Print.ShowDialog();

                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                    {
                        frmPrintReglasSolicitudesGetBySolicitudID reglas = new frmPrintReglasSolicitudesGetBySolicitudID();
                        reglas.OnInit(ID);
                        reglas.ShowDialog();
                    }
                }
                else
                {
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }


        private void SaveSolicitudesCreate()
        {
            try
            {

                OperationResult result = db.SolicitudesCreate(_ClienteID, (int)cmbTipoSolicitudes.SelectedValue, (int)cmbCondicionSolicitudes.SelectedValue, Convert.ToInt16(txtTiempo.Text), float.Parse(txtMonto.Text), (int)cmbEstados.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    GarantiasCreate(int.Parse(result.ResponseMessage));
                    gotoPrint(int.Parse(result.ResponseMessage));
                    //Aqui va el mensaje final              
                    //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSolicitudSuccess"), clsLenguajeBO.Find("msgTitle"));
                    
                    ClearAll();
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                
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
                clsValidacionesBO.Limpiar(gridTarjetas);
                clsValidacionesBO.Limpiar(gridDatosGarantiaComercial);

                gridSolicitudes.DataContext = new clsSolicitudesBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosGarantiaHipotecario.DataContext = new clsGarantiaHipotecariaBE();
                gridDatosGarantiaVehiculo.DataContext = new clsGarantiaVehiculosBE();
                gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                gridTarjetas.DataContext = new clsGarantiaTarjetasBE();
                gridDatosGarantiaComercial.DataContext = new clsComerciosBE();

                _Referencias = new List<clsReferenciasBE>();
                dtgContactoEmergencias.ItemsSource = _Referencias;

                _Dependientes = new List<clsDependientesBE>();
                dtgDependientes.ItemsSource = _Dependientes;

                photo = string.Empty;
                photoGarante = string.Empty;
                _SolicitudID = 0;
                _OcupacionID = -1;
                _HorarioID = -1;
                _IngresoID = -1;
                _SolicitudID = 0;
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                gridDatosEconomicosGarantes.DataContext = new clsDatosEconomicosBE { Fecha = DateTime.Now};
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

                cmbEstados.SelectedValue = _EstadoID;
                cmbTipoSolicitudes.SelectedValue = _TipoSolicitudID;
                cmbCondicionSolicitudes.SelectedValue = _CondicionID;

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

                cmbInstituciones.SelectedValue = 100001;
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                cmbTipoVehiculos.SelectedValue = -1;
                cmbTipoVehiculos.SelectedValue = -1;
                cmbColores.SelectedValue = -1;

                txtTiempo.Text = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Tiempo.ToString();
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;

                cmbExpirationMonth.SelectedValue = DateTime.Now.Month;
                cmbExpirationYear.SelectedValue = DateTime.Now.Year;

                _Rnc = string.Empty;
            }
            catch { }
        }

        private async void SolicitudesUpdate()
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarSolicitud == true && (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).SendSMS == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        string Code = Common.Generic.GenerarCodigo(4);
                        string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarSolicitud), Common.Generic.ShortName(txtNombres.Text), clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtMonto.Text, Code);

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                        {
                            string[] userID = new string[1];
                            userID[0] = txtDocumento.Text;
                            var response = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                            if (response.ResponseCode == "00")
                            {
                                frmPin Pin = new frmPin();
                                Pin.OnInit(Code);
                                Pin.Closed += (a, b) =>
                                {
                                    if (Pin.Confirmado == true)
                                    {
                                        SaveSolicitudesCreate();
                                        Pin.Close();
                                    }
                                };
                                Pin.ShowDialog();
                            }
                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                        {

                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                        {
                            if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                            {
                                var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsUrl, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, Mensaje, (int)cmbOperadores.SelectedValue);
                                if (smsResult.ResponseCode == "00")
                                {
                                    frmPin Pin = new frmPin();
                                    Pin.OnInit(Code);
                                    Pin.Closed += (a, b) =>
                                    {
                                        if (Pin.Confirmado == true)
                                        {
                                            SaveSolicitudesUpdate();
                                            Pin.Close();
                                        }

                                    };
                                    Pin.ShowDialog();
                                }
                                else
                                {
                                    clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                }
                            }
                            else
                            {
                                if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
                            }
                        }
                    }
                    else
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit("La red de mensajería no esta disponible en estos momentos. ¿Deseas continuar y almacenar los datos de la solicitud ?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += (arg, obj) => { if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); } };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close();  };
                        msgBox.ShowDialog();
                    }
                }
                else
                {
                    if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle"));  }
        }

        private void SaveSolicitudesUpdate()
        {
            try
            {
                OperationResult result = db.SolicitudesUpdate(_SolicitudID, _ClienteID, (int)cmbTipoSolicitudes.SelectedValue, (int)cmbCondicionSolicitudes.SelectedValue, Convert.ToInt16(txtTiempo.Text), decimal.Parse(txtMonto.Text), (int)cmbEstados.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    GarantiasCreate(_SolicitudID);
                    gotoPrint(_SolicitudID);
                    //Aqui va el mensaje final
                    //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSolicitudSuccess"), clsLenguajeBO.Find("msgTitle"));
                    ClearAll();
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }


        private void DependientesCreate()
        {
            try
            {
                var r = db.DependientesDeleteGetByDocumento(txtDocumento.Text);
                if (r.ResponseCode == "00")
                {
                    foreach (var row in _Dependientes)
                    {
                        db.DependientesCreate(row.ParentescoID, txtDocumento.Text, row.FechaNacimiento, row.Dependiente, row.Telefono, clsVariablesBO.UsuariosBE.Documento);
                    }
                }
            }
            catch { }
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

                    if (clsValidacionesBO.Validar(gridSolicitudes))
                    {
                        if (float.Parse(txtMonto.Text) <= 0)
                        {
                            txtMonto.Focus();
                            return;
                        }

                        if (float.Parse(txtTiempo.Text) <= 0)
                        {
                            txtTiempo.Focus();
                            return;
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

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                                                                                                                                                               // color = (n.Difference1 < 0) ? Negativo : (n.Difference1 > 0) ? Positivo : Balanceado,                                                            
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true &&
                   ( (int)cmbTipoSolicitudes.SelectedValue == 1 ? clsValidacionesBO.Validar(gridDatosGarantiaPersonal): (int)cmbTipoSolicitudes.SelectedValue == 2 ? clsValidacionesBO.Validar(gridDatosGarantiaHipotecario) : (int)cmbTipoSolicitudes.SelectedValue == 3 ? clsValidacionesBO.Validar(gridDatosGarantiaVehiculo) == true : (int)cmbTipoSolicitudes.SelectedValue == 6 ? Validartarjetas() == true :

                   (int)cmbTipoSolicitudes.SelectedValue == 4 ? clsValidacionesBO.Validar(gridDatosGarantiaComercial) :

                   true) == true)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {       
                        if((int)cmbTipoSolicitudes.SelectedValue == 6)
                        {
                            bool result =  ComprobarTarjeta();
                            if (!result)
                            {
                                return;
                            }
                        }
                        
                        if (_Documento == string.Empty)
                        {
                            if (clsValidacionesBO.Validar(gridDatosPersonales))
                            {
                                PersonasCreate();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_Documento))
                            {
                                if (clsValidacionesBO.Validar(gridDatosPersonales))
                                {
                                    PersonasUpdate();
                                }
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true &&
                       ((int)cmbTipoSolicitudes.SelectedValue == 1 ? clsValidacionesBO.Validar(gridDatosGarantiaPersonal) : (int)cmbTipoSolicitudes.SelectedValue == 2 ? clsValidacionesBO.Validar(gridDatosGarantiaHipotecario) : (int)cmbTipoSolicitudes.SelectedValue == 3 ? clsValidacionesBO.Validar(gridDatosGarantiaVehiculo) == true : (int)cmbTipoSolicitudes.SelectedValue == 6 ? Validartarjetas() == true : true) == true)
                    {
                        if (clsVariablesBO.Permiso.Modificar == true || PermissionAccess)
                        {
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

        private bool ComprobarTarjeta()
        {
            try
            {
                Root root = new Root();
                Random rnd = new Random();
                string monto = rnd.Next(1, 5).ToString();

                root.clientReferenceInformation = new ClientReferenceInformation {code = $"{Guid.NewGuid()}"};
                root.orderInformation = new OrderInformation { amountDetails = new AmountDetails { currency = clsVariablesBO.UsuariosBE.Sucursales.Clientes.FirstOrDefault().Personas.Ciudades.Provincias.Paises.Monedas.Codigo, totalAmount = monto }, billTo = new BillTo { country = "DO", email = string.IsNullOrEmpty(txtCorreoElectronico.Text)? "notiene@gmail.com" : txtCorreoElectronico.Text, address1 = string.IsNullOrEmpty(txtDireccion.Text)? cmbCiudades.Text : txtDireccion.Text, administrativeArea = clsVariablesBO.UsuariosBE.Sucursales.Clientes.FirstOrDefault().Personas.Ciudades.Provincias.Provincia, firstName = txtNombres.Text, lastName = txtApellidos.Text, phoneNumber = txtCelular.Text, locality = clsVariablesBO.UsuariosBE.Sucursales.Clientes.FirstOrDefault().Personas.Ciudades.Ciudad, postalCode = "43002" } };
                root.paymentInformation = new PaymentInformation { card = new Card { expirationMonth = ((int)cmbExpirationMonth.SelectedValue).ToString(), expirationYear = ((int)cmbExpirationYear.SelectedValue).ToString(), number = txtNumero.Text, securityCode = txtCvv.Text } };
                root.processingInformation = new ProcessingInformation { commerceIndicator = "internet" };
                string JsonObj = JsonConvert.SerializeObject(root);

                var result = db.ProcesarPagosGetByMerchantID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, JsonObj, Common.FingerPrint.GetKey());
                if (result.status == "AUTHORIZED")
                {
                    return true;
                }
                else
                {
                    clsMessage.ErrorMessage(result.errorInformation.message, clsLenguajeBO.Find("msgTitle"));
                    return false;
                }
            }
            catch(Exception ex) {
                clsMessage.ErrorMessage(clsLenguajeBO.Find("msgErroGateway"), clsLenguajeBO.Find("msgTitle"));
                return false; }
        }

        private bool Validartarjetas()
        {
            bool result = clsValidacionesBO.Validar(gridTarjetas);
            if(result)
            {

                var fvence = DateTime.Now.AddMonths(int.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses);
            
                if(DateTime.Now.Year >= (int)cmbExpirationYear.SelectedValue && DateTime.Now.Month >= (int)cmbExpirationMonth.SelectedValue)
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgExpiredCard"), clsLenguajeBO.Find("msgTitle"));
                    return false;
                }

                if (fvence.Year > (int)cmbExpirationYear.SelectedValue)
                {
                    frmMessageBox msgBox = new frmMessageBox();
                    msgBox.OnInit(clsLenguajeBO.Find("msgOverPassDateCreditCard"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                    msgBox.btnAceptar.Click += (arg, obj) => { result =  true; msgBox.Close(); };
                    msgBox.btnSalir.Click += (arg, obj) => { result = false; msgBox.Close(); };
                    msgBox.ShowDialog();

                    //return result;
                }

                if (fvence.Year == (int)cmbExpirationYear.SelectedValue)
                {
                    if (fvence.Month >= (int)cmbExpirationMonth.SelectedValue)
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit(clsLenguajeBO.Find("msgOverPassDateCreditCard"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += (arg, obj) => { result = true; msgBox.Close(); };
                        msgBox.btnSalir.Click += (arg, obj) => { result = false; msgBox.Close(); };
                        msgBox.ShowDialog();
                        //return result;
                    }
                    else
                    {
                        if (fvence.Month < (int)cmbExpirationMonth.SelectedValue)
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgErroGateway"), clsLenguajeBO.Find("msgTitle"));
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
            }

           return result;
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
                { e.Handled = false; }
                else
                { e.Handled = true; }

                if (e.Key == Key.Enter && string.IsNullOrEmpty(txtDocumento.Text))
                {
                    frmConsultasPersonas personas = new frmConsultasPersonas();
                    personas.Owner = this;
                    personas.IsQuery = true;
                    personas.Closed += (obj, arg) => {
                       

                        String Documento = personas.BE.Documento;
                        txtDocumento.Text = clsValidacionesBO.DocumentFormat(Documento);

                        if (Common.Generic.ValidarDocumento(txtDocumento.Text) == true)
                        {
                            if (!String.IsNullOrEmpty(txtDocumento.Text))
                            {
                                var Persona = db.PersonasGetByDocumento(txtDocumento.Text, clsVariablesBO.IsRemoteQuery, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                                if (Persona != null)
                                {
                                    if (ValidarListaNegra(Persona.Documento) == false)
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

                                        txtHolder.Text = $"{Common.Generic.ShortName(Persona.Nombres.ToUpper())} {Common.Generic.ShortName(Persona.Apellidos.ToUpper())}";
                                        lblHolder.Text = $"{Common.Generic.ShortName(Persona.Nombres.ToUpper())} {Common.Generic.ShortName(Persona.Apellidos.ToUpper())}";
                                    }
                                    else
                                    {
                                        clsMessage.ErrorMessage(string.Format(clsLenguajeBO.Find("msgBlackList"), Persona.Nombres + " " + Persona.Apellidos), clsLenguajeBO.Find("msgTitle"));
                                        _Documento = string.Empty;
                                        Persona = new clsPersonasBE();
                                        gridDatosPersonales.DataContext = Persona;
                                    }
                                    VerificarDeudas();
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

                                    cmbInstituciones.SelectedValue = 100001;
                                    cmbOcupaciones.SelectedValue = 1;
                                    cmbHorarios.SelectedValue = 1;
                                    cmbIngresos.SelectedValue = 1;
                                    cmbTipoVehiculos.SelectedValue = -1;
                                    cmbTipoVehiculos.SelectedValue = -1;
                                    cmbColores.SelectedValue = -1;

                                    cmbSexos.SelectedValue = 1;
                                    cmbEstadosCiviles.SelectedValue = 1;
                                    cmbOperadores.SelectedValue = 1;

                                }
                            }
                        }
                        else
                        {
                            clsMessage.ErrorMessage("Numero de documento incorrecto.", clsLenguajeBO.Find("msgTitle"));
                            txtDocumento.Text = "";
                        }



                    };
                    personas.ShowDialog();
                }
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
                            if (ValidarListaNegra(Persona.Documento)==false)
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

                                if(string.IsNullOrEmpty(Persona.Correo))
                                {
                                    txtCorreoElectronico.Text = "notiene@gmail.com";
                                }

                                txtHolder.Text = $"{Common.Generic.ShortName(Persona.Nombres.ToUpper())} {Common.Generic.ShortName(Persona.Apellidos.ToUpper())}";
                                lblHolder.Text = $"{Common.Generic.ShortName(Persona.Nombres.ToUpper())} {Common.Generic.ShortName(Persona.Apellidos.ToUpper())}";
                            }
                            else
                            {
                                clsMessage.ErrorMessage(string.Format(clsLenguajeBO.Find("msgBlackList"), Persona.Nombres + " " + Persona.Apellidos), clsLenguajeBO.Find("msgTitle"));
                                _Documento = string.Empty;
                                Persona = new clsPersonasBE();
                                gridDatosPersonales.DataContext = Persona;
                            }                            
                            VerificarDeudas();
                        }
                        else
                        {
                            string x = txtDocumento.Text;
                            clsValidacionesBO.Limpiar(gridDatosPersonales);
                            clsValidacionesBO.Limpiar(gridDatosEconomicos);
                            _Referencias = new List<clsReferenciasBE>();
                            dtgContactoEmergencias.ItemsSource = _Referencias;
                            rbSi.IsChecked = true;
                           
                            txtDocumento.Text = x;
                            txtFechaNacimiento.SelectedDate = DateTime.Now;
                            _Documento = string.Empty;

                            cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
                            rbNo.IsChecked = true;
                            cmbInstituciones.SelectedValue = 100001;
                            cmbOcupaciones.SelectedValue = 1;
                            cmbHorarios.SelectedValue = 1;
                            cmbIngresos.SelectedValue = 1;
                            cmbTipoVehiculos.SelectedValue = -1;
                            cmbTipoVehiculos.SelectedValue = -1;
                            cmbColores.SelectedValue = -1;

                            cmbSexos.SelectedValue = 1;
                            cmbEstadosCiviles.SelectedValue = 1;
                            cmbOperadores.SelectedValue = 1;

                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage("Numero de documento incorrecto.", clsLenguajeBO.Find("msgTitle"));
                    txtDocumento.Text = "";
                }
            }
            catch(Exception ex)
            { }
        }

        private bool ValidarListaNegra(string documento)
        {
            try
            {
                db = new Core.Manager();
                var result  = db.ListaNegrasGetByDocumento(documento);
                if (string.IsNullOrEmpty(result.Documento))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch { return false; }
        }

        private void VerificarDeudas()
        {
            try
            {
                if (!clsVariablesBO.IsCheckPrevious)
                {
                    var result = db.ContratosGetByDocumento(txtDocumento.Text).Where(xc => xc.EstadoID == 1).ToList();
                    foreach (clsContratosBE C in result)
                    {
                        clsCuotasView Cuotas = new clsCuotasView();
                        Cuotas.SetDataSource(C.ContratoID, 0);
                        var balance = (Cuotas.BalanceGetByReciboID() < 0 ? 0 : Cuotas.BalanceGetByReciboID());
                        if (balance > 0)
                        {
                            frmAvisoDeudas deudas = new frmAvisoDeudas();
                            deudas.Owner = this;
                            string message = string.Format(clsLenguajeBO.Find("msgBalancePending"), C.Solicitudes.Clientes.Personas.Nombres + " " + C.Solicitudes.Clientes.Personas.Apellidos, C.ContratoID, string.Format("{0:N2}", balance));
                            deudas.OnInit(message);
                            deudas.ShowDialog();
                        }
                    }
                }
                else
                {
                    var result = db.DataCreditosGetByDocumento(txtDocumento.Text, clsVariablesBO.IsRemoteQuery, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                    if (result.Count() != 0)
                    {
                        frmConsultasHistorial ws = new frmConsultasHistorial();
                        ws.Owner = this;
                        ws.OnInit((clsPersonasBE)gridDatosPersonales.DataContext, result);
                        ws.ShowDialog();
                    }
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


        private void rbGaranteNo_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFechaIngresoGarante.IsEnabled = false;
                cmbInstitucionesGarantes.IsEnabled = false;
                txtDireccionTrabajoGarante.IsEnabled = false;
                txtTelefonoTrabajoGarante.IsEnabled = false;
                cmbOcupacionesGarantes.IsEnabled = false;
                cmbHorariosGarantes.IsEnabled = false;
                cmbIngresosGarantes.IsEnabled = false;

                cmbInstitucionesGarantes.SelectedValue = 100001;
                _OcupacionID = (int)cmbOcupaciones.SelectedValue;
                _HorarioID = (int)cmbHorarios.SelectedValue;
                _IngresoID = (int)cmbIngresos.SelectedValue;


                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupacionesGarantes.SelectedValue = 1;
                cmbHorariosGarantes.SelectedValue = 1;
                cmbIngresosGarantes.SelectedValue = 1;
            }
            catch { }
        }

        private void rbGaranteSi_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFechaIngresoGarante.IsEnabled = true;
                cmbInstitucionesGarantes.IsEnabled = true;
                cmbOcupacionesGarantes.IsEnabled = true;
                cmbHorariosGarantes.IsEnabled = true;
                cmbIngresosGarantes.IsEnabled = true;


                cmbOcupacionesGarantes.SelectedValue = _OcupacionID;
                cmbHorariosGarantes.SelectedValue = _HorarioID;
                cmbIngresosGarantes.SelectedValue = _IngresoID;
            }
            catch { }
        }

        private void LoadCombox()
        {

            var meses = db.MonthGet().ToList();
            cmbExpirationMonth.ItemsSource = meses;
            cmbExpirationMonth.SelectedValuePath = "MonthID";
            cmbExpirationMonth.DisplayMemberPath = "Month";
            cmbExpirationMonth.SelectedValue = DateTime.Now.Month;

            var anos = db.YearGet().ToList();
            cmbExpirationYear.ItemsSource = anos;
            cmbExpirationYear.SelectedValuePath = "YearID";
            cmbExpirationYear.DisplayMemberPath = "Year";
            cmbExpirationYear.SelectedValue = DateTime.Now.Year;

            //Tipo de Solicitudes
            List<clsTipoSolicitudesBE> TipoSolicitudes = new List<clsTipoSolicitudesBE>();
            TipoSolicitudes = db.TipoSolicitudesGet(null).ToList();
            TipoSolicitudes.Add(new clsTipoSolicitudesBE { TipoSolicitudID = -1, TipoSolicitud = clsLenguajeBO.Find("itemSelect") });
            cmbTipoSolicitudes.ItemsSource = TipoSolicitudes;
            cmbTipoSolicitudes.SelectedValuePath = "TipoSolicitudID";
            cmbTipoSolicitudes.DisplayMemberPath = "TipoSolicitud";
            if (TipoSolicitudes.Count() > 1)
            {
                _TipoSolicitudID = TipoSolicitudes.Where(x => x.IsDefault == true).FirstOrDefault().TipoSolicitudID;
                cmbTipoSolicitudes.SelectedValue = _TipoSolicitudID;
            }
            else
            {
                cmbTipoSolicitudes.SelectedValue =  -1;
            }
            cmbTipoSolicitudes.SelectedValuePath = "TipoSolicitudID";

            //Condiciones
            List<clsCondicionesBE> Condiciones = new List<clsCondicionesBE>();
            Condiciones = db.CondicionesGet(null).ToList();
            Condiciones.Add(new clsCondicionesBE { CondicionID = -1, Condicion = clsLenguajeBO.Find("itemSelect") });
            cmbCondicionSolicitudes.ItemsSource = Condiciones;
            cmbCondicionSolicitudes.SelectedValuePath = "CondicionID";
            cmbCondicionSolicitudes.DisplayMemberPath = "Condicion";
            cmbCondicionSolicitudes.SelectedValue = Condiciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
            if (Condiciones.Count() > 1)
            {             
                _CondicionID = Condiciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
                cmbCondicionSolicitudes.SelectedValue = _CondicionID;
            }
            else
            {
                cmbCondicionSolicitudes.SelectedValue = -1;
            }


            //Estados Solicitudes
            List<clsEstadoSolicitudesBE> EstadoSolicitudes = new List<clsEstadoSolicitudesBE>();
            EstadoSolicitudes = db.EstadoSolicitudesGet(null).ToList();
            //EstadoSolicitudes.Add(new clsCondicionesBE { CondicionID = -1, Condicion = clsLenguajeBO.Find("itemSelect") });
            cmbEstados.ItemsSource = EstadoSolicitudes;
            cmbEstados.SelectedValuePath = "EstadoID";
            cmbEstados.DisplayMemberPath = "Estado";
            if (EstadoSolicitudes.Count() > 1)
            {
                _EstadoID = EstadoSolicitudes.Where(x => x.IsDefault == true).FirstOrDefault().EstadoID;
                cmbEstados.SelectedValue = _EstadoID;    
            }
            else
            {
                cmbTipoSolicitudes.SelectedValue = -1;
            }
            

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

            //Sucursales
            List<clsSucursalesBE> List = new List<clsSucursalesBE>();
            List = db.SucursalesGet(null).ToList();

            if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                cmbSucursales.IsEnabled = true;
                //List.Add(new clsSucursalesBE { SucursalID = -1, Sucursal = clsLenguajeBO.Find("itemAll") });
            }
            else
            {
                cmbSucursales.IsEnabled = false;
            }
            cmbSucursales.ItemsSource = List;
            cmbSucursales.SelectedValuePath = "SucursalID";
            cmbSucursales.DisplayMemberPath = "Sucursal";
            cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;

            //if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            //{
            //    cmbSucursales.SelectedValue = -1;
            //}
            //else
            //{
            //    cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
            //}

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

            //Estados Civiles
            cmbEstadosCivilesGarante.ItemsSource = EstadosCiviles;
            cmbEstadosCivilesGarante.SelectedValuePath = "EstadoCivilID";
            cmbEstadosCivilesGarante.DisplayMemberPath = "EstadoCivil";
            cmbEstadosCivilesGarante.SelectedValue = -1;


            InstitucionesGet();
            InstitucionesGarantesGet();

            cmbInstituciones.SelectedValue = 100001;
            cmbInstitucionesGarantes.SelectedValue = 100001;

            //Ocupaciones
            OcupacionesGet();
            OcupacionesGarantesGet();

            //Horarios
            List<clsHorariosBE> Horarios = new List<clsHorariosBE>();
            Horarios = db.HorariosGet(null).ToList();
            Horarios.Add(new clsHorariosBE { HorarioID = -1, Horario = clsLenguajeBO.Find("itemSelect") });
            cmbHorarios.ItemsSource = Horarios;
            cmbHorarios.SelectedValuePath = "HorarioID";
            cmbHorarios.DisplayMemberPath = "Horario";
            cmbHorarios.SelectedValue = 1;//Horarios.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            cmbHorariosGarantes.ItemsSource = Horarios;
            cmbHorariosGarantes.SelectedValuePath = "HorarioID";
            cmbHorariosGarantes.DisplayMemberPath = "Horario";
            cmbHorariosGarantes.SelectedValue = 1;//Horarios.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            //Ingresos
            List<clsIngresosBE> Ingresos = new List<clsIngresosBE>();
            Ingresos = db.IngresosGet(null).ToList();
            Ingresos.Add(new clsIngresosBE { IngresoID = -1, Ingreso = clsLenguajeBO.Find("itemSelect") });
            cmbIngresos.ItemsSource = Ingresos;
            cmbIngresos.SelectedValuePath = "IngresoID";
            cmbIngresos.DisplayMemberPath = "Ingreso";
            cmbIngresos.SelectedValue = 1;//Ingresos.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            cmbIngresosGarantes.ItemsSource = Ingresos;
            cmbIngresosGarantes.SelectedValuePath = "IngresoID";
            cmbIngresosGarantes.DisplayMemberPath = "Ingreso";
            cmbIngresosGarantes.SelectedValue = 1;//Ingresos.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;


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

            txtTiempo.Text = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Tiempo.ToString();
        }

        private void InstitucionesGarantesGet()
        {
            //Instituciones
            List<clsInstitucionesBE> Instituciones = new List<clsInstitucionesBE>();
            Instituciones = db.InstitucionesGet(null).ToList();
            Instituciones.Add(new clsInstitucionesBE { InstitucionID = -1, Institucion = clsLenguajeBO.Find("itemSelect") });
            cmbInstitucionesGarantes.ItemsSource = Instituciones;
            cmbInstitucionesGarantes.SelectedValuePath = "InstitucionID";
            cmbInstitucionesGarantes.DisplayMemberPath = "Institucion";
            cmbInstitucionesGarantes.SelectedValue = -1;//Instituciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
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
            try
            {
                if (VISTA == 1)
                {
                    txtFechaNacimiento.SelectedDate = DateTime.Now;
                    txtFechaIngreso.SelectedDate = DateTime.Now;
                    txtFechaIngresoGarante.SelectedDate = DateTime.Now;
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

                chkDocument.IsChecked = clsCookiesBO.getPrintApplicacion();
            }
            catch(Exception ex) 
            { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
