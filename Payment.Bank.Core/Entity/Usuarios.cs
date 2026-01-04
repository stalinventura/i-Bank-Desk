using System;

namespace Payment.Bank.Core.Entity
{
	public class Usuarios
	{
		public Usuarios()
		{
		}

		private string _documento;
		public string Documento
		{
			get{return _documento;}
			set{ _documento = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _contraseña;
		public string Contraseña
		{
			get{return _contraseña;}
			set{ _contraseña = value; }
		}

		private int _rolid;
		public int RolID
		{
			get{return _rolid;}
			set{ _rolid = value; }
		}

		private int _sucursalid;
		public int SucursalID
		{
			get{return _sucursalid;}
			set{ _sucursalid = value; }
		}

		private bool _estado;
		public bool Estado
		{
			get{return _estado;}
			set{ _estado = value; }
		}

		private int _preguntaid;
		public int PreguntaID
		{
			get{return _preguntaid;}
			set{ _preguntaid = value; }
		}

		private string _respuesta;
		public string Respuesta
		{
			get{return _respuesta;}
			set{ _respuesta = value; }
		}

		private int _lenguajeid;
		public int LenguajeID
		{
			get{return _lenguajeid;}
			set{ _lenguajeid = value; }
		}

		private string _tokenid;
		public string TokenID
		{
			get{return _tokenid;}
			set{ _tokenid = value; }
		}

		private DateTime _fechatokenid;
		public DateTime FechaTokenID
		{
			get{return _fechatokenid;}
			set{ _fechatokenid = value; }
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