using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleArqueos")]
	public class clsDetalleArqueosBE
	{
		public clsDetalleArqueosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int DetalleArqueoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int ArqueoID { get; set; }

        [Column(TypeName = "int")]
        public int DenominacionID { get; set; }

        [Column(TypeName = "int")]
        public int Cantidad { get; set; }

        [Column(TypeName = "float")]
        public float SubTotal { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones

        public virtual clsDenominacionesBE Denominaciones { get; set; }
        public virtual clsArqueosBE Arqueos { get; set; }

    }
}