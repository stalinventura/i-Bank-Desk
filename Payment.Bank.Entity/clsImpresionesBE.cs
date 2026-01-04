using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Impresiones")]
    public class clsImpresionesBE : IDataErrorInfo
    {
        public clsImpresionesBE()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ImpresionID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        public int SucursalID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string DispositivoID { set; get; }

        public int TipoImpresionID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Papel { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Local { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Red { get; set; }

        [Column(TypeName = "int")]
        public int Copias { get; set; }

        [Column(TypeName = "float")]
        public float Ancho { get; set; }

        [Column(TypeName = "float")]
        public float Alto { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsConfiguracionesBE Configuraciones { get; set; }
        public virtual clsTipoImpresionesBE TipoImpresiones { get; set; }
        public virtual clsDispositivosBE Dispositivos { get; set; }

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
                    case "TipoImpresionID":
                        if ((int)TipoImpresionID == -1)
                        {
                            return "Es necesario seleccionar un tipo de Impresion.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "DispositivoID":
                        if (string.IsNullOrEmpty(DispositivoID))
                        {
                            return "Es necesario seleccionar un dispositivo";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Local":
                        if (string.IsNullOrEmpty(Local))
                        {
                            return "Es necesario el nombre de una impresora local.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Red":
                        if (string.IsNullOrEmpty(Red))
                        {
                            return "Es necesario el nombre de una impresora de Red.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Papel":
                        if (string.IsNullOrEmpty(Papel))
                        {
                            return "Es necesario el nombre del papel";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Copias":
                        if (Copias == 0)
                        {
                            return "Es necesario la cantidad de copias";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Ancho":
                        if (Ancho == 0)
                        {
                            return "Es necesario el ancho del papel.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Alto":
                        if (Alto == 0)
                        {
                            return "Es necesario el alto del papel.";
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