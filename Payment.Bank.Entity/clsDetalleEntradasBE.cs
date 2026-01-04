using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleEntradas")]
    public class clsDetalleEntradasBE
    {
        public clsDetalleEntradasBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetalleEntradaID { set; get; }

        [Column(TypeName = "int")]
        public int EntradaID { set; get; }

        [Column(TypeName = "int")]
        public int TipoEntradaID { set; get; }

        [Column(TypeName = "int")]
        public int Numero { set; get; }

        [Column(TypeName = "int")]
        public int AuxiliarID { set; get; }

        [Column(TypeName = "float")]
        public float Debito { set; get; }

        [Column(TypeName = "float")]
        public float Credito { set; get; }

        [Column(TypeName = "bit")]
        public bool EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsEntradasBE Entradas { set; get; }
        public virtual clsAuxiliaresBE Auxiliares { set; get; }
        public virtual clsTipoEntradasBE TipoEntradas { set; get; }

    }
}