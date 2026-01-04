
using CrystalDecisions.CrystalReports.Engine;
using Payment.Bank.Core;
using Payment.Bank.Core.Model;
using Payment.Bank.Entity;
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
    public partial class frmPrintContratosAutenticoGetByContratoID : Window
    {
        Manager db = new Manager();
        clsContratosBE BE;
        bool IsPagare;
        public frmPrintContratosAutenticoGetByContratoID()
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
                var bs = db.PrintContratosGetByContratoID(BE.ContratoID, IsPagare);      
                if (bs.Tables[0].Rows.Count > 0)
                {
                    if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                    {
                        rptPagarePersonal info = new rptPagarePersonal();
                        info.SetDataSource(bs);
                        ReportViewer.ViewerCore.ReportSource = info;
                    }
                    else
                    {
                        rptPagarePersonalFisico info = new rptPagarePersonalFisico();
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

        public void OnInit(clsContratosBE _BE, bool _IsPagare)
        {
            BE = _BE;
            IsPagare = _IsPagare;
        }


    }
}
