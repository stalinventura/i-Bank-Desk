using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Payment.Bank.Modulos;
using Payment.Bank.Entity;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmDependientes.xaml
    /// </summary>
    public partial class frmDependientes : MetroWindow
    {
        int _DependienteID;
        public int _VISTA = 1;
        public List<clsDependientesBE> Dependientes = new List<clsDependientesBE>();
        Core.Manager db = new Core.Manager();

        public frmDependientes()
        {
            InitializeComponent();
           
 
            clsLenguajeBO.Load(gridDependientes);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

          
            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
         
            Loaded += frmContactos_Loaded;
        }



        void LoadCombox()
        {
            try
            {
                List<clsParentescosBE> List = new List<clsParentescosBE>();
                List = db.ParentescosGet(null).ToList();
                List.Add(new clsParentescosBE { ParentescoID = -1, Parentesco = clsLenguajeBO.Find("itemSelect") });
                cmbParentescos.ItemsSource = List;
                cmbParentescos.SelectedValuePath = "ParentescoID";
                cmbParentescos.DisplayMemberPath = "Parentesco";
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmContactos_Loaded(object sender, RoutedEventArgs e)
        {
            if (_VISTA == 1)
            {
                LoadCombox();
                DataContext = new clsDependientesBE { ParentescoID = -1 , FechaNacimiento = DateTime.Today};
            }
           
        }

        public void OnInit(clsDependientesBE _be, int VISTA)
        {
            try
            {
                _VISTA = VISTA;
                if (_VISTA == 2)
                {
                    LoadCombox();
                    _DependienteID = _be.DependienteID;
                    gridDependientes.DataContext = _be;
                    cmbParentescos.SelectedValue = _be.ParentescoID;
                }
            }catch{}
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_VISTA == 1 && clsValidacionesBO.Validar(gridDependientes))
                {
                    clsDependientesBE BE = new clsDependientesBE();
                    BE.DependienteID = Dependientes.Count + 1;
                    BE.Documento = clsVariablesBO.UsuariosBE.Documento;
                    BE.ParentescoID = (int)cmbParentescos.SelectedValue;
                    BE.FechaNacimiento = (DateTime)txtFechaNacimeinto.SelectedDate;
                    BE.Parentescos = new clsParentescosBE { ParentescoID = (int)cmbParentescos.SelectedValue, Fecha = DateTime.Now, FechaModificacion = DateTime.Now, Usuario = clsVariablesBO.UsuariosBE.Documento, ModificadoPor = clsVariablesBO.UsuariosBE.Documento, Parentesco = cmbParentescos.Text };
                    BE.Fecha = DateTime.Now;
                    BE.Dependiente = txtNombres.Text;
                    BE.Telefono = txtTelefono.Text; 
                    BE.Usuario = clsVariablesBO.UsuariosBE.Documento;
                    BE.ModificadoPor = clsVariablesBO.UsuariosBE.Documento;
                    BE.FechaModificacion = DateTime.Now;

                    Dependientes.Add(BE);
                    clsValidacionesBO.Limpiar(gridDependientes);
                    clsMessage.InfoMessage(clsLenguajeBO.Find("msgSuccess"), clsLenguajeBO.Find("msgTitle"));
                }
                else
                {
                    if (_VISTA == 2 && clsValidacionesBO.Validar(gridDependientes))
                    {
                        List<clsDependientesBE> Contactos = new List<clsDependientesBE>();
                        foreach (clsDependientesBE _BE in Dependientes)
                        {
                            if (_DependienteID == _BE.DependienteID)
                            {
                                _BE.ParentescoID = (int)cmbParentescos.SelectedValue;
                                _BE.FechaNacimiento = (DateTime)txtFechaNacimeinto.SelectedDate;
                                _BE.Dependiente = txtNombres.Text;
                                _BE.Telefono = txtTelefono.Text;
                            }
                            Contactos.Add(_BE);
                        }
                        Dependientes = new List<clsDependientesBE>();
                        Dependientes = Contactos;
                        //clsValidacionesBO.Limpiar(gridDependientes);
                        this.Close();
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
