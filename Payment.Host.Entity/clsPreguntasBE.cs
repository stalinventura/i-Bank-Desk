using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Host.Entity
{
    [Table("Preguntas")]
	public class clsPreguntasBE
	{
		public clsPreguntasBE()
		{
            Usuarios = new HashSet<clsUsuariosBE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int PreguntaID { get; set; }

        [Column(TypeName ="DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Pregunta { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsUsuariosBE> Usuarios { get; set; }



    }
}