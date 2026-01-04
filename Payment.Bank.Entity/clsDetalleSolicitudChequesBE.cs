using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleSolicitudCheques")]
    public class clsDetalleSolicitudChequesBE
	{
		public clsDetalleSolicitudChequesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int DetalleSolicitudChequeID { get; set; }

        [Column(TypeName = "int")]
        public int SolicitudChequeID { get; set; }

        [Column(TypeName = "int")]
        public int AuxiliarID { get; set; }

        [Column(TypeName = "float")]
        public float Debito { get; set; }

        [Column(TypeName = "float")]
        public float Credito { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsAuxiliaresBE Auxiliares { get; set; }
        public virtual clsSolicitudChequesBE SolicitudCheques { get; set; }
    }
}