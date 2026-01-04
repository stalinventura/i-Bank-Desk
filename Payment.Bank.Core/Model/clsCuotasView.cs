
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Payment.Bank.DAL;

namespace Payment.Bank.Core.Model
{
    public class clsCuotasView : BoxReport
    {

        private BoxReport ReportObject;

        public clsCuotasView() : base()
        {

        }

        public void SetDataSource(int ContratoID = 0, int ReciboID = 0)
        {

            db = new Core.Manager();
            DAL.Context cx = new DAL.Context();
            ReportObject = new BoxReport();
            IEnumerable<CuotasView> _Cuotas;
            IEnumerable<CuotasView> _DetalleNotaDebitos;
            IEnumerable<CuotasView> _DetalleNotaCreditos;

            if (ContratoID == 0)
            {
                _Cuotas = from C in db.CuotasGet() select new CuotasView { CuotaID = C.CuotaID, TipoContratoID = C.Contratos.TipoContratoID, ContratoID = C.ContratoID, Numero = C.Numero, Vence = C.Vence, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal, Seguro = C.Seguro, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal + C.Seguro, IsComputed = C.IsComputed, ContinueOnTime = C.Contratos.TipoContratos.ContinueOnTime, Tmp = C.Balance };
            }
            else
            {
                _Cuotas = from C in cx.clsCuotasBE.Where(x => x.ContratoID == ContratoID) select new CuotasView { CuotaID = C.CuotaID, TipoContratoID = C.Contratos.TipoContratoID, ContratoID = C.ContratoID, Numero = C.Numero, Vence = C.Vence, Capital = C.Capital, Comision = C.Comision, Interes = C.Interes, Mora = C.Mora, Legal = C.Legal, Seguro = C.Seguro, Balance = C.Capital + C.Comision + C.Interes + C.Mora + C.Legal + C.Seguro, IsComputed = C.IsComputed, ContinueOnTime = C.Contratos.TipoContratos.ContinueOnTime, Tmp = C.Balance };
            }

            if (ContratoID == 0)
            {
                _DetalleNotaDebitos = from ND in db.DetalleNotaDebitosGet(null) select new CuotasView { CuotaID = ND.CuotaID, TipoContratoID = ND.Cuotas.Contratos.TipoContratoID, ContratoID = ND.Cuotas.ContratoID, Numero = ND.Cuotas.Numero, Vence = ND.Cuotas.Vence, Capital = ND.Capital, Comision = ND.Comision, Interes = ND.Interes, Mora = ND.Mora, Legal = ND.Legal, Seguro = ND.Seguro, Balance = ND.Capital + ND.Comision + ND.Interes + ND.Mora + ND.Legal + ND.Seguro, IsComputed = ND.Cuotas.IsComputed, ContinueOnTime = ND.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = ND.Cuotas.Balance };
            }
            else
            {
                _DetalleNotaDebitos = from ND in cx.clsDetalleNotaDebitosBE.Where(x => x.Cuotas.ContratoID == ContratoID) select new CuotasView { CuotaID = ND.CuotaID, TipoContratoID = ND.Cuotas.Contratos.TipoContratoID, ContratoID = ND.Cuotas.ContratoID, Numero = ND.Cuotas.Numero, Vence = ND.Cuotas.Vence, Capital = ND.Capital, Comision = ND.Comision, Interes = ND.Interes, Mora = ND.Mora, Legal = ND.Legal, Seguro = ND.Seguro, Balance = ND.Capital + ND.Comision + ND.Interes + ND.Mora + ND.Legal + ND.Seguro, IsComputed = ND.Cuotas.IsComputed, ContinueOnTime = ND.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = ND.Cuotas.Balance };
            }


            if (ContratoID == 0)
            {

                _DetalleNotaCreditos = from NC in db.DetalleNotaCreditosGet(null) select new CuotasView { CuotaID = NC.CuotaID, TipoContratoID = NC.Cuotas.Contratos.TipoContratoID, ContratoID = NC.Cuotas.ContratoID, Numero = NC.Cuotas.Numero, Vence = NC.Cuotas.Vence, Capital = NC.Capital, Comision = NC.Comision, Interes = NC.Interes, Mora = NC.Mora, Legal = NC.Legal, Seguro = NC.Seguro, Balance = NC.Capital + NC.Comision + NC.Interes + NC.Mora + NC.Legal + NC.Seguro, IsComputed = NC.Cuotas.IsComputed, ContinueOnTime = NC.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = NC.Cuotas.Balance };
            }
            else
            {
                _DetalleNotaCreditos = from NC in cx.clsDetalleNotaCreditosBE.Where(x => x.Cuotas.ContratoID == ContratoID) select new CuotasView { CuotaID = NC.CuotaID, TipoContratoID = NC.Cuotas.Contratos.TipoContratoID, ContratoID = NC.Cuotas.ContratoID, Numero = NC.Cuotas.Numero, Vence = NC.Cuotas.Vence, Capital = NC.Capital, Comision = NC.Comision, Interes = NC.Interes, Mora = NC.Mora, Legal = NC.Legal, Seguro = NC.Seguro, Balance = NC.Capital + NC.Comision + NC.Interes + NC.Mora + NC.Legal + NC.Seguro, IsComputed = NC.Cuotas.IsComputed, ContinueOnTime = NC.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = NC.Cuotas.Balance };
            }

            IEnumerable<CuotasView> _DetalleRecibos;
            if (ReciboID == 0)
            {
                if (ContratoID == 0)
                {
                    _DetalleRecibos = from DR in db.DetalleRecibosGet().Where(x => x.Recibos.EstadoID == true) select new CuotasView { CuotaID = DR.CuotaID, ContratoID = DR.Recibos.ContratoID, Numero = DR.Cuotas.Numero, Vence = DR.Cuotas.Vence, Capital = DR.Capital, Comision = DR.Comision, Interes = DR.Interes, Mora = DR.Mora, Legal = DR.Legal, Seguro = DR.Seguro, Balance = DR.Capital + DR.Comision + DR.Interes + DR.Mora + DR.Legal + DR.Seguro, IsComputed = DR.Cuotas.IsComputed, ContinueOnTime = DR.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = DR.Cuotas.Balance };
                }
                else
                {
                    _DetalleRecibos = from DR in cx.clsDetalleRecibosBE.Where(x => x.Recibos.EstadoID == true && x.Recibos.ContratoID == ContratoID) select new CuotasView { CuotaID = DR.CuotaID, ContratoID = DR.Recibos.ContratoID, Numero = DR.Cuotas.Numero, Vence = DR.Cuotas.Vence, Capital = DR.Capital, Comision = DR.Comision, Interes = DR.Interes, Mora = DR.Mora, Legal = DR.Legal, Seguro = DR.Seguro, Balance = DR.Capital + DR.Comision + DR.Interes + DR.Mora + DR.Legal + DR.Seguro, IsComputed = DR.Cuotas.IsComputed, ContinueOnTime = DR.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = DR.Cuotas.Balance };
                }
            }
            else
            {
                _DetalleRecibos = from DR in db.DetalleRecibosGetByContratoID(ContratoID).Where(x => x.Recibos.EstadoID == true && x.ReciboID <= ReciboID) select new CuotasView { CuotaID = DR.CuotaID, ContratoID = DR.Recibos.ContratoID, Numero = DR.Cuotas.Numero, Vence = DR.Cuotas.Vence, Capital = DR.Capital, Comision = DR.Comision, Interes = DR.Interes, Mora = DR.Mora, Legal = DR.Legal, Seguro = DR.Seguro, Balance = DR.Capital + DR.Comision + DR.Interes + DR.Mora + DR.Legal + DR.Seguro, IsComputed = DR.Cuotas.IsComputed, ContinueOnTime = DR.Cuotas.Contratos.TipoContratos.ContinueOnTime, Tmp = DR.Cuotas.Balance };
            }

            var R1 = from Cuota in _Cuotas
                     select new BoxReportItem
                     {
                         CuotaID = Cuota.CuotaID,
                         ContratoID = Cuota.ContratoID,
                         Vence = Cuota.Vence,
                         Numero = Cuota.Numero,
                         Capital = Cuota.Capital,
                         Comision = Cuota.Comision,
                         Interes = Cuota.Interes,
                         Mora = Cuota.Mora,
                         Legal = Cuota.Legal,
                         Seguro = Cuota.Seguro,
                         TipoContratoID = Cuota.TipoContratoID,
                         IsComputed = Cuota.IsComputed,
                         ContinueOnTime = Cuota.ContinueOnTime,
                         Tmp = Cuota.Tmp

                     };
            if (R1 != null)
            {
                ReportObject.AddRange(R1);
            }

            var R2 = from DetalleNotaDebitos in _DetalleNotaDebitos
                     select new BoxReportItem
                     {
                         CuotaID = DetalleNotaDebitos.CuotaID,
                         ContratoID = DetalleNotaDebitos.ContratoID,
                         Vence = DetalleNotaDebitos.Vence,
                         Numero = DetalleNotaDebitos.Numero,
                         Capital = DetalleNotaDebitos.Capital,
                         Comision = DetalleNotaDebitos.Comision,
                         Interes = DetalleNotaDebitos.Interes,
                         Mora = DetalleNotaDebitos.Mora,
                         Legal = DetalleNotaDebitos.Legal,
                         Seguro = DetalleNotaDebitos.Seguro,
                         TipoContratoID = DetalleNotaDebitos.TipoContratoID,
                         IsComputed = DetalleNotaDebitos.IsComputed,
                         ContinueOnTime = DetalleNotaDebitos.ContinueOnTime,
                         Tmp = 0
                     };
            if (R2 != null)
            {
                ReportObject.AddRange(R2);
            }

            var R3 = from DetalleNotaCreditos in _DetalleNotaCreditos
                     select new BoxReportItem
                     {
                         CuotaID = DetalleNotaCreditos.CuotaID,
                         ContratoID = DetalleNotaCreditos.ContratoID,
                         Vence = DetalleNotaCreditos.Vence,
                         Numero = DetalleNotaCreditos.Numero,
                         Capital = DetalleNotaCreditos.Capital * -1,
                         Comision = DetalleNotaCreditos.Comision * -1,
                         Interes = DetalleNotaCreditos.Interes * -1,
                         Mora = DetalleNotaCreditos.Mora * -1,
                         Legal = DetalleNotaCreditos.Legal * -1,
                         Seguro = DetalleNotaCreditos.Seguro * -1,
                         TipoContratoID = DetalleNotaCreditos.TipoContratoID,
                         IsComputed = DetalleNotaCreditos.IsComputed,
                         ContinueOnTime = DetalleNotaCreditos.ContinueOnTime,
                         Tmp = 0
                     };
            if (R3 != null)
            {
                ReportObject.AddRange(R3);
            }

            var R4 = from DetalleRecibos in _DetalleRecibos
                     select new BoxReportItem
                     {
                         CuotaID = DetalleRecibos.CuotaID,
                         ContratoID = DetalleRecibos.ContratoID,
                         Vence = DetalleRecibos.Vence,
                         Numero = DetalleRecibos.Numero,
                         Capital = DetalleRecibos.Capital * -1,
                         Comision = DetalleRecibos.Comision * -1,
                         Interes = DetalleRecibos.Interes * -1,
                         Mora = DetalleRecibos.Mora * -1,
                         Legal = DetalleRecibos.Legal * -1,
                         Seguro = DetalleRecibos.Seguro * -1,
                         TipoContratoID = DetalleRecibos.TipoContratoID,
                         IsComputed = DetalleRecibos.IsComputed,
                         ContinueOnTime = DetalleRecibos.ContinueOnTime,
                         Tmp = 0
                     };
            if (R4 != null)
            {
                ReportObject.AddRange(R4);
            }

            this.AddRange(ReportObject);
        }


    }

    public interface IBoxReport<T>
    {
        IQueryable<T> GetGroup(); //Substituir Dictionary
    }

    public interface IBoxReportItem { }

    public abstract class GenericReportItem : IBoxReportItem
    {

    }

    public class BoxReportItem : GenericReportItem
    {
        public int CuotaID { get; set; }
        public int Numero { get; set; }
        public int ContratoID { get; set; }
        public DateTime Vence { get; set; }
        public float Capital { get; set; }
        public float Comision { get; set; }
        public float Interes { get; set; }
        public float Mora { get; set; }
        public float Legal { get; set; }
        public float Seguro { get; set; }
        public float Balance { get; set; }
        public float Tmp { get; set; }
        public float Atraso { get; set; }
        public int Cantidad { get; set; }
        public int TipoContratoID { get; set; }
        public bool IsComputed { get; set; }
        public bool ContinueOnTime { get; set; }
    }

    public class BoxReport : List<BoxReportItem>, IBoxReport<BoxReportItem>
    {

        protected Core.Manager db;
        public BoxReport()
        {
            this.db = new Core.Manager();
            this.SubReporte = new List<BoxReport>();
        }
        public List<BoxReport> SubReporte { get; set; }

        public IQueryable<BoxReportItem> GetGroup()
        {

            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                CuotaID = gr.Key.CuotaID,
                                ContratoID = gr.Key.ContratoID,
                                TipoContratoID = gr.FirstOrDefault().TipoContratoID,
                                Vence = gr.Key.Vence,
                                Numero = gr.Key.Numero,
                                IsComputed = gr.FirstOrDefault().IsComputed,
                                ContinueOnTime = gr.FirstOrDefault().ContinueOnTime,
                                Capital = gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0,
                                Comision = gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0,
                                Interes = gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0,
                                Mora = gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0,
                                Legal = gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0,
                                Seguro = gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0,
                                Tmp = gr.FirstOrDefault().Tmp,
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            var Monitor = RptResume.AsQueryable<BoxReportItem>();
            return RptResume.AsQueryable<BoxReportItem>();
        }

        public IQueryable<BoxReportItem> GetGroupWithOutMora()
        {
            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                CuotaID = gr.Key.CuotaID,
                                ContratoID = gr.Key.ContratoID,
                                TipoContratoID = gr.FirstOrDefault().TipoContratoID,
                                Vence = gr.Key.Vence,
                                IsComputed = gr.FirstOrDefault().IsComputed,
                                ContinueOnTime = gr.FirstOrDefault().ContinueOnTime,
                                Numero = gr.Key.Numero,
                                Capital = gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0,
                                Comision = gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0,
                                Interes = gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0,
                                Mora = gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0,
                                Legal = gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0,
                                Seguro = gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0,
                                Tmp = gr.FirstOrDefault().Tmp,
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            var Monitor = RptResume.AsQueryable<BoxReportItem>();
            return RptResume.AsQueryable<BoxReportItem>();
        }

        public IQueryable<BoxReportItem> GetGroupWithOutInteres()
        {
            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                CuotaID = gr.Key.CuotaID,
                                ContratoID = gr.Key.ContratoID,
                                TipoContratoID = gr.FirstOrDefault().TipoContratoID,
                                Vence = gr.Key.Vence,
                                Numero = gr.Key.Numero,
                                IsComputed = gr.FirstOrDefault().IsComputed,
                                ContinueOnTime = gr.FirstOrDefault().ContinueOnTime,
                                Capital = gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0,
                                Comision = gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0,
                                Interes = gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0,
                                Mora = gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0,
                                Legal = gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0,
                                Seguro = gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0,
                                Tmp = gr.FirstOrDefault().Tmp,
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0)
                            };
            var Monitor = RptResume.AsQueryable<BoxReportItem>();
            return RptResume.AsQueryable<BoxReportItem>();
        }

        public float AtrasoGet(DateTime _fecha)
        {
            var RptResume = from i in this.ToList()
                            where (i.Vence < _fecha)
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            return RptResume.Sum(x => x.Balance);
        }

        public IQueryable<BoxReportItem> VencidosGet()
        {
            var RptResume = from i in this.ToList()
                            where (i.Vence <= DateTime.Now)
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            return RptResume.AsQueryable<BoxReportItem>();
        }

        public int CantidadAtrasoGet(DateTime Hasta)
        {
            var RptResume = from i in this.ToList()
                            where (i.Vence <= Hasta)
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            return RptResume.Where(x => x.Balance > 1).Count();
        }

        public float BalanceGetByReciboID()
        {
            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            return RptResume.Sum(x => x.Balance);
        }

        public float BalanceCapitalGetByReciboID()
        {
            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0)
                            };
            return RptResume.Sum(x => x.Balance);

        }

        public float BalanceCapitalAtrasosGetByReciboID(DateTime _fecha)
        {
            var Capital = from i in this.ToList()
                          group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                          select new BoxReportItem
                          {
                              Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0)
                          };

            var Atrasos = from i in this.ToList()
                          where (i.Vence < _fecha)
                          group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                          select new BoxReportItem
                          {
                              Balance = (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                          };

            float Balance = Capital.Sum(x => x.Balance) + Atrasos.Sum(x => x.Balance);
            return Balance;

        }


        public float BalanceCapital()
        {
            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0)
                            };
            return RptResume.Sum(x => x.Balance);
        }

        public float PagoMinimoGet()
        {
            var RptResume = from i in this.ToList()
                            where (i.Vence <= DateTime.Now)
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                // (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) +  (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + 
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            return RptResume.Sum(x => x.Balance);
        }

        public float Balance()
        {
            var RptResume = from i in this.ToList()
                            group i by new { i.ContratoID, i.CuotaID, i.Numero, i.Vence } into gr
                            select new BoxReportItem
                            {
                                Balance = (gr.Sum(x => x.Capital) >= 0 ? gr.Sum(x => x.Capital) : 0) + (gr.Sum(x => x.Comision) >= 0 ? gr.Sum(x => x.Comision) : 0) + (gr.Sum(x => x.Interes) >= 0 ? gr.Sum(x => x.Interes) : 0) + (gr.Sum(x => x.Mora) >= 0 ? gr.Sum(x => x.Mora) : 0) + (gr.Sum(x => x.Legal) >= 0 ? gr.Sum(x => x.Legal) : 0) + (gr.Sum(x => x.Seguro) >= 0 ? gr.Sum(x => x.Seguro) : 0)
                            };
            return RptResume.Sum(x => x.Balance);
        }
    }

    class CuotasView
    {
        public int CuotaID { get; set; }
        public int Numero { get; set; }
        public int ContratoID { get; set; }
        public DateTime Vence { get; set; }
        public float Capital { get; set; }
        public float Comision { get; set; }
        public float Interes { get; set; }
        public float Mora { get; set; }
        public float Legal { get; set; }
        public float Seguro { get; set; }
        public float Balance { get; set; }
        public float Tmp { get; set; }
        public int TipoContratoID { get; set; }
        public bool IsComputed { get; set; }
        public bool ContinueOnTime { get; set; }

    }
}
