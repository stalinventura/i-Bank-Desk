using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.View.Formularios;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasNominas : MetroWindow
    {
        clsNominasBE BE = new clsNominasBE();
        clsNominasBE _BE = new clsNominasBE();

        Core.Manager db = new Core.Manager();
        int VISTA = 1;

        public frmConsultasNominas()
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
                Loaded += frmConsultasNominas_Loaded;
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
                frmPrintNominasGetByNominaID Facturas = new frmPrintNominasGetByNominaID();
                Facturas.Owner = this;
                Facturas.OnInit(BE.NominaID);
                Facturas.ShowDialog();

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

        private void frmConsultasNominas_Loaded(object sender, RoutedEventArgs e)
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
                if (BE.NominaID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        if (!BE.Contabilizado || clsVariablesBO.UsuariosBE.Roles.IsAdmin)
                        {
                            VISTA = 2;
                            frmNominas Search = new frmNominas();
                            Search.OnInit(_BE, 2);
                            Visibility = Visibility.Hidden;
                            Search.Closed += (obj, args) =>
                            {
                                BE = new clsNominasBE();
                                dataGrid1.SelectedIndex = -1;
                                Find();
                                Visibility = Visibility.Visible;
                            };
                            Search.Owner = this;
                            Search.ShowDialog();
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermissionPosted"), clsLenguajeBO.Find("msgTitle"));
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

        void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.Permiso.Agregar == true)
                {
                    VISTA = 1;
                    frmNominas Search = new frmNominas();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new clsNominasBE(); 
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
            catch { }
        }
        
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.NominaID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        if (BE.EstadoID == true)
                        {
                            frmNominasDelete Suspender = new frmNominasDelete();
                            Suspender.OnInit(_BE);
                            Suspender.Owner = this;
                            Suspender.Closed+= (obj, arg) => 
                            {
                                Find();
                            };
                            Suspender.ShowDialog();
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
            catch { 
                //clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                 BE = (clsNominasBE)dataGrid1.SelectedItem;
                _BE = db.NominasGetByNominaID(BE.NominaID);
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
                    var Result = db.NominasGet((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, txtBuscar.Text);
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

