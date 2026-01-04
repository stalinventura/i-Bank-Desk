using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmRelacionEntradasGetByFecha.xaml
    /// </summary>
    public partial class frmRelacionEntradasGetByFecha : MetroWindow
    {
        //int VISTA = 1;
        //clsCiudadesBE BE = new clsCiudadesBE();
        Core.Manager db = new Core.Manager();
        public frmRelacionEntradasGetByFecha() 
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridCiudades);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmRelacionEntradasGetByFecha_Loaded;
        }




        private void frmRelacionEntradasGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            //txtDesde.SelectedDate = DateTime.Today;
            //txtHasta.SelectedDate = DateTime.Today;
            //if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            //{
            //    cmbSucursales.SelectedValue = -1;
            //}
            //else
            //{
            //    cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
            //}
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

        public void OnInit(DateTime _desde , DateTime _hasta)
        {
            txtDesde.SelectedDate = _desde;
            txtHasta.SelectedDate = _hasta;
        }
 
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool IsResumen = chkResumen.IsChecked == true ? true : false;
                frmPrintEntradasGetByFecha Informes = new frmPrintEntradasGetByFecha();
                Informes.OnInit((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, (int)cmbSucursales.SelectedValue, IsResumen);
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
