using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Abogados")]
	public class clsAbogadosBE
	{
		public clsAbogadosBE()
		{
		}


        [Key]
        [Column(TypeName = "int")]
        public int SucursalID { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Exequatur { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { set; get; }
        public virtual clsSucursalesBE Sucursales { get; set; }

    }
}