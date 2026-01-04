using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Empresas")]
	public class clsEmpresasBE: IDataErrorInfo
    {
		public clsEmpresasBE()
		{
		}

		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(100)]
        public string EmpresaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Key { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Empresa { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Rnc { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Siglas { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Payment { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Device { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string CSR { get; set; }

        public byte[] Logo { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Site { get; set; }

        [Column(TypeName = "int")]
        public int PlanID { get; set; }

        [Column(TypeName = "int")]
        public int TipoEmpresaID { get; set; }

        [Column(TypeName = "int")]
        public int PaqueteID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsSucursalesBE> Sucursales { get; set; }
        public virtual ICollection<clsCorreosBE> Correos { get; set; }
        public virtual ICollection<clsTareasBE> Tareas { get; set; }
        public virtual ICollection<clsCartasBE> Cartas { get; set; }
        public virtual clsCopiasBE Copias { get; set; }
        public virtual ICollection<clsServiciosBE> Servicios { get; set; }
        public virtual clsPlanesBE Planes { get; set; }
        public virtual ICollection<clsDispositivosBE> Dispositivos { get; set; }
        public virtual clsTipoEmpresasBE TipoEmpresas { get; set; }
        public virtual ICollection<clsPasarelasBE> Pasarelas { get; set; }
        public virtual clsPaquetesBE Paquetes { get; set; }

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
                    case "EmpresaID":
                        if (string.IsNullOrEmpty(EmpresaID))
                        {
                            return "Es necesario el codigo de la empresa.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "TipoEmpresaID":
                        if ((int)TipoEmpresaID == -1)
                        {
                            return "Es necesario seleccionar un tipo de empresa.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Empresa":
                        if (string.IsNullOrEmpty(Empresa))
                        {
                            return "Es necesario el nombre de la empresa";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Siglas":
                        if (string.IsNullOrEmpty(Siglas))
                        {
                            return "Es necesario especificar una sigla";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Rnc":
                        if (string.IsNullOrEmpty(Rnc))
                        {
                            return "Es necesario especificar un Rnc";
                        }
                        else
                        {
                            goto default;
                        }
                    case "CSR":
                        if (string.IsNullOrEmpty(CSR))
                        {
                            return "Es necesario especificar el numero CSR";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Device":
                        if (string.IsNullOrEmpty(Device))
                        {
                            return "Es necesario especificar la cantidad de dispositivos";
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