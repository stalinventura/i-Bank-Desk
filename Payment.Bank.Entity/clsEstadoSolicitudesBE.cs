using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Payment.Bank.Entity
{
    [Table("EstadoSolicitudes")]
	public class clsEstadoSolicitudesBE
	{
		public clsEstadoSolicitudesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EstadoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Estado { set; get; }

        [Column(TypeName = "bit")]
        public Boolean IsDefault { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsSolicitudesBE> Solicitudes { set; get; }

    }
}