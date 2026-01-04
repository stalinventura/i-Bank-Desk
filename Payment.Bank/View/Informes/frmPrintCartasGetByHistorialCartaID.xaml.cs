
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
    /// Interaction logic for frmPrintCartasGetByHistorialCartaID.xaml
    /// </summary>
    public partial class frmPrintCartasGetByHistorialCartaID : Window
    {
        Manager db = new Manager();
        int HistorialCartaID = 0;

        public frmPrintCartasGetByHistorialCartaID()
        {
            InitializeComponent();
            Loaded += frmPrintCartasGetByHistorialCartaID_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintCartasGetByHistorialCartaID_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintCartasGetByHistorialCartaID(HistorialCartaID);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    var result = db.HistorialCartasGet(null).Where(x => x.HistorialCartaID == HistorialCartaID).FirstOrDefault();
                    switch(result.Cartas.TipoCartaID)
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
                                switch(result.Cuotas.Contratos.Solicitudes.TipoSolicitudID)
                                {
                                    case 1:
                                        {
                                            rptCartaIntimacion info = new rptCartaIntimacion();
                                            info.SetDataSource(bs);
                                            ReportViewer.ViewerCore.ReportSource = info;
                                        } break;
                                    default:
                                        {
                                            rptCartaIntimacionSinGarante info = new rptCartaIntimacionSinGarante();
                                            info.SetDataSource(bs);
                                            ReportViewer.ViewerCore.ReportSource = info;
                                        } break;
                                }

                            }
                            break;
                        case 7:
                            {
                                switch (result.Cuotas.Contratos.Solicitudes.TipoSolicitudID)
                                {
                                    case 1:
                                        {
                                            rptCartaMandamiento info = new rptCartaMandamiento();
                                            info.SetDataSource(bs);
                                            ReportViewer.ViewerCore.ReportSource = info;
                                        }
                                        break;
                                    default:
                                        {
                                            rptCartaMandamientoSinGarante info = new rptCartaMandamientoSinGarante();
                                            info.SetDataSource(bs);
                                            ReportViewer.ViewerCore.ReportSource = info;
                                        }
                                        break;
                                }
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


        public void OnInit(int _HistorialCartaID)
        {
            HistorialCartaID = _HistorialCartaID;
        }

     
    }
}
