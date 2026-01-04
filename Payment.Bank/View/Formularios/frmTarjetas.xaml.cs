using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;


using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.Core.Model.Request;
using Newtonsoft.Json;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmTarjetas.xaml
    /// </summary>
    public partial class frmTarjetas : MetroWindow
    {
        float Monto = 0;
        clsContratosBE BE = new clsContratosBE();
        Core.Manager db = new Core.Manager();
        public Core.Model.Response.Root response = new Core.Model.Response.Root();

        public frmTarjetas()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridTarjetas);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            DataContext = new clsGarantiaTarjetasBE();
            Loaded += frmTarjetas_Loaded;
            txtNumero.KeyDown += txtNumero_KeyDown;
            txtCvv.KeyDown += txtCvv_KeyDown;
        }

        private void txtCvv_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtNumero_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void frmTarjetas_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var meses = db.MonthGet().ToList();
                cmbExpirationMonth.ItemsSource = meses;
                cmbExpirationMonth.SelectedValuePath = "MonthID";
                cmbExpirationMonth.DisplayMemberPath = "Month";
                cmbExpirationMonth.SelectedValue = DateTime.Now.Month;

                var anos = db.YearGet().ToList();
                cmbExpirationYear.ItemsSource = anos;
                cmbExpirationYear.SelectedValuePath = "YearID";
                cmbExpirationYear.DisplayMemberPath = "Year";
                cmbExpirationYear.SelectedValue = DateTime.Now.Year;
            }
            catch { }
        }

        public bool Validartarjetas()
        {
            bool result = clsValidacionesBO.Validar(gridTarjetas);
            if (result)
            {
                if (DateTime.Now.Year >= (int)cmbExpirationYear.SelectedValue && DateTime.Now.Month >= (int)cmbExpirationMonth.SelectedValue)
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgExpiredCard"), clsLenguajeBO.Find("msgTitle"));
                    return false;
                }
            }

            return result;
        }


        public bool ComprobarTarjeta()
        {
            try
            {
                Root root = new Root();

                root.clientReferenceInformation = new ClientReferenceInformation { code = $"{Guid.NewGuid()}" };
                root.orderInformation = new OrderInformation { amountDetails = new AmountDetails { currency = BE.Solicitudes.Clientes.Personas.Ciudades.Provincias.Paises.Monedas.Codigo, totalAmount = Monto.ToString() }, billTo = new BillTo { country = "DO", email =string.IsNullOrEmpty( BE.Solicitudes.Clientes.Personas.Correo)? "notiene@gmail.com" : BE.Solicitudes.Clientes.Personas.Correo, address1 = string.IsNullOrEmpty(BE.Solicitudes.Clientes.Personas.Direccion)? BE.Solicitudes.Clientes.Personas.Ciudades.Ciudad: BE.Solicitudes.Clientes.Personas.Direccion, administrativeArea = clsVariablesBO.UsuariosBE.Sucursales.Clientes.FirstOrDefault().Personas.Ciudades.Provincias.Provincia, firstName = BE.Solicitudes.Clientes.Personas.Nombres, lastName = BE.Solicitudes.Clientes.Personas.Apellidos, phoneNumber = string.IsNullOrEmpty(BE.Solicitudes.Clientes.Personas.Celular.Replace("(","").Replace(")","").Replace("-",""))? BE.Solicitudes.Clientes.Personas.Telefono: BE.Solicitudes.Clientes.Personas.Celular , locality = clsVariablesBO.UsuariosBE.Sucursales.Clientes.FirstOrDefault().Personas.Ciudades.Ciudad, postalCode = "43002" } };
                root.paymentInformation = new PaymentInformation { card = new Card { expirationMonth = ((int)cmbExpirationMonth.SelectedValue).ToString(), expirationYear = ((int)cmbExpirationYear.SelectedValue).ToString(), number = txtNumero.Text, securityCode = txtCvv.Text } };
                root.processingInformation = new ProcessingInformation { commerceIndicator = "internet" };
                string JsonObj = JsonConvert.SerializeObject(root);

                response = db.ProcesarPagosGetByMerchantID(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, JsonObj, Common.FingerPrint.GetKey());
                if (response.status == "AUTHORIZED")//.processorInformation.responseCode == "100")
                {
                    return true;
                }
                else
                {
                    clsMessage.ErrorMessage(response.errorInformation.message, clsLenguajeBO.Find("msgTitle"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsMessage.ErrorMessage(clsLenguajeBO.Find("msgErroGateway"), clsLenguajeBO.Find("msgTitle"));
                return false;
            }
        }



        private void Limpiar()
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in this.gridTarjetas.Children)
                {
                    if (Obj is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        ctl.Text = String.Empty;
                    }
                }
            }
            catch { }
        }


        public void OnInit(clsContratosBE be, float _monto)
        {
            try
            {
                BE = be;
                Monto = _monto;
                lblHolder.Text = $"{Common.Generic.ShortName(BE.Solicitudes.Clientes.Personas.Nombres.ToUpper())} {Common.Generic.ShortName(be.Solicitudes.Clientes.Personas.Apellidos.ToUpper())}";
            }
            catch { }
        }     
    }
}
