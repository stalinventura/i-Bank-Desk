
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
    /// Interaction logic for frmPrintRelacionComisionIngresosGetByFecha.xaml
    /// </summary>
    public partial class frmPrintRelacionComisionIngresosGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Desde;
        DateTime Hasta;
        bool Eliminados = false;
        String Documento = "-1";
        bool Resumen;
        bool Saldos = false;
        int RutaID = -1;
        int FormaPagoID = -1;
        int OficialID = -1;

        public DataSet bs;
        public frmPrintRelacionComisionIngresosGetByFecha()
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
                bs = db.PrintRelacionIngresosGetByFecha(Desde, Hasta, SucursalID, Documento, RutaID, FormaPagoID, Eliminados, Saldos, OficialID);
                if (bs.Tables["Recibos"].Rows.Count > 0)
                {
                    ReportDocument info;
                    info = new rptRelacionComisionIngresosGetByFecha();
                                     

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


        public void OnInit(DateTime _Desde, DateTime _Hasta, int _SucursalID, string _Documento, int _RutaID, int _FormaPagoID, bool _resumen, bool _eliminado = false, bool _saldos = false, int _OficialID = -1)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            Desde = _Desde;
            Documento = _Documento;
            RutaID = _RutaID;
            FormaPagoID = _FormaPagoID;
            Resumen = _resumen;
            Eliminados = _eliminado;
            Saldos = _saldos;
            OficialID = _OficialID;
        }

     
    }
}
