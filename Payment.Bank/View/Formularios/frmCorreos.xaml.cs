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
    public partial class frmCorreos : MetroWindow
    {
        int VISTA = 1;
        clsCorreosBE BE = new clsCorreosBE();
        Core.Manager db = new Core.Manager();
        public frmCorreos()
        {
            InitializeComponent();
            DataContext = new clsCorreosBE();
            clsLenguajeBO.Load(gridCorreos);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmCorreos_Loaded;
        }

        private void frmCorreos_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            { cmbServidores.SelectedValue = -1; }              
        }

        void LoadCombox()
        {
            try
            {
                DataContext = new clsCorreosBE();
                List<clsServidoresBE> List = new List<clsServidoresBE>();
                List = db.ServidoresGet(null);
                List.Add(new clsServidoresBE { ServidorID = -1, Descripcion = clsLenguajeBO.Find("itemSelect") });
                cmbServidores.ItemsSource = List.OrderBy(x => x.Smtp);
                cmbServidores.SelectedValuePath = "ServidorID";
                cmbServidores.DisplayMemberPath = "Descripcion";
                cmbServidores.SelectedValue = -1;
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridCorreos) && !string.IsNullOrEmpty(txtPassword.Password))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.CorreosCreate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, txtCorreo.Text, txtPassword.Password, (int)cmbServidores.SelectedValue, (bool)chkPredetermined.IsChecked, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridCorreos) && !string.IsNullOrEmpty(txtPassword.Password))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.CorreosUpdate(BE.CorreoID, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, txtCorreo.Text, txtPassword.Password, (int)cmbServidores.SelectedValue, (bool)chkPredetermined.IsChecked, clsVariablesBO.UsuariosBE.Documento);
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
            txtPassword.Password = string.Empty;
            clsValidacionesBO.Limpiar(gridCorreos);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsCorreosBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                    txtPassword.Password = Common.Generic.Decoding(BE.Contraseña);
                }

            }
            catch { }
        }     
    }
}
