using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("DetalleNominas")]
	public class clsDetalleNominasBE
	{
		public clsDetalleNominasBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetalleNominaID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "int")]
        public int NominaID { set; get; }

        [Column(TypeName = "int")]
        public int EmpleadoID { set; get; }

        [Column(TypeName = "float")]
        public float Sueldo { set; get; }

        [Column(TypeName = "float")]
        public float Comision { set; get; }

        [Column(TypeName = "float")]
        public float SFS { set; get; }

        [Column(TypeName = "float")]
        public float AFP { set; get; }

        [Column(TypeName = "float")]
        public float ISR { set; get; }

        [Column(TypeName = "float")]
        public float Otros { set; get; }

        [Column(TypeName = "float")]
        public float SubTotal { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsNominasBE Nominas { get; set; }
        public virtual clsEmpleadosBE Empleados { get; set; }

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
                    case "EmpleadoID":
                        if (EmpleadoID == -1)
                        {
                            return "Es necesario seleccionar un empleado";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Sueldo":
                        if (Sueldo <= 0)
                        {
                            return "Es necesario el sueldo del empleado";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Comision":
                        if (Comision <= 0)
                        {
                            return "Es necesario la comision del empleado si aplica.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "SFS":
                        if (SFS <= 0)
                        {
                            return "Es necesario el monto de seguro familiar de salud del empleado";
                        }
                        else
                        {
                            goto default;
                        }

                    case "AFP":
                        if (AFP <= 0)
                        {
                            return "Es necesario el monto del Fondo de Pensiones del empleado";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ISR":
                        if (ISR <= 0)
                        {
                            return "Es necesario el monto de impuesto sobre la renta del empleado";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Otros":
                        if (Otros <= 0)
                        {
                            return "Es necesario el monto otros descuentos del empleado si aplica.";
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