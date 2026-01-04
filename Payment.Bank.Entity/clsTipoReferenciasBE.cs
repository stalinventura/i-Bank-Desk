using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("TipoReferencias")]
    public class clsTipoReferenciasBE
    {
        public clsTipoReferenciasBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TipoReferenciaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string TipoReferencia { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsDefault { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsReferenciasBE> Referencias { get; set; }
    }
}