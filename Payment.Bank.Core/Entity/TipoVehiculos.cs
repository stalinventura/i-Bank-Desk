using System;

namespace Payment.Bank.Core.Entity
{
	public class TipoVehiculos
	{
		public TipoVehiculos()
		{
		}

		private int _tipovehiculoid;
		public int TipoVehiculoID
		{
			get{return _tipovehiculoid;}
			set{ _tipovehiculoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _tipovehiculo;
		public string TipoVehiculo
		{
			get{return _tipovehiculo;}
			set{ _tipovehiculo = value; }
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