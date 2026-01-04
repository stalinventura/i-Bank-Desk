using MahApps.Metro.Controls;
using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Core.Entity;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.View.Informes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for frmConsultasEntradas.xaml
    /// </summary>
    public partial class frmConsultasEntradas : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        int VISTA = 1;
        clsEntradasBE BE = new clsEntradasBE();
        public frmConsultasEntradas()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            btnEditar.Click += btnEditar_Click;
            btnDelete.Click += btnDelete_Click;
            btnAgregar.Click += btnAgregar_Click;
            btnPrint.Click += btnPrint_Click;
            Loaded += frmConsultasEntradas_Loaded;
            txtBuscar.KeyUp += txtBuscar_KeyUp;
            txtDesde.SelectedDateChanged += txtDesde_SelectedDateChanged;
            txtHasta.SelectedDateChanged += txtHasta_SelectedDateChanged;
            dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
            txtBuscar.Focus();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmRelacionEntradasGetByFecha entradas = new frmRelacionEntradasGetByFecha();
                entradas.Owner = this;
                entradas.OnInit((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate);
                entradas.ShowDialog();
            }
            catch { }
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (BE != null)
                {
                    frmPrintEntradasGetByEntradaID entradas = new frmPrintEntradasGetByEntradaID();
                    entradas.Owner = this;
                    entradas.OnInit(BE.EntradaID);
                    entradas.ShowDialog();
                }
            }
            catch { }
        }

        private void frmConsultasEntradas_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDesde.SelectedDate = DateTime.Today;
                txtHasta.SelectedDate = DateTime.Today;
                Find();
            }
            catch { }


        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.EntradaID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmEntradas Search = new frmEntradas();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsEntradasBE(); dataGrid1.SelectedIndex = -1;
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
                    frmEntradas Search = new frmEntradas();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsEntradasBE(); dataGrid1.SelectedIndex = -1;
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
                if (BE.EntradaID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.EntradasDelete(BE.EntradaID);
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
                BE = (clsEntradasBE)dataGrid1.SelectedItem;
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

        public void Find()
        {
            try
            {
                var Result = db.EntradasGet((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, txtBuscar.Text, clsVariablesBO.UsuariosBE.Documento);
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count().ToString());
                if (Result.Count() == 0)
                {
                    btnPrint.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnPrint.Visibility = Visibility.Visible;
                }
                CalcularTotales(Result);
            }
            catch { }
        }

        void CalcularTotales(List<clsEntradasBE> entradas)
        {
            try
            {
                float debitos = 0;
                float creditos = 0;

                lblTotalDebito.Text = string.Format("{0:N2}", debitos);
                lblTotalCredito.Text = string.Format("{0:N2}", creditos);

                foreach (var row in entradas.Where(x => x.EstadoID == true))
                {
                    debitos += row.DetalleEntradas.Sum(x => x.Debito);
                    creditos += row.DetalleEntradas.Sum(x => x.Credito);
                }
                lblTotalDebito.Text = string.Format("{0:N2}", debitos);
                lblTotalCredito.Text = string.Format("{0:N2}", creditos);
                if (debitos == creditos)
                {
                    lblMessage.Text = string.Empty;
                }
                else
                {
                    lblMessage.Text = clsLenguajeBO.Find("AccountBalanced");
                }
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
