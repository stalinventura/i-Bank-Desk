using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Edificios")]
	public class clsEdificiosBE
	{
		public clsEdificiosBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EdificioID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Edificio { get; set; }

        [Column(TypeName = "int")]
        public int CiudadID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual ICollection<clsApartamentosBE> Apartamentos { get; set; }
        public virtual clsCiudadesBE Ciudades { get; set; }

    }
}