using System;

namespace Payment.Bank.Core.Entity
{
	public class Contratos
	{
		public Contratos()
		{
		}

        public int ContratoID { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Vence { get; set; }
        public string Completo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Apodo { get; set; }
        public int TipoSolicitudID { get; set; }
        public string TipoSolicitud { get; set; }
        public int Tiempo { get; set; }
        public string Condicion { get; set; }
        public float Monto { get; set; }
        public int EstadoID { get; set; }        

    }
}