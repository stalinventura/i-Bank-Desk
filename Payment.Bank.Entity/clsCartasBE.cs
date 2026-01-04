using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Cartas")]
	public class clsCartasBE:IDataErrorInfo
	{
		public clsCartasBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CartaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [MaxLength(100)]
        public string EmpresaID { set; get; }

        public int TipoCartaID { get; set; }

        [Column(TypeName = "int")]
        public int Desde { get; set; }
        
        [Column(TypeName = "int")]
        public int Hasta { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsTipoCartasBE TipoCartas  { get; set; }
        public virtual clsEmpresasBE Empresas { get; set; }
        public virtual ICollection<clsHistorialCartasBE> HistorialCartas { get; set; }



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
                    case "Monto":
                        if ((decimal)Monto <= 0)
                        {
                            return "Es necesario el monto de la carta";
                        }
                        else
                        {
                            goto default;
                        }
                    
                    case "Desde":
                        if ((int)Desde <= 0)
                        {
                            return "Es necesario valor desde";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Hasta":
                        if ((int)Hasta <= 0)
                        {
                            return "Es necesario valor Hasta";
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