using System;

namespace Payment.Bank.Core.Entity
{
	public class DetalleEntradas
	{
		public DetalleEntradas()
		{
		}

		private int _iddetalleentrada;
		public int IdDetalleEntrada
		{
			get{return _iddetalleentrada;}
			set{ _iddetalleentrada = value; }
		}

		private int _identrada;
		public int IdEntrada
		{
			get{return _identrada;}
			set{ _identrada = value; }
		}

		private int _idtipodocumento;
		public int IdTipoDocumento
		{
			get{return _idtipodocumento;}
			set{ _idtipodocumento = value; }
		}

		private int _numerodocumento;
		public int NumeroDocumento
		{
			get{return _numerodocumento;}
			set{ _numerodocumento = value; }
		}

		private int _auxiliarid;
		public int AuxiliarID
		{
			get{return _auxiliarid;}
			set{ _auxiliarid = value; }
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