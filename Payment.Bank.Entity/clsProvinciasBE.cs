using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Provincias")]
	public class clsProvinciasBE: IDataErrorInfo
    {
		public clsProvinciasBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProvinciaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Provincia { get; set; }

        [Column(TypeName = "int")]
        public int PaisID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPaisesBE Paises { get; set; }
        public virtual ICollection<clsCiudadesBE> Ciudades { get; set; }


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
                    case "Provincia":
                        if (string.IsNullOrEmpty(Provincia))
                        {
                            return "Es necesario la provincia.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ProvinciaID":
                        if ((int)ProvinciaID == -1)
                        {
                            return "Es necesario seleccionar una provincia.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "PaisID":
                        if ((int)PaisID == -1)
                        {
                            return "Es necesario seleccionar un pais.";
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