using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Ciudades")]
	public class clsCiudadesBE
	{
		public clsCiudadesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int CiudadID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Ciudad { get; set; }

        [Column(TypeName = "int")]
        public int ProvinciaID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones

        public virtual clsProvinciasBE Provincias { get; set; }
        public virtual ICollection<clsSucursalesBE> Sucursales { get; set; }
        public virtual ICollection<clsPersonasBE> Personas { get; set; }
        public virtual ICollection<clsEdificiosBE> Edificios { get; set; }

    }
}