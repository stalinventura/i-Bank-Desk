using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Host.Entity
{
    [Table("Historial")]
    public class clsHistorialBE
    {
        public clsHistorialBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int HistorialID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string EmpresaID { get; set; }

        [Column(TypeName = "int")]
        public int ContratoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Vence { set; get; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "float")]
        public float Pagado { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        [Column(TypeName = "float")]
        public float Cuota { get; set; }

        [Column(TypeName = "int")]
        public int Cantidad { set; get; }

        [Column(TypeName = "float")]
        public float Atraso { get; set; }

        [Column(TypeName = "float")]
        public float? MontoUltimoPago { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime? FechaUltimoPago { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(1)]
        public string Status { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { get; set; }
        public virtual clsEmpresasBE Empresas { get; set; }
    }
}