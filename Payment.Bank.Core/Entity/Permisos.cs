using System;

namespace Payment.Bank.Core.Entity
{
	public class Permisos
	{
		public Permisos()
		{
		}

		private int _permisoid;
		public int PermisoID
		{
			get{return _permisoid;}
			set{ _permisoid = value; }
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

		private int _productoid;
		public int ProductoID
		{
			get{return _productoid;}
			set{ _productoid = value; }
		}

		private string _permiso;
		public string Permiso
		{
			get{return _permiso;}
			set{ _permiso = value; }
		}

		private string _descripcion;
		public string Descripcion
		{
			get{return _descripcion;}
			set{ _descripcion = value; }
		}

		private string _url;
		public string Url
		{
			get{return _url;}
			set{ _url = value; }
		}

		private string _icon;
		public string Icon
		{
			get{return _icon;}
			set{ _icon = value; }
		}

		private int _idpadre;
		public int IdPadre
		{
			get{return _idpadre;}
			set{ _idpadre = value; }
		}

		private int _orden;
		public int Orden
		{
			get{return _orden;}
			set{ _orden = value; }
		}

		private bool _visible;
		public bool Visible
		{
			get{return _visible;}
			set{ _visible = value; }
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