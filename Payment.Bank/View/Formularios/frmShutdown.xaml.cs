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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmShutdown : Window
    {
        Core.Manager db = new Core.Manager();
        public bool IsValid = false;
        public frmShutdown()
        {
            InitializeComponent();
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridDatosPersonales);
            Loaded += frmShutdown_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            if(clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            {
                spButton.Visibility = Visibility.Visible;
            }
            else
            {
                spButton.Visibility = Visibility.Hidden;
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try {
                bool EstadoID = Environment.MachineName.ToUpper() == "SERVIDOR-PC" ? true : false;
                var result = db.DispositivosCreate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey(), Environment.MachineName, EstadoID, clsVariablesBO.UsuariosBE.Documento);
                if(result.ResponseCode == "00")
                {
                    IsValid = true;
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    Close();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgNotHaveLicence"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch(Exception ex) {
                clsMessage.ErrorMessage(ex.Message, clsLenguajeBO.Find("msgTitle"));
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown(0);
        }

        private void frmShutdown_Loaded(object sender, RoutedEventArgs e)
        {
            lblMessage.Text = clsLenguajeBO.Find("lblUnlicensed");
            lblKey.Text =clsCookiesBO.GetKey();

            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            int cont = 0;
            lblContador.Text = (60).ToString();
            Timer.Tick += (s, a) =>
            {               
                cont++;
                 lblContador.Text = (60 - cont).ToString();
                if (cont == 61)
                {
                    App.Current.Shutdown(0);
                }
            };

            Timer.Start();
        }

        }
    }