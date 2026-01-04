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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.Core;
using System.Drawing.Printing;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmImpresiones : MetroWindow
    {
        int VISTA = 1;
        clsImpresionesBE BE = new clsImpresionesBE();
        Core.Manager db = new Core.Manager();
        public frmImpresiones()
        {
            InitializeComponent(); 
            DataContext = new clsImpresionesBE();
            LoadCombox();
          
            clsLenguajeBO.Load(gridImpresiones);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmImpresiones_Loaded;
            cmbDispositivos.SelectionChanged += cmbDispositivos_SelectionChanged;
        }

        private void cmbDispositivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TipoImpresiones();
            }
            catch { }
        }

        private void frmImpresiones_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (VISTA == 1)
            {
                cmbDispositivos.SelectedValue = string.Empty;
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;
                cmbTipoImpresiones.SelectedValue = -1;
                cmbLocal.SelectedValue = string.Empty;
                cmbRed.SelectedValue = string.Empty;
            }
        }

        void LoadCombox()
        {
            try
            {

                cmbTipoImpresiones.ItemsSource = new List<clsTipoImpresionesBE>();

                List<clsTipoImpresionesBE> List = new List<clsTipoImpresionesBE>();
                List = db.TipoImpresionesGetNotTask(null);
                List.Add(new clsTipoImpresionesBE { TipoImpresionID = -1, Descripcion = clsLenguajeBO.Find("itemSelect") });
                if (VISTA == 2)
                {
                    List.Add(new clsTipoImpresionesBE { TipoImpresionID = BE.TipoImpresionID, Descripcion = BE.TipoImpresiones.Descripcion });
                }

                cmbTipoImpresiones.ItemsSource = List.OrderBy(x => x.Descripcion);
                cmbTipoImpresiones.SelectedValuePath = "TipoImpresionID";
                cmbTipoImpresiones.DisplayMemberPath = "Descripcion";
                if (VISTA == 1)
                {
                    cmbTipoImpresiones.SelectedValue = -1;
                }


                //Sucursales
                List<clsSucursalesBE> Sucursales = new List<clsSucursalesBE>();
                Sucursales = db.SucursalesGet(null).ToList();

                if (clsVariablesBO.UsuariosBE.Roles.IsAdmin == true)
                {
                    cmbSucursales.IsEnabled = true;
                    //List.Add(new clsSucursalesBE { SucursalID = -1, Sucursal = clsLenguajeBO.Find("itemAll") });
                }
                else
                {
                    cmbSucursales.IsEnabled = false;
                }
                cmbSucursales.ItemsSource = Sucursales;
                cmbSucursales.SelectedValuePath = "SucursalID";
                cmbSucursales.DisplayMemberPath = "Sucursal";
                cmbSucursales.SelectedValue = clsVariablesBO.UsuariosBE.SucursalID;


                List<clsDispositivosBE> dispositivos = new List<clsDispositivosBE>();
                dispositivos = db.DispositivosGet(null);
                dispositivos.Add(new clsDispositivosBE { DispositivoID = string.Empty, Dispositivo = clsLenguajeBO.Find("itemSelect") });

                cmbDispositivos.ItemsSource = dispositivos;
                cmbDispositivos.SelectedValuePath = "DispositivoID";
                cmbDispositivos.DisplayMemberPath = "Dispositivo";
                cmbDispositivos.SelectedValue = string.Empty;


                List<Model.Dispositivos> impresoras = new List<Model.Dispositivos>();
                foreach(var printer in PrinterSettings.InstalledPrinters)
                {
                    impresoras.Add(new Model.Dispositivos { DispositivoID =  printer.ToString(), Dispositivo = printer.ToString() }); ;
                }

                impresoras.Add(new Model.Dispositivos { DispositivoID = string.Empty,  Dispositivo = clsLenguajeBO.Find("itemSelect") });

                cmbLocal.ItemsSource = impresoras;
                cmbLocal.SelectedValuePath = "DispositivoID";
                cmbLocal.DisplayMemberPath = "Dispositivo";
                cmbLocal.SelectedValue = string.Empty;

                cmbRed.ItemsSource = impresoras;
                cmbRed.SelectedValuePath = "DispositivoID";
                cmbRed.DisplayMemberPath = "Dispositivo";
                cmbRed.SelectedValue = string.Empty;

            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        void TipoImpresiones()
        {
            try {
                cmbTipoImpresiones.ItemsSource = new List<clsTipoImpresionesBE>();

                List<clsTipoImpresionesBE> List = new List<clsTipoImpresionesBE>();
                List = db.TipoImpresionesGetNotTask(null, (string)cmbDispositivos.SelectedValue);
                List.Add(new clsTipoImpresionesBE { TipoImpresionID = -1, Descripcion = clsLenguajeBO.Find("itemSelect") });
                if (VISTA == 2)
                {
                    if (List.Where(x => x.TipoImpresionID == BE.TipoImpresionID).Count() == 0)
                    {
                        List.Add(new clsTipoImpresionesBE { TipoImpresionID = BE.TipoImpresionID, Descripcion = BE.TipoImpresiones.Descripcion });
                    }
                }

                cmbTipoImpresiones.ItemsSource = List.OrderBy(x => x.Descripcion);
                cmbTipoImpresiones.SelectedValuePath = "TipoImpresionID";
                cmbTipoImpresiones.DisplayMemberPath = "Descripcion";
                if (VISTA == 1)
                {
                    cmbTipoImpresiones.SelectedValue = -1;
                }
                else
                {
                    cmbTipoImpresiones.SelectedValue = BE.TipoImpresionID;
                }

            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VISTA == 1 && clsValidacionesBO.Validar(gridImpresiones))
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        OperationResult result = db.ImpresionesCreate((int)cmbSucursales.SelectedValue, (int)cmbTipoImpresiones.SelectedValue, (string)cmbDispositivos.SelectedValue, txtPapel.Text, (string)cmbLocal.SelectedValue, (string)cmbRed.SelectedValue, int.Parse(txtCopy.Text), float.Parse(txtAncho.Text), float.Parse(txtAlto.Text), clsVariablesBO.UsuariosBE.Documento);
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
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridImpresiones))
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            OperationResult result = db.ImpresionesUpdate(BE.ImpresionID, (int)cmbSucursales.SelectedValue, (int)cmbTipoImpresiones.SelectedValue, (string)cmbDispositivos.SelectedValue, txtPapel.Text, (string)cmbLocal.SelectedValue, (string)cmbRed.SelectedValue, int.Parse(txtCopy.Text), float.Parse(txtAncho.Text), float.Parse(txtAlto.Text), clsVariablesBO.UsuariosBE.Documento);
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
            clsValidacionesBO.Limpiar(gridImpresiones);
            VISTA = 1;
            LoadCombox();

        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsImpresionesBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                    cmbTipoImpresiones.SelectedValue = be.TipoImpresionID;
                }

            }
            catch { }
        }
    }
}
