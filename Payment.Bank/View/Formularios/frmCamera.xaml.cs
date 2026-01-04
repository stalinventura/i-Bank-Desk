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
using System.ServiceModel;
using Payment.Bank.Controles;
using Microsoft.Win32;
using System.IO;
using System.Web;
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes;
using Payment.Bank.Properties;
using System.Threading;
using System.Windows.Threading;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmCamera : Window
    {
        clsCamera webcam = new clsCamera();
       public bool TakePhoto = false;
        public string photo = "cliente.jpg";
        public frmCamera()
        {
            InitializeComponent();
            clsLenguajeBO.Load(LayoutRoot);
            Loaded += frmCamera_Loaded;
            btnSalir.Click += btnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;


        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Capture();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
            Close();
        }

        private void frmCamera_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                webcam.InitializeWebCam(WebCamSrc);
                webcam.Start();
            }
            catch { }
        }

        private void Capture()
        {
            photoSrc.Source = WebCamSrc.Source;
            clsHelper.SaveImageCapture((BitmapSource)photoSrc.Source, photo);
            TakePhoto = true;
        }

    }
}