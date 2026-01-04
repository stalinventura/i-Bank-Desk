using System;

namespace Payment.Bank.Core.Entity
{
	public class Paises
	{
		public Paises()
		{
		}

		private int _paisid;
		public int PaisID
		{
			get{return _paisid;}
			set{ _paisid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _pais;
		public string Pais
		{
			get{return _pais;}
			set{ _pais = value; }
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