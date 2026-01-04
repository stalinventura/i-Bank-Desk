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

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmZonas.xaml
    /// </summary>
    public partial class frmZonas : MetroWindow
    {
        int VISTA = 1;
        clsZonasBE BE = new clsZonasBE();
        Core.Manager db = new Core.Manager();
        public frmZonas()
        {
            InitializeComponent();
            //DataContext = new clsZonasBE() { RutaID =-1 };
            clsLenguajeBO.Load(gridModelos);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            btnAdd.Click += btnAdd_Click;
            Loaded += frmModelos_Loaded;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                frmRutas rutas = new frmRutas();
                rutas.Owner = this;
                rutas.Closed += (obj, arg) => 
                {
                    LoadCombox();
                };
                rutas.ShowDialog();
            }
            catch { }
        }

  

        private void frmModelos_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            { cmbRutas.SelectedValue = -1; }
            gridModelos.DataContext = new clsZonasBE() { RutaID = -1 };
        }

        void LoadCombox()
        {
            try
            {
                List<clsRutasBE> List = new List<clsRutasBE>();
                List = db.RutasGet(null).ToList();
                List.Add(new clsRutasBE { RutaID = -1, Ruta = clsLenguajeBO.Find("itemSelect") });
                cmbRutas.ItemsSource = List;
                cmbRutas.SelectedValuePath = "RutaID";
                cmbRutas.DisplayMemberPath = "Ruta";
                gridModelos.DataContext = new clsZonasBE() { RutaID = -1 };
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridModelos))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.ZonasCreate(txtModelo.Text, (int)cmbRutas.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                            Limpiar();
                        }
                        else
                        {
                            clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                        }                       
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridModelos))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.ZonasUpdate(BE.ZonaID, txtModelo.Text, (int)cmbRutas.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                                Limpiar();
                            }
                            else
                            {
                                clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            }
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                        }
                    }
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void Limpiar()
        {
            clsValidacionesBO.Limpiar(gridModelos);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsZonasBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                }

            }
            catch { }
        }     
    }
}
