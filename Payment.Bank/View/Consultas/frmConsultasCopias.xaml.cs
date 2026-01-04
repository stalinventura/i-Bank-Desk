using MahApps.Metro.Controls;
using Payment.Bank.Controles;
using Payment.Bank.Core.Common;
using Payment.Bank.Core.Model;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using Payment.Bank.View.Formularios;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Payment.Bank.View.Consultas
{
    /// <summary>
    /// Interaction logic for frmConsultasCopias.xaml
    /// </summary>
    public partial class frmConsultasCopias : MetroWindow
    {
        Bank.Core.Manager db = new Core.Manager();
        int VISTA = 1;
        Copias BE = new Copias();
        public frmConsultasCopias()
        {
            InitializeComponent();
            Title = clsLenguajeBO.Find(Title.ToString());
            clsLenguajeBO.Load(gridMenu);
            btnSalir.Click += btnSalir_Click;
            Loaded += frmConsultasCopias_Loaded;
            dataGrid1.MouseDoubleClick += dataGrid1_MouseDoubleClick;
            txtBuscar.KeyUp += txtBuscar_KeyUp;
            txtBuscar.Focus();
            btnEditar.Click += btnEditar_Click;
            btnDelete.Click += btnDelete_Click;
            btnAgregar.Click += btnAgregar_Click;
        }

        void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.CopiaID > 0)
                {
                    if (clsVariablesBO.Permiso.Modificar == true)
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                    else
                    {
                        clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsVariablesBO.Permiso.Agregar == true)
                {
                    VISTA = 1;
                    frmBackUp Search = new frmBackUp();
                    Visibility = Visibility.Hidden;
                    Search.Closed += (obj, args) =>
                    {
                        BE = new Copias(); dataGrid1.SelectedIndex = -1;
                        Find();
                        Visibility = Visibility.Visible;
                    };
                    Search.Owner = this;
                    Search.ShowDialog();
                }
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("msgPermission"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BE.CopiaID > 0)
                {
                    if (clsVariablesBO.Permiso.Eliminar == true)
                    {
                        frmMessageBox Mensaje = new frmMessageBox();
                        Mensaje.OnInit(clsLenguajeBO.Find("msgConfirm") + "?", clsLenguajeBO.Find("msgTitle"), frmMessageBox.MessageType.Alert);
                        Mensaje.Title = clsLenguajeBO.Find("msgTitle");
                        Mensaje.btnAceptar.Click += (obj, args) =>
                        {
                            clsFtp ftp = new clsFtp();
                            OperationResult result = ftp.DeleteFile(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, BE.Copia);
                            if (result.ResponseCode == "00")
                            {
                                clsMessage.InfoMessage(clsLenguajeBO.Find("msgDelete"), clsLenguajeBO.Find("msgTitle"));
                            }
                            else
                            {
                                clsMessage.ErrorMessage(result.ResponseMessage, clsLenguajeBO.Find("msgTitle"));
                            }
                            Find();
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
                else
                {
                    clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle"));
                }
            }
            catch { clsMessage.ErrorMessage(clsLenguajeBO.Find("itemSelect"), clsLenguajeBO.Find("msgTitle")); }
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                clsFtp ftp = new clsFtp();
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.Filter = "Zip Files (*.zip)|*.zip";
                saveFileDialog.FileName = BE.Copia;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ftp.DownloadFile(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, BE.Copia, saveFileDialog.FileName);
                }
            }
            catch { }
        }

        private void frmConsultasCopias_Loaded(object sender, RoutedEventArgs e)
        {
            Find();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BE = (Copias)dataGrid1.SelectedItem;
            }
            catch { }
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Find();
                }
        }
            catch { }
        }

        public void Find()
        {
            try
            {
                var Result = new clsFtp().GetFiles(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID);
                dataGrid1.ItemsSource = Result.ToList();
                lblTotal.Text = clsLenguajeBO.Find(lblTotal.Name.ToString()) + " " + String.Format("{0:N}", Result.Count.ToString());
            }
            catch (Exception Ex) { clsMessage.ErrorMessage(Ex.Message, clsLenguajeBO.Find("msgTitle")); }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
