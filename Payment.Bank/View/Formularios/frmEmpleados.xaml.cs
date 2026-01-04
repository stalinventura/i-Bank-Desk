using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Payment.Bank.Entity;


using Payment.Bank.Modulos;
using Microsoft.Win32;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmEmpleados.xaml
    /// </summary>
    public partial class frmEmpleados : Window
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        int _EmpleadoID = 0;
        string _DocumentoGarante = string.Empty;
        //Service.ServiceClient smsWS;

        //Valiables Temporales
        string _Institucion;
        string _Direccion;
        string _Telefono;
        int _OcupacionID;
        int _HorarioID;
        int _IngresoID;
        int _SolicitudID;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>(); 
        Core.Manager db = new Core.Manager();
        public frmEmpleados()
        {
            InitializeComponent();
            LoadCombox();
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridDatosPersonales);
            clsLenguajeBO.Load(gridDatosEconomicos);
            clsLenguajeBO.Load(gridContactoEmergencia);
            clsLenguajeBO.Load(gridDatosEmpleados);

            btnSalir.Click += btnSalir_Click;
            Loaded += frmEmpleados_Loaded;
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
            btnSalir.Click += btnSalir_Click;
            btnAdd.Click += btnAdd_Click;
            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;

            cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;
            btnAddBusiness.Click += btnAddBusiness_Click;
        }

        private void btnAddBusiness_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmInstituciones f = new frmInstituciones();
                f.Owner = this;
                f.Closed += (obj, arg) => { InstitucionesGet(); };
                f.ShowDialog();
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

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        public void OnInit(clsEmpleadosBE row, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                //LoadCombox();

                _EmpleadoID = row.EmpleadoID;
                _Documento = row.Documento;
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
                gridDatosEmpleados.DataContext = new clsEmpleadosBE {PuestoID = row.PuestoID, SucursalID = row.SucursalID, Sueldo = row.Sueldo };


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
               OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, openFileDialog.FileName, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
               if(result.ResponseCode == "00")
                {
                    try
                    {
                        if ((srcPhoto.DataContext as clsPersonasBE).Fotografias != null)
                        {
                            byte[] data = (srcPhoto.DataContext as clsPersonasBE).Fotografias.Foto;
                            SavePhoto(txtDocumento.Text, data);
                        }
                    }
                    catch { }

                    DatosEconomicosCreate();
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
                        try
                        {
                            if ((srcPhoto.DataContext as clsPersonasBE).Fotografias != null)
                            {
                                byte[] data = (srcPhoto.DataContext as clsPersonasBE).Fotografias.Foto;
                                SavePhoto(txtDocumento.Text, data);
                            }
                        }
                        catch { }

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
                        UsuariosCreate();
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
                        UsuariosUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

      



        private void UsuariosCreate()
        {
            try
            {
                OperationResult result = db.EmpleadosCreate(txtDocumento.Text, (int)cmbSucursales.SelectedValue, (int)cmbPuestos.SelectedValue, float.Parse(txtSuelto.Text), clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
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
                clsValidacionesBO.Limpiar(gridDatosPersonales);
                clsValidacionesBO.Limpiar(gridDatosEconomicos);
                clsValidacionesBO.Limpiar(gridDatosEmpleados);

              
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosEmpleados.DataContext = new clsEmpleadosBE();

                _Referencias = new List<clsReferenciasBE>();

                dtgContactoEmergencias.ItemsSource = _Referencias;
                _EmpleadoID = 0;
                srcPhoto.Source = null;
                _SolicitudID = 0;
                _OcupacionID = 0;
                _HorarioID = 0;
                _IngresoID = 0;
                _SolicitudID = 0;
                _Documento = string.Empty;               
                VISTA = 1;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
                cmbPuestos.SelectedValue = -1;
            }
            catch { }
        }


        private void UsuariosUpdate()
        {
            try
            {
                OperationResult result = db.EmpleadosUpdate(_EmpleadoID, txtDocumento.Text, (int)cmbSucursales.SelectedValue, (int)cmbPuestos.SelectedValue, float.Parse(txtSuelto.Text), clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgUpdate"), clsLenguajeBO.Find("msgTitle"));
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
                        UsuariosCreate();
                    }
                    else
                    {
                        UsuariosUpdate();
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
                if (VISTA == 1 && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true && clsValidacionesBO.Validar(gridDatosEmpleados) == true)
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridDatosEconomicos) == true && clsValidacionesBO.Validar(gridDatosEmpleados) == true)
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
                            txtFechaNacimiento.SelectedDate = Persona.FechaNacimiento;
                            txtNombres.Text = Persona.Nombres;
                            txtApellidos.Text = Persona.Apellidos;
                            txtApodo.Text = Persona.Apodo;

                            

                            cmbCiudades.SelectedValue = Persona.CiudadID;
                            txtDireccion.Text = Persona.Direccion;
                            txtCorreoElectronico.Text = Persona.Correo;
                            txtTelefono.Text = Persona.Telefono;
                            txtCelular.Text = Persona.Celular;
                            cmbOperadores.SelectedValue = Persona.OperadorID;
                            cmbSexos.SelectedValue = Persona.SexoID;
                            cmbEstadosCiviles.SelectedValue = Persona.EstadoCivilID;

                            srcPhoto.DataContext = Persona;

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
                                //txtInstitucionTrabaja.Text = Persona.DatosEconomicos.Empresa;
                                //txtDireccionTrabajo.Text = Persona.DatosEconomicos.Direccion;
                                //txtTelefonoTrabajo.Text = Persona.DatosEconomicos.Telefono;
                                cmbOcupaciones.SelectedValue = Persona.DatosEconomicos.OcupacionID;
                                cmbIngresos.SelectedValue = Persona.DatosEconomicos.IngresoID;
                                cmbHorarios.SelectedValue = Persona.DatosEconomicos.HorarioID;
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
                cmbOcupaciones.IsEnabled = false;
            cmbHorarios.IsEnabled = false;
            cmbIngresos.IsEnabled = false;
                
            _Direccion = txtDireccionTrabajo.Text;
            _Telefono = txtTelefonoTrabajo.Text;
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
                cmbOcupaciones.IsEnabled = true;
                cmbHorarios.IsEnabled = true;
                cmbIngresos.IsEnabled = true;
                
                txtDireccionTrabajo.Text = _Direccion;
                txtTelefonoTrabajo.Text = _Telefono;
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

            //Instituciones
            InstitucionesGet();
            cmbInstituciones.SelectedValue = 100001;//Instituciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

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

            //Sucursales
            List<clsSucursalesBE> Sucursales = new List<clsSucursalesBE>();
            Sucursales = db.SucursalesGet(null).ToList();
            cmbSucursales.ItemsSource = Sucursales;
            cmbSucursales.SelectedValuePath = "SucursalID";
            cmbSucursales.DisplayMemberPath = "Sucursal";
            cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;

            if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                cmbSucursales.IsEnabled = true;
            }
            else
            {
                cmbSucursales.IsEnabled = false;
            }


            //Puestos
            List<clsPuestosBE> puestos = new List<clsPuestosBE>();
            puestos = db.PuestosGet(null).ToList();
            puestos.Add(new clsPuestosBE { PuestoID = -1, Puesto = clsLenguajeBO.Find("itemSelect") });
            cmbPuestos.ItemsSource = puestos;
            cmbPuestos.SelectedValuePath = "PuestoID";
            cmbPuestos.DisplayMemberPath = "Puesto";
            cmbPuestos.SelectedValue = -1;

        }
        private void frmEmpleados_Loaded(object sender, RoutedEventArgs e)
        {

            if (VISTA == 1)
            {
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
               
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;

                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosEmpleados.DataContext = new clsEmpleadosBE { PuestoID = -1, SucursalID = clsVariablesBO.UsuariosBE.SucursalID };

            }
        }

    }
}
