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
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for frmConsultasHistorialCartasGetByContratoID.xaml
    /// </summary>
    public partial class frmConsultasHistorialCartasGetByContratoID : MetroWindow
    {
        Bank.Core.Manager db = new Core.Manager();
        int VISTA = 1;
        clsHistorialCartasBE BE = new clsHistorialCartasBE();
        public  int ContratoID = 0;
        public frmConsultasHistorialCartasGetByContratoID()
        {
            InitializeComponent();
         
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            Loaded += frmConsultasHistorialCartasGetByContratoID_Loaded;
            dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                frmPrintCartasGetByHistorialCartaID informe = new frmPrintCartasGetByHistorialCartaID();
                informe.Owner = this;
                informe.OnInit(BE.HistorialCartaID);
                informe.ShowDialog();
            }
            catch { }
        }

        public void OnInit(clsContratosBE BE)
        {
            try
            {
                foreach (var x in db.TareasGet(null).OrderBy(a => a.TipoTareaID))
                {
                    switch (x.TipoTareas.Codigo)
                    {
                        case "CALCULATEINTERESTS":
                            {
                               _= db.CalcularInteres(DateTime.Today, BE.ContratoID);
                            }
                            break;
                        case "CALCULATELATEFEES":
                            {
                               _= db.CalcularMoras(DateTime.Today, BE.ContratoID);
                            }
                            break;
                        case "CALCULATELEGAL":
                            {
                               _= db.CalcularLegal(DateTime.Today, BE.ContratoID);
                            }
                            break;
                    }
                }

                Find();

            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void frmConsultasHistorialCartasGetByContratoID_Loaded(object sender, RoutedEventArgs e)
        {
            var result = db.ContratosGetByContratoID(ContratoID);
            Title = "[" + string.Format("{0:000000}", ContratoID) + "] " + result.Solicitudes.Clientes.Personas.Nombres + " " + result.Solicitudes.Clientes.Personas.Apellidos;
            OnInit(result);
        }
          
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsHistorialCartasBE)dataGrid1.SelectedItem;
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
                var Result = db.HistorialCartasGetByContratoID(ContratoID);
                dataGrid1.ItemsSource = Result.ToList();

            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
