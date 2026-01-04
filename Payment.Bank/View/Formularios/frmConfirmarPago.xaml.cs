using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmConfirmarPago.xaml
    /// </summary>
    public partial class frmConfirmarPagos : Window
    {

        public bool Confirmado = false;
        float Monto = 0;
        public float MontoPagado = 0;
        public float Cambio = 0;

        public frmConfirmarPagos()
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridLogin);
            clsLenguajeBO.Load(gridPagos);

            Loaded += frmConfirmarFacturas_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            txtPagoInicial.KeyDown += txtPagoInicial_KeyDown;
            txtPagoInicial.LostFocus += txtPagoInicial_LostFocus;
            txtPagoInicial.TextChanged += txtPagoInicial_TextChanged;
            txtMontoPagado.KeyDown += txtMontoPagado_KeyDown;
            txtMontoPagado.LostFocus += txtMontoPagado_LostFocus;
            txtMontoPagado.TextChanged += txtMontoPagado_TextChanged;


        }

        private void txtPagoInicial_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (float.Parse(txtPagoInicial.Text) <= Monto)
                {
                    CalcularPago();
                }
                else
                {
                    txtPagoInicial.Text = string.Format("{0:N2}", Monto);
                }
            }
            catch { }
        }

        void CalcularPago()
        {
            try
            {

             
                MontoPagado = float.Parse(txtMontoPagado.Text);


                Cambio = MontoPagado  - Monto;
                txtCambio.Text = string.Format("{0:N2}", Cambio < 0 ? 0 : Cambio);
                
               
            }
            catch { }
        }

        private void txtMontoPagado_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                    if (float.Parse(txtMontoPagado.Text) >= Monto)
                    {
                        CalcularPago();
                    }

            }
            catch { }
        }

        private void txtMontoPagado_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Monto = double.Parse(txtMontoPagado.Text);
                txtMontoPagado.Text = String.Format("{0:N2}", Monto);
            }
            catch { }
        }

        private void txtPagoInicial_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double Monto = double.Parse(txtPagoInicial.Text);
                txtPagoInicial.Text = String.Format("{0:N2}", Monto);
            }
            catch { }
        }

        private void txtMontoPagado_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtPagoInicial_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        public void OnInit(float _Monto, clsRecibosBE BE)
        {
            try
            {
                Monto = _Monto;

                txtPagoInicial.Text = string.Format("{0:N2}", Monto);
                txtMontoPagado.Text = string.Format("{0:N2}", 0);
                txtCambio.Text = string.Format("{0:N2}", 0);


                txtPagoInicial.IsReadOnly = true;
                txtMontoPagado.IsReadOnly = false;
                txtMontoPagado.Focus();
                txtMontoPagado.Select(0, txtMontoPagado.Text.Length);
                if (BE.ReciboID > 0)
                {
                    txtMontoPagado.Text = string.Format("{0:N2}", BE.Total);
                }

                CalcularPago();

            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Confirmado = false;
                if (clsValidacionesBO.Validar(gridPagos) == true)
                {
                    if (Monto > float.Parse(txtMontoPagado.Text))
                    {
                        txtMontoPagado.Focus();
                        Confirmado = false;
                    }
                    else
                    {
                        Confirmado = true;
                    }

                    if (Confirmado == true)
                    {
                        this.Close();
                    }
                }
            }
            catch { }
        }

        private void frmConfirmarFacturas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Confirmado = false;
            Close();
        }


    }
}
