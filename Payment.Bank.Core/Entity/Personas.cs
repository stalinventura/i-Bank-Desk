using System;

namespace Payment.Bank.Core.Entity
{
	public class Personas
	{
		public Personas()
		{
		}

        public string Documento { set; get; }
        public DateTime Fecha { set; get; }
        public DateTime FechaNacimiento { set; get; }
        public string Nombres { set; get; }
        public string Apellidos { set; get; }
        public string Completo { set; get; }
        public string Ciudad { set; get; }
        public string Apodo { set; get; }
        public string Fotografia { set; get; }
        public int CiudadID { set; get; }
        public string Direccion { set; get; }
        public string Correo { set; get; }
        public string Telefono { set; get; }
        public int OperadorID { set; get; }
        public string Celular { set; get; }
        public int SexoID { set; get; }
        public string Sexo { set; get; }
        public int EstadoCivilID { set; get; }
        public bool IsAsync { set; get; }
        public string Usuario { set; get; }
        public string ModificadoPor { set; get; }
        public DateTime FechaModificacion { set; get; }
    }
}