using Payment.Bank.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Recibos")]
	public class clsRecibosBE: IDataErrorInfo
	{
		public clsRecibosBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReciboID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int ContratoID { get; set; }

        [Column(TypeName = "int")]
        public int SucursalID { get; set; }

        [Column(TypeName = "int")]
        public int ComprobanteID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        public string Ncf { get; set; }

        [Column(TypeName = "int")]
        public int FormaPagoID { get; set; }

        [Column(TypeName = "float")]
        public float SubTotal { get; set; }

        [Column(TypeName = "float")]
        public float Descuento { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "float")]
        public float Total { get; set; }

        [Column(TypeName = "float")]
        public float Cambio { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Informacion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string TokenID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Motivo { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { get; set; }

        [Column(TypeName = "bit")]
        public bool Contabilizado { get; set; }

        //[Column(TypeName = "nvarchar")]
        //[MaxLength(50)]
        //public string TokenID => $"{ContratoID}-{Fecha.Month}{Fecha.Day}{Fecha.Year}{Fecha.Hour}{Fecha.Minute}{Fecha.Second}-{SucursalID}-{Monto}-{Usuario}";

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
        public virtual ICollection<clsDetalleRecibosBE> DetalleRecibos { get; set; }
        public virtual clsFormaPagosBE FormaPagos { get; set; }
        public virtual clsComprobantesBE Comprobantes { get; set; }
        public virtual clsSucursalesBE Sucursales { get; set; }

        #region IDataErrorInfo Members
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                if (columnName == "FormaPagoID")
                {
                    if ((int)FormaPagoID == -1)
                        result = "Seleccione una forma de pago";
                }

                if (columnName == "ComprobanteID")
                {
                    if ((int)ComprobanteID == -1)
                        result = "Seleccione un tipo de comprobante";
                }

                if (columnName == "Motivo")
                {
                    if (string.IsNullOrEmpty(Motivo))
                    {
                        result = "Es necesario el motivo de la suspencion del recibo";
                    }
                }

                return result;
            }
        }

        #endregion
    }
}