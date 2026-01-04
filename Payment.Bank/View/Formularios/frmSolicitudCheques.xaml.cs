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
    /// Interaction logic for frmSolicitudCheques.xaml
    /// </summary>
    public partial class frmSolicitudCheques : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        clsAuxiliaresBE BE = new clsAuxiliaresBE();
        clsSolicitudChequesBE _BE = new clsSolicitudChequesBE();
        List<clsDetalleSolicitudChequesBE> detalleBE = new List<clsDetalleSolicitudChequesBE>();
        int VISTA = 1;

        public frmSolicitudCheques()
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
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;

            txtDebito.TextChanged += txtDebito_TextChanged;
            txtDebito.LostFocus += txtDebito_LostFocus;
            txtMonto.LostFocus += txtMonto_LostFocus;

            txtCredito.TextChanged += txtCredito_TextChanged;
            txtCredito.LostFocus += txtCredito_LostFocus;
         
       
            txtFecha.SelectedDate = DateTime.Today;

            txtCodigo.KeyDown += txtCodigo_KeyDown;  

            if(clsVariablesBO.UsuariosBE.Roles.IsAdmin== true)
            {
                txtFecha.IsEnabled = true;
            }
            else
            {
                txtFecha.IsEnabled = false;
            }

            DataContext = new clsSolicitudChequesBE { BancoID =-1 };
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (dataGrid1.SelectedItem as clsDetalleSolicitudChequesBE).Auxiliares;
            }
            catch { }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE != null)
                {
                    foreach (var row in detalleBE)
                    {
                        if (row.AuxiliarID == BE.AuxiliarID)
                        {
                            detalleBE.Remove(row);
                            dataGrid1.ItemsSource = new List<clsDetalleSolicitudChequesBE>();
                            dataGrid1.ItemsSource = detalleBE;
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
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEncabezado) && float.Parse(lblTotalDebito.Text) == float.Parse(lblTotalCredito.Text) && detalleBE.Count() > 0)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.SolicitudChequesCreate((DateTime)txtFecha.SelectedDate, 0, (int)cmbBancos.SelectedValue, txtBeneficiario.Text.ToUpper(), txtConcepto.Text.ToUpper(), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            if (int.Parse(result.ResponseMessage) > 0)
                            {
                                foreach(var row in detalleBE)
                                {
                                    db.DetalleSolicitudChequesCreate(int.Parse(result.ResponseMessage), row.AuxiliarID, row.Debito, row.Credito, clsVariablesBO.UsuariosBE.Documento);
                                }
                                GetReport(int.Parse(result.ResponseMessage));
                                //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                Limpiar();
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridEncabezado) && float.Parse(lblTotalDebito.Text) == float.Parse(lblTotalCredito.Text) && detalleBE.Count() > 0)
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.SolicitudChequesUpdate(_BE.SolicitudChequeID, (DateTime)txtFecha.SelectedDate, _BE.ContratoID, (int)cmbBancos.SelectedValue, txtBeneficiario.Text.ToUpper(), txtConcepto.Text.ToUpper(), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                foreach (var row in detalleBE)
                                {
                                    db.DetalleSolicitudChequesDelete(_BE.SolicitudChequeID, clsVariablesBO.UsuariosBE.Documento);
                                }
                                foreach (var row in detalleBE)
                                {
                                    db.DetalleSolicitudChequesCreate(_BE.SolicitudChequeID, row.AuxiliarID, row.Debito, row.Credito, clsVariablesBO.UsuariosBE.Documento);
                                }

                                GetReport(_BE.SolicitudChequeID);
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


        void GetReport(int ID)
        {
            frmPrintSolicitudChequesGetBySolicitudChequeID Solicitud = new frmPrintSolicitudChequesGetBySolicitudChequeID();
            Solicitud.OnInit(ID);
            Solicitud.ShowDialog();
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
                _BE = new clsSolicitudChequesBE();
                BE = new clsAuxiliaresBE();
                detalleBE = new List<clsDetalleSolicitudChequesBE>();
                dataGrid1.ItemsSource = detalleBE;
                CalcularTotales();
                VISTA = 1;
            }
            catch { }
        }

        private void txtCredito_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = float.Parse(txtCredito.Text);
                txtCredito.Text = string.Format("{0:N2}", c);
            }
            catch { }
        }

        private void txtDebito_LostFocus(object sender, RoutedEventArgs e)
        {
            try {
                var d = float.Parse(txtDebito.Text);
                txtDebito.Text = string.Format("{0:N2}", d);
            }
            catch { }
        }

        private void txtCredito_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (float.Parse(txtCredito.Text) > 0)
                {
                    txtDebito.Text = string.Format("{0:N2}", 0);
                }
            }
            catch { }
        }

        private void txtDebito_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {

                if(float.Parse(txtDebito.Text) > 0)
                {
                    txtCredito.Text = string.Format("{0:N2}", 0);
                }
            }
            catch { }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                if(string.IsNullOrEmpty(txtBeneficiario.Text))
                {
                    txtBeneficiario.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtConcepto.Text))
                {
                    txtConcepto.Focus();
                    return;
                }

                if (cmbBancos.SelectedItem == null || (int)cmbBancos.SelectedValue ==-1)
                {
                    cmbBancos.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtMonto.Text) || float.Parse(txtMonto.Text) == 0)
                {
                    txtMonto.Focus();
                    return;
                }

                if (BE != null )
                {
                    if(float.Parse(txtDebito.Text) == 0 && float.Parse(txtCredito.Text) == 0)
                    {
                        return;
                    }

                    foreach(var row in detalleBE)
                    {
                        if(row.AuxiliarID == (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID )
                        {
                            detalleBE.Remove(row);                            
                        }
                    }

                    
                    detalleBE.Add(new clsDetalleSolicitudChequesBE { AuxiliarID = BE.AuxiliarID, Debito = float.Parse(txtDebito.Text), Credito = float.Parse(txtCredito.Text), SolicitudChequeID = 0, DetalleSolicitudChequeID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo= BE.Codigo, Auxiliar = BE.Auxiliar, AuxiliarID = BE.AuxiliarID }  });
                    detalleBE.Add(new clsDetalleSolicitudChequesBE { AuxiliarID = (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, Debito = 0, Credito = float.Parse(txtMonto.Text), SolicitudChequeID = 0, DetalleSolicitudChequeID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = (cmbBancos.SelectedItem as clsBancosBE).Auxiliares.Codigo, Auxiliar = (cmbBancos.SelectedItem as clsBancosBE).Auxiliares.Auxiliar, AuxiliarID = (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID } });

                    BE = new clsAuxiliaresBE();

                    dataGrid1.ItemsSource = new List<clsDetalleSolicitudChequesBE>();
                    dataGrid1.ItemsSource = detalleBE;

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
            catch {

                try
                {
                    detalleBE.Add(new clsDetalleSolicitudChequesBE { AuxiliarID = BE.AuxiliarID, Debito = float.Parse(txtDebito.Text), Credito = float.Parse(txtCredito.Text), SolicitudChequeID = 0, DetalleSolicitudChequeID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = BE.Codigo, Auxiliar = BE.Auxiliar, AuxiliarID = BE.AuxiliarID } });
                    detalleBE.Add(new clsDetalleSolicitudChequesBE { AuxiliarID = (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID, Debito = 0, Credito = float.Parse(txtMonto.Text), SolicitudChequeID = 0, DetalleSolicitudChequeID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = (cmbBancos.SelectedItem as clsBancosBE).Auxiliares.Codigo, Auxiliar = (cmbBancos.SelectedItem as clsBancosBE).Auxiliares.Auxiliar, AuxiliarID = (cmbBancos.SelectedItem as clsBancosBE).AuxiliarID } });

                    BE = new clsAuxiliaresBE();

                    dataGrid1.ItemsSource = new List<clsDetalleSolicitudChequesBE>();
                    dataGrid1.ItemsSource = detalleBE;

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

        public void OnInit(clsSolicitudChequesBE view, int _Vista)
        {
            try
            {
                VISTA = _Vista;
                _BE = view;
                DataContext = _BE;
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
                foreach(var row in detalleBE)
                {
                    debitos += row.Debito;
                    creditos += row.Credito;
                }
                lblTotalDebito.Text = string.Format("{0:N2}", debitos);
                lblTotalCredito.Text = string.Format("{0:N2}", creditos);
                if(debitos == creditos)
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
            if(e.Key == Key.Enter)
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
                     
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
