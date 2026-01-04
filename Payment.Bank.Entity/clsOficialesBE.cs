using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Oficiales")]
	public class clsOficialesBE: IDataErrorInfo
    {
		public clsOficialesBE()
		{
		}



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int OficialID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        public string Documento { set; get; }
        
        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "float")]
        public float Porciento { get; set; }

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
        public virtual clsPersonasBE Personas { get; set; }
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual ICollection<clsContratosBE> Contratos { get; set; }

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

                    case "SucursalID":
                        if ((int)SucursalID == -1)
                        {
                            return "Es necesario seleccionar una sucursal.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Pordentaje":
                        if ((float)Porciento < 0)
                        {
                            return "Es necesario el porciento.";
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