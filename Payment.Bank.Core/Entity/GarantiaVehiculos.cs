using System;

namespace Payment.Bank.Core.Entity
{
	public class GarantiaVehiculos
	{
		public GarantiaVehiculos()
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

        private int _ano;
        public int Ano
        {
            get { return _ano; }
            set { _ano = value; }
        }


        private int _tipovehiculoid;
		public int TipoVehiculoID
		{
			get{return _tipovehiculoid;}
			set{ _tipovehiculoid = value; }
		}

        private string _tipovehiculo;
        public string TipoVehiculo
        {
            get { return _tipovehiculo; }
            set { _tipovehiculo = value; }
        }

        private int _modeloid;
		public int ModeloID
		{
			get{return _modeloid;}
			set{ _modeloid = value; }
		}

        private string _modelo;
        public string Modelo
        {
            get { return _modelo; }
            set { _modelo = value; }
        }

        private string _marca;
        public string Marca
        {
            get { return _marca; }
            set { _marca = value; }
        }

        private int _colorid;
		public int ColorID
		{
			get{return _colorid;}
			set{ _colorid = value; }
		}

        private string _color;
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private string _chasis;
		public string Chasis
		{
			get{return _chasis;}
			set{ _chasis = value; }
		}

		private string _placa;
		public string Placa
		{
			get{return _placa;}
			set{ _placa = value; }
		}

		private string _registro;
		public string Registro
		{
			get{return _registro;}
			set{ _registro = value; }
		}

		private DateTime _fechaexpedicion;
		public DateTime FechaExpedicion
		{
			get{return _fechaexpedicion;}
			set{ _fechaexpedicion = value; }
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