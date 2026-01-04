using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using System;
using System.Linq;
using System.Windows;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmNominasDelete.xaml
    /// </summary>
    public partial class frmNominasDelete : Window
    {
        clsNominasBE BE = new clsNominasBE();
        Manager db = new Manager();

        public frmNominasDelete()
        {
            InitializeComponent();
            Loaded += frmPin_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            Title= clsLenguajeBO.Find(this.Title);
            clsLenguajeBO.Load(gridLogin);

            DataContext = new clsNominasBE();
        }


        public void OnInit(clsNominasBE _be)
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
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.NominasDelete(BE.NominaID, txtRazon.Text.ToUpper(), clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                var ex = db.DetalleEntradasGet(null).Where(x => x.Numero == BE.NominaID && x.TipoEntradaID == 8).FirstOrDefault();
                                if (ex != null)
                                {
                                    db.EntradasDelete(ex.EntradaID);
                                }

                                this.Close();

                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgDelete"), clsLenguajeBO.Find("msgTitle"));



                                //try
                                //{
                                //    var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/CancelarNominas.html");
                                //    string Body = string.Format(message, BE.Sucursales.Empresas.Empresa, BE.Sucursales.Direccion, BE.Sucursales.Telefonos, BE.Sucursales.Empresas.Rnc, BE.Sucursales.Gerentes.Personas.Nombres + " " + BE.Sucursales.Gerentes.Personas.Apellidos, clsVariablesBO.UsuariosBE.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Personas.Apellidos, clsVariablesBO.UsuariosBE.Documento, BE.NominaID, txtRazon.Text.ToUpper(), BE.Sucursales.Empresas.Empresa, BE.Sucursales.Direccion, BE.Sucursales.Telefonos, BE.Sucursales.Empresas.Rnc, DateTime.Now.Year, BE.Sucursales.Empresas.Empresa, string.Empty);

                                //    Payment.Bank.Core.Common.clsMisc ms = new Payment.Bank.Core.Common.clsMisc();
                                //    ms.SendMail(BE.Sucursales.Gerentes.Personas.Correo, string.Format("Información Nomina #{0} | {1}", BE.NominaID, BE.Sucursales.Empresas.Empresa), Body);
                                //}
                                //catch { }

                                Mensaje.Close();

                            }
                            else
                            {
                                clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            }
                            Mensaje.Close();
                        };

                        Mensaje.btnSalir.Click += (obj, args) =>
                        {
                            Mensaje.Close();
                        };
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
