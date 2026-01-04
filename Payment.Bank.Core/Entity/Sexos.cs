using System;

namespace Payment.Bank.Core.Entity
{
	public class Sexos
	{
		public Sexos()
		{
		}

		private int _sexoid;
		public int SexoID
		{
			get{return _sexoid;}
			set{ _sexoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _sexo;
		public string Sexo
		{
			get{return _sexo;}
			set{ _sexo = value; }
		}

		private string _usuario;
		public string Usuario
		{
			get{return _usuario;}
			set{ _usuario = value; }
		}

		private string _modificadopor;
		public string ModificadoPor
		{
			get{return _modificadopor;}
			set{ _modificadopor = value; }
		}

		private DateTime _fechamodificacion;
		public DateTime FechaModificacion
		{
			get{return _fechamodificacion;}
			set{ _fechamodificacion = value; }
		}
	}
}