using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Sucursales")]
	public class clsSucursalesBE : IDataErrorInfo
    {
		public clsSucursalesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName ="int")]
		public int SucursalID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Sucursal { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string EmpresaID { set; get; }

        [Column(TypeName = "int")]
        public int CiudadID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Telefonos { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }
        
        public virtual clsEmpresasBE Empresas { get; set; }
        public virtual clsCiudadesBE Ciudades { get; set; }
        public virtual ICollection<clsClientesBE> Clientes { get; set; }
        public virtual ICollection<clsUsuariosBE> Usuarios { get; set; }
        public virtual clsGerentesBE Gerentes { get; set; }
        public virtual ICollection<clsNotariosPublicoBE> NotariosPublico { get; set; }
        public virtual clsConfiguracionesBE Configuraciones { get; set; }
        public virtual ICollection<clsOficialesBE> Oficiales { get; set; }
        public virtual ICollection<clsRecibosBE> Recibos { get; set; }
        public virtual ICollection<clsTransaccionesBE> Transacciones { get; set; }
        public virtual ICollection<clsCertificadosBE> Certificados { get; set; }
        public virtual ICollection<clsBancosBE> Bancos { get; set; }
        public virtual ICollection<clsPagoRentasBE> PagoRentas { get; set; }
        public virtual clsAbogadosBE Abogados { get; set; }
        public virtual clsAlguacilesBE Alguaciles { get; set; }
        public virtual ICollection<clsEntradasBE> Entradas { get; set; }
        public virtual ICollection<clsDesembolsosBE> Desembolsos { get; set; }
        public virtual ICollection<clsEmpleadosBE> Empleados { get; set; }

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
                        if ((int)SucursalID == -1)
                        {
                            return "Es necesario seleccionar una sucursal.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "CiudadID":
                        if ((int)CiudadID == -1)
                        {
                            return "Es necesario seleccionar una ciudad.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "EmpresaID":
                        if (string.IsNullOrEmpty(EmpresaID))
                        {
                            return "Es necesario especificar una empresa";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Sucursal":
                        if (string.IsNullOrEmpty(Sucursal))
                        {
                            return "Es necesario especificar una sucursal";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Direccion":
                        if (string.IsNullOrEmpty(Direccion))
                        {
                            return "Es necesario especificar una direccion";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Telefonos":
                        if (string.IsNullOrEmpty(Telefonos))
                        {
                            return "Es necesario especificar un telefono";
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