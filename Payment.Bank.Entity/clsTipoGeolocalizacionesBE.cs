using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("TipoGeolocalizaciones")]
	public class clsTipoGeolocalizacionesBE
    {
		public clsTipoGeolocalizacionesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int TipoGeolocalizacionID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string TipoGeolocalizacion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones

        public virtual ICollection<clsGeolocalizacionesBE> Geolocalizaciones { get; set; }

    }
}