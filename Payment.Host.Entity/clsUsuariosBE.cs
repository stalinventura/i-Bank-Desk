using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Host.Entity
{
    [Table("Usuarios")]
	public class clsUsuariosBE
    {
		public clsUsuariosBE()
		{
		}

        [Key]
        [ForeignKey("Personas")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName ="DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Contraseña { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { get; set; }

        [Column(TypeName = "int")]
        public int PreguntaID { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Respuesta { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }


        //Relaciones
       
        public virtual clsPreguntasBE Preguntas { get; set; }
        public virtual clsPersonasBE Personas { get; set; }



    }
}