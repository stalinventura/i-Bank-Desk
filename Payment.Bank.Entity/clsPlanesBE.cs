using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Planes")]
	public class clsPlanesBE:IDataErrorInfo
    {
		public clsPlanesBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int PlanID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string Plan { get; set; }

        [Column(TypeName = "int")]
        public int Desde { set; get; }

        [Column(TypeName = "int")]
        public int Hasta { set; get; }

        [Column(TypeName = "float")]
        public float Monto { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual List<clsEmpresasBE> Empresas { get; set; }

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

                    case "Plan":
                        if (string.IsNullOrEmpty(Plan.ToString()))
                        {
                            return "Es necesario especificar el nombre del Plan";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Desde":
                        if ((int)Desde == 0)
                        {
                            return "Es necesario especificar el valor desde";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Hasta":
                        if ((int)Hasta == 0)
                        {
                            return "Es necesario especificar el valor hasta";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Monto":
                        if ((float)Monto == 0)
                        {
                            return "Es necesario especificar el monto del Plan";
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