using System;
using System.Windows;


using Payment.Bank.Modulos;
using System.Windows.Threading;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmAvisoDeudas : Window
    {
        Core.Manager db = new Core.Manager();
        string Message = string.Empty;
        public frmAvisoDeudas()
        {
            InitializeComponent();
            clsLenguajeBO.Load(LayoutRoot);
            clsLenguajeBO.Load(gridDatosPersonales);
            Loaded += frmShutdown_Loaded;
            btnSalir.Click += btnSalir_Click;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void OnInit(string message)
        {
            Message = message;
        }

        private void frmShutdown_Loaded(object sender, RoutedEventArgs e)
        {
            lblMessage.Text = Message;
        }


    }
}