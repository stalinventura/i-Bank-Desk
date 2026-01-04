using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Consultas;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes;
using System.Text.RegularExpressions;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmNominas.xaml
    /// </summary>
    public partial class frmNominas : MetroWindow
    {
        int VISTA = 1;

        clsNominasBE nominasBE = new clsNominasBE();
        List<clsDetalleNominasBE> detalleNominasBE = new List<clsDetalleNominasBE>();
        clsDetalleNominasBE BE = new clsDetalleNominasBE();

        Core.Manager db = new Core.Manager();
        public frmNominas()
        {
            InitializeComponent();
            LoadComboxBancos();
            LoadCombox();

            clsLenguajeBO.Load(gridEncabezado);
            clsLenguajeBO.Load(gridDetalleCuotas);
            clsLenguajeBO.Load(gridMenu);
            clsLenguajeBO.Load(gridOpciones);
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridInformacion);
            clsLenguajeBO.Load(gridMode);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            btnDelete.Click += btnDelete_Click;
            btnAdd.Click += btnAdd_Click;
            dataGrid1.SelectionChanged += dataGrid1_SelectionChanged;

            cmbEmpleado.SelectionChanged += cmbEmpleado_SelectionChanged;
            chkDobleSueldo.Checked += ChkDobleSueldo_Checked;
            chkDobleSueldo.Unchecked += ChkDobleSueldo_Unchecked;
            rbMensual.Checked += rbMensual_Checked;
            rbQuincenal.Checked += rbQuincenal_Checked;

            txtDescuento.TextChanged += txtDescuento_TextChanged;
            txtDescuento.KeyDown += txtDescuento_KeyDown;
            txtDescuento.LostFocus += txtDescuento_LostFocus;
            txtFecha.SelectedDateChanged += txtFecha_SelectedDateChanged;
            txtComision.TextChanged += txtComision_TextChanged;
            Loaded += frmNominas_Loaded;

        }

        private void rbQuincenal_Checked(object sender, RoutedEventArgs e)
        {
            getDescription();
            CalcularRubros();
        }

        private void rbMensual_Checked(object sender, RoutedEventArgs e)
        {
            getDescription();
            CalcularRubros();
        }

        private void ChkDobleSueldo_Unchecked(object sender, RoutedEventArgs e)
        {
            rbMensual.IsEnabled = true;
            rbQuincenal.IsEnabled = true;
            getDescription();
            CalcularRubros();
        }

        private void ChkDobleSueldo_Checked(object sender, RoutedEventArgs e)
        {
            rbMensual.IsChecked = true;
            rbMensual.IsEnabled = false;
            rbQuincenal.IsEnabled = false;
            getDescription();
            CalcularRubros();
        }

        private void txtComision_TextChanged(object sender, TextChangedEventArgs e)
        {
           try
            {
                if ((int)cmbEmpleado.SelectedValue > 0 &&  double.Parse(txtSueldo.Text) > 0)
                {
                    CalcularRubros();
                }
            }
            catch { }
        }

        private void cmbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          try
            {
                CalcularRubros();
            }
            catch(Exception ex)
            { txtSueldo.Text = string.Format("{0:N2}", 0); }
        }


        void CalcularRubros()
        {
            try
            {

                bool dobleSueldo = (bool)chkDobleSueldo.IsChecked;
                bool Modalidad = dobleSueldo == true ? true : (bool)rbMensual.IsChecked;

                double sueldo = Modalidad == true ? double.Parse((cmbEmpleado.SelectedItem as clsEmpleadosBE).Sueldo.ToString()) : double.Parse((cmbEmpleado.SelectedItem as clsEmpleadosBE).Sueldo.ToString()) / 2;

                txtSueldo.Text = string.Format("{0:N2}", sueldo);
                double comision = double.Parse(txtComision.Text);

                txtSfs.Text = string.Format("{0:N2}", 0);
                txtAfp.Text = string.Format("{0:N2}", 0);
                txtIsr.Text = string.Format("{0:N2}", 0);
                txtOtros.Text = string.Format("{0:N2}", 0);

                txtSfs.Text = string.Format("{0:N2}", dobleSueldo == true || (txtCodigo.Text.EndsWith("15") && rbQuincenal.IsChecked == true) ? 0 : ((Modalidad == false ? (sueldo * 2) : sueldo) * 3.04 / 100));
                txtAfp.Text = string.Format("{0:N2}", dobleSueldo == true || (txtCodigo.Text.EndsWith("15") && rbQuincenal.IsChecked == true) ? 0 : ((Modalidad == false ? (sueldo * 2) : sueldo) * 2.87 / 100));

                Core.Manager db = new Core.Manager();
                List<clsDetalleNominasBE> tmp = new List<clsDetalleNominasBE>();
                foreach(var item in detalleNominasBE)
                {
                    float _salario = db.EmpleadosGetByEmpleadoID(item.EmpleadoID).Sueldo;
                    item.Sueldo = Modalidad == true ? _salario : _salario / 2;

                    item.SFS = 0;
                    item.AFP = 0;
                    //item.ISR = 0;
                    //item.Otros = 0;

                    item.SFS = float.Parse((dobleSueldo == true || (txtCodigo.Text.EndsWith("15") && rbQuincenal.IsChecked == true) ? 0 : ((Modalidad == false ? (item.Sueldo * 2) : item.Sueldo) * (float)3.04 / 100)).ToString());
                    item.AFP = float.Parse((dobleSueldo == true || (txtCodigo.Text.EndsWith("15") && rbQuincenal.IsChecked == true) ? 0 : ((Modalidad == false ? (item.Sueldo * 2) : item.Sueldo) * (float)2.87 / 100)).ToString());

                    item.SubTotal = item.Sueldo + item.Comision - item.SFS - item.AFP - item.ISR - item.Otros;
                    tmp.Add(item);
                }

                dataGrid1.ItemsSource = new List<clsDetalleNominasBE>();
                dataGrid1.ItemsSource = tmp;
                CalcularTotales();

            }
            catch (Exception ex)
            {
                CalcularTotales();
                txtSueldo.Text = string.Format("{0:N2}", 0); }
        }

        private void txtFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            getDescription();
            CalcularRubros();
        }

        public void OnInit(clsNominasBE row, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                txtFecha.SelectedDate = row.Fecha;

                rbMensual.IsChecked = row.IsMensual == true ? true : false;
                rbQuincenal.IsChecked = row.IsMensual == true ? false : true;
                chkDobleSueldo.IsChecked = row.IsRegalia;
                cmbBancos.SelectedValue = row.BancoID;
                nominasBE = row;
                gridEncabezado.DataContext = nominasBE;
                detalleNominasBE = row.DetalleNominas.ToList();
                dataGrid1.ItemsSource = detalleNominasBE;
                gridOpciones.DataContext = row;
                txtInformacion.Text = row.Informacion;
                getDescription();
                CalcularRubros();
                CalcularTotales();
                LoadCombox();
            }
            catch { }
        }

        void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridEncabezado) == true && clsValidacionesBO.Validar(gridOpciones) == true && detalleNominasBE != null)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                      
                        OperationResult result = db.NominasCreate(txtCodigo.Text, (DateTime)txtFecha.SelectedDate, txtNomina.Text, (bool)rbMensual.IsChecked, (bool)chkDobleSueldo.IsChecked, (int)cmbBancos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            DetalleNominasCreate(int.Parse(result.ResponseMessage));
                            //db.Contabilizar(2, Convert.ToInt32(result.ResponseMessage), clsVariablesBO.UsuariosBE.Documento);
                            ClearAll();
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridEncabezado) == true && clsValidacionesBO.Validar(gridOpciones) == true && detalleNominasBE != null)
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.NominasUpdate(nominasBE.NominaID, txtCodigo.Text, (DateTime)txtFecha.SelectedDate, txtNomina.Text, (bool)rbMensual.IsChecked, (bool)chkDobleSueldo.IsChecked, (int)cmbBancos.SelectedValue, float.Parse(txtSubTotal.Text), float.Parse(txtDescuento.Text), float.Parse(txtTotal.Text), txtInformacion.Text, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                DetalleNominasCreate(nominasBE.NominaID);
                                //db.Contabilizar(2, _ReciboID, clsVariablesBO.UsuariosBE.Documento);
                                ClearAll();
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
            catch (Exception sqlEx) { clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void DetalleNominasCreate(int NominaID)
        {
            try
            {
                db.DetalleNominasDeleteGetByNominaID(NominaID);
                OperationResult result = new OperationResult();
                foreach (clsDetalleNominasBE row in detalleNominasBE)
                {
                    result = db.DetalleNominasCreate(NominaID, row.EmpleadoID, row.Sueldo, row.Comision, row.SFS, row.AFP, row.ISR, row.Otros, row.SubTotal, clsVariablesBO.UsuariosBE.Documento);
                }

                if (result.ResponseCode == "00")
                {
                    if (chkPreview.IsChecked == true)
                    {
                        frmPrintNominasGetByNominaID Recibos = new frmPrintNominasGetByNominaID();
                        Recibos.Owner = this;
                        Recibos.OnInit(NominaID);
                        Recibos.ShowDialog();
                    }
                    else
                    {
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                    }
                    //NotificarPago(ReciboID, balance);
                }

            }
            catch { }
            }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (clsDetalleNominasBE)dataGrid1.SelectedItem;
            }
            catch { }
        }


        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsValidacionesBO.Validar(gridDetalleCuotas) == true && (int)cmbEmpleado.SelectedValue > 0)
                {
                    float SubTotal = 0;
                    SubTotal = float.Parse(txtSueldo.Text) + float.Parse(txtComision.Text) - float.Parse(txtSfs.Text) - float.Parse(txtIsr.Text) - float.Parse(txtAfp.Text) - float.Parse(txtOtros.Text);
                    detalleNominasBE.Add(new clsDetalleNominasBE
                    {
                        DetalleNominaID = detalleNominasBE.Count() + 1,
                        NominaID = int.Parse(txtCodigo.Text),
                        EmpleadoID = (int)cmbEmpleado.SelectedValue,
                        Sueldo = float.Parse(txtSueldo.Text),
                        Comision = float.Parse(txtComision.Text),
                        Fecha = (DateTime)txtFecha.SelectedDate,
                        SFS = float.Parse(txtSfs.Text),
                        ISR = float.Parse(txtIsr.Text),
                        AFP = float.Parse(txtAfp.Text),
                        Otros = float.Parse(txtOtros.Text),
                        SubTotal = SubTotal,
                        Usuario=clsVariablesBO.UsuariosBE.Documento,
                        ModificadoPor = clsVariablesBO.UsuariosBE.Documento,
                        FechaModificacion = DateTime.Now,
                        Empleados = new clsEmpleadosBE { EmpleadoID = (int)cmbEmpleado.SelectedValue , Sueldo = float.Parse(txtSueldo.Text), Personas = new clsPersonasBE { Nombres = cmbEmpleado.Text} }
                    }) ;

                    if (SubTotal > 0)
                    {
                        dataGrid1.ItemsSource = new List<clsDetalleNominasBE>();
                        dataGrid1.ItemsSource = detalleNominasBE;
                        CalcularTotales();
                        LoadCombox();
                        clsValidacionesBO.Limpiar(gridDetalleCuotas);
                        gridDetalleCuotas.DataContext = new clsDetalleNominasBE() { EmpleadoID = -1 };
                        cmbEmpleado.Focus();
                    }
                }
            }
            catch(Exception ex) 
            { }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.Permiso.Eliminar == true)
                {
                    if (BE.DetalleNominaID > 0)
                    {
                        foreach (clsDetalleNominasBE fila in detalleNominasBE)
                        {
                            if (fila == dataGrid1.SelectedItem)
                            {
                                detalleNominasBE.Remove(fila);
                                dataGrid1.ItemsSource = null;//new List<clsDetalleNominasBE>();
                                dataGrid1.ItemsSource = detalleNominasBE;
                                CalcularTotales();
                                LoadCombox();
                            }
                        }
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }


        private void txtTotal_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                if (Convert.ToDouble(txtSubTotal.Text) > 0)
                {
                    e.Handled = false;
                }
                else
                {
                    if (e.Key == Key.Decimal)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                e.Handled = true;
            }

        }

        private void txtDescuento_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double subtotal = Convert.ToDouble(txtSubTotal.Text);
                double descuento = Convert.ToDouble(txtDescuento.Text);
                if (descuento > 0)
                {
                    if (descuento > Convert.ToDouble(txtSubTotal.Text))
                    {
                        txtDescuento.Text = subtotal.ToString();
                    }
                    CalcularTotales();
                }

            }
            catch { }
        }

        private void txtDescuento_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double descuento = Convert.ToDouble(txtDescuento.Text);
                txtDescuento.Text = string.Format("{0:N2}", descuento);
                CalcularTotales();
            }
            catch { }

        }

        private void CalcularTotales()
        {
            try
            {
                double SubTotal = 0, Descuento = 0;
                txtTotal.Text = string.Format("{0:N2}", 0);
                foreach (clsDetalleNominasBE Fila in detalleNominasBE)
                {
                    SubTotal = SubTotal + Convert.ToDouble(Fila.Sueldo) + Convert.ToDouble(Fila.Comision);
                    Descuento= Descuento + Convert.ToDouble(Fila.SFS) + Convert.ToDouble(Fila.AFP) + Convert.ToDouble(Fila.ISR) + Convert.ToDouble(Fila.Otros);
                }             
                txtSubTotal.Text = string.Format("{0:N2}", SubTotal);
                txtDescuento.Text = string.Format("{0:N2}", Descuento);
                txtTotal.Text = string.Format("{0:N2}", SubTotal - Descuento);
            }
            catch (Exception ex)
            { }
        }


        void ClearAll()
        {
            try
            {
                LimpiarEncabezado();
                LimpiarDetalleRecibo();
                LimpiarOpciones();
                getDescription();
                LoadCombox();
                gridDetalleCuotas.DataContext = new clsDetalleNominasBE() {EmpleadoID =-1 };
                VISTA = 1;
            }
            catch { }
        }
        void LimpiarEncabezado()
        {
            try
            {

                //lblFecha.Text = string.Empty;
                //lblFechaVencimiento.Text = string.Empty;
                //lblMonto.Text = string.Empty;
                //lblInteres.Text = string.Empty;
                ////lblMoras.Text = string.Empty;
                gridEncabezado.DataContext = new clsNominasBE { BancoID =-1};
                VISTA = 1;
                detalleNominasBE = new List<clsDetalleNominasBE>();

                //cuotasBE = new List<BoxReportItem>();
                clsValidacionesBO.Limpiar(gridEncabezado);
                clsValidacionesBO.Limpiar(gridDetalleCuotas);
                txtCodigo.Focus();
            }
            catch { }
        }

        void LimpiarDetalleRecibo()
        {
            try
            {
                detalleNominasBE = new List<clsDetalleNominasBE>();
                dataGrid1.ItemsSource = new List<clsDetalleNominasBE>();
            }
            catch { }
        }

        void LimpiarOpciones()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridOpciones);
                txtSubTotal.Text = string.Format("{0:N2}", 0);
                txtDescuento.Text = string.Format("{0:N2}", 0);
                txtTotal.Text = string.Format("{0:N2}", 0);
            }
            catch { }
        }

        string MonthName(int MesID)
        {
            string Mes = string.Empty;
            switch(MesID)
            {
                case 1:
                    { Mes = "ENERO";}break;
                case 2:
                    { Mes = "FEBREO"; }
                    break;
                case 3:
                    { Mes = "MARZO"; }
                    break;
                case 4:
                    { Mes = "ABRIL"; }
                    break;
                case 5:
                    { Mes = "MAYO"; }
                    break;
                case 6:
                    { Mes = "JUNIO"; }
                    break;
                case 7:
                    { Mes = "JULIO"; }
                    break;
                case 8:
                    { Mes = "AGOSTO"; }
                    break;
                case 9:
                    { Mes = "SEPTIEMBRE"; }
                    break;
                case 10:
                    { Mes = "OCTUBRE"; }
                    break;
                case 11:
                    { Mes = "NOVIEMBRE"; }
                    break;
                case 12:
                    { Mes = "DICIEMBRE"; }
                    break;
            }
            return Mes;
        }

        private void frmNominas_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

               
                gridOpciones.DataContext = new clsNominasBE();
                gridDetalleCuotas.DataContext = new clsDetalleNominasBE() { EmpleadoID =-1};
                if (VISTA == 1)
                {
                    gridEncabezado.DataContext = new clsNominasBE { BancoID = -1 };
                    txtFecha.SelectedDate = DateTime.Today;
                    txtInformacion.Text = string.IsNullOrEmpty(clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Informacion) == true ? "" : clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.Informacion;
                }
                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == false)
                {
                    txtFecha.IsEnabled = false;
                    txtDescuento.IsReadOnly = true;
                }
                else
                { CalcularTotales(); }
                getDescription();
            }
            catch { }
        }

        void getDescription()
        {
            txtCodigo.Text = $"{txtFecha.SelectedDate.Value.Year}{string.Format("{0:00}", txtFecha.SelectedDate.Value.Month)}{(txtFecha.SelectedDate.Value.Day <= 15 ? 15 : DateTime.DaysInMonth(txtFecha.SelectedDate.Value.Year, txtFecha.SelectedDate.Value.Month))}";
            txtNomina.Text = (bool)chkDobleSueldo.IsChecked == false ? (bool)rbMensual.IsChecked == true ? $"NOMINA MES DE {MonthName(txtFecha.SelectedDate.Value.Month)} DEL {txtFecha.SelectedDate.Value.Year}" : $"NOMINA {(txtFecha.SelectedDate.Value.Day <= 15 ? "PRIMERA" : "SEGUNDA")} QUINCENA DEL MES DE {MonthName(txtFecha.SelectedDate.Value.Month)} DEL {txtFecha.SelectedDate.Value.Year}" : $"NOMINA REGALIA PASCUAL CORRESPONDIENTE A {MonthName(txtFecha.SelectedDate.Value.Month)} DEL {txtFecha.SelectedDate.Value.Year}";
        }

        private void LoadComboxBancos()
        {
            try
            {
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

        private void LoadCombox()
        {

            //Empleados
            List<clsEmpleadosBE> empleados = new List<clsEmpleadosBE>();
            empleados = db.EmpleadosGet(null).Where(x => x.EstadoID == true).ToList();
            empleados.Add(new clsEmpleadosBE { EmpleadoID = -1, Personas = new clsPersonasBE { Nombres = clsLenguajeBO.Find("itemSelect") } });

            List<clsEmpleadosBE> ListOfUser = new List<clsEmpleadosBE>();
            foreach (var row in empleados)
            {
                bool existe = false;
                foreach (var e in detalleNominasBE)
                {
                    if (e.EmpleadoID == row.EmpleadoID)
                    {
                        existe = true;
                    }
                }
                if (!existe)
                {
                    ListOfUser.Add(new clsEmpleadosBE { EmpleadoID = row.EmpleadoID, Documento = row.Documento, Personas = new clsPersonasBE { Nombres = row.Personas.Nombres + " " + row.Personas.Apellidos }, Sueldo = row.Sueldo });
                }
            }

            cmbEmpleado.ItemsSource = ListOfUser;
            cmbEmpleado.SelectedValuePath = "EmpleadoID";
            cmbEmpleado.DisplayMemberPath = "Personas.Nombres";
            cmbEmpleado.SelectedValue = -1;
        }


   

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



    }
}
