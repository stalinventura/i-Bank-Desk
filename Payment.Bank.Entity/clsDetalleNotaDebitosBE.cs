using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleNotaDebitos")]
	public class clsDetalleNotaDebitosBE:IDataErrorInfo
	{
		public clsDetalleNotaDebitosBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NotaDebitoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int CuotaID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Concepto { get; set; }

        [Column(TypeName = "float")]
        public float Capital { get; set; }

        [Column(TypeName = "float")]
        public float Comision { get; set; }

        [Column(TypeName = "float")]
        public float Interes { get; set; }

        [Column(TypeName = "float")]
        public float Mora { get; set; }

        [Column(TypeName = "float")]
        public float Legal { get; set; }

        [Column(TypeName = "float")]
        public float Seguro { get; set; }

        [Column(TypeName = "bit")]
        public bool IsAutomatic { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsCuotasBE Cuotas { get; set; }


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
                    case "Concepto":
                        if (string.IsNullOrEmpty(Concepto))
                        {
                            return "Es necesario el concepto de la nota de debito.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Capital":
                        if ((decimal)Capital <= 0)
                        {
                            return "Es necesario el monto del capital";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Comision":
                        if ((decimal)Comision <= 0)
                        {
                            return "Es necesario el monto de la comision";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Interes":
                        if ((decimal)Interes <= 0)
                        {
                            return "Es necesario el monto del interes";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Mora":
                        if ((decimal)Mora <= 0)
                        {
                            return "Es necesario el monto de la mora";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Legal":
                        if ((decimal)Legal <= 0)
                        {
                            return "Es necesario el monto del legal";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Seguro":
                        if ((decimal)Seguro <= 0)
                        {
                            return "Es necesario el monto del seguro";
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