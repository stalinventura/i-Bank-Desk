using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("GarantiaHipotecaria")]
    public class clsGarantiaHipotecariaBE : IDataErrorInfo
    {
        public clsGarantiaHipotecariaBE()
        {
        }


        [Key]
        [ForeignKey("Solicitudes")]
        [Column(TypeName = "int")]
        public int SolicitudID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string Descripcion { set; get; }

        [Column(TypeName = "float")]
        public float Monto { set; get; }

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
                    case "Descripcion":
                        if (string.IsNullOrEmpty(Descripcion))
                        {
                            return "Es necesario la descripcion del inmueble";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Monto":
                        if ((decimal)Monto == 0)
                        {
                            return "Es necesario el monto del inmueble";
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