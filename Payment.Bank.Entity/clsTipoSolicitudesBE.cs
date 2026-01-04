using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("TipoSolicitudes")]
	public class clsTipoSolicitudesBE:IDataErrorInfo
	{
		public clsTipoSolicitudesBE()
		{
            
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TipoSolicitudID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string TipoSolicitud { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Imagen { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsDefault { set; get; }

        [Column(TypeName = "int")]
        public int? AuxiliarID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        public virtual ICollection<clsSolicitudesBE> Solicitudes { set; get; }
        public virtual clsAuxiliaresBE Auxiliares { set; get; }

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
                    case "TipoSolicitud":
                        if (string.IsNullOrEmpty(TipoSolicitud))
                        {
                            return "Es necesario el tipo de solicitud.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "TipoSolicitudID":
                        if ((int)TipoSolicitudID == -1)
                        {
                            return "Es necesario seleccionar un tipo de solicitud.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "AuxiliarID":
                        if ((int)AuxiliarID == -1)
                        {
                            return "Es necesario seleccionar un auxiliar";
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