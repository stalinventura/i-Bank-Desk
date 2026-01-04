using System;

namespace Payment.Bank.Core.Entity
{
	public class Modelos
	{
		public Modelos()
		{
		}

		private int _modeloid;
		public int ModeloID
		{
			get{return _modeloid;}
			set{ _modeloid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _modelo;
		public string Modelo
		{
			get{return _modelo;}
			set{ _modelo = value; }
		}

		private int _marcaid;
		public int MarcaID
		{
			get{return _marcaid;}
			set{ _marcaid = value; }
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