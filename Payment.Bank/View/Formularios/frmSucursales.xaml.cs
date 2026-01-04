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

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmSucursales : MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;

        //Valiables Temporales

        int _OcupacionID;
        int _HorarioID;
        int _IngresoID;
        int _SucursalID;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>(); 
        Core.Manager db = new Core.Manager();
        public frmSucursales()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridSucursales);
            clsLenguajeBO.Load(gridDatosPersonales);
            clsLenguajeBO.Load(gridDatosEconomicos);
            clsLenguajeBO.Load(gridContactoEmergencia);

            if (VISTA == 1)
            {
                gridSucursales.DataContext = new clsSucursalesBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
            }

            btnSalir.Click += btnSalir_Click;
            Loaded += frmSucursales_Loaded;
            rbSi.Checked += rbSi_Checked;
            rbNo.Checked += rbNo_Checked;
            txtDocumento.LostFocus += txtDocumento_LostFocus;
            txtDocumento.KeyDown += txtDocumento_KeyDown;
            txtTelefono.LostFocus += txtTelefono_LostFocus;
            txtTelefono.KeyDown += txtTelefono_KeyDown;
            txtCelular.LostFocus += txtCelular_LostFocus;
            txtCelular.KeyDown += txtCelular_KeyDown;
            txtTelefonoTrabajo.LostFocus += txtTelefonoTrabajo_LostFocus;
            txtTelefonoTrabajo.KeyDown += txtTelefonoTrabajo_KeyDown;
  
            btnAceptar.Click += ttnAceptar_Click;
            btnAdd.Click += btnAdd_Click;
            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;


            cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;

            btnAddBusiness.Click += btnAddBusiness_Click;
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

        public void OnInit(int SucursalID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                clsSucursalesBE row = db.SucursalesGetBySucursalID(SucursalID);
                _SucursalID = SucursalID;
                _Documento = row.Documento;
                
                //Sucursal
                gridSucursales.DataContext = row;
                gridDatosPersonales.DataContext = row.Gerentes.Personas;


                if (row.Gerentes.Personas.DatosEconomicos.Trabaja == true)
                {
                    rbSi.IsChecked = true;
                }
                else
                {
                    rbNo.IsChecked = true;
                }

                gridDatosEconomicos.DataContext = row.Gerentes.Personas.DatosEconomicos;
                dtgContactoEmergencias.ItemsSource = row.Gerentes.Personas.Referencias;
                _Referencias = row.Gerentes.Personas.Referencias.ToList();

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

        private void PersonasCreate()
        {
            try
            {
               OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, null, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
               if(result.ResponseCode == "00")
                {
                    GerentesCreate();
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
                        GerentesCreate();
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

        private void GerentesCreate()
        {
            try
            {
                OperationResult result = db.GerentesCreate(txtDocumento.Text, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    _Documento = result.ResponseMessage;
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
                        SucursalesCreate();
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
                        SucursalesUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }



        private void SucursalesCreate()
        {
            try
            {
                OperationResult result = db.SucursalesCreate(txtSucursal.Text, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbCiudadesSucursales.SelectedValue, _Documento, txtDireccionSucursal.Text, txtTelefonoSucursal.Text, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    //Aqui va el mensaje final              
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));

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
                clsValidacionesBO.Limpiar(gridSucursales);
                clsValidacionesBO.Limpiar(gridDatosPersonales);
                clsValidacionesBO.Limpiar(gridDatosEconomicos);
                _Referencias = new List<clsReferenciasBE>();
                dtgContactoEmergencias.ItemsSource = _Referencias;
                srcPhoto.Source = null;
                _SucursalID = 0;
                _OcupacionID = 0;
                _HorarioID = 0;
                _IngresoID = 0;
                _Documento = string.Empty;                          
                VISTA = 1;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
                cmbInstituciones.SelectedValue = 100001;
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;

            }
            catch { }
        }

  

        private void SucursalesUpdate()
        {
            try
            {
                OperationResult result = db.SucursalesUpdate(_SucursalID, txtSucursal.Text, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbCiudadesSucursales.SelectedValue, _Documento,txtDireccionSucursal.Text, txtTelefonoSucursal.Text, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    //Aqui va el mensaje final
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
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
                        SucursalesCreate();
                    }
                    else
                    {
                        SucursalesUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }

        private void ttnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                                                                                                                                                               // color = (n.Difference1 < 0) ? Negativo : (n.Difference1 > 0) ? Positivo : Balanceado,                                                            
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSucursales) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true)
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSucursales) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true)
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
            catch (Exception sqlEx) { RadBusyIndicator.IsActive = false; clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
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
                                gridDatosEconomicos.DataContext = Persona.DatosEconomicos;
                                cmbOcupaciones.SelectedValue = Persona.DatosEconomicos.OcupacionID;
                                cmbIngresos.SelectedValue = Persona.DatosEconomicos.IngresoID;
                                cmbHorarios.SelectedValue = Persona.DatosEconomicos.HorarioID;

                                if (Persona.DatosEconomicos.Trabaja == true)
                                {
                                    rbSi.IsChecked = true;
                                }
                                else
                                {
                                    rbNo.IsChecked = true;
                                }

                            }

                            if (Persona.Referencias.Count > 0)
                            {

                                dtgContactoEmergencias.ItemsSource = Persona.Referencias;
                                _Referencias = Persona.Referencias.ToList();
                            }
                        }
                        else
                        {
                            _Documento = txtDocumento.Text;
                            clsValidacionesBO.Limpiar(gridDatosPersonales);
                            clsValidacionesBO.Limpiar(gridDatosEconomicos);
                            _Referencias = new List<clsReferenciasBE>();
                            dtgContactoEmergencias.ItemsSource = _Referencias;
                            rbNo.IsChecked = true;
                            txtDocumento.Text = _Documento;
                            _Documento = string.Empty;
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
          
            //Ciudades
            List<clsCiudadesBE> Ciudades = new List<clsCiudadesBE>();
            Ciudades = db.CiudadesGet(null).ToList();
            Ciudades.Add(new clsCiudadesBE { CiudadID = -1, Ciudad = clsLenguajeBO.Find("itemSelect") });
            cmbCiudades.ItemsSource = Ciudades;
            cmbCiudades.SelectedValuePath = "CiudadID";
            cmbCiudades.DisplayMemberPath = "Ciudad";
            cmbCiudades.SelectedValue = -1;//Ciudades.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            cmbCiudadesSucursales.ItemsSource = Ciudades;
            cmbCiudadesSucursales.SelectedValuePath = "CiudadID";
            cmbCiudadesSucursales.DisplayMemberPath = "Ciudad";
            cmbCiudadesSucursales.SelectedValue = -1;


            //Operadores
            List<clsOperadoresBE> Operadores = new List<clsOperadoresBE>();
            Operadores = db.OperadoresGet(null).ToList();
            Operadores.Add(new clsOperadoresBE { OperadorID = -1, Operador = clsLenguajeBO.Find("itemSelect") });
            cmbOperadores.ItemsSource = Operadores;
            cmbOperadores.SelectedValuePath = "OperadorID";
            cmbOperadores.DisplayMemberPath = "Operador";
            cmbOperadores.SelectedValue = -1;//Operadores.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID


            //Sexos
            List<clsSexosBE> Sexos = new List<clsSexosBE>();
            Sexos = db.SexosGet(null).ToList();
            Sexos.Add(new clsSexosBE { SexoID = -1, Sexo = clsLenguajeBO.Find("itemSelect") });
            cmbSexos.ItemsSource = Sexos;
            cmbSexos.SelectedValuePath = "SexoID";
            cmbSexos.DisplayMemberPath = "Sexo";
            cmbSexos.SelectedValue = -1;//Sexos.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            //Estados Civiles
            List<clsEstadosCivilesBE> EstadosCiviles = new List<clsEstadosCivilesBE>();
            EstadosCiviles = db.EstadosCivilesGet(null).ToList();
            EstadosCiviles.Add(new clsEstadosCivilesBE { EstadoCivilID = -1, EstadoCivil = clsLenguajeBO.Find("itemSelect") });
            cmbEstadosCiviles.ItemsSource = EstadosCiviles;
            cmbEstadosCiviles.SelectedValuePath = "EstadoCivilID";
            cmbEstadosCiviles.DisplayMemberPath = "EstadoCivil";
            cmbEstadosCiviles.SelectedValue = -1;//EstadosCiviles.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
            
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

        private void frmSucursales_Loaded(object sender, RoutedEventArgs e)
        {

            if (VISTA == 1)
            {
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;

                LoadCombox();
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
