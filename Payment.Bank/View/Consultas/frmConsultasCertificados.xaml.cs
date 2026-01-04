using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Xml.Linq;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasCertificados : MetroWindow
    {
        clsCertificadosBE BE = new clsCertificadosBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasCertificados()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                LoadCombox();
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                cmbEstadoContratos.SelectionChanged += cmbEstadoContratos_SelectionChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                btnCuotas.Click += btnCuotas_Click;
                btnEstadoCuentas.Click += btnEstadoCuentas_Click;
                btnContratos.Click += btnContratos_Click;
                btnSms.Click += btnSms_Click;
                txtBuscar.Focus();
            }
            catch{}
        }

        private void btnSms_Click(object sender, RoutedEventArgs e)
        {
            frmCertificados Certificados = new frmCertificados();
            Certificados.Owner = this;
            Certificados.ShowDialog();
        }

        private void btnContratos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.CertificadoID > 0)
                {
                    frmPrintCertificados Certificados = new frmPrintCertificados();
                    Certificados.OnInit(BE.CertificadoID);
                    Certificados.Owner = this;
                    Certificados.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnEstadoCuentas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.CertificadoID > 0)
                {
                    frmPrintEstadoCuentasGetByContratoID Cuotas = new frmPrintEstadoCuentasGetByContratoID();
                    Cuotas.OnInit(BE.CertificadoID);
                    //Cuotas.Closed += (arg, obj)=> { OpcionesDesactivar(); BE = new clsContratosBE(); };
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch(Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnCuotas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.CertificadoID > 0)
                {
                    //frmConsultasCuotas Cuotas = new frmConsultasCuotas();
                    //Cuotas.OnInit(BE);
                    ////Cuotas.Closed += (arg, obj)=> { OpcionesDesactivar(); BE = new clsContratosBE(); };
                    //Cuotas.Owner = this;
                    //Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

 

        private void cmbEstadoContratos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                Find();
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Estados Contratos
            List<clsEstadoContratosBE> EstadoContratos = new List<clsEstadoContratosBE>();
            EstadoContratos.Add(new clsEstadoContratosBE { EstadoID = 1, Estado = "Activo" });
            EstadoContratos.Add(new clsEstadoContratosBE { EstadoID = 0, Estado = "Inactivo" });

            cmbEstadoContratos.ItemsSource = EstadoContratos;
            cmbEstadoContratos.SelectedValuePath = "EstadoID";
            cmbEstadoContratos.DisplayMemberPath = "Estado";
            cmbEstadoContratos.SelectedValue = 1;
     
            Find();
        }

        private void OpcionesActivar()
        {
            btnEstadoCuentas.Visibility = Visibility.Visible;
            btnCuotas.Visibility = Visibility.Visible;
            btnSms.Visibility = Visibility.Visible;
            btnContratos.Visibility = Visibility.Visible;
        }

        private void OpcionesDesactivar()
        {
            btnEstadoCuentas.Visibility = Visibility.Collapsed;
            btnCuotas.Visibility = Visibility.Collapsed;
            btnSms.Visibility = Visibility.Collapsed;
            btnContratos.Visibility = Visibility.Collapsed;
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.CertificadoID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmCertificados Search = new frmCertificados();
                        Search.OnInit(BE.CertificadoID, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsCertificadosBE(); dataGrid1.SelectedIndex = -1;
                            Find();
                            Visibility = Visibility.Visible;
                        };
                        Search.Owner = this;
                        Search.ShowDialog();

                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.Permiso.Agregar == true)
                {
                    VISTA = 1;
                    frmCertificados Search = new frmCertificados();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsCertificadosBE(); dataGrid1.SelectedIndex = -1;
                        Find();
                        Visibility = Visibility.Visible;

                    };
                    Search.Owner = this;
                    Search.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }
        
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (BE.CertificadoID > 0)
                //{
                //    if (clsVariablesBO.Permiso.Eliminar == true)
                //    {
                //        switch(BE.EstadoID)
                //        {
                //            case 1:
                //                {
                //                    frmSuspenderContratos Suspender = new frmSuspenderContratos();
                //                    Suspender.OnInit(BE);
                //                    Suspender.Owner = this;
                //                    Suspender.btnSalir.Click += (obj, arg) => { Find(); };
                //                    Suspender.ShowDialog();
                //                }
                //                break;
                //            case 2:
                //                {
                //                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgCancelContract"), clsLenguajeBO.Find("msgTitle"));
                //                } break;
                //            default:
                //                {
                //                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgSuspendContract"), clsLenguajeBO.Find("msgTitle"));
                //                }
                //                break;
                //        }

                //    }
                //    else
                //    {
                //        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                //    }
                //}
                //else
                //{
                //    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                //}
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsCertificadosBE)dataGrid1.SelectedItem;
                OpcionesActivar();
            }
            catch { }
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Find();
                }
            }
            catch { }
        }

        public void Find()
        {
            try
            {
                OpcionesDesactivar();
                int value = (int)cmbEstadoContratos.SelectedValue;
                var Result = db.CertificadosGet(txtBuscar.Text, Convert.ToBoolean(value));
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " +String.Format("{0:N}", Result.Count.ToString());
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }


    }


}

