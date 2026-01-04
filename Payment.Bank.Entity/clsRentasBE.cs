using Payment.Bank.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Rentas")]
	public class clsRentasBE: IDataErrorInfo
	{
		public clsRentasBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RentaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int AlquilerID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Concepto { get; set; }

        [Column(TypeName = "int")]
        public int MesID { get; set; }

        [Column(TypeName = "int")]
        public int Año { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "bit")]
        public bool IsDeposito { get; set; }

        [Column(TypeName = "bit")]
        public bool Contabilizado { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }


        //Relaciones
        public virtual clsAlquileresBE Alquileres { get; set; }
        public virtual ICollection<clsDetallePagoRentasBE> DetallePagoRentas { get; set; }


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
                //if (columnName == "FormaPagoID")
                //{
                //    if ((int)FormaPagoID == -1)
                //        result = "Seleccione una forma de pago";
                //}

                //if (columnName == "IdComprobante")
                //{
                //    if ((int)ComprobanteID == -1)
                //        result = "Seleccione un tipo de comprobante";
                //}

                return result;
            }
        }

        #endregion
    }
}