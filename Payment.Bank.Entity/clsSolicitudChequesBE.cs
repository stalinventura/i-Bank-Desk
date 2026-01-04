using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("SolicitudCheques")]
	public class clsSolicitudChequesBE: IDataErrorInfo
    {
		public clsSolicitudChequesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int SolicitudChequeID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int? ContratoID { get; set; }

        [Column(TypeName = "int")]
        public int BancoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Beneficiario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Concepto { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "int")]
        public int EstadoID { get; set; }

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
        public virtual ICollection<clsDetalleSolicitudChequesBE> DetalleSolicitudCheques { get; set; }
        public virtual clsEstadoSolictudChequesBE Estados { get; set; }
        public virtual clsBancosBE Bancos { get; set; }
        public virtual clsChequesBE Cheques { get; set; }

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
                    case "Beneficiario":
                        if (string.IsNullOrEmpty(Beneficiario))
                        {
                            return "Es necesario un beneficiario.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Concepto":
                        if (string.IsNullOrEmpty(Concepto))
                        {
                            return "Es necesario un concepto.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "BancoID":
                        if ((int)BancoID == -1)
                        {
                            return "Es necesario seleccionar un banco.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Monto":
                        if ((decimal)Monto == 0)
                        {
                            return "Es necesario especificar el monto de la solicitud";
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