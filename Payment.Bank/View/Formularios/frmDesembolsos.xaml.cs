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
using System.Xml.Linq;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmDesembolsos.xaml
    /// </summary>
    public partial class frmDesembolsos : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        clsAuxiliaresBE BE = new clsAuxiliaresBE();
        clsDesembolsosBE _BE = new clsDesembolsosBE();
        List<clsDetalleDesembolsosBE> detalleBE = new List<clsDetalleDesembolsosBE>();
        clsAuxiliaresBE cajaBE = new clsAuxiliaresBE();
        int VISTA = 1;
        string _Documento;


        public frmDesembolsos()
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridInformacion);
            clsLenguajeBO.Load(gridEncabezado);
            clsLenguajeBO.Load(gridDetalleEntradas);
            clsLenguajeBO.Load(gridProducto);
            Title = clsLenguajeBO.Find(Title.ToString());
            Loaded += frmDesembolsos_Loaded;
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

            if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                txtFecha.IsEnabled = true;
            }
            else
            {
                txtFecha.IsEnabled = false;
            }

            DataContext = new clsDesembolsosBE();
        }

        private void frmDesembolsos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var xml = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Contabilizaciones.Where(x => x.ContabilizacionID == 100004).FirstOrDefault().Xml;
                XDocument doc = XDocument.Parse(xml);
                XElement Nodo = doc.Root.Element("settings");
                cajaBE = db.AuxiliaresGetByAuxilarID(int.Parse(Nodo.Element("Credito").Value));
            }
            catch { }
        }



        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (dataGrid1.SelectedItem as clsDetalleDesembolsosBE).Auxiliares;
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
                            dataGrid1.ItemsSource = new List<clsDetalleDesembolsosBE>();
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
                //List<clsBancosBE> Bancos = new List<clsBancosBE>();
                //Bancos = db.BancosGet(null, clsVariablesBO.UsuariosBE.Documento).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID).ToList();

                //Bancos.Add(new clsBancosBE { BancoID = -1, Banco = clsLenguajeBO.Find("itemSelect") });

                //cmbBancos.ItemsSource = Bancos;
                //cmbBancos.SelectedValuePath = "BancoID";
                //cmbBancos.DisplayMemberPath = "Banco";
                //cmbBancos.SelectedValue = -1;
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
                        OperationResult result = db.DesembolsosCreate((DateTime)txtFecha.SelectedDate, clsVariablesBO.UsuariosBE.SucursalID, txtBeneficiario.Text.ToUpper(), txtConcepto.Text.ToUpper(), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            if (int.Parse(result.ResponseMessage) > 0)
                            {
                                foreach (var row in detalleBE)
                                {
                                    db.DetalleDesembolsosCreate(int.Parse(result.ResponseMessage), row.AuxiliarID, row.Debito, row.Credito, clsVariablesBO.UsuariosBE.Documento);
                                }
                                GetReport(int.Parse(result.ResponseMessage));
                                db.Contabilizar(4, int.Parse(result.ResponseMessage), clsVariablesBO.UsuariosBE.Documento);
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
                            OperationResult result = db.DesembolsosUpdate(_BE.DesembolsoID, (DateTime)txtFecha.SelectedDate, _BE.SucursalID, txtBeneficiario.Text.ToUpper(), txtConcepto.Text.ToUpper(), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                foreach (var row in detalleBE)
                                {
                                    db.DetalleDesembolsosDelete(_BE.DesembolsoID, clsVariablesBO.UsuariosBE.Documento);
                                }

                                foreach (var row in detalleBE)
                                {
                                    db.DetalleDesembolsosCreate(_BE.DesembolsoID, row.AuxiliarID, row.Debito, row.Credito, clsVariablesBO.UsuariosBE.Documento);
                                }

                                GetReport(_BE.DesembolsoID);
                                db.Contabilizar(4, _BE.DesembolsoID, clsVariablesBO.UsuariosBE.Documento);
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
            frmPrintDesembolsosGetByDesembolsoID Solicitud = new frmPrintDesembolsosGetByDesembolsoID();
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
                _BE = new clsDesembolsosBE();
                BE = new clsAuxiliaresBE();
                detalleBE = new List<clsDetalleDesembolsosBE>();
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
            try
            {
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
            try
            {

                if (float.Parse(txtDebito.Text) > 0)
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

                if (string.IsNullOrEmpty(txtBeneficiario.Text))
                {
                    txtBeneficiario.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtConcepto.Text))
                {
                    txtConcepto.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtMonto.Text) || float.Parse(txtMonto.Text) == 0)
                {
                    txtMonto.Focus();
                    return;
                }

                if (BE != null)
                {
                    if (float.Parse(txtDebito.Text) == 0 && float.Parse(txtCredito.Text) == 0)
                    {
                        return;
                    }

                    foreach (var row in detalleBE)
                    {
                        if (row.AuxiliarID == cajaBE.AuxiliarID)
                        {
                            detalleBE.Remove(row);
                        }
                    }

                    if (VerificarBalance(cajaBE.AuxiliarID) == true)
                    {
                        detalleBE.Add(new clsDetalleDesembolsosBE { AuxiliarID = BE.AuxiliarID, Debito = float.Parse(txtDebito.Text), Credito = float.Parse(txtCredito.Text), DesembolsoID = 0, DetalleDesembolsoID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = BE.Codigo, Auxiliar = BE.Auxiliar, AuxiliarID = BE.AuxiliarID } });
                        detalleBE.Add(new clsDetalleDesembolsosBE { AuxiliarID = cajaBE.AuxiliarID, Debito = 0, Credito = float.Parse(txtMonto.Text), DesembolsoID = 0, DetalleDesembolsoID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = cajaBE.Codigo, Auxiliar = cajaBE.Auxiliar, AuxiliarID = cajaBE.AuxiliarID } });

                        BE = new clsAuxiliaresBE();

                        dataGrid1.ItemsSource = new List<clsDetalleDesembolsosBE>();
                        dataGrid1.ItemsSource = detalleBE;

                        txtCodigo.Text = String.Empty;
                        txtAuxiliar.Text = String.Empty;
                        txtDebito.Text = String.Empty;
                        txtCredito.Text = String.Empty;
                        CalcularTotales();
                        txtCodigo.Focus();
                    }
                }
                else
                {
                    txtCodigo.Focus();
                }
            }
            catch
            {
                try
                {
                    if (VerificarBalance(cajaBE.AuxiliarID) == true)
                    {
                        detalleBE.Add(new clsDetalleDesembolsosBE { AuxiliarID = BE.AuxiliarID, Debito = float.Parse(txtDebito.Text), Credito = float.Parse(txtCredito.Text), DesembolsoID = 0, DetalleDesembolsoID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = BE.Codigo, Auxiliar = BE.Auxiliar, AuxiliarID = BE.AuxiliarID } });
                        detalleBE.Add(new clsDetalleDesembolsosBE { AuxiliarID = cajaBE.AuxiliarID, Debito = 0, Credito = float.Parse(txtMonto.Text), DesembolsoID = 0, DetalleDesembolsoID = detalleBE.Count() + 1, Auxiliares = new clsAuxiliaresBE { Codigo = cajaBE.Codigo, Auxiliar = cajaBE.Auxiliar, AuxiliarID = cajaBE.AuxiliarID } });

                        BE = new clsAuxiliaresBE();

                        dataGrid1.ItemsSource = new List<clsDetalleDesembolsosBE>();
                        dataGrid1.ItemsSource = detalleBE;

                        txtCodigo.Text = String.Empty;
                        txtAuxiliar.Text = String.Empty;
                        txtDebito.Text = String.Empty;
                        txtCredito.Text = String.Empty;
                        CalcularTotales();
                        txtCodigo.Focus();
                    }
                }
                catch { }
            }
        }

        private bool VerificarBalance(int AuxiliarID)
        {
            try
            {
                db = new Manager();
                var rows = db.DetalleEntradasGetByAuxiliarID(AuxiliarID);
                float balance = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.PettyCash + rows.Sum(x => x.Debito) - rows.Sum(x => x.Credito);
                if (balance >= float.Parse(txtMonto.Text))
                {
                    return true;
                }
                else
                {
                    clsMessage.ErrorMessage(string.Format(clsLenguajeBO.Find("msgPettyCash"), string.Format("{0:N2}", balance)), clsLenguajeBO.Find("msgTitle"));
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void OnInit(clsDesembolsosBE view, int _Vista)
        {
            try
            {
                VISTA = _Vista;
                _BE = view;
                DataContext = _BE;
                detalleBE = view.DetalleDesembolsos.ToList();
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
                foreach (var row in detalleBE)
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

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
