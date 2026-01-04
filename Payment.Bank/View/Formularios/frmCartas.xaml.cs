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
    public partial class frmCartas : MetroWindow
    {
        int VISTA = 1;
        clsCartasBE BE = new clsCartasBE();
        Core.Manager db = new Core.Manager();
        public frmCartas()
        {
            InitializeComponent();
            DataContext = new clsCartasBE();
            clsLenguajeBO.Load(gridCartas);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
       
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmCartas_Loaded;
        }

        private void frmCartas_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCombox();
            if (VISTA == 1)
            { cmbTipoCartas.SelectedValue = -1; }
        }

        void LoadCombox()
        {
            try
            {
              
                cmbTipoCartas.ItemsSource = new List<clsTipoCartasBE>();              

                List<clsTipoCartasBE> List = new List<clsTipoCartasBE>();

                List = db.TipoCartasGetNotTask(null);

                List.Add(new clsTipoCartasBE { TipoCartaID = -1, TipoCarta = clsLenguajeBO.Find("itemSelect") });

                if(VISTA==2)
                {
                    List.Add(new clsTipoCartasBE { TipoCartaID = BE.TipoCartaID, TipoCarta = BE.TipoCartas.TipoCarta });
                }

                cmbTipoCartas.ItemsSource = List.OrderBy(x => x.TipoCartaID);
                cmbTipoCartas.SelectedValuePath = "TipoCartaID";
                cmbTipoCartas.DisplayMemberPath = "TipoCarta";

                if (VISTA == 1)
                {
                    cmbTipoCartas.SelectedValue = -1;
                }
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridCartas))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.CartasCreate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbTipoCartas.SelectedValue, int.Parse(txtDesde.Text), int.Parse(txtHasta.Text), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridCartas))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.CartasUpdate(BE.CartaID, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, (int)cmbTipoCartas.SelectedValue, int.Parse(txtDesde.Text), int.Parse(txtHasta.Text), float.Parse(txtMonto.Text), clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridCartas);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsCartasBE be, int _VISTA)
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
