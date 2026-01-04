using Payment.Bank.Modulos;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmAbout.xaml
    /// </summary>
    public partial class frmAbout : Window
    {
        Core.Manager core = new Core.Manager();
        public frmAbout()
        {
            InitializeComponent();
            Loaded += frmAbout_Loaded;

            btnSalir.Click += BtnSalir_Click;
            btnAceptar.Click += btnAceptar_Click;
            clsLenguajeBO.Load(gridLogin);
            Title = clsLenguajeBO.Find(Title.ToString());
        }

       
        private void frmAbout_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode;
                encoder.TryEncode(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, out qrCode);
                //encoder.TryEncode("BD3C-892E-010C-5E51-6DFB-9F8B-9C7F-95DC1", out qrCode);
                WriteableBitmapRenderer wRenderer = new WriteableBitmapRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Colors.Black, Colors.White);
                WriteableBitmap wBitmap = new WriteableBitmap(66, 66, 96, 96, PixelFormats.Gray8, null);
                wRenderer.Draw(wBitmap, qrCode.Matrix);
                
                QrCodeImage.Source = wBitmap;
            }
            catch { }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
   
        }




        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch { }
        }

    }
}
