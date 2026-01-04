using System;

namespace Payment.Bank.Core.Entity
{
	public class SubCuentaControl
	{
		public SubCuentaControl()
		{
		}

		private int _subcuentacontrolid;
		public int SubCuentaControlID
		{
			get{return _subcuentacontrolid;}
			set{ _subcuentacontrolid = value; }
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

		private string _nombre;
		public string Nombre
		{
			get{return _nombre;}
			set{ _nombre = value; }
		}

		private int _cuentacontrolid;
		public int CuentaControlID
		{
			get{return _cuentacontrolid;}
			set{ _cuentacontrolid = value; }
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