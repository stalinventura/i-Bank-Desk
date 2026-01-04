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
using Payment.Bank.Modulos;
using Payment.Bank.Entity;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmReferencias : MetroWindow
    {
        int _ReferenciaID;
        int _VISTA = 1;
        public List<clsReferenciasBE> Referencias = new List<clsReferenciasBE>();
        Core.Manager db = new Core.Manager();

        public frmReferencias()
        {
            InitializeComponent();
            Loaded += frmContactos_Loaded;
 
            clsLenguajeBO.Load(gridReferenciasPersonales);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());
            

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
        }

        void LoadCombox()
        {
            try
            {
                //DataContext = new clsReferenciasBE();
                List<clsTipoReferenciasBE> List = new List<clsTipoReferenciasBE>();
                List = db.TipoReferenciasGet(null).ToList();
                List.Add(new clsTipoReferenciasBE { TipoReferenciaID = -1, TipoReferencia = clsLenguajeBO.Find("itemSelect") });
                cmbTipoReferencias.ItemsSource = List;
                cmbTipoReferencias.SelectedValuePath = "TipoReferenciaID";
                cmbTipoReferencias.DisplayMemberPath = "TipoReferencia";
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmContactos_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new clsReferenciasBE { TipoReferenciaID = -1 };
            LoadCombox();
        }

        public void OnInit(clsReferenciasBE _be, int VISTA)
        {
            try
            {
                _VISTA = VISTA;
                if (_VISTA == 2)
                {
                    LoadCombox();
                    gridReferenciasPersonales.DataContext = _be;
                    _ReferenciaID = _be.ReferenciaID;
                }
            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_VISTA == 1 && clsValidacionesBO.Validar(gridReferenciasPersonales))
                {
                    clsReferenciasBE BE = new clsReferenciasBE();
                    BE.ReferenciaID = 1; //Referencias.Count + 1;
                    BE.Documento = "049-0065028-6";
                    BE.TipoReferenciaID = (int)cmbTipoReferencias.SelectedValue;
                    BE.TipoReferencias = new clsTipoReferenciasBE { TipoReferenciaID = (int)cmbTipoReferencias.SelectedValue, Fecha = DateTime.Now, FechaModificacion = DateTime.Now, IsDefault = true, Usuario = clsVariablesBO.UsuariosBE.Documento, ModificadoPor = clsVariablesBO.UsuariosBE.Documento, TipoReferencia = cmbTipoReferencias.Text };
                    BE.Fecha = DateTime.Now;
                    BE.Referencia = txtNombres.Text;
                    BE.Direccion = txtDireccion.Text;
                    BE.Telefono = txtTelefono.Text;
                    BE.Usuario = clsVariablesBO.UsuariosBE.Documento;
                    BE.ModificadoPor = clsVariablesBO.UsuariosBE.Documento;
                    BE.FechaModificacion = DateTime.Now;

                    Referencias.Add(BE);
                    clsValidacionesBO.Limpiar(gridReferenciasPersonales);
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                }
                else
                {
                    if (_VISTA == 2 && clsValidacionesBO.Validar(gridReferenciasPersonales))
                    {
                        List<clsReferenciasBE> Contactos = new List<clsReferenciasBE>();
                        foreach (clsReferenciasBE _BE in Referencias)
                        {
                            if (_ReferenciaID == _BE.ReferenciaID)
                            {
                                _BE.TipoReferenciaID = (int)cmbTipoReferencias.SelectedValue;
                                _BE.Referencia = txtNombres.Text;
                                _BE.Direccion = txtDireccion.Text;
                                _BE.Telefono = txtTelefono.Text;
                            }
                            Contactos.Add(_BE);
                        }
                        Referencias.Clear();
                        Referencias = Contactos;
                        clsValidacionesBO.Limpiar(gridReferenciasPersonales);
                        clsMessage.InfoMessage(clsLenguajeBO.Find("msgUpdate"), clsLenguajeBO.Find("msgTitle"));
                    }
                }
            }
            catch(Exception ex) { clsMessage.ErrorMessage(ex.Message, ""); }
        }

      
        private void txtTelefono_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txtTelefono_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                String Phone = txtTelefono.Text;
                txtTelefono.Text = clsValidacionesBO.PhoneFormat(Phone);
            }
            catch { }
        }

    }
}
