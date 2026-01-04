using System;

namespace Payment.Bank.Core.Entity
{
	public class GarantiaHipotecaria
	{
		public GarantiaHipotecaria()
		{
		}

		private int _solicitudid;
		public int SolicitudID
		{
			get{return _solicitudid;}
			set{ _solicitudid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _descripcion;
		public string Descripcion
		{
			get{return _descripcion;}
			set{ _descripcion = value; }
		}

		private double _monto;
		public double Monto
		{
			get{return _monto;}
			set{ _monto = value; }
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