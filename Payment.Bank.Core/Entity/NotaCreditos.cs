using System;

namespace Payment.Bank.Core.Entity
{
	public class DetalleNotaCreditos
	{
		public DetalleNotaCreditos()
		{
		}

        public int NotaCreditoID { get; set; }
        public DateTime Fecha { get; set; }
        public int CuotaID { get; set; }
        public string Concepto { get; set; }
        public float Capital { get; set; }
        public float Comision { get; set; }
        public float Interes { get; set; }
        public float Mora { get; set; }
        public float Legal { get; set; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}