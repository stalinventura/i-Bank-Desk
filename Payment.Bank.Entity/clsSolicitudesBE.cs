using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Solicitudes")]
	public class clsSolicitudesBE:IDataErrorInfo
	{
		public clsSolicitudesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]        
        public int SolicitudID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }
        
        [Column(TypeName = "int")]
        public int ClienteID { get; set; }
        
        [Column(TypeName = "int")]
        public int TipoSolicitudID { get; set; }

        [Column(TypeName = "int")]
        public int CondicionID { get; set; }

        [Column(TypeName = "int")]
        public int Tiempo { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Monto { get; set; }
        
        [Column(TypeName = "int")]
        public int EstadoID { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsEstadoSolicitudesBE Estados { get; set; }
        public virtual clsClientesBE Clientes { get; set; }
        public virtual clsGarantiaPersonalBE GarantiaPersonal { get; set; }
        public virtual clsGarantiaVehiculosBE GarantiaVehiculos { get; set; }
        public virtual clsGarantiaHipotecariaBE GarantiaHipotecaria { get; set; }
        public virtual clsGarantiaTarjetasBE GarantiaTarjetas { get; set; }
        public virtual clsGarantiaComercialesBE GarantiaComerciales { get; set; }
        public virtual clsCondicionesBE Condiciones { get; set; }
        public virtual clsTipoSolicitudesBE TipoSolicitudes { get; set; }
        public virtual ICollection<clsContratosBE> Contratos { get; set; }

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
                    case "CondicionID":
                        if ((int)CondicionID == -1)
                        {
                            return "Es necesario seleccionar una condicion.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "TipoSolicitudID":
                        if ((int)TipoSolicitudID == -1)
                        {
                            return "Es necesario seleccionar un tipo de solicitud.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "EstadoID":
                        if ((int)EstadoID == -1)
                        {
                            return "Es necesario seleccionar un estatus para esta solicitud.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Tiempo":
                        if ((int)Tiempo == 0)
                        {
                            return "Es necesario especificar el tiempo del monto solicitado";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Monto":
                        if ((decimal)Monto == 0)
                        {
                            return "Es necesario especificar el monto solicitado";
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