using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes;
using System.Diagnostics;
using System.Text;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasHistorial : MetroWindow
    {
       public DateTime Fecha = DateTime.Now;
        Core.Manager db = new Core.Manager();
        public float Monto = 0;
        float PagoMinimo = 0;
        public List<BoxReportItem> cuotasBE = new List<BoxReportItem>();
        public BoxReportItem BE = new BoxReportItem();
        List<BoxReportItem> result = new List<BoxReportItem>();
        clsContratosBE row = new clsContratosBE();
        clsCuotasView Cuotas = new clsCuotasView();
        bool IsLate = false;
        public frmConsultasHistorial()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                btnSalir.Click += btnSalir_Click;
            }
            catch{}
        }


        public void OnInit(clsPersonasBE BE, List<clsDataCreditosBE> data)
        {
            try
            {
                clsLenguajeBO.Load(gridMenu);
                clsLenguajeBO.Load(LayoutRoot);
                gridDatosPersonales.DataContext = BE;
                dataGrid1.ItemsSource = data;
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }


}

