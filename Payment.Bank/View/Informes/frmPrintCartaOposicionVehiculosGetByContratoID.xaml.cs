using Payment.Bank.Core;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes.Reportes;
using System.Windows;

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintCartaOposicionVehiculosGetByContratoID.xaml
    /// </summary>
    public partial class frmPrintCartaOposicionVehiculosGetByContratoID : Window
    {
        Manager db = new Manager();
        clsContratosBE BE;

        public frmPrintCartaOposicionVehiculosGetByContratoID()
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
                var bs = db.PrintContratosGetByContratoID(BE.ContratoID, false);
                if (bs.Tables[0].Rows.Count > 0)
                {
                    switch (BE.Solicitudes.TipoSolicitudID)
                    {
                        case 2:
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                {
                                    rptActoEntregaVoluntariaHipotecasEmpresas info = new rptActoEntregaVoluntariaHipotecasEmpresas();
                                    info.SetDataSource(bs);
                                    ReportViewer.ViewerCore.ReportSource = info;
                                }
                                else
                                {
                                    rptActoEntregaVoluntariaHipotecasFisica info = new rptActoEntregaVoluntariaHipotecasFisica();
                                    info.SetDataSource(bs);
                                    ReportViewer.ViewerCore.ReportSource = info;
                                }
                            }
                            break;
                        case 3:
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                {
                                    rptCartaOposicionVehiculos info = new rptCartaOposicionVehiculos();
                                    info.SetDataSource(bs);
                                    ReportViewer.ViewerCore.ReportSource = info;
                                }
                                else
                                {
                                    rptCartaOposicionVehiculosFisico info = new rptCartaOposicionVehiculosFisico();
                                    info.SetDataSource(bs);
                                    ReportViewer.ViewerCore.ReportSource = info;
                                }
                            }break;

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

        public void OnInit(clsContratosBE _BE)
        {
            BE = _BE;
        }


    }
}
