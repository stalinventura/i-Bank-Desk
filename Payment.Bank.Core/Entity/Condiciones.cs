using System;

namespace Payment.Bank.Core.Entity
{
	public class Condiciones
	{
		public Condiciones()
		{
		}

        public int CondicionID { set; get; }        
        public DateTime Fecha { set; get; }
        public string Condicion { set; get; }
        public int Dias { set; get; }
        public int Meses { set; get; }
        public bool IsDefault { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}