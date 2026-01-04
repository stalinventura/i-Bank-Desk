using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Clientes")]
	public class clsClientesBE
	{
		public clsClientesBE()
		{
		}


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int ClienteID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName ="nvarchar")]
         [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "int")]
        public int RutaID { set; get; }
        
        //[Column(TypeName = "float")]
        public double Latitude { get; set; }

        //[Column(TypeName = "float")]
        public double Longitude { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsPersonasBE Personas { get; set; }
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual ICollection<clsSolicitudesBE> Solicitudes { get; set; }
        public virtual ICollection<clsCuentasBE> Cuentas { get; set; }
        //public virtual clsRutasBE Rutas { get; set; }
    }
}