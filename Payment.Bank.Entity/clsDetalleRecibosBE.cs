using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleRecibos")]
    public class clsDetalleRecibosBE
	{
		public clsDetalleRecibosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int DetalleReciboID { get; set; }

        [Column(TypeName = "int", Order =0)]
        public int ReciboID { get; set; }

        [Column(TypeName = "int")]
        public int CuotaID { get; set; }

        [Column(TypeName = "int")]
        public int Numero { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Concepto { get; set; }

        [Column(TypeName = "float")]
        public float Capital { get; set; }

        [Column(TypeName = "float")]
        public float Comision { get; set; }

        [Column(TypeName = "float")]
        public float Interes { get; set; }

        [Column(TypeName = "float")]
        public float Mora { get; set; }

        [Column(TypeName = "float")]
        public float Legal { get; set; }

        [Column(TypeName = "float")]
        public float Seguro { get; set; }

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
        public virtual clsRecibosBE Recibos { get; set; }
        public virtual clsCuotasBE Cuotas { get; set; }
    }
}