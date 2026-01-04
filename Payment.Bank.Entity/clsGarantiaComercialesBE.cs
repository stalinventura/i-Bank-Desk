using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("GarantiaComerciales")]
	public class clsGarantiaComercialesBE:IDataErrorInfo
	{
		public clsGarantiaComercialesBE()
		{
		}


        [Key]
        [ForeignKey("Solicitudes")]
        [Column(TypeName = "int")]
        public int SolicitudID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Rnc { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsSolicitudesBE Solicitudes { get; set; }
        public virtual clsComerciosBE Comercios { get; set; }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Rnc":
                        if (string.IsNullOrEmpty(Rnc))
                        {
                            return "Es necesario el Rnc del comercio o  negocio.";
                        }
                        else
                        {
                            goto default;
                        }
                    default:
                        return null;
                }
            }
        }
        #endregion  

    }
}