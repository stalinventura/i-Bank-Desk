using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.DGII.Models
{
    public class OperationResult
    {
        public string Rnc { get; set; }
        public string Empresa { get; set; }
        public string Siglas { get; set; }
        public string Categoría { get; set; }
        public string RegimenDePagos { get; set; }
        public string Estado { get; set; }
        public string Actividad { get; set; }
        public string Direccion { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
