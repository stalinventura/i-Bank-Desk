using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Tareas")]
	public class clsTareasBE:IDataErrorInfo
    {
		public clsTareasBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int TareaID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [MaxLength(100)]
        public string EmpresaID { set; get; }

        public int TipoTareaID { set; get; }

        public int Dia { get; set; }

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
        public virtual clsTipoTareasBE TipoTareas { get; set; }

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
                    case "TipoTareaID":
                        if ((int)TipoTareaID == -1)
                        {
                            return "Es necesario seleccionar un tipo de tarea.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Dia":
                        if (string.IsNullOrEmpty(Dia.ToString()))
                        {
                            return "Es necesario especificar los dias de anticipación o posteriores a la tarea";
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