using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("SubGrupos")]
	public class clsSubGruposBE
	{
		public clsSubGruposBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubGrupoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Codigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string SubGrupo { set; get; }

        [Column(TypeName = "int")]
        public int GrupoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        public virtual clsGruposBE Grupos { set; get; }
        public virtual ICollection<clsCuentaControlBE> CuentasControl { set; get; }
    }
}