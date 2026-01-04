using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Payment.Bank.Entity;


using Payment.Bank.Modulos;
using Payment.Bank.Core;
using System.Collections.ObjectModel;
using Payment.Bank.Model;
using System.Windows.Media;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmCierres : Window
    {
  
        Core.Manager db = new Manager();
        int VISTA = 1;
        float _montoInicial = 0;
        float _efectivos = 0;
        float _caja = 0;
        public ObservableCollection<Denominaciones> Denominaciones { get; set; }
        List<Core.Entity.Recibos> Recibos = new List<Core.Entity.Recibos>();
        List<Core.Entity.Recibos> Contratos = new List<Core.Entity.Recibos>();
        List<Core.Entity.Recibos> Rentas = new List<Core.Entity.Recibos>();
       
        string _documento = "-1";

        public frmCierres()
        {
            InitializeComponent();
            ClearAll();
            LoadCombox();

            Loaded += frmCierres_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            txtFecha.SelectedDateChanged += txtFecha_SelectedDateChanged;
            cmbCajas.SelectionChanged += cmbCajas_SelectionChanged;

            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridHeader);
            clsLenguajeBO.Load(gridRecibos);
            clsLenguajeBO.Load(gridDenomination);
            clsLenguajeBO.Load(gridDetalleRecibos);
            clsLenguajeBO.Load(gridRentas);
            clsLenguajeBO.Load(gridContratos);
            clsLenguajeBO.Load(gridCashBalance); 

            gbRecibos.Header = clsLenguajeBO.Find(gbRecibos.Header.ToString());
            gbDetalleRecibos.Header = clsLenguajeBO.Find(gbDetalleRecibos.Header.ToString());
            gbPagoRentas.Header = clsLenguajeBO.Find(gbPagoRentas.Header.ToString());
            gbDenomination.Header = clsLenguajeBO.Find(gbDenomination.Header.ToString());
            gbContratos.Header = clsLenguajeBO.Find(gbContratos.Header.ToString());
            gbCashBalance.Header = clsLenguajeBO.Find(gbCashBalance.Header.ToString());
            gbDenomination.Header = clsLenguajeBO.Find(gbDenomination.Header.ToString());
            txtFecha.SelectedDate = DateTime.Today;
            gridHeader.DataContext = new clsTurnosBE { Documento = _documento };
        }

        private void cmbCajas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            LoadData();
        }

        void LoadCombox()
        {
            try
            {
                List<clsPersonasBE> Cajeros = db.OpenBoxGetByFecha((DateTime)txtFecha.SelectedDate);
                Cajeros.Add(new clsPersonasBE { Documento = "-1", Nombres = clsLenguajeBO.Find("itemAll") });
                cmbCajas.ItemsSource = Cajeros;
                cmbCajas.SelectedValuePath = "Documento";
                cmbCajas.DisplayMemberPath = "Nombres";
                if(clsVariablesBO.UsuariosBE.Roles.IsAdmin)
                {
                    cmbCajas.IsEnabled = true;
                    txtFecha.IsEnabled = true;
                    cmbCajas.SelectedValue = "-1";
                }
                else
                {
                    cmbCajas.IsEnabled = false;
                    txtFecha.IsEnabled = false;
                    cmbCajas.SelectedValue = clsVariablesBO.UsuariosBE.Documento;
                }

             
                _documento = cmbCajas.SelectedValue.ToString();  
                LoadData();


            }
            catch (Exception ex) {  }
        }

        private void txtFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCombox();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridHeader) && Denominaciones.Count() > 0 && (string)cmbCajas.SelectedValue !="-1")
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        int _TurnoID = db.OpenBoxGetByDocumento((string)cmbCajas.SelectedValue, (DateTime)txtFecha.SelectedDate).TurnoID;
                        OperationResult result = db.ArqueosCreate(_TurnoID, (_efectivos + _montoInicial - _caja), float.Parse(Denominaciones.Sum(x => x.SubTotal).ToString()), $"{lblSurplusBox.Text}{lblSobrante.Text}", clsVariablesBO.UsuariosBE.Documento); ;
                        if (result.ResponseCode == "00")
                        {
                            foreach(var item in Denominaciones)
                            {
                                db.DetalleArqueosCreate(int.Parse(result.ResponseMessage), item.DenominacionID, item.Cantidad, (float)item.SubTotal, clsVariablesBO.UsuariosBE.Documento);
                            }
                            var response =  db.TurnosDelete(_TurnoID);
                            if (response.ResponseCode == "00")
                            {
                                //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                frmPrintArqueosGetByArqueoID forms = new frmPrintArqueosGetByArqueoID();
                                forms.Owner = this;
                                forms.OnInit(int.Parse(result.ResponseMessage));
                                forms.ShowDialog();
                                
                                LoadCombox();
                                LoadDenominaciones();
                               
                                ClearAll(); 
                                CalcularCuadre();
                            }
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
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridHeader))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            //OperationResult result = db.CajasUpdate(BE.CajaID, txtCaja.Text, clsVariablesBO.UsuariosBE.Documento);
                            //if (result.ResponseCode == "00")
                            //{
                            //    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                            //    Limpiar();
                            //}
                            //else
                            //{
                            //    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            //}
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                        }
                    }
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmCierres_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDenominaciones();

            }
            catch { }
        }

        private void LoadData()
        {

            ClearAll();
            _documento = (string)cmbCajas.SelectedValue;
            loadRecibos();
            loadDetalleRecibos();
            loadPagoRentas();
            loadContratos();
            CalcularCuadre();
        }

        void CalcularCuadre()
        {
            try
            {
                List<clsTurnosBE> BE = new List<clsTurnosBE>();
                var cajeros = (List<clsPersonasBE>)cmbCajas.ItemsSource;
                if (cajeros.Count() > 1)
                {
                    foreach (var item in cajeros)
                    {
                        if (item.Usuarios != null)
                        {
                            BE.Add(item.Usuarios.Turnos.Where(x=>x.EstadoID==true).FirstOrDefault());
                        }
                    }
                }

                _montoInicial = (cmbCajas.SelectedItem as clsPersonasBE).Usuarios == null ? BE.Sum(x=>x.Monto) : (cmbCajas.SelectedItem as clsPersonasBE).Usuarios.Turnos.Where(x => x.EstadoID == true).Sum(x => x.Monto);
                lblMontoInicial.Text = string.Format("{0:C}", _montoInicial);

               

                _efectivos= (Recibos.Count() == 0 ? 0 : Recibos.Where(x => x.FormaPagoID == 1).Sum(x => x.Monto)) + (Rentas.Count() == 0 ? 0 : Rentas.Where(x => x.FormaPagoID == 1).Sum(x => x.Monto));
                float _tarjetas = (Recibos.Count() == 0 ? 0 : Recibos.Where(x => x.FormaPagoID == 2).Sum(x => x.Monto)) + (Rentas.Count() == 0 ? 0 : Rentas.Where(x => x.FormaPagoID == 2).Sum(x => x.Monto));
                float _cheques = (Recibos.Count() == 0 ? 0 : Recibos.Where(x => x.FormaPagoID == 3).Sum(x => x.Monto)) + (Rentas.Count() == 0 ? 0 : Rentas.Where(x => x.FormaPagoID == 3).Sum(x => x.Monto));
                float _transferencias = (Recibos.Count() == 0 ? 0 : Recibos.Where(x => x.FormaPagoID >= 4).Sum(x => x.Monto)) + (Rentas.Count() == 0 ? 0 : Rentas.Where(x => x.FormaPagoID >= 4).Sum(x => x.Monto));

                lblIngresos.Text = string.Format("{0:C}", Recibos.Sum(x=>x.Monto) + Rentas.Sum(x => x.Monto));

                lblEfectivo.Text = string.Format("{0:C}", _efectivos);
                lblTotalTarjetas.Text = string.Format("{0:C}", _tarjetas);
                lblTotalCheques.Text = string.Format("{0:C}", _cheques);
                lblTotalTransferencias.Text = string.Format("{0:C}", _transferencias);

                lblegresos.Text = string.Format("{0:C}", Contratos.Sum(x => x.Monto));
                _caja = Contratos.Count() == 0 ? 0 : Contratos.Where(x => x.FormaPagoID == 1).Sum(x => x.Monto);
                float _bancos = Contratos.Count() == 0 ? 0 : Contratos.Where(x => x.FormaPagoID != 1).Sum(x => x.Monto);
                lblTotalCajaGeneral.Text = string.Format("{0:C}", _caja);
                lblTotalBancos.Text = string.Format("{0:C}", _bancos);


                lblArquero.Text = string.Format("{0:C}", _efectivos + _montoInicial - _caja);
                lblSobrante.Text = string.Format("{0:C}", _efectivos + _montoInicial - _caja - float.Parse(Denominaciones.Sum(x => x.SubTotal).ToString()));
                float _direfencia = (_efectivos + _montoInicial) - (_caja + float.Parse(Denominaciones.Sum(x => x.SubTotal).ToString()));

                if (_direfencia < 0 )
                {
                    lblSurplusBox.Foreground = new SolidColorBrush(Colors.Red);
                    lblSobrante.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    lblSurplusBox.Foreground = new SolidColorBrush(Colors.Black);
                    lblSobrante.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            catch(Exception ex) 
            { }
        }


        void ClearAll()
        {

            Recibos = new List<Core.Entity.Recibos>();
            Rentas = new List<Core.Entity.Recibos>();
            Contratos = new List<Core.Entity.Recibos>();

            dtgRecibos.ItemsSource = Recibos;
            dtgDetalleRecibos.ItemsSource = Recibos;
            dtgPagoRentas.ItemsSource = Rentas;
            dtgContratos.ItemsSource = Contratos;

            _montoInicial = 0;
             _efectivos = 0;
            _caja = 0;

            lblTotalDetalleRecibos.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", 0)}";
            lblTotalContratos.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", 0)}";
            lblTotalRecibos.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", 0)}";
            lblTotalPagoRentas.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", 0)}";
        }

        private void loadDetalleRecibos()
        {
            try
            {
                var result = db.DetalleRecibosGroupByFecha((DateTime)txtFecha.SelectedDate, _documento);
                dtgDetalleRecibos.ItemsSource = result;
                lblTotalDetalleRecibos.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", result.Sum(x => x.SubTotal))}";
            }
            catch { }
        }

        private void loadContratos()
        {
            try
            {
                Contratos = new List<Core.Entity.Recibos>();
                Contratos = db.ContratosGroupByFormaPago((DateTime)txtFecha.SelectedDate, _documento);
                dtgContratos.ItemsSource = Contratos;
                lblTotalContratos.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", Contratos.Sum(x => x.Monto))}";
            }
            catch { }
        }

        private void loadRecibos()
        {
            try
            {
                Recibos = new List<Core.Entity.Recibos>();
                Recibos = db.RecibosGroupByFormaPago((DateTime)txtFecha.SelectedDate, _documento);
                dtgRecibos.ItemsSource = Recibos;
                lblTotalRecibos.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", Recibos.Sum(x => x.Monto))}";
            }
            catch(Exception ex)
            { }
        }

        private void loadPagoRentas()
        {
            try
            {
                Rentas = new List<Core.Entity.Recibos>();
                Rentas = db.RentasGroupByFormaPago((DateTime)txtFecha.SelectedDate, _documento);
                dtgPagoRentas.ItemsSource = Rentas;
                lblTotalPagoRentas.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", Rentas.Sum(x => x.Monto))}";
            }
            catch { }
        }

        private void CalcularTotal()
        {
            lblTotalEfectivo.Text = $"{clsLenguajeBO.Find("lblTotal")}: {string.Format("{0:C}", Denominaciones.Sum(x => x.SubTotal))}";
            CalcularCuadre();
        }

        private void LoadDenominaciones()
        {
            try
            {
                Denominaciones = new ObservableCollection<Denominaciones>();
                foreach (var item in db.DenominacionesGet())
                {
                    Denominaciones.Add(new Denominaciones {DenominacionID = item.DenominacionID, Denominacion = item.Denominacion, Cantidad = 0 });
                }
                dtgDenominaciones.ItemsSource = Denominaciones;
                CalcularTotal();
            }
            catch { }
        }

 

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CalcularTotal();
        }
    }
}