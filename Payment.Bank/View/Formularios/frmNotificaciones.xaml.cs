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
    /// Interaction logic for frmSuspenderContratos.xaml
    /// </summary>
    public partial class frmNotificaciones : Window
    {
        clsContratosBE BE = new clsContratosBE();
        Manager db = new Manager();
        List<clsUsuariosBE> Source = new List<clsUsuariosBE>();

        public frmNotificaciones()
        {
            InitializeComponent();
            Loaded += frmPin_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            Title= clsLenguajeBO.Find(this.Title);
            clsLenguajeBO.Load(gridLogin);

            DataContext = new clsContratosBE();
           
        }



        private async void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsValidacionesBO.Validar(gridLogin) == true)
                {
                    if (Common.Generic.InternetAccess())
                    {
                        string[] list = { };
                        list = new string[Source.Count];
                        if (cmbUsuarios.SelectedValue.ToString() == "1")
                        {
                            int i = 0;
                            foreach(var p in Source)
                            {
                                list[i] = p.Documento;
                                i++;
                            }
                        }
                        else
                        {
                            list[0] = cmbUsuarios.SelectedValue.ToString();
                        }

                        var result = await db.NotificacionesSent(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa, txtRazon.Text, list, clsVariablesBO.UsuariosBE.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Personas.Apellidos);

                        if (result.ResponseCode == "00")
                        {
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgNotification"), clsLenguajeBO.Find("msgTitle"));
                        }
                        else
                        {
                            clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgInternetAccess"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
            }
            catch(Exception ex) { clsMessage.ErrorMessage(ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void frmPin_Loaded(object sender, RoutedEventArgs e)
        {
            txtRazon.Focus();
            LoadCombox();
        }

        void LoadCombox()
        {
            try
            {

                Source = db.UsuariosGet(null).Where(x=> x.EstadoID == true && x.Pin != null).ToList();
                Source.Add(new clsUsuariosBE { Documento = "1", Personas = new clsPersonasBE { Documento = "1", Nombres = clsLenguajeBO.Find("itemAll") } });
                cmbUsuarios.ItemsSource = Source.OrderBy(x => x.Personas.Nombres);
                cmbUsuarios.SelectedValuePath = "Documento";
                cmbUsuarios.DisplayMemberPath = "Personas.Nombres";
                cmbUsuarios.SelectedValue = 1;
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
