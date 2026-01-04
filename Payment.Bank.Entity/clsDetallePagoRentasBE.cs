using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetallePagoRentas")]
    public class clsDetallePagoRentasBE
	{
		public clsDetallePagoRentasBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int DetallePagoRentaID { get; set; }

        [Column(TypeName = "int")]
        public int PagoRentaID { get; set; }

        [Column(TypeName = "int")]
        public int RentaID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Concepto { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }
       
        //Relaciones
        public virtual clsPagoRentasBE PagoRentas { get; set; }
        public virtual clsRentasBE Rentas { get; set; }
    }
}