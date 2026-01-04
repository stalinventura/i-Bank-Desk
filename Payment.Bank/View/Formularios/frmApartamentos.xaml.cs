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
    public partial class frmApartamentos : MetroWindow
    {
        int VISTA = 1;
        clsApartamentosBE BE = new clsApartamentosBE();
        Core.Manager db = new Core.Manager();
        public frmApartamentos()
        {
            InitializeComponent();
            DataContext = new clsApartamentosBE();
            clsLenguajeBO.Load(gridApartamentos);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            LoadCombox();
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmApartamentos_Loaded;
        }

        private void frmApartamentos_Loaded(object sender, RoutedEventArgs e)
        {
         if(VISTA==1)
            { cmbEdificios.SelectedValue = -1; cmbPisos.SelectedValue = -1; }              
        }

        void LoadCombox()
        {
            try
            {
                DataContext = new clsApartamentosBE();

                List<clsTipoApartamentosBE> TipoApartamentos = new List<clsTipoApartamentosBE>();
                TipoApartamentos = db.TipoApartamentosGet(null).ToList();
                TipoApartamentos.Add(new clsTipoApartamentosBE { TipoApartamentoID = -1, TipoApartamento = clsLenguajeBO.Find("itemSelect") });
                cmbTipoApartamentos.ItemsSource = TipoApartamentos;
                cmbTipoApartamentos.SelectedValuePath = "TipoApartamentoID";
                cmbTipoApartamentos.DisplayMemberPath = "TipoApartamento";
                cmbTipoApartamentos.SelectedValue = -1;

                List<clsEdificiosBE> Edificios = new List<clsEdificiosBE>();
                Edificios = db.EdificiosGet(null).ToList();
                Edificios.Add(new clsEdificiosBE { EdificioID = -1, Edificio = clsLenguajeBO.Find("itemSelect") });
                cmbEdificios.ItemsSource = Edificios;
                cmbEdificios.SelectedValuePath = "EdificioID";
                cmbEdificios.DisplayMemberPath = "Edificio";
                cmbEdificios.SelectedValue =- 1;

                List<clsPisosBE> Pisos = new List<clsPisosBE>();
                Pisos = db.PisosGet(null).ToList();
                Pisos.Add(new clsPisosBE { PisoID = -1, Piso = clsLenguajeBO.Find("itemSelect") });
                cmbPisos.ItemsSource = Pisos;
                cmbPisos.SelectedValuePath = "PisoID";
                cmbPisos.DisplayMemberPath = "Piso";
                cmbPisos.SelectedValue = -1;
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridApartamentos))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.ApartamentosCreate(txtApartamento.Text, (int)cmbEdificios.SelectedValue, (int)cmbPisos.SelectedValue, txtDescripcion.Text, (int)cmbTipoApartamentos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridApartamentos))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.ApartamentosUpdate(BE.ApartamentoID, txtApartamento.Text, (int)cmbEdificios.SelectedValue, (int)cmbPisos.SelectedValue, txtDescripcion.Text, (int)cmbTipoApartamentos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridApartamentos);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsApartamentosBE be, int _VISTA)
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
