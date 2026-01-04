using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("TipoContratos")]
    public class clsTipoContratosBE
    {
        public clsTipoContratosBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TipoContratoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string TipoContrato { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsDefault { set; get; }

        [Column(TypeName = "bit")]
        public bool ContinueOnTime { get; set; }

        [Column(TypeName = "bit")]
        public Boolean InteresDiario { get; set; }

        [Column(TypeName = "bit")]
        public Boolean MoraDiaria { get; set; }
                
        [Column(TypeName = "bit")]
        public Boolean CanMinimumPay { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsMoraOverTime { get; set; }

        [Column(TypeName = "bit")]
        public Boolean OnlyInteger { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsContratosBE> Contratos { get; set; }
    }
}