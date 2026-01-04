using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("SubCuentaControl")]
    public class clsSubCuentaControlBE
    {
        public clsSubCuentaControlBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubCuentaControlID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Codigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Nombre { set; get; }

        [Column(TypeName = "int")]
        public int CuentaControlID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        public virtual clsCuentaControlBE CuentaControl { set; get; }
        public virtual ICollection<clsAuxiliaresBE> Auxiliares { set; get; }
    }
}