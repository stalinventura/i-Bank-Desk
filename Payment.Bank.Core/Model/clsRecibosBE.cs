

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Core.Models
{
  
	public class clsRecibosBE
	{
		public clsRecibosBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReciboID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int ContratoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Cliente { get; set; }

        [Column(TypeName = "int")]
        public int SucursalID { get; set; }

        [Column(TypeName = "int")]
        public int ComprobanteID { get; set; }

        [Column(TypeName = "int")]
        public int FormaPagoID { get; set; }

        [Column(TypeName = "float")]
        public float SubTotal { get; set; }

        [Column(TypeName = "float")]
        public float Descuento { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Informacion { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { get; set; }

        [Column(TypeName = "bit")]
        public bool Contabilizado { get; set; }

        [Column(TypeName = "bit")]
        public bool Sync { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string TokenID
        {
            get
            {
                return $"{ContratoID}-{Fecha.Month}{Fecha.Day}{Fecha.Year}{Fecha.Hour}{Fecha.Minute}{Fecha.Second}-{SucursalID}-{Monto}-{Usuario}";
            }
        }

[Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

    
    }
}