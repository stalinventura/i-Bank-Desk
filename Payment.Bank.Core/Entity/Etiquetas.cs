using System;

namespace Payment.Bank.Core.Entity
{
	public class Etiquetas
	{
		public Etiquetas()
		{
		}

		private int _idetiqueta;
		public int IdEtiqueta
		{
			get{return _idetiqueta;}
			set{ _idetiqueta = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private int _lenguajeid;
		public int LenguajeID
		{
			get{return _lenguajeid;}
			set{ _lenguajeid = value; }
		}

		private string _nombre;
		public string Nombre
		{
			get{return _nombre;}
			set{ _nombre = value; }
		}

		private string _texto;
		public string Texto
		{
			get{return _texto;}
			set{ _texto = value; }
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