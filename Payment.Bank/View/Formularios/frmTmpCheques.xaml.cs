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
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmTmpCheques : MetroWindow
    {
        int VISTA = 1;
        Core.Manager db = new Core.Manager();
        public frmTmpCheques()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridTmpCheques);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            txtFecha.SelectedDate = DateTime.Today;
        
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int banco = (Popular.IsChecked == true? 1: 0);
                frmPrintCheques cheques = new frmPrintCheques();
                cheques.OnInit(banco, (DateTime)txtFecha.SelectedDate, txtNombres.Text, txtConcepto.Text, float.Parse(txtMonto.Text));
                cheques.Owner = this;
                cheques.ShowDialog();
                
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void Limpiar()
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in this.gridTmpCheques.Children)
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

  
    }
}
