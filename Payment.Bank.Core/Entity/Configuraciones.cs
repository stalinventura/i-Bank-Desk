using System;

namespace Payment.Bank.Core.Entity
{
	public class Configuraciones
	{
		public Configuraciones()
		{
		}

		private int _sucursalid;
		public int SucursalID
		{
			get{return _sucursalid;}
			set{ _sucursalid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private bool _hasnetworkaccess;
		public bool HasNetworkAccess
		{
			get{return _hasnetworkaccess;}
			set{ _hasnetworkaccess = value; }
		}

		private bool _hasautentication;
		public bool HasAutentication
		{
			get{return _hasautentication;}
			set{ _hasautentication = value; }
		}

		private string _username;
		public string UserName
		{
			get{return _username;}
			set{ _username = value; }
		}

		private string _password;
		public string Password
		{
			get{return _password;}
			set{ _password = value; }
		}

		private bool _notificarcelular;
		public bool NotificarCelular
		{
			get{return _notificarcelular;}
			set{ _notificarcelular = value; }
		}

		private string _msgnotificarcelular;
		public string msgNotificarCelular
		{
			get{return _msgnotificarcelular;}
			set{ _msgnotificarcelular = value; }
		}

		private bool _notificarsolicitud;
		public bool NotificarSolicitud
		{
			get{return _notificarsolicitud;}
			set{ _notificarsolicitud = value; }
		}

		private string _msgnotificarsolicitud;
		public string msgNotificarSolicitud
		{
			get{return _msgnotificarsolicitud;}
			set{ _msgnotificarsolicitud = value; }
		}

		private bool _notificarcontrato;
		public bool NotificarContrato
		{
			get{return _notificarcontrato;}
			set{ _notificarcontrato = value; }
		}

		private string _msgnotificarcontrato;
		public string msgNotificarContrato
		{
			get{return _msgnotificarcontrato;}
			set{ _msgnotificarcontrato = value; }
		}

		private bool _notificarrecibo;
		public bool NotificarRecibo
		{
			get{return _notificarrecibo;}
			set{ _notificarrecibo = value; }
		}

		private string _msgnotificarrecibo;
		public string msgNotificarRecibo
		{
			get{return _msgnotificarrecibo;}
			set{ _msgnotificarrecibo = value; }
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