using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Entity;


using Payment.Bank.Modulos;
using Microsoft.Win32;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmEmpresas : MetroWindow
    {
        int VISTA = 1;

        //Valiables Temporales

        clsEmpresasBE BE = new clsEmpresasBE();

        OpenFileDialog openFileDialog = new OpenFileDialog();

        Core.Manager db = new Core.Manager();
        public frmEmpresas()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridSucursales);

            if (VISTA == 1)
            {
                LoadCombox();
                gridSucursales.DataContext = new clsEmpresasBE() { TipoEmpresaID =-1 };
             
            }

            btnSalir.Click += btnSalir_Click;
            Loaded += frmSucursales_Loaded;
  
            btnAceptar.Click += ttnAceptar_Click;
            btnAdd_srcPhoto.Click += btnAdd_srcPhoto_Click;
        }



        private void btnAdd_srcPhoto_Click(object sender, RoutedEventArgs e)
        {
            try {
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.png *.jpg *.bmp) |*.png; *.jpg; *.bmp|All Files(*.*) |*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (openFileDialog.ShowDialog() == true)
                {                    
                    //srcPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                    BE.Logo = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                }
            }
            catch { }
        }

        public void OnInit(String EmpresaID, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                LoadCombox();
                BE= db.EmpresasGetByEmpresaID(EmpresaID);
                
                //Sucursal
                gridSucursales.DataContext = BE;
            }
            catch { }
        }


        void ClearAll()
        {
            try
            {
                clsValidacionesBO.Limpiar(gridSucursales);
                //srcPhoto.Source = null;
                BE = new clsEmpresasBE();
                VISTA = 1;

            }
            catch { }
        }


        private void ttnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {                                                                                                                                                               // color = (n.Difference1 < 0) ? Negativo : (n.Difference1 > 0) ? Positivo : Balanceado,                                                            
                if (VISTA == 1 && clsValidacionesBO.Validar(gridSucursales) == true)
                {
                    if (clsVariablesBO.Permiso.Agregar == true)
                    {
                        byte[] data = null;
                        if (!string.IsNullOrEmpty(openFileDialog.FileName))
                        { 
                             data = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                        }

                        OperationResult result = db.EmpresasCreate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, txtEmpresa.Text, txtRnc.Text, txtSiglas.Text, null, data, txtCsr.Text, Common.FingerPrint.GetHash(txtDevices.Text), 0, (int)cmbTipoEmpresas.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                        if (result.ResponseCode == "00")
                        {
                            //Aqui va el mensaje final
                            clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
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
                else
                {
                    if (VISTA == 2 && clsValidacionesBO.Validar(gridSucursales) == true)
                    {
                        if (clsVariablesBO.Permiso.Modificar == true)
                        {
                            byte[] data = null;
                            if (!string.IsNullOrEmpty(openFileDialog.FileName))
                            {
                                data = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                            }

                            OperationResult result = db.EmpresasUpdate(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, txtEmpresa.Text, txtRnc.Text, txtSiglas.Text, BE.Key, data, txtCsr.Text, Common.FingerPrint.GetHash(txtDevices.Text), (int)BE.PlanID, (int)cmbTipoEmpresas.SelectedValue, clsVariablesBO.UsuariosBE.Documento);
                            if (result.ResponseCode == "00")
                            {
                                //Aqui va el mensaje final
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
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
            catch (Exception sqlEx) { RadBusyIndicator.IsActive = false; clsMessage.ErrorMessage(sqlEx.Message, clsLenguajeBO.Find("msgTitle")); }
        }

        private void txtTelefonoTrabajo_KeyDown(object sender, KeyEventArgs e)
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

        private void txtCelular_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTelefono_KeyDown(object sender, KeyEventArgs e)
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

        private void txtDocumento_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTelefonoTrabajo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //String Phone = txtTelefonoTrabajo.Text;
                //txtTelefonoTrabajo.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void txtCelular_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //String Phone = txtCelular.Text;
                //txtCelular.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        private void txtTelefono_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //String Phone = txtTelefono.Text;
                //txtTelefono.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

        void SavePhoto(string numero, byte[] data)
        {
            try
            {
                if (data.Length > 0)
                {
                    db.FotografiasCreate(numero, data, clsVariablesBO.UsuariosBE.Documento);
                }
            }
            catch { }
        }



        private void LoadCombox()
        {
          
            //Ciudades
            List<clsTipoEmpresasBE> TipoEmpresas = new List<clsTipoEmpresasBE>();
            TipoEmpresas = db.TipoEmpresasGet(null).ToList();
            TipoEmpresas.Add(new clsTipoEmpresasBE { TipoEmpresaID = -1, TipoEmpresa = clsLenguajeBO.Find("itemSelect") });
            cmbTipoEmpresas.ItemsSource = TipoEmpresas;
            cmbTipoEmpresas.SelectedValuePath = "TipoEmpresaID";
            cmbTipoEmpresas.DisplayMemberPath = "TipoEmpresa";
            cmbTipoEmpresas.SelectedValue = 0;
        }


        private void frmSucursales_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
