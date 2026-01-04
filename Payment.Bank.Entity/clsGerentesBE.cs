using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Gerentes")]
	public class clsGerentesBE
	{
		public clsGerentesBE()
		{
		}

        [Key]
        [ForeignKey("Personas")]
        [Column(TypeName ="nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "bit")]
        public bool Estado { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { set; get; }
        public virtual ICollection<clsSucursalesBE> Sucursales { set; get; }
    }
}