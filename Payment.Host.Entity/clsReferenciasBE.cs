using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Host.Entity
{
    [Table("Referencias")]
	public class clsReferenciasBE
	{
		public clsReferenciasBE()
		{
		}


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int ReferenciaID { set; get; }

        [Column(TypeName = "int")]
        public int TipoReferenciaID { set; get; }

        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Referencia { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Telefono { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsPersonasBE Personas { set; get; }
        public virtual clsTipoReferenciasBE TipoReferencias { set; get; }

    }
}