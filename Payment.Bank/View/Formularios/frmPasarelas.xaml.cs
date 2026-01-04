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
    /// Interaction logic for frmPasarelas.xaml
    /// </summary>
    public partial class frmPasarelas : MetroWindow
    {
        int VISTA = 1;
        clsPasarelasBE BE = new clsPasarelasBE();
        Core.Manager db = new Core.Manager();
        public frmPasarelas()
        {
            InitializeComponent();
            DataContext = new clsPasarelasBE() {  ProcesadorID = -1 };
            clsLenguajeBO.Load(gridGateway);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmPasarelas_Loaded;
        }

        private void frmPasarelas_Loaded(object sender, RoutedEventArgs e)
        {  
            LoadCombox();
        }

        void LoadCombox()
        {
            try
            {
                //DataContext = new clsProcesadoresBE();
                List<clsProcesadoresBE> List = new List<clsProcesadoresBE>();
                List = db.ProcesadoresGet(null);
                List.Add(new clsProcesadoresBE { ProcesadorID = -1, Procesador = clsLenguajeBO.Find("itemSelect") });
                cmbProcesadores.ItemsSource = List;
                cmbProcesadores.SelectedValuePath = "ProcesadorID";
                cmbProcesadores.DisplayMemberPath = "Procesador";
                //cmbProcesadores.SelectedValue = -1;
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridGateway))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.PasarelasCreate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbProcesadores.SelectedValue, txtMerchantID.Text, txtKeyID.Text, txtSecretKey.Text , (bool)chkPredetermined.IsChecked, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridGateway))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.PasarelasUpdate(BE.PasarelaID, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbProcesadores.SelectedValue, txtMerchantID.Text, txtKeyID.Text, txtSecretKey.Text, (bool)chkPredetermined.IsChecked, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridGateway);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsPasarelasBE be, int _VISTA)
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
