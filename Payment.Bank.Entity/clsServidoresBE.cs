using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Servidores")]
	public class clsServidoresBE
	{
		public clsServidoresBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ServidorID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Smtp { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Descripcion { set; get; }

        public int Puerto { set; get; }

        public bool EnableSsl { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsCorreosBE> Correos { get; set; }


    }
}