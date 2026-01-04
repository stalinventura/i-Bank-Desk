using System;

namespace Payment.Bank.Core.Entity
{
	public class Ingresos
	{
		public Ingresos()
		{
		}

        public int IngresoID { set; get; }        
        public DateTime Fecha { set; get; }
        public string Ingreso { set; get; }        
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}