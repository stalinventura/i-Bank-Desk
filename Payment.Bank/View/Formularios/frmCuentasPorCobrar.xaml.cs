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
using Payment.Bank.Model;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmCuentasPorCobrar : MetroWindow
    {
        int VISTA = 1;
        clsCiudadesBE BE = new clsCiudadesBE();
        Core.Manager db = new Core.Manager();
        public frmCuentasPorCobrar() 
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
                    List.Add(new clsSucursalesBE { SucursalID = -1, Sucursal = clsLenguajeBO.Find("itemAll") });
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


                //Rutas
                List<clsRutasBE> Rutas = new List<clsRutasBE>();
                Rutas = db.RutasGet(null);
                Rutas.Add(new clsRutasBE { RutaID = -1, Ruta = clsLenguajeBO.Find("itemAll") });

                cmbRutas.ItemsSource = Rutas;
                cmbRutas.SelectedValuePath = "RutaID";
                cmbRutas.DisplayMemberPath = "Ruta";
                cmbRutas.SelectedValue = -1;


                List<clsOrderByBE> OrderBy = new List<clsOrderByBE>();
                OrderBy.Add(new clsOrderByBE { OrderID = 0, Order = clsLenguajeBO.Find("itemAll") });
                OrderBy.Add(new clsOrderByBE { OrderID = 1, Order = "Fecha" });
                OrderBy.Add(new clsOrderByBE { OrderID = 2, Order = "Cliente" });
                OrderBy.Add(new clsOrderByBE { OrderID = 3, Order = "Monto" });
                OrderBy.Add(new clsOrderByBE { OrderID = 4, Order = "Capital" });
                OrderBy.Add(new clsOrderByBE { OrderID = 5, Order = "Interes" });
                OrderBy.Add(new clsOrderByBE { OrderID = 6, Order = "Mora" });
                OrderBy.Add(new clsOrderByBE { OrderID = 7, Order = "Legal" });
                OrderBy.Add(new clsOrderByBE { OrderID = 8, Order = "Seguro" });

                cmbOrdenar.ItemsSource = OrderBy;
                cmbOrdenar.SelectedValuePath = "OrderID";
                cmbOrdenar.DisplayMemberPath = "Order";
                cmbOrdenar.SelectedValue = 0;

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string OrderBy = string.Empty;
                switch(cmbOrdenar.SelectedValue)
                { 
                    case 0: { OrderBy = "Order by Contratos.ContratoID"; }break;
                    case 1: { OrderBy = "Order by Contratos.Fecha"; } break;
                    case 2: { OrderBy = "Order by Personas.Nombres"; } break;
                    case 3: { OrderBy = "Order by Solicitudes.Monto"; } break;
                    case 4: { OrderBy = "Order by Capital"; } break;
                    case 5: { OrderBy = "Order by Interes"; } break;
                    case 6: { OrderBy = "Order by Mora"; } break;
                    case 7: { OrderBy = "Order by Legal"; } break;
                    case 8: { OrderBy = "Order by Seguro"; } break;
                    default:{ OrderBy = "Order by Contratos.ContratoID"; } break;
                }
                bool ShowResume = lblSummary.IsChecked == true ? true : false;
                frmPrintCuentasPorCobrar Informes = new frmPrintCuentasPorCobrar();
                Informes.OnInit((DateTime)txtHasta.SelectedDate, (int)cmbSucursales.SelectedValue, (int)cmbTipoSolicitudes.SelectedValue, (int)cmbRutas.SelectedValue, ShowResume, OrderBy);
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
