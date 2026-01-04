using System;

namespace Payment.Bank.Core.Entity
{
	public class Ocupaciones
	{
		public Ocupaciones()
		{
		}

        public int OcupacionID { set; get; }
        public DateTime Fecha { set; get; }
        public string Ocupacion { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}