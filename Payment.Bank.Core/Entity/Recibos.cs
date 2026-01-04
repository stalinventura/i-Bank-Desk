using System;

namespace Payment.Bank.Core.Entity
{
	public class Recibos
	{
		public Recibos()
		{
		}

        public int ReciboID { get; set; }
        public DateTime Fecha { get; set; }
        public int ContratoID { get; set; }
        public int SucursalID { get; set; }
        public string Completo { get; set; }
        public int ComprobanteID { get; set; }
        public string Ncf { get; set; }
        public string Documento { get; set; }
        public int FormaPagoID { get; set; }
        public string Nombre { get; set; }
        public float SubTotal { get; set; }
        public float Descuento { get; set; }
        public float Monto { get; set; }
        public string Informacion { get; set; }
        public bool EstadoID { get; set; }
        public bool Contabilizado { get; set; }

    }
}