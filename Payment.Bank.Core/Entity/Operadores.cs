using System;

namespace Payment.Bank.Core.Entity
{
	public class Operadores
	{
		public Operadores()
		{
		}

		private int _operadorid;
		public int OperadorID
		{
			get{return _operadorid;}
			set{ _operadorid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _operador;
		public string Operador
		{
			get{return _operador;}
			set{ _operador = value; }
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