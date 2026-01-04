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
    public partial class frmTipoContratos : MetroWindow
    {
        int VISTA = 1;
        clsTipoContratosBE BE = new clsTipoContratosBE();
        Core.Manager db = new Core.Manager();
        public frmTipoContratos()
        {
            InitializeComponent();
            DataContext = new clsTipoContratosBE();
            clsLenguajeBO.Load(gridTipoContratos);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmTipoContratos_Loaded;
        }

        private void frmTipoContratos_Loaded(object sender, RoutedEventArgs e)
        {
             
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ContinueOnTime = rbContinueOnTime_Si.IsChecked == true ? true : false;
                bool InteresDiario = rbInteresDiario_Si.IsChecked == true ? true : false;
                bool MoraDiaria = rbMoraDiaria_Si.IsChecked == true ? true : false;
                bool CanMinimumPay = rbCanMinimumPay_Si.IsChecked == true ? true : false;

                if (VISTA == 1 && clsValidacionesBO.Validar(gridTipoContratos))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.TipoContratosCreate(txtTipoContrato.Text, (bool)chkPredetermined.IsChecked, ContinueOnTime, InteresDiario, MoraDiaria, CanMinimumPay, clsVariablesBO.UsuariosBE.Documento);
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridTipoContratos))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.TipoContratosUpdate(BE.TipoContratoID, txtTipoContrato.Text, (bool)chkPredetermined.IsChecked, ContinueOnTime, InteresDiario, MoraDiaria, CanMinimumPay, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridTipoContratos);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsTipoContratosBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                    if (be.InteresDiario == true)
                    { rbInteresDiario_Si.IsChecked = true; }
                    else { rbInteresDiario_No.IsChecked = true; }

                    if (be.MoraDiaria == true)
                    { rbMoraDiaria_Si.IsChecked = true; }
                    else { rbMoraDiaria_No.IsChecked = true; }

                    if (be.ContinueOnTime == true)
                    { rbContinueOnTime_Si.IsChecked = true; }
                    else { rbContinueOnTime_No.IsChecked = true; }

                    if (be.CanMinimumPay == true)
                    { rbCanMinimumPay_Si.IsChecked = true; }
                    else { rbCanMinimumPay_No.IsChecked = true; }
                }

            }
            catch { }
        }     
    }
}
