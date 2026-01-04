using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("GarantiaTarjetas")]
	public class clsGarantiaTarjetasBE:IDataErrorInfo
	{
		public clsGarantiaTarjetasBE()
		{
		}


        [Key]
        [ForeignKey("Solicitudes")]
        [Column(TypeName = "int")]
        public int SolicitudID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Holder { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Numero { set; get; }

        [Column(TypeName = "int")]
        public int Mes { set; get; }

        [Column(TypeName = "int")]
        public int Ano { set; get; }

        [Column(TypeName = "int")]
        public int Cvv { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsSolicitudesBE Solicitudes { get; set; }

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
                    case "Holder":
                        if (string.IsNullOrEmpty(Holder))
                        {
                            return "Es necesario el nombre del tarjeta habiente.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Numero":
                        if (string.IsNullOrEmpty(Numero))
                        {
                            return "Es necesario un numero de tarjetas.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Mes":
                        if ((int)Mes <= 0)
                        {
                            return "Es necesario el mes de vencimiento de la tarjeta";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Ano":
                        if ((int)Ano <= 0)
                        {
                            return "Es necesario el año de vencimiento de la tarjeta";
                        }
                        else
                        {
                            goto default;
                        }
                    case "CVV":
                        if ((int)Cvv <= 0)
                        {
                            return "Es necesario el codigo cvv de la tarjeta";
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