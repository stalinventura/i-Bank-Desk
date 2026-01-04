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
    public partial class frmConsultasSolicitudCheques : MetroWindow
    {
        clsSolicitudChequesBE BE = new clsSolicitudChequesBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        public bool IsQuery;

        public int _SolicitudID = 0;
        public frmConsultasSolicitudCheques()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                LoadCombox();
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                cmbEstados.SelectionChanged += cmbEstados_SelectionChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtBuscar.Focus();
            }
            catch{}
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           try
            {
                frmPrintSolicitudChequesGetBySolicitudChequeID Solicitud = new frmPrintSolicitudChequesGetBySolicitudChequeID();
                Solicitud.OnInit(BE.SolicitudChequeID);
                Solicitud.ShowDialog();
            }
            catch { }
        }

        private void cmbEstados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                Find();
            }
            catch { }
        }

        private void LoadCombox()
        {
            //Estados Solicitudes
            List<clsEstadoSolictudChequesBE> Estados = new List<clsEstadoSolictudChequesBE>();
            Estados = db.EstadoSolicitudChequesGet(null).ToList();
            Estados.Add(new clsEstadoSolictudChequesBE { EstadoID = -1, Estado = clsLenguajeBO.Find("itemSelect") });
            cmbEstados.ItemsSource = Estados;
            cmbEstados.SelectedValuePath = "EstadoID";
            cmbEstados.DisplayMemberPath = "Estado";
            if (Estados.Count() > 1)
            {
                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbEstados.IsEnabled = true;
                }
                else
                {
                    cmbEstados.IsEnabled = false;
                }

                cmbEstados.SelectedValue = Estados.Where(x => x.IsDefault == true).FirstOrDefault().EstadoID;
            }
            else
            {
                cmbEstados.SelectedValue = -1;
            }

            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.SolicitudChequeID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmSolicitudCheques Search = new frmSolicitudCheques();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsSolicitudChequesBE(); dataGrid1.SelectedIndex = -1;
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
                if (clsVariablesBO.Permiso.Agregar == true && BE.Cheques == null)
                {
                    VISTA = 1;
                    frmSolicitudCheques Search = new frmSolicitudCheques();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsSolicitudChequesBE(); dataGrid1.SelectedIndex = -1;
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
                if (BE.SolicitudChequeID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        if (BE.Cheques == null)
                        {
                            frmMessageBox Mensaje = new frmMessageBox();
                            Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                            Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                            Mensaje.btnAceptar.Click += (obj, args) =>
                            {
                                OperationResult result = db.SolicitudChequesDelete(BE.SolicitudChequeID, 0, clsVariablesBO.UsuariosBE.Documento);
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
                BE = (clsSolicitudChequesBE)dataGrid1.SelectedItem;
                if (IsQuery == true)
                {
                    _SolicitudID = BE.SolicitudChequeID;
                    Close();
                }
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
                if (IsQuery == true)
                {
                    cmbEstados.IsEnabled = false;
                }

                var Result = db.SolicitudChequesGet(txtBuscar.Text, (int)cmbEstados.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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

