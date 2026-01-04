
using Payment.Bank.Core;
using Payment.Bank.Core.Model;
using Payment.Bank.Modulos;
using Payment.Bank.View.Informes.Reportes;
using Payment.Bank.View.Informes.tmp;
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
    /// Interaction logic for frmSterlizerForm.xaml
    /// </summary>
    public partial class frmPrintCheques : Window
    {
        Manager db = new Manager();
        String Dia = string.Empty;
        String Mes = string.Empty;
        String Año = string.Empty;
        string Nombres = string.Empty;
        string Concepto = string.Empty;
        float Monto = 0;
        int Banco = 0;

        public frmPrintCheques()
        {
            InitializeComponent();
            Loaded += frmPrintCuentasPorCobrar_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintCuentasPorCobrar_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                DataTable Cheques = new DataTable("Cheques");
                Cheques.Columns.Add("Dia", typeof(String));
                Cheques.Columns.Add("Mes", typeof(String));
                Cheques.Columns.Add("Año", typeof(String));
                Cheques.Columns.Add("Nombres", typeof(string));
                Cheques.Columns.Add("Concepto", typeof(string));
                Cheques.Columns.Add("Monto", typeof(float));

                DataRow dataRow = Cheques.NewRow();
                dataRow["Dia"] = Dia;
                dataRow["Mes"] = Mes;
                dataRow["Año"] = Año;
                dataRow["Nombres"] = Nombres;
                dataRow["Concepto"] = Concepto;
                dataRow["Monto"] = Monto;
                Cheques.Rows.Add(dataRow);

                ds.Tables.Add(Cheques);


                var bs = ds;
                if (bs.Tables[0].Rows.Count > 0)
                {
                    if (Banco == 1)
                    {
                        rptChequePopular info = new rptChequePopular();
                        info.SetDataSource(bs);
                        ReportViewer.ViewerCore.ReportSource = info;
                    }
                    else
                    {
                        rptChequeReservas info = new rptChequeReservas();
                        info.SetDataSource(bs);
                        ReportViewer.ViewerCore.ReportSource = info;
                    }

                }
                else
                {
                    rptEmpty Empty = new rptEmpty();
                    ReportViewer.ViewerCore.ReportSource = Empty;
                }
            }
            catch
            {
                rptErrorData error = new rptErrorData();
                ReportViewer.ViewerCore.ReportSource = error;
            }
        }


        public void OnInit(int banco, DateTime fecha, string nombres, string concepto, float monto)
        {
            Banco = banco;
            Dia = fecha.Day.ToString().Length == 1? "0"+ fecha.Day.ToString(): fecha.Day.ToString();
            Mes = fecha.Month.ToString().Length == 1 ? "0" + fecha.Month.ToString() : fecha.Month.ToString();
            Año = fecha.Year.ToString();
            Nombres = nombres;
            Concepto = concepto;
            Monto = monto;
        }

     
    }
}
