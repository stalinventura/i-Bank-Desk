
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
    /// Interaction logic for frmPrintRelacionContratosGetByFecha.xaml
    /// </summary>
    public partial class frmPrintRelacionContratosGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Hasta;
        DateTime Desde;
        int OficialID =-1;
        int RutaID = -1;
        int ObjetivoID = -1;

        public frmPrintRelacionContratosGetByFecha()
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
                var bs = db.PrintRelacionContratosGetByFecha(Desde, Hasta, SucursalID, OficialID, RutaID, ObjetivoID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    rptListadoContratosGetByFecha info = new rptListadoContratosGetByFecha();
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


        public void OnInit(DateTime _Desde, DateTime _Hasta, int _SucursalID, int _OficialID, int _RutaID, int _ObjetivoID)
        {
            SucursalID = _SucursalID;
            Desde = _Desde;
            Hasta = _Hasta;
            OficialID = _OficialID;
            RutaID = _RutaID;
            ObjetivoID = _ObjetivoID;
        }

     
    }
}
