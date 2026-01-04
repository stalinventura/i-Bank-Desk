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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmEdificios : MetroWindow
    {
        int VISTA = 1;
        clsEdificiosBE BE = new clsEdificiosBE();
        Core.Manager db = new Core.Manager();
        public frmEdificios()
        {
            InitializeComponent();
            DataContext = new clsEdificiosBE();
            clsLenguajeBO.Load(gridEdificios);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmEdificios_Loaded;
        }

        private void frmEdificios_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            { cmbCiudades.SelectedValue = -1; }              
        }

        void LoadCombox()
        {
            try
            {
                DataContext = new clsEdificiosBE();
                List<clsCiudadesBE> List = new List<clsCiudadesBE>();
                List = db.CiudadesGet(null).ToList();
                List.Add(new clsCiudadesBE { CiudadID = -1, Ciudad = clsLenguajeBO.Find("itemSelect") });
                cmbCiudades.ItemsSource = List;
                cmbCiudades.SelectedValuePath = "CiudadID";
                cmbCiudades.DisplayMemberPath = "Ciudad";
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEdificios))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.EdificiosCreate(txtEdificio.Text, (int)cmbCiudades.SelectedValue, txtDireccion.Text, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridEdificios))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.EdificiosUpdate(BE.EdificioID, txtEdificio.Text, (int)cmbCiudades.SelectedValue, txtDireccion.Text, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridEdificios);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsEdificiosBE be, int _VISTA)
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
