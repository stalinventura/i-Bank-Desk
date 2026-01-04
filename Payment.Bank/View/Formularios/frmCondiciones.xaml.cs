using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

using Payment.Bank.Entity;
using Payment.Bank.Modulos;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmCondiciones : MetroWindow
    {
        int VISTA = 1;
        clsCondicionesBE BE = new clsCondicionesBE();
        Core.Manager db = new Core.Manager();
        public frmCondiciones()
        {
            InitializeComponent();

            if (VISTA == 1)
            {
                DataContext = new clsCondicionesBE();
            }

            clsLenguajeBO.Load(gridCondiciones);
            clsLenguajeBO.Load(gridLegal);
            clsLenguajeBO.Load(gridSeguro);
            clsLenguajeBO.Load(gridMoraAutomaticas);
            clsLenguajeBO.Load(gridDataCredito);
            clsLenguajeBO.Load(gridCartas);
            clsLenguajeBO.Load(gridBank);

            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
       
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmCondiciones_Loaded;

            txtInteres.LostFocus += txtInteres_LostFocus;
            txtInteres.KeyDown += txtInteres_KeyDown;

            txtComision.LostFocus += txtComision_LostFocus;
            txtComision.KeyDown += txtComision_KeyDown;

            rbMorasAut_Si.Checked += rbMorasAut_Si_Checked;
            rbMorasAut_No.Checked += rbMorasAut_No_Checked;

            rbLegal_Si.Checked += rbLegal_Si_Checked;
            rbLegal_No.Checked += rbLegal_No_Checked;

            txtLegal.LostFocus += txtLegal_LostFocus;
            txtLegal.KeyDown += txtLegal_KeyDown;


            rbInsurance_Si.Checked += rbInsurance_Si_Checked;
            rbInsurance_No.Checked += rbInsurance_No_Checked;
            
            txtTasaSeguro.LostFocus += txtTasaSeguro_LostFocus;
            txtTasaSeguro.KeyDown += txtTasaSeguro_KeyDown;

            rbBankRequest_Si.Checked += rbBankRequest_Si_Checked;
            rbBankRequest_No.Checked += rbBankRequest_No_Checked;
        }

        private void rbBankRequest_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbBancos.IsEnabled = true;
                cmbBancos.Focus();
            }
            catch { }
        }

        private void rbBankRequest_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbBancos.IsEnabled = false;
                cmbBancos.SelectedValue = -1;
            }
            catch { }
        }

        private void frmCondiciones_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext = new clsCondicionesBE();
                //Bancos
                List<clsBancosBE> Bancos = new List<clsBancosBE>();
                Bancos = db.BancosGet(null, clsVariablesBO.UsuariosBE.Documento).Where(x => x.SucursalID == clsVariablesBO.UsuariosBE.SucursalID).ToList();

                Bancos.Add(new clsBancosBE { BancoID = -1, Banco = clsLenguajeBO.Find("itemSelect") });

                cmbBancos.ItemsSource = Bancos;
                cmbBancos.SelectedValuePath = "BancoID";
                cmbBancos.DisplayMemberPath = "Banco";
                cmbBancos.SelectedValue = -1;
            }
            catch { }
        }



        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ISDefault = rbDefault_Si.IsChecked == true ? true : false;
                bool SendSMS = rbSendSMS_Si.IsChecked == true ? true : false;
                bool MoraAutomatica = rbMorasAut_Si.IsChecked == true ? true : false;
                bool Islegal = rbLegal_Si.IsChecked == true ? true : false;
                bool IsSeguro = rbInsurance_Si.IsChecked == true ? true : false;
                bool DataCredito = rbDataCredito_Si.IsChecked == true ? true : false;
                bool ShowComision = rbComision_Si.IsChecked == true ? true : false;
                bool LetterDelay = rbCartas_Si.IsChecked == true ? true : false;
                bool IsGenerateRequest = rbBankRequest_Si.IsChecked == true ? true : false;

                if (VISTA == 1 && clsValidacionesBO.Validar(gridCondiciones))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        //OperationResult result = db.CondicionesCreate( float.Parse(txtAlto.Text), clsVariablesBO.UsuariosBE.Documento);
                        //if (result.ResponseCode == "00")
                        //{
                        //    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                        //    Limpiar();
                        //}
                        //else
                        //{
                        //    clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                        //}
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridCondiciones))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.CondicionesUpdate(BE.CondicionID, BE.Condicion, BE.Meses, BE.Dias, ISDefault, SendSMS, int.Parse(txtTiempo.Text), float.Parse(txtInteres.Text), float.Parse(txtComision.Text), Islegal, float.Parse(txtLegal.Text),IsSeguro, float.Parse(txtTasaSeguro.Text),MoraAutomatica, float.Parse(txtMoras.Text), int.Parse(txtDiasGracia.Text),ShowComision, DataCredito, LetterDelay, IsGenerateRequest, (int)cmbBancos.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridCondiciones);
            VISTA = 1;
            
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsCondicionesBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;               
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                    Title = Title + $" [{BE.Condicion}]";

                    if (BE.IsDefault)
                    {
                        rbDefault_Si.IsChecked = true;
                        rbDefault_No.IsChecked = false;
                    }
                    else
                    {
                        rbDefault_Si.IsChecked = false;
                        rbDefault_No.IsChecked = true;
                    }

                    if (BE.SendSMS)
                    {
                        rbSendSMS_Si.IsChecked = true;
                        rbSendSMS_No.IsChecked = false;
                    }
                    else
                    {
                        rbSendSMS_Si.IsChecked = false;
                        rbSendSMS_No.IsChecked = true;
                    }

                    if (BE.IsLegal)
                    {
                        rbLegal_Si.IsChecked = true;
                        rbLegal_No.IsChecked = false;
                    }
                    else
                    {
                        rbLegal_Si.IsChecked = false;
                        rbLegal_No.IsChecked = true;
                    }

                    if (BE.IsSeguro)
                    {
                        rbInsurance_Si.IsChecked = true;
                        rbInsurance_No.IsChecked = false;
                    }
                    else
                    {
                        rbInsurance_Si.IsChecked = false;
                        rbInsurance_No.IsChecked = true;
                    }

                    if (BE.MoraAutomatica)
                    {
                        rbMorasAut_Si.IsChecked = true;
                        rbMorasAut_No.IsChecked = false;
                    }
                    else
                    {
                        rbMorasAut_Si.IsChecked = false;
                        rbMorasAut_No.IsChecked = true;
                    }

                    if (BE.ShowComision)
                    {
                        rbComision_Si.IsChecked = true;
                        rbComision_No.IsChecked = false;
                    }
                    else
                    {
                        rbComision_Si.IsChecked = false;
                        rbComision_No.IsChecked = true;
                    }

                    if (BE.DataCredito)
                    {
                        rbDataCredito_Si.IsChecked = true;
                        rbDataCredito_No.IsChecked = false;
                    }
                    else
                    {
                        rbDataCredito_Si.IsChecked = false;
                        rbDataCredito_No.IsChecked = true;
                    }

                    if (BE.LetterDelay)
                    {
                        rbCartas_Si.IsChecked = true;
                        rbCartas_No.IsChecked = false;
                    }
                    else
                    {
                        rbCartas_Si.IsChecked = false;
                        rbCartas_No.IsChecked = true;
                    }

                    if (BE.IsGenerateRequest)
                    {
                        rbBankRequest_Si.IsChecked = true;
                        rbBankRequest_No.IsChecked = false;
                        cmbBancos.IsEnabled = true;
                    }
                    else
                    {
                        rbBankRequest_Si.IsChecked = false;
                        rbBankRequest_No.IsChecked = true;
                        cmbBancos.IsEnabled = false;
                    }

                    txtTiempo.Text = string.Format("{0:0}", BE.Tiempo);
                    txtInteres.Text = string.Format("{0:N3}", BE.Interes);
                    txtLegal.Text = string.Format("{0:0}", BE.Legal);
                    txtTasaSeguro.Text = string.Format("{0:0}", BE.Seguro);
                    txtMoras.Text = string.Format("{0:0}", BE.Mora);
                    txtDiasGracia.Text = string.Format("{0:0}", BE.DiasGracia);
                    if(BE.MoraAutomatica)
                    { 
                        txtMoras.IsEnabled = true;
                        txtDiasGracia.IsEnabled = true;
                    }
                    txtTiempo.Focus();
                }

            }
            catch { }
        }

        private void txtTasaSeguro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double seguro = double.Parse(txtTasaSeguro.Text);
                txtTasaSeguro.Text = string.Format("{0:0.000}", seguro);
            }
            catch { txtTasaSeguro.Text = string.Format("{0:0.000}", 0); }
        }

        private void txtLegal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void rbInsurance_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTasaSeguro.IsEnabled = false;
                txtTasaSeguro.Text = string.Format("{0:N3}", 0);
            }
            catch { }
        }

        private void txtComision_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtInteres_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtTasaSeguro_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch { }
        }

        private void txtInteres_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double interes = double.Parse(txtInteres.Text);
                txtInteres.Text = String.Format("{0:0.000}", interes);
            }
            catch { }
        }

        private void txtComision_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double comision = double.Parse(txtComision.Text);
                txtComision.Text = string.Format("{0:N2}", comision);
            }
            catch { }
        }



        private void rbInsurance_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTasaSeguro.IsEnabled = true;
                txtTasaSeguro.Focus();
            }
            catch { }
        }

        private void rbLegal_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtLegal.IsEnabled = false;
                txtLegal.Text = string.Format("{0:N3}", 0);
            }
            catch { }
        }

        private void txtLegal_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double legal = double.Parse(txtLegal.Text);
                txtLegal.Text = string.Format("{0:0.000}", legal);
            }
            catch { txtLegal.Text = string.Format("{0:0.000}", 0); }
        }

        private void rbLegal_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtLegal.IsEnabled = true;
                txtLegal.Focus();
            }
            catch { }
        }

        private void rbMorasAut_No_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMoras.IsEnabled = false;
                txtDiasGracia.IsEnabled = false;
                txtMoras.Text = string.Format("{0:N2}", 0);
                txtDiasGracia.Text = string.Format("{0:0}", 0);
            }
            catch { }
        }

        private void rbMorasAut_Si_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMoras.IsEnabled = true;
                txtDiasGracia.IsEnabled = true;
                txtMoras.Focus();
            }
            catch { }
        }
    }
}
