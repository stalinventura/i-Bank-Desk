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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasPagoRentas : MetroWindow
    {
        PagoRentas BE = new PagoRentas();
        clsPagoRentasBE _BE = new clsPagoRentasBE();

        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasPagoRentas()
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
                Loaded += frmConsultasPagoRentas_Loaded;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                txtBuscar.Focus();
                
                if(clsVariablesBO.UsuariosBE.Roles.CanQuery == true)
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

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                frmPrintPagoRentasGetByPagoRentaID PagoRentas = new frmPrintPagoRentasGetByPagoRentaID();
                PagoRentas.Owner = this;
                PagoRentas.OnInit(BE.PagoRentaID);
                PagoRentas.ShowDialog();

            }
            catch { }
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
            try {
                Find();
            }
            catch { }
        }

        private void frmConsultasPagoRentas_Loaded(object sender, RoutedEventArgs e)
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
                if (BE.PagoRentaID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmPagoRentas Search = new frmPagoRentas();
                        Search.OnInit(_BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new PagoRentas(); dataGrid1.SelectedIndex = -1;
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

                    if (!db.IsOpenBox(clsVariablesBO.UsuariosBE.Documento) && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.IsOpenCloseBox == true)
                    {
                        var row = db.OpenBoxGetByDocumento(clsVariablesBO.UsuariosBE.Documento);
                        if (row == null)
                        {
                            frmTurnos turnos = new frmTurnos();
                            turnos.Owner = this;
                            turnos.btnSalir.Click += (obj, arg) =>
                            {
                                if (!db.IsOpenBox(clsVariablesBO.UsuariosBE.Documento))
                                {
                                    return;
                                }
                            };
                            turnos.ShowDialog();
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgOpenBox"), clsLenguajeBO.Find("msgTitle"));
                        }
                    }
                    else
                    {
                        VISTA = 1;
                        frmPagoRentas Search = new frmPagoRentas();
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new PagoRentas(); dataGrid1.SelectedIndex = -1;
                            Find();
                            Visibility = Visibility.Visible;
                        };
                        Search.Owner = this;
                        Search.ShowDialog();
                    }
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
                if (BE.PagoRentaID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.PagoRentasDelete(BE.PagoRentaID);
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
                BE = (PagoRentas)dataGrid1.SelectedItem;
                _BE = db.PagoRentasGetByPagoRentaID(BE.PagoRentaID);
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
                    var Result = db.PagoRentasGet((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, txtBuscar.Text, clsVariablesBO.UsuariosBE.Documento);
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

