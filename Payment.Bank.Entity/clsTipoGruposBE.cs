using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("TipoGrupos")]
	public class clsTipoGruposBE
	{
		public clsTipoGruposBE()
		{

		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TipoGrupoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string TipoGrupo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        public virtual ICollection<clsGruposBE> Grupos { set; get; }
    }
}