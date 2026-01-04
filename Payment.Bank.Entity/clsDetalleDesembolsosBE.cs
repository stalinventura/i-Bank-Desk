using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleDesembolsos")]
	public class clsDetalleDesembolsosBE
	{
		public clsDetalleDesembolsosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetalleDesembolsoID { set; get; }

        [Column(TypeName = "int")]
        public int DesembolsoID { set; get; }

        [Column(TypeName = "int")]
        public int AuxiliarID { set; get; }

        [Column(TypeName = "float")]
        public float Debito { set; get; }

        [Column(TypeName = "float")]
        public float Credito { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsDesembolsosBE Desembolsos { set; get; }
        public virtual clsAuxiliaresBE Auxiliares { set; get; }

    }
}