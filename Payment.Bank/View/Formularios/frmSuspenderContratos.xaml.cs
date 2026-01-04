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
    public partial class frmSuspenderContratos : Window
    {
        clsContratosBE BE = new clsContratosBE();
        Manager db = new Manager();
        public bool PermissionAccess = false;

        public frmSuspenderContratos()
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
                    if (clsVariablesBO.Permiso.Eliminar == true || PermissionAccess)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.Unloaded += (obj, args) => {
                            try
                            {
                                var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/CancelarContratos.html");
                                string Body = string.Format(message, BE.Solicitudes.Clientes.Sucursales.Empresas.Empresa, BE.Solicitudes.Clientes.Sucursales.Direccion, BE.Solicitudes.Clientes.Sucursales.Telefonos, BE.Solicitudes.Clientes.Sucursales.Empresas.Rnc, BE.Solicitudes.Clientes.Sucursales.Gerentes.Personas.Nombres + " " + BE.Solicitudes.Clientes.Sucursales.Gerentes.Personas.Apellidos, clsVariablesBO.UsuariosBE.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Personas.Apellidos, clsVariablesBO.UsuariosBE.Documento, BE.ContratoID, txtRazon.Text.ToUpper(), BE.Solicitudes.Clientes.Sucursales.Empresas.Empresa, BE.Solicitudes.Clientes.Sucursales.Direccion, BE.Solicitudes.Clientes.Sucursales.Telefonos, BE.Solicitudes.Clientes.Sucursales.Empresas.Rnc, DateTime.Now.Year, BE.Solicitudes.Clientes.Sucursales.Empresas.Empresa, BE.Solicitudes.Clientes.Sucursales.Empresas.Site);

                                Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                ms.SendMail(BE.Solicitudes.Clientes.Sucursales.Gerentes.Personas.Correo, string.Format("Alerta Contrato #{0} | {1}", BE.ContratoID, BE.Solicitudes.Clientes.Sucursales.Empresas.Empresa), Body);
                            }
                            catch { }
                        };

                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.ContratosDeleteGetByContratoID(BE.ContratoID, 3, txtRazon.Text, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgDelete"), clsLenguajeBO.Find("msgTitle"));
                            }
                            else
                            {
                                clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            }
                            Mensaje.Close();
                            this.Close();
                        };

                        Mensaje.btnSalir.Click += (obj, args) =>
                        {
                            Mensaje.Close();
                        };
                        Mensaje.Owner = this;
                        Mensaje.ShowDialog();
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
            }
            catch {
            }
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
