
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
    /// Interaction logic for frmPrintRelacionIngresosInmueblesGetByFecha.xaml
    /// </summary>
    public partial class frmPrintRelacionIngresosInmueblesGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Desde;
        DateTime Hasta;
        String Documento = "-1";
        bool Resumen;
        int RutaID = -1;
        public frmPrintRelacionIngresosInmueblesGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintRelacionIngresosInmueblesGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintRelacionIngresosInmueblesGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintRelacionIngresosInmueblesGetByFecha(Desde, Hasta, SucursalID, Documento);
                if (bs.Tables["PagoRentas"].Rows.Count > 0)
                {
                    ReportDocument info;
                    info = new rptRecibosInmueblesGetByFecha();
                    info.SetDataSource(bs);
                    ReportViewer.ViewerCore.ReportSource = info;
                }
                else
                {
                    if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "F764-0764-BD11-4648-BC52-13A6-16E4-D436")
                    {
                        var data = db.PrintPagoRentasEmplyGetByFecha(Desde, Hasta);
                        rptEmptyRobxiel Empty = new rptEmptyRobxiel();
                        Empty.SetDataSource(data);
                        ReportViewer.ViewerCore.ReportSource = Empty;
                    }
                    else
                    {
                        rptErrorData error = new rptErrorData();
                        ReportViewer.ViewerCore.ReportSource = error;
                    }
                }
            }
            catch
            {
                rptErrorData error = new rptErrorData();
                ReportViewer.ViewerCore.ReportSource = error;
            }
        }


        public void OnInit(DateTime _Desde, DateTime _Hasta, int _SucursalID, string _Documento)
        {
            SucursalID = _SucursalID;
            Hasta = _Hasta;
            Desde = _Desde;
            Documento = _Documento;
        }

     
    }
}
