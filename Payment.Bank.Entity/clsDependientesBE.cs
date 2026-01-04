using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Dependientes")]
	public class clsDependientesBE: IDataErrorInfo
	{
		public clsDependientesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int DependienteID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaNacimiento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Dependiente { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Telefono { set; get; }

        [Column(TypeName = "int")]
        public int ParentescoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsPersonasBE Personas { set; get; }
        public virtual clsParentescosBE Parentescos { get; set; }

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
                    case "Dependiente":
                        if (string.IsNullOrEmpty(Dependiente))
                        {
                            return "Es necesario el nombre del dependiente";
                        }
                        else
                        {
                            goto default;
                        }

                    case "DependienteID":
                        if (DependienteID == -1)
                        {
                            return "Es necesario seleccionar un dependiente";
                        }
                        else
                        {
                            goto default;
                        }

                    case "ParentescoID":
                        if (ParentescoID == -1)
                        {
                            return "Es necesario seleccionar un parentesco";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Telefono":
                        if (string.IsNullOrEmpty(Telefono))
                        {
                            return "Es necesario el telefono del dependiente";
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