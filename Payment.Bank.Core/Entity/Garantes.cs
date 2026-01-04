using System;

namespace Payment.Bank.Core.Entity
{
	public class Garantes
	{
		public Garantes()
		{
		}

		private string _documento;
		public string Documento
		{
			get{return _documento;}
			set{ _documento = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private bool _estado;
		public bool Estado
		{
			get{return _estado;}
			set{ _estado = value; }
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