
using CrystalDecisions.CrystalReports.Engine;
using Payment.Bank.Core;
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
    /// Interaction logic for frmPrintBalanzaComprobacionGetByFecha.xaml
    /// </summary>
    public partial class frmPrintBalanzaComprobacionGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        Boolean IsResumen = true;
        DateTime _Desde = DateTime.Now;
        DateTime _Hasta = DateTime.Now;

        public frmPrintBalanzaComprobacionGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintEntradasGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintEntradasGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintBalanzaComprobacionGetByFecha(_Desde, _Hasta, SucursalID);
                if (bs != null)
                {
                    rptBalanzaComprobacionGetByFecha info = new rptBalanzaComprobacionGetByFecha();
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

        public void OnInit(DateTime desde, DateTime hasta, int sucursalID, bool _IsResumen = false)
        {
            _Desde = desde;
            _Hasta = hasta;
            SucursalID = sucursalID;
            IsResumen = _IsResumen;
        }

     
    }
}
