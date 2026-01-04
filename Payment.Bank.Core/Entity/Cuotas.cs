using System;

namespace Payment.Bank.Core.Entity
{
	public class Cuotas
	{
		public Cuotas()
		{
		}

        public int CuotaID { get; set; }
        public int Numero { get; set; }
        public int ContratoID { get; set; }
        public DateTime Vence { get; set; }
        public float Capital { get; set; }
        public float Comision { get; set; }
        public float Interes { get; set; }
        public float Mora { get; set; }
        public float Legal { get; set; }
        public float Balance { get; set; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}