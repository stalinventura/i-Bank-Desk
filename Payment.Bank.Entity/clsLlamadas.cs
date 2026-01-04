using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Llamadas")]
	public class clsLlamadasBE: IDataErrorInfo
    {
		public clsLlamadasBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        public int LlamadaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int ContratoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string HableCon { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Mensaje { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsContratosBE Contratos { get; set; }


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
                    case "Mensaje":
                        if (string.IsNullOrEmpty(Mensaje))
                        {
                            return "Es necesario el mensaje.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "LlamadaID":
                        if ((int)LlamadaID == -1)
                        {
                            return "Es necesario seleccionar un Llamada.";
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