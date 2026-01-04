using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Cuotas")]
	public class clsCuotasBE
	{
		public clsCuotasBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CuotaID { get; set; }

        [Column(TypeName = "int")]
        public int Numero { get; set; }

        [Column(TypeName = "int")]
        public int ContratoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Vence { get; set; }

        [Column(TypeName = "float")]
        public float Capital { get; set; }

        [Column(TypeName = "float")]
        public float Comision { get; set; }

        [Column(TypeName = "float")]
        public float Interes { get; set; }

        [Column(TypeName = "float")]
        public float Mora { get; set; }

        [Column(TypeName = "float")]
        public float Legal { get; set; }

        [Column(TypeName = "float")]
        public float Seguro { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        [Column(TypeName = "bit")]
        public bool IsComputed { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsContratosBE Contratos { get; set; }
        public virtual ICollection<clsDetalleNotaDebitosBE> DetalleNotaDebitos { get; set; }
        public virtual ICollection<clsDetalleNotaCreditosBE> DetalleNotaCreditos { get; set; }
        public virtual ICollection<clsDetalleRecibosBE> DetalleRecibos { get; set; }
        public virtual ICollection<clsHistorialCartasBE> HistorialCartas { get; set; }
    }
}