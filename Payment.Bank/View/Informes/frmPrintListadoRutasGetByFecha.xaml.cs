
using Payment.Bank.Core;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Windows;

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintListadoRutasGetByFecha.xaml
    /// </summary>
    public partial class frmPrintListadoRutasGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Hasta;
        int RutaID =-1;
        int OficialID = -1;
        int ZonaID = -1;

        public frmPrintListadoRutasGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintListadoRutasGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintListadoRutasGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintListadoRutasGetByFecha(Hasta, SucursalID, RutaID, ZonaID, OficialID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    rptListadoRutasGetByFecha info = new rptListadoRutasGetByFecha();
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


        public void OnInit( DateTime _Hasta, int _SucursalID, int _RutaID, int _OficialD, int _ZonaID)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            RutaID = _RutaID;
            OficialID = _OficialD;
            ZonaID = _ZonaID;
        }

     
    }
}
