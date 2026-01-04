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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Payment.Bank.Modulos;
using Payment.Bank.Entity;
using Payment.Bank.Core;
using Payment.Bank.View.Consultas;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmEntradas.xaml
    /// </summary>
    public partial class frmEntradas : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        clsEntradasBE EntradaBE = new clsEntradasBE();
        List<clsDetalleEntradasBE> detalleEntradasBE = new List<clsDetalleEntradasBE>();
        clsDetalleEntradasBE detalleBE = new clsDetalleEntradasBE();
        clsAuxiliaresBE BE = new clsAuxiliaresBE();
        public frmEntradas()
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridInformacion);
            clsLenguajeBO.Load(gridEncabezado);
            clsLenguajeBO.Load(gridDetalleEntradas);
            clsLenguajeBO.Load(gridProducto);
            Title = clsLenguajeBO.Find(Title.ToString());
            
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            if (VISTA == 1)
            {
                LoadCombox();
            }
            txtFecha.SelectedDate = DateTime.Today;
            txtCodigo.KeyDown += txtCodigo_KeyDown;

            dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                detalleBE = (clsDetalleEntradasBE)dataGrid1.SelectedItem;
            }
            catch { }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((int)cmbTipoMovimientos.SelectedValue == -1 || cmbTipoMovimientos.SelectedItem == null )
                {
                    cmbTipoMovimientos.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtReferencia.Text))
                {
                    txtReferencia.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtConcepto.Text))
                {
                    txtConcepto.Focus();
                    return;
                }


                if (detalleEntradasBE != null)
                {
                    if (float.Parse(string.IsNullOrEmpty(txtDebito.Text) ? "0": txtDebito.Text) == 0 && float.Parse(string.IsNullOrEmpty(txtCredito.Text) ? "0" : txtCredito.Text) == 0)
                    {
                        return;
                    }

                    foreach (var row in detalleEntradasBE)
                    {
                        if (row.AuxiliarID == BE.AuxiliarID)
                        {
                            return;
                        }
                    }


                    detalleEntradasBE.Add(new clsDetalleEntradasBE { AuxiliarID = BE.AuxiliarID, EstadoID  = true, EntradaID = detalleEntradasBE.Count() == 0 ? 0 : detalleEntradasBE.FirstOrDefault().EntradaID, Numero = 0, TipoEntradaID = 0, Usuario = clsVariablesBO.UsuariosBE.Documento, ModificadoPor = clsVariablesBO.UsuariosBE.Documento, FechaModificacion = DateTime.Now, Debito =string.IsNullOrEmpty(txtDebito.Text) ? 0 : float.Parse(txtDebito.Text) , Credito = string.IsNullOrEmpty(txtCredito.Text) ? 0 : float.Parse(txtCredito.Text), DetalleEntradaID = detalleEntradasBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = BE.Codigo, Auxiliar = BE.Auxiliar, AuxiliarID = BE.AuxiliarID }, TipoEntradas = new clsTipoEntradasBE { TipoEntradaID = 0, TipoEntrada= "Contabilidad"  } });

                    //BE = new clsAuxiliaresBE();

                    dataGrid1.ItemsSource = new List<clsDetalleSolicitudChequesBE>();
                    dataGrid1.ItemsSource = detalleEntradasBE;

                    txtCodigo.Text = String.Empty;
                    txtAuxiliar.Text = String.Empty;
                    txtDebito.Text = String.Empty;
                    txtCredito.Text = String.Empty;
                    CalcularTotales();
                    txtCodigo.Focus();
                }
                else
                {
                    txtCodigo.Focus();
                }
            }
            catch(Exception ex)
            {

                try
                {

                    BE = new clsAuxiliaresBE();

                    dataGrid1.ItemsSource = new List<clsDetalleSolicitudChequesBE>();
                    dataGrid1.ItemsSource = detalleEntradasBE;

                    txtCodigo.Text = String.Empty;
                    txtAuxiliar.Text = String.Empty;
                    txtDebito.Text = String.Empty;
                    txtCredito.Text = String.Empty;
                    CalcularTotales();
                    txtCodigo.Focus();
                }
                catch { }
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEncabezado) && float.Parse(lblTotalDebito.Text) == float.Parse(lblTotalCredito.Text))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.EntradasCreate((DateTime)txtFecha.SelectedDate, (int)cmbTipoMovimientos.SelectedValue, clsVariablesBO.UsuariosBE.SucursalID, txtReferencia.Text, txtConcepto.Text, float.Parse(lblTotalDebito.Text),float.Parse(lblTotalCredito.Text), clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            db.DetalleEntradasDelete(int.Parse(result.ResponseMessage));
                            foreach (var item in detalleEntradasBE.ToList())
                            {
                                db.DetalleEntradasCreate(int.Parse(result.ResponseMessage), item.AuxiliarID, 0, 0, item.Debito, item.Credito, clsVariablesBO.UsuariosBE.Documento);
                            }
                            GetReport(int.Parse(result.ResponseMessage));
                            //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                            Limpiar();
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridEncabezado) && float.Parse(lblTotalDebito.Text) == float.Parse(lblTotalCredito.Text))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.EntradasUpdate(EntradaBE.EntradaID, (DateTime)txtFecha.SelectedDate, (int)cmbTipoMovimientos.SelectedValue, clsVariablesBO.UsuariosBE.SucursalID, txtReferencia.Text, txtConcepto.Text, float.Parse(lblTotalDebito.Text), float.Parse(lblTotalCredito.Text), clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                db.DetalleEntradasDelete(EntradaBE.EntradaID);
                                foreach (var item in detalleEntradasBE.ToList())
                                {
                                    db.DetalleEntradasCreate(EntradaBE.EntradaID, item.AuxiliarID, item.TipoEntradaID, item.Numero, item.Debito, item.Credito, clsVariablesBO.UsuariosBE.Documento);
                                }
                                GetReport(EntradaBE.EntradaID);
                                //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                Limpiar();
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
            catch { }
        }

        private void Limpiar()
        {
          try
            {
                VISTA = 1;
                gridEncabezado.DataContext = new clsEntradasBE();
                gridDetalleEntradas.DataContext = new clsAuxiliaresBE();
                EntradaBE = new clsEntradasBE();
                detalleEntradasBE = new List<clsDetalleEntradasBE>();
                detalleBE = new clsDetalleEntradasBE();
                BE = new clsAuxiliaresBE();
                dataGrid1.ItemsSource = detalleEntradasBE;
                CalcularTotales();
                txtFecha.SelectedDate = DateTime.Now;
            }
            catch { }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (detalleBE != null)
                {
                    foreach (var row in detalleEntradasBE)
                    {
                        if (row.AuxiliarID == detalleBE.AuxiliarID)
                        {
                            detalleEntradasBE.Remove(row);
                            dataGrid1.ItemsSource = new List<clsDetalleEntradasBE>();
                            dataGrid1.ItemsSource = detalleEntradasBE;
                        }
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
                CalcularTotales();
            }
            catch { CalcularTotales(); }
        }

        void CalcularTotales()
        {
            try
            {
                float debitos = 0;
                float creditos = 0;

                lblTotalDebito.Text = string.Format("{0:N2}", debitos);
                lblTotalCredito.Text = string.Format("{0:N2}", creditos);

                foreach (var row in detalleEntradasBE)
                {
                    debitos += row.Debito;
                    creditos += row.Credito;
                }
                lblTotalDebito.Text = string.Format("{0:N2}", debitos);
                lblTotalCredito.Text = string.Format("{0:N2}", creditos);
                if (debitos == creditos)
                {
                    lblMessage.Text = string.Empty;
                }
                else
                {
                    lblMessage.Text = clsLenguajeBO.Find("AccountBalanced");
                }
            }
            catch { }
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                frmConsultasAuxiliares form = new frmConsultasAuxiliares();
                form.isQuery = true;
                form.Owner = this;
                form.Closed += (obj, arg) =>
                {
                    BE = form.BE;
                    txtCodigo.Text = BE.Codigo;
                    txtAuxiliar.Text = BE.Auxiliar;
                    if (float.Parse(lblTotalDebito.Text) == float.Parse(lblTotalCredito.Text))
                    {
                        txtDebito.Focus();
                    }
                    else
                    {
                        if (float.Parse(lblTotalDebito.Text) > float.Parse(lblTotalCredito.Text))
                        {
                            txtCredito.Focus();
                        }
                        else
                        {
                            txtDebito.Focus();
                        }
                    }
                };
                form.ShowDialog();
            }
        }

        void GetReport(int ID)
        {
            try
            {
                frmPrintEntradasGetByEntradaID entradas = new frmPrintEntradasGetByEntradaID();
                entradas.Owner = this;
                entradas.OnInit(ID);
                entradas.ShowDialog();
            }
            catch { }
        }

        public void OnInit(clsEntradasBE _be, int vista)
        {
           
            if (vista == 2)
            {
                VISTA = vista;
                LoadCombox();
                EntradaBE = _be;
                DataContext = EntradaBE;
                detalleEntradasBE = _be.DetalleEntradas.ToList();
                dataGrid1.ItemsSource = _be.DetalleEntradas;
                cmbTipoMovimientos.SelectedValue = _be.TipoMovimientoID;
                CalcularTotales();
            }
        }

        private void LoadCombox()
        {
            try
            {
                if (VISTA == 1)
                {
                    DataContext = new clsEntradasBE { Fecha = DateTime.Today, TipoMovimientoID = -1 };
                }

                //Tipo de Movimientos
                List<clsTipoMovimientosBE> Movimientos = new List<clsTipoMovimientosBE>();
                Movimientos = db.TipoMovimientosGet(null).ToList();
                Movimientos.Add(new clsTipoMovimientosBE { TipoMovimientoID = -1, Movimiento = clsLenguajeBO.Find("itemSelect") });
                cmbTipoMovimientos.ItemsSource = Movimientos;
                cmbTipoMovimientos.SelectedValuePath = "TipoMovimientoID";
                cmbTipoMovimientos.DisplayMemberPath = "Movimiento";

            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
