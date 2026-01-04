using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Comprobantes")]
    public class clsComprobantesBE
    {
        public clsComprobantesBE()
        {
        }

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ComprobanteID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Comprobante { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Prefijo { get; set; }

        [Column(TypeName = "int")]
        public int Desde { get; set; }
        [Column(TypeName = "int")]
        public int Hasta { get; set; }

        [Column(TypeName = "int")]
        public int Secuencia { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsDefault { set; get; }

        [Column(TypeName = "bit")]
        public Boolean EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsRecibosBE> Recibos { get; set; }

    }
}