using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Grupos")]
    public class clsGruposBE
    {
        public clsGruposBE()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GrupoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Codigo { set; get; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Grupo { set; get; }

        [Column(TypeName = "int")]
        public int TipoGrupoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        public virtual ICollection<clsSubGruposBE> SubGrupos { set; get; }
        public virtual clsTipoGruposBE TipoGrupos { set; get; }
    }
}