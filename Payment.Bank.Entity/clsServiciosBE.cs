using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Servicios")]
	public class clsServiciosBE: IDataErrorInfo
    {
		public clsServiciosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ServicioID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [MaxLength(100)]
        public string EmpresaID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Servicio { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Imagen { get; set; }

        [Column(TypeName = "bit")]
        public Boolean EstadoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsEmpresasBE Empresas { get; set; }

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
                 

                    case "Servicio":
                        if (String.IsNullOrEmpty(Servicio))
                        {
                            return "Es necesario el Servicio electrónico";
                        }
                        else
                        {
                            string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
                            Regex re = new Regex(emailExpression);
                            if (!re.IsMatch(Servicio))
                            {
                                return "Es invalido el formato de Servicio ";
                            }
                            else
                            {
                                goto default;
                            }
                        }

                    default:
                        return null;
                }
            }
        }
        #endregion  
    }
}