using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Correos")]
	public class clsCorreosBE: IDataErrorInfo
    {
		public clsCorreosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int CorreoID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [MaxLength(100)]
        public string EmpresaID { set; get; }
        public int ServidorID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Correo { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Contraseña { get; set; }

        public bool IsDefault { set; get; }

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
        public virtual clsServidoresBE Servidores { get; set; }

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
                    case "ServidorID":
                        if ((int)ServidorID == -1)
                        {
                            return "Es necesario seleccionar un servidor.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Correo":
                        if (String.IsNullOrEmpty(Correo))
                        {
                            return "Es necesario el Correo electrónico";
                        }
                        else
                        {
                            string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
                            Regex re = new Regex(emailExpression);
                            if (!re.IsMatch(Correo))
                            {
                                return "Es invalido el formato de correo ";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "Contraseña":
                        if (string.IsNullOrEmpty(Contraseña))
                        {
                            return "Es necesario la Contraseña";
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