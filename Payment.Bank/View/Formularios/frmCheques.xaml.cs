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

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmCheques.xaml
    /// </summary>
    public partial class frmCheques : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        clsSolicitudChequesBE BE = new clsSolicitudChequesBE();
        clsChequesBE chequesBE = new clsChequesBE();
        int _SolicitudChequeID = 0;

        List<clsDetalleSolicitudChequesBE> detalleBE = new List<clsDetalleSolicitudChequesBE>();
        int VISTA = 1;

        public frmCheques()
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridInformacion);
            clsLenguajeBO.Load(gridEncabezado);
            clsLenguajeBO.Load(gridDetalleEntradas);
            clsLenguajeBO.Load(gridProducto);
            Title = clsLenguajeBO.Find(Title.ToString());

            LoadCombox();

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
            txtMonto.LostFocus += txtMonto_LostFocus;
            txtNumero.KeyDown += txtNumero_KeyDown;

            txtFecha.SelectedDate = DateTime.Now;

            if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                txtFecha.IsEnabled = true;
            }
            else
            {
                txtFecha.IsEnabled = false;
            }

            DataContext = new clsSolicitudChequesBE { BancoID = -1 };
            txtNumero.Focus();
        }

        private void txtNumero_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    e.Handled = false;
                }
                else
                {
                    if (e.Key == Key.Enter && !String.IsNullOrEmpty(txtNumero.Text))
                    {
                        BE = db.SolicitudChequesGetBySolicitudChequeID(int.Parse(txtNumero.Text));
                        if (BE != null && BE.EstadoID == 1)
                        {
                            _SolicitudChequeID = BE.SolicitudChequeID;
                            DataContext = BE;
                            detalleBE = BE.DetalleSolicitudCheques.ToList();
                            dataGrid1.ItemsSource = BE.DetalleSolicitudCheques;
                        }
                        else
                        {
                            _SolicitudChequeID = 0;
                            BE = new clsSolicitudChequesBE();
                            DataContext = BE;
                            dataGrid1.ItemsSource = BE.DetalleSolicitudCheques;
                        }
                    }
                    else
                    {
                        if (e.Key == Key.Enter && string.IsNullOrEmpty(txtNumero.Text))
                        {
                            frmConsultasSolicitudCheques cheques = new frmConsultasSolicitudCheques();
                            cheques.Owner = this;
                            cheques.IsQuery = true;
                            cheques.Closed += (arg, obj) =>
                            {
                                if (cheques._SolicitudID > 0)
                                {
                                    txtNumero.Text = cheques._SolicitudID.ToString();
                                    BE = db.SolicitudChequesGetBySolicitudChequeID(cheques._SolicitudID);
                                    if (BE != null && BE.EstadoID == 1)
                                    {
                                        _SolicitudChequeID = BE.SolicitudChequeID;

                                        DataContext = BE;
                                        dataGrid1.ItemsSource = BE.DetalleSolicitudCheques;
                                    }
                                    else
                                    {
                                        _SolicitudChequeID = 0;
                                        BE = new clsSolicitudChequesBE();
                                        DataContext = BE;
                                    }
                                }
                            };
                            cheques.ShowDialog();
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
            catch { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // BE = (dataGrid1.SelectedItem as clsDetalleSolicitudChequesBE).Auxiliares;
            }
            catch { }
        }

        private void txtMonto_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var m = float.Parse(txtMonto.Text);
                txtMonto.Text = string.Format("{0:N2}", m);
            }
            catch { }
        }

        private void LoadCombox()
        {
            try
            {
                //Bancos
                List<clsBancosBE> Bancos = new List<clsBancosBE>();
                Bancos = db.BancosGet(null, clsVariablesBO.UsuariosBE.Documento).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID).ToList();

                Bancos.Add(new clsBancosBE { BancoID = -1, Banco = clsLenguajeBO.Find("itemSelect") });

                cmbBancos.ItemsSource = Bancos;
                cmbBancos.SelectedValuePath = "BancoID";
                cmbBancos.DisplayMemberPath = "Banco";
                cmbBancos.SelectedValue = -1;
            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEncabezado) && float.Parse(lblTotalDebito.Text) == float.Parse(lblTotalCredito.Text))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.ChequesCreate(_SolicitudChequeID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            db.SolicitudChequesDelete(_SolicitudChequeID, 2, clsVariablesBO.UsuariosBE.Documento);

                            if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsAutomaticAccountting == true)
                            {
                                db.Contabilizar(3, _SolicitudChequeID, clsVariablesBO.UsuariosBE.Documento);
                            }
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
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
                            OperationResult result = db.ChequesUpdate(_SolicitudChequeID, (DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                db.SolicitudChequesDelete(_SolicitudChequeID, 2, clsVariablesBO.UsuariosBE.Documento);
                                db.Contabilizar(3, _SolicitudChequeID, clsVariablesBO.UsuariosBE.Documento);
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
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
                clsValidacionesBO.Limpiar(gridEncabezado);
                clsValidacionesBO.Limpiar(gridDetalleEntradas);
                clsValidacionesBO.Limpiar(gridInformacion);
                clsValidacionesBO.Limpiar(gridOpciones);
                clsValidacionesBO.Limpiar(gridProducto);

                BE = new clsSolicitudChequesBE();
                detalleBE = new List<clsDetalleSolicitudChequesBE>();
                dataGrid1.ItemsSource = detalleBE;
                CalcularTotales();
                VISTA = 1;
            }
            catch { }
        }

        public void OnInit(clsSolicitudChequesBE view, int _Vista)
        {
            try
            {
                VISTA = _Vista;

                _SolicitudChequeID = view.SolicitudChequeID;
                gridEncabezado.DataContext = view;
                txtFecha.SelectedDate = view.Cheques.Fecha;
                txtNumero.Text = view.SolicitudChequeID.ToString();
                detalleBE = view.DetalleSolicitudCheques.ToList();
                dataGrid1.ItemsSource = detalleBE;
                CalcularTotales();
            }
            catch { }
        }

        void CalcularTotales()
        {
            try
            {
                float debitos = 0;
                float creditos = 0;

                lblTotalDebito.Text = string.Format("{0:N2}", debitos);
                lblTotalCredito.Text = string.Format("{0:N2}", creditos);

                foreach (var row in BE.DetalleSolicitudCheques)
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



        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
