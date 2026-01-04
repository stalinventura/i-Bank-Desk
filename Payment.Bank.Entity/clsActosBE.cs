using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Actos")]
	public class clsActosBE:IDataErrorInfo
	{
		public clsActosBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActoID { get; set; }

        [Column(TypeName = "int")]
        public int NotarioPublicoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int Ano { get; set; }

        [Column(TypeName = "int")]
        public int Acto { get; set; }

        [Column(TypeName = "int")]
        public int Secuencia { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsNotariosPublicoBE NotariosPublico { get; set; }


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
                    
                    default:
                        return null;
                }
            }
        }
        #endregion  

    }
}