using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("HistorialCartas")]
	public class clsHistorialCartasBE
	{
		public clsHistorialCartasBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HistorialCartaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        public int CartaID { set; get; }

        public int CuotaID { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "float")]
        public float Atraso { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsCartasBE Cartas { get; set; }
        public virtual clsCuotasBE Cuotas { get; set; }

    }
}