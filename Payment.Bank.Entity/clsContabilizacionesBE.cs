using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Contabilizaciones")]
	public class clsContabilizacionesBE: IDataErrorInfo
    {
		public clsContabilizacionesBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ContabilizacionID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Contabilizacion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Xml { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsConfiguracionesBE Configuraciones { get; set; }

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
                    case "ContabilizacionID":
                        if ((int)ContabilizacionID == -1)
                        {
                            return "Es necesario seleccionar un servidor.";
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