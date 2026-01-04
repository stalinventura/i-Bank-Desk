using System;

namespace Payment.Bank.Core.Entity
{
	public class DetalleRecibos
	{
		public DetalleRecibos()
		{
		}

        public int DetalleReciboID { get; set; }        
        public int ReciboID { get; set; }        
        public int CuotaID { get; set; }        
        public int Numero { get; set; }
        public string Concepto { get; set; }        
        public float Capital { get; set; }        
        public float Comision { get; set; }
        public float Interes { get; set; }        
        public float Mora { get; set; }        
        public float Legal { get; set; }        
        public float SubTotal { get; set; }        
        public string Usuario { get; set; }        
        public string ModificadoPor { get; set; }        
        public DateTime FechaModificacion { get; set; }
    }
}