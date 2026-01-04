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
    public partial class frmBancos : MetroWindow
    {
        int VISTA = 1;
        clsBancosBE BE = new clsBancosBE();
        Core.Manager db = new Core.Manager();
        public frmBancos()
        {
            InitializeComponent();
            DataContext = new clsBancosBE();
            clsLenguajeBO.Load(gridBancos);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmBancos_Loaded;
        }

        private void frmBancos_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            {
                cmbAuxiliares.SelectedValue = -1;
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
            }              
        }

        void LoadCombox()
        {
            try
            {
                DataContext = new clsBancosBE();
                List<clsAuxiliaresBE> List = new List<clsAuxiliaresBE>();
                foreach (var item in db.AuxiliaresGet(null).ToList())
                {
                    List.Add(new clsAuxiliaresBE { AuxiliarID = item.AuxiliarID, Fecha = item.Fecha, Auxiliar = string.Format("{0} {1}", item.Codigo, item.Auxiliar), Codigo = item.Codigo });
                }
                List.Add(new clsAuxiliaresBE { AuxiliarID = -1, Auxiliar = clsLenguajeBO.Find("itemSelect") });
                cmbAuxiliares.ItemsSource = List;
                cmbAuxiliares.SelectedValuePath = "AuxiliarID";
                cmbAuxiliares.DisplayMemberPath = "Auxiliar";


                List<clsSucursalesBE> Sucursales = new List<clsSucursalesBE>();
                Sucursales = db.SucursalesGet(null).ToList();

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.IsEnabled = true;
                }
                else
                {
                    cmbSucursales.IsEnabled = false;                    
                }
                cmbSucursales.ItemsSource = Sucursales;
                cmbSucursales.SelectedValuePath = "SucursalID";
                cmbSucursales.DisplayMemberPath = "Sucursal";
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridBancos))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.BancosCreate( txtBanco.Text, (int)cmbAuxiliares.SelectedValue, (int)cmbSucursales.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridBancos))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.BancosUpdate(BE.BancoID, txtBanco.Text, (int)cmbAuxiliares.SelectedValue, (int)cmbSucursales.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridBancos);
            cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsBancosBE be, int _VISTA)
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
