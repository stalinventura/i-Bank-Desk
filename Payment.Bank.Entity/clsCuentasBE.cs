using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Cuentas")]
	public class clsCuentasBE:IDataErrorInfo
    {
		public clsCuentasBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int CuentaID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Cuenta { get; set; }

        public int ClienteID { get; set; }

        public int TipoCuentaID { set; get; }

        [Column(TypeName = "bit")]
        public Boolean EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Motivo { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsTipoCuentasBE TipoCuentas { get; set; }
        public virtual clsClientesBE Clientes { get; set; }
        public virtual ICollection<clsTransaccionesBE> Transacciones { get; set; }

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
                    case "TipoCuentaID":
                        if ((int)TipoCuentaID == -1)
                        {
                            return "Es necesario seleccionar un tipo de Cuenta.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Cuenta":
                        if (string.IsNullOrEmpty(Cuenta.ToString()))
                        {
                            return "Es necesario especificar el numero de cuenta";
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