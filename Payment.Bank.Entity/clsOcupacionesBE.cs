using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Payment.Bank.Entity
{
    [Table("Ocupaciones")]
    public class clsOcupacionesBE:IDataErrorInfo
    {
        public clsOcupacionesBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OcupacionID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Ocupacion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
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
                    case "Ocupacion":
                        if (string.IsNullOrEmpty(Ocupacion))
                        {
                            return "Es necesario la ocupacion.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "OcupacionID":
                        if ((int)OcupacionID == -1)
                        {
                            return "Es necesario seleccionar una ocupacion.";
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