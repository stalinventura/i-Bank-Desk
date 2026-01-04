using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;


using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmListadoLlamadasGetByFecha.xaml
    /// </summary>
    public partial class frmListadoLlamadasGetByFecha : MetroWindow
    {
 
        Core.Manager db = new Core.Manager();
        public frmListadoLlamadasGetByFecha() 
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridCiudades);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmRelacionContratosGetByFecha_Loaded;
            cmbSucursales.SelectionChanged += cmbSucursales_SelectionChanged;
            
        }

        private void cmbSucursales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
              
            }
            catch { }
        }

        private void frmRelacionContratosGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            txtDesde.SelectedDate = DateTime.Today;
            txtHasta.SelectedDate = DateTime.Today;

            if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                cmbSucursales.SelectedValue = -1;
            }
            else
            {
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
            }
            cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
          
        }

        void LoadCombox()
        {
            try
            {
                //DataContext = new clsSucursalesBE();
                List<clsSucursalesBE> List = new List<clsSucursalesBE>();
                List = db.SucursalesGet(null).ToList();

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.IsEnabled = true;
                    //List.Add(new clsSucursalesBE { SucursalID = -1, Sucursal = clsLenguajeBO.Find("itemAll") });
                }
                else
                {
                    cmbSucursales.IsEnabled = false;
                }                
                cmbSucursales.ItemsSource = List;
                cmbSucursales.SelectedValuePath = "SucursalID";
                cmbSucursales.DisplayMemberPath = "Sucursal";

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.SelectedValue = -1;
                }
                else
                {
                    cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
                }

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmPrintListadoLlamadasGetByFecha Informes = new frmPrintListadoLlamadasGetByFecha();
                Informes.OnInit((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, (int)cmbSucursales.SelectedValue);
                Informes.Owner = this;
                Informes.ShowDialog();

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void Limpiar()
        {
            clsValidacionesBO.Limpiar(gridCiudades);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


       
    }
}
