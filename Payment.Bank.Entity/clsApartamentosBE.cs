using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Apartamentos")]
	public class clsApartamentosBE
	{
		public clsApartamentosBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApartamentoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Apartamento { get; set; }

        [Column(TypeName = "int")]
        public int EdificioID { get; set; }

        [Column(TypeName = "int")]
        public int PisoID { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Descripcion { get; set; }

        [Column(TypeName = "int")]
        public int TipoApartamentoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones

        public virtual clsEdificiosBE Edificios { get; set; }
        public virtual clsPisosBE Pisos { get; set; }
        public virtual clsTipoApartamentosBE TipoApartamentos { get; set; }
        public virtual ICollection<clsAlquileresBE> Alquileres { get; set; }

    }
}