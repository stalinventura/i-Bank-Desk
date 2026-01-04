using System;

namespace Payment.Bank.Core.Entity
{
	public class EstadosCiviles
	{
		public EstadosCiviles()
		{
		}

        public int EstadoCivilID { set; get; }
        public DateTime Fecha { set; get; }
        public string EstadoCivil { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}