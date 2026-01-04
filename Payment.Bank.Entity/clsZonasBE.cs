using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Zonas")]
    public class clsZonasBE: IDataErrorInfo
    {
		public clsZonasBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int ZonaID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Zona { get; set; }

        [Column(TypeName = "int")]
        public int RutaID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsContratosBE> Contratos { get; set; }
        public virtual clsRutasBE Rutas { get; set; }

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
                    case "Zona":
                        if (string.IsNullOrEmpty(Zona))
                        {
                            return "Es necesario la Zona.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ZonaID":
                        if ((int)ZonaID == -1)
                        {
                            return "Es necesario seleccionar una Zona.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "RutaID":
                        if ((int)RutaID == -1)
                        {
                            return "Es necesario seleccionar una ruta.";
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