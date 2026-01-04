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


using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.Core;
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmContratosVencidosGetByFecha : MetroWindow
    {
        int VISTA = 1;
        clsCiudadesBE BE = new clsCiudadesBE();
        Core.Manager db = new Core.Manager();
        public frmContratosVencidosGetByFecha() 
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridCiudades);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmCiudades_Loaded;
        }

        private void frmCiudades_Loaded(object sender, RoutedEventArgs e)
        {
            txtHasta.SelectedDate = DateTime.Today;
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
                DataContext = new clsSucursalesBE();
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

                //Tipo de Solicitudes
                List<clsTipoSolicitudesBE> TipoSolicitudes = new List<clsTipoSolicitudesBE>();
                TipoSolicitudes = db.TipoSolicitudesGet(null).ToList();
                TipoSolicitudes.Add(new clsTipoSolicitudesBE { TipoSolicitudID = -1, TipoSolicitud = clsLenguajeBO.Find("itemAll") });
                cmbTipoSolicitudes.ItemsSource = TipoSolicitudes;
                cmbTipoSolicitudes.SelectedValuePath = "TipoSolicitudID";
                cmbTipoSolicitudes.DisplayMemberPath = "TipoSolicitud";
                cmbTipoSolicitudes.SelectedValue = -1;


            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmPrintContratosVencidosGetByFecha Informes = new frmPrintContratosVencidosGetByFecha();
                Informes.OnInit((DateTime)txtHasta.SelectedDate.Value, (int)cmbSucursales.SelectedValue, (int)cmbTipoSolicitudes.SelectedValue, (bool)chkInteres.IsChecked);
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
