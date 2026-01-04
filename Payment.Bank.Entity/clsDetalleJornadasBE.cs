using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleJornadas")]
	public class clsDetalleJornadasBE
	{
		public clsDetalleJornadasBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DetalleJornadaID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "int")]
        public int JornadaID { get; set; }

        [Column(TypeName = "int")]
        public int DiaID { get; set; }

        [Column(TypeName = "int")]
        public int Desde { get; set; }

        [Column(TypeName = "int")]
        public int Hasta { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsJornadasBE Jornadas { get; set; }
        public virtual clsDiasBE Dias { get; set; }


    }
}