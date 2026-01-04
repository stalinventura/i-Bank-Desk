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
    public partial class frmInstituciones : MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        //Valiables Temporales


        int _InstitucionID;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>();
        Core.Manager db = new Core.Manager();
        public frmInstituciones()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridSolicitudes);
            clsLenguajeBO.Load(gridDatosPersonales);


            if (VISTA == 1)
            {
                gridSolicitudes.DataContext = new clsInstitucionesBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
            }

            btnSalir.Click += btnSalir_Click;
            Loaded += frmSolicitudes_Loaded;
            txtDocumento.LostFocus += txtDocumento_LostFocus;
            txtDocumento.KeyDown += txtDocumento_KeyDown;
            txtTelefono.LostFocus += txtTelefono_LostFocus;
            txtTelefono.KeyDown += txtTelefono_KeyDown;
            txtCelular.LostFocus += txtCelular_LostFocus;
            txtCelular.KeyDown += txtCelular_KeyDown;
            txtTelefonoInstitucion.LostFocus += txtTelefonoInstitucion_LostFocus;


            btnAceptar.Click += ttnAceptar_Click;

            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;


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

        public void OnInit(int InstitucionID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                clsInstitucionesBE row = db.InstitucionesGetByInstitucionID(_InstitucionID);
                _InstitucionID = InstitucionID;
                _Documento = row.Documento;


                //Solicitud
                gridSolicitudes.DataContext = row;
                gridDatosPersonales.DataContext = row.Personas;


            }
            catch { }
        }



        private void PersonasCreate()
        {
            try
            {
                OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, null, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    InstitucionesCreate();
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
                //var _byte = Common.Generic.ImageFromUrl(openFileDialog.FileName);               
                OperationResult result = db.PersonasUpdate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, null, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    if (VISTA == 1)
                    {
                        InstitucionesCreate();
                        if ((gridDatosPersonales.DataContext as clsPersonasBE).Fotografias != null)
                        {
                            byte[] data = (gridDatosPersonales.DataContext as clsPersonasBE).Fotografias.Foto;
                            SavePhoto(txtDocumento.Text, data);
                        }
                    }
                    else
                    {
                        InstitucionesUpdate();
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void InstitucionesCreate()
        {
            try
            {
                OperationResult result = new OperationResult();
                result = db.InstitucionesCreate(txtInstitucion.Text, txtDireccionInstitucion.Text, txtTelefonoInstitucion.Text, txtDocumento.Text, clsVariablesBO.UsuariosBE.Documento);
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
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); RadBusyIndicator.IsActive = false; }

        }

        private void InstitucionesUpdate()
        {
            try
            {
                OperationResult result = new OperationResult();
                result = db.InstitucionesUpdate(_InstitucionID, txtInstitucion.Text, txtDireccionInstitucion.Text, txtTelefonoInstitucion.Text, txtDocumento.Text, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    ClearAll();
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle")); RadBusyIndicator.IsActive = false;
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); RadBusyIndicator.IsActive = false; }

        }

        void ClearAll()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridSolicitudes);
                clsValidacionesBO.Limpiar(gridDatosPersonales);
                RadBusyIndicator.IsActive = false;

                srcPhoto.Source = null;
  
                _InstitucionID = 0;
              
                _Documento = string.Empty;                     
                VISTA = 1;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
            }
            catch { }
        }

        private void ttnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                                                                                                                                                               // color = (n.Difference1 < 0) ? Negativo : (n.Difference1 > 0) ? Positivo : Balanceado,                                                            
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true )
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true )
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

        private void txtTelefonoInstitucion_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtTelefonoInstitucion.Text;
                txtTelefonoInstitucion.Text = clsValidacionesBO.PhoneFormat(Phone);
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

                            srcPhoto.DataContext = Persona.Fotografia;


                        }
                        else
                        {
                            _Documento = txtDocumento.Text;
                            clsValidacionesBO.Limpiar(gridDatosPersonales);
                            _Referencias = new List<clsReferenciasBE>();
               
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


        }
        private void frmSolicitudes_Loaded(object sender, RoutedEventArgs e)
        {

            if (VISTA == 1)
            {
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                LoadCombox();
            }
        
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
