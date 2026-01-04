using System;

namespace Payment.Bank.Core.Entity
{
	public class Colores
	{
		public Colores()
		{
		}

		private int _colorid;
		public int ColorID
		{
			get{return _colorid;}
			set{ _colorid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _color;
		public string Color
		{
			get{return _color;}
			set{ _color = value; }
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