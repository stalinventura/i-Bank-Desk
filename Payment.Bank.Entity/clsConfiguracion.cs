using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Configuraciones")]
	public class clsConfiguracionesBE: IDataErrorInfo
    {
		public clsConfiguracionesBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        public int SucursalID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName ="bit")]
        public bool HasNetworkAccess { get; set; }

        [Column(TypeName = "bit")]
        public bool HasAutentication { get; set; }

        [Column(TypeName = "bit")]
        public bool IsAutomaticAccountting { get; set; }

        [Column(TypeName = "bit")]
        public bool IsMultipleRequest { get; set; }

        [Column(TypeName = "bit")]
        public bool SetLogo { get; set; }

        [Column(TypeName = "bit")]
        public bool GetLogo { get; set; }

        [Column(TypeName = "bit")]
        public bool IsOpenCloseBox { get; set; }

        [Column(TypeName = "int")]
        public int PettyCash { get; set; }

        [Column(TypeName = "bit")]
        public bool IsEasyContract { get; set; }

        [Column(TypeName = "bit")]
        public bool ValidarDocumento { get; set; }

        [Column(TypeName = "bit")]
        public bool ValidateContractByClient { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsPayPoint { set; get; }

        [Column(TypeName = "float")]
        public float MinimumAmount { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string smsHost { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string smsUrl { get; set; }

        [Column(TypeName = "bit")]
        public bool LocalNotification { get; set; }

        [Column(TypeName = "bit")]
        public bool RemoteNotification { get; set; }

        [Column(TypeName = "bit")]
        public bool SmsNotification { get; set; }

        [Column(TypeName = "bit")]
        public bool EmailNotification { get; set; }

        [Column(TypeName = "bit")]
        public bool NotificarCelular { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string msgNotificarCelular { get; set; }

        [Column(TypeName = "bit")]
        public bool NotificarSolicitud { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string msgNotificarSolicitud { get; set; }

        [Column(TypeName = "bit")]
        public bool NotificarContrato { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string msgNotificarContrato { get; set; }

        [Column(TypeName = "bit")]
        public bool NotificarRecibo { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string msgNotificarRecibo { get; set; }

        [Column(TypeName = "bit")]
        public bool NotificarAlquiler { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string msgNotificarAlquiler { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Informacion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual ICollection<clsImpresionesBE> Impresiones { get; set; }
        public virtual ICollection<clsContabilizacionesBE> Contabilizaciones { get; set; }

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

                    default:
                        return null;
                }
            }
        }
        #endregion  
    }
}