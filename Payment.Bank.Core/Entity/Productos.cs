using System;

namespace Payment.Bank.Core.Entity
{
	public class Productos
	{
		public Productos()
		{
		}

		private int _productoid;
		public int ProductoID
		{
			get{return _productoid;}
			set{ _productoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _producto;
		public string Producto
		{
			get{return _producto;}
			set{ _producto = value; }
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