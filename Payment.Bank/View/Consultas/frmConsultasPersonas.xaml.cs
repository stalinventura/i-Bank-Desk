using MahApps.Metro.Controls;
using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmConsultasPersonas.xaml
    /// </summary>
    public partial class frmConsultasPersonas : MetroWindow
    {
        Bank.Core.Manager db = new Core.Manager();
       public clsPersonasBE BE = new clsPersonasBE();
        public bool IsQuery = false; 

        public frmConsultasPersonas()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            Loaded += frmConsultasPersonas_Loaded;
            txtBuscar.KeyDown += txtBuscar_KeyUp;
            txtBuscar.Focus();
        }

        private void frmConsultasPersonas_Loaded(object sender, RoutedEventArgs e)
        {
            Find();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsPersonasBE)dataGrid1.SelectedItem;
                if(IsQuery == true)
                {
                    Close();
                }
            }
            catch { }
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Find();
                }
            }
            catch { }
        }

        public void Find()
        {
            try
            {
                var Result = db.PersonasGet(txtBuscar.Text);
                dataGrid1.ItemsSource = Result;
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
