using System;

namespace Payment.Bank.Core.Entity
{
	public class Roles
	{
		public Roles()
		{
		}

		private int _rolid;
		public int RolID
		{
			get{return _rolid;}
			set{ _rolid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _rol;
		public string Rol
		{
			get{return _rol;}
			set{ _rol = value; }
		}

		private bool _isadmin;
		public bool IsAdmin
		{
			get{return _isadmin;}
			set{ _isadmin = value; }
		}

		private int _graficoid;
		public int GraficoID
		{
			get{return _graficoid;}
			set{ _graficoid = value; }
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