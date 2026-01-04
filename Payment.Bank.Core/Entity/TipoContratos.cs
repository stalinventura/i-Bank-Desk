using System;

namespace Payment.Bank.Core.Entity
{
	public class TipoContratos
	{
		public TipoContratos()
		{
		}

		private int _tipocontratoid;
		public int TipoContratoID
		{
			get{return _tipocontratoid;}
			set{ _tipocontratoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _tipocontrato;
		public string TipoContrato
		{
			get{return _tipocontrato;}
			set{ _tipocontrato = value; }
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