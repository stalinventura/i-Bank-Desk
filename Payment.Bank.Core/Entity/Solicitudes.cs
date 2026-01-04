using System;

namespace Payment.Bank.Core.Entity
{
	public class Solicitudes
	{
		public Solicitudes()
		{
		}

        public int SolicitudID { get; set; }
        public DateTime Fecha { get; set; }
        public string Completo { get; set; }
        public string TipoSolicitud { get; set; }
        public int Tiempo { get; set; }
        public string Condicion { get; set; }
        public float Monto { get; set; }
        public int EstadoID { get; set; }

        public int ClienteID { get; set; }
        public int CondicionID { get; set; }
        public int TipoSolicitudID { get; set; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}