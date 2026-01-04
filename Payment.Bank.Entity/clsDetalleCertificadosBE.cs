using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleCertificados")]
	public class clsDetalleCertificadosBE
	{
		public clsDetalleCertificadosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DetalleCertificadoID { set; get; }

        [Column(TypeName = "int")]
        public int CertificadoID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Descripcion { set; get; }

        [Column(TypeName = "float")]
        public float Interes { set; get; }

        [Column(TypeName = "float")]
        public float Monto { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsCertificadosBE Certificados { get; set; }


    }
}