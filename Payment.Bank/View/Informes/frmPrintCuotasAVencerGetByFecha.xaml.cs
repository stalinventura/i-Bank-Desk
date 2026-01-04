
using Payment.Bank.Core;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Windows;

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintCuotasAVencerGetByFecha.xaml
    /// </summary>
    public partial class frmPrintCuotasAVencerGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        int RutaID = -1;
        DateTime Desde;
        DateTime Hasta;

        public frmPrintCuotasAVencerGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintCuotasVencidasGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintCuotasVencidasGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintCuotasAVencerGetByFecha(Desde, Hasta, SucursalID, RutaID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    rptCuotasAVencerGetByFecha info = new rptCuotasAVencerGetByFecha();
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


        public void OnInit(DateTime _Desde, DateTime _Hasta, int _SucursalID, int _RutaID)
        {
            SucursalID = _SucursalID;
            RutaID = _RutaID;
            Desde = _Desde;
            Hasta = _Hasta;
        }

     
    }
}
