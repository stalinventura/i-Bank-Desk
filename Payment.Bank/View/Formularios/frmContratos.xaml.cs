using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;

using Payment.Bank.Modulos;
using Payment.Bank.Core;
using Microsoft.Win32;
using Payment.Bank.View.Informes;
using Payment.Bank.View.Consultas;
using Payment.Bank.Controles;
using System.Xml.Linq;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmContratos : MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        string _DocumentoGarante = string.Empty;
        int OficialID = -1;
        int NotarioPublicoID = -1;
        int OperadorID = 1;
        int RutaID = -1;
        string photo = string.Empty;
        string photoGarante = string.Empty;
        float TasaLegal = 0;
        float TasaSeguro = 0;
        //float TasaInteres = 0;
        public bool PermissionAccess = false;


        //Valiables Temporales
        string _Direccion;
        string _Telefono;
        int _OcupacionID;
        int _HorarioID;
        int _IngresoID;
        int _ClienteID;
        int _SolicitudID;
        int _TipoContratoID;
        int _TipoSolicitudID;
        int _CondicionID;
        int _ObjetivoID = 0;
        int _ContratoID = 0;
        double Cuota = 0;
        DateTime Vence = DateTime.Now;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>();
        Core.Manager db = new Core.Manager();
        public frmContratos()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height * 0.95;
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridSolicitudes);
            clsLenguajeBO.Load(gridDatosPersonales);
            clsLenguajeBO.Load(gridDatosEconomicos);
            clsLenguajeBO.Load(gridContactoEmergencia);
            clsLenguajeBO.Load(gridDatosGarantiaHipotecario);
            clsLenguajeBO.Load(gridDatosGarantiaPersonal);
            clsLenguajeBO.Load(gridDatosGarantiaVehiculo);
            clsLenguajeBO.Load(gridContratos);
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
                gridContratos.DataContext = new clsContratosBE();
            }

            btnSalir.Click += btnSalir_Click;
            cmbTipoSolicitudes.SelectionChanged += cmbTipoSolicitudes_SelectionChanged;
            cmbCondicionSolicitudes.SelectionChanged += cmbCondicionSolicitudes_SelectionChanged;

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
            lblLegal.TextChanged += lblLegal_TextChanged;

            lblLegal.KeyDown += lblLegal_KeyDown;
            lblSeguro.KeyDown += lblSeguro_KeyDown;

            lblLegal.LostFocus += lblLegal_LostFocus;
            lblSeguro.LostFocus += lblSeguro_LostFocus;

            btnAceptar.Click += btnAceptar_Click;
            btnAdd.Click += btnAdd_Click;
            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;
            btnAdd_srcPhotoGarante.Click += btnAdd_srcPhotoGarante_Click;

            btnAdd_srcCamera.Click += btnAdd_srcCamera_Click;
            btnAdd_srcCameraGarante.Click += btnAdd_srcCameraGarante_Click;

            //Garantias
            txtDocumentoGarante.TextChanged += txtDocumentoGarante_TextChanged;
            txtDocumentoGarante.LostFocus += txtDocumentoGarante_LostFocus;
            txtDocumentoGarante.KeyDown += txtDocumentoGarante_KeyDown;
            txtTelefonoGarante.LostFocus += txtTelefonoGarante_LostFocus;
            txtTelefonoGarante.KeyDown += txtTelefonoGarante_KeyDown;
            txtCelularGarante.LostFocus += txtCelularGarante_LostFocus;
            txtCelularGarante.KeyDown += txtCelularGarante_KeyDown;

            //Contratos
            txtNumero.KeyDown += txtNumero_KeyDown;
            txtNumero.TextChanged += txtNumero_TextChanged;
            cmbTipoContratos.SelectionChanged += cmbTipoContratos_SelectionChanged;
            txtInteres.TextChanged += txtInteres_TextChanged;
            txtInteres.LostFocus += txtInteres_LostFocus;
            txtInteres.KeyDown += txtInteres_KeyDown;

            lblCuota.TextChanged += lblCuota_TextChanged;
            lblCuota.LostFocus += lblCuota_LostFocus;
            lblCuota.KeyDown += lblCuota_KeyDown;
            chkCierre.Checked += chkCierre_Checked;
            chkCierre.Unchecked += chkCierre_Unchecked;

            chkSeguro.Checked += chkSeguro_Checked;
            chkSeguro.Unchecked += chkSeguro_Unchecked;

            txtComision.TextChanged += txtComision_TextChanged;
            txtComision.LostFocus += txtComision_LostFocus;
            txtComision.KeyDown += txtComision_KeyDown;
            txtMontoPrestamo.KeyDown += txtMontoPrestamo_KeyDown;
            cmbOficiales.SelectionChanged += cmbOficiales_SelectionChanged;
            cmbNotarioPublico.SelectionChanged += cmbNotarioPublico_SelectionChanged;
            cmbZonas.SelectionChanged += cmbZonas_SelectionChanged;

            rbMorasAut_Si.Checked += rbMorasAut_Si_Checked;
            rbMorasAut_No.Checked += rbMorasAut_No_Checked;

            rbLegal_Si.Checked += rbLegal_Si_Checked;
            rbLegal_No.Checked += rbLegal_No_Checked;

            lblSeguro.TextChanged += lblSeguro_TextChanged;
            txtLegal.TextChanged += txtLegal_TextChanged;
            txtLegal.LostFocus += txtLegal_LostFocus;
            txtLegal.KeyDown += txtLegal_KeyDown;


            rbInsurance_Si.Checked += rbInsurance_Si_Checked;
            rbInsurance_No.Checked += rbInsurance_No_Checked;

            txtTasaSeguro.TextChanged += txtTasaSeguro_TextChanged;
            txtTasaSeguro.LostFocus += txtTasaSeguro_LostFocus;
            txtTasaSeguro.KeyDown += txtTasaSeguro_KeyDown;


            cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;
            btnAddRoad.Click += btnAddRoad_Click;

            btnAddBusiness.Click += btnAddBusiness_Click;

            cmbSucursales.SelectionChanged += cmbSucursales_SelectionChanged;

            //Bancos
            cmbBancos.IsEnabled = false;
            rbBankRequest_Si.Checked += rbBankRequest_Si_Checked;
            rbBankRequest_No.Checked += rbBankRequest_No_Checked;

            chkDocument.Checked += chkDocument_Checked;
            chkDocument.Unchecked += chkDocument_Unchecked;

            try
            {

                if ((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).IsGenerateRequest == true)
                {
                    rbBankRequest_Si.IsChecked = true;
                }
                else
                {
                    rbBankRequest_No.IsChecked = true;
                }
            }
            catch { }

        }

        private void chkDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = (CheckBox)sender;
                clsCookiesBO.PrintContract((bool)obj.IsChecked);
            }
            catch { }
        }

        private void chkDocument_Checked(object sender, RoutedEventArgs e)
        {
            try {
                var obj = (CheckBox)sender;
                clsCookiesBO.PrintContract((bool)obj.IsChecked);

            } catch { }
        }

        private void lblSeguro_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularSeguroGetByMonto();
        }

        private void lblSeguro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double seguro = double.Parse(lblSeguro.Text);
                lblSeguro.Text = string.Format("{0:N2}", seguro);
            }
            catch { lblSeguro.Text = string.Format("{0:N2}", 0); }
        }

        private void lblLegal_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double legal = double.Parse(lblLegal.Text);
                lblLegal.Text = string.Format("{0:N2}", legal);
            }
            catch { lblLegal.Text = string.Format("{0:N2}", 0); }
        }

        private void chkCierre_Unchecked(object sender, RoutedEventArgs e)
        {
            txtLegal.IsReadOnly = false;
            lblLegal.IsReadOnly = true;
        }

        private void chkCierre_Checked(object sender, RoutedEventArgs e)
        {
            txtLegal.IsReadOnly = true;
            lblLegal.IsReadOnly = false;
        }

        private void chkSeguro_Unchecked(object sender, RoutedEventArgs e)
        {
            txtTasaSeguro.IsReadOnly = false;
            lblSeguro.IsReadOnly = true;
        }

        private void chkSeguro_Checked(object sender, RoutedEventArgs e)
        {
            txtTasaSeguro.IsReadOnly = true;
            lblSeguro.IsReadOnly = false;
            lblSeguro.IsEnabled = true;
        }

        private void lblSeguro_KeyDown(object sender, KeyEventArgs e)
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

        private void lblLegal_KeyDown(object sender, KeyEventArgs e)
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

        private void lblLegal_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularLegalGetByMonto();
        }

        private void cmbSucursales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                OficialesGetBySucursalID();
            }
            catch { }
        }

        private void OficialesGetBySucursalID()
        {
            try
            {
                if ((int)cmbSucursales.SelectedValue != -1)
                {
                    //Oficiales
                    List<clsOficialesBE> Oficiales = new List<clsOficialesBE>();
                    Oficiales = db.OficialesGet(null).Where(x => x.SucursalID == (int)cmbSucursales.SelectedValue).ToList();
                    Oficiales.Add(new clsOficialesBE { OficialID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemSelect") } });

                    List<clsOficialesBE> ListOfAgent = new List<clsOficialesBE>();
                    foreach (var row in Oficiales)
                    {
                        ListOfAgent.Add(new clsOficialesBE { OficialID = row.OficialID, SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                    }

                    cmbOficiales.ItemsSource = ListOfAgent;
                    cmbOficiales.SelectedValuePath = "OficialID";
                    cmbOficiales.DisplayMemberPath = "Personas.Nombres";
                    cmbOficiales.SelectedValue = -1;
                }
            }
            catch { }
        }

        private void txtTasaSeguro_KeyDown(object sender, KeyEventArgs e)
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
                        string filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), camera.photo);
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
                    if (camera.TakePhoto)
                    {
                        string filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), camera.photo);
                        srcPhoto.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));
                        photo = filename;
                    }
                };
                camera.ShowDialog();
            }
            catch { }
        }
        private void txtTasaSeguro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (chkSeguro.IsChecked == false)
            {
                CalcularSeguro();
            }
        }

        private void CalcularSeguro()
        {
            try
            {
                if (float.Parse(txtMonto.Text) > 0 && float.Parse(txtTasaSeguro.Text) > 0)
                {
                    float value = (float.Parse(txtMonto.Text) * float.Parse(txtTasaSeguro.Text)) / 100;
                    TasaSeguro = (value / float.Parse(txtMonto.Text)) * 100;
                    lblSeguro.Text = string.Format("{0:N2}", value);
                }
            }
            catch (Exception ex) { }
        }

        private void txtTasaSeguro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double seguro = double.Parse(txtTasaSeguro.Text);
                txtTasaSeguro.Text = string.Format("{0:0.000}", seguro);
            }
            catch { txtTasaSeguro.Text = string.Format("{0:0.000}", 0); }
        }

        private void txtLegal_KeyDown(object sender, KeyEventArgs e)
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

        private void txtLegal_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (chkCierre.IsChecked == false)
            {
                CalcularLegal();
            }
        }

        private void CalcularLegal()
        {
            try
            {
                if (float.Parse(txtMonto.Text) > 0 && float.Parse(txtLegal.Text) > 0)
                {
                    float value = float.Parse(txtMonto.Text) * (float.Parse(txtLegal.Text)/100);
                    TasaLegal = (value / float.Parse(txtMonto.Text)) * 100;
                    lblLegal.Text = string.Format("{0:N2}", value);
                }
            }
            catch (Exception ex) { }
        }


        private void CalcularSeguroGetByMonto()
        {
            try
            {
                if (float.Parse(txtMonto.Text) > 0 && rbInsurance_Si.IsChecked == true && chkSeguro.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(lblSeguro.Text))
                    {
                        float value = (float.Parse(lblSeguro.Text) / float.Parse(txtMonto.Text)) * 100;
                        TasaSeguro = value;
                        txtTasaSeguro.Text = value.ToString();//string.Format("{0:N2}", value);
                    }
                    else
                    {
                        txtTasaSeguro.Text = string.Format("{0:N2}", 0);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void CalcularLegalGetByMonto()
        {
            try
            {
                if (float.Parse(txtMonto.Text) > 0 && rbLegal_Si.IsChecked == true && chkCierre.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(lblLegal.Text))
                    {
                        float value = (float.Parse(lblLegal.Text) / float.Parse(txtMonto.Text)) * 100;
                        TasaLegal = value;
                        txtLegal.Text = value.ToString();
                    }
                    else
                    {
                        txtLegal.Text = string.Format("{0:N2}", 0);
                    }
                }

            }
            catch (Exception ex) { }
        }


        private void txtLegal_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double legal = double.Parse(txtLegal.Text);
                //txtLegal.Text = string.Format("{0:0.000}", legal);
            }
            catch { txtLegal.Text = string.Format("{0:0.000}", 0); }
        }

        private void rbBankRequest_No_Checked(object sender, RoutedEventArgs e)
        {
            cmbBancos.IsEnabled = false;
            cmbBancos.SelectedValue = -1;
        }

        private void rbBankRequest_Si_Checked(object sender, RoutedEventArgs e)
        {
            cmbBancos.IsEnabled = true;
        }

        private void cmbZonas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RutaID = (int)cmbZonas.SelectedValue;
            }
            catch { }
        }

        private void cmbNotarioPublico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                NotarioPublicoID = (int)cmbNotarioPublico.SelectedValue;
            }
            catch { }
        }

        private void cmbOficiales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((int)cmbOficiales.SelectedValue != -1)
                {
                    OficialID = (int)cmbOficiales.SelectedValue;
                }
            }
            catch { }
        }

        private void lblCuota_KeyDown(object sender, KeyEventArgs e)
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

        private void lblCuota_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Cuota = double.Parse(lblCuota.Text);
                lblCuota.Text = String.Format("{0:N2}", Cuota);
            }
            catch { }
        }

        private void lblCuota_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (chkCuotas.IsChecked == true)
                {
                    CalcularContratosGetByCuota();
                }
            }
            catch { }
        }

        private void CalcularContratosGetByCuota()
        {
            try
            {
                if (float.Parse(txtMonto.Text) > 0 && float.Parse(txtInteres.Text) >= 0)
                { 
                    double MontoCapital = 0;
                    MontoCapital = (Convert.ToDouble(txtMonto.Text) / Convert.ToDouble(txtTiempo.Text));

                    double Tasa = 0;
                    Tasa = (((Convert.ToDouble(lblCuota.Text) - MontoCapital) / (MontoCapital * Convert.ToDouble(txtTiempo.Text))) * (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses) * 100;

                    CalcularFecha();
                    //ValorInteres = MontoInteres / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Dias;

                    txtInteres.Text = String.Format("{0:0.0000}", Tasa);
                    //TasaInteres = (float)Tasa;
                    txtMontoPrestamo.Text = String.Format("{0:N2}", Convert.ToDouble(lblCuota.Text) * Convert.ToDouble(txtTiempo.Text));
                }
                else
                {
                    txtInteres.Text = String.Format("{0:0.0000}", 0);
                    //TasaInteres = 0;
                    txtMontoPrestamo.Text = String.Format("{0:N2}", txtMonto.Text);
                    //var x = (Convert.ToDouble(txtMonto.Text) / Convert.ToDouble(txtTiempo.Text));
                    //lblCuota.Text = String.Format("{0:N2}", x);
                }
            }
            catch
            {
            }
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

        private void btnAddRoad_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                frmZonas ruta = new frmZonas();
                ruta.Owner = this;
                ruta.Closed += (obj, arg) => { ZonasGet(); };
                ruta.ShowDialog();
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

        private void txtNumero_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNumero.Text))
                {
                    ClearAll();
                }
            }
            catch { }
        }

        private void txtNumero_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter )
                {
                    if (string.IsNullOrEmpty(txtNumero.Text)) { txtNumero.Text = "0"; }
                    if (Convert.ToInt16(txtNumero.Text) > 0)
                    {
                        SolicitudesGetBySolicitudID(int.Parse(txtNumero.Text));
                    }
                    else
                    {
                        frmConsultasSolicitudes Query = new frmConsultasSolicitudes();
                        Query.Owner = this;
                        Query.IsQuery = true;
                        Query._EstadoID = 2;
                        Query.Closed += (obj, arj) => 
                        {
                            if (Query._SolicitudID > 0)
                            {
                                SolicitudesGetBySolicitudID(Query._SolicitudID);
                            }
                            else
                            {
                                txtNumero.Focus();
                            }
                        };
                        Query.Owner = this;
                        Query.ShowDialog();

                    }
                }
            }
            catch(Exception ex) { }
        }

        private void SolicitudesGetBySolicitudID(int ID)
        {
            try
            {
                if (ID > 0)
                {
                    var BE = db.SolicitudesGetBySolicitudID(ID);
                    if (BE != null)
                    {
                        switch (BE.EstadoID)
                        {
                            case 1:
                                {
                                    clsMessage.ErrorMessage(string.Format("El numero de solicitud #{0} fue cancelada anteriormente.", BE.SolicitudID), clsLenguajeBO.Find("msgTitle"));
                                }
                                break;
                            case 3:
                                {
                                    clsMessage.ErrorMessage(string.Format("El numero de solicitud #{0} tiene estatus aprobada.", BE.SolicitudID), clsLenguajeBO.Find("msgTitle"));
                                }
                                break;
                            case 2:
                                {
                                    gridSolicitudes.DataContext = BE;
                                    _SolicitudID = BE.SolicitudID;
                                    gridDatosPersonales.DataContext = BE.Clientes.Personas;
                                    gridDatosEconomicos.DataContext = BE.Clientes.Personas.DatosEconomicos;
                                    gridContactoEmergencia.DataContext = BE.Clientes.Personas.Referencias;
                                    cmbSucursales.SelectedValue = BE.Clientes.SucursalID;
                                    OficialesGetBySucursalID();

                                    CalcularContratos();
                                    _Referencias = BE.Clientes.Personas.Referencias.ToList();
                                    //gridDatosGarantiaHipotecario.DataContext = BE.GarantiaHipotecaria;
                                    //gridDatosGarantiaVehiculo.DataContext = BE.GarantiaVehiculos;
                                    LoadConfig();
                                    //gridDatosGarantiaPersonal.DataContext = BE.GarantiaPersonal.Garantes.Personas;
                                    CalcularLegal();
                                    CalcularSeguro();


                                    switch (BE.TipoSolicitudID)
                                    {
                                        case 1:
                                            {
                                                _DocumentoGarante = BE.GarantiaPersonal.Documento;
                                                gridDatosGarantiaPersonal.DataContext = BE.GarantiaPersonal.Garantes.Personas;
                                            }
                                            break;
                                        case 2:
                                            {
                                                gridDatosGarantiaHipotecario.DataContext = BE.GarantiaHipotecaria;
                                            }
                                            break;
                                        case 3:
                                            {
                                                gridDatosGarantiaVehiculo.DataContext = BE.GarantiaVehiculos;
                                            }
                                            break;
                                    }

                                }
                                break;
                            default:
                                {
                                    clsMessage.ErrorMessage("Numero de solicitud no encontrado.", clsLenguajeBO.Find("msgTitle"));
                                }
                                break;
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage("Numero de solicitud no encontrado.", clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage("Numero de solicitud no encontrado.", clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }


        private void cmbCondicionSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               // LoadConfig();
            }
            catch { }
        }

        private void LoadConfig()
        {
            try
            {
                bool DataCredito = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).DataCredito == true ? true : false;
                bool MoraAutomatica = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).MoraAutomatica == true ? true : false;
                bool LetterDelay = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).LetterDelay == true ? true : false;
                bool IsLegal = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).IsLegal == true ? true : false;
                bool IsSeguro = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).IsSeguro == true ? true : false;
                bool InteresDiario = (cmbTipoContratos.SelectedItem as clsTipoContratosBE).InteresDiario == true ? true : false;
                bool MoraDiaria = (cmbTipoContratos.SelectedItem as clsTipoContratosBE).MoraDiaria == true ? true : false;

                txtInteres.Text = string.Format("{0:0.000}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Interes);
                txtComision.Text = string.Format("{0:0.000}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Comision);
                txtLegal.Text = string.Format("{0:0.000}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Legal);
                txtTasaSeguro.Text = string.Format("{0:0.000}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Seguro);

                if ((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).MoraAutomatica == true)
                {
                    txtMoras.Text = string.Format("{0:N2}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Mora);
                    txtDiasGracia.Text = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).DiasGracia.ToString();
                }
                else
                {
                    txtMoras.Text = string.Format("{0:N2}", 0);
                    txtDiasGracia.Text = "0";
                }

                if (InteresDiario == true)
                {
                    rbInteresDiario_Si.IsChecked  = true;
                }
                else
                {
                    rbInteresDiario_No.IsChecked = true;
                }

                if (MoraDiaria == true)
                {
                    rbMoraDiaria_Si .IsChecked = true;
                }
                else
                {
                    rbMoraDiaria_No.IsChecked = true;
                }

                if (DataCredito == true)
                { rbDataCredito_Si.IsChecked = DataCredito; }
                else { rbDataCredito_No.IsChecked = true; }

                if (MoraAutomatica == true)
                { rbMorasAut_Si.IsChecked = MoraAutomatica; }
                else { rbMorasAut_No.IsChecked = true; }

                if (LetterDelay == true)
                { rbCartas_Si.IsChecked = true; }
                else { rbCartas_No.IsChecked = true; }

                if (IsLegal == true)
                { rbLegal_Si.IsChecked = true; }
                else { rbLegal_No.IsChecked = true; }

                if (IsSeguro == true)
                { rbInsurance_Si.IsChecked = true; }
                else { rbInsurance_No.IsChecked = true; }

                if ((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).IsGenerateRequest == true)
                {
                    rbBankRequest_Si.IsChecked = true;

                    if ((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).BancoID > 0)
                    {
                        cmbBancos.SelectedValue = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).BancoID;
                    }
                }
                else
                {
                    rbBankRequest_No.IsChecked = true;
                }

                chkDocument.IsChecked = clsCookiesBO.getPrintContract();

                // gridContratos.DataContext = new clsContratosBE { OficialID = -1, Fecha = (DateTime)txtFechaContrato.SelectedDate, Interes = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Interes , Comision = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Comision };
            }
            catch { }
        }

        private void txtMontoPrestamo_KeyDown(object sender, KeyEventArgs e)
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

        private void cmbTipoContratos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
                if (VISTA == 1) { LoadConfig(); }
            }
            catch { }
        }

        private void rbInsurance_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTasaSeguro.IsEnabled = false;
                lblSeguro.IsEnabled = false;
                chkSeguro.IsEnabled = false;
                chkSeguro.IsChecked = false;
                txtTasaSeguro.Text = string.Format("{0:N2}", 0);
                lblSeguro.Text = string.Format("{0:N2}", 0);
            }
            catch { }
        }

        private void rbInsurance_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTasaSeguro.IsEnabled = true;
                lblSeguro.IsEnabled = true;
                chkSeguro.IsEnabled = true;
                chkSeguro.IsChecked = false;
                txtTasaSeguro.Focus();
            }
            catch { }
        }

        private void rbLegal_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtLegal.IsEnabled = false;
                lblLegal.IsEnabled = false;
                chkCierre.IsEnabled = false;
                txtLegal.Text = string.Format("{0:N2}", 0);
                lblLegal.Text = string.Format("{0:N2}", 0);
            }
            catch { }
        }

        private void rbLegal_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtLegal.IsEnabled = true;
                lblLegal.IsEnabled = true;
                chkCierre.IsEnabled = true;
                txtLegal.Focus();
            }
            catch { }
        }

        private void rbMorasAut_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMoras.IsEnabled = false;
                txtDiasGracia.IsEnabled = false;
                txtMoras.Text = string.Format("{0:N2}", 0);
                txtDiasGracia.Text = string.Format("{0:0}", 0);
            }
            catch { }
        }

        private void rbMorasAut_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMoras.IsEnabled = true;
                txtDiasGracia.IsEnabled = true;
            }
            catch { }
        }

        private void txtInteres_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (chkCuotas.IsChecked == false)
                {
                    CalcularContratos();
                }
            }
            catch { }
        }

        private void txtInteres_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double interes = double.Parse(txtInteres.Text);
                txtInteres.Text = interes.ToString(); //String.Format("{0:0.000}", interes);
            }
            catch { }
        }

        private void txtComision_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
            }
            catch { }
        }

        private void txtComision_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double comision = double.Parse(txtComision.Text);
                txtComision.Text = comision.ToString(); //string.Format("{0:0.000}", comision);
            }
            catch { }
        }

        
        private void CalcularFecha()
        {
            try
            {
                DateTime Fecha;
                Fecha = (DateTime)txtFechaContrato.SelectedDate;
                int Tiempo = Convert.ToInt16(txtTiempo.Text);  
                switch ((int)cmbCondicionSolicitudes.SelectedValue)
                {
                    case 1:
                        {
                            Vence = Fecha.AddMonths(Tiempo);
                        }
                        break;
                    case 2:
                        {
                            Vence = Fecha.AddDays((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Dias * Tiempo);
                        }
                        break;
                    case 3:
                        {
                            Vence = Fecha.AddDays((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Dias * Tiempo);
                        }
                        break;
                    case 4:
                        {
                            Vence = Fecha.AddDays((cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Dias * Tiempo);
                        }
                        break;
                }                
           }
            catch { }
        }

        private void CalcularContratos()
        {
            try
            {
                if (double.Parse(txtMonto.Text) > 0)
                {

                    double MontoCapital = 0;
                    double MontoComision = 0;
                    double MontoInteres = 0;
                    double ACapital = 0;
                    double AComision = 0;
                    double AInteres = 0;
                    double Cuotas = 0;
                    double Monto = 0;


                    int IdTipoContrato = (int)cmbTipoContratos.SelectedValue;
                    Boolean IsComision = rbComision_Si.IsChecked == true ? true : false;
                    clsCondicionesBE Condiciones = (clsCondicionesBE)cmbCondicionSolicitudes.SelectedItem;
                    double Interes = Convert.ToDouble(txtInteres.Text);
                    double Comision = Convert.ToDouble(txtComision.Text);
                    double Tiempo = Convert.ToDouble(txtTiempo.Text);

                    if (IsComision == true)
                    {
                        Monto = Convert.ToDouble(txtMonto.Text);
                    }
                    else
                    {
                        Monto = Convert.ToDouble(txtMonto.Text) + (Convert.ToDouble(txtMonto.Text) * (Convert.ToDouble(txtComision.Text) / 100));
                    }

                    MontoCapital = (Convert.ToDouble(Monto) / Convert.ToDouble(txtTiempo.Text));

                    if (IsComision == true)
                    {
                        MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100))); //(Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                    }
                    else
                    {
                        MontoComision = 0;
                    }

                    if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                    {
                        MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;

                    }
                    else
                    {
                        MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;
                    }

                    lblCuota.Text = string.Format("{0:N2}", Math.Round(MontoCapital + MontoComision + MontoInteres, 0));
                    Cuota = Math.Round(MontoCapital + MontoComision + MontoInteres, 0);

                    txtMontoPrestamo.Text = string.Format("{0:N2}", Convert.ToDouble(txtTiempo.Text) * Math.Round(MontoCapital + MontoComision + MontoInteres, 0));
                    CalcularFecha();

                    if ((int)cmbTipoContratos.SelectedValue == 1)
                    {
                        double Factor1 = 0, Factor2 = 0, Factor = 0;
                        Interes = 0; Tiempo = 0;
                        Interes = Convert.ToDouble(txtInteres.Text) / 100;
                        Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                        Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                        Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                        Factor = Factor1 / Factor2;
                        if (Factor > 0)
                        {
                            lblCuota.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor), 0);
                            txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo / Factor) * Tiempo, 0));
                            Cuota = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0);
                        }
                    }
                    else
                    {
                        if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                        {
                            double a = 0;
                            if (this.rbComision_Si.IsChecked == true)
                            {
                                a = Math.Round(MontoComision + MontoInteres, 0);
                            }
                            else
                            {
                                a = Math.Round(MontoInteres, 0);
                            }
                            lblCuota.Text = string.Format("{0:N2}", a);
                            Cuota = a;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CalcularContratos1()
        {
            try
            {
                if (double.Parse(txtMonto.Text) > 0)
                {
                    double MontoCapital = 0;
                    double MontoComision = 0;
                    double MontoInteres = 0;

                    MontoCapital = (Convert.ToDouble(txtMonto.Text) / Convert.ToDouble(txtTiempo.Text));
                    MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100))); //(Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                    MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses));

                    lblCuota.Text = string.Format("{0:N2}", Math.Round(MontoCapital + MontoComision + MontoInteres));
                    Cuota = Math.Round(MontoCapital + MontoComision + MontoInteres);

                    txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(Convert.ToDouble(txtTiempo.Text) * (MontoCapital + MontoComision + MontoInteres)));
                    CalcularFecha();

                    if ((int)cmbTipoContratos.SelectedValue == 1)
                    {
                        //double Factor1 = 0, Factor2 = 0, Factor = 0;
                        //double Interes = 0; double Tiempo = 0;
                        //Interes = Convert.ToDouble(txtInteres.Text) / 100;
                        //Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                        //Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                        //Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                        //Factor = Factor1 / Factor2;
                        //if (Factor > 0)
                        //{
                        //    lblCuota.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor), 0);
                        //    txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo / Factor) * int.Parse(txtTiempo.Text), 0));
                        //    Cuota = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor,0);
                        //}

                        double Factor1 = 0, Factor2 = 0, Factor = 0;
                        double Interes = 0; double Tiempo = 0;
                        Interes = Convert.ToDouble(txtInteres.Text) / 100;
                        Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                        Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                        Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                        Factor = Factor1 / Factor2;
                        if (Factor > 0)
                        {
                            Cuota = float.Parse(Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0).ToString());
                        }
                    }
                    else
                    {
                        if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                        {
                            double a = 0;
                            if (this.rbComision_Si.IsChecked == true)
                            {
                                a = Math.Round(MontoComision + MontoInteres,0);
                            }
                            else
                            {
                                a = Math.Round(MontoInteres,0);
                            }
                            lblCuota.Text = string.Format("{0:N2}", a);
                            Cuota = a;
                        }
                    }

                    //MontoCapital = (Convert.ToDouble(txtMonto.Text) / Convert.ToDouble(txtTiempo.Text));

                   // double Tasa = 0;
                    //TasaInteres = (float)(((Convert.ToDouble(lblCuota.Text) - MontoCapital) / (MontoCapital * Convert.ToDouble(txtTiempo.Text))) * (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses) * 100;
                }
            }
            catch
            {
            }
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

        private void txtDocumentoGarante_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Documento = txtDocumentoGarante.Text;
                txtDocumentoGarante.Text = clsValidacionesBO.DocumentFormat(Documento);
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
                        txtDocumentoGarante.Text = clsValidacionesBO.DocumentFormat(Documento);
                    }
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
                            gridDatosGarantiaPersonal.Visibility = Visibility.Visible;
                            //gridDatosGarantiaPersonal.DataContext = new clsPersonasBE();
                            wpGarantias.Visibility = Visibility.Visible;

                        }
                        break;
                    case 2:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Visible;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaPersonal.Visibility = Visibility.Collapsed;
                            //gridDatosGarantiaHipotecario.DataContext = new clsPersonasBE();
                            wpGarantias.Visibility = Visibility.Visible;
                        }
                        break;
                    case 3:
                        {
                            gridDatosGarantiaHipotecario.Visibility = Visibility.Collapsed;
                            gridDatosGarantiaVehiculo.Visibility = Visibility.Visible;
                            gridDatosGarantiaPersonal.Visibility = Visibility.Collapsed;
                            //gridDatosGarantiaHipotecario.DataContext = new clsPersonasBE();
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

        public void OnInit(int ContratoID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                clsContratosBE row = db.ContratosGetByContratoID(ContratoID);
                _ContratoID = ContratoID;
                _SolicitudID = row.Solicitudes.SolicitudID;
                _ClienteID = row.Solicitudes.Clientes.ClienteID;
                _Documento = row.Solicitudes.Clientes.Documento;


                txtMoras.Text = string.Format("{0:N2}", row.Mora);
                txtDiasGracia.Text = row.DiasGracia.ToString();

                gridContratos.DataContext = row;

                //Solicitud
                gridSolicitudes.DataContext = row.Solicitudes;
                gridDatosPersonales.DataContext = row.Solicitudes.Clientes.Personas;

                if (row.ShowComision == true)
                { rbComision_Si.IsChecked = true; }
                else { rbComision_No.IsChecked = true; }


                if (row.MoraAutomatica == true)
                { rbMorasAut_Si.IsChecked = true; }
                else { rbMorasAut_No.IsChecked = true; }

                if (row.DataCredito == true)
                { rbDataCredito_Si.IsChecked = true; }
                else { rbDataCredito_No.IsChecked = true; }

                if (row.LetterDelay == true)
                { rbCartas_Si.IsChecked = true; }
                else { rbCartas_No.IsChecked = true; }

                if (row.IsLegal == true)
                { rbLegal_Si.IsChecked = true; }
                else { rbLegal_No.IsChecked = true; }

                if (row.IsSeguro == true)
                { rbInsurance_Si.IsChecked = true; }
                else { rbInsurance_No.IsChecked = true; }



                //image.Source = Common.Generic.ByteToImage(Common.Generic.ImageFromText(SolicitudID.ToString()));

                cmbSucursales.SelectedValue = row.Solicitudes.Clientes.SucursalID;
                OficialesGetBySucursalID();

                if (row.Solicitudes.Clientes.Personas.DatosEconomicos.Trabaja == true)
                {
                    rbSi.IsChecked = true;
                }
                else
                {
                    rbNo.IsChecked = true;
                }

                CalcularSeguro();
                CalcularLegal();
                CalcularContratos();

                if (row.InteresDiario == true)
                { rbInteresDiario_Si.IsChecked = true; }
                else { rbInteresDiario_No.IsChecked = true; }

                if (row.MoraDiaria == true)
                { rbMoraDiaria_Si.IsChecked = true; }
                else { rbMoraDiaria_No.IsChecked = true; }

                TasaLegal = row.Legal;
                TasaSeguro = row.Seguro;
                //TasaInteres = row.Interes;

                if (row.SolicitudCheques.Count() > 0)
                {
                    rbBankRequest_Si.IsChecked = true;
                    cmbBancos.SelectedValue = row.SolicitudCheques.FirstOrDefault().BancoID;
                }
                else
                {
                    rbBankRequest_No.IsChecked = true;
                    cmbBancos.SelectedValue = -1;
                }

                gridDatosEconomicos.DataContext = row.Solicitudes.Clientes.Personas.DatosEconomicos;
                dtgContactoEmergencias.ItemsSource = row.Solicitudes.Clientes.Personas.Referencias;
                _Referencias = row.Solicitudes.Clientes.Personas.Referencias.ToList();

                switch (row.Solicitudes.TipoSolicitudID)
                {
                    case 1:
                        {
                            _DocumentoGarante = row.Solicitudes.GarantiaPersonal.Documento;
                            gridDatosGarantiaPersonal.DataContext = row.Solicitudes.GarantiaPersonal.Garantes.Personas;
                        }
                        break;
                    case 2:
                        {
                            gridDatosGarantiaHipotecario.DataContext = row.Solicitudes.GarantiaHipotecaria;
                        }
                        break;
                    case 3:
                        {
                            gridDatosGarantiaVehiculo.DataContext = row.Solicitudes.GarantiaVehiculos;
                        }
                        break;
                }


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
                OperationResult result = db.PersonasCreate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, photo, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    ClientesCreate();
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
                OperationResult result = db.PersonasUpdate(1, txtDocumento.Text, (DateTime)txtFechaNacimiento.SelectedDate, txtNombres.Text, txtApellidos.Text, txtApodo.Text, photo, (int)cmbCiudades.SelectedValue, txtDireccion.Text, txtCorreoElectronico.Text, txtTelefono.Text, (int)cmbOperadores.SelectedValue, txtCelular.Text, (int)cmbSexos.SelectedValue, (int)cmbEstadosCiviles.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    if (VISTA == 1)
                    {
                        ClientesCreate();
                    }
                    else
                    {
                        ClientesUpdate();
                        //DatosEconomicosUpdate();
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
                OperationResult result = db.ClientesCreate(txtDocumento.Text, (int)cmbSucursales.SelectedValue, 0,0, clsVariablesBO.UsuariosBE.Documento);
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

        private void ClientesUpdate()
        {
            try
            {
                OperationResult result = db.ClientesUpdate(_ClienteID,txtDocumento.Text, (int)cmbSucursales.SelectedValue, 0, 0, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
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
               
                if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
                
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private async void NotificarContrato(int ID)
        {
            try
            {
                if (VISTA == 1 &&  clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarContrato == true && (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).SendSMS == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        var row = db.ContratosGetByContratoID(ID);
                        string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarContrato), Common.Generic.ShortName(row.Solicitudes.Clientes.Personas.Nombres), row.Solicitudes.Clientes.Sucursales.Empresas.Empresa, row.Solicitudes.Condiciones.Dias, string.Format("{0:N2}", row.Cuota), row.Solicitudes.Tiempo, row.Solicitudes.Condiciones.Condicion);


                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                        {
                            string[] userID = new string[1];
                            userID[0] = clsVariablesBO.UsuariosBE.Documento;
                            var response = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                          
                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                        {
                            try
                            {
                                var template = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/ApprovedContract.html");
                                string Body = string.Format(template, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, txtNombres.Text + " " + txtApellidos.Text, Mensaje, clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos , clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, DateTime.Now.Year, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Site);

                                Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                ms.SendMail(txtCorreoElectronico.Text, string.Format("Aprobacion Contrato #{0} | {1}", ID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
                            }
                            catch { }
                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                        {
                            if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                            {
                                var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), txtCelular.Text, Mensaje, OperadorID);
                                if (smsResult.ResponseCode != "00")
                                {
                                    clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                }
                                db.SmsCreate(txtCelular.Text, Mensaje, ID, clsVariablesBO.UsuariosBE.Documento);
                            }
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage("La red de mensajería no esta disponible en estos momentos.", clsLenguajeBO.Find("msgTitle"));

                    }
                }
                ClearAll();
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void GarantiasCreate(int ID)
        {
            try
            {
                switch ((int)cmbTipoSolicitudes.SelectedValue)
                {
                    case 1: //Personales
                        {
                            GarantesCreate();
                            db.GarantiaPersonalCreate(ID, _DocumentoGarante, clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    case 2: //Hipotecarios
                        {
                            db.GarantiaHipotecariaCreate(ID, txtDescripcion.Text, float.Parse(txtMontoHipoteca.Text), clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    case 3: //Vehiculos
                        {
                           // db.GarantiaVehiculosCreate(ID, (int)cmbTipoVehiculos.SelectedValue, (int)cmbModelos.SelectedValue, (int)cmbColores.SelectedValue, txtChassis.Text, txtPlaca.Text, txtRegistro.Text, (DateTime)txtFechaExpedicion.SelectedDate, int.Parse(txtAno.Text), clsVariablesBO.UsuariosBE.Documento);
                        }
                        break;
                    default: //Otros
                        {
                            GarantesCreate();
                            db.GarantiaPersonalCreate(ID, _DocumentoGarante, clsVariablesBO.UsuariosBE.Documento);
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
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }



        private void SaveSolicitudesCreate()
        {
            try
            {
                CalcularLegal();
                CalcularSeguro();
                CalcularFecha();  
                
                bool DataCredito = rbDataCredito_Si.IsChecked == true ? true : false;
                bool MoraAutomatica = rbMorasAut_Si.IsChecked == true ? true : false;
                bool LetterDelay = rbCartas_Si.IsChecked == true ? true : false;
                bool IsLegal = rbLegal_Si.IsChecked == true ? true : false;
                bool IsSeguro = rbInsurance_Si.IsChecked == true ? true : false;
                bool InteresDiario = rbInteresDiario_Si.IsChecked == true ? true : false;
                bool MoraDiaria = rbMoraDiaria_Si.IsChecked == true ? true : false;

                //txtInteres.Text = TasaInteres.ToString();

                var count = db.ContratosGet(null, 1, clsVariablesBO.UsuariosBE.Documento).Count();
                if (count <= clsVariablesBO.UsuariosBE.Sucursales.Empresas.Planes.Hasta)
                {
                    OperationResult result = db.ContratosCreate((DateTime)txtFechaContrato.SelectedDate, Vence, _SolicitudID, (int)cmbTipoContratos.SelectedValue, txtActo.Text, float.Parse(txtComision.Text), float.Parse(txtInteres.Text), float.Parse(txtMoras.Text), float.Parse(lblCuota.Text), DataCredito, MoraAutomatica, LetterDelay, Convert.ToInt16(txtDiasGracia.Text), (int)cmbNotarioPublico.SelectedValue, (int)cmbOficiales.SelectedValue, (int)cmbZonas.SelectedValue, IsLegal, TasaLegal, IsSeguro, TasaSeguro, (int)cmbObjetivos.SelectedValue,InteresDiario, MoraDiaria, clsVariablesBO.UsuariosBE.Documento);
                    if (result.ResponseCode == "00")
                    {
                        CuotasCreateAutomatica(Convert.ToInt32(result.ResponseMessage), clsVariablesBO.UsuariosBE.Documento, true);
                        var r = db.ContratosGetByContratoID(Convert.ToInt32(result.ResponseMessage));

                        if (rbBankRequest_Si.IsChecked == true && (int)cmbBancos.SelectedValue != -1)
                        {
                            if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsMultipleRequest == false)
                            {
                                float _monto = (float.Parse(txtMonto.Text) * (((float.Parse(txtComision.Text)) / 100) * (float.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                                var response = db.SolicitudChequesCreate((DateTime)txtFechaContrato.SelectedDate, r.ContratoID, (int)cmbBancos.SelectedValue, r.Solicitudes.Clientes.Personas.Nombres + " " + r.Solicitudes.Clientes.Personas.Apellidos, "CONTRATO " + (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud.ToUpper() + " #" + r.ContratoID.ToString(), float.Parse(txtMonto.Text) + _monto, clsVariablesBO.UsuariosBE.Documento);
                                if (response.ResponseCode == "00")
                                {
                                    //float Descuento = (float.Parse(lblSeguro.Text) + float.Parse(lblLegal.Text));
                                    db.DetalleSolicitudChequesCreate(int.Parse(response.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), float.Parse(txtMonto.Text), 0, clsVariablesBO.UsuariosBE.Documento);
                                    db.DetalleSolicitudChequesCreate(int.Parse(response.ResponseMessage), (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, 0, float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);

                                    GetReport(int.Parse(response.ResponseMessage));
                                }
                            }
                            else
                            {
                                float _monto = (float.Parse(txtMonto.Text) * (((float.Parse(txtComision.Text)) / 100) * (float.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                                var response = db.SolicitudChequesCreate((DateTime)txtFechaContrato.SelectedDate, r.ContratoID, (int)cmbBancos.SelectedValue, r.Solicitudes.Clientes.Personas.Nombres + " " + r.Solicitudes.Clientes.Personas.Apellidos, "CONTRATO " + (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud.ToUpper() + " #" + r.ContratoID.ToString(), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);

                                db.DetalleSolicitudChequesCreate(int.Parse(response.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), float.Parse(txtMonto.Text), 0, clsVariablesBO.UsuariosBE.Documento);
                                db.DetalleSolicitudChequesCreate(int.Parse(response.ResponseMessage), (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, 0, float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);

                                GetReport(int.Parse(response.ResponseMessage));

                                if (_monto > 0)
                                {
                                    db.SolicitudChequesCreate((DateTime)txtFechaContrato.SelectedDate, r.ContratoID, (int)cmbBancos.SelectedValue, r.Solicitudes.Clientes.Personas.Nombres + " " + r.Solicitudes.Clientes.Personas.Apellidos, "COMISION CONTRATO " + (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud.ToUpper() + " #" + r.ContratoID.ToString(), _monto, clsVariablesBO.UsuariosBE.Documento);

                                    db.DetalleSolicitudChequesCreate(int.Parse(response.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), _monto, 0, clsVariablesBO.UsuariosBE.Documento);
                                    db.DetalleSolicitudChequesCreate(int.Parse(response.ResponseMessage), (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, 0, _monto, clsVariablesBO.UsuariosBE.Documento);

                                    GetReport(int.Parse(response.ResponseMessage));
                                }
                            }
                        }
                        else
                        {
                            var response = db.EntradasCreate((DateTime)txtFechaContrato.SelectedDate, 1, clsVariablesBO.UsuariosBE.SucursalID, $"ED{txtFechaContrato.SelectedDate.Value.Day}{txtFechaContrato.SelectedDate.Value.Month}{txtFechaContrato.SelectedDate.Value.Year}", $"DESEMBOLSO EN EFECTIVO CONTRATO #{r.ContratoID} - {r.Solicitudes.Clientes.Personas.Nombres.ToUpper()} {r.Solicitudes.Clientes.Personas.Apellidos.ToUpper()}", float.Parse(txtMonto.Text), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                            if(response.ResponseCode == "00")
                            {
                                var xml = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Contabilizaciones.Where(x => x.ContabilizacionID == 100005).FirstOrDefault().Xml;
                                XDocument doc = XDocument.Parse(xml);
                                XElement Nodo = doc.Root.Element("settings");
                                var auxiliarID = db.AuxiliaresGetByAuxilarID(int.Parse(Nodo.Element("Credito").Value)).AuxiliarID;

                                if (response.ResponseCode == "00")
                                {
                                    db.DetalleEntradasCreate(int.Parse(response.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), 1, r.ContratoID, float.Parse(txtMonto.Text), 0, clsVariablesBO.UsuariosBE.Documento);
                                    db.DetalleEntradasCreate(int.Parse(response.ResponseMessage), auxiliarID, 1, r.ContratoID, 0, float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);

                                }
                            }
                        }

                        ContratosGet(r);
                    }
                    else
                    {
                        clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                }

            }
            catch (Exception sqlEx) 
            { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }


        void GetReport(int ID)
        {
            try
            {
                frmPrintSolicitudChequesGetBySolicitudChequeID Solicitud = new frmPrintSolicitudChequesGetBySolicitudChequeID();
                Solicitud.OnInit(ID);
                Solicitud.ShowDialog();
            }
            catch { ClearAll(); }
        }

        private void ContratosGet(clsContratosBE C)
        {
            try
            {
                if (C.ContratoID > 0)
                {

                    if ((bool)chkDocument.IsChecked)
                    {
                        switch (C.Solicitudes.TipoSolicitudID)
                        {

                            case 2:
                                {
                                    frmPrintContratosHipotecariosGetByContratoID Hipoteca = new frmPrintContratosHipotecariosGetByContratoID();
                                    Hipoteca.OnInit(C);
                                    Hipoteca.Owner = this;
                                    Hipoteca.ShowDialog();

                                    frmPrintContratosVentaHipotecariosGetByContratoID Venta = new frmPrintContratosVentaHipotecariosGetByContratoID();
                                    Venta.OnInit(C);
                                    Venta.Owner = this;
                                    Venta.ShowDialog();

                                    frmPrintActoEntregaVoluntariaGetByContratoID entrega = new frmPrintActoEntregaVoluntariaGetByContratoID();
                                    entrega.OnInit(C);
                                    entrega.Owner = this;
                                    entrega.ShowDialog();
                                }
                                break;
                            case 3:
                                {
                                    frmPrintContratosVehiculosGetByContratoID Vehiculos = new frmPrintContratosVehiculosGetByContratoID();
                                    Vehiculos.OnInit(C);
                                    Vehiculos.Owner = this;
                                    Vehiculos.ShowDialog();

                                    frmPrintActoEntregaVoluntariaGetByContratoID entrega = new frmPrintActoEntregaVoluntariaGetByContratoID();
                                    entrega.OnInit(C);
                                    entrega.Owner = this;
                                    entrega.ShowDialog();

                                    frmPrintCartaOposicionVehiculosGetByContratoID oposicion = new frmPrintCartaOposicionVehiculosGetByContratoID();
                                    oposicion.OnInit(C);
                                    oposicion.Owner = this;
                                    oposicion.ShowDialog();
                                }
                                break;

                            case 6:
                                {

                                    frmPrintActoEntregaVoluntariaGetByContratoID entrega = new frmPrintActoEntregaVoluntariaGetByContratoID();
                                    entrega.OnInit(C);
                                    entrega.Owner = this;
                                    entrega.ShowDialog();
                                }
                                break;
                        }

                        frmPrintContratosGetByContratoID Cuotas = new frmPrintContratosGetByContratoID();
                        Cuotas.OnInit(C, false);
                        Cuotas.Owner = this;
                        Cuotas.ShowDialog();

                        frmPrintContratosAutenticoGetByContratoID Acto = new frmPrintContratosAutenticoGetByContratoID();
                        Acto.OnInit(C, true);
                        Acto.Owner = this;
                        Acto.ShowDialog();

                        frmPrintCuotasGetByContratoID Contrato = new frmPrintCuotasGetByContratoID();
                        Contrato.OnInit(C.ContratoID);
                        Contrato.Owner = this;
                        Contrato.ShowDialog();
                    }
                    else
                    {
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    }

                    NotificarContrato(C.ContratoID);
                    
                    
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); ClearAll(); }
        }


        private OperationResult CuotasCreateAutomatica(int _ContratoID, string Usuario, bool save)
        {
            try
            {

            List<clsCuotasBE> lstCuotas = new List<clsCuotasBE>();

            double MontoCapital = 0;
            double MontoComision = 0;
            double MontoInteres = 0;
            double ACapital = 0;
            double AComision = 0;
            double AInteres = 0;
            double Cuotas = 0;
            double Monto = 0;


            int IdTipoContrato = (int)cmbTipoContratos.SelectedValue;
            Boolean IsComision = rbComision_Si.IsChecked == true ? true : false;
            clsCondicionesBE Condiciones = (clsCondicionesBE)cmbCondicionSolicitudes.SelectedItem;
            double Interes = Convert.ToDouble(txtInteres.Text);
            double Comision = Convert.ToDouble(txtComision.Text);
            double Tiempo = Convert.ToDouble(txtTiempo.Text);


                if (IsComision == true)
                {
                    Monto = Convert.ToDouble(txtMonto.Text);
                }
                else
                {
                    Monto = Convert.ToDouble(txtMonto.Text) + (Convert.ToDouble(txtMonto.Text) * (Convert.ToDouble(txtComision.Text) / 100));
                }

                MontoCapital = (Convert.ToDouble(Monto) / Convert.ToDouble(txtTiempo.Text));
                if (IsComision == true)
                {
                    MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100)));
                }
                else
                {
                    MontoComision = 0;
                }

                if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                {
                    MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;

                }
                else
                {
                    MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;
                }

                Cuota = Math.Round(MontoCapital + MontoComision + MontoInteres);

                txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(Convert.ToDouble(txtTiempo.Text) * (MontoCapital + MontoComision + MontoInteres)));


                double Factor1 = 0, Factor2 = 0, Factor = 0;
                if ((int)cmbTipoContratos.SelectedValue == 1)
                {

                    //double Interes = 0; double Tiempo = 0;
                    Interes = Convert.ToDouble(txtInteres.Text) / 100;
                    Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                    Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                    Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                    Factor = Factor1 / Factor2;
                    if (Factor > 0)
                    {
                        lblCuota.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor), 0);
                        txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo / Factor) * int.Parse(txtTiempo.Text), 0));
                        Cuota = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0);
                        Cuotas = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0);
                    }
                }
                else
                {
                    if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                    {
                        double a = 0;
                        if (this.rbComision_Si.IsChecked == true)
                        {
                            a = Math.Round(MontoComision + MontoInteres, 0);
                        }
                        else
                        {
                            a = Math.Round(MontoInteres, 0);
                        }
                        lblCuota.Text = string.Format("{0:N2}", a);
                        Cuota = a;
                    }
                }


                int FormaPagoID = 1 ;
            if ((float.Parse(lblSeguro.Text) + float.Parse(lblLegal.Text)) > 0 && (int)cmbBancos.SelectedValue > 0)
            {
                //db = new Manager();
                //int ID = db.FormaPagosGetByAuxiliarID((int)cmbBancos.SelectedValue).FormaPagoID;
                FormaPagoID = 3;
            }

            System.DateTime Fecha;
            Fecha = (DateTime)txtFechaContrato.SelectedDate;
            int dia = Fecha.Day;
            DateTime _fecha = Fecha;
            DateTime _tmp = Fecha;
            int residuo = 0; 

            for (int i = 1; i <= Convert.ToInt16(txtTiempo.Text); i++)
            {

                db = new Manager();
                switch ((int)cmbCondicionSolicitudes.SelectedValue)
                {
                    #region Mensual
                    case 1:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                             AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = Math.Round( ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses);
                                        ACapital = Cuota - AInteres - AComision;
                                        Monto = Monto - ACapital;

                                        var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, (float)ACapital, (float)AComision, (float)AInteres, 0, 0, 0, 0, Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA==1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value,0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / Condiciones.Meses));
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request  = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                        if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                        {
                                                            string _concepto = string.Empty;
                                                            if (float.Parse(lblSeguro.Text) > 0)
                                                            {
                                                                _concepto = "SEGURO DE VIDA";
                                                                db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                            }

                                                            if (float.Parse(lblLegal.Text) > 0)
                                                            {
                                                                if (string.IsNullOrEmpty(_concepto))
                                                                {
                                                                    _concepto = "GASTOS DE CIERRE ";
                                                                }
                                                                else
                                                                {
                                                                    _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                                }
                                                                db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                            }

                                                            float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                                var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                                if (row.ResponseCode == "00")
                                                                {
                                                                    db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                    db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                    frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                    Recibos.Owner = this;
                                                                    Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                    Recibos.ShowDialog();
                                                                }
                                                            }
                                                        }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                                var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                                if (row.ResponseCode == "00")
                                                                {
                                                                    db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                    db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                    frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                    Recibos.Owner = this;
                                                                    Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                    Recibos.ShowDialog();
                                                                }
                                                            }
                                                        }
                                                }
                                            }
                                        }

                                    }
                                    break;
                                case 5:
                                    {
                                        MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / Condiciones.Meses));
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                    if (i == 1 && save == true)
                                                    {
                                                        if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                        {
                                                            string _concepto = string.Empty;
                                                            if (float.Parse(lblSeguro.Text) > 0)
                                                            {
                                                                _concepto = "SEGURO DE VIDA";
                                                                db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                            }

                                                            if (float.Parse(lblLegal.Text) > 0)
                                                            {
                                                                if (string.IsNullOrEmpty(_concepto))
                                                                {
                                                                    _concepto = "GASTOS DE CIERRE ";
                                                                }
                                                                else
                                                                {
                                                                    _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                                }
                                                                db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                            }

                                                            float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                                var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                                if (row.ResponseCode == "00")
                                                                {
                                                                    db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                    db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                    frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                    Recibos.Owner = this;
                                                                    Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                    Recibos.ShowDialog();
                                                                }
                                                            }
                                                        }
                                                    }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                                var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                                if (row.ResponseCode == "00")
                                                                {
                                                                    db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                    db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                    frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                    Recibos.Owner = this;
                                                                    Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                    Recibos.ShowDialog();
                                                                }
                                                            }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    break;
                                default:
                                    {
                                        
                                        var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, (float)MontoCapital, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                    float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                    if (_value > 0 && VISTA == 1)
                                                    {
                                                        var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                        if (row.ResponseCode == "00")
                                                        {
                                                            db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                            db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                            frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                            Recibos.Owner = this;
                                                            Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                            Recibos.ShowDialog();
                                                        }
                                                    }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Quincenal
                    case 2:
                        {  
                            if(dia == 1 || dia == 15)
                            {
                                int DayOfMonth = DateTime.DaysInMonth(_tmp.Year, _tmp.Month);
                                switch(DayOfMonth)
                                {
                                    case 28:
                                        {
                                                if (_fecha.Day == 15)
                                                {
                                                    _fecha = new DateTime(_tmp.Year, _tmp.Month, 28);

                                                }
                                                else
                                                {
                                                    _fecha = new DateTime(_tmp.Year, _tmp.Month, 15);
                                                    residuo = -2;
                                                }
                                        }
                                        break;

                                    case 29: 
                                        {
                                            if (_fecha.Day == 15)
                                            {
                                                _fecha = new DateTime(_tmp.Year, _tmp.Month, 29);
                                            }
                                            else
                                            {
                                                _fecha = new DateTime(_tmp.Year, _tmp.Month, 15);
                                                    residuo = -1;
                                            }
                                        } break;

                                    case 31: 
                                        {
                                            if (_fecha.Day == 15)
                                            {
                                                _fecha = new DateTime(_tmp.Year, _tmp.Month, 30);
                                            }
                                            else
                                            {
                                                _fecha = new DateTime(_tmp.Year, _tmp.Month, 15);
                                            }
                                        } break;

                                    default: 
                                        {
                                            if (_fecha.Day == 15)
                                            {
                                                _fecha = new DateTime(_tmp.Year, _tmp.Month, 30);
                                            }
                                            else
                                            {
                                                _fecha = new DateTime(_tmp.Year, _tmp.Month, 15);
                                            }
                                        } break;
                                }

                                _tmp = _fecha.AddDays(Condiciones.Dias+ residuo);
                                    residuo = 0;
                            }
                            else
                            {
                                _fecha = Fecha.AddDays(Condiciones.Dias * i);
                            }

                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                                AComision = MontoComision ;
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }

                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - (AInteres + AComision);

                                        Monto = Monto - ACapital;

                                        //var request =   db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, (float)ACapital, (float)AComision, (float)AInteres, 0, 0, 0,  Usuario);
                                        var request = db.CuotasCreate(i, _fecha, _ContratoID, (float)ACapital, (float)AComision, (float)AInteres, 0, 0, 0, 0, Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                    if (_value > 0 && VISTA == 1)
                                                    {
                                                        var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                        if (row.ResponseCode == "00")
                                                        {
                                                            db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                            db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                            frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                            Recibos.Owner = this;
                                                            Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                            Recibos.ShowDialog();
                                                        }
                                                    }
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / Condiciones.Meses));
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, _fecha, _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                               var request = db.CuotasCreate(i, _fecha, _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                        if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                        {
                                                            string _concepto = string.Empty;
                                                            if (float.Parse(lblSeguro.Text) > 0)
                                                            {
                                                                _concepto = "SEGURO DE VIDA";
                                                                db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                            }

                                                            if (float.Parse(lblLegal.Text) > 0)
                                                            {
                                                                if (string.IsNullOrEmpty(_concepto))
                                                                {
                                                                    _concepto = "GASTOS DE CIERRE ";
                                                                }
                                                                else
                                                                {
                                                                    _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                                }
                                                                db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                            }

                                                            float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                                var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                                if (row.ResponseCode == "00")
                                                                {
                                                                    db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                    db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                    frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                    Recibos.Owner = this;
                                                                    Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                    Recibos.ShowDialog();
                                                                }
                                                            }
                                                        }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, _fecha, _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0,   Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / Condiciones.Meses));
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, _fecha, _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request = db.CuotasCreate(i, _fecha, _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, _fecha, _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                            if (_value > 0 && VISTA == 1)
                                                            {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        var request = db.CuotasCreate(i, _fecha, _ContratoID, (float)MontoCapital, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                    if (_value > 0 && VISTA == 1)
                                                    {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Semanal
                    case 3:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                                AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;
 
                                        var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, (float)ACapital, (float)AComision, (float)AInteres, 0, 0, 0, 0,  Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA==1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / Condiciones.Meses));
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / Condiciones.Meses));
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, (float)MontoCapital, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA == 1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Diario
                    case 4:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                                AComision = MontoComision;
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;
                                        var request = db.CuotasCreate(i, Fecha.AddDays(i), _ContratoID, (float)ACapital, (float)AComision, (float)AInteres, 0, 0, 0, 0,  Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA==1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i ) + 4), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                               var request = db.CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i ) + 4), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i ) + 4), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        var request = db.CuotasCreate(i, Fecha.AddDays(i), _ContratoID, (float)MontoCapital, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0,  Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA==1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Default
                    default:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                            AComision = MontoComision;
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;
                                        var request = db.CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, (float)ACapital, (float)AComision, (float)AInteres, 0, 0, 0, 0, Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA==1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                               var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request =  db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i ), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, (float)Monto, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var request = db.CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                                if (i == 1 && save == true)
                                                {
                                                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                                    {
                                                        string _concepto = string.Empty;
                                                        if (float.Parse(lblSeguro.Text) > 0)
                                                        {
                                                            _concepto = "SEGURO DE VIDA";
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        if (float.Parse(lblLegal.Text) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(_concepto))
                                                            {
                                                                _concepto = "GASTOS DE CIERRE ";
                                                            }
                                                            else
                                                            {
                                                                _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                            }
                                                            db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                        }

                                                        float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                        if (_value > 0 && VISTA==1)
                                                        {
                                                            var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                            if (row.ResponseCode == "00")
                                                            {
                                                                db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                                db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                                frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                                Recibos.Owner = this;
                                                                Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                                Recibos.ShowDialog();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        var request = db.CuotasCreate(i, Fecha.AddDays(i), _ContratoID, (float)MontoCapital, (float)MontoComision, (float)MontoInteres, 0, 0, 0, 0, Usuario);
                                        if (i == 1 && save == true)
                                        {
                                            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                                            {
                                                string _concepto = string.Empty;
                                                if (float.Parse(lblSeguro.Text) > 0)
                                                {
                                                    _concepto = "SEGURO DE VIDA";
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "SEGURO DE VIDA CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, 0, float.Parse(lblSeguro.Text), true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                if (float.Parse(lblLegal.Text) > 0)
                                                {
                                                    if (string.IsNullOrEmpty(_concepto))
                                                    {
                                                        _concepto = "GASTOS DE CIERRE ";
                                                    }
                                                    else
                                                    {
                                                        _concepto = _concepto + " Y GASTOS DE CIERRE ";
                                                    }
                                                    db.DetalleNotaDebitosCreate(int.Parse(request.ResponseMessage), "GASTOS DE CIERRE CONTRATO #" + string.Format("{0:000000}", _ContratoID), 0, 0, 0, 0, float.Parse(lblLegal.Text), 0, true, clsVariablesBO.UsuariosBE.Documento);
                                                }

                                                float _value = (float.Parse(lblLegal.Text) + float.Parse(lblSeguro.Text));
                                                if (_value > 0 && VISTA==1)
                                                {
                                                    var row = db.RecibosCreate(_ContratoID, DateTime.Now, (int)cmbSucursales.SelectedValue, 0, FormaPagoID, _value, 0, _value, 0, 0, "", clsVariablesBO.UsuariosBE.Documento);
                                                    if (row.ResponseCode == "00")
                                                    {
                                                        db.DetalleRecibosCreate(int.Parse(request.ResponseMessage), int.Parse(row.ResponseMessage), 1, _concepto, 0, 0, 0, 0, float.Parse(lblLegal.Text), float.Parse(lblSeguro.Text), _value, clsVariablesBO.UsuariosBE.Documento);
                                                        db.Contabilizar(2, int.Parse(row.ResponseMessage), Usuario);

                                                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                                                        Recibos.Owner = this;
                                                        Recibos.OnInit(int.Parse(row.ResponseMessage));
                                                        Recibos.ShowDialog();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                        #endregion
                }
            }

                //db.CuotasGroupCreate(lstCuotas);

                //if (_ContratoID > 0)
                //{
                //    ContratosGet();
                //}

                return new OperationResult { ResponseCode = "00", ResponseMessage = "" };

            }
            catch(Exception ex)
            {
                return new OperationResult { ResponseCode = "01", ResponseMessage =ex.Message };
            }
        }

        void ClearAll()
        {
            try
            {
                txtActo.Text = string.Empty;
                clsValidacionesBO.Limpiar(gridSolicitudes);
                clsValidacionesBO.Limpiar(gridDatosPersonales);
                clsValidacionesBO.Limpiar(gridDatosEconomicos);
                clsValidacionesBO.Limpiar(gridDatosGarantiaHipotecario);
                clsValidacionesBO.Limpiar(gridDatosGarantiaPersonal);
                clsValidacionesBO.Limpiar(gridDatosGarantiaVehiculo);
                clsValidacionesBO.Limpiar(gridContratos);

                gridSolicitudes.DataContext = new clsSolicitudesBE();
                gridDatosPersonales.DataContext = new clsPersonasBE();
                gridDatosEconomicos.DataContext = new clsDatosEconomicosBE();
                gridDatosGarantiaHipotecario.DataContext = new clsGarantiaHipotecariaBE();
                gridDatosGarantiaVehiculo.DataContext = new clsGarantiaVehiculosBE();
                gridDatosGarantiaPersonal.DataContext = new clsGarantiaPersonalBE();
                //gridContratos.DataContext = new clsContratosBE();
                _Referencias = new List<clsReferenciasBE>();

                dtgContactoEmergencias.ItemsSource = _Referencias;
                
                _SolicitudID = 0;
                _OcupacionID = 0;
                _HorarioID = 0;
                _IngresoID = 0;
                _SolicitudID = 0;
                _Documento = string.Empty;
                _ClienteID = 0;
                _ContratoID = 0;
                VISTA = 1;
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaNacimientoGarante.SelectedDate = DateTime.Now;
                //txtFechaContrato.SelectedDate = DateTime.Now;
                lblCuota.Text = string.Format("{0:N2}", 0);

                txtFechaIngreso.SelectedDate = DateTime.Now;
                cmbInstituciones.SelectedValue = 100001;
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;

                cmbTipoSolicitudes.SelectedValue = _TipoSolicitudID;
                cmbCondicionSolicitudes.SelectedValue = _CondicionID;
                cmbTipoContratos.SelectedValue = _TipoContratoID;

                LoadConfig();

                cmbZonas.SelectedValue = RutaID;
                cmbOficiales.SelectedValue = OficialID;
                cmbNotarioPublico.SelectedValue = NotarioPublicoID;
                cmbObjetivos.SelectedValue = _ObjetivoID;
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
                if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private  async void SaveSolicitudesUpdate()
        {
            try
            {
                bool DataCredito = rbDataCredito_Si.IsChecked == true ? true : false;
                bool MoraAutomatica = rbMorasAut_Si.IsChecked == true ? true : false;

                CalcularLegal();
                CalcularSeguro();
                CalcularFecha();

                DateTime _fecha = (DateTime)txtFechaContrato.SelectedDate;
                float _interes = float.Parse(txtInteres.Text);
                float _comision = float.Parse(txtComision.Text);
                float _mora = float.Parse(txtMoras.Text);
                float _cuota = float.Parse(lblCuota.Text);
                int _diasGracia = Convert.ToInt32(txtDiasGracia.Text);
                int _tipoContratoID = (int)cmbTipoContratos.SelectedValue;
                int _OficialID = (int)cmbOficiales.SelectedValue;
                int _notarioID = (int)cmbNotarioPublico.SelectedValue;
                bool LetterDelay = rbCartas_Si.IsChecked == true ? true : false;
                bool IsLegal = rbLegal_Si.IsChecked == true ? true : false;
                bool IsSeguro = rbInsurance_Si.IsChecked == true ? true : false;
                bool InteresDiario = rbInteresDiario_Si.IsChecked == true ? true : false;
                bool MoraDiaria = rbMoraDiaria_Si.IsChecked == true ? true : false;

                //txtInteres.Text = TasaInteres.ToString();

                OperationResult result = await db.ContratosUpdate(_ContratoID, _fecha, Vence, _SolicitudID, txtActo.Text, _tipoContratoID, _comision, _interes, _mora, _cuota, DataCredito, MoraAutomatica, LetterDelay, _diasGracia, _notarioID, _OficialID, (int)cmbZonas.SelectedValue, IsLegal, TasaLegal, IsSeguro, TasaSeguro, (int)cmbObjetivos.SelectedValue, InteresDiario, MoraDiaria, clsVariablesBO.UsuariosBE.Documento);
                if (result.ResponseCode == "00")
                {
                    bool recalcular = true;

                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "3F6A-B879-167A-A21A-46D5-EEB3-D0EE-ADDC" && _tipoContratoID != 2)
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit(clsLenguajeBO.Find("msgRecalculatePayment"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += async (arg, obj) =>
                        {
                            var dr = db.DetalleRecibosDeleteGetByContratoID(_ContratoID);
                            var nd = db.DetalleNotaDebitosDeleteGetByContratoID(_ContratoID);
                            var nc = db.DetalleNotaCreditosDeleteGetByContratoID(_ContratoID);
                            msgBox.Close();
                        };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                        msgBox.Owner = this;
                        msgBox.ShowDialog();
                    }

                    OperationResult response = CuotasCreateAutomatica(_ContratoID, clsVariablesBO.UsuariosBE.Documento, recalcular); //db.CuotasDeleteGetByContratoID(_ContratoID);
                    if (response.ResponseCode == "00")
                    {
                        if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID != "3F6A-B879-167A-A21A-46D5-EEB3-D0EE-ADDC" && _tipoContratoID != 2)
                        {
                            await db.ReCalcularRecibosGetByContratoID(_ContratoID, false);
                        }

                        var r = db.ContratosGetByContratoID(_ContratoID);    
                        if(r.SolicitudCheques.Count() == 0)
                        {
                            if (rbBankRequest_Si.IsChecked == true && (int)cmbBancos.SelectedValue != -1)
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsMultipleRequest == false)
                                {
                                    float _monto = (float.Parse(txtMonto.Text) * (((float.Parse(txtComision.Text)) / 100) * (float.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                                    var request = db.SolicitudChequesCreate((DateTime)txtFechaContrato.SelectedDate, r.ContratoID, (int)cmbBancos.SelectedValue, r.Solicitudes.Clientes.Personas.Nombres + " " + r.Solicitudes.Clientes.Personas.Apellidos, "CONTRATO " + (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud.ToUpper() + " #" + r.ContratoID.ToString(), float.Parse(txtMonto.Text) + _monto, clsVariablesBO.UsuariosBE.Documento);

                                    db.DetalleSolicitudChequesCreate(int.Parse(request.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), float.Parse(txtMonto.Text) , 0, clsVariablesBO.UsuariosBE.Documento);
                                    db.DetalleSolicitudChequesCreate(int.Parse(request.ResponseMessage), (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, 0, float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                                }
                                else
                                {
                                    float _monto = (float.Parse(txtMonto.Text) * (((float.Parse(txtComision.Text)) / 100) * (float.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                                    var request = db.SolicitudChequesCreate((DateTime)txtFechaContrato.SelectedDate, r.ContratoID, (int)cmbBancos.SelectedValue, r.Solicitudes.Clientes.Personas.Nombres + " " + r.Solicitudes.Clientes.Personas.Apellidos, "CONTRATO " + (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud.ToUpper() + " #" + r.ContratoID.ToString(), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);

                                    db.DetalleSolicitudChequesCreate(int.Parse(request.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), float.Parse(txtMonto.Text), 0, clsVariablesBO.UsuariosBE.Documento);
                                    db.DetalleSolicitudChequesCreate(int.Parse(request.ResponseMessage), (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, 0, float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);

                                    if (_monto > 0)
                                    {
                                        db.SolicitudChequesCreate((DateTime)txtFechaContrato.SelectedDate, r.ContratoID, (int)cmbBancos.SelectedValue, r.Solicitudes.Clientes.Personas.Nombres + " " + r.Solicitudes.Clientes.Personas.Apellidos, "COMISION CONTRATO " + (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud.ToUpper() + " #" + r.ContratoID.ToString(), _monto, clsVariablesBO.UsuariosBE.Documento);

                                        db.DetalleSolicitudChequesCreate(int.Parse(request.ResponseMessage), Convert.ToInt32(r.Solicitudes.TipoSolicitudes.AuxiliarID), _monto, 0, clsVariablesBO.UsuariosBE.Documento);
                                        db.DetalleSolicitudChequesCreate(int.Parse(request.ResponseMessage), (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, 0, _monto, clsVariablesBO.UsuariosBE.Documento);

                                    }
                                }
                            }
                        }

                        ContratosGet(r);
                    }
                    else
                    {
                        clsMessage.ErrorMessage(response.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                }
                //RadBusyIndicator.IsActive = false;
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

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OperadorID = (int)cmbOperadores.SelectedValue;
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridContratos) && rbBankRequest_Si.IsChecked == true ? (int)cmbBancos.SelectedValue != -1 == true ? true : false : true)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {

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
                        clsMessage.ErrorMessage(string.Format(clsLenguajeBO.Find("msgLimitPlan"), clsVariablesBO.UsuariosBE.Sucursales.Empresas.Planes.Hasta, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Planes.Plan), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSolicitudes) == true && clsValidacionesBO.Validar(gridDatosPersonales) == true && clsValidacionesBO.Validar(gridContratos))
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
            try
            {
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
                    cmbTipoSolicitudes.SelectedValue = -1;
                }

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
                List<clsSucursalesBE> Sucursales = new List<clsSucursalesBE>();
                Sucursales = db.SucursalesGet(null).ToList();

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.IsEnabled = true;
                }
                else
                {
                    cmbSucursales.IsEnabled = false;
                }
                cmbSucursales.ItemsSource = Sucursales;
                cmbSucursales.SelectedValuePath = "SucursalID";
                cmbSucursales.DisplayMemberPath = "Sucursal";
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;

                //Garantias
                cmbOperadoresGarante.ItemsSource = Operadores;
                cmbOperadoresGarante.SelectedValuePath = "OperadorID";
                cmbOperadoresGarante.DisplayMemberPath = "Operador";
                cmbOperadoresGarante.SelectedValue = -1;

                //Instituciones
                InstitucionesGet();
                cmbInstituciones.SelectedValue = 100001;

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
                foreach (var row in Modelos)
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

                //Tipo de Contratos
                List<clsTipoContratosBE> TipoContratos = new List<clsTipoContratosBE>();
                TipoContratos = db.TipoContratosGet(null).ToList();
                TipoContratos.Add(new clsTipoContratosBE { TipoContratoID = -1, TipoContrato = clsLenguajeBO.Find("itemSelect") });
                cmbTipoContratos.ItemsSource = TipoContratos;
                cmbTipoContratos.SelectedValuePath = "TipoContratoID";
                cmbTipoContratos.DisplayMemberPath = "TipoContrato";
                if (TipoContratos.Count() > 1)
                {
                    _TipoContratoID = TipoContratos.Where(x => x.IsDefault == true).FirstOrDefault().TipoContratoID;
                    cmbTipoContratos.SelectedValue = _TipoContratoID;
                }
                else
                {
                    cmbTipoContratos.SelectedValue = -1;
                }

                //Notarios Publico
                List<clsNotariosPublicoBE> Notarios = new List<clsNotariosPublicoBE>();
                if (VISTA == 1)
                {
                    Notarios = db.NotariosPublicoGet(null).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID && x.EstadoID == true).ToList();
                }
                else
                {
                    Notarios = db.NotariosPublicoGet(null).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID).ToList();
                }

               

                List<clsNotariosPublicoBE> List = new List<clsNotariosPublicoBE>();
                foreach (var row in Notarios)
                {
                    List.Add(new clsNotariosPublicoBE { NotarioPublicoID = row.NotarioPublicoID, SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                }

                int NotarioPublicoID = -1;
                if(List.Count() == 1)
                {
                    NotarioPublicoID = List.FirstOrDefault().NotarioPublicoID;
                }
                List.Add(new clsNotariosPublicoBE { NotarioPublicoID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemSelect") } });

                cmbNotarioPublico.ItemsSource = List;
                cmbNotarioPublico.SelectedValuePath = "NotarioPublicoID";
                cmbNotarioPublico.DisplayMemberPath = "Personas.Nombres";
                cmbNotarioPublico.SelectedValue = NotarioPublicoID;



                //Oficiales
                List<clsOficialesBE> Oficiales = new List<clsOficialesBE>();
                Oficiales = db.OficialesGet(null).Where(x => x.SucursalID == (int)cmbSucursales.SelectedValue && x.EstadoID ==true).ToList();
                int OficialID = -1;
                if (Oficiales.Count() == 1)
                {
                    OficialID = Oficiales.FirstOrDefault().OficialID;
                }
                Oficiales.Add(new clsOficialesBE { OficialID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemSelect") } });

                List<clsOficialesBE> ListOfAgent = new List<clsOficialesBE>();
                foreach (var row in Oficiales)
                {
                    ListOfAgent.Add(new clsOficialesBE { OficialID = row.OficialID, SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                }

                cmbOficiales.ItemsSource = ListOfAgent;
                cmbOficiales.SelectedValuePath = "OficialID";
                cmbOficiales.DisplayMemberPath = "Personas.Nombres";
                cmbOficiales.SelectedValue = OficialID;

                ZonasGet();

                //Bancos
                List<clsBancosBE> Bancos = new List<clsBancosBE>();
                Bancos = db.BancosGet(null, clsVariablesBO.UsuariosBE.Documento).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID).ToList();
                int BancoID = -1;
                if((bool)rbBankRequest_Si.IsChecked && Bancos.Count() == 1)
                {
                    BancoID = Bancos.FirstOrDefault().BancoID;
                }
                Bancos.Add(new clsBancosBE { BancoID = -1, Banco = clsLenguajeBO.Find("itemSelect") });

                cmbBancos.ItemsSource = Bancos;
                cmbBancos.SelectedValuePath = "BancoID";
                cmbBancos.DisplayMemberPath = "Banco";
                cmbBancos.SelectedValue = BancoID;

                //Objetivos
                List<clsObjetivosBE> Objetivos = new List<clsObjetivosBE>();
                Objetivos = db.ObjetivosGet(null).ToList();

                Objetivos.Add(new clsObjetivosBE { ObjetivoID = -1, Objetivo = clsLenguajeBO.Find("itemSelect") });

                cmbObjetivos.ItemsSource = Objetivos;
                cmbObjetivos.SelectedValuePath = "ObjetivoID";
                cmbObjetivos.DisplayMemberPath = "Objetivo";
                _ObjetivoID = Objetivos.Where(x => x.IsDefault == true).FirstOrDefault().ObjetivoID;
                cmbObjetivos.SelectedValue = _ObjetivoID;
            }
            catch { }

        }

        private void ZonasGet()
        {
            try {
                //Zonas
                List<clsZonasBE> Zonas = new List<clsZonasBE>();
                foreach(var item in db.ZonasGet(null).ToList())
                {
                    Zonas.Add(new clsZonasBE { ZonaID= item.ZonaID, Fecha = item.Fecha, Zona = $"{item.Rutas.Ruta} / {item.Zona}", RutaID = item.RutaID, Usuario = item.Usuario, ModificadoPor = item.ModificadoPor, FechaModificacion = item.FechaModificacion  });
                }

                int ZonaID = -1;
                if (Zonas.Count() == 1)
                {
                    ZonaID = Zonas.FirstOrDefault().ZonaID;
                }
                Zonas.Add(new clsZonasBE { ZonaID = -1, Zona = clsLenguajeBO.Find("itemSelect") });
                cmbZonas.ItemsSource = Zonas;
                cmbZonas.SelectedValuePath = "ZonaID";
                cmbZonas.DisplayMemberPath = "Zona";
                cmbZonas.SelectedValue = ZonaID;
            }
            catch { }
        }

        private void frmContratos_Loaded(object sender, RoutedEventArgs e)
        {
            if (VISTA == 1)
            {
                txtFechaNacimiento.SelectedDate = DateTime.Now;
                txtFechaNacimientoGarante.SelectedDate = DateTime.Now;
                txtFechaExpedicion.SelectedDate = DateTime.Now;
                LoadCombox();
                txtDireccionTrabajo.Text = "-";
                txtTelefonoTrabajo.Text = "-";
                cmbOcupaciones.SelectedValue = 1;
                cmbHorarios.SelectedValue = 1;
                cmbIngresos.SelectedValue = 1;
                txtFechaContrato.SelectedDate = DateTime.Now;                
                LoadConfig();
                txtNumero.Focus();
            }
            TipoSolicitudChange();

            chkDocument.IsChecked = clsCookiesBO.getPrintContract();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
