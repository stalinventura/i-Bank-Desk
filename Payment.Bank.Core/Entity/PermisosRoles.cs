using System;

namespace Payment.Bank.Core.Entity
{
	public class PermisosRoles
	{
		public PermisosRoles()
		{
		}

		private int _rolid;
		public int RolID
		{
			get{return _rolid;}
			set{ _rolid = value; }
		}

		private int _permisoid;
		public int PermisoID
		{
			get{return _permisoid;}
			set{ _permisoid = value; }
		}

		private bool _agregar;
		public bool Agregar
		{
			get{return _agregar;}
			set{ _agregar = value; }
		}

		private bool _modificar;
		public bool Modificar
		{
			get{return _modificar;}
			set{ _modificar = value; }
		}

		private bool _imprimir;
		public bool Imprimir
		{
			get{return _imprimir;}
			set{ _imprimir = value; }
		}

		private bool _eliminar;
		public bool Eliminar
		{
			get{return _eliminar;}
			set{ _eliminar = value; }
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