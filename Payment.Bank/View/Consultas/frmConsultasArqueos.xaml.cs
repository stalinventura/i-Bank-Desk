using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.Controles;
using Payment.Bank.View.Informes;
using Payment.Bank.Core.Entity;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasArqueos : MetroWindow
    {
        clsArqueosBE BE = new clsArqueosBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasArqueos()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtDesde.SelectedDate = DateTime.Today;
                txtHasta.SelectedDate = DateTime.Today;
                txtDesde.SelectedDateChanged += txtDesde_SelectedDateChanged;   
                txtHasta.SelectedDateChanged += txtHasta_SelectedDateChanged;   
                txtBuscar.Focus();
                Loaded += frmConsultasArqueos_Loaded;
            }
            catch{}
        }

        private void frmConsultasArqueos_Loaded(object sender, RoutedEventArgs e)
        {
            Find();
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(BE.ArqueoID >0)
                {
                    frmPrintArqueosGetByArqueoID Search = new frmPrintArqueosGetByArqueoID();
                    Search.OnInit(BE.ArqueoID);
                    Search.Owner = this;
                    Search.ShowDialog();    
                }
            }
            catch { }
        }

        private void txtDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Find();
        }

        private void txtHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           Find();
        }

        private void cmbEstadoSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                if (BE.ArqueoID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true && clsVariablesBO.UsuariosBE.Roles.IsAdmin && BE.EstadoID == true)
                    {
                        VISTA = 2;
                        frmCierres Search = new frmCierres();
                        //Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsArqueosBE(); 
                            dataGrid1.SelectedIndex = -1;
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
                    var row = db.OpenBoxGetByDocumento(clsVariablesBO.UsuariosBE.Documento);
                    if (row == null)
                    {
                        VISTA = 1;
                        frmCierres Search = new frmCierres();
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsArqueosBE();
                            dataGrid1.SelectedIndex = -1;
                            Find();
                            Visibility = Visibility.Visible;
                        };
                        Search.Owner = this;
                        Search.ShowDialog();
                    }
                    else
                    {   
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgOpenBox"), clsLenguajeBO.Find("msgTitle"));
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
                if (BE.ArqueoID > 0)
                {
                    TimeSpan ts = DateTime.Now - BE.Fecha;
                    if (clsVariablesBO.Permiso.Eliminar == true && clsVariablesBO.UsuariosBE.Roles.IsAdmin && ts.Minutes < 2)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.ArqueosDelete(BE.ArqueoID);
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
                BE = (clsArqueosBE)dataGrid1.SelectedItem;
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
                var Result = db.ArqueosGet(txtBuscar.Text, (DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate);
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

