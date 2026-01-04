using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Entradas")]
	public class clsEntradasBE: IDataErrorInfo
    {
		public clsEntradasBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EntradaID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "int")]
        public int TipoMovimientoID { set; get; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Referencia { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Concepto { set; get; }

        [Column(TypeName = "float")]
        public float Debito { set; get; }

        [Column(TypeName = "float")]
        public float Credito { set; get; }

        [Column(TypeName = "bit")]
        public bool EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }


        //Relaciones
        public virtual clsTipoMovimientosBE TipoMovimientos { set; get; }
        public virtual ICollection<clsDetalleEntradasBE> DetalleEntradas { set; get; }
        public virtual clsSucursalesBE Sucursales { set; get; }

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
                    case "TipoMovimientoID":
                        if ((int)TipoMovimientoID == -1)
                        {
                            return "Es necesario seleccionar un tipo de movimiento.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Referencia":
                        if (string.IsNullOrEmpty(Referencia))
                        {
                            return "Es necesario un numero de referencia.";
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
                    case "Debito":
                        if ((double)Debito <= 0)
                        {
                            return "Es necesario el monto del debito";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Credito":
                        if ((double)Credito <= 0)
                        {
                            return "Es necesario el monto del credito";
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