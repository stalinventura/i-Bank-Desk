using System;

namespace Payment.Bank.Core.Entity
{
	public class Empresas
	{
		public Empresas()
		{
		}
        
        public string EmpresaID { get; set; }        
        public DateTime Fecha { get; set; }
        public string Empresa { get; set; }
        public string Direccion { set; get; }
        public string Telefonos { set; get; }
        public string Rnc { get; set; }
        public string Siglas { get; set; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}