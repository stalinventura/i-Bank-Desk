using System;

namespace Payment.Bank.Core.Entity
{
	public class Preguntas
	{
		public Preguntas()
		{
		}

		private int _preguntaid;
		public int PreguntaID
		{
			get{return _preguntaid;}
			set{ _preguntaid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _pregunta;
		public string Pregunta
		{
			get{return _pregunta;}
			set{ _pregunta = value; }
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