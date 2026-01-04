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
using Payment.Bank.Core.Model;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmDetalleNotaDebitos.xaml
    /// </summary>
    public partial class frmDetalleNotaDebitos : MetroWindow
    {
        int VISTA = 1;
        BoxReportItem BE = new BoxReportItem();
        Core.Manager db = new Core.Manager();
        public frmDetalleNotaDebitos()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridNotas);
            clsLenguajeBO.Load(LayoutRoot);
      

            txtCapital.KeyDown += txtCapital_KeyDown;
            txtCapital.LostFocus += txtCapital_LostFocus;

            txtComision.KeyDown += txtComision_KeyDown;
            txtComision.LostFocus += txtComision_LostFocus;

            txtInteres.KeyDown += txtInteres_KeyDown;
            txtInteres.LostFocus += txtInteres_LostFocus;

            txtMoras.KeyDown += txtMoras_KeyDown;
            txtMoras.LostFocus += txtMoras_LostFocus;

            txtLegal.KeyDown += txtLegal_KeyDown;
            txtLegal.LostFocus += txtLegal_LostFocus;

            txtSeguro.LostFocus += txtSeguro_LostFocus;
            txtSeguro.KeyDown += txtSeguro_KeyDown;

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            DataContext = new clsDetalleNotaDebitosBE();
        }

        private void txtSeguro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Seguro = double.Parse(txtSeguro.Text);
                txtSeguro.Text = String.Format("{0:0.000}", Seguro);
            }
            catch { }
        }

        private void txtSeguro_KeyDown(object sender, KeyEventArgs e)
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


        private void txtLegal_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Legal = double.Parse(txtLegal.Text);
                txtLegal.Text = String.Format("{0:0.000}", Legal);
            }
            catch { }
        }

        private void txtMoras_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Moras = double.Parse(txtMoras.Text);
                txtMoras.Text = String.Format("{0:0.000}", Moras);
            }
            catch { }
        }

        private void txtInteres_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Interes = double.Parse(txtInteres.Text);
                txtInteres.Text = String.Format("{0:0.000}", Interes);
            }
            catch { }
        }

        private void txtComision_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Comision = double.Parse(txtComision.Text);
                txtComision.Text = String.Format("{0:0.000}", Comision);
            }
            catch { }
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

        private void txtMoras_KeyDown(object sender, KeyEventArgs e)
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

        private void txtCapital_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Capital = double.Parse(txtCapital.Text);
                txtCapital.Text = String.Format("{0:0.000}", Capital);
            }
            catch { }
        }

        private void txtCapital_KeyDown(object sender, KeyEventArgs e)
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

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridNotas))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.DetalleNotaDebitosCreate(BE.CuotaID,txtConcepto.Text, float.Parse(txtCapital.Text), float.Parse(txtComision.Text), float.Parse(txtInteres.Text), float.Parse(txtMoras.Text), float.Parse(txtLegal.Text), float.Parse(txtSeguro.Text), false, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                            Limpiar();
                            Close();
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridNotas))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.DetalleNotaDebitosCreate(BE.CuotaID, txtConcepto.Text, float.Parse(txtCapital.Text), float.Parse(txtComision.Text), float.Parse(txtInteres.Text), float.Parse(txtMoras.Text), float.Parse(txtLegal.Text), float.Parse(txtSeguro.Text), false, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                Limpiar();
                                Close();
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
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void Limpiar()
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in this.gridNotas.Children)
                {
                    if (Obj is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        ctl.Text = String.Empty;
                    }
                }
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(BoxReportItem be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                //if (VISTA == 2)
                //{
                BE = be;
                Title = clsLenguajeBO.Find(Title.ToString()) + string.Format(" - {0:000}", be.Numero);
                //lblNumero.Text = string.Format("{0:000}", be.Numero);
                //DataContext = be;
                //}

                if (clsVariablesBO.UsuariosBE.RolID == 0)
                {
                    txtCapital.IsReadOnly = false;
                }

            }
            catch { }
        }     
    }
}
