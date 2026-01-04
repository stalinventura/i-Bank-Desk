using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.Core.Model
{
    public class clsDataCreditosBE
    {
        public clsDataCreditosBE()
        {
        }
        public string EmpresaID { set; get; }
        public string Empresa { set; get; }
        public DateTime Fecha { get; set; }
        public DateTime Vence { get; set; }
        public string Documento { set; get; }
        public float Monto { get; set; }
        public float Pagado { get; set; }
        public float Balance { get; set; }
        public float Cuota { get; set; }
        public int Cantidad { get; set; }
        public float Atraso { get; set; }
        public string Status { get; set; }

        //Relaciones


    }
}
