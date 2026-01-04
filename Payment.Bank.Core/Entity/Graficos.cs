using System;

namespace Payment.Bank.Core.Entity
{
	public class Graficos
	{
		public Graficos()
		{
		}

		private int _graficoid;
		public int GraficoID
		{
			get{return _graficoid;}
			set{ _graficoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _grafico;
		public string Grafico
		{
			get{return _grafico;}
			set{ _grafico = value; }
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