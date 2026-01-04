using System;

namespace Payment.Bank.Core.Entity
{
	public class FormaPagos
	{
		public FormaPagos()
		{
		}

        public int FormaPagoID { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public Boolean IsDefault { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}