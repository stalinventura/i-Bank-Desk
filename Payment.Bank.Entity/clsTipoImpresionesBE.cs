using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("TipoImpresiones")]
	public class clsTipoImpresionesBE
	{
		public clsTipoImpresionesBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int TipoImpresionID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Descripcion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsImpresionesBE> Impresiones { get; set; }


    }
}