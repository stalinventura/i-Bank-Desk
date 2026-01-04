
using Payment.Bank.Core;
using Payment.Bank.Core.Model;

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
    /// Interaction logic for frmPrintRenovacionesGetByCuota.xaml
    /// </summary>
    public partial class frmPrintRenovacionesGetByCuota : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        int Cantidad;
        int OficialID =-1;
        int RutaID = -1;

        public frmPrintRenovacionesGetByCuota()
        {
            InitializeComponent();
            Loaded += frmPrintRelacionContratosGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintRelacionContratosGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintRenovacionGetByCuota(Cantidad, SucursalID, OficialID, RutaID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    rptRenovacionesGetByCuota info = new rptRenovacionesGetByCuota();
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


        public void OnInit(int _cantidad, int _SucursalID, int _OficialID, int _RutaID)
        {
            SucursalID = _SucursalID;
            Cantidad = _cantidad;
            OficialID = _OficialID;
            RutaID = _RutaID;
        }

     
    }
}
