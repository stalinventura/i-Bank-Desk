using System;

namespace Payment.Bank.Core.Entity
{
	public class Llamadas
	{
		public Llamadas()
		{
		}

		private int _llamadaid;
		public int LlamadaID
		{
			get{return _llamadaid;}
			set{ _llamadaid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private int _contratoid;
		public int ContratoID
		{
			get{return _contratoid;}
			set{ _contratoid = value; }
		}

		private string _hablecon;
		public string HableCon
		{
			get{return _hablecon;}
			set{ _hablecon = value; }
		}

		private string _mensaje;
		public string Mensaje
		{
			get{return _mensaje;}
			set{ _mensaje = value; }
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