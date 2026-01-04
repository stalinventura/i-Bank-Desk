using System;

namespace Payment.Bank.Core.Entity
{
	public class TipoMovimientos
	{
		public TipoMovimientos()
		{
		}

		private int _tipomovimientoid;
		public int TipoMovimientoID
		{
			get{return _tipomovimientoid;}
			set{ _tipomovimientoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _movimiento;
		public string Movimiento
		{
			get{return _movimiento;}
			set{ _movimiento = value; }
		}

		private int _usuario;
		public int Usuario
		{
			get{return _usuario;}
			set{ _usuario = value; }
		}

		private int _modificadopor;
		public int ModificadoPor
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