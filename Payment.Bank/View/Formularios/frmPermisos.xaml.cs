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
using System.Data;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmPermisos.xaml
    /// </summary>
    public partial class frmPermisos : MetroWindow
    {
        int RolID;
        List<clsPermisosRolesBE> permisosRolesBE = new List<clsPermisosRolesBE>();
        List<clsPermisosBE> permisosBE = new List<clsPermisosBE>();
        clsPermisosBE P = new clsPermisosBE();
        clsPermisosRolesBE R = new clsPermisosRolesBE();
        Core.Manager db = new Core.Manager();



        public frmPermisos()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridPermisos);
            clsLenguajeBO.Load(gridPermisosRoles);
            clsLenguajeBO.Load(LayoutRoot);


            Title = clsLenguajeBO.Find(Title.ToString());


            btnSalir.Click += btnSalir_Click;
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
            dataGrid2.SelectionChanged += dataGrid2_SelectionChanged;
            btnAceptar.Click += btnAceptar_Click; ;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    db = new Manager();
                    foreach (var row in dataGrid2.Items)
                    {

                        int PermisoID = (row as clsPermisosRolesBE).PermisoID;

                        var agregar = dataGrid2.Columns[1].GetCellContent(row).GetChildObjects(true).FirstOrDefault();
                        bool add = (bool)(agregar as CheckBox).IsChecked;

                        var modificar = dataGrid2.Columns[2].GetCellContent(row).GetChildObjects(true).FirstOrDefault();
                        bool edit = (bool)(modificar as CheckBox).IsChecked;

                        var eliminar = dataGrid2.Columns[3].GetCellContent(row).GetChildObjects(true).FirstOrDefault();
                        bool del = (bool)(eliminar as CheckBox).IsChecked;

                        var imprimir = dataGrid2.Columns[4].GetCellContent(row).GetChildObjects(true).FirstOrDefault();
                        bool print = (bool)(imprimir as CheckBox).IsChecked;

                        var result = db.PermisosRolesUpdate(RolID, PermisoID, add, edit, del, print, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode != "00")
                        {
                            clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            return;
                        }
                    }

                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    LoadData();
                }
                else
                {

                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch (Exception ex)
            { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (R.PermisoID > 0)
                {
                    var result = db.PermisosRolesDelete(RolID, R.PermisoID);
                    if (result.ResponseCode == "00")
                    {
                        LoadData();
                    }

                }
            }
            catch (Exception ex)
            { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P = (clsPermisosBE)dataGrid1.SelectedItem;
            }
            catch { }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                R = (clsPermisosRolesBE)dataGrid2.SelectedItem;
            }
            catch { }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (P.PermisoID > 0)
                {
                    var result = db.PermisosRolesCreate(RolID, P.PermisoID, true, true, true, true, clsVariablesBO.UsuariosBE.Documento);
                    if (result.ResponseCode == "00")
                    {
                        //clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                        LoadData();
                    }

                }
            }
            catch (Exception ex)
            { }
        }



        private void Limpiar()
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in this.gridPermisosRoles.Children)
                {
                    if (Obj is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        ctl.Text = String.Empty;
                    }
                }
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(int _rolID)
        {
            try
            {
                RolID = _rolID;
                LoadData();

            }
            catch { }
        }


        public void LoadData()
        {
            try
            {
                permisosBE = db.PermisosGetNotInRolID(RolID);
                dataGrid1.ItemsSource = permisosBE;
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", permisosBE.Count.ToString());

                permisosRolesBE = db.PermisosRolesGetByRolID(RolID);
                dataGrid2.ItemsSource = permisosRolesBE;
                lblTotal1.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", permisosRolesBE.Count.ToString());
            }
            catch { }
        }


    }
}
