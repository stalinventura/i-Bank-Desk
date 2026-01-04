using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("FormaPagos")]
	public class clsFormaPagosBE: IDataErrorInfo
	{
		public clsFormaPagosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int FormaPagoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }


        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Nombre { get; set; }

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
        
        //Relaciones
        public virtual ICollection<clsRecibosBE> Recibos { get; set; }
        public virtual ICollection<clsTransaccionesBE> Transacciones { get; set; }
        public virtual clsAuxiliaresBE Auxiliares { get; set; }

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
                    case "Nombre":
                        if (string.IsNullOrEmpty(Nombre))
                        {
                            return "Es necesario la forma de pago.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "FormaPagoID":
                        if ((int)FormaPagoID == -1)
                        {
                            return "Es necesario seleccionar una forma de pago.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "AuxiliarID":
                        if ((int)AuxiliarID == -1)
                        {
                            return "Es necesario seleccionar un auxiliar.";
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