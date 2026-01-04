using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Firmas")]
	public class clsFirmasBE
	{
		public clsFirmasBE()
		{
		}


        [Key]
        [ForeignKey("Personas")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }
        public byte[] Firma { get; set; }

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

    }
}