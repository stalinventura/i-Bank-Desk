
using CrystalDecisions.CrystalReports.Engine;
using Payment.Bank.Core;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes.Reportes;
using System.Windows;

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmSterlizerForm.xaml
    /// </summary>
    public partial class frmPrintContratosGetByContratoID : Window
    {
        Manager db = new Manager();
        clsContratosBE BE;
        bool IsPagare;

        public frmPrintContratosGetByContratoID()
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
                    ReportDocument info;
                    switch (BE.Solicitudes.TipoSolicitudID)
                    {
                        case 1:
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                {
                                    info = new rptContratoPersonal();
                                }
                                else
                                {
                                    info = new rptContratoPersonalFisico();
                                }
                            }
                            break;
                        case 2:
                            {
                                //if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                //{
                                //    info = new rptActoAutenticoHipotecas();
                                //}
                                //else
                                //{
                                //    info = new rptContratoPersonalFisico();
                                //}

                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                {
                                    if (BE.Solicitudes.GarantiaPersonal == null)
                                    {
                                        info = new rptActoAutenticoHipotecaEmpresas();
                                    }
                                    else
                                    {
                                        info = new rptActoAutenticoHipotecaEmpresasPersonal();
                                    }
                                }
                                else
                                {
                                    if (BE.Solicitudes.GarantiaPersonal == null)
                                    {
                                        info = new rptActoAutenticoHipotecaFisico();
                                    }
                                    else
                                    {
                                        info = new rptActoAutenticoHipotecaPersonalFisico();
                                    }
                                }
                            }
                            break;
                        case 3:
                            {
                                //if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                //{
                                //    info = new rptActoAutenticoVehiculos();
                                //}
                                //else
                                //{
                                //    info = new rptContratoPersonalFisico();
                                //}

                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                {
                                    if (BE.Solicitudes.GarantiaPersonal == null)
                                    {
                                        info = new rptActoAutenticoVehiculoEmpresas();
                                    }
                                    else
                                    {
                                        info = new rptActoAutenticoVehiculoEmpresasPersonal();
                                    }
                                }
                                else
                                {
                                    if (BE.Solicitudes.GarantiaPersonal == null)
                                    {
                                        info = new rptActoAutenticoVehiculoFisico();
                                    }
                                    else
                                    {
                                        info = new rptActoAutenticoVehiculoPersonalFisico();
                                    }
                                }
                            }
                            break;
                        default:
                            {
                                if (clsVariablesBO.UsuariosBE.Sucursales.Empresas.TipoEmpresaID == 0)
                                {
                                    if (BE.Solicitudes.GarantiaPersonal == null)
                                    {
                                        info = new rptContrato();
                                    }
                                    else
                                    {
                                        info = new rptContratoPersonal();
                                    }
                                }
                                else
                                {
                                    if (BE.Solicitudes.GarantiaPersonal == null)
                                    {
                                        info = new rptContratoFisico();
                                    }
                                    else
                                    {
                                        info = new rptContratoPersonalFisico();
                                    }
                                }
                            }
                            break;
                    }
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


        public void OnInit(clsContratosBE _BE, bool _IsPagare)
        {
            BE = _BE;
            IsPagare = _IsPagare;
        }

     
    }
}
