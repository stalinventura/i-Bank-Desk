
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
    public partial class frmPrintCuentasPorCobrar : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Hasta;
        bool Summary = false;
        int TipoSolicitudID =-1;
        int RutaID = -1;
        string OrderBy = string.Empty;

        public frmPrintCuentasPorCobrar()
        {
            InitializeComponent();
            Loaded += frmPrintCuentasPorCobrar_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintCuentasPorCobrar_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintCuentasPorCobrar(Hasta, SucursalID, TipoSolicitudID, RutaID, OrderBy);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    if (Summary)
                    {
                        rptResumenCuentasPorCobrar info = new rptResumenCuentasPorCobrar();
                        info.SetDataSource(bs);
                        ReportViewer.ViewerCore.ReportSource = info;
                    }
                    else
                    {
                        rptCuentasPorCobrar info = new rptCuentasPorCobrar();
                        info.SetDataSource(bs);
                        ReportViewer.ViewerCore.ReportSource = info;
                    }
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


        public void OnInit( DateTime _Hasta, int _SucursalID,int _TipoSolicitudID, int _RutaID, bool _summary, string _OrderBy)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            TipoSolicitudID = _TipoSolicitudID;
            RutaID = _RutaID;
            Summary = _summary;
            OrderBy = _OrderBy;
        }

     
    }
}
