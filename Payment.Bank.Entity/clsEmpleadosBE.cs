using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Empleados")]
	public class clsEmpleadosBE:IDataErrorInfo
	{
		public clsEmpleadosBE()
		{
		}



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int EmpleadoID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        public string Documento { set; get; }
        
        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "int")]
        public int PuestoID { set; get; }

        [Column(TypeName = "float")]
        public float Sueldo { set; get; }

        [Column(TypeName = "bit")]
        public bool EstadoID { set; get; }

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
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual clsPuestosBE Puestos { get; set; }
        public virtual ICollection<clsDetalleNominasBE> DetalleNominas { get; set; }


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

                    case "SucursalID":
                        if (SucursalID == -1)
                        {
                            return "Es necesario seleccionar una sucursal";
                        }
                        else
                        {
                            goto default;
                        }

                    case "PuestoID":
                        if (PuestoID == -1)
                        {
                            return "Es necesario seleccionar un puesto de trabajo";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Sueldo":
                        if (Sueldo == 0)
                        {
                            return "Es necesario el sueldo del empleado(a)";
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