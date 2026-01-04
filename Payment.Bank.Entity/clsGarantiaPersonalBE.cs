using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("GarantiaPersonal")]
	public class clsGarantiaPersonalBE
	{
		public clsGarantiaPersonalBE()
		{
		}


        [Key]
        [ForeignKey("Solicitudes")]
        [Column(TypeName = "int")]
        public int SolicitudID { set; get; }

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
        public virtual clsSolicitudesBE Solicitudes { get; set; }
        public virtual clsGarantesBE Garantes { get; set; }



    }
}