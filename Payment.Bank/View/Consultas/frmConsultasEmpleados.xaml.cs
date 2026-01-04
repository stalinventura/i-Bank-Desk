using MahApps.Metro.Controls;
using Payment.Bank.Controles;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for frmConsultasEmpleados.xaml
    /// </summary>
    public partial class frmConsultasEmpleados : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        clsEmpleadosBE BE = new clsEmpleadosBE();
        int VISTA = 1;

        public frmConsultasEmpleados()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            btnEditar.Click += btnEditar_Click;
            btnDelete.Click += btnDelete_Click;
            btnAgregar.Click += btnAgregar_Click;
            Loaded += frmConsultasEmpleados_Loaded;
            txtBuscar.KeyUp += txtBuscar_KeyUp;
            txtBuscar.Focus();
        }

        private void frmConsultasEmpleados_Loaded(object sender, RoutedEventArgs e)
        {
            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(BE.Documento))
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmEmpleados Search = new frmEmpleados();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsEmpleadosBE(); dataGrid1.SelectedIndex = -1;
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
                    frmEmpleados Search = new frmEmpleados();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                   {
                       BE = new clsEmpleadosBE(); dataGrid1.SelectedIndex = -1;
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
                if (!string.IsNullOrEmpty(BE.Documento))
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                            frmMessageBox Mensaje = new frmMessageBox();
                            Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                            Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                            Mensaje.btnAceptar.Click += (obj, args) =>
                            {
                                OperationResult result = db.EmpleadosDelete(BE.EmpleadoID);
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
                        Mensaje.Owner = this;
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
                BE = (clsEmpleadosBE)dataGrid1.SelectedItem;
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
                var Result = db.EmpleadosGet(txtBuscar.Text);
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count().ToString());
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
