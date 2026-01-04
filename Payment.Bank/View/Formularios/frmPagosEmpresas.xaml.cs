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
using System.Xml.Linq;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes;
using System.Data;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmPagosEmpresas : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        public List<BoxReportItem> cuotasBE;
        List<clsDetalleRecibosBE> detalleRecibosBE;

        DataSet ds = new DataSet();
        public frmPagosEmpresas()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
           
                btnAceptar.Click += btnAceptar_Click;
                btnSalir.Click += btnSalir_Click;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                txtHasta.SelectedDate = DateTime.Today;
                cmbSucursales.SelectionChanged += cmbSucursales_SelectionChanged;
                cmbInstituciones.SelectionChanged += cmbInstituciones_SelectionChanged;
                txtHasta.SelectedDateChanged += txtHasta_SelectedDateChanged;
                LoadCombox();
            }
            catch{}
        }

        private void txtHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch { }
        }

        private void cmbInstituciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch { }
        }

        private void cmbSucursales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                LoadData();
            }
            catch { }
        }

        void LoadCombox()
        {
            try
            {
                //DataContext = new clsSucursalesBE();
                List<clsSucursalesBE> List = new List<clsSucursalesBE>();
                List = db.SucursalesGet(null).ToList();

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.IsEnabled = true;
                    List.Add(new clsSucursalesBE { SucursalID = -1, Sucursal = clsLenguajeBO.Find("itemAll") });
                }
                else
                {
                    cmbSucursales.IsEnabled = false;
                }
                cmbSucursales.ItemsSource = List;
                cmbSucursales.SelectedValuePath = "SucursalID";
                cmbSucursales.DisplayMemberPath = "Sucursal";

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.SelectedValue = -1;
                    txtHasta.IsEnabled = true;
                }
                else
                {
                    cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
                    txtHasta.IsEnabled = false;
                }

                List<clsInstitucionesBE> Business = new List<clsInstitucionesBE>();
                Business = db.InstitucionesGet(null).ToList();
                Business.Add(new clsInstitucionesBE { InstitucionID = -1, Institucion = clsLenguajeBO.Find("itemAll") });
                cmbInstituciones.ItemsSource = Business;
                cmbInstituciones.SelectedValuePath = "InstitucionID";
                cmbInstituciones.DisplayMemberPath = "Institucion";
                cmbInstituciones.SelectedValue = -1;

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           try
            {
                     
               
            }
            catch { }
        }

       
        private void LoadData()
        {
            try
            {
                ds = db.PrintListadoCobroGetByInstitucionID((DateTime)txtHasta.SelectedDate, (int)cmbSucursales.SelectedValue, (int)cmbInstituciones.SelectedValue);
                dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
                lblCantidad.Text = clsLenguajeBO.Find("TOTAL:") + " " + ds.Tables[0].Rows.Count.ToString();
                CalcularTotales();
            }
            catch { }
        }


        private void CalcularTotales()
        {
            try
            {
                decimal Monto = 0;
                foreach (DataRowView items in dataGrid1.Items)
                {
                    var value = dataGrid1.Columns[6].GetCellContent(items).GetChildObjects(true).ToList();
                    TextBox txt = (TextBox)value[0] as TextBox;
                    Monto += Convert.ToDecimal(txt.Text);
                }
                lblTotal.Text = string.Format("{0:N2}", Monto);                
            }
            catch { }
        }

        //bool ValidarPago()
        //{
        //    try
        //    {
        //        bool Continuar = false;
        //        double Valor = double.Parse(txtMonto.Text);
        //        if (Valor >= PagoMinimo)
        //        {
        //            Continuar = true;
        //        }
        //        return Continuar;
        //    }
        //    catch { return false; }
        //}

        void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                int _ContratoID = 0;
                if (dataGrid1.Items.Count > 0)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {

                        frmMessageBox msgBox = new frmMessageBox();
                        msgBox.OnInit(clsLenguajeBO.Find("msgAutomaticReceipt"), clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        msgBox.btnAceptar.Click += (arg, obj) =>
                        {
                            foreach (DataRowView items in dataGrid1.Items)
                            {
                                RadBusyIndicator.IsActive = true;
                                var x = dataGrid1.Columns[0].GetCellContent(items).GetChildObjects(true).ToList();
                                TextBlock lbl = (TextBlock)x[0] as TextBlock;
                                _ContratoID = Convert.ToInt32(lbl.Text);

                                if (_ContratoID > 0)
                                {
                                    var value = dataGrid1.Columns[6].GetCellContent(items).GetChildObjects(true).ToList();
                                    TextBox txt = (TextBox)value[0] as TextBox;
                                    float Monto = float.Parse(txt.Text);
                                    if (Monto > 0)
                                    {
                                        PagosAutomaticos(_ContratoID, Monto);
                                        float SubTotal = CalcularTotalesDetalle();
                                        if (SubTotal > 0)
                                        {
                                            OperationResult result = db.RecibosCreate(_ContratoID, (DateTime)txtHasta.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, 0, 0, SubTotal, 0, SubTotal, 0,0, "PAGOS AUTOMATICOS EN FECHA " + txtHasta.SelectedDate.Value.Add(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)).ToString(), clsVariablesBO.UsuariosBE.Documento);
                                            if (result.ResponseCode == "00")
                                            {
                                                DetalleRecibosCreate(Convert.ToInt32(result.ResponseMessage), _ContratoID);
                                            }
                                        }
                                    }
                                }
                            }
                            msgBox.Close();
                            RadBusyIndicator.IsActive = false;
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                            LoadData();
                        };
                        msgBox.btnSalir.Click += (arg, obj) => { msgBox.Close(); };
                        msgBox.Owner = this;
                        msgBox.ShowDialog();
                    }
                    else
                    {
                        RadBusyIndicator.IsActive = false;
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgNoData"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch(Exception ex) { }
        }

        private float CalcularTotalesDetalle()
        {
            try
            {
                float SubTotal = 0;
                foreach (clsDetalleRecibosBE Fila in detalleRecibosBE)
                {
                    SubTotal = SubTotal + Fila.Capital + Fila.Comision + Fila.Interes + Fila.Mora + Fila.Legal;
                }

                return SubTotal;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void DetalleRecibosCreate(int ReciboID, int ContratoID)
        {
            try
            {
                db.DetalleRecibosDeleteGetByReciboID(ReciboID);
                OperationResult result = new OperationResult();
                foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                {
                    result = db.DetalleRecibosCreate(row.CuotaID, ReciboID, row.Numero, row.Concepto, row.Capital, row.Comision, row.Interes, row.Mora, row.Legal, row.Seguro, row.SubTotal, clsVariablesBO.UsuariosBE.Documento);
                }

                if (result.ResponseCode == "00")
                {
                    clsCuotasView Cuotas = new clsCuotasView();
                    Cuotas.SetDataSource(ContratoID, ReciboID);
                    var R = Cuotas.GetGroup();
                    var balance = (Cuotas.BalanceGetByReciboID() < 0 ? 0 : Cuotas.BalanceGetByReciboID());

                    if (balance <= 0)
                    {
                       db.ContratosDeleteGetByContratoID(ContratoID, 2, "SALDO A CONTRATO #" + ContratoID.ToString(), clsVariablesBO.UsuariosBE.Documento);
                    }

                }

            }
            catch { }
        }

        public void PagosAutomaticos(int ContratoID, float Monto)
        {
            try
            {

                clsCuotasView Cuotas = new clsCuotasView();
                Cuotas.SetDataSource(ContratoID);
                var result = Cuotas.GetGroup();

                cuotasBE = new List<BoxReportItem>();
                cuotasBE = result.ToList();

                detalleRecibosBE = new List<clsDetalleRecibosBE>();
                clsDetalleRecibosBE _detalleRecibosBE;
                foreach (BoxReportItem fila in cuotasBE.ToList())
                {
                    bool Salir = false;
                    _detalleRecibosBE = new clsDetalleRecibosBE();

                    if (fila.Balance > 0 && Monto > 0)
                    {
                        if (Monto >= fila.Balance)
                        {
                            _detalleRecibosBE.Legal = fila.Legal;
                            _detalleRecibosBE.Mora = fila.Mora;
                            _detalleRecibosBE.Interes = fila.Interes;
                            _detalleRecibosBE.Comision = fila.Comision;
                            _detalleRecibosBE.Capital = fila.Capital;
                            _detalleRecibosBE.CuotaID = fila.CuotaID;
                            _detalleRecibosBE.Concepto = clsLenguajeBO.Find("colSaldo") + string.Format("{0:000}", fila.Numero);
                            _detalleRecibosBE.Numero = fila.Numero;
                            _detalleRecibosBE.SubTotal = fila.Legal + fila.Mora + fila.Comision + fila.Interes + fila.Capital;

                            int i = 0; bool add = true;
                            foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                            {
                                if (row.CuotaID == _detalleRecibosBE.CuotaID)
                                {
                                    detalleRecibosBE[i].Concepto = _detalleRecibosBE.Concepto;
                                    detalleRecibosBE[i].Capital += _detalleRecibosBE.Capital;
                                    detalleRecibosBE[i].Comision += _detalleRecibosBE.Comision;
                                    detalleRecibosBE[i].Interes += _detalleRecibosBE.Interes;
                                    detalleRecibosBE[i].Mora += _detalleRecibosBE.Mora;
                                    detalleRecibosBE[i].Legal += _detalleRecibosBE.Legal;
                                    detalleRecibosBE[i].SubTotal = (detalleRecibosBE[i].Legal + detalleRecibosBE[i].Mora + detalleRecibosBE[i].Comision + detalleRecibosBE[i].Interes + detalleRecibosBE[i].Capital);
                                    add = false;
                                }
                                i++;
                            }
                            if (add == true)
                            {
                                detalleRecibosBE.Add(_detalleRecibosBE);
                            }
                            Monto = Monto - (fila.Legal + fila.Mora + fila.Comision + fila.Interes + fila.Capital);
                        }
                        else
                        {

                            if (Monto > fila.Legal)
                            {
                                _detalleRecibosBE.Legal = fila.Legal;
                                Monto = Monto - fila.Legal;
                                Salir = false;
                            }
                            else
                            {
                                _detalleRecibosBE.Legal = Monto;
                                Monto = Monto - fila.Legal;
                                Salir = true;
                            }

                            if (Salir == false)
                            {
                                if (Monto > fila.Mora)
                                {
                                    _detalleRecibosBE.Mora = fila.Mora;
                                    Monto = Monto - fila.Mora;
                                    Salir = false;
                                }
                                else
                                {
                                    _detalleRecibosBE.Mora = Monto;
                                    Monto = Monto - fila.Mora;
                                    Salir = true;
                                }
                            }

                            if (Salir == false)
                            {
                                if (Monto > fila.Interes)
                                {
                                    _detalleRecibosBE.Interes = fila.Interes;
                                    Monto = Monto - fila.Interes;
                                    Salir = false;
                                }
                                else
                                {
                                    _detalleRecibosBE.Interes = Monto;
                                    Monto = Monto - fila.Interes;
                                    Salir = true;
                                }
                            }

                            if (Salir == false)
                            {
                                if (Monto > fila.Comision)
                                {
                                    _detalleRecibosBE.Comision = fila.Comision;
                                    Monto = Monto - fila.Comision;
                                    Salir = false;
                                }
                                else
                                {
                                    _detalleRecibosBE.Comision = Monto;
                                    Monto = Monto - fila.Comision;
                                    Salir = true;
                                }
                            }

                            if (Salir == false)
                            {
                                if (Monto > fila.Capital)
                                {
                                    _detalleRecibosBE.Capital = fila.Capital;
                                    Monto = Monto - fila.Capital;
                                    Salir = false;
                                }
                                else
                                {
                                    _detalleRecibosBE.Capital = Monto;
                                    Monto = Monto - fila.Capital;
                                    Salir = true;
                                }
                            }
                            _detalleRecibosBE.Concepto = clsLenguajeBO.Find("colAbono") + string.Format("{0:000}", fila.Numero);
                            _detalleRecibosBE.CuotaID = fila.CuotaID;
                            _detalleRecibosBE.Numero = fila.Numero;
                            _detalleRecibosBE.SubTotal = (_detalleRecibosBE.Legal + _detalleRecibosBE.Mora + _detalleRecibosBE.Comision + _detalleRecibosBE.Interes + _detalleRecibosBE.Capital);

                            int i = 0; bool add = true;
                            foreach (clsDetalleRecibosBE row in detalleRecibosBE)
                            {
                                if (row.CuotaID == _detalleRecibosBE.CuotaID)
                                {
                                    detalleRecibosBE[i].Concepto = _detalleRecibosBE.Concepto;
                                    detalleRecibosBE[i].Capital += _detalleRecibosBE.Capital;
                                    detalleRecibosBE[i].Comision += _detalleRecibosBE.Comision;
                                    detalleRecibosBE[i].Interes += _detalleRecibosBE.Interes;
                                    detalleRecibosBE[i].Mora += _detalleRecibosBE.Mora;
                                    detalleRecibosBE[i].Legal += _detalleRecibosBE.Legal;
                                    detalleRecibosBE[i].SubTotal = (detalleRecibosBE[i].Legal + detalleRecibosBE[i].Mora + detalleRecibosBE[i].Comision + detalleRecibosBE[i].Interes + detalleRecibosBE[i].Capital);
                                    add = false;
                                }
                                i++;
                            }
                            if (add == true)
                            {
                                detalleRecibosBE.Add(_detalleRecibosBE);
                            }
                            Monto = Monto - (_detalleRecibosBE.Legal + _detalleRecibosBE.Mora + _detalleRecibosBE.Comision + _detalleRecibosBE.Interes + _detalleRecibosBE.Capital);
                        }
                    }
                }
                //dataGrid1.ItemsSource = new List<clsDetalleRecibosBE>();
                //dataGrid1.ItemsSource = detalleRecibosBE;
                //CalcularTotales();
                Monto = 0;
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtPagado_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal Pagado = 0;
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Pagado = Convert.ToDecimal(((TextBox)sender).Text);
                }        
                ((TextBox)sender).Text = string.Format("{0:N2}", Pagado);
            }
            catch { }
        }

        private void txtPagado_KeyDown(object sender, KeyEventArgs e)
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

        private void txtPagado_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            { CalcularTotales(); }
            catch { }
        }
    }


}

