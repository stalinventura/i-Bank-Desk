using System;

namespace Payment.Bank.Core.Entity
{
	public class Sucursales
	{
		public Sucursales()
		{
		}

        public int SucursalID { set; get; }        
        public DateTime Fecha { set; get; }
        public string Sucursal { set; get; }
        public string EmpresaID { set; get; }
        public int CiudadID { set; get; }
        public string Documento { set; get; }
        public string NotarioPublicoID { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}