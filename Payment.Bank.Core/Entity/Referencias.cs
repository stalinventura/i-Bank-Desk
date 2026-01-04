using System;

namespace Payment.Bank.Core.Entity
{
	public class Referencias
	{
		public Referencias()
		{
		}

        public int ReferenciaID { set; get; }
        public int TipoReferenciaID { set; get; }
        public string TipoReferencia { set; get; }
        public string Documento { set; get; }
        public DateTime Fecha { set; get; }
        public string Referencia { set; get; }
        public string Direccion { set; get; }
        public string Telefono { set; get; }
        public string Usuario { set; get; }
        public string ModificadoPor { set; get; }
        public DateTime FechaModificacion { set; get; }
    }
}