using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
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
using System.Windows.Shapes;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmBlackList.xaml
    /// </summary>
    public partial class frmBlackList : Window
    {
        clsContratosBE BE = new clsContratosBE();
        Manager db = new Manager();

        public frmBlackList()
        {
            InitializeComponent();
            Loaded += frmPin_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            Title= clsLenguajeBO.Find(this.Title);
            clsLenguajeBO.Load(gridLogin);

            DataContext = new clsContratosBE();
        }


        public void OnInit(clsContratosBE _be)
        {
            BE = _be;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsValidacionesBO.Validar(gridLogin) == true)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        OperationResult result = db.ListaNegrasCreate(BE.Solicitudes.Clientes.Documento, txtRazon.Text, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                            this.Close();
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
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void frmPin_Loaded(object sender, RoutedEventArgs e)
        {
            txtRazon.Focus();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
