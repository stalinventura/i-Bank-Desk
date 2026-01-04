using System;

namespace Payment.Bank.Core.Entity
{
	public class DatosEconomicos
	{
		public DatosEconomicos()
		{
		}

        public string Documento { set; get; }        
        public DateTime Fecha { set; get; }
        public Boolean Trabaja { set; get; }
        public string Empresa { set; get; }        
        public string Direccion { set; get; }
        public string Telefono { set; get; }
        public int OcupacionID { set; get; }
        public int HorarioID { set; get; }
        public int IngresoID { set; get; }
        public string Usuario { set; get; }
        public string ModificadoPor { set; get; }
        public DateTime FechaModificacion { set; get; }
    }
}