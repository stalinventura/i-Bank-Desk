
using CrystalDecisions.CrystalReports.Engine;
using Payment.Bank.Core;
using Payment.Bank.Core.Model;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintAlquileresGetByAlquilerID.xaml
    /// </summary>
    public partial class frmPrintAlquileresGetByAlquilerID : Window
    {
        Manager db = new Manager();
        clsAlquileresBE BE;

        public frmPrintAlquileresGetByAlquilerID()
        {
            InitializeComponent();
            Loaded += frmPrintCuentasPorCobrar_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintCuentasPorCobrar_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintAlquileresGetByAlquilerID(BE.AlquilerID);      
                if (bs.Tables[0].Rows.Count > 0)
                {
                    ReportDocument info;
                    switch (BE.TipoAlquilerID)
                    {
                        case 1:
                            {
                                info = new rptContratoAlquilerGarante();
                            }
                            break;
                        default:
                            {
                                info = new rptContratoAlquiler();
                            }
                            break;
                    }
                    info.SetDataSource(bs);
                    ReportViewer.ViewerCore.ReportSource = info;
                }
                else
                {
                    rptEmpty Empty = new rptEmpty();
                    ReportViewer.ViewerCore.ReportSource = Empty;
                }
            }
            catch
            {
                rptErrorData error = new rptErrorData();
                ReportViewer.ViewerCore.ReportSource = error;
            }
        }


        public void OnInit(int ID)
        {           
            BE = db.AlquileresGetByAlquilerID(ID);
        }

     
    }
}
