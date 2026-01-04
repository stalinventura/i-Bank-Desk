using Payment.Bank.Modulos;
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

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmPin.xaml
    /// </summary>
    public partial class frmPin : Window
    {
        string _Code = string.Empty;
        public bool Confirmado = false;
        public frmPin()
        {
            InitializeComponent();
            Loaded += frmPin_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            txtPin.KeyDown += txtPin_KeyDown;
        }

        private void txtPin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Key == Key.Enter && !string.IsNullOrEmpty(txtPin.Password))
                //{
                //    if (_Code.ToString() == txtPin.Password)
                //    {
                //        Confirmado = true;
                //        Close();
                //    }
                //    else
                //    {
                //        clsMessage.ErrorMessage("Codigo o Pin incorrecto.", "Mensaje");
                //        txtPin.Focus();
                //        txtPin.SelectAll();
                //    }
                //}
            }
            catch { }
        }
        public void OnInit(string Code)
        {
            _Code = Code;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_Code.ToString() == txtPin.Password)
                {
                    Confirmado = true;
                    Close();
                }
                else
                {
                    clsMessage.ErrorMessage("Codigo o Pin incorrecto.", "Mensaje");
                    txtPin.Focus();
                    txtPin.SelectAll();
                }
            }
            catch { }
        }

        private void frmPin_Loaded(object sender, RoutedEventArgs e)
        {
            txtPin.Focus();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
