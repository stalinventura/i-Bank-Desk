using MahApps.Metro.Controls;
using Payment.Bank.Modulos;
using Payment.Bank.Controles;
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
using System.Windows.Shapes;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Payment.Bank.Properties;
using System.Windows.Threading;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmUpgradeProgress.xaml
    /// </summary>
    public partial class frmUpgradeProgress : Window
    {
        Core.Manager core = new Core.Manager();
        public frmUpgradeProgress()
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
                Timer.Interval = new TimeSpan(0, 0, 5);
                Timer.Tick += (s, a) =>
                {
                    Timer.Stop();
                    ProcessAsync();
                };
                Timer.Start();


                }
                catch (Exception ex)
                {
                    clsMessage.ErrorMessage(ex.Message, "Mensaje");
                }
        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Configure the ProgressBar
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = short.MaxValue;
            ProgressBar.Value = 0;

            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar.SetValue);




            //Tight Loop:  Loop until the ProgressBar.Value reaches the max
            do
            {
                value  = e.ProgressPercentage;

                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });
                lblProgress.Text = value.ToString();

            }
            while (ProgressBar.Value != ProgressBar.Maximum);
            Close();
        }

        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private async Task ProcessAsync()
        {
            try
            {

                using (var httpClient = new HttpClient())
                {
                    var fileUrl = "http://softarch.ddns.net/i-bank-update/i-bank.zip";
                    using (var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "i-bank.zip");
                        using (var ms = await response.Content.ReadAsStreamAsync())
                        using (var fs = File.Create(filePath))
                        {
                            await ms.CopyToAsync(fs);
                            fs.Flush();
                        }
                    }
                }


                if (File.Exists(@".\i-bank.msi")) { File.Delete(@".\i-bank.msi"); }
                string zipPath = @".\i-bank.zip";
                string extractPath = @".\";
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                Process process = new Process();
                process.StartInfo.FileName = "msiexec.exe";
                process.StartInfo.Arguments = string.Format($"/i i-bank.msi");

                process.Start();
                Environment.Exit(0);
            }
            catch(Exception ex) 
            { }
        }




      
    }
}
