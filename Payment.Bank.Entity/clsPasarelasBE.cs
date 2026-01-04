using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Pasarelas")]
    public class clsPasarelasBE: IDataErrorInfo
    {
        public clsPasarelasBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PasarelaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string EmpresaID { get; set; }

        [Column(TypeName = "int")]
        public int ProcesadorID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string MerchantID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string KeyID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string SecretKey { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsDefault { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsEmpresasBE Empresas { get; set; }
        public virtual clsProcesadoresBE Procesadores { get; set; }

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
                    case "MerchantID":
                        if (string.IsNullOrEmpty(MerchantID))
                        {
                            return "Es necesario el MerchantID de la empresa.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "KeyID":
                        if (string.IsNullOrEmpty(KeyID))
                        {
                            return "Es necesario el KeyID de la empresa.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "SecretKey":
                        if (string.IsNullOrEmpty(SecretKey))
                        {
                            return "Es necesario el SecretKey de la empresa.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ProcesadorID":
                        if ((int)ProcesadorID == -1)
                        {
                            return "Es necesario seleccionar un procesador de pago.";
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