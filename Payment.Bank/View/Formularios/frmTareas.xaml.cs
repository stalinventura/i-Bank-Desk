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
    public partial class frmTareas : MetroWindow
    {
        int VISTA = 1;
        clsTareasBE BE = new clsTareasBE();
        Core.Manager db = new Core.Manager();
        public frmTareas()
        {
            InitializeComponent();
            DataContext = new clsTareasBE();
            clsLenguajeBO.Load(gridTareas);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
       
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmTareas_Loaded;
        }

        private void frmTareas_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCombox();
            if (VISTA == 1)
            { cmbTipoTareas.SelectedValue = -1; }
        }

        void LoadCombox()
        {
            try
            {
              
                cmbTipoTareas.ItemsSource = new List<clsTipoTareasBE>();              

                List<clsTipoTareasBE> List = new List<clsTipoTareasBE>();

                List = db.TipoTareasGetNotTask(null);

                List.Add(new clsTipoTareasBE { TipoTareaID = -1, Descripcion = clsLenguajeBO.Find("itemSelect") });

                if(VISTA==2)
                {
                    List.Add(new clsTipoTareasBE { TipoTareaID = BE.TipoTareaID, Descripcion = BE.TipoTareas.Descripcion });
                }

                cmbTipoTareas.ItemsSource = List.OrderBy(x => x.Descripcion);
                cmbTipoTareas.SelectedValuePath = "TipoTareaID";
                cmbTipoTareas.DisplayMemberPath = "Descripcion";
                if (VISTA == 1)
                {
                    cmbTipoTareas.SelectedValue = -1;
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridTareas))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.TareasCreate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbTipoTareas.SelectedValue, int.Parse(txtDia.Text) , clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridTareas))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.TareasUpdate(BE.TareaID, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbTipoTareas.SelectedValue, int.Parse(txtDia.Text), clsVariablesBO.UsuariosBE.Documento);
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
            VISTA = 1;
            LoadCombox();
            clsValidacionesBO.Limpiar(gridTareas);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsTareasBE be, int _VISTA)
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
