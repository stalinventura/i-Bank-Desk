using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Alquileres")]
	public class clsAlquileresBE : IDataErrorInfo
    {
		public clsAlquileresBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int AlquilerID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int TipoAlquilerID { get; set; }

        [Column(TypeName = "int")]
        public int ApartamentoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { get; set; }

        [Column(TypeName = "int")]
        public int Tiempo { get; set; }

        [Column(TypeName = "int")]
        public int NotarioPublicoID { get; set; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "bit")]
        public Boolean EstadoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones

        public virtual clsTipoAlquileresBE TipoAlquileres { get; set; }
        public virtual clsNotariosPublicoBE NotariosPublico { get; set; }
        public virtual clsApartamentosBE Apartamentos { get; set; }
        public virtual clsPersonasBE Personas { get; set; }
        public virtual ICollection<clsRentasBE> Rentas { get; set; }
        public virtual ICollection<clsPagoRentasBE> PagoRentas { get; set; }
        public virtual clsGarantiaAlquileresBE GarantiaAlquileres { get; set; }

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
                    case "Monto":
                        if (string.IsNullOrEmpty(Monto.ToString()))
                        {
                            return "Es necesario el monto del alquiler.";
                        }
                        else
                        {
                            if (Monto <= 0)
                            {
                                return "Es necesario el monto del alquiler.";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "Tiempo":
                        if (string.IsNullOrEmpty(Tiempo.ToString()))
                        {
                            return "Es necesario el tiempo de deposito entregado.";
                        }
                        else
                        {
                            if (Tiempo <= 0)
                            {
                                return "Es necesario el tiempo de deposito entregado.";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "TipoAlquilerID":
                        if ((int)TipoAlquilerID == -1)
                        {
                            return "Es necesario seleccionar un tipo de alquiler.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "NotarioPublicoID":
                        if ((int)NotarioPublicoID == -1)
                        {
                            return "Es necesario seleccionar un notario publico.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ApartamentoID":
                        if ((int)ApartamentoID == -1)
                        {
                            return "Es necesario seleccionar un apartamento";
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