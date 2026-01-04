using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Desembolsos")]
	public class clsDesembolsosBE:IDataErrorInfo
	{
		public clsDesembolsosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DesembolsoID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Beneficiario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Concepto { set; get; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "float")]
        public float Monto { set; get; }

        [Column(TypeName = "bit")]
        public Boolean EstadoID { set; get; }

        [Column(TypeName = "bit")]
        public Boolean Contabilizado { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual ICollection<clsDetalleDesembolsosBE> DetalleDesembolsos { get; set; }

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
                    case "Monto":
                        if ((decimal)Monto == 0)
                        {
                            return "Es necesario especificar el monto";
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