using System;

namespace Payment.Bank.Core.Entity
{
	public class NotariosPublico
	{
		public NotariosPublico()
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

		private string _direccion;
		public string Direccion
		{
			get{return _direccion;}
			set{ _direccion = value; }
		}

		private string _documentoprimertestigo;
		public string DocumentoPrimerTestigo
		{
			get{return _documentoprimertestigo;}
			set{ _documentoprimertestigo = value; }
		}

		private string _nombreprimertestigo;
		public string NombrePrimerTestigo
		{
			get{return _nombreprimertestigo;}
			set{ _nombreprimertestigo = value; }
		}

		private string _direccionprimertestigo;
		public string DireccionPrimerTestigo
		{
			get{return _direccionprimertestigo;}
			set{ _direccionprimertestigo = value; }
		}

		private string _documentosegundotestigo;
		public string DocumentoSegundoTestigo
		{
			get{return _documentosegundotestigo;}
			set{ _documentosegundotestigo = value; }
		}

		private string _nombresegundotestigo;
		public string NombreSegundoTestigo
		{
			get{return _nombresegundotestigo;}
			set{ _nombresegundotestigo = value; }
		}

		private string _direccionsegundotestigo;
		public string DireccionSegundoTestigo
		{
			get{return _direccionsegundotestigo;}
			set{ _direccionsegundotestigo = value; }
		}

		private string _excecuatur;
		public string Excecuatur
		{
			get{return _excecuatur;}
			set{ _excecuatur = value; }
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