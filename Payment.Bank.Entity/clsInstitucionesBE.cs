using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Instituciones")]
	public class clsInstitucionesBE: IDataErrorInfo
    {
		public clsInstitucionesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int InstitucionID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Institucion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Telefono { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { get; set; }
        public virtual ICollection<clsDatosEconomicosBE> DatosEconomicos { get; set; }


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
                    case "Institucion":
                        if (string.IsNullOrEmpty(Institucion))
                        {
                            return "Es necesario el nombre Institucion.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Direccion":
                        if (string.IsNullOrEmpty(Direccion))
                        {
                            return "Es necesario la direccion de la Institucion.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Telefono":
                        if (string.IsNullOrEmpty(Telefono))
                        {
                            return "Es necesario el telefono de la Institucion.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "InstitucionID":
                        if ((int)InstitucionID == -1)
                        {
                            return "Es necesario seleccionar un Institucion.";
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