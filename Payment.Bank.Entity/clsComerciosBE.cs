using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Comercios")]
	public class clsComerciosBE: IDataErrorInfo
    {
		public clsComerciosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Rnc { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Comercio { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Telefono { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsGarantiaComercialesBE> GarantiaComerciales { get; set; }


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
                    case "Comercio":
                        if (string.IsNullOrEmpty(Comercio))
                        {
                            return "Es necesario el nombre del comercio o negocio.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Direccion":
                        if (string.IsNullOrEmpty(Direccion))
                        {
                            return "Es necesario la direccion del comercio o negocio.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Telefono":
                        if (string.IsNullOrEmpty(Telefono))
                        {
                            return "Es necesario el telefono del comercio o negocio.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Rnc":
                        if (string.IsNullOrEmpty(Rnc))
                        {
                            return "Es necesario el Rnc del comercio o negocio.";
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