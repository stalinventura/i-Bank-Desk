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
    public partial class frmRelacionIngresosInmueblesGetByFecha : MetroWindow
    {
        //int VISTA = 1;
        //clsCiudadesBE BE = new clsCiudadesBE();
        Core.Manager db = new Core.Manager();
        public frmRelacionIngresosInmueblesGetByFecha() 
        {
            InitializeComponent();
            DataContext = new clsCiudadesBE();
            clsLenguajeBO.Load(gridCiudades);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmCiudades_Loaded;
            cmbSucursales.SelectionChanged += cmbSucursales_SelectionChanged;
        }




        private void frmCiudades_Loaded(object sender, RoutedEventArgs e)
        {
            txtDesde.SelectedDate = DateTime.Today;
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
            CargarUsuarios();
        }

        private void cmbSucursales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CargarUsuarios();
            }
            catch { }
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

                //Oficiales
                List<clsUsuariosBE> Usuarios = new List<clsUsuariosBE>();
                cmbCajeros.ItemsSource = Usuarios;
                Usuarios = db.UsuariosGet(null).ToList();

                Usuarios.Add(new clsUsuariosBE { Documento = "-1", Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemAll") } });

                List<clsUsuariosBE> ListOfUser = new List<clsUsuariosBE>();
                foreach (var row in Usuarios)
                {
                    ListOfUser.Add(new clsUsuariosBE { SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                }

                cmbCajeros.ItemsSource = ListOfUser;
                cmbCajeros.SelectedValuePath = "Documento";
                cmbCajeros.DisplayMemberPath = "Personas.Nombres";
                cmbCajeros.SelectedValue = -1;

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        void CargarUsuarios()
        {
            try
            {
                //Usuarios
                List<clsUsuariosBE> Usuarios = new List<clsUsuariosBE>();
                cmbCajeros.ItemsSource = Usuarios;

                Usuarios = clsVariablesBO.UsuariosBE.Roles.IsAdmin == false ? db.UsuariosGet(null).Where(x => x.SucursalID == (int)cmbSucursales.SelectedValue).ToList() : (int)cmbSucursales.SelectedValue == -1 || cmbSucursales.SelectedValue == null ? db.UsuariosGet(null).ToList() : db.UsuariosGet(null).Where(x => x.SucursalID == (int)cmbSucursales.SelectedValue).ToList();

                Usuarios.Add(new clsUsuariosBE { Documento = "-1", Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemAll") } });

                List<clsUsuariosBE> ListOfUser = new List<clsUsuariosBE>();
                foreach (var row in Usuarios)
                {
                    ListOfUser.Add(new clsUsuariosBE { SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                }

                cmbCajeros.ItemsSource = ListOfUser;
                cmbCajeros.SelectedValuePath = "Documento";
                cmbCajeros.DisplayMemberPath = "Personas.Nombres";
                cmbCajeros.SelectedValue = -1;
            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                     
                frmPrintRelacionIngresosInmueblesGetByFecha Informes = new frmPrintRelacionIngresosInmueblesGetByFecha();
                Informes.OnInit((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, (int)cmbSucursales.SelectedValue, cmbCajeros.SelectedValue.ToString());
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
