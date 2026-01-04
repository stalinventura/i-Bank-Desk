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
using Payment.Bank.Common;
using Payment.Bank.View.Informes.Reportes;
using System.IO;
using System.Data;
using Payment.Bank.View.Informes;
using Payment.Bank.Core.Model;
using Payment.Bank.Core.Entity;
using System.Drawing.Printing;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for frmConsultasCheques.xaml
    /// </summary>
    public partial class frmConsultasCheques : MetroWindow
    {
        clsChequesBE BE = new clsChequesBE();

        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasCheques()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);

                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtDesde.SelectedDateChanged += txtDesde_SelectedDateChanged;
                txtHasta.SelectedDateChanged += txtHasta_SelectedDateChanged;
                Loaded += frmConsultasRecibos_Loaded;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                txtBuscar.Focus();

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
            catch { }
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void txtHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Find();
            }
            catch { }
        }

        private void txtDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Find();
            }
            catch { }
        }

        private void frmConsultasRecibos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDesde.SelectedDate = DateTime.Today;
                txtHasta.SelectedDate = DateTime.Today;
            }
            catch { }
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
                        frmCheques Search = new frmCheques();
                        Search.OnInit(BE.SolicitudCheques, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsChequesBE(); dataGrid1.SelectedIndex = -1;
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
                    frmCheques Search = new frmCheques();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsChequesBE(); dataGrid1.SelectedIndex = -1;
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
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.ChequesDelete(BE.SolicitudChequeID);
                            if (result.ResponseCode == "00")
                            {
                                var item = db.DetalleEntradasGet(null).Where(x => x.Numero == BE.SolicitudChequeID && x.TipoEntradaID == 3).FirstOrDefault();
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
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsChequesBE)dataGrid1.SelectedItem;
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
                if (txtDesde.SelectedDate != null && txtHasta.SelectedDate != null)
                {
                    var Result = db.ChequesGet((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, txtBuscar.Text, clsVariablesBO.UsuariosBE.Documento);
                    dataGrid1.ItemsSource = Result.ToList();
                    lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
                }

            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }


}

