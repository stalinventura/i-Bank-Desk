using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;


using Payment.Bank.Entity;
using Payment.Bank.Modulos;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmBitacoras.xaml
    /// </summary>
    public partial class frmBitacoras : MetroWindow
    {
        int VISTA = 1;
        clsLlamadasBE BE = new clsLlamadasBE();
        Core.Manager db = new Core.Manager();
        public int ContratoID = 0;
        clsContratosBE row = new clsContratosBE();

        public frmBitacoras()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridSms);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            DataContext = new clsSmsBE() { Fecha=DateTime.Now};
            Loaded += frmSms_Loaded;
            txtNombres.KeyDown += TxtCelular_KeyDown;
        }

        private void TxtCelular_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }



        private void frmSms_Loaded(object sender, RoutedEventArgs e)
        {
            row = db.ContratosGetByContratoID(ContratoID);
            txtNombres.Text = $"{row.Solicitudes.Clientes.Personas.Nombres} {row.Solicitudes.Clientes.Personas.Apellidos}";

            txtFecha.SelectedDate = DateTime.Now;
        }

        private string ShortName(string Name)
        {
            try
            {
                string Short = string.Empty;
                foreach (char x in Name.ToCharArray())
                {
                    if (x.ToString() == " ")
                    {
                        break;
                    }
                    Short += x;
                }
                return Short;
            }
            catch { return string.Empty; }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSms))
                {
                    var result = new OperationResult();
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        var request = db.LlamadasCreate((DateTime)txtFecha.SelectedDate, txtNombres.Text, txtMensaje.Text.ToUpper(), ContratoID, clsVariablesBO.UsuariosBE.Documento);
                        if (request.ResponseCode == "00")
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
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSms))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            var result = new OperationResult();

                            result = db.LlamadasUpdate(BE.LlamadaID, (DateTime)txtFecha.SelectedDate, txtNombres.Text, txtMensaje.Text.ToUpper(), ContratoID, clsVariablesBO.UsuariosBE.Documento);
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
            try
            {
                txtMensaje.Text = string.Empty;
                txtMensaje.Focus();
                VISTA = 1;
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsLlamadasBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                }
            }
            catch { }
        }     
    }
}
