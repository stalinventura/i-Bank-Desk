using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Monedas")]
	public class clsMonedasBE
	{
		public clsMonedasBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int MonedaID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Codigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Moneda { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsPaisesBE> Paises { get; set; }
        public virtual ICollection<clsCertificadosBE> Certificados { get; set; }
        public virtual ICollection<clsTipoCuentasBE> TipoCuentas { get; set; }

    }
}