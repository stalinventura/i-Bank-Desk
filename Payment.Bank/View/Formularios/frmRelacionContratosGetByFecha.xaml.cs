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
    /// Interaction logic for frmRelacionContratosGetByFecha.xaml
    /// </summary>
    public partial class frmRelacionContratosGetByFecha : MetroWindow
    {
        int VISTA = 1;
        //clsCiudadesBE BE = new clsCiudadesBE();
        Core.Manager db = new Core.Manager();
        public frmRelacionContratosGetByFecha() 
        {
            InitializeComponent();
            clsLenguajeBO.Load(gridCiudades);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
           
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmRelacionContratosGetByFecha_Loaded;
            cmbSucursales.SelectionChanged += cmbSucursales_SelectionChanged;
            
        }

        private void cmbSucursales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                CargarOficiales();
            }
            catch { }
        }

        private void frmRelacionContratosGetByFecha_Loaded(object sender, RoutedEventArgs e)
        {
            txtDesde.SelectedDate = DateTime.Today;
            txtHasta.SelectedDate = DateTime.Today;
            LoadCombox();
            //if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            //{
            //    cmbSucursales.SelectedValue = -1;
            //}
            //else
            //{
            //    cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
            //}
            cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
            //CargarOficiales();
            cmbObjetivos.SelectedValue = -1;
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
                    List.Add(new clsSucursalesBE { SucursalID = -1, Sucursal = clsLenguajeBO.Find("itemAll") });
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
                List<clsOficialesBE> Oficiales = new List<clsOficialesBE>();
                cmbOficiales.ItemsSource = Oficiales;
                Oficiales = db.OficialesGet(null).ToList();

                Oficiales.Add(new clsOficialesBE { OficialID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemAll") } });

                List<clsOficialesBE> ListOfAgent = new List<clsOficialesBE>();
                foreach (var row in Oficiales)
                {
                    ListOfAgent.Add(new clsOficialesBE { OficialID = row.OficialID, SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                }

                cmbOficiales.ItemsSource = ListOfAgent;
                cmbOficiales.SelectedValuePath = "OficialID";
                cmbOficiales.DisplayMemberPath = "Personas.Nombres";
                cmbOficiales.SelectedValue = -1;

                //Rutas
                List<clsRutasBE> Rutas = new List<clsRutasBE>();
                Rutas = db.RutasGet(null);
                Rutas.Add(new clsRutasBE { RutaID = -1, Ruta = clsLenguajeBO.Find("itemAll") });

                cmbRutas.ItemsSource = Rutas;
                cmbRutas.SelectedValuePath = "RutaID";
                cmbRutas.DisplayMemberPath = "Ruta";
                cmbRutas.SelectedValue = -1;

                //Objetivos
                List<clsObjetivosBE> Objetivos = new List<clsObjetivosBE>();
                Objetivos = db.ObjetivosGet(null);
                Objetivos.Add(new clsObjetivosBE { ObjetivoID = -1, Objetivo = clsLenguajeBO.Find("itemAll") });
               

                cmbObjetivos.ItemsSource = Objetivos;
                cmbObjetivos.SelectedValuePath = "ObjetivoID";
                cmbObjetivos.DisplayMemberPath = "Objetivo";
                cmbObjetivos.SelectedValue = -1;

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

       void CargarOficiales()
        {
            try
            {
                //Oficiales
                List<clsOficialesBE> Oficiales = new List<clsOficialesBE>();
                cmbOficiales.ItemsSource = Oficiales;

                Oficiales = clsVariablesBO.UsuariosBE.Roles.IsAdmin == false ? db.OficialesGet(null).Where(x => x.SucursalID == (int)cmbSucursales.SelectedValue).ToList() : (int)cmbSucursales.SelectedValue == -1 || cmbSucursales.SelectedValue == null ? db.OficialesGet(null).ToList() : db.OficialesGet(null).Where(x => x.SucursalID == (int)cmbSucursales.SelectedValue).ToList();

                Oficiales.Add(new clsOficialesBE { OficialID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemAll") } });

                List<clsOficialesBE> ListOfAgent = new List<clsOficialesBE>();
                foreach (var row in Oficiales)
                {
                    ListOfAgent.Add(new clsOficialesBE { OficialID = row.OficialID, SucursalID = row.SucursalID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos } });
                }

                cmbOficiales.ItemsSource = ListOfAgent;
                cmbOficiales.SelectedValuePath = "OficialID";
                cmbOficiales.DisplayMemberPath = "Personas.Nombres";
                cmbOficiales.SelectedValue = -1;
            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmPrintRelacionContratosGetByFecha Informes = new frmPrintRelacionContratosGetByFecha();
                Informes.OnInit((DateTime)txtDesde.SelectedDate, (DateTime)txtHasta.SelectedDate, (int)cmbSucursales.SelectedValue, (int)cmbOficiales.SelectedValue, (int)cmbRutas.SelectedValue, (int)cmbObjetivos.SelectedValue);
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
