using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Dias")]
	public class clsDiasBE
	{
		public clsDiasBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DiaID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Dia { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsDetalleJornadasBE> DetalleJornadas { get; set; }


    }
}