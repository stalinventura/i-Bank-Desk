
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
    /// Interaction logic for frmPrintCuotasGetByContratoID.xaml
    /// </summary>
    public partial class frmPrintCuotasGetByContratoID : Window
    {
        Manager db = new Manager();
        int ContratoID = 0;

        public frmPrintCuotasGetByContratoID()
        {
            InitializeComponent();
            Loaded += frmPrintRecibosGetByContratoID_Loaded;
            btnSalir.Click += btnSalir_Click;    
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void frmPrintRecibosGetByContratoID_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var bs = db.PrintCuotasGetByContratoID(ContratoID);
                if (bs != null)
                {
                    rptCuotasGetByContratoID info = new rptCuotasGetByContratoID();
                    info.SetDataSource(bs);
                    ReportViewer.ViewerCore.ReportSource = info;
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


        public DataSet EntityToDataSet<TEntity>(DataSet ds, List<TEntity> MyEntities)
        {
            string strTableName = null;
            DataRow drNewRow = default(DataRow);
            foreach (var POCO in MyEntities)
            {              
                dynamic EntityFields = POCO.GetType().GetProperties().Where(a => a.CanRead);
                strTableName = "Recibos";// POCO.GetType().Name;
                drNewRow = ds.Tables[strTableName].NewRow();
                foreach (var field in EntityFields)
                {                                                               
                    if (drNewRow.Table.Columns.Contains(field.Name))
                    {
                        drNewRow[field.Name] = field.GetValue(POCO, null);
                    }
                }
                ds.Tables[strTableName].Rows.Add(drNewRow);
            }
            return ds;
        }


        public DataTable EntityToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }


        public void OnInit(int _ContratoID)
        {
            ContratoID = _ContratoID;
        }

     
    }
}
