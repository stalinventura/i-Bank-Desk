using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Etiquetas")]
    public class clsEtiquetasBE
    {
        public clsEtiquetasBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EtiquetaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int LenguajeID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Nombre { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Texto { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }


        //Relaciones
        public virtual clsLenguajesBE Lenguajes { get; set; }
    }
}