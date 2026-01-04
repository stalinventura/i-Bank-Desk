using System;

namespace Payment.Bank.Core.Entity
{
	public class SubGrupos
	{
		public SubGrupos()
		{
		}

		private int _subgrupoid;
		public int SubGrupoID
		{
			get{return _subgrupoid;}
			set{ _subgrupoid = value; }
		}

		private DateTime _fecha;
		public DateTime Fecha
		{
			get{return _fecha;}
			set{ _fecha = value; }
		}

		private string _codigo;
		public string Codigo
		{
			get{return _codigo;}
			set{ _codigo = value; }
		}

		private string _subgrupo;
		public string SubGrupo
		{
			get{return _subgrupo;}
			set{ _subgrupo = value; }
		}

		private int _grupoid;
		public int GrupoID
		{
			get{return _grupoid;}
			set{ _grupoid = value; }
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