using System;

namespace Payment.Bank.Core.Entity
{
	public class Horarios
	{
		public Horarios()
		{
		}

        public int HorarioID { set; get; }
        public DateTime Fecha { set; get; }
        public string Horario { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}