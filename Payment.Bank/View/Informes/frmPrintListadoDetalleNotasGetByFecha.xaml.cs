
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
    /// Interaction logic for frmPrintListadoDetalleNotasGetByFecha.xaml
    /// </summary>
    public partial class frmPrintListadoDetalleNotasGetByFecha : Window
    {
        Manager db = new Manager();
        DateTime Desde;
        DateTime Hasta;
        String Documento = "-1";
        bool IsDebit = true;
        public DataSet bs;
        public frmPrintListadoDetalleNotasGetByFecha()
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
                bs = db.PrintDetalleNotasGetByFecha(Desde, Hasta, Documento, IsDebit);
                if (bs.Tables["DetalleNotas"].Rows.Count > 0)
                {
                    ReportDocument info;
                    info = new rptDetalleNotasGetByFecha();
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


        public void OnInit(DateTime _Desde, DateTime _Hasta, string _Documento, bool _isDebit)
        {
            Hasta = _Hasta;
            Desde = _Desde;
            Documento = _Documento;
            IsDebit = _isDebit;
        }

     
    }
}
