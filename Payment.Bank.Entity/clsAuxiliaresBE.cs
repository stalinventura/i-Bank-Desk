using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Auxiliares")]
    public class clsAuxiliaresBE
    {
        public clsAuxiliaresBE()
        {
            TipoSolicitudes = new HashSet<clsTipoSolicitudesBE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AuxiliarID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Codigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Auxiliar { set; get; }

        [Column(TypeName = "int")]
        public int SubCuentaControlID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsSubCuentaControlBE SubCuentaControl { set; get; }
        public virtual ICollection<clsDetalleEntradasBE> DetalleEntradas { set; get; }
        public virtual ICollection<clsTipoSolicitudesBE> TipoSolicitudes { set; get; }
        public virtual ICollection<clsBancosBE> Bancos { set; get; }
        public virtual ICollection<clsDetalleDesembolsosBE> DetalleDesembolsos { set; get; }
        public virtual ICollection<clsFormaPagosBE> FormaPagos { get; set; }
    }
}