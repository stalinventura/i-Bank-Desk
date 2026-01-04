
using CrystalDecisions.CrystalReports.Engine;
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
    /// Interaction logic for frmSterlizerForm.xaml
    /// </summary>
    public partial class frmPrintResumenIngresosGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Desde;
        DateTime Hasta;
        bool Eliminados = false;
        bool Saldos = false;
        String Documento = "-1";
        int RutaID = -1;
        int FormaPagoID = -1;

        public frmPrintResumenIngresosGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintRelacionIngresosGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintRelacionIngresosGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                        //b.PrintRelacionIngresosGetByFecha(Desde, Hasta, SucursalID, Documento, RutaID, Eliminados, Saldos);
                var bs = db.PrintRelacionIngresosGetByFecha(Desde, Hasta, SucursalID, Documento, RutaID, FormaPagoID, Eliminados, Saldos);
                if (bs.Tables["Recibos"].Rows.Count > 0)
                {
                    ReportDocument info;
                    info = new rptResumenIngresos();                                   

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


        public void OnInit(DateTime _Desde, DateTime _Hasta, int _SucursalID, string _Documento, int _RutaID, int _FormaPagoID, bool _eliminados,  bool _saldos)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            Desde = _Desde;
            Documento = _Documento;
            Eliminados = _eliminados;
            Saldos = _saldos;
            RutaID = _RutaID;
            FormaPagoID = _FormaPagoID;
        }

     
    }
}
