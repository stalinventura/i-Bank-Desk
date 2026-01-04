
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
    /// Interaction logic for frmPrintPagoRentasGetByPagoRentaID.xaml
    /// </summary>
    public partial class frmPrintPagoRentasGetByPagoRentaID : Window
    {
        Manager db = new Manager();
        int ReciboID = 0;

        public frmPrintPagoRentasGetByPagoRentaID()
        {
            InitializeComponent();
            Loaded += frmPrintPagoRentasGetByPagoRentaID_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintPagoRentasGetByPagoRentaID_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintPagoRentasGetByPagoRentaID(ReciboID);
                if (bs != null)
                {
                    rptPagoRentasGetByPagoRentaID info = new rptPagoRentasGetByPagoRentaID();
                    info.SetDataSource(bs);
                    var setting = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Impresiones.Where(x => x.TipoImpresionID == 100003 && x.DispositivoID == Common.FingerPrint.GetKey());
                    if (setting.Count() == 0)
                    {
                        ReportViewer.ViewerCore.ReportSource = info;
                    }
                    else
                    {
                        bool _Preview = false;
                        int _From = 1;
                        int _To = bs.Tables[0].Rows.Count;
                        int _Copy = setting.FirstOrDefault().Copias;

                        frmPreview Preview = new frmPreview();
                        Preview.To = _To;
                        Preview.Owner = this;
                        Preview.Copy = _Copy;
                        Preview.Closed += (obj, arg) =>
                        {
                            _Preview = Preview.Preview;
                            _From = Preview.From;
                            _To = Preview.To;
                            _Copy = Preview.Copy;

                            if (Preview.Preview == true)
                            {
                                ReportViewer.ViewerCore.ReportSource = info;
                            }
                            else
                            {
                                var Impresora = setting.FirstOrDefault();
                                if (clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsPayPoint == true)
                                {                                    
                                    foreach (String Impresoras in PrinterSettings.InstalledPrinters)
                                    {
                                        if (Impresoras.ToString() == Impresora.Local || Impresoras.ToString() == Impresora.Red)
                                        {
                                            Modulos.clsPrinterBE Print = new Modulos.clsPrinterBE();
                                            for (int i = 1; i <= _Copy; i++)
                                            {
                                                Print.RentasPrint(ReciboID, Impresoras.ToString(), Impresora.Papel, Convert.ToSingle(Impresora.Ancho), Convert.ToSingle(Impresora.Alto));
                                            }
                                            Print.OpenCashDrawer();
                                            Close();
                                        }
                                    }
                                }
                                else
                                {             
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
            catch
            {
                rptErrorData error = new rptErrorData();
                ReportViewer.ViewerCore.ReportSource = error;
            }
        }


        public DataSet EntityToDataSet<TEntity>(DataSet ds, List<TEntity> MyEntities)
        {
            string strTableName = null;
            DataRow drNewRow = default(DataRow);
            foreach (var POCO in MyEntities)
            {              
                dynamic EntityFields = POCO.GetType().GetProperties().Where(a => a.CanRead);
                strTableName = "Recibos";// POCO.GetType().Name;
                drNewRow = ds.Tables[strTableName].NewRow();
                foreach (var field in EntityFields)
                {                                                               
                    if (drNewRow.Table.Columns.Contains(field.Name))
                    {
                        drNewRow[field.Name] = field.GetValue(POCO, null);
                    }
                }
                ds.Tables[strTableName].Rows.Add(drNewRow);
            }
            return ds;
        }


        public DataTable EntityToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }


        public void OnInit(int _ReciboID)
        {
            ReciboID = _ReciboID;
        }

     
    }
}
