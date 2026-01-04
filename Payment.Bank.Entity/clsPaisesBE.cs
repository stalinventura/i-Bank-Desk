using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Paises")]
	public class clsPaisesBE: IDataErrorInfo
    {
		public clsPaisesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int PaisID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Pais { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Nacionalidad { get; set; }

        [Column(TypeName = "int")]
        public int MonedaID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsProvinciasBE> Provincias { get; set; }
        public virtual clsMonedasBE Monedas { get; set; }


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
                    case "Pais":
                        if (string.IsNullOrEmpty(Pais))
                        {
                            return "Es necesario el pais.";
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