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

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasRentas : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        public float Monto = 0;
        float PagoMinimo = 0;
        public List<RentasBoxReportItem> rentasBE = new List<RentasBoxReportItem>();
        public RentasBoxReportItem BE = new RentasBoxReportItem();
        clsRentasBE row = new clsRentasBE();
        public frmConsultasRentas()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                btnAceptar.Click += btnAceptar_Click;
                btnSalir.Click += btnSalir_Click;
                txtMonto.KeyDown += txtMonto_KeyDown;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnPrint.Click += btnPrint_Click;
         
            }
            catch{}
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //frmPrintCuotasGetByContratoID informe = new frmPrintCuotasGetByContratoID();
                //informe.Owner = this;
                //informe.OnInit(int.Parse(lblContrato.Text));
                //informe.ShowDialog();
            }
            catch { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           try
            {
                BE = (RentasBoxReportItem)dataGrid1.SelectedItem;
                if (BE.Monto >= 0 && clsVariablesBO.UsuariosBE.Roles.IsAdmin == true )//&& BE.Balance >0)
                {
                    Habilitar();
                }
                else
                { DesHabilitar(); }
                txtMonto.Focus();
               
            }
            catch { }
        }

        private void Habilitar()
        {
            try
            {
                //btnNotaDebito.Visibility = Visibility.Visible;
                //btnNotaCredito.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void DesHabilitar()
        {
            try
            {
                //btnNotaDebito.Visibility = Visibility.Collapsed;
                //btnNotaCredito.Visibility = Visibility.Collapsed;
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


        public void OnInit(clsAlquileresBE BE)
        {
            try
            {
                clsLenguajeBO.Load(gridMenu);
                clsLenguajeBO.Load(LayoutRoot);
                gridDatosPersonales.DataContext = BE;

                LoadData(BE.AlquilerID);
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void LoadData(int AlquilerID)
        {
            try
            {
                clsRentasView Cuotas = new clsRentasView();
                Cuotas.SetDataSource(AlquilerID);
                var result = Cuotas.GetGroup().Where(x=>x.Monto >0);

                dataGrid1.ItemsSource = result.ToList();
                rentasBE = result.ToList();
                lblAtraso.Text = clsLenguajeBO.Find("lblAtraso") + " " + string.Format("{0:N2}", Cuotas.AtrasoGet());
            }
            catch { }
        }

        bool ValidarPago()
        {
            try
            {
                bool Continuar = false;
                double Valor = double.Parse(txtMonto.Text);
                if (Valor >= PagoMinimo)
                {
                    Continuar = true;
                }
                return Continuar;
            }
            catch { return false; }
        }

        void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float _Monto = float.Parse(txtMonto.Text);
                if (_Monto > 0)
                {
                    if (ValidarPago() == true)
                    {
                        Monto = _Monto;
                        Close();
                    }
                    else
                    {
                        if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                        {
                            Monto = _Monto;
                            Close();
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgMinimumPayment") + string.Format("{0:C3}", PagoMinimo), clsLenguajeBO.Find("msgTitle"));
                        }
                    }                    
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

