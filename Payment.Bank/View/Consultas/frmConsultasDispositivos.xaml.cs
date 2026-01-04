using MahApps.Metro.Controls;
using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
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
    /// Interaction logic for frmConsultasDispositivos.xaml
    /// </summary>
    public partial class frmConsultasDispositivos : MetroWindow
    {
        Bank.Core.Manager db = new Core.Manager();
        int VISTA = 1;
        clsDispositivosBE BE = new clsDispositivosBE();
        public frmConsultasDispositivos()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            btnEditar.Click += btnEditar_Click;
            btnDelete.Click += btnDelete_Click;
            btnAgregar.Click += btnAgregar_Click;
            Loaded += frmConsultasDispositivos_Loaded;
            txtBuscar.KeyUp += txtBuscar_KeyUp;
            txtBuscar.Focus();
        }

        private void frmConsultasDispositivos_Loaded(object sender, RoutedEventArgs e)
        {
            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(BE.DispositivoID))
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmDispositivos Search = new frmDispositivos();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsDispositivosBE(); dataGrid1.SelectedIndex = -1;
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
                    frmDispositivos Search = new frmDispositivos();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                   {
                       BE = new clsDispositivosBE(); dataGrid1.SelectedIndex = -1;
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
                if (!string.IsNullOrEmpty(BE.DispositivoID))
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                            frmMessageBox Mensaje = new frmMessageBox();
                            Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                            Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                            Mensaje.btnAceptar.Click += (obj, args) =>
                            {
                                OperationResult result = db.DispositivosDelete(BE.DispositivoID);
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
                BE = (clsDispositivosBE)dataGrid1.SelectedItem;
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
                var Result = db.DispositivosGet(txtBuscar.Text);
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
