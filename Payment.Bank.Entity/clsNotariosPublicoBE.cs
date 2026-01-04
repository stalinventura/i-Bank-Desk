using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("NotariosPublico")]
	public class clsNotariosPublicoBE
	{
		public clsNotariosPublicoBE()
		{
		}


        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NotarioPublicoID { set; get; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string DocumentoPrimerTestigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string NombrePrimerTestigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string DireccionPrimerTestigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string DocumentoSegundoTestigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string NombreSegundoTestigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string DireccionSegundoTestigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Exequatur { set; get; }

        [Column(TypeName = "bit")]
        public Boolean EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { set; get; }
        public virtual clsSucursalesBE Sucursales { set; get; }
        public virtual ICollection<clsContratosBE> Contratos { set; get; }
        public virtual ICollection<clsAlquileresBE> Alquileres { set; get; }
        public virtual ICollection<clsActosBE> Actos { get; set; }

    }
}