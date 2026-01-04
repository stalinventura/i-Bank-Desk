using MahApps.Metro.Controls;
using Payment.Bank.Controles;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.View.Informes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for frmConsultasAuxiliares.xaml
    /// </summary>
    public partial class frmConsultasAuxiliares : MetroWindow
    {
        Core.Manager db = new Core.Manager();
        public clsAuxiliaresBE BE = new clsAuxiliaresBE();
        int VISTA = 1;
        public bool isQuery;
        public frmConsultasAuxiliares()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            btnEditar.Click += btnEditar_Click;
            btnPrint.Visibility = Visibility.Visible;
            btnDelete.Click += btnDelete_Click;
            btnAgregar.Click += btnAgregar_Click;
            btnPrint.Click += btnPrint_Click;
            Loaded += frmConsultasAuxiliares_Loaded;
            txtBuscar.KeyUp += txtBuscar_KeyUp;
            txtBuscar.Focus();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmPrintCatalogo catalogo = new frmPrintCatalogo();
                catalogo.OnInit(clsVariablesBO.UsuariosBE.SucursalID);
                catalogo.ShowDialog();
            }
            catch { }
        }

        private void frmConsultasAuxiliares_Loaded(object sender, RoutedEventArgs e)
        {
            if (isQuery == true)
            {
                btnAgregar.Visibility = Visibility.Collapsed;
                btnEditar.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }
            Find();
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.AuxiliarID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        VISTA = 2;
                        frmAuxiliares Search = new frmAuxiliares();
                        Search.OnInit(BE, 2);
                        Visibility = Visibility.Hidden;
                        Search.Closed += (obj, args) =>
                        {
                            BE = new clsAuxiliaresBE(); dataGrid1.SelectedIndex = -1;
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
                    frmAuxiliares Search = new frmAuxiliares();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsAuxiliaresBE(); dataGrid1.SelectedIndex = -1;
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
                if (BE.AuxiliarID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.AuxiliaresDelete(BE.AuxiliarID);
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
                BE = (clsAuxiliaresBE)dataGrid1.SelectedItem;
                if (isQuery)
                {
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
                var Result = db.AuxiliaresGet(txtBuscar.Text);

                // ListCollectionView collection = new ListCollectionView(Result);
                // collection.GroupDescriptions.Add(new PropertyGroupDescription("Codigo"));

                dataGrid1.ItemsSource = Result; //collection;
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
