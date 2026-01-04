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
using Payment.Bank.View.Informes;
using Payment.Bank.Properties;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmCertificados: MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        string _DocumentoGarante = string.Empty;
        //Service.ServiceClient smsWS;

        //Valiables Temporales

        string _Direccion;
        string _Telefono;
        int _OcupacionID;
        int _HorarioID;
        int _IngresoID;
        int _ClienteID;

        int _CertificadoID = 0;

        DateTime Vence = DateTime.Now;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>();
        Core.Manager db = new Core.Manager();
        public frmCertificados()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());

            clsLenguajeBO.Load(gridContactoEmergencia);
            clsLenguajeBO.Load(gridDatosPersonales);
            clsLenguajeBO.Load(gridDatosEconomicos);

            clsLenguajeBO.Load(gridContratos);

            if (VISTA == 1)
            {
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridContratos.DataContext = new clsCertificadosBE();
            }

            btnSalir.Click += btnSalir_Click;
            Loaded += frmContratos_Loaded;
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

            btnAceptar.Click += ttnAceptar_Click;
            btnAdd.Click += btnAdd_Click;
            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;

            //Contratos
            txtInteres.TextChanged += txtInteres_TextChanged;
            txtInteres.LostFocus += txtInteres_LostFocus;
            txtInteres.KeyDown += txtInteres_KeyDown;
            
            txtMonto.KeyDown += txtMonto_KeyDown;
            txtMonto.LostFocus += txtMonto_LostFocus;

            cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;

            btnAddBusiness.Click += btnAddBusiness_Click;
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
            try
            {
                frmInstituciones f = new frmInstituciones();
                f.Owner = this;
                f.Closed += (obj, arg) => { InstitucionesGet(); };
                f.ShowDialog();
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

        private void txtMonto_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Monto = double.Parse(txtMonto.Text);
                txtMonto.Text = String.Format("{0:N2}", Monto);
            }
            catch { }
        }

        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtComision_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtInteres_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }



        private void txtInteres_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //CalcularContratos();
            }
            catch { }
        }

        private void txtInteres_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double interes = double.Parse(txtInteres.Text);
                txtInteres.Text = String.Format("{0:0.000}", interes);
            }
            catch { }
        }
              
        private void CalcularFecha()
        {
            try
            {
                DateTime Fecha;
                Fecha = (DateTime)txtFechaCertificado.SelectedDate;
                int Tiempo = Convert.ToInt16(txtTiempo.Text);
                Vence = Fecha.AddMonths(Tiempo);
            }
            catch { }
        }

        //private void CalcularContratos()
        //{
        //    try
        //    {
        //        if (double.Parse(txtMonto.Text) > 0)
        //        {
        //            double MontoCapital = 0;
        //            double MontoComision = 0;
        //            double MontoInteres = 0;

        //            MontoCapital = (Convert.ToDouble(txtMonto.Text) / Convert.ToDouble(txtTiempo.Text));
        //            MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
        //            MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses));

        //            lblCuota.Text = string.Format("{0:N2}", MontoCapital + MontoComision + MontoInteres);
        //            Cuota = MontoCapital + MontoComision + MontoInteres;

        //            txtMontoPrestamo.Text = string.Format("{0:N2}", Convert.ToDouble(txtTiempo.Text) * (MontoCapital + MontoComision + MontoInteres));
        //            CalcularFecha();

        //            if ((int)cmbTipoContratos.SelectedValue == 1)
        //            {
        //                double Factor1 = 0, Factor2 = 0, Factor = 0;
        //                double Interes = 0; double Tiempo = 0;
        //                Interes = Convert.ToDouble(txtInteres.Text) / 100;
        //                Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
        //                Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
        //                Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
        //                Factor = Factor1 / Factor2;
        //                if (Factor > 0)
        //                {
        //                    lblCuota.Text = string.Format("{0:N2}", ((MontoCapital + MontoComision) * Tiempo) / Factor);
        //                    txtMontoPrestamo.Text = string.Format("{0:N2}", ((MontoCapital + MontoComision) * Tiempo / Factor) * Tiempo);
        //                    Cuota = ((MontoCapital + MontoComision) * Tiempo) / Factor;
        //                }
        //            }
        //            else
        //            {
        //                if ((int)cmbTipoContratos.SelectedValue == 3)
        //                {
        //                    double a = 0;
        //                    if (this.rbComision_Si.IsChecked == true)
        //                    {
        //                        a = MontoComision + MontoInteres;
        //                    }
        //                    else
        //                    {
        //                        a = MontoInteres;
        //                    }
        //                    lblCuota.Text = string.Format("{0:N2}", a);
        //                    Cuota = a;
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        
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

        private void btnAdd_srcPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

        
        public void OnInit(int CertificadoID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                clsCertificadosBE row = db.CertificadosGetByCertificadoID(CertificadoID);
                _CertificadoID = CertificadoID;
                _Documento = row.Documento;

                gridContratos.DataContext = row;

                //Solicitud
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
                txtDireccionTrabajo.Text = (cmbInstituciones.SelectedItem as clsInstitucionesBE).Direccion;
                txtTelefonoTrabajo.Text = (cmbInstituciones.SelectedItem as clsInstitucionesBE).Telefono;
                dtgContactoEmergencias.ItemsSource = row.Personas.Referencias;
                _Referencias = row.Personas.Referencias.ToList();
                //CalcularContratos();
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
                Contactos.Owner = this;
                Contactos.ShowDialog();

            }
            catch (Exception ex) { clsMessage.ErrorMessage(ex.Message, "-"); }
        }

        private void PersonasCreate()
        {
            try
            {
                OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, openFileDialog.FileName, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
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
                bool trabaja = (bool)rbSi.IsChecked;
                OperationResult result = db.DatosEconomicosCreate(txtDocumento.Text, trabaja, (int)cmbInstituciones.SelectedValue, (int)cmbOcupaciones.SelectedValue, (int)cmbHorarios.SelectedValue, (int)cmbIngresos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
                bool trabaja = (bool)rbSi.IsChecked;
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
               
                if (VISTA == 1) { CertificadosCreate(); } else { /*SaveSolicitudesUpdate();*/ }
                
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void NotificarContrato(int ID)
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarContrato == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        //smsWS = new Service.ServiceClient();
                        //smsWS.Endpoint.Address = new EndpointAddress(Settings.Default.smsHost);
                        //var row = db.CertificadosGetByCertificadoID(ID);
                                        
                        //var smsResult = smsWS.EnviarSMSAsync(row.Solicitudes.Clientes.Personas.Celular, string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarContrato), Common.Generic.ShortName(row.Solicitudes.Clientes.Personas.Nombres), row.Solicitudes.Clientes.Sucursales.Empresas.Empresa, row.Solicitudes.Condiciones.Dias, string.Format("{0:N2}",row.Cuota), row.Solicitudes.Tiempo, row.Solicitudes.Condiciones.Condicion));
                        //if (smsResult.Result.ResponseCode != "00")
                        //{                            
                        //    clsMessage.ErrorMessage(smsResult.Result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                        //}
                    }
                    else
                    {
                        clsMessage.ErrorMessage("La red de mensajería no esta disponible en estos momentos.", clsLenguajeBO.Find("msgTitle"));

                    }
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }


        private void CertificadosCreate()
        {
            try
            {
                CalcularFecha();

                OperationResult result = db.CertificadosCreate((DateTime)txtFechaCertificado.SelectedDate, Vence, _Documento, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbTipoCertificados.SelectedValue, (int)cmbMonedas.SelectedValue, Convert.ToInt16(txtTiempo.Text), float.Parse(txtInteres.Text), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {

                    //Aqui va el mensaje final              
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));  
                    ContratosGet(Convert.ToInt32(result.ResponseMessage));
                    ClearAll();

                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                //RadBusyIndicator.IsActive = false;
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void ContratosGet(int v)
        {
            frmPrintCertificados Certificado = new frmPrintCertificados();
            Certificado.Owner = this;
            Certificado.OnInit(v);
            Certificado.ShowDialog();
        }

        private void CertificadosUpdate()
        {
            try
            {
                CalcularFecha();

                OperationResult result = db.CertificadosUpdate(_CertificadoID, (DateTime)txtFechaCertificado.SelectedDate, Vence, _Documento, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbTipoCertificados.SelectedValue, (int)cmbMonedas.SelectedValue, Convert.ToInt16(txtTiempo.Text), float.Parse(txtInteres.Text), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {

                    //Aqui va el mensaje final              
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));       
                    ContratosGet(_CertificadoID);
                    ClearAll();

                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                //RadBusyIndicator.IsActive = false;
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        void ClearAll()
        {
            try
            {
                //clsValidacionesBO.Limpiar(gridSolicitudes);
                clsValidacionesBO.Limpiar(gridDatosPersonales);
                clsValidacionesBO.Limpiar(gridDatosEconomicos);

                clsValidacionesBO.Limpiar(gridContratos);

                gridContratos.DataContext = new clsCertificadosBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();

                _Referencias = new List<clsReferenciasBE>();

                dtgContactoEmergencias.ItemsSource = _Referencias;

                _CertificadoID = 0;
                _OcupacionID = 0;
                _HorarioID = 0;
                _IngresoID = 0;
                _Documento = string.Empty;
                _ClienteID = 0;
                _CertificadoID = 0;
                VISTA = 1;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
                txtFechaCertificado.SelectedDate = DateTime.Now;
                lblCuota.Text = string.Format("{0:N2}", 0);

                txtFechaIngreso.SelectedDate = DateTime.Now;
                cmbInstituciones.SelectedValue = 100001;
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
            }
            catch {

                //clsCuotasView Cuotas = new clsCuotasView();
                //Cuotas.SetDataSource();

            }
        }

        private void SolicitudesUpdate()
        {
            try
            {
                if (VISTA == 1) { CertificadosCreate(); } else { CertificadosUpdate();}
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

        private void ttnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // color = (n.Difference1 < 0) ? Negativo : (n.Difference1 > 0) ? Positivo : Balanceado,                                                            
                if (VISTA == 1 && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridContratos))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        //RadBusyIndicator.IsActive = true;
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridContratos))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            //RadBusyIndicator.IsActive = true;
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
            //Tipo de Certificados
            List<clsTipoCertificadosBE> TipoCertificados = new List<clsTipoCertificadosBE>();
            TipoCertificados = db.TipoCertificadosGet(null).ToList();
            TipoCertificados.Add(new clsTipoCertificadosBE { TipoCertificadoID = -1, TipoCertificado = clsLenguajeBO.Find("itemSelect") });
            cmbTipoCertificados.ItemsSource = TipoCertificados;
            cmbTipoCertificados.SelectedValuePath = "TipoCertificadoID";
            cmbTipoCertificados.DisplayMemberPath = "TipoCertificado";
            cmbTipoCertificados.SelectedValue = -1;
            
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

            //Monedas
            List<clsMonedasBE> Monedas = new List<clsMonedasBE>();
            Monedas = db.MonedasGet(null).ToList();
     

            List<clsMonedasBE> items = new List<clsMonedasBE>();
            foreach(var element in Monedas)
            {
                items.Add(new clsMonedasBE { MonedaID = element.MonedaID, Fecha = element.Fecha, Codigo = element.Codigo, Moneda = element.Codigo + " - " + element.Moneda });
            }

            items.Add(new clsMonedasBE { MonedaID = -1, Moneda = clsLenguajeBO.Find("itemSelect") });

            cmbMonedas.ItemsSource = items;
            cmbMonedas.SelectedValuePath = "MonedaID";
            cmbMonedas.DisplayMemberPath = "Moneda";
            cmbMonedas.SelectedValue = -1;

        }

        private void frmContratos_Loaded(object sender, RoutedEventArgs e)
        {
            if (VISTA == 1)
            {
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                LoadCombox();
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                txtFechaCertificado.SelectedDate = DateTime.Now;
                txtFechaIngreso.SelectedDate = DateTime.Now;
            }
            //TipoSolicitudChange();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
