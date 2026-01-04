using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Geolocalizaciones")]
	public class clsGeolocalizacionesBE
	{
		public clsGeolocalizacionesBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int GeolocalizacionID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "int")]
        public int TipoGeolocalizacionID { set; get; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { get; set; }
        public virtual clsTipoGeolocalizacionesBE TipoGeolocalizaciones { get; set; }

    }
}