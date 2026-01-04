
using Payment.Bank.Core;
using Payment.Bank.Core.Model;

using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for frmPrintRecibosRutasGetByFecha.xaml
    /// </summary>
    public partial class frmPrintRecibosRutasGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Hasta;
        int RutaID =-1;
        int OficialID = -1;
        int ZonaID = -1;

        public frmPrintRecibosRutasGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintRecibosRutasGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintRecibosRutasGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintRecibosRutasGetByFecha(Hasta, SucursalID, RutaID, ZonaID, OficialID);
                
                if (bs.Tables[0].Rows.Count > 0)
                {
                    rptRecibosRuta info = new rptRecibosRuta();
                    info.SetDataSource(bs);
                    var setting = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Impresiones.Where(x => x.TipoImpresionID == 100002);
                    if (setting.Count() == 0)
                    {
                        ReportViewer.ViewerCore.ReportSource = info;
                    }
                    else
                    {
                        bool _Preview = false;
                        int _From = 1;
                        int _To = bs.Tables[0].Rows.Count;

                        frmPreview Preview = new frmPreview();
                        Preview.To = _To;
                        Preview.Owner = this;
                        Preview.Closed += (obj, arg) =>
                        {
                            _Preview = Preview.Preview;
                            _From = Preview.From;
                            _To = Preview.To;

                            if (Preview.Preview == true)
                            {
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            else
                            {
                                var Impresora = setting.FirstOrDefault();
                                foreach (String Impresoras in PrinterSettings.InstalledPrinters)
                                {
                                    if (Impresoras.ToString() == Impresora.Local || Impresoras.ToString() == Impresora.Red)
                                    {
                                        info.PrintOptions.PrinterName = Impresoras.ToString();
                                        PrinterSettings Printer = new PrinterSettings();

                                        foreach (PaperSize Size in Printer.PaperSizes)
                                        {
                                            int rawKind;
                                            if (Size.PaperName == Impresora.Papel)
                                            {
                                                rawKind = Convert.ToInt32(Size.GetType().GetField("kind", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Size));
                                                info.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                                                info.PrintToPrinter(1, true, _From, _To);
                                                Close();
                                                break;
                                            }
                                        }
                                        //Info.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"D:\Recibos.pdf");                           
                                    }
                                }
                            }
                        };
                        Preview.ShowDialog();
                    }
                }
                else
                {
                    rptEmpty Empty = new rptEmpty();
                    ReportViewer.ViewerCore.ReportSource = Empty;
                }
            }
            catch(Exception ex)
            {
                Payment.Bank.Core.Common.clsMisc Correo = new Core.Common.clsMisc();
                Correo.SendMail("stalin_ventura@outlook.com", "Error Payment Bank - " + clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, ex.Message);
                rptErrorData error = new rptErrorData();
                ReportViewer.ViewerCore.ReportSource = error;
            }
        }


        public void OnInit( DateTime _Hasta, int _SucursalID, int _RutaID, int _OficialID, int _ZonaID)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            RutaID = _RutaID;
            OficialID = _OficialID;
            ZonaID = _ZonaID;
        }

     
    }
}
