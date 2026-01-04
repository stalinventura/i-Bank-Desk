
using Payment.Bank.Core;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Windows;

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintListadoAnalisisMorosidad.xaml
    /// </summary>
    public partial class frmPrintListadoAnalisisMorosidad : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Hasta;
        int RutaID =-1;
        int OficialID = -1;

        public frmPrintListadoAnalisisMorosidad()
        {
            InitializeComponent();
            Loaded += frmPrintListadoAnalisisMorocidad_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintListadoAnalisisMorocidad_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintListadoAnalisisMorosidad(Hasta, SucursalID, RutaID, OficialID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    rptAnalisisMorosidad info = new rptAnalisisMorosidad();
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


        public void OnInit( DateTime _Hasta, int _SucursalID, int _RutaID, int _OficialD)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            RutaID = _RutaID;
            OficialID = _OficialD;
        }

     
    }
}
