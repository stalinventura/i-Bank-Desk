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
    /// Interaction logic for frmConsultasDesembolsos.xaml
    /// </summary>
    public partial class frmConsultasDesembolsos : MetroWindow
    {
        clsDesembolsosBE BE = new clsDesembolsosBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        public bool IsQuery;

        public int _SolicitudID = 0;
        public frmConsultasDesembolsos()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
               
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;

                txtDesde.SelectedDateChanged += txtDesde_SelectedDateChanged;
                txtHasta.SelectedDateChanged += txtHasta_SelectedDateChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtBuscar.Focus();
                Loaded += frmConsultasDesembolsos_Loaded;

                if (clsVariablesBO.UsuariosBE.Roles.CanQuery == true)
                {
                    txtDesde.IsEnabled = true;
                    txtHasta.IsEnabled = true;
                }
                else
                {
                    txtDesde.IsEnabled = false;
                    txtHasta.IsEnabled = false;
                }
            }
            catch{}
        }

        private void frmConsultasDesembolsos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDesde.SelectedDate = DateTime.Today;
                txtHasta.SelectedDate = DateTime.Today;
            }
            catch { }
        }

        private void txtHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Find();
        }

        private void txtDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Find();
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           try
            {
                frmPrintDesembolsosGetByDesembolsoID Solicitud = new frmPrintDesembolsosGetByDesembolsoID();
                Solicitud.OnInit(BE.DesembolsoID);
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

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.DesembolsoID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmDesembolsos Search = new frmDesembolsos();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsDesembolsosBE(); dataGrid1.SelectedIndex = -1;
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
                    frmDesembolsos Search = new frmDesembolsos();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsDesembolsosBE(); dataGrid1.SelectedIndex = -1;
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
                if (BE.DesembolsoID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        if (BE != null)
                        {
                            frmMessageBox Mensaje = new frmMessageBox();
                            Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                            Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                            Mensaje.btnAceptar.Click += (obj, args) =>
                            {
                                OperationResult result = db.DesembolsosDelete(BE.DesembolsoID, clsVariablesBO.UsuariosBE.Documento);
                                if (result.ResponseCode == "00")
                                {
                                    var item = db.DetalleEntradasGet(null).Where(x => x.Numero == BE.DesembolsoID && x.TipoEntradaID == 4).FirstOrDefault();
                                    db.EntradasDelete(item.EntradaID);

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
                BE = (clsDesembolsosBE)dataGrid1.SelectedItem;
                if (IsQuery == true)
                {
                    _SolicitudID = BE.DesembolsoID;
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
                var Result = db.DesembolsosGet((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, txtBuscar.Text, clsVariablesBO.UsuariosBE.Documento);
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }


}

