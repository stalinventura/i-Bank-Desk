using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Host.Entity
{
    [Table("Empresas")]
	public class clsEmpresasBE
	{
		public clsEmpresasBE()
		{
		}

		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string EmpresaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Key { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Empresa { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Telefonos { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Rnc { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Siglas { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Url { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Local { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Payment { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string CSR { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Logo { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Site { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Correo { get; set; }

        [Column(TypeName = "int")]
        public int DroidBuild { get; set; }

        [Column(TypeName = "int")]
        public int iOSBuild { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Device { get; set; }

        [Column(TypeName = "bit")]
        public bool ClientAccess { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Distance { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsHistorialBE> Historial { get; set; }
    }
}