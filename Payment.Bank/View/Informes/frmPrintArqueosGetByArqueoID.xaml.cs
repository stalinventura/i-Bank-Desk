
using Payment.Bank.Core;
using Payment.Bank.Core.Model;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace Payment.Bank.View.Informes
{
    /// <summary>
    /// Interaction logic for frmPrintArqueosGetByArqueoID.xaml
    /// </summary>
    public partial class frmPrintArqueosGetByArqueoID : Window
    {
        Manager db = new Manager();
        int ArqueoID = 0;

        public frmPrintArqueosGetByArqueoID()
        {
            InitializeComponent();
            Loaded += frmPrintSolicitudsGetByArqueoID_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintSolicitudsGetByArqueoID_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintArqueosGetByArqueoID(ArqueoID);
                if (bs != null)
                {
                    rptArqueosGetByTurnoID info = new rptArqueosGetByTurnoID();
                    info.SetDataSource(bs);
                    ReportViewer.ViewerCore.ReportSource = info;
                }
                else
                {
                    rptEmpty Empty = new rptEmpty();
                    ReportViewer.ViewerCore.ReportSource = Empty;
                }
            }
            catch(Exception ex)
            {
                Payment.Bank.Core.Common.clsMisc misc = new Payment.Bank.Core.Common.clsMisc();
                misc.SaveToEventLog(ex);
                rptErrorData error = new rptErrorData();
                ReportViewer.ViewerCore.ReportSource = error;
            }
        }



        public void OnInit(int _ArqueoID)
        {
            ArqueoID = _ArqueoID;
        }

     
    }
}
