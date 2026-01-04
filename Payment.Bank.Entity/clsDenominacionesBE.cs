using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Denominaciones")]
	public class clsDenominacionesBE
	{
		public clsDenominacionesBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DenominacionID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "decimal")]
        public decimal Denominacion { set; get; }

        [Column(TypeName ="int")]
        public int TipoDenominacionID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsTipoDenominacionesBE TipoDenominaciones { get; set; }

        public virtual ICollection<clsDetalleArqueosBE> DetalleArqueos { get; set; }
    }
}