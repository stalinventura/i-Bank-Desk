using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("GarantiaAlquileres")]
	public class clsGarantiaAlquileresBE
	{
		public clsGarantiaAlquileresBE()
		{
		}


        [Key]
        [ForeignKey("Alquileres")]
        [Column(TypeName = "int")]
        public int AlquilerID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsAlquileresBE Alquileres { get; set; }
        public virtual clsPersonasBE Personas { get; set; }



    }
}