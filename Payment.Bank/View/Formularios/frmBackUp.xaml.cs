using Payment.Bank.Modulos;
using System;
using System.Windows;
using System.Windows.Threading;
using Payment.Bank.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Payment.Bank.DAL;
using Payment.Bank.Core;

namespace Payment.Bank.View.Formularios
{
    /// <summary>
    /// Interaction logic for frmBackUp.xaml
    /// </summary>
    public partial class frmBackUp : Window
    {
        Core.Manager core = new Core.Manager();
        clsCopiasBE BE = new clsCopiasBE();
        string result = string.Empty;
        public frmBackUp()
        {
            InitializeComponent();
            Loaded += FrmLogin_Loaded;

            clsLenguajeBO.Load(gridLogin);
            Title = clsLenguajeBO.Find(Title.ToString());

        }

        private void FrmLogin_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DispatcherTimer Timer = new DispatcherTimer();
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Tick += (s, a) =>
                {
                    Timer.Stop();
                    SetBackUp();
                };
                Timer.Start();

            }
            catch (Exception ex) { Close(); }
        }

       private void SetBackUp()
       {          

            try
            {
                Manager db = new Manager();
                db.SetBackUpMySQL(clsVariablesBO.UsuariosBE.Sucursales.Empresas.Copias, true);

                //SaveFileDialog saveFileDialog = new SaveFileDialog();
                //saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //saveFileDialog.Filter = "Zip Files (*.zip)|*.zip";
                //saveFileDialog.FileName = "i-bank-17-11-2024-18-57-38-932.zip";
                //if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    ftp.DownloadFile(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, "i-bank-17-11-2024-18-57-38-932.zip", saveFileDialog.FileName);
                //}
                   // File.WriteAllText(saveFileDialog.FileName, txtEditor.Text);

               

                //BE = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Copias;
                //var result = core.RemoteBackUp(BE.Host, clsVariablesBO.UsuariosBE.SucursalID, clsVariablesBO.UsuariosBE.Sucursales.EmpresaID, Common.FingerPrint.GetKey());
                //for(int i=0; i<=100; i=i+10)
                //{
                //    lblProgress.Text = string.Format("{0:N2}", i) + "%";
                //    Task.Delay(100);
                //}

                //string data = Common.Generic.Decoding(result.Result);
                //Process.Start("IExplore.exe", BE.Host + $"/BackUp/{data}.zip");
                this.Close();
            }
            catch (Exception ex)
            {
                clsMessage.ErrorMessage(ex.Message, clsLenguajeBO.Find("msgTitle"));
                Close();
            }
        }

     //   private void BkpDBFull_PercentComplete(object sender, PercentCompleteEventArgs e)
     //   {

     //       double value =10 +e.Percent;
     //       UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar.SetValue);

     //       Dispatcher.Invoke(updatePbDelegate,
     //System.Windows.Threading.DispatcherPriority.Background,
     //new object[] { ProgressBar.ValueProperty, value });
     //       lblProgress.Text = string.Format("{0:N2}", value) + "%";
     //   }


        //private void Backup_Completed(object sender, ServerMessageEventArgs args)
        //{
        //    Process.Start("IExplore.exe", BE.Servidor + "/BackUp/"  + result);
        //    this.Close();
        //}


        //private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        //private void Process()
        //{
        //    //Configure the ProgressBar
        //    ProgressBar.Minimum = 0;
        //    ProgressBar.Maximum = short.MaxValue;
        //    ProgressBar.Value = 0;

        //    //Stores the value of the ProgressBar
        //    double value = 0;

        //    //Create a new instance of our ProgressBar Delegate that points
        //    //  to the ProgressBar's SetValue method.
        //    UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar.SetValue);




        //    //Tight Loop:  Loop until the ProgressBar.Value reaches the max
        //    do
        //    {
        //        value += 10;

        //       Dispatcher.Invoke(updatePbDelegate,
        //            System.Windows.Threading.DispatcherPriority.Background,
        //            new object[] { ProgressBar.ValueProperty, value });
        //        lblProgress.Text = string.Format("{0:N2}", (value / 32767) * 100) + "%";

        //    }
        //    while (ProgressBar.Value != ProgressBar.Maximum);
        //    Close();
        //}





    }
}
