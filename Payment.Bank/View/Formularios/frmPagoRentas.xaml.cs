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
using Payment.Bank.View.Consultas;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes;
using System.ServiceModel;
using Payment.Bank.Properties;
using System.Text.RegularExpressions;
using System.Drawing.Printing;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmPagoRentas.xaml
    /// </summary>
    public partial class frmPagoRentas : MetroWindow
    {
        int VISTA = 1;
        int _AlquilerID = 0;
        int _PagoRentaID = 0;
        int _FormaPagoID = -1;
        int _ComprobanteID = -1;
        int _TipoAlquilerID = 0;

        //clsTipoAlquileresBE BE = new clsTipoAlquileresBE();
        clsAlquileresBE AlquileresBE = new clsAlquileresBE();
        List<clsDetallePagoRentasBE> detallePagoRentasBE = new List<clsDetallePagoRentasBE>();
        clsDetallePagoRentasBE BE = new clsDetallePagoRentasBE();
        public List<RentasBoxReportItem> rentasBE = new List<RentasBoxReportItem>();

        Core.Manager db = new Core.Manager();
        public frmPagoRentas()
        {
            InitializeComponent();
            LoadCombox();

            clsLenguajeBO.Load(gridEncabezado);
            clsLenguajeBO.Load(gridDetalleCuotas);
            clsLenguajeBO.Load(gridMenu);
            clsLenguajeBO.Load(gridOpciones);
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridInformacion);

            Title = clsLenguajeBO.Find(Title.ToString());

            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            btnDelete.Click += btnDelete_Click;
            btnAdd.Click += btnAdd_Click;
            dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;

            //Contratos
            txtAlquiler.KeyDown += txtAlquiler_KeyDown;
            txtAlquiler.TextChanged += txtAlquiler_TextChanged;
            txtCuota.KeyDown += txtCuota_KeyDown;
            txtDescuento.TextChanged += txtDescuento_TextChanged;
            txtDescuento.KeyDown += txtDescuento_KeyDown;
            txtDescuento.LostFocus += txtDescuento_LostFocus;
            Loaded += frmRecibos_Loaded;
        }


        public void OnInit(clsPagoRentasBE row, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                txtAlquiler.Text = row.AlquilerID.ToString();

                _AlquilerID = row.AlquilerID;
                _PagoRentaID = row.PagoRentaID;
                txtInformacion.Text = row.Informacion;
                cmbComprobantes.SelectedValue = row.ComprobanteID;
                cmbFormaPagos.SelectedValue = row.FormaPagoID;
                txtFecha.SelectedDate = row.Fecha;

                AlquileresBE = row.Alquileres;
                gridEncabezado.DataContext = AlquileresBE;
                detallePagoRentasBE = row.DetallePagoRentas.ToList();
                dataGrid1.ItemsSource = detallePagoRentasBE;
                gridOpciones.DataContext = row;

                CalcularTotales();
            }
            catch { }
        }

        void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEncabezado) == true && clsValidacionesBO.Validar(gridOpciones) == true && detallePagoRentasBE != null)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        if (_AlquilerID > 0)
                        {
                            OperationResult result = db.PagoRentasCreate(_AlquilerID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbComprobantes.SelectedValue, (int)cmbFormaPagos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                detallePagoRentasCreate(Convert.ToInt32(result.ResponseMessage));
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridEncabezado) == true && clsValidacionesBO.Validar(gridOpciones) == true && detallePagoRentasBE != null)
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.PagoRentasUpdate(_PagoRentaID, _AlquilerID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbComprobantes.SelectedValue, (int)cmbFormaPagos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                detallePagoRentasCreate(_PagoRentaID);
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
                }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void detallePagoRentasCreate(int PagoRentaID)
        {
            try
            {
                db.DetallePagoRentasDeleteGetByPagoRentaID(PagoRentaID);
                OperationResult result = new OperationResult();
                foreach (clsDetallePagoRentasBE row in detallePagoRentasBE)
                {
                    result = db.DetallePagoRentasCreate(row.RentaID, PagoRentaID, row.Concepto, row.Monto, clsVariablesBO.UsuariosBE.Documento);
                }

                if (result.ResponseCode == "00")
                {
                    if (chkPreview.IsChecked == true)
                    {
                        frmPrintPagoRentasGetByPagoRentaID Recibos = new frmPrintPagoRentasGetByPagoRentaID();
                        Recibos.Owner = this;
                        Recibos.OnInit(PagoRentaID);
                        Recibos.ShowDialog();
                    }
                    else
                    {
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    }

                    clsRentasView Cuotas = new clsRentasView();
                    Cuotas.SetDataSource(_AlquilerID, PagoRentaID);
                    var R = Cuotas.GetGroup();
                    var balance = (Cuotas.BalanceGetByPagoRentaID(DateTime.Now) < 0 ? 0 : Cuotas.BalanceGetByPagoRentaID(DateTime.Now));

                    NotificarPago(PagoRentaID, balance);
                    ClearAll();
                }

            }
            catch { }
        }

        private void NotificarPago(int ID, float Balance)
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarAlquiler == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                        {
                            var row = db.PagoRentasGetByPagoRentaID(ID);

                            //var smsResult = smsWS.EnviarSMSAsync(row.Alquileres.Personas.Celular, string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarAlquiler), row.Sucursales.Empresas.Empresa, row.PagoRentaID, row.AlquilerID, string.Format("{0:N2}", row.Monto), string.Format("{0:N2}", Balance)), row.Alquileres.Personas.OperadorID);
                            string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarAlquiler), row.Sucursales.Empresas.Empresa, row.PagoRentaID, row.AlquilerID, string.Format("{0:N2}", row.Monto), string.Format("{0:N2}", Balance));
                            var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsUrl, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), row.Alquileres.Personas.Celular, Mensaje, row.Alquileres.Personas.OperadorID);

                            if (smsResult.ResponseCode == "00")
                            {

                            }
                            else
                            {
                                clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            }
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage("La red de mensajería no esta disponible en estos momentos.", clsLenguajeBO.Find("msgTitle"));

                    }
                }

            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsDetallePagoRentasBE)dataGrid1.SelectedItem;
            }
            catch { }
        }


        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsValidacionesBO.Validar(gridDetalleCuotas) == true)
                {
                    float Monto = 0;
                    foreach (RentasBoxReportItem c in rentasBE)
                    {
                        Monto += c.Monto;
                    }

                    clsValidacionesBO.Limpiar(gridDetalleCuotas);

                    if (Monto > 0)
                    {
                        dataGrid1.ItemsSource = new List<clsDetallePagoRentasBE>();
                        PagosAutomaticos(Monto);
                    }
                }
            }
            catch { }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.RentaID > 0)
                {
                    foreach (clsDetallePagoRentasBE fila in detallePagoRentasBE)
                    {
                        if (fila == dataGrid1.SelectedItem)
                        {
                            detallePagoRentasBE.Remove((clsDetallePagoRentasBE)dataGrid1.SelectedItem);
                            dataGrid1.ItemsSource = new List<clsDetallePagoRentasBE>();
                            dataGrid1.ItemsSource = detallePagoRentasBE;
                            CalcularTotales();
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

        private void txtAlquiler_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                if (e.Key == Key.Enter && !String.IsNullOrEmpty(txtAlquiler.Text))
                {
                    AlquileresBE = db.AlquileresGetByAlquilerID(int.Parse(txtAlquiler.Text));
                    if (AlquileresBE != null && AlquileresBE.EstadoID == true)
                    {
                        _AlquilerID = AlquileresBE.AlquilerID;
                        _TipoAlquilerID = AlquileresBE.TipoAlquilerID;

                        gridEncabezado.DataContext = AlquileresBE;
                        CallRentas();
                    }
                    else
                    {
                        _AlquilerID = 0;
                        AlquileresBE = new clsAlquileresBE();
                        gridEncabezado.DataContext = AlquileresBE;
                    }
                }
                else
                {
                    if (e.Key == Key.Enter && string.IsNullOrEmpty(txtAlquiler.Text))
                    {
                        frmConsultasAlquileres Contratos = new frmConsultasAlquileres();
                        Contratos.Owner = this;
                        Contratos.IsQuery = true;
                        Contratos.Closed += (arg, obj) =>
                        {
                            if (Contratos._AlquilerID > 0)
                            {
                                txtAlquiler.Text = Contratos._AlquilerID.ToString();
                                AlquileresBE = db.AlquileresGetByAlquilerID(Contratos._AlquilerID);
                                if (AlquileresBE != null && AlquileresBE.EstadoID == true)
                                {
                                    _AlquilerID = AlquileresBE.AlquilerID;
                                    _TipoAlquilerID = AlquileresBE.TipoAlquilerID;

                                    gridEncabezado.DataContext = AlquileresBE;
                                    CallRentas();
                                }
                                else
                                {
                                    _AlquilerID = 0;
                                    AlquileresBE = new clsAlquileresBE();
                                    gridEncabezado.DataContext = AlquileresBE;
                                }
                            }
                        };
                        Contratos.ShowDialog();
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtTotal_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                if (Convert.ToDouble(txtSubTotal.Text) > 0)
                {
                    e.Handled = false;
                }
                else
                {
                    if (e.Key == Key.Decimal)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                e.Handled = true;
            }

        }

        private void txtDescuento_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double subtotal = Convert.ToDouble(txtSubTotal.Text);
                double descuento = Convert.ToDouble(txtDescuento.Text);
                if (descuento > 0)
                {
                    if (descuento > Convert.ToDouble(txtSubTotal.Text))
                    {
                        txtDescuento.Text = subtotal.ToString();
                    }
                    CalcularTotales();
                }

            }
            catch { }
        }

        private void txtDescuento_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double descuento = Convert.ToDouble(txtDescuento.Text);
                txtDescuento.Text = string.Format("{0:N2}", descuento);
                CalcularTotales();
            }
            catch { }

        }

        private void txtCuota_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    if (e.Key == Key.Enter)
                    {
                        CallRentas();
                    }
                }
            }
            catch { }
        }

        private void CallRentas()
        {
            try
            {

                if (AlquileresBE != null && AlquileresBE.AlquilerID > 0)
                {
                    if (string.IsNullOrEmpty(AlquileresBE.Personas.Direccion) || string.IsNullOrEmpty(AlquileresBE.Personas.Telefono) || string.IsNullOrEmpty(AlquileresBE.Personas.Celular) || AlquileresBE.Personas.Telefono.Length != 13 || AlquileresBE.Personas.Celular.Length != 13 || !Regex.IsMatch(AlquileresBE.Personas.Correo, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                    {
                        frmUpdateData Update = new frmUpdateData();
                        Update.Owner = this;
                        Update.OnInit(AlquileresBE.Documento, 2);
                        Update.Closed += (obj, arg) =>
                        {
                            frmConsultasRentas cuotas = new frmConsultasRentas();
                            cuotas.btnAceptar.Visibility = Visibility.Visible;
                            cuotas.txtMonto.Visibility = Visibility.Visible;
                            cuotas.txtMonto.Focus();
                            cuotas.Owner = this;
                            cuotas.OnInit(AlquileresBE);
                            cuotas.Closed += (s, b) =>
                            {
                                if (cuotas.Monto > 0)
                                {
                                    dataGrid1.ItemsSource = new List<clsDetallePagoRentasBE>();
                                    rentasBE = cuotas.rentasBE;
                                    PagosAutomaticos(cuotas.Monto);
                                }
                            };
                            cuotas.Show();
                        };
                        Update.ShowDialog();
                    }
                    else
                    {
                        frmConsultasRentas cuotas = new frmConsultasRentas();
                        cuotas.btnAceptar.Visibility = Visibility.Visible;
                        cuotas.txtMonto.Visibility = Visibility.Visible;
                        cuotas.txtMonto.Focus();
                        cuotas.OnInit(AlquileresBE);
                        cuotas.Closed += (s, b) =>
                        {
                            if (cuotas.Monto > 0)
                            {
                                dataGrid1.ItemsSource = new List<clsDetallePagoRentasBE>();
                                rentasBE = cuotas.rentasBE;
                                PagosAutomaticos(cuotas.Monto);                                
                            }
                        };
                        cuotas.Show();
                    }
                }
            }
            catch { }
        }


        public void PagosAutomaticos(float Monto)
        {
            try
            {
                detallePagoRentasBE = new List<clsDetallePagoRentasBE>();
                clsDetallePagoRentasBE _detallePagoRentasBE;
                foreach (RentasBoxReportItem fila in rentasBE.OrderBy(x => x.RentaID).ToList())
                {
                    bool Salir = false;
                    _detallePagoRentasBE = new clsDetallePagoRentasBE();

                    if (fila.Monto > 0 && Monto > 0)
                    {
                        if (Monto >= fila.Monto)
                        {
                            _detallePagoRentasBE.Monto = fila.Monto;
                            _detallePagoRentasBE.RentaID = fila.RentaID;
                            _detallePagoRentasBE.Concepto = "SALDO " + fila.Concepto;

                            int i = 0; bool add = true;
                            foreach (clsDetallePagoRentasBE row in detallePagoRentasBE)
                            {
                                if (row.RentaID == _detallePagoRentasBE.RentaID)
                                {
                                    detallePagoRentasBE[i].Concepto = _detallePagoRentasBE.Concepto;
                                    detallePagoRentasBE[i].Monto += _detallePagoRentasBE.Monto;                                    
                                    add = false;
                                }
                                i++;
                            }
                            if (add == true)
                            {
                                detallePagoRentasBE.Add(_detallePagoRentasBE);
                            }
                            Monto = Monto - (fila.Monto);
                        }
                        else
                        {

                            if (Monto > fila.Monto)
                            {
                                _detallePagoRentasBE.Monto = fila.Monto;
                                Monto = Monto - fila.Monto;
                                Salir = false;
                            }
                            else
                            {
                                _detallePagoRentasBE.Monto = Monto;
                                Monto = Monto - fila.Monto;
                                Salir = true;
                            }


                              _detallePagoRentasBE.Concepto = "ABONO A "+ fila.Concepto;
                            _detallePagoRentasBE.RentaID = fila.RentaID;
                            _detallePagoRentasBE.Monto = (_detallePagoRentasBE.Monto);

                            int i = 0; bool add = true;
                            foreach (clsDetallePagoRentasBE row in detallePagoRentasBE)
                            {
                                if (row.RentaID == _detallePagoRentasBE.RentaID)
                                {
                                    detallePagoRentasBE[i].Concepto = _detallePagoRentasBE.Concepto;
                                    detallePagoRentasBE[i].Monto += _detallePagoRentasBE.Monto;
                                    add = false;
                                }
                                i++;
                            }
                            if (add == true)
                            {
                                detallePagoRentasBE.Add(_detallePagoRentasBE);
                            }
                            Monto = Monto - (_detallePagoRentasBE.Monto);
                        }
                    }
                }
                dataGrid1.ItemsSource = new List<clsDetallePagoRentasBE>();
                dataGrid1.ItemsSource = detallePagoRentasBE;
                CalcularTotales();
                Monto = 0;


            }
            catch { }
        }

        private void CalcularTotales()
        {
            try
            {
                double SubTotal = 0, Descuento = 0;
                foreach (clsDetallePagoRentasBE Fila in detallePagoRentasBE)
                {
                    SubTotal = SubTotal + Convert.ToDouble(Fila.Monto);
                }
                Descuento = Convert.ToDouble(txtDescuento.Text);
                txtSubTotal.Text = string.Format("{0:N2}", SubTotal);
                txtTotal.Text = string.Format("{0:N2}", SubTotal - Convert.ToDouble(txtDescuento.Text));
            }
            catch (Exception ex)
            { }
        }

        private void txtAlquiler_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAlquiler.Text))
                {
                    ClearAll();
                }
            }
            catch { }
        }

        void ClearAll()
        {
            try
            {
                LimpiarEncabezado();
                LimpiarDetalleRecibo();
                LimpiarOpciones();
                cmbComprobantes.SelectedValue = _ComprobanteID;
                cmbFormaPagos.SelectedValue = _FormaPagoID;
            }
            catch { }
        }
        void LimpiarEncabezado()
        {
            try
            {

                //lblFecha.Text = string.Empty;
                //lblFechaVencimiento.Text = string.Empty;
                //lblMonto.Text = string.Empty;
                //lblInteres.Text = string.Empty;
                ////lblMoras.Text = string.Empty;
                gridEncabezado.DataContext = new clsPagoRentasBE();
                _AlquilerID = 0;
                _PagoRentaID = 0;
                _TipoAlquilerID = 0;
                VISTA = 1;
                detallePagoRentasBE = new List<clsDetallePagoRentasBE>();

                rentasBE = new List<RentasBoxReportItem>();
                clsValidacionesBO.Limpiar(gridEncabezado);
                clsValidacionesBO.Limpiar(gridDetalleCuotas);
                txtAlquiler.Focus();
            }
            catch { }
        }

        void LimpiarDetalleRecibo()
        {
            try
            {
                detallePagoRentasBE = new List<clsDetallePagoRentasBE>();
                dataGrid1.ItemsSource = new List<clsDetallePagoRentasBE>();
            }
            catch { }
        }

        void LimpiarOpciones()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridOpciones);
                txtSubTotal.Text = string.Format("{0:N2}", 0);
                txtDescuento.Text = string.Format("{0:N2}", 0);
                txtTotal.Text = string.Format("{0:N2}", 0);
                txtInformacion.Text = string.Empty;
            }
            catch { }
        }

        private void frmRecibos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                gridOpciones.DataContext = new clsPagoRentasBE();
                if (VISTA == 1) { txtFecha.SelectedDate = DateTime.Today; }
                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == false)
                {
                    txtFecha.IsEnabled = false;
                }
                else
                { 
                    CalcularTotales(); 
                }

                txtAlquiler.Focus();
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Formas de Pago
            List<clsFormaPagosBE> FormaPagos = new List<clsFormaPagosBE>();
            FormaPagos = db.FormaPagosGet(null).ToList();
            FormaPagos.Add(new clsFormaPagosBE { FormaPagoID = -1, Nombre = clsLenguajeBO.Find("itemSelect") });
            cmbFormaPagos.ItemsSource = FormaPagos;
            cmbFormaPagos.SelectedValuePath = "FormaPagoID";
            cmbFormaPagos.DisplayMemberPath = "Nombre";

            if (FormaPagos.Count() > 1)
            {
                cmbFormaPagos.SelectedValue = FormaPagos.Where(x => x.IsDefault == true).FirstOrDefault().FormaPagoID;
                _FormaPagoID = FormaPagos.Where(x => x.IsDefault == true).FirstOrDefault().FormaPagoID;
            }
            else
            {
                cmbFormaPagos.SelectedValue = -1;
            }

            //Comprobantes
            List<clsComprobantesBE> Comprobantes = new List<clsComprobantesBE>();
            Comprobantes = db.ComprobantesGet(null).ToList();
            Comprobantes.Add(new clsComprobantesBE { ComprobanteID = -1, Comprobante = clsLenguajeBO.Find("itemSelect") });
            cmbComprobantes.ItemsSource = Comprobantes;
            cmbComprobantes.SelectedValuePath = "ComprobanteID";
            cmbComprobantes.DisplayMemberPath = "Comprobante";

            if (Comprobantes.Count() > 1)
            {
                cmbComprobantes.SelectedValue = Comprobantes.Where(x => x.IsDefault == true).FirstOrDefault().ComprobanteID;
                _ComprobanteID = Comprobantes.Where(x => x.IsDefault == true).FirstOrDefault().ComprobanteID;
            }
            else
            {
                cmbComprobantes.SelectedValue = -1;
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



    }
}
