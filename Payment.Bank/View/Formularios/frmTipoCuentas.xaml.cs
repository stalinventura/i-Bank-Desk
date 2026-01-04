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
    public partial class frmTipoCuentas : MetroWindow
    {
        int VISTA = 1;
        clsTipoCuentasBE BE = new clsTipoCuentasBE();
        Core.Manager db = new Core.Manager();
        public frmTipoCuentas()
        {
            InitializeComponent();
            DataContext = new clsTipoCuentasBE();
            clsLenguajeBO.Load(gridTipoCuentas);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmTipoCuentas_Loaded;
        }

        private void frmTipoCuentas_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            { cmbMonedas.SelectedValue = -1; }              
        }

        void LoadCombox()
        {
            try
            {
                DataContext = new clsTipoCuentasBE();
                List<clsMonedasBE> List = new List<clsMonedasBE>();
                foreach(clsMonedasBE BE in db.MonedasGet(null).ToList() )
                {
                    List.Add(new clsMonedasBE { MonedaID = BE.MonedaID, Codigo = BE.Codigo, Moneda = BE.Codigo + " " + BE.Moneda });
                }
                List.Add(new clsMonedasBE { MonedaID = -1, Moneda = clsLenguajeBO.Find("itemSelect") });
                cmbMonedas.ItemsSource = List.OrderBy(x => x.Codigo);
                cmbMonedas.SelectedValuePath = "MonedaID";
                cmbMonedas.DisplayMemberPath = "Moneda";
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridTipoCuentas))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.TipoCuentasCreate(txtDescripcion.Text, (int)cmbMonedas.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridTipoCuentas))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.TipoCuentasUpdate(BE.TipoCuentaID, txtDescripcion.Text, (int)cmbMonedas.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridTipoCuentas);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsTipoCuentasBE be, int _VISTA)
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
