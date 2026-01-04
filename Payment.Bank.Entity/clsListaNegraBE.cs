using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("ListaNegra")]
	public class clsListaNegrasBE: IDataErrorInfo
    {
		public clsListaNegrasBE()
		{
		}

        [Key]
        [ForeignKey("Personas")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName ="DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
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
       
        public virtual clsPersonasBE Personas { get; set; }


        #region IDataErrorInfo Members
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;


                return result;
            }
        }

        #endregion
    }
}