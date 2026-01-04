
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
    /// Interaction logic for frmPrintCartasGetByRutaID.xaml
    /// </summary>
    public partial class frmPrintCartasGetByRutaID : Window
    {
        Manager db = new Manager();

        int TipoCartaID = 0;
        int SucursalID = 0;
        DateTime Desde;
        DateTime Hasta;
        int RutaID;

        public frmPrintCartasGetByRutaID()
        {
            InitializeComponent();
            Loaded += frmPrintCartasGetByRutaID_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintCartasGetByRutaID_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintCartasGetByRutaID(Desde, Hasta, SucursalID, TipoCartaID, RutaID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    switch(TipoCartaID)
                    {
                        case 1:
                            {
                                rptCartaPrimera info = new rptCartaPrimera();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info; 
                            }
                            break;
                        case 2:
                            {
                                rptCartaSegunda info = new rptCartaSegunda();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            break;
                        case 3:
                            {
                                rptCartaGerente info = new rptCartaGerente();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            break;
                        case 4:
                            {
                                rptCartaCuarta info = new rptCartaCuarta();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            break;
                        case 5:
                            {
                                rptCartaAbogado info = new rptCartaAbogado();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            break;
                        case 6:
                            {
                                rptCartaIntimacionSinGarante info = new rptCartaIntimacionSinGarante();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            break;
                        case 7:
                            {
                                rptCartaMandamientoSinGarante info = new rptCartaMandamientoSinGarante();
                                info.SetDataSource(bs);
                                ReportViewer.ViewerCore.ReportSource = info;                            
                            }
                            break;
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


        public void OnInit(DateTime _Desde, DateTime _Hasta,int _SucursalID, int _TipoCartaID, int _RutaID)
        {
            Desde = _Desde;
            Hasta = _Hasta;
            SucursalID = _SucursalID;
            TipoCartaID = _TipoCartaID;
            RutaID = _RutaID;
        }

     
    }
}
