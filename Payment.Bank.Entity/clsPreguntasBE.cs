using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Preguntas")]
	public class clsPreguntasBE:IDataErrorInfo
	{
		public clsPreguntasBE()
		{
            Usuarios = new HashSet<clsUsuariosBE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int PreguntaID { get; set; }

        [Column(TypeName ="DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Pregunta { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsUsuariosBE> Usuarios { get; set; }

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
                    case "Pregunta":
                        if (string.IsNullOrEmpty(Pregunta))
                        {
                            return "Es necesario la pregunta.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "PreguntaID":
                        if ((int)PreguntaID == -1)
                        {
                            return "Es necesario seleccionar una pregunta.";
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