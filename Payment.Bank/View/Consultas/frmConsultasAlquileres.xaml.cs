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
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes;
using Payment.Bank.Core.Entity;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasAlquileres : MetroWindow
    {
        clsAlquileresBE BE = new clsAlquileresBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        public bool IsQuery;
        public int _AlquilerID = 0;
        public int _EstadoID;
        public frmConsultasAlquileres()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                Loaded += FrmConsultasAlquileres_Loaded;
                LoadCombox();
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                cmbEstadoSolicitudes.SelectionChanged += cmbEstadoSolicitudes_SelectionChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;

                btnCuotas.Click += btnCuotas_Click;
                btnSms.Click += btnSms_Click;
                btnEstadoCuentas.Click += btnEstadoCuentas_Click;
                btnContratos.Click += btnContratos_Click;

                txtBuscar.Focus();
            }
            catch{}
        }

        private void FrmConsultasAlquileres_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsQuery == true)
            {
                cmbEstadoSolicitudes.IsEnabled = false;
            }
        }


        private void btnContratos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.AlquilerID > 0)
                {
                    frmPrintAlquileresGetByAlquilerID Cuotas = new frmPrintAlquileresGetByAlquilerID();
                    Cuotas.OnInit(BE.AlquilerID);
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();
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
                if (BE.AlquilerID > 0)
                {
                    frmPrintEstadoCuentasGetByContratoID Cuotas = new frmPrintEstadoCuentasGetByContratoID();
                    Cuotas.OnInit(BE.AlquilerID);
                    //Cuotas.Closed += (arg, obj)=> { OpcionesDesactivar(); BE = new clsAlquileresBE(); };
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSms_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.AlquilerID == 0)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        frmConsultasSms sms = new frmConsultasSms();
                        sms.Owner = this;
                        sms.ContratoID = BE.AlquilerID;
                        sms.ShowDialog();
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
        private void btnCuotas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.AlquilerID > 0)
                {
                    frmConsultasRentas Cuotas = new frmConsultasRentas();
                    Cuotas.OnInit(BE);
                    //Cuotas.Closed += (arg, obj)=> { OpcionesDesactivar(); BE = new clsAlquileresBE(); };
                    Cuotas.Owner = this;
                    Cuotas.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void cmbEstadoSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                Find();
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Estados clsAlquileresBE
            List<clsEstadoSolicitudesBE> EstadoclsAlquileresBE = new List<clsEstadoSolicitudesBE>();
            EstadoclsAlquileresBE.Add(new clsEstadoSolicitudesBE { EstadoID = 0, Estado = clsLenguajeBO.Find("Inactive") });
            EstadoclsAlquileresBE.Add(new clsEstadoSolicitudesBE { EstadoID = 1, Estado = clsLenguajeBO.Find("Active") });
            cmbEstadoSolicitudes.ItemsSource = EstadoclsAlquileresBE;
            cmbEstadoSolicitudes.SelectedValuePath = "EstadoID";
            cmbEstadoSolicitudes.DisplayMemberPath = "Estado";
            _EstadoID = 1;
            cmbEstadoSolicitudes.SelectedValue = _EstadoID;
         

            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.AlquilerID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmAlquileres Search = new frmAlquileres();
                        Search.OnInit(BE.AlquilerID, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsAlquileresBE(); dataGrid1.SelectedIndex = -1;
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
                    frmAlquileres Search = new frmAlquileres();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsAlquileresBE(); dataGrid1.SelectedIndex = -1;
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
                if (BE.AlquilerID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.AlquileresDelete(BE.AlquilerID);
                            if (result.ResponseCode == "00")
                            {
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgDelete"), clsLenguajeBO.Find("msgTitle"));
                            }
                            else
                            {
                                clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            }
                            Find();
                            Mensaje.Close();
                        };

                        Mensaje.btnSalir.Click += (obj, args) =>
                        {
                            Mensaje.Close();
                        };
                        Mensaje.ShowDialog();
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

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsAlquileresBE)dataGrid1.SelectedItem;
                if (IsQuery == true)
                {
                    _AlquilerID = BE.AlquilerID;
                    Close();
                }
                else
                {
                    OpcionesActivar();
                }

            }
            catch { }
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
                if (IsQuery == true)
                {
                    cmbEstadoSolicitudes.IsEnabled = false;
                    cmbEstadoSolicitudes.SelectedValue = _EstadoID;
                }
                OpcionesDesactivar();
                var Result = db.AlquileresGet(txtBuscar.Text, Convert.ToBoolean((int)cmbEstadoSolicitudes.SelectedValue));
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }


}

