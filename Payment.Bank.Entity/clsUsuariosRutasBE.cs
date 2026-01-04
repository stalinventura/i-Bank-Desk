using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("UsuariosRutas")]
	public class clsUsuariosRutasBE
	{
		public clsUsuariosRutasBE()
		{
		}
        

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Rutas")]
        public int RutaID { set; get; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Usuarios")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsRutasBE Rutas { get; set; }
        public virtual clsUsuariosBE Usuarios { get; set; }

    }
}