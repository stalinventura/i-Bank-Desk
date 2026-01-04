using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Payment.Bank.Model;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmConsultasCuotas : Window
    {
        public DateTime Fecha = DateTime.Now;
        Core.Manager db = new Core.Manager();
        public float Monto = 0;
        float PagoMinimo = 0;
        double Riesgo = 0;
        public List<BoxReportItem> cuotasBE = new List<BoxReportItem>();
        public BoxReportItem BE = new BoxReportItem();
        List<BoxReportItem> result = new List<BoxReportItem>();
        clsContratosBE row = new clsContratosBE();
        clsCuotasView Cuotas = new clsCuotasView();

        bool IsLate = false;
        bool Vencido = false;
        
        public frmConsultasCuotas()
        {
            InitializeComponent();
            try
            {
                gridPanel.Visibility = clsVariablesBO.IsCheckPrevious == true ? Visibility.Visible : Visibility.Collapsed;
                clsLenguajeBO.Load(gridTitulo);
                Title = clsLenguajeBO.Find(Title.ToString());
                btnAceptar.Click += btnAceptar_Click;
                btnSalir.Click += btnSalir_Click;
                txtMonto.KeyDown += txtMonto_KeyDown;
                dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;
                btnNotaDebito.Click += btnNotaDebito_Click;
                btnNotaCredito.Click += btnNotaCredito_Click;
                btnPrint.Click += btnPrint_Click;
                btnWhatsApp.Click += btnWhatsApp_Click;
            }
            catch{}
        }


        private void btnWhatsApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Cuotas.FirstOrDefault().ContratoID > 0)
                {
                    var c = db.ContratosGetByContratoID(Cuotas.FirstOrDefault().ContratoID);

                    string Celular = c.Solicitudes.Clientes.Personas.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ","");

                    if (Celular.Length == 10 && Celular != "0000000000")
                    {
                        string text = string.Empty;
                        if (IsLate)
                        {
                            text = $"Estimado(a) *{c.Solicitudes.Clientes.Personas.Nombres}*, por este medio le informamos que usted tiene pendiente un monto en atraso de *RD${ string.Format("{0:N2}", Cuotas.AtrasoGet(Fecha))}*. Favor realizar su pago lo antes posible, evite cargos. Cordialmente, *{clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres} {clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos}* de *{ c.Solicitudes.Clientes.Sucursales.Empresas.Empresa.Replace("&", "y")}*";
                        }
                        else
                        {
                            var row = result.Where(x=>x.Balance>0).FirstOrDefault();
                            text = $"Estimado(a) *{c.Solicitudes.Clientes.Personas.Nombres}*, por este medio le recordamos que la cuota numero *{row.Numero}* de su prestamo *{c.Solicitudes.TipoSolicitudes.TipoSolicitud}* vence el *{string.Format("{0:dd/MM/yyyy}", row.Vence)}* por un monto de *RD${ string.Format("{0:N2}", row.Balance)}*. Favor realizar su pago, evite cargos. Cordialmente, *{clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres} {clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos}* de *{ c.Solicitudes.Clientes.Sucursales.Empresas.Empresa.Replace("&", "y")}*";
                        }

                        if(Vencido)
                        {
                            if (c.Vence.AddDays(c.DiasGracia) >= Fecha)
                            {
                                text = $"Estimado(a) *{c.Solicitudes.Clientes.Personas.Nombres}*, por este medio le informamos que su prestamo *{c.Solicitudes.TipoSolicitudes.TipoSolicitud}* numero *{string.Format("{0:000000}", c.ContratoID)}* vencio el *{string.Format("{0:dd/MM/yyyy}", c.Vence)}* y tiene pendiente un monto a la fecha de *RD${ string.Format("{0:N2}", Cuotas.AtrasoGet(Fecha))}*. Favor realizar su pago lo antes posible, evite cargos adicionales. Cordialmente, *{clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres} {clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos}* de *{ c.Solicitudes.Clientes.Sucursales.Empresas.Empresa.Replace("&", "y")}*";
                            }
                            else
                            {
                                text = $"Estimado(a) *{c.Solicitudes.Clientes.Personas.Nombres}*, por este medio le informamos que su prestamo *{c.Solicitudes.TipoSolicitudes.TipoSolicitud}* numero *{string.Format("{0:000000}", c.ContratoID)}* vencio el *{string.Format("{0:dd/MM/yyyy}", c.Vence)}* y tiene pendiente un monto a la fecha de *RD${ string.Format("{0:N2}", Cuotas.AtrasoGet(Fecha))}*. Ademas informarle que desde el dia *{string.Format("{0:dd/MM/yyyy}", c.Vence.AddDays(c.DiasGracia + 1))}*, nuestro *DEPARTAMENTO LEGAL* han sido apoderado de su caso. Le recomendamos solucionar su situacion financiera en nuestra empresa lo antes posible y evite cualquier eventualidad legal que pudiera perjudicar su imagen personal, familiar y laboral. Le esperamos a mas tardar el proximo *{string.Format("{0:dd/MM/yyyy}", DateTime.Now.AddDays(3))}*, Cordialmente, *{clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Nombres} {clsVariablesBO.UsuariosBE.Sucursales.Gerentes.Personas.Apellidos}* de *{ c.Solicitudes.Clientes.Sucursales.Empresas.Empresa.Replace("&", "y")}*";
                            }
                        }

                      
                        //Estimado(a) , por este medio le recordamos que la fecha de pago de su cuota vence el {} por un monto de *RD$ 
                        byte[] data = Encoding.Default.GetBytes(text);
                        string message = Encoding.UTF8.GetString(data);

                        try
                        {
                            var process = $"whatsapp://send?phone=+1{Celular}&text={message}";
                            Process.Start(process);

                            Task.Delay(5000);

                            var url = $"https://wa.me/+1{Celular}/?text={message}&type=phone_number&app_absent=0";
                            Process.Start(url);
                        }
                        catch
                        {
                            var url = $"https://wa.me/1{Celular}/?text={message}&type=phone_number&app_absent=0";
                            Process.Start(url);
                        }
                    }
                    else
                    {
                     clsMessage.ErrorMessage("Numero de teléfono no es valido!", "Mensaje");
                    }
                }
            }
            catch
            {

            }
        }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmPrintCuotasGetByContratoID informe = new frmPrintCuotasGetByContratoID();
                informe.Owner = this;
                informe.OnInit(int.Parse(lblContrato.Text));
                informe.ShowDialog();
            }
            catch { }
        }

        private void btnNotaCredito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.Balance >= 0 && BE.CuotaID > 0)
                {
                    frmDetalleNotaCreditos NC = new frmDetalleNotaCreditos();
                    NC.OnInit(BE, 1);
                    NC.Owner = this;
                    NC.Closed += (arg, obj) => { LoadData(BE.ContratoID); };
                    NC.ShowDialog();
                }
            }
            catch { }
        }

        private void btnNotaDebito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(BE.Balance >= 0 && BE.CuotaID > 0)
                {
                    frmDetalleNotaDebitos ND = new frmDetalleNotaDebitos();
                    ND.OnInit(BE, 1);
                    ND.Owner = this;
                    ND.Closed += (arg, obj) => { LoadData(BE.ContratoID); };
                    ND.ShowDialog();
                }
            }
            catch { }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           try
            {
                BE = (BoxReportItem)dataGrid1.SelectedItem;
                if (BE.Balance >= 0 && clsVariablesBO.UsuariosBE.Roles.IsAdmin == true )//&& BE.Balance >0)
                {
                    Habilitar();
                }
                else
                { DesHabilitar(); }
                txtMonto.Focus();
               
            }
            catch { }
        }

        private void Habilitar()
        {
            try
            {
                btnNotaDebito.Visibility = Visibility.Visible;
                btnNotaCredito.Visibility = Visibility.Visible;
            }
            catch { }
        }

        private void DesHabilitar()
        {
            try
            {
                btnNotaDebito.Visibility = Visibility.Collapsed;
                btnNotaCredito.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        private void txtMonto_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || Key.OemPeriod == e.Key || e.Key == Key.Tab || Key.Decimal == e.Key)
                { e.Handled = false; }
                else
                { e.Handled = true; }

                if (cuotasBE.FirstOrDefault().ContratoID > 0)
                {
                    switch (e.Key)
                    {
                        case Key.F1:
                            {
                                frmPrintEstadoCuentasGetByContratoID estados = new frmPrintEstadoCuentasGetByContratoID();
                                estados.Owner = this;
                                estados.OnInit(cuotasBE.FirstOrDefault().ContratoID);
                                estados.ShowDialog();
                            }
                            break;

                        case Key.F5:
                            {
                                LoadData(cuotasBE.FirstOrDefault().ContratoID);
                            }
                            break;
                    }
                }
            }
            catch { }

        }


        public async void OnInit(clsContratosBE BE, DateTime _date)
        {
            try
            {
                Fecha = _date;
                foreach(var x in db.TareasGet(null).OrderBy(a=>a.TipoTareaID))
                {
                    switch(x.TipoTareas.Codigo)
                    {
                        case "CALCULATEINTERESTS":
                            {
                               await db.CalcularInteres(Fecha, BE.ContratoID);
                            } break;
                        case "CALCULATELATEFEES":
                            {
                                await db.CalcularMoras(Fecha, BE.ContratoID);
                            }
                            break;
                        case "CALCULATELEGAL":
                            {
                               await db.CalcularLegal(Fecha, BE.ContratoID);
                            }
                            break;
                    }
                }

                clsLenguajeBO.Load(gridMenu);
                clsLenguajeBO.Load(LayoutRoot);
                gridDatosPersonales.DataContext = BE;
                gridPhotoSrc.DataContext = BE;

                LoadData(BE.ContratoID);
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void LoadData(int ContratoID)
        {
            try
            {
                Cuotas = new clsCuotasView();
                row = db.ContratosGetByContratoID(ContratoID);

                Riesgo = db.CalcularRiesgoGetByContratoID(ContratoID, Fecha);
                this.BorderPhotoSrc.DataContext = new RiesgoViewModel { Riesgo = Riesgo };
                lblRiesgo.Text = string.Format("{0:0.00}%", Riesgo);

                Cuotas.SetDataSource(ContratoID);
                result = Cuotas.GetGroup().OrderBy(x=>x.Numero).ToList();

                dataGrid1.ItemsSource = result.ToList();
                cuotasBE = result.ToList();
                lblAtraso.Text = clsLenguajeBO.Find("lblAtraso") + " " + string.Format("{0:N2}", Cuotas.AtrasoGet(Fecha));
                lblMinimumPayment.Text = clsLenguajeBO.Find("lblMinimumPayment") + " " + string.Format("{0:N2}", Cuotas.PagoMinimoGet());
                PagoMinimo = Cuotas.PagoMinimoGet();



                if (Cuotas.AtrasoGet(Fecha) > 0)
                {
                    IsLate = true;
                    //btnWhatsApp.Visibility = Visibility.Visible;
                }
                else
                {
                    IsLate = false;
                    //btnWhatsApp.Visibility = Visibility.Collapsed;
                }

                if(row.Vence < Fecha)
                {
                    Vencido = true;
                }
                else
                {
                    Vencido = false;
                }
            }
            catch { }
        }

        bool ValidarPago()
        {
            try
            {
                bool Continuar = false;
                float Valor = float.Parse(txtMonto.Text);
                if (Valor >= PagoMinimo)
                {
                    Continuar = true;
                }
                return Continuar;
            }
            catch { return false; }
        }

        void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float _Monto = float.Parse(txtMonto.Text);
                if (_Monto > 0)
                {
                    if (ValidarPago() == true)
                    {
                        Monto = _Monto;
                        Close();
                    }
                    else
                    {
                        if (row.TipoContratos.CanMinimumPay == true || clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                        {
                            Monto = _Monto;
                            Close();
                        }
                        else
                        {
                            clsMessage.ErrorMessage(clsLenguajeBO.Find("msgMinimumPayment") + string.Format("{0:C3}", PagoMinimo), clsLenguajeBO.Find("msgTitle"));
                        }
                    }                    
                }
            }
            catch { }
        }


        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }


}

