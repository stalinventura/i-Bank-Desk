using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DatosEconomicos")]
	public class clsDatosEconomicosBE:IDataErrorInfo
	{
		public clsDatosEconomicosBE()
		{
		}

        [Key]
        [ForeignKey("Personas")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "bit")]
        public Boolean Trabaja { set; get; }

        [Column(TypeName = "int")]
        public int InstitucionID { set; get; }

        //[Column(TypeName = "nvarchar")]
        //[MaxLength(50)]
        //public string Empresa { set; get; }

        //[Column(TypeName = "nvarchar")]
        //[MaxLength(200)]
        //public string Direccion { set; get; }

        //[Column(TypeName = "nvarchar")]
        //[MaxLength(13)]
        //public string Telefono { set; get; }

        [Column(TypeName = "int")]
        public int OcupacionID { set; get; }

        [Column(TypeName = "int")]
        public int HorarioID { set; get; }

        [Column(TypeName = "int")]
        public int IngresoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsPersonasBE Personas { get; set; }
        public virtual clsIngresosBE Ingresos { get; set; }
        public virtual clsOcupacionesBE Ocupaciones { get; set; }
        public virtual clsHorariosBE Horarios { get; set; }
        public virtual clsInstitucionesBE Instituciones { get; set; }

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

                    case "InstitucionID":
                        if ((int)InstitucionID == -1)
                        {
                            return "Es necesario seleccionar una empresa.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "OcupacionID":
                        if ((int)OcupacionID == -1)
                        {
                            return "Es necesario seleccionar una ocupacion.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "IngresoID":
                        if ((int)IngresoID == -1)
                        {
                            return "Es necesario seleccionar un rango de ingresos.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "HorarioID":
                        if ((int)HorarioID == -1)
                        {
                            return "Es necesario seleccionar un horario.";
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