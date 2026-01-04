using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Cheques")]
	public class clsChequesBE:IDataErrorInfo
    {
		public clsChequesBE()
		{
		}


        [Key]
        [ForeignKey("SolicitudCheques")]
        [Column(TypeName = "int")]
        public int SolicitudChequeID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        public int Numero { set; get; }

        [Column(TypeName = "bit")]
        public bool EstadoID { get; set; }

        [Column(TypeName = "bit")]
        public bool Contabilizado { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsSolicitudChequesBE SolicitudCheques { get; set; }

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
                    case "SolicitudChequeID":
                        if ((int)SolicitudChequeID == -1)
                        {
                            return "Es necesario el numero de cheque";
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