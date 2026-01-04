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
using System.ServiceModel;
using Payment.Bank.Controles;
using Microsoft.Win32;
using System.IO;
using System.Web;
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes;
using Payment.Bank.Properties;
using System.Threading;
using System.Windows.Threading;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmDisclaimer.xaml
    /// </summary>
    public partial class frmDisclaimer : Window
    {
        Core.Manager db = new Core.Manager();
        int SucursalID = -1;
        DateTime Hasta = DateTime.Now;
        bool HasNewFormat = false;
        public frmDisclaimer()
        {
            InitializeComponent();
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridDatosPersonales);
            Loaded += frmShutdown_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            lblDecline.Checked += rbNo_Checked;
            lblAcept.Checked += rbSi_Checked;
        }

        private void rbSi_Checked(object sender, RoutedEventArgs e)
        {
            //btnAceptar.Visibility = Visibility.Visible;
            HasNewFormat = true;
        }

        private void rbNo_Checked(object sender, RoutedEventArgs e)
        {
            //btnAceptar.Visibility = Visibility.Collapsed;
            HasNewFormat = false;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try {

                clsDataCreditoBO DC = new clsDataCreditoBO();
                if (HasNewFormat)
                {
                    DC.EquifaxGetByFechaModificado(Hasta, SucursalID);
                }
                else
                {
                    DC.EquifaxGetByFecha(Hasta, SucursalID);
                }


            }
            catch { }
        }

        public void OnInit(DateTime _hasta, int _SucursalID)
        {
            Hasta = _hasta;
            SucursalID = _SucursalID;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmShutdown_Loaded(object sender, RoutedEventArgs e)
        {
            btnAceptar.Visibility = Visibility.Visible;
            lblMessage.Text =clsLenguajeBO.Find("lblDisclaimer");
        }


    }
    }