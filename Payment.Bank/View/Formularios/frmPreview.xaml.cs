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

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmPreview.xaml
    /// </summary>
    public partial class frmPreview : MetroWindow
    {
        int VISTA = 1;
        clsTareasBE BE = new clsTareasBE();
        Core.Manager db = new Core.Manager();

        public bool Preview = true;
        public int From = 1;
        public int To = 1;
        public int Copy = 1;

        public frmPreview()
        {
            InitializeComponent();
            DataContext = new clsTareasBE();
            clsLenguajeBO.Load(gridTareas);
            clsLenguajeBO.Load(LayoutRoot);
            Title = clsLenguajeBO.Find(Title.ToString());

            btnAceptar.Click += btnAceptar_Click;
            btnSalir.Click += btnSalir_Click;
            Loaded += frmPreview_Loaded;
        }

        private void frmPreview_Loaded(object sender, RoutedEventArgs e)
        {
            txtHasta.Text = To.ToString();
            txtCopias.Text = Copy.ToString();
        }



        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (optImpresora.IsChecked == true)
                {
                    Preview = false;
                }
                else
                {
                    Preview = true;
                }
                From = Convert.ToInt16(txtDesde.Text);
                To = Convert.ToInt16(txtHasta.Text);
                Copy = Convert.ToInt16(txtCopias.Text);
                Close();
            }
            catch (Exception ex) { clsMessage.ErrorMessage(clsLenguajeBO.Find(ex.Message), clsLenguajeBO.Find("msgTitle")); }
        }

        private void Limpiar()
        {
            VISTA = 1;
            clsValidacionesBO.Limpiar(gridTareas);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(clsTareasBE be, int _VISTA)
        {
            try
            {
                VISTA = _VISTA;
                if (VISTA == 2)
                {
                    BE = be;
                    DataContext = BE;
                }

            }
            catch { }
        }
    }
}
