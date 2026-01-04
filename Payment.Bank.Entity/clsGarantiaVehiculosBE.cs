using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("GarantiaVehiculos")]
    public class clsGarantiaVehiculosBE : IDataErrorInfo
    {
        public clsGarantiaVehiculosBE()
        {
        }


        [Key]
        [ForeignKey("Solicitudes")]
        [Column(TypeName = "int")]
        public int SolicitudID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "int")]
        public int TipoVehiculoID { set; get; }

        [Column(TypeName = "int")]
        public int ModeloID { set; get; }

        [Column(TypeName = "int")]
        public int Ano { set; get; }

        [Column(TypeName = "int")]
        public int ColorID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        public string Chasis { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Placa { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        public string Registro { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaExpedicion { set; get; }

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
        public virtual clsTipoVehiculosBE TipoVehiculos { get; set; }
        public virtual clsModelosBE Modelos { get; set; }
        public virtual clsColoresBE Colores { get; set; }


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
                    case "ColorID":
                        if ((int)ColorID == -1 )
                        {
                            return "Es necesario seleccionar un color.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ModeloID":
                        if ((int)ModeloID == -1)
                        {
                            return "Es necesario seleccionar un modelo.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "TipoVehiculoID":
                        if ((int)TipoVehiculoID == -1)
                        {
                            return "Es necesario seleccionar un tipo de vehiculo.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Placa":
                        {
                            if (string.IsNullOrEmpty(Placa))
                            {
                                return "Es necesario el numero de placa del vehiculo";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "Chasis":
                        {
                            if (string.IsNullOrEmpty(Chasis))
                            {
                                return "Es necesario el numero de chassis del vehiculo";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "Ano":
                        {
                            if (Convert.ToInt32(Ano) == 0)
                            {
                                return "Es necesario el año del vehiculo";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "Registro":
                        {
                            if (string.IsNullOrEmpty(Registro))
                            {
                                return "Es necesario el numero de registro del vehiculo";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                   
                    default:
                        return null;
                }
            }
        }
        #endregion  

    }
}