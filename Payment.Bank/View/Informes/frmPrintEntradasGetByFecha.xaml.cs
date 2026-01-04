
using CrystalDecisions.CrystalReports.Engine;
using Payment.Bank.Core;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Windows;

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintEntradasGetByFecha.xaml
    /// </summary>
    public partial class frmPrintEntradasGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        Boolean IsResumen = true;
        DateTime _Desde = DateTime.Now;
        DateTime _Hasta = DateTime.Now;

        public frmPrintEntradasGetByFecha()
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
                var bs = db.PrintEntradasGetByFecha(_Desde, _Hasta, SucursalID);
                if (bs != null)
                {
                    ReportDocument info;
                    if(!IsResumen)
                    {
                        info = new rptEntradasGetByFecha();
                    }
                    else
                    {
                        bs = db.PrintDetalleEntradasGetByFecha(_Desde, _Hasta, SucursalID);
                        info = new rptDetalleEntradasGetByFecha();
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

        public void OnInit(DateTime desde, DateTime hasta, int sucursalID, bool _IsResumen = false)
        {
            _Desde = desde;
            _Hasta = hasta;
            SucursalID = sucursalID;
            IsResumen = _IsResumen;
        }

     
    }
}
