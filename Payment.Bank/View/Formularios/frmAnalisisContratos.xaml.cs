using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;


using Payment.Bank.Modulos;
using Microsoft.Win32;
using Payment.Bank.View.Informes;
using System.Data;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmAnalisisContratos : MetroWindow
    {
        int VISTA = 1;
        string _Documento = string.Empty;
        string _DocumentoGarante = string.Empty;

        double Cuota = 0;
        DateTime Vence = DateTime.Now;

        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<clsReferenciasBE> _Referencias = new List<clsReferenciasBE>();
        Core.Manager db = new Core.Manager();
        DataTable Detalle;

        public frmAnalisisContratos()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());

            clsLenguajeBO.Load(gridSolicitudes);
            clsLenguajeBO.Load(gridContratos);

            if (VISTA == 1)
            {
                gridSolicitudes.DataContext = new clsSolicitudesBE();
                gridContratos.DataContext = new clsContratosBE();
            }

            btnSalir.Click += btnSalir_Click;
            Loaded += frmContratos_Loaded;
            
            txtTiempo.KeyDown += txtTiempo_KeyDown;
            txtTiempo.TextChanged += txtTiempo_TextChanged;
            txtMonto.TextChanged += txtMonto_TextChanged;


            btnAceptar.Click += bnAceptar_Click;

            //Contratos
            cmbTipoContratos.SelectionChanged += cmbTipoContratos_SelectionChanged;
            txtInteres.TextChanged += txtInteres_TextChanged;
            txtInteres.LostFocus += txtInteres_LostFocus;
            txtInteres.KeyDown += txtInteres_KeyDown;

            txtComision.TextChanged += txtComision_TextChanged;
            txtComision.LostFocus += txtComision_LostFocus;
            txtComision.KeyDown += txtComision_KeyDown;
            txtMontoPrestamo.KeyDown += txtMontoPrestamo_KeyDown;
            txtMonto.KeyDown += txtMonto_KeyDown;

            cmbCondicionSolicitudes.SelectionChanged += cmbCondicionSolicitudes_SelectionChanged;
        }


        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
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

        private void txtMonto_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
            }
            catch { }
        }

        private void txtTiempo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
            }
            catch { }
        }

        private void cmbCondicionSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadConfig();
                CalcularContratos();
            }
            catch { }
        }

        private void LoadConfig()
        {
            //bool DataCredito = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.DataCredito == true ? true : false;
            //bool MoraAutomatica = clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.MoraAutomatica == true ? true : false;

            //txtInteres.Text = string.Format("{0:N2}", clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Interes);
            //txtComision.Text = string.Format("{0:N2}", clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Comision);
            //

            bool DataCredito = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).DataCredito == true ? true : false;
            bool MoraAutomatica = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).MoraAutomatica == true ? true : false;

            txtInteres.Text = string.Format("{0:N2}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Interes);
            txtComision.Text = string.Format("{0:N2}", (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Comision);
        }
        private void txtMontoPrestamo_KeyDown(object sender, KeyEventArgs e)
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

        private void cmbTipoContratos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
            }
            catch { }
        }

        private void txtInteres_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
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

        private void txtComision_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalcularContratos();
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

        
        private void CalcularFecha()
        {
            try
            {
                DateTime Fecha;
                Fecha = (DateTime)txtFechaContrato.SelectedDate;
                int Tiempo = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Dias * Convert.ToInt16(txtTiempo.Text);
                Vence = Fecha.AddMonths(int.Parse(txtTiempo.Text));
            }
            catch { }
        }

        private void CalcularContratos0()
        {
            try
            {
                if (double.Parse(txtMonto.Text) > 0)
                {
                    double MontoCapital = 0;
                    double MontoComision = 0;
                    double MontoInteres = 0;

                    MontoCapital = (Convert.ToDouble(txtMonto.Text) / Convert.ToDouble(txtTiempo.Text));
                    MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100))); //(Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                    MontoInteres = (Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses));

                    lblCuota.Text = string.Format("{0:N2}", MontoCapital + MontoComision + MontoInteres);
                    Cuota = MontoCapital + MontoComision + MontoInteres;

                    txtMontoPrestamo.Text = string.Format("{0:N2}", Convert.ToDouble(txtTiempo.Text) * (MontoCapital + MontoComision + MontoInteres));
                    CalcularFecha();

                    if ((int)cmbTipoContratos.SelectedValue == 1)
                    {
                        double Factor1 = 0, Factor2 = 0, Factor = 0;
                        double Interes = 0; double Tiempo = 0;
                        Interes = Convert.ToDouble(txtInteres.Text) / 100;
                        Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                        Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                        Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                        Factor = Factor1 / Factor2;
                        if (Factor > 0)
                        {
                            lblCuota.Text = string.Format("{0:N2}", ((MontoCapital + MontoComision) * Tiempo) / Factor);
                            txtMontoPrestamo.Text = string.Format("{0:N2}", ((MontoCapital + MontoComision) * Tiempo / Factor) * int.Parse(txtTiempo.Text));
                            Cuota = ((MontoCapital + MontoComision) * Tiempo) / Factor;
                        }
                    }
                    else
                    {
                        if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                        {
                            double a = 0;
                            //if (this.rbComision_Si.IsChecked == true)
                            //{
                            //a = MontoComision + MontoInteres;
                            //}
                            //else
                            //{
                            a = MontoInteres;
                            //}
                            lblCuota.Text = string.Format("{0:N2}", a);
                            Cuota = a;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CalcularContratos()
        {
            try
            {
                if (double.Parse(txtMonto.Text) > 0)
                {

                    double MontoCapital = 0;
                    double MontoComision = 0;
                    double MontoInteres = 0;
                    double ACapital = 0;
                    double AComision = 0;
                    double AInteres = 0;
                    double Cuotas = 0;
                    double Monto = 0;


                    int IdTipoContrato = (int)cmbTipoContratos.SelectedValue;
                    Boolean IsComision = rbComision_Si.IsChecked == true ? true : false;
                    clsCondicionesBE Condiciones = (clsCondicionesBE)cmbCondicionSolicitudes.SelectedItem;
                    double Interes = Convert.ToDouble(txtInteres.Text);
                    double Comision = Convert.ToDouble(txtComision.Text);
                    double Tiempo = Convert.ToDouble(txtTiempo.Text);


                    if (IsComision == true)
                    {
                        Monto = Convert.ToDouble(txtMonto.Text);
                    }
                    else
                    {
                        Monto = Convert.ToDouble(txtMonto.Text) + (Convert.ToDouble(txtMonto.Text) * (Convert.ToDouble(txtComision.Text) / 100));
                    }

                    MontoCapital = (Convert.ToDouble(Monto) / Convert.ToDouble(txtTiempo.Text));
                    if (IsComision == true)
                    {
                        MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100))); //(Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
                    }
                    else
                    {
                        MontoComision = 0;
                    }

                    if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                    {
                        //MontoInteres = double.Parse(lblCuota.Text); //(Monto * float.Parse(txtInteres.Text)/100) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;  //(Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses));
                        //MontoCapital = float.Parse(lblCuota.Text) - (MontoComision + MontoInteres);
                        MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;

                    }
                    else
                    {

                        //MontoInteres = double.Parse(lblCuota.Text) - (MontoCapital + MontoComision);
                        //MontoInteres = (Monto * double.Parse(txtInteres.Text)/100) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                        MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;
                    }





                    lblCuota.Text = string.Format("{0:N2}", Math.Round(MontoCapital + MontoComision + MontoInteres, 0));
                    Cuota = Math.Round(MontoCapital + MontoComision + MontoInteres, 0);

                    txtMontoPrestamo.Text = string.Format("{0:N2}", Convert.ToDouble(txtTiempo.Text) * Math.Round(MontoCapital + MontoComision + MontoInteres, 0));
                    CalcularFecha();

                    if ((int)cmbTipoContratos.SelectedValue == 1)
                    {
                        double Factor1 = 0, Factor2 = 0, Factor = 0;
                        Interes = 0; Tiempo = 0;
                        Interes = Convert.ToDouble(txtInteres.Text) / 100;
                        Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                        Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                        Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                        Factor = Factor1 / Factor2;
                        if (Factor > 0)
                        {
                            lblCuota.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor), 0);
                            txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo / Factor) * Tiempo, 0));
                            Cuota = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0);
                        }
                    }
                    else
                    {
                        if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                        {
                            double a = 0;
                            if (this.rbComision_Si.IsChecked == true)
                            {
                                a = Math.Round(MontoComision + MontoInteres, 0);
                            }
                            else
                            {
                                a = Math.Round(MontoInteres, 0);
                            }
                            lblCuota.Text = string.Format("{0:N2}", a);
                            Cuota = a;
                        }
                    }
                }
            }
            catch
            {
            }
        }


        private void SolicitudesCreate()
        {
            try
            {               
                if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }                
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

                   

        private void SaveSolicitudesCreate()
        {
            try
            {
                CalcularFecha();   
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void CuotasCreateAutomatica(int _ContratoID, string Usuario)
        {
            //try
            //{
            List<clsCuotasBE> lstCuotas = new List<clsCuotasBE>();

            double MontoCapital = 0;
            double MontoComision = 0;
            double MontoInteres = 0;
            double ACapital = 0;
            double AComision = 0;
            double AInteres = 0;
            double Cuotas = 0;
            double Monto = 0;


            int IdTipoContrato = (int)cmbTipoContratos.SelectedValue;
            Boolean IsComision = rbComision_Si.IsChecked == true ? true : false;
            clsCondicionesBE Condiciones = (clsCondicionesBE)cmbCondicionSolicitudes.SelectedItem;
            double Interes = Convert.ToDouble(txtInteres.Text);
            double Comision = Convert.ToDouble(txtComision.Text);
            double Tiempo = Convert.ToDouble(txtTiempo.Text);


            if (IsComision == true)
            {
                Monto = Convert.ToDouble(txtMonto.Text);
            }
            else
            {
                Monto = Convert.ToDouble(txtMonto.Text) + (Convert.ToDouble(txtMonto.Text) * (Convert.ToDouble(txtComision.Text) / 100));
            }

            MontoCapital = (Convert.ToDouble(Monto) / Convert.ToDouble(txtTiempo.Text));
            if (IsComision == true)
            {
                MontoComision = (Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100))); //(Convert.ToDouble(MontoCapital) * (((Convert.ToDouble(txtComision.Text)) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses)));
            }
            else
            {
                MontoComision = 0;
            }

            if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
            {
                //MontoInteres = double.Parse(lblCuota.Text); //(Monto * float.Parse(txtInteres.Text)/100) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;  //(Convert.ToDouble(MontoCapital + MontoComision) * (Convert.ToDouble(txtInteres.Text) / 100) * (Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses));
                //MontoCapital = float.Parse(lblCuota.Text) - (MontoComision + MontoInteres);
                MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;

            }
            else
            {

                //MontoInteres = double.Parse(lblCuota.Text) - (MontoCapital + MontoComision);
                //MontoInteres = (Monto * double.Parse(txtInteres.Text)/100) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                MontoInteres = ((MontoCapital + MontoComision) * double.Parse(txtInteres.Text) / 100) * double.Parse(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses; ;
            }

            //lblCuota.Text = string.Format("{0:N2}", Math.Round(MontoCapital + MontoComision + MontoInteres));
            Cuota = Math.Round(MontoCapital + MontoComision + MontoInteres);

            txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(Convert.ToDouble(txtTiempo.Text) * (MontoCapital + MontoComision + MontoInteres)));


            double Factor1 = 0, Factor2 = 0, Factor = 0;
            if ((int)cmbTipoContratos.SelectedValue == 1)
            {

                //double Interes = 0; double Tiempo = 0;
                Interes = Convert.ToDouble(txtInteres.Text) / 100;
                Tiempo = Convert.ToDouble(txtTiempo.Text) / (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Meses;
                Factor1 = Math.Round(((Math.Pow((1 + Interes), Tiempo)) - 1), 6);
                Factor2 = Math.Round(Interes * Math.Pow((1 + Interes), Tiempo), 6);
                Factor = Factor1 / Factor2;
                if (Factor > 0)
                {
                    lblCuota.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor), 0);
                    txtMontoPrestamo.Text = string.Format("{0:N2}", Math.Round(((MontoCapital + MontoComision) * Tiempo / Factor) * int.Parse(txtTiempo.Text), 0));
                    Cuota = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0);
                    Cuotas = Math.Round(((MontoCapital + MontoComision) * Tiempo) / Factor, 0);
                }
            }
            else
            {
                if ((int)cmbTipoContratos.SelectedValue == 3 || (int)cmbTipoContratos.SelectedValue == 5)
                {
                    double a = 0;
                    if (this.rbComision_Si.IsChecked == true)
                    {
                        a = Math.Round(MontoComision + MontoInteres, 0);
                    }
                    else
                    {
                        a = Math.Round(MontoInteres, 0);
                    }
                    lblCuota.Text = string.Format("{0:N2}", a);
                    Cuota = a;
                }
            }

            //MontoComision = MontoComision * double.Parse(txtTiempo.Text);


            for (int i = 1; i <= Convert.ToInt16(txtTiempo.Text); i++)
            {   
                System.DateTime Fecha;
                Fecha = (DateTime)txtFechaContrato.SelectedDate;
                switch ((int)cmbCondicionSolicitudes.SelectedValue)
                {
                    #region Mensual
                    case 1:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                            AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;//MontoCapital;
                                        CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, ACapital, AComision, AInteres, 0, 0, Usuario);
                                    }
                                    break;
                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, MontoCapital, MontoComision, MontoInteres, 0, 0, Usuario);
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Quincenal
                    case 2:
                        {
                            //if (i == 1)
                            //{
                            //    if (Fecha.Day <= 5)
                            //    {
                            //        Fecha = new DateTime(Fecha.Year, Fecha.Month, 1).AddDays(14);
                            //    }
                            //    else
                            //    {
                            //        if (Fecha.Day >= 20)
                            //        {
                            //            Fecha = new DateTime(Fecha.Year, Fecha.Month + 1, 1).AddDays(14);
                            //        }
                            //        else
                            //        {
                            //            int dia = DateTime.DaysInMonth(Fecha.Year, Fecha.Month) < 30 ? DateTime.DaysInMonth(Fecha.Year, Fecha.Month)-15 : 15;
                            //            Fecha = new DateTime(Fecha.Year, Fecha.Month, 15).AddDays(dia);
                            //        }
                            //    }
                            //}
                            //else
                            //{

                            //    int dia = 15;
                            //    if(Fecha.Day == 15)
                            //    {
                            //         dia = DateTime.DaysInMonth(Fecha.Year, Fecha.Month) - 15;
                            //    }
                            //    Fecha = Fecha.AddDays(dia);                                

                            //}

                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        //if (IsComision == true)
                                        //{
                                        //    AComision = (Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        //}
                                        //else
                                        //{
                                        //    AComision = 0;
                                        //}
                                        //AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        //ACapital = Cuotas - AInteres - AComision;
                                        //Monto = Monto - ACapital;
                                        //CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, ACapital, AComision, AInteres, 0, 0, Usuario);

                                        if (IsComision == true)
                                        {
                                            AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;//MontoCapital;
                                        CuotasCreate(i, Fecha.AddMonths(Condiciones.Dias * i), _ContratoID, ACapital, AComision, AInteres, 0, 0, Usuario);

                                    }
                                    break;

                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, MontoCapital, MontoComision, MontoInteres, 0, 0, Usuario);
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Semanal
                    case 3:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                            AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;
                                        CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, ACapital, AComision, AInteres, 0, 0, Usuario);
                                    }
                                    break;
                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, MontoCapital, MontoComision, MontoInteres, 0, 0, Usuario);
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Diario
                    case 4:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                            AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;
                                        CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, ACapital, AComision, AInteres, 0, 0, Usuario);
                                    }
                                    break;
                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        CuotasCreate(i, Fecha.AddDays((Condiciones.Dias * i) + 4), _ContratoID, MontoCapital, MontoComision, MontoInteres, 0, 0, Usuario);
                                    }
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region Default
                    default:
                        {
                            switch ((int)cmbTipoContratos.SelectedValue)
                            {
                                case 1:
                                    {
                                        if (IsComision == true)
                                        {
                                            AComision = MontoComision; //(Monto * (Convert.ToDouble(txtComision.Text) / 100) / Condiciones.Meses);
                                        }
                                        else
                                        {
                                            AComision = 0;
                                        }
                                        AInteres = ((Monto + AComision) * (Convert.ToDouble(txtInteres.Text) / 100)) / Condiciones.Meses;
                                        ACapital = Cuotas - AInteres - AComision;
                                        Monto = Monto - ACapital;
                                        CuotasCreate(i, Fecha.AddMonths(i), _ContratoID, ACapital, AComision, AInteres, 0, 0, Usuario);
                                    }
                                    break;
                                case 3:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        if (i == Convert.ToInt16(txtTiempo.Text))
                                        {
                                            CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, Monto, MontoComision, MontoInteres, 0, 0, Usuario);
                                        }
                                        else
                                        {
                                            if (IsComision == true)
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, MontoComision, MontoInteres, 0, 0, Usuario);
                                            }
                                            else
                                            {
                                                CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, 0, 0, MontoInteres, 0, 0, Usuario);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        CuotasCreate(i, Fecha.AddDays(Condiciones.Dias * i), _ContratoID, MontoCapital, MontoComision, MontoInteres, 0, 0, Usuario);
                                    }
                                    break;
                            }
                        }
                        break;
                        #endregion
                }
            }
            //if (_ContratoID > 0)
            //{
            //    ContratosGet();
            //}

            //}
            //catch
            //{ }
        }


        private void CuotasCreate(int Numero, DateTime Vence, int ContratoID, double Capital, double Comision, double Interes, double Mora, double Legal, string Usuario)
        {
            try
            {
                DataRow row = Detalle.NewRow();
                row["Numero"] = Numero;
                row["Vencimiento"] = Vence;
                row["Capital"] = Capital;
                row["Comision"] = Comision;
                row["Interes"] = Interes;
                row["Mora"] = Mora;
                row["Legal"] = Legal;
                row["Balance"] = Capital + Comision + Interes + Mora + Legal;
                Detalle.Rows.Add(row);
            }
            catch(Exception ex) { }
        }

        void ClearAll()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridSolicitudes);
                clsValidacionesBO.Limpiar(gridContratos);
                _Referencias = new List<clsReferenciasBE>();

                VISTA = 1;
                txtFechaContrato.SelectedDate = DateTime.Now;
                lblCuota.Text = string.Format("{0:N2}", 0);
            }
            catch {

                //clsCuotasView Cuotas = new clsCuotasView();
                //Cuotas.SetDataSource();

            }
        }

        private void SolicitudesUpdate()
        {
            try
            {
                if (VISTA == 1) { SaveSolicitudesCreate(); } else { SaveSolicitudesUpdate(); }
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void SaveSolicitudesUpdate()
        {
            try
            {
                CalcularFecha();
            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }
            
        private void txtTiempo_KeyDown(object sender, KeyEventArgs e)
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

        private void bnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (clsValidacionesBO.Validar(gridContratos))
                {

                    //Detalle
                    Detalle = new DataTable("Detalle");
                    Detalle.Columns.Add("Numero", typeof(int));
                    Detalle.Columns.Add("Vencimiento", typeof(DateTime));
                    Detalle.Columns.Add("Capital", typeof(float));
                    Detalle.Columns.Add("Comision", typeof(float));
                    Detalle.Columns.Add("Interes", typeof(float));
                    Detalle.Columns.Add("Mora", typeof(float));
                    Detalle.Columns.Add("Legal", typeof(float));
                    Detalle.Columns.Add("Balance", typeof(float));

                    DataSet ds = new DataSet();
                    CuotasCreateAutomatica(0, clsVariablesBO.UsuariosBE.Documento);
                    ds.Tables.Add(Detalle);

                    //Header
                    DataTable Header = new DataTable("Header");
                    Header.Columns.Add("Fotografia", typeof(byte[]));
                    Header.Columns.Add("BarCode", typeof(byte[]));
                    DataRow dataRow = Header.NewRow();
                    //dataRow["Fotografia"] = (byte[])db.clsContratosBE.Where(x => x.ContratoID == ContratoID).FirstOrDefault().Solicitudes.Clientes.Personas.Fotografia);//ImageFromUrl(@"C:\Users\SERVIDOR\Documents\App Developer\SoftArch\Payment Bank\Payment.Bank\Images\13335.png");// db.clsContratosBE.Where(x => x.ContratoID == ContratoID).FirstOrDefault().Solicitudes.Clientes.Personas.Fotografia);
                    dataRow["BarCode"] = Payment.Bank.Core.Manager.ImageFromText(DateTime.Today.ToString());
                    Header.Rows.Add(dataRow);
                    ds.Tables.Add(Header);


                    //Header
                    DataTable Cuotas = new DataTable("Cuotas");
                    Cuotas.Columns.Add("Empresa", typeof(string));
                    Cuotas.Columns.Add("Direccion", typeof(string));
                    Cuotas.Columns.Add("Telefonos", typeof(string));
                    Cuotas.Columns.Add("Rnc", typeof(string));
                    Cuotas.Columns.Add("Sucursal", typeof(string));
                    Cuotas.Columns.Add("Fecha", typeof(DateTime));
                    Cuotas.Columns.Add("Vence", typeof(DateTime));
                    Cuotas.Columns.Add("Interes", typeof(double));
                    Cuotas.Columns.Add("Comision", typeof(double));
                    Cuotas.Columns.Add("Monto", typeof(double));
                    Cuotas.Columns.Add("TipoSolicitud", typeof(string));
                    Cuotas.Columns.Add("Tiempo", typeof(int));
                    Cuotas.Columns.Add("Condicion", typeof(string));

                    DataRow dataRowCuotas = Cuotas.NewRow();

                    dataRowCuotas["Empresa"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                    dataRowCuotas["Direccion"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                    dataRowCuotas["Telefonos"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                    dataRowCuotas["Rnc"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                    dataRowCuotas["Sucursal"] = clsVariablesBO.UsuariosBE.Sucursales.Sucursal;
                    dataRowCuotas["TipoSolicitud"] = (cmbTipoSolicitudes.SelectedItem as clsTipoSolicitudesBE).TipoSolicitud;
                    dataRowCuotas["Condicion"] = (cmbCondicionSolicitudes.SelectedItem as clsCondicionesBE).Condicion;
                    dataRowCuotas["Tiempo"] = int.Parse(txtTiempo.Text);
                    dataRowCuotas["Monto"] = double.Parse(txtMonto.Text);
                    dataRowCuotas["Fecha"] = (DateTime)txtFechaContrato.SelectedDate;
                    dataRowCuotas["Vence"] = Vence;
                    dataRowCuotas["Interes"] = double.Parse(txtInteres.Text);
                    dataRowCuotas["Comision"] = double.Parse(txtComision.Text);


                    Cuotas.Rows.Add(dataRowCuotas);
                    ds.Tables.Add(Cuotas);

                    frmPrintAnalisisContratos Analisis = new frmPrintAnalisisContratos();
                    Analisis.Owner = this;
                    Analisis.OnInit(ds);
                    Analisis.ShowDialog();
                }


            }
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        
        private void LoadCombox()
        {
            //Tipo de Solicitudes
            List<clsTipoSolicitudesBE> TipoSolicitudes = new List<clsTipoSolicitudesBE>();
            TipoSolicitudes = db.TipoSolicitudesGet(null).ToList();
            TipoSolicitudes.Add(new clsTipoSolicitudesBE { TipoSolicitudID = -1, TipoSolicitud = clsLenguajeBO.Find("itemSelect") });
            cmbTipoSolicitudes.ItemsSource = TipoSolicitudes;
            cmbTipoSolicitudes.SelectedValuePath = "TipoSolicitudID";
            cmbTipoSolicitudes.DisplayMemberPath = "TipoSolicitud";

            if (TipoSolicitudes.Count() > 1)
            {
                cmbTipoSolicitudes.SelectedValue = TipoSolicitudes.Where(x => x.IsDefault == true).FirstOrDefault().TipoSolicitudID;
            }
            else
            {
                cmbTipoSolicitudes.SelectedValue = -1;
            }

            //Condiciones
            List<clsCondicionesBE> Condiciones = new List<clsCondicionesBE>();
            Condiciones = db.CondicionesGet(null).ToList();
            Condiciones.Add(new clsCondicionesBE { CondicionID = -1, Condicion = clsLenguajeBO.Find("itemSelect") });
            cmbCondicionSolicitudes.ItemsSource = Condiciones;
            cmbCondicionSolicitudes.SelectedValuePath = "CondicionID";
            cmbCondicionSolicitudes.DisplayMemberPath = "Condicion";
            cmbCondicionSolicitudes.SelectedValue = Condiciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;

            if (Condiciones.Count() > 1)
            {
                cmbCondicionSolicitudes.SelectedValue = Condiciones.Where(x => x.IsDefault == true).FirstOrDefault().CondicionID;
            }
            else
            {
                cmbTipoSolicitudes.SelectedValue = -1;
            }
            

            //Tipo de Contratos
            List<clsTipoContratosBE> TipoContratos = new List<clsTipoContratosBE>();
            TipoContratos = db.TipoContratosGet(null).ToList();
            TipoContratos.Add(new clsTipoContratosBE { TipoContratoID = -1, TipoContrato = clsLenguajeBO.Find("itemSelect") });
            cmbTipoContratos.ItemsSource = TipoContratos;
            cmbTipoContratos.SelectedValuePath = "TipoContratoID";
            cmbTipoContratos.DisplayMemberPath = "TipoContrato";
            if (TipoContratos.Count() > 1)
            {
                cmbTipoContratos.SelectedValue = TipoContratos.Where(x => x.IsDefault == true).FirstOrDefault().TipoContratoID;
            }
            else
            {
                cmbTipoContratos.SelectedValue = -1;
            }           
           

        }

        private void frmContratos_Loaded(object sender, RoutedEventArgs e)
        {

            LoadCombox();
            txtFechaContrato.SelectedDate = DateTime.Now;
        
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
