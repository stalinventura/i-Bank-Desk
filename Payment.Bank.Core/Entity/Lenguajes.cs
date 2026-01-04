using System;

namespace Payment.Bank.Core.Entity
{
	public class Lenguajes
	{
		public Lenguajes()
		{
		}

		private int _lenguajeid;
		public int LenguajeID
		{
			get{return _lenguajeid;}
			set{ _lenguajeid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _codigo;
		public string Codigo
		{
			get{return _codigo;}
			set{ _codigo = value; }
		}

		private string _lenguaje;
		public string Lenguaje
		{
			get{return _lenguaje;}
			set{ _lenguaje = value; }
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