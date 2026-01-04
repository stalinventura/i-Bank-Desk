using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Payment.Bank.Entity
{
    [Table("Productos")]
	public class clsProductosBE
	{
		public clsProductosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Producto { set; get; }

        [Column(TypeName = "int")]
        public int Orden { set; get; }

        [Column(TypeName = "bit")]
        public bool Visible { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }


        //Relaciones
        public virtual ICollection<clsPermisosBE> Permisos { get; set; }
    }
}