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
    public partial class frmTipoSolicitudes : MetroWindow
    {
        int VISTA = 1;
        clsTipoSolicitudesBE BE = new clsTipoSolicitudesBE();
        Core.Manager db = new Core.Manager();
        public frmTipoSolicitudes()
        {
            InitializeComponent();
            DataContext = new clsTipoSolicitudesBE();
            clsLenguajeBO.Load(gridTipoSolicitudes);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmTipoSolicitudes_Loaded;
      
        }

        private void frmTipoSolicitudes_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            { cmbAuxiliares.SelectedValue = -1; }              
        }

        void LoadCombox()
        {
            try
            {
                DataContext = new clsTipoSolicitudesBE();
                List<clsAuxiliaresBE> List = new List<clsAuxiliaresBE>();
                foreach(clsAuxiliaresBE BE in db.AuxiliaresGet(null).ToList() )
                {
                    List.Add(new clsAuxiliaresBE { AuxiliarID = BE.AuxiliarID, Codigo = BE.Codigo, Auxiliar = BE.Codigo + " " + BE.Auxiliar });
                }
                List.Add(new clsAuxiliaresBE { AuxiliarID = -1, Auxiliar = clsLenguajeBO.Find("itemSelect") });
                cmbAuxiliares.ItemsSource = List.OrderBy(x => x.Codigo);
                cmbAuxiliares.SelectedValuePath = "AuxiliarID";
                cmbAuxiliares.DisplayMemberPath = "Auxiliar";
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridTipoSolicitudes))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.TipoSolicitudesCreate(txtTipoSolicitud.Text, (int)cmbAuxiliares.SelectedValue, (bool)chkPredetermined.IsChecked, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridTipoSolicitudes))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.TipoSolicitudesUpdate(BE.TipoSolicitudID, txtTipoSolicitud.Text, (int)cmbAuxiliares.SelectedValue, (bool)chkPredetermined.IsChecked, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridTipoSolicitudes);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsTipoSolicitudesBE be, int _VISTA)
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
