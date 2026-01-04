using System;

namespace Payment.Bank.Core.Entity
{
	public class Ciudades
	{
		public Ciudades()
		{
		}

        public int CiudadID { get; set; }        
        public DateTime Fecha { get; set; }        
        public string Ciudad { get; set; }        
        public int ProvinciaID { get; set; }        
        public string Usuario { get; set; }        
        public string ModificadoPor { get; set; }        
        public DateTime FechaModificacion { get; set; }
    }
}