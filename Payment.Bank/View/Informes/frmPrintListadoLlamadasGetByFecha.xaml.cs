
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
    /// Interaction logic for frmPrintListadoLlamadasGetByFecha.xaml
    /// </summary>
    public partial class frmPrintListadoLlamadasGetByFecha : Window
    {
        Manager db = new Manager();
        int SucursalID = -1;
        DateTime Hasta;
        DateTime Desde;
        bool IsQuery = false;

        public frmPrintListadoLlamadasGetByFecha()
        {
            InitializeComponent();
            Loaded += frmPrintListadoLlamadasGetByFecha_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintListadoLlamadasGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintListadoLlamadasGetByFecha(Desde, Hasta, SucursalID, IsQuery);
                if (bs.Tables[1].Rows.Count > 0)
                {
                    rptListadoLlamadasGetByFecha info = new rptListadoLlamadasGetByFecha();
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


        public void OnInit(DateTime _Desde, DateTime _Hasta, int _SucursalID, bool _isquery = false)
        {
            SucursalID = _SucursalID;
            Desde = _Desde;
            Hasta = _Hasta;
            IsQuery = _isquery;
        }

     
    }
}
