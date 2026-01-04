using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Referencias")]
	public class clsReferenciasBE:IDataErrorInfo
	{
		public clsReferenciasBE()
		{
		}


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int ReferenciaID { set; get; }

        [Column(TypeName = "int")]
        public int TipoReferenciaID { set; get; }

        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Referencia { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Telefono { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsPersonasBE Personas { set; get; }
        public virtual clsTipoReferenciasBE TipoReferencias { set; get; }

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
                    case "Referencia":
                        if (string.IsNullOrEmpty(Referencia))
                        {
                            return "Es necesario el nombre de la referencia.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Direccion":
                        if (string.IsNullOrEmpty(Direccion))
                        {
                            return "Es necesario la direccion de la referencia.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Telefono":
                        if (string.IsNullOrEmpty(Telefono))
                        {
                            return "Es necesario el telefono de la referencia.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ReferenciaID":
                        if ((int)ReferenciaID == -1)
                        {
                            return "Es necesario seleccionar una referencia.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "TipoReferenciaID":
                        if ((int)TipoReferenciaID == -1)
                        {
                            return "Es necesario seleccionar un tipo de referencia.";
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