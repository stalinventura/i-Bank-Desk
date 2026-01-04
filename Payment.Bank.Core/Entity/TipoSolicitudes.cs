using System;

namespace Payment.Bank.Core.Entity
{
	public class TipoSolicitudes
	{
		public TipoSolicitudes()
		{
		}

        public int TipoSolicitudID { set; get; }
        public DateTime Fecha { set; get; }
        public string TipoSolicitud { set; get; }
        public Boolean IsDefault { set; get; }
        public int? AuxiliarID { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}