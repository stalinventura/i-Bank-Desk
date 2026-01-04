using System;

namespace Payment.Bank.Core.Entity
{
	public class EstadoSolicitudes
	{
		public EstadoSolicitudes()
		{
		}

		private int _estadoid;
		public int EstadoID
		{
			get{return _estadoid;}
			set{ _estadoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _estado;
		public string Estado
		{
			get{return _estado;}
			set{ _estado = value; }
		}

		private bool _isdefault;
		public bool IsDefault
		{
			get{return _isdefault;}
			set{ _isdefault = value; }
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