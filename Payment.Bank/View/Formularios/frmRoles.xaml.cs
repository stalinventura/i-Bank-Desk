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
    /// Interaction logic for frmRoles.xaml
    /// </summary>
    public partial class frmRoles : MetroWindow
    {
        int VISTA = 1;
        clsRolesBE BE = new clsRolesBE();
        Core.Manager db = new Core.Manager();
        public frmRoles()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridRoles);
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridIsAdmin);
            clsLenguajeBO.Load(gridCanQuery);
            clsLenguajeBO.Load(gridViewAllApplications);
            clsLenguajeBO.Load(gridViewAllAccountting);
            clsLenguajeBO.Load(gridViewAllApplicationCheck);
            clsLenguajeBO.Load(gridViewAllBank);
            clsLenguajeBO.Load(gridViewAllCheck);
            clsLenguajeBO.Load(gridViewAllContract);
            clsLenguajeBO.Load(gridViewAllExpenditure);
            clsLenguajeBO.Load(gridViewAllReceipt);


            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            DataContext = new clsRolesBE();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool IsAdmin = rbIsAdmin_Si.IsChecked == true ? true : false;
                bool CanQuery = rbCanQuery_Si.IsChecked == true ? true : false;
                bool ViewAllAccountting = rbViewAllAccountting_Si.IsChecked == true ? true : false;
                bool ViewAllApplication = rbViewAllApplications_Si.IsChecked == true ? true : false;
                bool ViewAllApplicationCheck = rbViewAllApplicationCheck_Si.IsChecked == true ? true : false;
                bool ViewAllBank = rbViewAllBank_Si.IsChecked == true ? true : false;
                bool ViewAllCheck = rbViewAllCheck_Si.IsChecked == true ? true : false;
                bool ViewAllContract = rbViewAllContract_Si.IsChecked == true ? true : false;
                bool ViewAllExpenditure = rbViewAllExpenditure_Si.IsChecked == true ? true : false;
                bool ViewAllReceipt = rbViewAllReceipt_Si.IsChecked == true ? true : false;

                if (VISTA == 1 && clsValidacionesBO.Validar(gridRoles))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.RolesCreate(txtRol.Text, txtDescripcion.Text, IsAdmin, CanQuery, ViewAllApplication, ViewAllContract, ViewAllReceipt, ViewAllBank, ViewAllCheck, ViewAllApplicationCheck, ViewAllAccountting, ViewAllExpenditure, 1,  clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridRoles))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.RolesUpdate(BE.RolID, txtRol.Text, txtDescripcion.Text, IsAdmin, CanQuery, ViewAllApplication, ViewAllContract, ViewAllReceipt, ViewAllBank, ViewAllCheck, ViewAllApplicationCheck, ViewAllAccountting, ViewAllExpenditure, 1, clsVariablesBO.UsuariosBE.Documento);
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
            try
            {
                foreach (System.Windows.Controls.Control Obj in this.gridRoles.Children)
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


        public void OnInit(clsRolesBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                    if (BE.IsAdmin) { rbIsAdmin_Si.IsChecked = true; } else { rbIsAdmin_No.IsChecked = true; }
                    if (BE.CanQuery) { rbCanQuery_Si.IsChecked = true; } else { rbCanQuery_No.IsChecked = true; }
                    if (BE.ViewAllAccountting) { rbViewAllAccountting_Si.IsChecked = true; } else { rbViewAllAccountting_No.IsChecked = true; }
                    if (BE.ViewAllApplication) { rbViewAllApplications_Si.IsChecked = true; } else { rbViewAllApplications_No.IsChecked = true; }
                    if (BE.ViewAllApplicationCheck) { rbViewAllApplicationCheck_Si.IsChecked = true; } else { rbViewAllApplicationCheck_No.IsChecked = true; }
                    if (BE.ViewAllBank) { rbViewAllBank_Si.IsChecked = true; } else { rbViewAllBank_No.IsChecked = true; }
                    if (BE.ViewAllCheck) { rbViewAllCheck_Si.IsChecked = true; } else { rbViewAllCheck_No.IsChecked = true; }
                    if (BE.ViewAllContract) { rbViewAllContract_Si.IsChecked = true; } else { rbViewAllContract_No.IsChecked = true; }
                    if (BE.ViewAllExpenditure) { rbViewAllExpenditure_Si.IsChecked = true; } else { rbViewAllExpenditure_No.IsChecked = true; }
                    if (BE.ViewAllReceipt) { rbViewAllReceipt_Si.IsChecked = true; } else { rbViewAllReceipt_No.IsChecked = true; }
                   
                }

            }
            catch { }
        }     
    }
}
