using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Payment.Bank.Core.Model;
using Payment.Bank.Entity;

namespace Payment.Bank.Core.Services
{
    public class data
    {
        Manager db = new Manager();
        List<BoxReportItem> cuotasBE;
        List<clsDetalleRecibosBE> detalleRecibosBE;

        public async Task<OperationResult> RecibosCreateAsync(int ReciboID, int ContratoID, DateTime Fecha, int SucursalID, int ComprobanteID, int FormaPagoID, float Monto, string Informacion, string TokenID, bool EstadoID, string Usuario)
        {
            OperationResult result = new OperationResult();
            db = new Manager();
            var Token0 = db.RecibosGetByTokenID(TokenID);
            var Token1 = db.RecibosGetByTokenID(TokenID);
            if (Token0 == false && Token1 == false)
            {
                detalleRecibosBE = await db.PagosAutomaticos(ContratoID, Monto, Usuario);
                if (Monto > 0)
                {
                    result = db.RecibosCreate(ContratoID, Convert.ToDateTime(Fecha), SucursalID, ComprobanteID, FormaPagoID, Monto, 0, Monto,0,0, Informacion, Usuario, EstadoID);
                    if (result.ResponseCode == "00")
                    {
                        var response = DetalleRecibosCreate(Convert.ToInt32(result.ResponseMessage), ContratoID, Usuario);
                        var R = db.RecibosGetByReciboID(Convert.ToInt32(result.ResponseMessage));
                        if (response == false && R.DetalleRecibos.Count == 0)
                        {
                            response = DetalleRecibosCreate(Convert.ToInt32(result.ResponseMessage), ContratoID, Usuario);
                            if (!response)
                            {
                                result.ResponseCode = "01";
                                db.RecibosDelete(Convert.ToInt32(result.ResponseMessage), "NO SE HA COMPLETADO LA OPERACION.", Usuario);
                            }
                        }
                    }
                    result.ResponseMessage = TokenID;
                }
            }
            else
            {
                result = new OperationResult { ResponseCode = "00", ResponseMessage = TokenID };
            }
            return result;
        }

        public bool DetalleRecibosCreate(int ReciboID, int ContratoID, string Usuario)
        {
            try
            {
                bool response = false;
                db.DetalleRecibosDeleteGetByReciboID(ReciboID);
                OperationResult result = new OperationResult();
                foreach (clsDetalleRecibosBE row in detalleRecibosBE.OrderBy(x => x.Numero))
                {
                    result = db.DetalleRecibosCreate(row.CuotaID, ReciboID, row.Numero, row.Concepto, row.Capital, row.Comision, row.Interes, row.Mora, row.Legal, row.Seguro, row.SubTotal, Usuario);
                }

                if (result.ResponseCode == "00")
                {
                    response = true;
                    db.Contabilizar(2, ReciboID, Usuario);

                    clsCuotasView Cuotas = new clsCuotasView();
                    Cuotas.SetDataSource(ContratoID, ReciboID);
                    float Minimo = db.ContratosGetByContratoID(ContratoID).Solicitudes.Clientes.Sucursales.Configuraciones.MinimumAmount;
                    var R = Cuotas.GetGroup();
                    var balance = (Cuotas.BalanceGetByReciboID() < Minimo ? 0 : Cuotas.BalanceGetByReciboID());

                    if (balance <= Minimo)
                    {
                        db.ContratosDeleteGetByContratoID(ContratoID, 2, "SALDO A CONTRATO #" + ContratoID.ToString(), Usuario);
                    }
                }
                return response;
            }
            catch (Exception ex)
            { return false; }
        }

    }
}
