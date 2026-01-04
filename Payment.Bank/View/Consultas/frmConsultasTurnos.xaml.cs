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
    public partial class frmConsultasTurnos : MetroWindow
    {
        clsTurnosBE BE = new clsTurnosBE();
        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasTurnos()
        {
            InitializeComponent();
            try
            {
                Title = clsLenguajeBO.Find(Title.ToString());
                clsLenguajeBO.Load(gridMenu);
                LoadCombox();
                txtBuscar.KeyUp += txtBuscar_KeyUp;
                btnSalir.Click += btnSalir_Click;
                cmbEstadoSolicitudes.SelectionChanged += cmbEstadoSolicitudes_SelectionChanged;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnEditar.Click += btnEditar_Click;
                btnDelete.Click += btnDelete_Click;
                btnAgregar.Click += btnAgregar_Click;
                txtBuscar.Focus();
            }
            catch{}
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
            //Estados Solicitudes
            List<clsEstadoSolicitudesBE> EstadoSolicitudes = new List<clsEstadoSolicitudesBE>();
            EstadoSolicitudes.Add(new clsEstadoSolicitudesBE { EstadoID = 0, Estado = "Inactivo(a)" });
            EstadoSolicitudes.Add(new clsEstadoSolicitudesBE { EstadoID = 1, Estado = "Activo(a)" });
            cmbEstadoSolicitudes.ItemsSource = EstadoSolicitudes;
            cmbEstadoSolicitudes.SelectedValuePath = "EstadoID";
            cmbEstadoSolicitudes.DisplayMemberPath = "Estado";
            cmbEstadoSolicitudes.SelectedValue = 1;
            

            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.TurnoID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true && clsVariablesBO.UsuariosBE.Roles.IsAdmin && BE.EstadoID == true)
                    {
                        VISTA = 2;
                        frmTurnos Search = new frmTurnos();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsTurnosBE(); 
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
                        frmTurnos Search = new frmTurnos();
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsTurnosBE();
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
                if (BE.TurnoID > 0)
                {
                    TimeSpan ts = DateTime.Now - BE.Fecha;
                    if (clsVariablesBO.Permiso.Eliminar == true && clsVariablesBO.UsuariosBE.Roles.IsAdmin && ts.Minutes < 2)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.TurnosDelete(BE.TurnoID);
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
                BE = (clsTurnosBE)dataGrid1.SelectedItem;
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
                if (!clsVariablesBO.UsuariosBE.Roles.IsAdmin)
                {
                    cmbEstadoSolicitudes.IsEnabled = false;
                }

                var Result = db.TurnosGet(txtBuscar.Text, (int)cmbEstadoSolicitudes.SelectedValue);
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

