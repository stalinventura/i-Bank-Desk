using System;

namespace Payment.Bank.Core.Entity
{
	public class CuentasControl
	{
		public CuentasControl()
		{
		}

		private int _cuentacontrolid;
		public int CuentaControlID
		{
			get{return _cuentacontrolid;}
			set{ _cuentacontrolid = value; }
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

		private int _subgrupoid;
		public int SubGrupoID
		{
			get{return _subgrupoid;}
			set{ _subgrupoid = value; }
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