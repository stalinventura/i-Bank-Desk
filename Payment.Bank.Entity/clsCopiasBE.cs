using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Copias")]
	public class clsCopiasBE
	{
		public clsCopiasBE()
		{
		}

        [Key]
        [ForeignKey("Empresas")]
        [MaxLength(100)]
        public string EmpresaID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Host { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string User { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Database { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Url { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsEmpresasBE Empresas { get; set; }


    }
}