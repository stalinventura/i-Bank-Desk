using Payment.Bank.Modulos;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Payment.Bank
{
    /// <summary>
    /// Interaction logic for frmProgress.xaml
    /// </summary>
    public partial class frmProgress : Window
    {
        Core.Manager core = new Core.Manager();
        public frmProgress()
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
                    Process();
                };
                Timer.Start();
                
            }
            catch { }
        }

        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void Process()
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
                value += 10;

                /*Update the Value of the ProgressBar:
		          1)  Pass the "updatePbDelegate" delegate that points to the ProgressBar.SetValue method
			      2)  Set the DispatcherPriority to "Background"
				  3)  Pass an Object() Array containing the property to update (ProgressBar.ValueProperty) and the new value */
                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });
                lblProgress.Text = string.Format("{0:N2}",(value/ 32767)* 100) + "%";
                //if(value == 1000)
                //{ clsVariablesBO.Host.Load(); }
            }
            while (ProgressBar.Value != ProgressBar.Maximum);
            Close();
        }




      
    }
}
