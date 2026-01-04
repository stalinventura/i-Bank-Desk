using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Consultas;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes;
using System.Text.RegularExpressions;
using Payment.Bank.Controles;
using System.IO;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmRecibos : MetroWindow
    {
        int VISTA = 1;
        int _ContratoID = 0;
        int _ReciboID = 0;
        int _FormaPagoID = -1;
        int _ComprobanteID = -1;
        int _TipoContratoID = 0;
        public bool PermissionAccess = false;


        clsRecibosBE recibosBE = new clsRecibosBE();
        clsContratosBE ContratosBE = new clsContratosBE();
        List<clsDetalleRecibosBE> detalleRecibosBE = new List<clsDetalleRecibosBE>();
        clsDetalleRecibosBE BE = new clsDetalleRecibosBE();
        public List<BoxReportItem> cuotasBE = new List<BoxReportItem>();

        Core.Manager db = new Core.Manager();
        public frmRecibos()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height * 0.95;
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
            txtContrato.KeyDown += txtContrato_KeyDown;
            txtContrato.TextChanged += txtContrato_TextChanged;
            txtCuota.KeyDown += txtCuota_KeyDown;
            txtDescuento.TextChanged += txtDescuento_TextChanged;
            txtDescuento.KeyDown += txtDescuento_KeyDown;
            txtDescuento.LostFocus += txtDescuento_LostFocus;
            Loaded += frmRecibos_Loaded;


            chkDocument.Checked += chkDocument_Checked;
            chkDocument.Unchecked += chkDocument_Unchecked;
        }

        private void chkDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = (CheckBox)sender;
                clsCookiesBO.PrintReceipt((bool)obj.IsChecked);
            }
            catch { }
        }

        private void chkDocument_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = (CheckBox)sender;
                clsCookiesBO.PrintReceipt((bool)obj.IsChecked);

            }
            catch { }
        }

        public void OnInit(clsRecibosBE row, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                txtContrato.Text = row.ContratoID.ToString();
                recibosBE = row;
                _ContratoID = row.ContratoID;
                _ReciboID = row.ReciboID;
            
                cmbComprobantes.SelectedValue = row.ComprobanteID;
                cmbFormaPagos.SelectedValue = row.FormaPagoID;
                txtFecha.SelectedDate = row.Fecha;

                ContratosBE = row.Contratos;
                gridEncabezado.DataContext = ContratosBE;
                detalleRecibosBE = row.DetalleRecibos.ToList();
                dataGrid1.ItemsSource = detalleRecibosBE;
                gridOpciones.DataContext = row;
                txtInformacion.Text = row.Informacion;
                txtDescuento.Text = string.Format("{0:N3}", row.Descuento);
                //CalcularTotales();
            }
            catch { }
        }

        void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEncabezado) == true && clsValidacionesBO.Validar(gridOpciones) == true && detalleRecibosBE != null)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        if (_ContratoID > 0)
                        {
                            bool ProcesarPago = true;
                            if((int)cmbFormaPagos.SelectedValue == 2)
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.Pasarelas.Count() > 0)
                                {
                                    frmTarjetas tarjetas = new frmTarjetas();
                                    tarjetas.Owner = this;
                                    tarjetas.OnInit(ContratosBE, float.Parse(txtTotal.Text));
                                    tarjetas.btnAceptar.Click += (arg, obj) =>
                                    {
                                        if (clsValidacionesBO.Validar(tarjetas.gridTarjetas))
                                        {
                                            var response = tarjetas.ComprobarTarjeta();
                                            if (!response)
                                            {
                                                ProcesarPago = false;
                                                return;
                                            }
                                            else
                                            {
                                                ProcesarPago = true;
                                                txtInformacion.Text += $"{tarjetas.txtNumero.Text.Substring(0, 4)} **** **** {tarjetas.txtNumero.Text.Substring(12, 4)} Número de Aprobación: {tarjetas.response.processorInformation.approvalCode}";
                                                tarjetas.Close();
                                            }
                                        }
                                    };
                                    tarjetas.btnSalir.Click += (arg, obj) =>
                                    {
                                        tarjetas.Close();
                                        ProcesarPago = false;
                                        return;
                                    };
                                    tarjetas.ShowDialog();
                                }
                                else
                                {
                                    ProcesarPago = false;
                                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPaymentGatewayRequired"), clsLenguajeBO.Find("msgTitle"));
                                    return;
                                }
                            }

                            if (ProcesarPago)
                            {
                                if (detalleRecibosBE.Count > 0)
                                {
                                    if ((int)cmbFormaPagos.SelectedValue == 1)
                                    {
                                        frmConfirmarPagos form = new frmConfirmarPagos();
                                        form.Owner = this;
                                        form.OnInit(float.Parse(txtTotal.Text), new clsRecibosBE());
                                        form.Closed += async (o, w) =>
                                        {
                                            if (form.Confirmado)
                                            {
                                                OperationResult result = db.RecibosCreate(_ContratoID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbComprobantes.SelectedValue, (int)cmbFormaPagos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), form.MontoPagado, form.Cambio, txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                                                if (result.ResponseCode == "00")
                                                {
                                                    DetalleRecibosCreate(Convert.ToInt32(result.ResponseMessage));
                                                    db.Contabilizar(2, Convert.ToInt32(result.ResponseMessage), clsVariablesBO.UsuariosBE.Documento);
                                                    ClearAll();
                                                }
                                            }
                                        };
                                        form.ShowDialog();
                                    }
                                    else
                                    {
                                        OperationResult result = db.RecibosCreate(_ContratoID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbComprobantes.SelectedValue, (int)cmbFormaPagos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), float.Parse(txtTotal.Text), 0, txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                                        if (result.ResponseCode == "00")
                                        {
                                            DetalleRecibosCreate(Convert.ToInt32(result.ResponseMessage));
                                            db.Contabilizar(2, Convert.ToInt32(result.ResponseMessage), clsVariablesBO.UsuariosBE.Documento);
                                            ClearAll();
                                        }
                                    }
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridEncabezado) == true && clsValidacionesBO.Validar(gridOpciones) == true && detalleRecibosBE != null)
                    {
                        if (clsVariablesBO.Permiso.Modificar == true || PermissionAccess)
                        {
                            frmConfirmarPagos form = new frmConfirmarPagos();
                            form.Owner = this;
                            form.OnInit(float.Parse(txtTotal.Text), recibosBE);
                            form.Closed += async (o, w) =>
                            {
                                if (form.Confirmado)
                                {
                                    OperationResult result = db.RecibosUpdate(_ReciboID, _ContratoID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, (int)cmbComprobantes.SelectedValue, (int)cmbFormaPagos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), form.MontoPagado, form.Cambio, txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                                    if (result.ResponseCode == "00")
                                    {
                                        DetalleRecibosCreate(_ReciboID);
                                        db.Contabilizar(2, _ReciboID, clsVariablesBO.UsuariosBE.Documento);
                                        ClearAll();
                                    }
                                    else
                                    {
                                        clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                    }
                                }
                            };
                            form.ShowDialog();
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

        private void DetalleRecibosCreate(int ReciboID)
        {
            try
            {
                db.DetalleRecibosDeleteGetByReciboID(ReciboID);
                OperationResult result = new OperationResult();
                foreach (clsDetalleRecibosBE row in (List<clsDetalleRecibosBE>)dataGrid1.ItemsSource)
                {
                    result = db.DetalleRecibosCreate(row.CuotaID, ReciboID, row.Numero, row.Concepto, row.Capital, row.Comision, row.Interes, row.Mora, row.Legal, row.Seguro, row.SubTotal, clsVariablesBO.UsuariosBE.Documento);
                }

                if (result.ResponseCode == "00")
                {
                    if (chkDocument.IsChecked == true)
                    {
                        frmPrintRecibosGetByReciboID Recibos = new frmPrintRecibosGetByReciboID();
                        Recibos.Owner = this;
                        Recibos.OnInit(ReciboID);
                        Recibos.ShowDialog();
                    }
                    else
                    {
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    }

                    clsCuotasView Cuotas = new clsCuotasView();
                    Cuotas.SetDataSource(_ContratoID, ReciboID);
                    var R = Cuotas.GetGroup();
                    float balance = (Cuotas.BalanceGetByReciboID() < 0 ? 0 : Cuotas.BalanceGetByReciboID());

                    if (balance < (float)clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.MinimumAmount)
                    {
                        frmPrintCartaSaldo Saldo = new frmPrintCartaSaldo();
                        Saldo.Owner = this;
                        Saldo.OnInit(ReciboID);
                        Saldo.Closed += (obj, arg) =>
                        {
                            db.ContratosDeleteGetByContratoID(_ContratoID, 2, "SALDO A CONTRATO #" + _ContratoID.ToString(), clsVariablesBO.UsuariosBE.Documento);
                        };
                        Saldo.ShowDialog();

                        var request = db.RecibosGetByReciboID(ReciboID);
                        switch(request.Contratos.Solicitudes.TipoSolicitudID)
                        {
                            case 2:
                                {
                                    frmPrintCertificacionDocumentos doc = new frmPrintCertificacionDocumentos();
                                    doc.Owner = this;
                                    doc.OnInit(ReciboID);
                                    doc.ShowDialog();

                                } break;
                            case 3:
                                {
                                    frmPrintCertificacionDocumentos doc = new frmPrintCertificacionDocumentos();
                                    doc.Owner = this;
                                    doc.OnInit(ReciboID);
                                    doc.ShowDialog();
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (ContratosBE.EstadoID == 2)
                        {
                            db.ContratosDeleteGetByContratoID(_ContratoID, 1, "ACTIVANDO CONTRATO #" + _ContratoID.ToString(), clsVariablesBO.UsuariosBE.Documento);
                        }
                    }

                    if (VISTA == 1)
                    {
                        NotificarPago(ReciboID, balance);
                    }

                }

            }
            catch { ClearAll(); }
        }

        private async void NotificarPago(int ID, float Balance)
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.HasNetworkAccess == true && clsVariablesBO.GatewaySMS == true && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.NotificarRecibo == true && ContratosBE.Solicitudes.Condiciones.SendSMS == true)
                {
                    if (Common.Generic.HasAccess() == true)
                    {
                        var row = db.RecibosGetByReciboID(ID);
                        string Mensaje = string.Format(clsLenguajeBO.Find(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.msgNotificarRecibo), row.Contratos.Solicitudes.Clientes.Personas.Nombres, row.Contratos.Solicitudes.Clientes.Sucursales.Empresas.Empresa, row.ReciboID, row.ContratoID, string.Format("{0:N3}", row.Monto));

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                        {
                            string[] userID = new string[1];
                            userID[0] = row.Contratos.Solicitudes.Clientes.Documento;
                            await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                        {
                            try
                            {
                                if (Regex.IsMatch(row.Contratos.Solicitudes.Clientes.Personas.Correo, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                                {
                                    if (row.Monto > 0)
                                    {
                                        var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/RecibosGetByReciboID.html");
                                        string Body = string.Format(message, row.Sucursales.Empresas.Empresa, row.Sucursales.Direccion, row.Sucursales.Telefonos, row.Sucursales.Empresas.Rnc, row.Contratos.Solicitudes.Clientes.Personas.Nombres + " " + row.Contratos.Solicitudes.Clientes.Personas.Apellidos, string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", row.Fecha), row.Contratos.Solicitudes.TipoSolicitudes.TipoSolicitud, string.Format("{0:000000}", row.ContratoID), string.Format("{0:N2}", row.Monto), row.Sucursales.Gerentes.Personas.Nombres + " " + row.Sucursales.Gerentes.Personas.Apellidos, DateTime.Now.Year, row.Sucursales.Empresas.Empresa, row.Sucursales.Empresas.Site); ;

                                        string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Correos", $"{row.ReciboID}.pdf");
                                        Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                        ms.SendMail(row.Contratos.Solicitudes.Clientes.Personas.Correo, string.Format("Comprobante de Ingresos #{0} | {1}", row.ReciboID, row.Sucursales.Empresas.Empresa), Body, file);
                                    }
                                }
                            }
                            catch { }
                        }

                        if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.SmsNotification)
                        {
                            if (db.CanSendSmS(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID))
                            {
                                var smsResult = db.EnviarSMS(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.smsHost, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), row.Contratos.Solicitudes.Clientes.Personas.Celular, Mensaje, row.Contratos.Solicitudes.Clientes.Personas.OperadorID);
                                if (smsResult.ResponseCode == "00")
                                {
                                    db.SmsCreate(ContratosBE.Solicitudes.Clientes.Personas.Celular, Mensaje, row.ContratoID, clsVariablesBO.UsuariosBE.Documento);
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
                BE = (clsDetalleRecibosBE)dataGrid1.SelectedItem;
            }
            catch { }
        }


        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsValidacionesBO.Validar(gridDetalleCuotas) == true)
                {
                    float SubTotal = 0;
                    foreach (BoxReportItem c in cuotasBE)
                    {
                        SubTotal += c.Capital + c.Comision + c.Interes + c.Mora + c.Legal + c.Seguro;
                    }

                    clsValidacionesBO.Limpiar(gridDetalleCuotas);

                    if (SubTotal > 0)
                    {
                        dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                        detalleRecibosBE = db.PagosAutomaticos(ContratosBE.ContratoID, SubTotal, clsVariablesBO.UsuariosBE.Documento).Result;
                        dataGrid1.ItemsSource = detalleRecibosBE;
                        CalcularTotales();
                    }
                }
            }
            catch { }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (clsVariablesBO.Permiso.Eliminar == true)
                if (clsVariablesBO.Permiso.Modificar == true || clsVariablesBO.Permiso.Eliminar == true)
                {
                    if (BE.CuotaID > 0)
                    {
                        foreach (clsDetalleRecibosBE fila in detalleRecibosBE)
                        {
                            if (fila == dataGrid1.SelectedItem)
                            {
                                detalleRecibosBE.Remove((clsDetalleRecibosBE)dataGrid1.SelectedItem);
                                dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                                dataGrid1.ItemsSource = detalleRecibosBE;
                                CalcularTotales();
                            }
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                    }
            }
                else
            {
                clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
            }
        }
            catch { }
        }

        private void txtContrato_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                if (e.Key == Key.Enter && !String.IsNullOrEmpty(txtContrato.Text))
                {
                    ContratosBE = db.ContratosGetByContratoID(int.Parse(txtContrato.Text));
                    if (ContratosBE != null && ContratosBE.EstadoID == 1)
                    {
                        _ContratoID = ContratosBE.ContratoID;
                        _TipoContratoID = ContratosBE.TipoContratoID;

                        gridEncabezado.DataContext = ContratosBE;
                        HabilitarOpciones();
                        CallCuotas();
                    }
                    else
                    {
                        _ContratoID = 0;
                        ContratosBE = new clsContratosBE();
                        gridEncabezado.DataContext = ContratosBE;
                    }
                }
                else
                {
                    if (e.Key == Key.Enter && string.IsNullOrEmpty(txtContrato.Text))
                    {
                        frmConsultasContratos Contratos = new frmConsultasContratos();
                        Contratos.Owner = this;
                        Contratos.IsQueryPay = true;
                        Contratos.Closed += (arg, obj) => {
                            if (Contratos._ContratoID > 0)
                            {
                                txtContrato.Text = Contratos._ContratoID.ToString();
                                ContratosBE = db.ContratosGetByContratoID(Contratos._ContratoID);
                                if (ContratosBE != null && ContratosBE.EstadoID == 1)
                                {
                                    _ContratoID = ContratosBE.ContratoID;
                                    _TipoContratoID = ContratosBE.TipoContratoID;

                                    gridEncabezado.DataContext = ContratosBE;
                                    HabilitarOpciones();
                                    CallCuotas();
                                }
                                else
                                {
                                    _ContratoID = 0;
                                    ContratosBE = new clsContratosBE();
                                    gridEncabezado.DataContext = ContratosBE;
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

            if (ContratosBE.ContratoID > 0)
            {
                switch (e.Key)
                {
                    case Key.F1:
                        {
                            frmPrintEstadoCuentasGetByContratoID estados = new frmPrintEstadoCuentasGetByContratoID();
                            estados.Owner = this;
                            estados.OnInit(ContratosBE.ContratoID);
                            estados.ShowDialog();
                        }
                        break;

                    case Key.F2:
                        {
                            frmUpdateData estados = new frmUpdateData();
                            estados.Owner = this;
                            estados.OnInit(ContratosBE.Solicitudes.Clientes.Documento, 2);
                            estados.ShowDialog();
                        }
                        break;

                }
            }
        }

        private void HabilitarOpciones()
        {
            try
            {
                lblF1.Visibility = Visibility.Visible;
                lblF2.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void DesHabilitarOpciones()
        {
            try
            {
                lblF1.Visibility = Visibility.Collapsed;
                lblF2.Visibility = Visibility.Collapsed;
            }
            catch { }
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
                txtDescuento.Text = string.Format("{0:N3}", descuento);
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
                        CallCuotas();
                    }
                }
            }
            catch { }
        }

        private void CallCuotas()
        {
            try
            {

                if (ContratosBE != null && ContratosBE.ContratoID > 0)
                {
                    if (ValidarListaNegra(ContratosBE.Solicitudes.Clientes.Personas.Documento) == false)
                    {
                        gotoCuotas(false);
                    }
                    else 
                    {
                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.Height = 280;
                        msgBox.OnInit(string.Format(clsLenguajeBO.Find("msgBlackListReceipt"), ContratosBE.Solicitudes.Clientes.Personas.Nombres + " " + ContratosBE.Solicitudes.Clientes.Personas.Apellidos), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += async (arg, obj) =>
                        {
                            msgBox.Close();
                            gotoCuotas(true);
                        };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                        msgBox.Owner = this;
                        msgBox.ShowDialog();
                    }
                }
            }
            catch { }
        }

        private bool ValidarListaNegra(string documento)
        {
            try
            {
                db = new Core.Manager();
                var result = db.ListaNegrasGetByDocumento(documento);
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

        private void gotoCuotas(bool IsOnBlackList)
        {
            try
            {
                if (ContratosBE != null && ContratosBE.ContratoID > 0)
                {

                    frmMessageBox msgBox = new frmMessageBox();
                    if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == false && IsOnBlackList)
                    {
                        bool HasSend = false;
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
                                    string Mensaje = string.Format(clsLenguajeBO.Find("msgPermissionPaymentBlackList"), Common.Generic.ShortName(clsVariablesBO.UsuariosBE.Personas.Nombres), ContratosBE.ContratoID, ContratosBE.Solicitudes.Clientes.Personas.Nombres + " " + ContratosBE.Solicitudes.Clientes.Personas.Apellidos, Code);

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.RemoteNotification)
                                    {
                                        string[] userID = new string[1];
                                        userID[0] = clsVariablesBO.UsuariosBE.Documento;
                                        var response = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, Mensaje, userID, clsVariablesBO.UsuariosBE.Documento);
                                        if (response.ResponseCode == "00")
                                        {
                                            HasSend = true;
                                        }
                                    }

                                    if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.EmailNotification)
                                    {
                                        try
                                        {
                                            var template = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/PermissionRequest.html");
                                            string Body = string.Format(template, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos, Mensaje, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, DateTime.Now.Year, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Site);

                                            Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                            var response  = ms.SendMail(clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Correo, string.Format("Permiso Acceso Contrato #{0} | {1}", ContratosBE.ContratoID, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa), Body);
                                            if(response == "SUCCESS")
                                            {
                                                HasSend = true;
                                            }
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
                                                HasSend = true;
                                            }
                                            else
                                            {
                                                clsMessage.ErrorMessage(smsResult.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                                            }
                                        }
                                    }


                                    if (HasSend)
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
                        msgBox.btnSalir.Click += (arg, obj) => { 
                            msgBox.Close(); };
                        msgBox.Owner = this;
                        msgBox.ShowDialog();
                    }
                    else
                    {
                        ModificacionAutorizada(true);
                    }
                }
            }
            catch { }
        }

        void ModificacionAutorizada(bool PermissionAccess = false)
        {
            try
            {
                if (string.IsNullOrEmpty(ContratosBE.Solicitudes.Clientes.Personas.Direccion) || string.IsNullOrEmpty(ContratosBE.Solicitudes.Clientes.Personas.Telefono) || string.IsNullOrEmpty(ContratosBE.Solicitudes.Clientes.Personas.Celular) || ContratosBE.Solicitudes.Clientes.Personas.Telefono.Length != 13 || ContratosBE.Solicitudes.Clientes.Personas.Celular.Length != 13 || !Regex.IsMatch(ContratosBE.Solicitudes.Clientes.Personas.Correo, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {
                    frmUpdateData Update = new frmUpdateData();
                    Update.Owner = this;
                    Update.OnInit(ContratosBE.Solicitudes.Clientes.Documento, 2);
                    Update.Closed += (obj, arg) =>
                    {
                        frmConsultasCuotas cuotas = new frmConsultasCuotas();
                        cuotas.btnAceptar.Visibility = Visibility.Visible;
                        cuotas.txtMonto.Visibility = Visibility.Visible;
                        cuotas.txtMonto.Focus();
                        cuotas.OnInit(ContratosBE, (DateTime)txtFecha.SelectedDate);
                        cuotas.Closed += async (s, b) =>
                        {
                            if (cuotas.Monto > 0)
                            {
                                cuotasBE = cuotas.cuotasBE;
                                dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                                detalleRecibosBE = await db.PagosAutomaticos(ContratosBE.ContratoID, (float)cuotas.Monto, clsVariablesBO.UsuariosBE.Documento);
                                dataGrid1.ItemsSource = detalleRecibosBE;
                                CalcularTotales();
                            }
                        };
                        cuotas.Owner = this;
                        cuotas.Show();
                    };
                    Update.ShowDialog();
                }
                else
                {
                    frmConsultasCuotas cuotas = new frmConsultasCuotas();
                    cuotas.btnAceptar.Visibility = Visibility.Visible;
                    cuotas.txtMonto.Visibility = Visibility.Visible;
                    cuotas.txtMonto.Focus();
                    cuotas.OnInit(ContratosBE, (DateTime)txtFecha.SelectedDate);
                    cuotas.Closed += async (s, b) =>
                    {
                        if (cuotas.Monto > 0)
                        {
                            dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                            cuotasBE = cuotas.cuotasBE;

                            dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                            detalleRecibosBE = await db.PagosAutomaticos(ContratosBE.ContratoID, (float)cuotas.Monto, clsVariablesBO.UsuariosBE.Documento);
                            dataGrid1.ItemsSource = detalleRecibosBE;
                            CalcularTotales();
                        }
                    };
                    cuotas.Owner = this;
                    cuotas.ShowDialog();
                }
            }
            catch { }
        }

                #region Codigo Original de Pagos Automaticos

                //public void PagosAmortizadosAutomaticos(float Monto)
                //{
                //    try
                //    {
                //        clsDetalleRecibosBE _detalleRecibosBE;
                //        foreach (BoxReportItem fila in cuotasBE.OrderByDescending(x => x.Numero).ToList())
                //        {
                //            bool Salir = false;
                //            _detalleRecibosBE = new clsDetalleRecibosBE();

                //            if (fila.Balance > 0.5 && Monto > 0)
                //            {
                //                if (Monto >= fila.Balance)
                //                {
                //                    _detalleRecibosBE.Seguro = fila.Seguro;
                //                    _detalleRecibosBE.Legal = fila.Legal;
                //                    _detalleRecibosBE.Mora = fila.Mora;
                //                    _detalleRecibosBE.Interes = fila.Interes;
                //                    _detalleRecibosBE.Comision = fila.Comision;
                //                    _detalleRecibosBE.Capital = fila.Capital;
                //                    _detalleRecibosBE.CuotaID = fila.CuotaID;
                //                    _detalleRecibosBE.Concepto = clsLenguajeBO.Find("colSaldo") + string.Format("{0:000}", fila.Numero);
                //                    _detalleRecibosBE.Numero = fila.Numero;
                //                    _detalleRecibosBE.SubTotal = fila.Seguro + fila.Legal + fila.Mora + fila.Comision + fila.Interes + fila.Capital;

                //                    int i = 0; bool add = true;
                //                    foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                //                    {
                //                        if (row.CuotaID == _detalleRecibosBE.CuotaID)
                //                        {
                //                            detalleRecibosBE[i].Concepto = _detalleRecibosBE.Concepto;
                //                            detalleRecibosBE[i].Capital += _detalleRecibosBE.Capital;
                //                            detalleRecibosBE[i].Comision += _detalleRecibosBE.Comision;
                //                            detalleRecibosBE[i].Interes += _detalleRecibosBE.Interes;
                //                            detalleRecibosBE[i].Mora += _detalleRecibosBE.Mora;
                //                            detalleRecibosBE[i].Legal += _detalleRecibosBE.Legal;
                //                            detalleRecibosBE[i].Seguro += _detalleRecibosBE.Seguro;
                //                            detalleRecibosBE[i].SubTotal = (detalleRecibosBE[i].Seguro + detalleRecibosBE[i].Legal + detalleRecibosBE[i].Mora + detalleRecibosBE[i].Comision + detalleRecibosBE[i].Interes + detalleRecibosBE[i].Capital);
                //                            add = false;
                //                        }
                //                        i++;
                //                    }
                //                    if (add == true)
                //                    {
                //                        detalleRecibosBE.Add(_detalleRecibosBE);
                //                    }
                //                    Monto = Monto - (fila.Seguro + fila.Legal + fila.Mora + fila.Comision + fila.Interes + fila.Capital);
                //                }
                //                else
                //                {

                //                    if (Monto > fila.Seguro)
                //                    {
                //                        _detalleRecibosBE.Seguro = fila.Seguro;
                //                        Monto = Monto - fila.Seguro;
                //                        Salir = false;
                //                    }
                //                    else
                //                    {
                //                        _detalleRecibosBE.Seguro = Monto;
                //                        Monto = Monto - fila.Seguro;
                //                        Salir = true;
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Legal)
                //                        {
                //                            _detalleRecibosBE.Legal = fila.Legal;
                //                            Monto = Monto - fila.Legal;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Legal = Monto;
                //                            Monto = Monto - fila.Legal;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Mora)
                //                        {
                //                            _detalleRecibosBE.Mora = fila.Mora;
                //                            Monto = Monto - fila.Mora;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Mora = Monto;
                //                            Monto = Monto - fila.Mora;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Interes)
                //                        {
                //                            _detalleRecibosBE.Interes = fila.Interes;
                //                            Monto = Monto - fila.Interes;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Interes = Monto;
                //                            Monto = Monto - fila.Interes;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Comision)
                //                        {
                //                            _detalleRecibosBE.Comision = fila.Comision;
                //                            Monto = Monto - fila.Comision;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Comision = Monto;
                //                            Monto = Monto - fila.Comision;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Capital)
                //                        {
                //                            _detalleRecibosBE.Capital = fila.Capital;
                //                            Monto = Monto - fila.Capital;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Capital = Monto;
                //                            Monto = Monto - fila.Capital;
                //                            Salir = true;
                //                        }
                //                    }

                //                    _detalleRecibosBE.Concepto = clsLenguajeBO.Find("colAbono") + string.Format("{0:000}", fila.Numero);
                //                    _detalleRecibosBE.CuotaID = fila.CuotaID;
                //                    _detalleRecibosBE.Numero = fila.Numero;
                //                    _detalleRecibosBE.SubTotal = (_detalleRecibosBE.Seguro + _detalleRecibosBE.Legal + _detalleRecibosBE.Mora + _detalleRecibosBE.Comision + _detalleRecibosBE.Interes + _detalleRecibosBE.Capital);

                //                    int i = 0; bool add = true;
                //                    foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                //                    {
                //                        if (row.CuotaID == _detalleRecibosBE.CuotaID)
                //                        {
                //                            detalleRecibosBE[i].Concepto = _detalleRecibosBE.Concepto;
                //                            detalleRecibosBE[i].Capital += _detalleRecibosBE.Capital;
                //                            detalleRecibosBE[i].Comision += _detalleRecibosBE.Comision;
                //                            detalleRecibosBE[i].Interes += _detalleRecibosBE.Interes;
                //                            detalleRecibosBE[i].Mora += _detalleRecibosBE.Mora;
                //                            detalleRecibosBE[i].Legal += _detalleRecibosBE.Legal;
                //                            detalleRecibosBE[i].Seguro += _detalleRecibosBE.Seguro;
                //                            detalleRecibosBE[i].SubTotal = (detalleRecibosBE[i].Seguro + detalleRecibosBE[i].Legal + detalleRecibosBE[i].Mora + detalleRecibosBE[i].Comision + detalleRecibosBE[i].Interes + detalleRecibosBE[i].Capital);
                //                            add = false;
                //                        }
                //                        i++;
                //                    }
                //                    if (add == true && _detalleRecibosBE.SubTotal > 0.5)
                //                    {
                //                        detalleRecibosBE.Add(_detalleRecibosBE);
                //                    }
                //                    Monto = Monto - (_detalleRecibosBE.Seguro + _detalleRecibosBE.Legal + _detalleRecibosBE.Mora + _detalleRecibosBE.Comision + _detalleRecibosBE.Interes + _detalleRecibosBE.Capital);
                //                }
                //            }
                //        }
                //        dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                //        dataGrid1.ItemsSource = detalleRecibosBE.OrderBy(x=>x.Numero);
                //        CalcularTotales();
                //        Monto = 0;
                //    }
                //    catch { }
                //}

                //public void PagosAutomaticos(float Monto)
                //{
                //    try
                //    {
                //        detalleRecibosBE = new List<clsDetalleRecibosBE>();
                //        clsDetalleRecibosBE _detalleRecibosBE;
                //        foreach (BoxReportItem fila in cuotasBE.OrderBy(x => x.Numero).ToList())
                //        {
                //            bool Salir = false;
                //            _detalleRecibosBE = new clsDetalleRecibosBE();

                //            if (fila.Balance > 0.5 && Monto > 0)
                //            {
                //                if (Monto >= fila.Balance)
                //                {
                //                    _detalleRecibosBE.Seguro = fila.Seguro;
                //                    _detalleRecibosBE.Legal = fila.Legal;
                //                    _detalleRecibosBE.Mora = fila.Mora;
                //                    _detalleRecibosBE.Interes = fila.Interes;
                //                    _detalleRecibosBE.Comision = fila.Comision;
                //                    _detalleRecibosBE.Capital = fila.Capital;
                //                    _detalleRecibosBE.CuotaID = fila.CuotaID;
                //                    _detalleRecibosBE.Concepto = clsLenguajeBO.Find("colSaldo") + string.Format("{0:000}", fila.Numero);
                //                    _detalleRecibosBE.Numero = fila.Numero;
                //                    _detalleRecibosBE.SubTotal = fila.Seguro + fila.Legal + fila.Mora + fila.Comision + fila.Interes + fila.Capital;

                //                    int i = 0; bool add = true;
                //                    foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                //                    {
                //                        if (row.CuotaID == _detalleRecibosBE.CuotaID)
                //                        {
                //                            detalleRecibosBE[i].Concepto = _detalleRecibosBE.Concepto;
                //                            detalleRecibosBE[i].Capital += _detalleRecibosBE.Capital;
                //                            detalleRecibosBE[i].Comision += _detalleRecibosBE.Comision;
                //                            detalleRecibosBE[i].Interes += _detalleRecibosBE.Interes;
                //                            detalleRecibosBE[i].Mora += _detalleRecibosBE.Mora;
                //                            detalleRecibosBE[i].Legal += _detalleRecibosBE.Legal;
                //                            detalleRecibosBE[i].Seguro += _detalleRecibosBE.Seguro;
                //                            detalleRecibosBE[i].SubTotal = (detalleRecibosBE[i].Seguro + detalleRecibosBE[i].Legal + detalleRecibosBE[i].Mora + detalleRecibosBE[i].Comision + detalleRecibosBE[i].Interes + detalleRecibosBE[i].Capital);
                //                            add = false;
                //                        }
                //                        i++;
                //                    }
                //                    if (add == true)
                //                    {
                //                        if ((ContratosBE.TipoContratoID == 2 || ContratosBE.TipoContratoID == 3 || ContratosBE.TipoContratoID == 5) && fila.Interes > 0)
                //                        {
                //                            detalleRecibosBE.Add(_detalleRecibosBE);
                //                        }
                //                        else
                //                        {
                //                            if ((ContratosBE.TipoContratoID == 1 || ContratosBE.TipoContratoID == 4) && (fila.Comision + fila.Interes + fila.Mora + fila.Legal + fila.Seguro) == 0)
                //                            {
                //                                PagosAmortizadosAutomaticos(Monto);
                //                                return;
                //                            }
                //                            else
                //                            {
                //                                detalleRecibosBE.Add(_detalleRecibosBE);
                //                            }
                //                        }
                //                    }
                //                    Monto = Monto - (fila.Seguro + fila.Legal + fila.Mora + fila.Comision + fila.Interes + fila.Capital);
                //                }
                //                else
                //                {

                //                    if ((ContratosBE.TipoContratoID == 1 || ContratosBE.TipoContratoID == 4) && fila.Interes == 0)
                //                    {

                //                        PagosAmortizadosAutomaticos(Monto);
                //                        return;
                //                    }

                //                    if (Monto > fila.Seguro)
                //                    {
                //                        _detalleRecibosBE.Seguro = fila.Seguro;
                //                        Monto = Monto - fila.Seguro;
                //                        Salir = false;
                //                    }
                //                    else
                //                    {
                //                        _detalleRecibosBE.Seguro = Monto;
                //                        Monto = Monto - fila.Seguro;
                //                        Salir = true;
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Legal)
                //                        {
                //                            _detalleRecibosBE.Legal = fila.Legal;
                //                            Monto = Monto - fila.Legal;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Legal = Monto;
                //                            Monto = Monto - fila.Legal;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Mora)
                //                        {
                //                            _detalleRecibosBE.Mora = fila.Mora;
                //                            Monto = Monto - fila.Mora;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Mora = Monto;
                //                            Monto = Monto - fila.Mora;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Interes)
                //                        {
                //                            _detalleRecibosBE.Interes = fila.Interes;
                //                            Monto = Monto - fila.Interes;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Interes = Monto;
                //                            Monto = Monto - fila.Interes;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Comision)
                //                        {
                //                            _detalleRecibosBE.Comision = fila.Comision;
                //                            Monto = Monto - fila.Comision;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Comision = Monto;
                //                            Monto = Monto - fila.Comision;
                //                            Salir = true;
                //                        }
                //                    }

                //                    if (Salir == false)
                //                    {
                //                        if (Monto > fila.Capital)
                //                        {
                //                            _detalleRecibosBE.Capital = fila.Capital;
                //                            Monto = Monto - fila.Capital;
                //                            Salir = false;
                //                        }
                //                        else
                //                        {
                //                            _detalleRecibosBE.Capital = Monto;
                //                            Monto = Monto - fila.Capital;
                //                            Salir = true;
                //                        }
                //                    }

                //                    _detalleRecibosBE.Concepto = clsLenguajeBO.Find("colAbono") + string.Format("{0:000}", fila.Numero);
                //                    _detalleRecibosBE.CuotaID = fila.CuotaID;
                //                    _detalleRecibosBE.Numero = fila.Numero;
                //                    _detalleRecibosBE.SubTotal = (_detalleRecibosBE.Seguro + _detalleRecibosBE.Legal + _detalleRecibosBE.Mora + _detalleRecibosBE.Comision + _detalleRecibosBE.Interes + _detalleRecibosBE.Capital);

                //                    int i = 0; bool add = true;
                //                    foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                //                    {
                //                        if (row.CuotaID == _detalleRecibosBE.CuotaID)
                //                        {
                //                            detalleRecibosBE[i].Concepto = _detalleRecibosBE.Concepto;
                //                            detalleRecibosBE[i].Capital += _detalleRecibosBE.Capital;
                //                            detalleRecibosBE[i].Comision += _detalleRecibosBE.Comision;
                //                            detalleRecibosBE[i].Interes += _detalleRecibosBE.Interes;
                //                            detalleRecibosBE[i].Mora += _detalleRecibosBE.Mora;
                //                            detalleRecibosBE[i].Legal += _detalleRecibosBE.Legal;
                //                            detalleRecibosBE[i].Seguro += _detalleRecibosBE.Seguro;
                //                            detalleRecibosBE[i].SubTotal = (detalleRecibosBE[i].Seguro + detalleRecibosBE[i].Legal + detalleRecibosBE[i].Mora + detalleRecibosBE[i].Comision + detalleRecibosBE[i].Interes + detalleRecibosBE[i].Capital);
                //                            add = false;
                //                        }
                //                        i++;
                //                    }
                //                    if (add == true && _detalleRecibosBE.SubTotal > 0.5)
                //                    {
                //                        detalleRecibosBE.Add(_detalleRecibosBE);
                //                    }
                //                    Monto = Monto - (_detalleRecibosBE.Seguro + _detalleRecibosBE.Legal + _detalleRecibosBE.Mora + _detalleRecibosBE.Comision + _detalleRecibosBE.Interes + _detalleRecibosBE.Capital);
                //                }
                //            }
                //        }


                //        dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                //        dataGrid1.ItemsSource = detalleRecibosBE;
                //        CalcularTotales();
                //        Monto = 0;
                //    }
                //    catch { }
                //}

                #endregion


        private void CalcularTotales()
        {
            try
            {
                double SubTotal = 0, Descuento = 0;
                foreach (clsDetalleRecibosBE Fila in detalleRecibosBE)
                {
                    SubTotal = SubTotal + Convert.ToDouble(Fila.Capital) + Convert.ToDouble(Fila.Comision) + Convert.ToDouble(Fila.Interes) + Convert.ToDouble(Fila.Mora) + Convert.ToDouble(Fila.Legal) + Convert.ToDouble(Fila.Seguro);
                }
                Descuento = Convert.ToDouble(txtDescuento.Text);
                txtSubTotal.Text = string.Format("{0:N3}", SubTotal);
                txtTotal.Text = string.Format("{0:N3}", SubTotal - Convert.ToDouble(txtDescuento.Text));
            }
            catch (Exception ex)
            { }
        }

        private void txtContrato_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtContrato.Text))
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
                DesHabilitarOpciones();
                LimpiarEncabezado();
                LimpiarDetalleRecibo();
                LimpiarOpciones();
                cmbComprobantes.SelectedValue = _ComprobanteID;
                cmbFormaPagos.SelectedValue = _FormaPagoID;
                txtInformacion.Text = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Informacion;
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
                gridEncabezado.DataContext = new clsRecibosBE();
                _ContratoID = 0;
                _ReciboID = 0;
                _TipoContratoID = 0;
                VISTA = 1;
                detalleRecibosBE = new List<clsDetalleRecibosBE>();

                cuotasBE = new List<BoxReportItem>();
                clsValidacionesBO.Limpiar(gridEncabezado);
                clsValidacionesBO.Limpiar(gridDetalleCuotas);
                txtContrato.Focus();
            }
            catch { }
        }

        void LimpiarDetalleRecibo()
        {
            try
            {
                detalleRecibosBE = new List<clsDetalleRecibosBE>();
                dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
            }
            catch { }
        }

        void LimpiarOpciones()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridOpciones);
                txtSubTotal.Text = string.Format("{0:N3}", 0);
                txtDescuento.Text = string.Format("{0:N3}", 0);
                txtTotal.Text = string.Format("{0:N3}", 0);
            }
            catch { }
        }

        private void frmRecibos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                gridOpciones.DataContext = new clsRecibosBE();
                if (VISTA == 1)
                {
                    txtFecha.SelectedDate = DateTime.Today;
                    txtInformacion.Text = string.IsNullOrEmpty(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Informacion)== true? "": clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Informacion;
                }
                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == false)
                {
                    txtFecha.IsEnabled = false;
                    txtDescuento.IsReadOnly = true;
                }
                else
                { CalcularTotales(); }
                txtContrato.Focus();

                chkDocument.IsChecked = clsCookiesBO.getPrintReceipt();
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
