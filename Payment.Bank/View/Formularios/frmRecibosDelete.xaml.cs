using Payment.Bank.Controles;
using Payment.Bank.Core;
using Payment.Bank.Core.Model;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes;
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
    /// Interaction logic for frmRecibosDelete.xaml
    /// </summary>
    public partial class frmRecibosDelete : Window
    {
        clsRecibosBE BE = new clsRecibosBE();
        Manager db = new Manager();
        public bool PermissionAccess= false;

        public frmRecibosDelete()
        {
            InitializeComponent();
            Loaded += frmPin_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            Title= clsLenguajeBO.Find(this.Title);
            clsLenguajeBO.Load(gridLogin);

            DataContext = new clsRecibosBE();
        }


        public void OnInit(clsRecibosBE _be)
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
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            OperationResult result = db.RecibosDelete(BE.ReciboID, txtRazon.Text.ToUpper(), clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                var item = db.DetalleEntradasGet(null).Where(x => x.Numero == BE.ReciboID && x.TipoEntradaID == 2).FirstOrDefault();
                                if (item != null)
                                {
                                    db.EntradasDelete(item.EntradaID);
                                }

                                Close();

                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgDelete"), clsLenguajeBO.Find("msgTitle"));

                                clsCuotasView Cuotas = new clsCuotasView();
                                Cuotas.SetDataSource(BE.ContratoID, BE.ReciboID);
                                var R = Cuotas.GetGroup();
                                var balance = (Cuotas.BalanceGetByReciboID() < 0 ? 0 : Cuotas.BalanceGetByReciboID());

                                if (balance < clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.MinimumAmount)
                                {
                                    frmPrintCartaSaldo Saldo = new frmPrintCartaSaldo();
                                    Saldo.Owner = this;
                                    Saldo.OnInit(BE.ReciboID);
                                    Saldo.Closed += (objs, arg) =>
                                    {
                                        db.ContratosDeleteGetByContratoID(BE.ContratoID, 2, "SALDO A CONTRATO #" + BE.ContratoID.ToString(), clsVariablesBO.UsuariosBE.Documento);
                                    };
                                    Saldo.ShowDialog();
                                }
                                else
                                {
                                    var row = db.ContratosGetByContratoID(BE.ContratoID);
                                    if (row.EstadoID == 2)
                                    {
                                        db.ContratosDeleteGetByContratoID(BE.ContratoID, 1, "ACTIVANDO CONTRATO #" + BE.ContratoID.ToString(), clsVariablesBO.UsuariosBE.Documento);
                                    }
                                }

                                try
                                {
                                    var message = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Templates/CancelarRecibos.html");
                                    string Body = string.Format(message, BE.Sucursales.Empresas.Empresa, BE.Sucursales.Direccion, BE.Sucursales.Telefonos, BE.Sucursales.Empresas.Rnc, BE.Sucursales.Gerentes.Personas.Nombres + " " + BE.Sucursales.Gerentes.Personas.Apellidos, clsVariablesBO.UsuariosBE.Personas.Nombres + " " + clsVariablesBO.UsuariosBE.Personas.Apellidos, clsVariablesBO.UsuariosBE.Documento, BE.ReciboID, txtRazon.Text.ToUpper(), BE.Sucursales.Empresas.Empresa, BE.Sucursales.Direccion, BE.Sucursales.Telefonos, BE.Sucursales.Empresas.Rnc, DateTime.Now.Year, BE.Sucursales.Empresas.Empresa, BE.Sucursales.Empresas.Site);

                                    Core.Common.clsMisc ms = new Core.Common.clsMisc();
                                    ms.SendMail(BE.Sucursales.Gerentes.Personas.Correo, string.Format("Alerta Recibo #{0} | {1}", BE.ReciboID, BE.Sucursales.Empresas.Empresa), Body);
                                }
                                catch { }

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
