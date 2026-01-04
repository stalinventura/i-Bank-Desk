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
    public partial class frmAviso : Window
    {
        Core.Manager db = new Core.Manager();
        public bool IsValid = false;
        public int dias=0;
        public frmAviso()
        {
            InitializeComponent();
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridDatosPersonales);
            Loaded += frmShutdown_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;

            //if(clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
            //{
            //spButton.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    spButton.Visibility = Visibility.Hidden;
            //}

          
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try {

                if (!string.IsNullOrEmpty(lblKey.Text))
                {
                    var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/DepositosBancarios.html");
                    string Body = string.Format(message, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, clsVariablesBO.UsuariosBE.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Personas.Apellidos, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, lblKey.Text, clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Direccion, clsVariablesBO.UsuariosBE.Sucursales.Telefonos, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc, DateTime.Now.Year, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, clsVariablesBO.UsuariosBE.Sucursales.Empresas.Site);

                    Core.Common.clsMisc ms = new Core.Common.clsMisc();
                    var result = ms.SendMail("stalin_ventura@outlook.com", string.Format("Depósito licencia #{0}", clsVariablesBO.UsuariosBE.Sucursales.EmpresaID), Body);

                    if (result == "SUCCESS")
                    {
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgSendMailDeposit"), clsLenguajeBO.Find("msgTitle"));
                        if (dias == 0)
                        {
                            App.Current.Shutdown(0);
                        }
                        else
                        {
                            Close();
                        }

                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgErrorSendMail"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
           }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            if (dias == 0)
            {
                App.Current.Shutdown(0);
            }
            else
            {
                Close();
            }
        }

        private void frmShutdown_Loaded(object sender, RoutedEventArgs e)
        {
            btnAceptar.Visibility = Visibility.Visible;
            lblMessage.Text = string.Format(clsLenguajeBO.Find("lblUnlicensedService"), dias);
            //lblKey.Text =clsCookiesBO.GetKey();

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
                    if (dias == 0)
                    {
                        App.Current.Shutdown(0);
                    }
                    else
                    {
                        Close();
                    }
                }
            };

            Timer.Start();
        }


    }
    }