using System;

namespace Payment.Bank.Core.Entity
{
	public class Comprobantes
	{
		public Comprobantes()
		{
		}

		private int _comprobanteid;
		public int ComprobanteID
		{
			get{return _comprobanteid;}
			set{ _comprobanteid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _comprobante;
		public string Comprobante
		{
			get{return _comprobante;}
			set{ _comprobante = value; }
		}

		private string _prefijo;
		public string Prefijo
		{
			get{return _prefijo;}
			set{ _prefijo = value; }
		}

		private int _desde;
		public int Desde
		{
			get{return _desde;}
			set{ _desde = value; }
		}

		private int _hasta;
		public int Hasta
		{
			get{return _hasta;}
			set{ _hasta = value; }
		}

		private int _secuencia;
		public int Secuencia
		{
			get{return _secuencia;}
			set{ _secuencia = value; }
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