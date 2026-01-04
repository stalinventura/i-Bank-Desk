using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Permisos")]
    public class clsPermisosBE
	{
		public clsPermisosBE()
		{

		}


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int PermisoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Codigo { get; set; }

        [Column(TypeName = "int")]
        public int ProductoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Permiso { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Url { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Icon { get; set; }

        [Column(TypeName = "int")]
        public int IdPadre { get; set; }

        [Column(TypeName = "int")]
        public int Orden { get; set; }

        [Column(TypeName = "bit")]
        public bool Visible { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsProductosBE Productos { get; set; }
        public virtual ICollection<clsPermisosRolesBE> PermisosRoles { get; set; }
    }
}