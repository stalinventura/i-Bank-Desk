using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;


using Payment.Bank.Entity;
using Payment.Bank.Modulos;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmTurnos.xaml
    /// </summary>
    public partial class frmTurnos : MetroWindow
    {
        int VISTA = 1;
        clsTurnosBE BE = new clsTurnosBE();
        Core.Manager db = new Core.Manager();
        public int TurnoID = 0;

        public frmTurnos()
        {
            InitializeComponent();

            clsLenguajeBO.Load(gridSms);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            DataContext = new clsTurnosBE() { CajaID =-1};
            Loaded += frmTurnos_Loaded;
            txtMonto.KeyDown += txtMonto_KeyDown;
            txtMonto.LostFocus += txtMonto_LostFocus;
            if (VISTA == 1)
            {
                LoadCombox();
            }
        }

        private void txtMonto_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double monto = double.Parse(txtMonto.Text);
                txtMonto.Text = String.Format("{0:N2}", monto);
            }
            catch { }
        }

        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
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

        private void frmTurnos_Loaded(object sender, RoutedEventArgs e)
        {
            if (VISTA == 1)
            {
                txtCajero.Text = $"{clsVariablesBO.UsuariosBE.Personas.Nombres.ToUpper()} {clsVariablesBO.UsuariosBE.Personas.Apellidos.ToUpper()}";
            }
            else
            {
                txtCajero.Text = $"{BE.Usuarios.Personas.Nombres.ToUpper()} {BE.Usuarios.Personas.Apellidos.ToUpper()}";
            }
        }


        void LoadCombox()
        {
            try
            {
                List<clsCajasBE> cajas = VISTA == 1 ? db.CajasGetNotOpen() : new List<clsCajasBE>();
                if (VISTA == 1)
                {
                    cajas.Add(new clsCajasBE { CajaID = -1, Caja = clsLenguajeBO.Find("itemSelect") });
                }
                else
                {
                    cajas = new List<clsCajasBE>();
                    var row = db.CajasGetByCajaID(BE.CajaID);
                    cajas.Add(new clsCajasBE { CajaID = row.CajaID, Caja = row.Caja });
                }
                cmbCajas.ItemsSource = cajas;
                cmbCajas.SelectedValue = -1;
                cmbCajas.SelectedValuePath = "CajaID";
                cmbCajas.DisplayMemberPath = "Caja";
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
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
                        var row = db.OpenBoxGetByDocumento(clsVariablesBO.UsuariosBE.Documento);
                        if (row == null)
                        {
                            var request = db.TurnosCreate(clsVariablesBO.UsuariosBE.Documento, (int)cmbCajas.SelectedValue, float.Parse(txtMonto.Text), txtObservaciones.Text.ToUpper(), clsVariablesBO.UsuariosBE.Documento);
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
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgOpenBox"), clsLenguajeBO.Find("msgTitle"));
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

                            result = db.TurnosUpdate(BE.TurnoID, BE.Documento, (int)cmbCajas.SelectedValue, float.Parse(txtMonto.Text), txtObservaciones.Text.ToUpper(), clsVariablesBO.UsuariosBE.Documento);
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
                Close();
                VISTA = 1;
            }
            catch { }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsTurnosBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                { 
                    BE = be;
                    LoadCombox();
                  
                    DataContext = BE;
                }
            }
            catch { }
        }     
    }
}
