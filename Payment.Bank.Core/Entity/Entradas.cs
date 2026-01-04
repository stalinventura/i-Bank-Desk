using System;

namespace Payment.Bank.Core.Entity
{
	public class Entradas
	{
		public Entradas()
		{
		}

		private int _identrada;
		public int IdEntrada
		{
			get{return _identrada;}
			set{ _identrada = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private int _tipomovimientoid;
		public int TipoMovimientoID
		{
			get{return _tipomovimientoid;}
			set{ _tipomovimientoid = value; }
		}

		private string _referencia;
		public string Referencia
		{
			get{return _referencia;}
			set{ _referencia = value; }
		}

		private string _concepto;
		public string Concepto
		{
			get{return _concepto;}
			set{ _concepto = value; }
		}

		private double _debito;
		public double Debito
		{
			get{return _debito;}
			set{ _debito = value; }
		}

		private double _credito;
		public double Credito
		{
			get{return _credito;}
			set{ _credito = value; }
		}

		private bool _idestado;
		public bool IdEstado
		{
			get{return _idestado;}
			set{ _idestado = value; }
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