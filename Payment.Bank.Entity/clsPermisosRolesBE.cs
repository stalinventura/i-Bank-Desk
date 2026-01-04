using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("PermisosRoles")]
	public class clsPermisosRolesBE
	{
		public clsPermisosRolesBE()
		{
		}
        

        [Key]
        [Column(Order = 0)]
        [ForeignKey("Roles")]
        public int RolID { set; get; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Permisos")]
        public int PermisoID { set; get; }

        [Column(TypeName = "bit")]
        public bool Agregar { get; set; }

        [Column(TypeName = "bit")]
        public bool Modificar { get; set; }

        [Column(TypeName = "bit")]
        public bool Imprimir { get; set; }

        [Column(TypeName = "bit")]
        public bool Eliminar { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsRolesBE Roles { get; set; }
        public virtual clsPermisosBE Permisos { get; set; }

    }
}