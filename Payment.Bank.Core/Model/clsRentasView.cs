
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Payment.Bank.DAL;

namespace Payment.Bank.Core.Model
{
    public class clsRentasView : RentasBoxReport
    {
        
        private RentasBoxReport ReportObject;
         
        public clsRentasView() : base()
        {

        }

        public void SetDataSource(int AlquilerID =0, int PagoRentaID = 0)
        {

            db = new Core.Manager();
            DAL.Context cx = new DAL.Context();
            ReportObject = new RentasBoxReport();
            IEnumerable<RentasView> _Rentas;

            if (AlquilerID == 0)
            {
                _Rentas = from C in cx.clsRentasBE select new RentasView { RentaID = C.RentaID,  AlquilerID = C.AlquilerID, Fecha = C.Fecha, Concepto = C.Concepto, Monto = C.Monto};
            }
            else
            {
                _Rentas = from C in cx.clsRentasBE.Where(x=>x.AlquilerID == AlquilerID) select new RentasView { RentaID = C.RentaID, AlquilerID = C.AlquilerID, Fecha = C.Fecha, Concepto = C.Concepto, Monto = C.Monto };
            }



            IEnumerable<RentasView> _DetallePagoRentas;
            if (PagoRentaID == 0)
            {
                if (AlquilerID == 0)
                {
                    _DetallePagoRentas = from DR in cx.clsDetallePagoRentasBE.Where(x => x.PagoRentas.EstadoID == true) select new RentasView { RentaID = DR.RentaID, AlquilerID = DR.PagoRentas.AlquilerID, Fecha = DR.Rentas.Fecha, Concepto = DR.Rentas.Concepto, Monto = DR.Monto };
                }
                else
                {
                    _DetallePagoRentas = from DR in cx.clsDetallePagoRentasBE.Where(x => x.PagoRentas.EstadoID == true && x.PagoRentas.AlquilerID == AlquilerID) select new RentasView { RentaID = DR.RentaID, AlquilerID = DR.PagoRentas.AlquilerID, Fecha = DR.Rentas.Fecha, Concepto = DR.Rentas.Concepto, Monto = DR.Monto };
                }
            }
            else
            {
                _DetallePagoRentas = from DR in cx.clsDetallePagoRentasBE.Where(x => x.PagoRentas.EstadoID == true && x.PagoRentaID  <= PagoRentaID) select new RentasView { RentaID = DR.RentaID, AlquilerID = DR.PagoRentas.AlquilerID, Fecha = DR.Rentas.Fecha, Concepto = DR.Rentas.Concepto, Monto = DR.Monto };
            }

            var R1 = from Rentas in _Rentas
                     select new RentasBoxReportItem
                     {
                           AlquilerID = Rentas.AlquilerID,
                           RentaID = Rentas.RentaID,
                           Fecha = Rentas.Fecha,
                           Concepto = Rentas.Concepto,
                           Monto = Rentas.Monto
                     };
            if (R1 != null)
            {
                ReportObject.AddRange(R1);
            }

            var R2 = from DetallePagoRentas in _DetallePagoRentas
                     select new RentasBoxReportItem
                     {
                         AlquilerID = DetallePagoRentas.AlquilerID,
                         RentaID = DetallePagoRentas.RentaID,
                         Fecha = DetallePagoRentas.Fecha,
                         Concepto = DetallePagoRentas.Concepto,
                         Monto = DetallePagoRentas.Monto * -1
                     };
            if (R2 != null)
            {
                ReportObject.AddRange(R2);
            }



            this.AddRange(ReportObject);
        }


    }

    public interface IRentasBoxReport<T>
    {
        IQueryable<T> GetGroup(); //Substituir Dictionary
    }

    public interface IRentasBoxReportItem { }

    public abstract class GenericRentasReportItem : IRentasBoxReportItem
    {

    }

    public class RentasBoxReportItem : GenericReportItem
    {
        public int RentaID { get; set; }
        public int AlquilerID { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public float Monto { get; set; }
    }

    public class RentasBoxReport : List<RentasBoxReportItem>, IRentasBoxReport<RentasBoxReportItem>
    {

        protected Core.Manager db;
        public RentasBoxReport()
        {
            this.db = new Core.Manager();
            this.SubReporte = new List<RentasBoxReport>();
        }
        public List<RentasBoxReport> SubReporte { get; set; }

        public IQueryable<RentasBoxReportItem> GetGroup()
        {

            var RptResume = from i in this.ToList()
                            group i by new { i.AlquilerID, i.RentaID, i.Fecha } into gr
                            select new RentasBoxReportItem
                            {
                                RentaID = gr.Key.RentaID,
                                AlquilerID = gr.Key.AlquilerID,
                                Fecha = gr.Key.Fecha,
                                Concepto = gr.FirstOrDefault().Concepto,                  
                                Monto =   (gr.Sum(x => x.Monto) >= 0 ? gr.Sum(x => x.Monto) : 0)
                            };
            var Monitor = RptResume.AsQueryable<RentasBoxReportItem>();
            return RptResume.AsQueryable<RentasBoxReportItem>();
        }

        public float AtrasoGet()
        {
            var RptResume = from i in this.ToList() where (i.Fecha <= DateTime.Now)
                            group i by new { i.AlquilerID, i.RentaID, i.Fecha } into gr
                            select new RentasBoxReportItem
                            {
                                Monto = (gr.Sum(x => x.Monto) >= 0 ? gr.Sum(x => x.Monto) : 0)
                            };
            return RptResume.Sum(x=>x.Monto);
        }

        public IQueryable<RentasBoxReportItem> VencidosGet()
        {
            var RptResume = from i in this.ToList()
                            where (i.Fecha <= DateTime.Now)
                            group i by new { i.AlquilerID, i.RentaID, i.Fecha } into gr
                            select new RentasBoxReportItem
                            {
                                Monto = (gr.Sum(x => x.Monto) >= 0 ? gr.Sum(x => x.Monto) : 0) 
                            };
            return RptResume.AsQueryable<RentasBoxReportItem>();
        }

     

        public float BalanceGetByPagoRentaID(DateTime _fecha)
        {
            var RptResume = from i in this.Where(x=>x.Fecha <= _fecha).ToList()
                            group i by new { i.AlquilerID, i.RentaID, i.Fecha } into gr
                            select new RentasBoxReportItem
                            {
                                Monto = (gr.Sum(x => x.Monto) >= 0 ? gr.Sum(x => x.Monto) : 0)
                            };
            return RptResume.Sum(x => x.Monto);
        }

    }
    
    class RentasView
    {
        public int RentaID { get; set; }
        public int AlquilerID { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public float Monto { get; set; }        

    }
}
