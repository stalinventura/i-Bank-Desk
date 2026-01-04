
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Payment.Bank.DAL;

namespace Payment.Bank.Core.Model
{
    public class clsCobroRutasView : RoadReport
    {
        
        private RoadReport ReportObject;
         
        public clsCobroRutasView() : base()
        {

        }

        public void SetDataSource(int InstitucionID, DateTime Fecha)
        {

            DateTime _Fecha = Fecha.Add(new TimeSpan(23, 59, 59));

            db = new DAL.Context();
           ReportObject = new RoadReport();
            IEnumerable<RoadView> _Cuotas;
            IEnumerable<RoadView> _DetalleNotaDebitos;
            IEnumerable<RoadView> _DetalleNotaCreditos;
            IEnumerable<RoadView> _DetalleRecibos;

            //_Cuotas = from C in db.CuotasGet().Where(x => x.Vence <= _Fecha && x.Contratos.Solicitudes.Clientes.Personas.DatosEconomicos.InstitucionID == InstitucionID) select new RoadView { ContratoID = C.ContratoID, Fecha = C.Vence, Numero = C.Numero, Completo = C.Contratos.Solicitudes.Clientes.Personas.Nombres + " " + C.Contratos.Solicitudes.Clientes.Personas.Nombres, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal, Atraso = 0, Monto = C.Contratos.Cuota, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal };
            //_DetalleNotaDebitos = from ND in db.DetalleNotaDebitosGet(null).Where(x => x.Cuotas.Vence <= _Fecha && x.Cuotas.Contratos.Solicitudes.Clientes.Personas.DatosEconomicos.InstitucionID == InstitucionID) select new RoadView { ContratoID = ND.Cuotas.ContratoID, Fecha = ND.Cuotas.Vence, Numero = ND.Cuotas.Numero, Completo = ND.Cuotas.Contratos.Solicitudes.Clientes.Personas.Nombres + " " + ND.Cuotas.Contratos.Solicitudes.Clientes.Personas.Nombres, Capital = ND.Capital, Comision = ND.Comision, Interes = ND.Interes, Mora = ND.Mora, Legal = ND.Legal, Atraso = 0, Monto = ND.Cuotas.Contratos.Cuota, Balance = ND.Capital + ND.Comision + ND.Interes + ND.Mora + ND.Legal };
            //_DetalleNotaCreditos = from NC in db.DetalleNotaCreditosGet(null).Where(x => x.Cuotas.Vence <= _Fecha && x.Cuotas.Contratos.Solicitudes.Clientes.Personas.DatosEconomicos.InstitucionID == InstitucionID) select new RoadView { ContratoID = NC.Cuotas.ContratoID, Fecha = NC.Cuotas.Vence, Numero = NC.Cuotas.Numero, Completo = NC.Cuotas.Contratos.Solicitudes.Clientes.Personas.Nombres + " " + NC.Cuotas.Contratos.Solicitudes.Clientes.Personas.Nombres, Capital = NC.Capital, Comision = NC.Comision, Interes = NC.Interes, Mora = NC.Mora, Legal = NC.Legal, Atraso = 0, Monto = NC.Cuotas.Contratos.Cuota, Balance = NC.Capital + NC.Comision + NC.Interes + NC.Mora + NC.Legal };                        
            //_DetalleRecibos = from DR in db.DetalleRecibosGet().Where(x => x.Recibos.Fecha <= _Fecha && x.Recibos.EstadoID == true && x.Recibos.Contratos.Solicitudes.Clientes.Personas.DatosEconomicos.InstitucionID == InstitucionID) select new RoadView { ContratoID = DR.Recibos.ContratoID, Fecha = DR.Recibos.Fecha, Numero = DR.Numero, Completo = DR.Recibos.Contratos.Solicitudes.Clientes.Personas.Nombres + " " + DR.Recibos.Contratos.Solicitudes.Clientes.Personas.Nombres, Capital = DR.Capital, Comision = DR.Comision, Interes = DR.Interes, Mora = DR.Mora, Legal = DR.Legal, Atraso = 0, Monto = DR.Recibos.Contratos.Cuota, Balance = DR.Capital + DR.Comision + DR.Interes + DR.Mora + DR.Legal };

            _Cuotas = from C in db.clsCuotasBE join Co in db.clsContratosBE on C.ContratoID equals Co.ContratoID join S in db.clsSolicitudesBE on Co.SolicitudID equals S.SolicitudID join Ci in db.clsClientesBE on S.ClienteID equals Ci.ClienteID join P in db.clsPersonasBE on Ci.Documento equals P.Documento join De in db.clsDatosEconomicosBE on P.Documento equals De.Documento where De.InstitucionID == InstitucionID && Co.EstadoID ==1 select new RoadView { ContratoID = C.ContratoID, Fecha = C.Vence, Numero = C.Numero, Completo = P.Nombres + " " + P.Apellidos, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal, Monto = Co.Cuota, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal, Cuota = Co.Cuota };
            _DetalleNotaDebitos = from ND in db.clsDetalleNotaDebitosBE join C in db.clsCuotasBE on ND.CuotaID equals C.CuotaID join Co in db.clsContratosBE on C.ContratoID equals Co.ContratoID join S in db.clsSolicitudesBE on Co.SolicitudID equals S.SolicitudID join Ci in db.clsClientesBE on S.ClienteID equals Ci.ClienteID join P in db.clsPersonasBE on Ci.Documento equals P.Documento join De in db.clsDatosEconomicosBE on P.Documento equals De.Documento where De.InstitucionID == InstitucionID && Co.EstadoID == 1 select new RoadView { ContratoID = C.ContratoID, Fecha = C.Vence, Numero = C.Numero, Completo = P.Nombres + " " + P.Apellidos, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal,   Monto = Co.Cuota, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal, Cuota = Co.Cuota };
            _DetalleNotaCreditos = from NC in db.clsDetalleNotaCreditosBE join C in db.clsCuotasBE on NC.CuotaID equals C.CuotaID join Co in db.clsContratosBE on C.ContratoID equals Co.ContratoID join S in db.clsSolicitudesBE on Co.SolicitudID equals S.SolicitudID join Ci in db.clsClientesBE on S.ClienteID equals Ci.ClienteID join P in db.clsPersonasBE on Ci.Documento equals P.Documento join De in db.clsDatosEconomicosBE on P.Documento equals De.Documento where De.InstitucionID == InstitucionID && Co.EstadoID == 1 select new RoadView { ContratoID = C.ContratoID, Fecha = C.Vence, Numero = C.Numero, Completo = P.Nombres + " " + P.Apellidos, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal,   Monto = Co.Cuota, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal, Cuota = Co.Cuota };
            _DetalleRecibos = from DR in db.clsDetalleRecibosBE.Where(x=>x.Recibos.EstadoID == true) join C in db.clsCuotasBE on DR.CuotaID equals C.CuotaID join Co in db.clsContratosBE on C.ContratoID equals Co.ContratoID join S in db.clsSolicitudesBE on Co.SolicitudID equals S.SolicitudID join Ci in db.clsClientesBE on S.ClienteID equals Ci.ClienteID join P in db.clsPersonasBE on Ci.Documento equals P.Documento join De in db.clsDatosEconomicosBE on P.Documento equals De.Documento where De.InstitucionID == InstitucionID && Co.EstadoID == 1 select new RoadView { ContratoID = C.ContratoID, Fecha = C.Vence, Numero = C.Numero, Completo = P.Nombres + " " + P.Apellidos, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal, Monto = Co.Cuota, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal, Cuota = Co.Cuota };


            var R1 = from Cuota in _Cuotas
                     select new RoadReportItem
                     {
                         ContratoID = Cuota.ContratoID,
                         Fecha = Cuota.Fecha,
                         Numero = Cuota.Numero,
                         Completo = Cuota.Completo,
                         Monto = Cuota.Monto,
                         Capital = Cuota.Capital,
                         Comision= Cuota.Comision,
                         Interes = Cuota.Interes,
                         Mora = Cuota.Mora,
                         Legal = Cuota.Legal,
                         Balance = Cuota.Balance                        
                     };
            if (R1 != null)
            {
                ReportObject.AddRange(R1);
            }

            var R2 = from DetalleNotaDebitos in _DetalleNotaDebitos
                     select new RoadReportItem
                     {
                         ContratoID = DetalleNotaDebitos.ContratoID,
                         Fecha = DetalleNotaDebitos.Fecha,
                         Numero = DetalleNotaDebitos.Numero,
                         Completo = DetalleNotaDebitos.Completo,
                         Monto = DetalleNotaDebitos.Monto,
                         Capital = DetalleNotaDebitos.Capital,
                         Comision = DetalleNotaDebitos.Comision,
                         Interes = DetalleNotaDebitos.Interes,
                         Mora = DetalleNotaDebitos.Mora,
                         Legal = DetalleNotaDebitos.Legal,
                         Balance = DetalleNotaDebitos.Balance
                     };
            if (R2 != null)
            {
                ReportObject.AddRange(R2);
            }

            var R3 = from DetalleNotaCreditos in _DetalleNotaCreditos
                     select new RoadReportItem
                     {
                         ContratoID =  DetalleNotaCreditos.ContratoID,
                         Fecha = DetalleNotaCreditos.Fecha,
                         Numero = DetalleNotaCreditos.Numero,
                         Completo = DetalleNotaCreditos.Completo,
                         Monto = DetalleNotaCreditos.Monto,
                         Capital =  DetalleNotaCreditos.Capital *-1,
                         Comision = DetalleNotaCreditos.Comision * -1,
                         Interes = DetalleNotaCreditos.Interes * -1,
                         Mora = DetalleNotaCreditos.Mora * -1,
                         Legal = DetalleNotaCreditos.Legal * -1,
                         Balance = DetalleNotaCreditos.Balance
                     };
            if (R3 != null)
            {
                ReportObject.AddRange(R3);
            }

            var R4 = from DetalleRecibos in _DetalleRecibos
                     select new RoadReportItem
                     {
                         ContratoID = DetalleRecibos.ContratoID,
                         Fecha = DetalleRecibos.Fecha,
                         Numero = DetalleRecibos.Numero,
                         Completo = DetalleRecibos.Completo,
                         Monto = DetalleRecibos.Monto,
                         Capital = DetalleRecibos.Capital * -1,
                         Comision = DetalleRecibos.Comision * -1,
                         Interes = DetalleRecibos.Interes * -1,
                         Mora = DetalleRecibos.Mora * -1,
                         Legal = DetalleRecibos.Legal * -1,
                         Balance = DetalleRecibos.Balance
                     };
            if (R4 != null)
            {
                ReportObject.AddRange(R4);
            }

            this.AddRange(ReportObject);
        }


    }

    public interface IRoadReport<T>
    {
        IQueryable<T> GetGroup(); //Substituir Dictionary
    }

    public interface IRoadReportItem { }

    public abstract class GenericRoadReportItem : IRoadReportItem
    {

    }

    public class RoadReportItem : GenericRoadReportItem
    {
        public int ContratoID { get; set; }
        public DateTime Fecha { get; set; }
        public int Numero { get; set; }
        public string Completo { get; set; }
        public float Monto { get; set; }

        public float Capital { get; set; }
        public float Comision { get; set; }
        public float Interes { get; set; }
        public float Mora { get; set; }
        public float Legal { get; set; }
        public float Cuota { get; set; }
        public float Atraso {
            get
            {
                return Capital + Comision + Interes + Mora + Legal;
            }
        }
        public float Balance { get; set; }
    }

    public class RoadReport : List<RoadReportItem>, IRoadReport<RoadReportItem>
    {

        protected DAL.Context db;
        public RoadReport()
        {
            this.db = new DAL.Context();
            this.SubReporte = new List<RoadReport>();
        }
        public List<RoadReport> SubReporte { get; set; }

        public IQueryable<RoadReportItem> GetGroup()
        {

            var RptResume = from i in this.ToList()
                            where (i.Fecha <= DateTime.Now)
                            group i by new { i.ContratoID} into gr
                            select new RoadReportItem
                            {
                                ContratoID = gr.Key.ContratoID,
                                Fecha = gr.FirstOrDefault().Fecha,                            
                                Numero = gr.FirstOrDefault().Numero,
                                Completo = gr.FirstOrDefault().Completo,
                                Capital = gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0,
                                Comision = gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0,
                                Interes = gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0,
                                Mora = gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0,
                                Legal = gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0,
                                //Atraso = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0),
                                Cuota = gr.FirstOrDefault().Cuota,
                                Balance =  AtrasoGet(gr.Key.ContratoID)
                            };
            var Monitor = RptResume.AsQueryable<RoadReportItem>();
            return RptResume.AsQueryable<RoadReportItem>();
        }

        //public IQueryable<RoadReportItem> GetGroupWithOutMora()
        //{
        //    var RptResume = from i in this.ToList()
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        CuotaID = gr.Key.CuotaID,
        //                        ContratoID = gr.Key.ContratoID,
        //                        TipoContratoID = gr.FirstOrDefault().TipoContratoID,
        //                        Vence = gr.Key.Vence,
        //                        Numero = gr.Key.Numero,
        //                        Capital = gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0,
        //                        Comision = gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0,
        //                        Interes = gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0,
        //                        Mora = gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0,
        //                        Legal = gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0,
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0)
        //                    };
        //    var Monitor = RptResume.AsQueryable<RoadReportItem>();
        //    return RptResume.AsQueryable<RoadReportItem>();
        //}

        //public IQueryable<RoadReportItem> GetGroupWithOutInteres()
        //{
        //    var RptResume = from i in this.ToList()
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        CuotaID = gr.Key.CuotaID,
        //                        ContratoID = gr.Key.ContratoID,
        //                        TipoContratoID = gr.FirstOrDefault().TipoContratoID,
        //                        Vence = gr.Key.Vence,
        //                        Numero = gr.Key.Numero,
        //                        Capital = gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0,
        //                        Comision = gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0,
        //                        Interes = gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0,
        //                        Mora = gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0,
        //                        Legal = gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0,
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0 )
        //                    };
        //    var Monitor = RptResume.AsQueryable<RoadReportItem>();
        //    return RptResume.AsQueryable<RoadReportItem>();
        //}

        public float AtrasoGet(int ContratoID)
        {
            var RptResume = from i in this.ToList()
                            where (i.Fecha <= DateTime.Now) && (i.ContratoID == ContratoID)
                            group i by new { i.ContratoID, i.Numero, i.Fecha } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0)
                            };
            return RptResume.Sum(x => x.Balance);
        }

        //public IQueryable<RoadReportItem> VencidosGet()
        //{
        //    var RptResume = from i in this.ToList()
        //                    where (i.Vence <= DateTime.Now)
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0)
        //                    };
        //    return RptResume.AsQueryable<RoadReportItem>();
        //}

        //public int CantidadAtrasoGet(DateTime Hasta)
        //{
        //    var RptResume = from i in this.ToList()
        //                    where (i.Vence <= Hasta)
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0)
        //                    };
        //    return RptResume.Where(x=> x.Balance> 0).Count();
        //}

        //public float BalanceGetByReciboID()
        //{
        //    var RptResume = from i in this.ToList()
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0)
        //                    };
        //    return RptResume.Sum(x => x.Balance);
        //}

        //public float BalanceCapitalGetByReciboID()
        //{
        //    var RptResume = from i in this.ToList()
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0)
        //                    };
        //    return RptResume.Sum(x => x.Balance);
        //}

        //public float BalanceCapital()
        //{
        //    var RptResume = from i in this.ToList()
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0)
        //                    };
        //    return RptResume.Sum(x => x.Balance);
        //}

        //public float PagoMinimoGet()
        //{
        //    var RptResume = from i in this.ToList()
        //                    where (i.Vence <= DateTime.Now)
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0)
        //                    };
        //    return RptResume.Sum(x => x.Balance);
        //}

        //public float Balance()
        //{
        //    var RptResume = from i in this.ToList()
        //                    group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
        //                    select new RoadReportItem
        //                    {
        //                        Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0)
        //                    };
        //    return RptResume.Sum(x => x.Balance);
        //}
    }
    
    class RoadView
    {
        public int ContratoID { get; set; }
        public DateTime Fecha { get; set; }
        public int Numero { get; set; }
        public string Completo { get; set; }
        public float Monto { get; set; }

        public float Capital { get; set; }
        public float Comision { get; set; }
        public float Interes { get; set; }
        public float Mora { get; set; }
        public float Legal { get; set; }
        public float Cuota { get; set; }
        public float Atraso
        {
            get
            {
                return Capital + Comision + Interes + Mora + Legal;
            }
        }
        public float Balance { get; set; }

    }
}
