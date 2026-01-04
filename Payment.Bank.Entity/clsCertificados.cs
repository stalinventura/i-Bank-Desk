using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Certificados")]
	public class clsCertificadosBE:IDataErrorInfo
	{
		public clsCertificadosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int CertificadoID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Vence { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "int")]
        public int TipoCertificadoID { set; get; }

        [Column(TypeName = "int")]
        public int MonedaID { set; get; }

        [Column(TypeName = "int")]
        public int Tiempo { set; get; }

        [Column(TypeName = "float")]
        public float Interes { set; get; }

        [Column(TypeName = "float")]
        public float Monto { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Motivo { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsMonedasBE Monedas { get; set; }
        public virtual clsTipoCertificadosBE TipoCertificados { get; set; }
        public virtual clsPersonasBE Personas { get; set; }
        public virtual ICollection<clsDetalleCertificadosBE> DetalleCertificados { get; set; }
        public virtual clsSucursalesBE Sucursales { get; set; }

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
                    case "TipoCertificadoID":
                        if ((int)TipoCertificadoID == -1)
                        {
                            return "Es necesario seleccionar un tipo de certificado.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "MonedaID":
                        if ((int)MonedaID == -1)
                        {
                            return "Es necesario seleccionar una moneda.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Tiempo":
                        if ((int)Tiempo <= 0)
                        {
                            return "Es necesario especificar el tiempo en meses del certificado";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Interes":
                        if ((double)Interes <= 0)
                        {
                            return "Es necesario la tasa de interes del certificado";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Monto":
                        if ((decimal)Monto == 0)
                        {
                            return "Es necesario especificar el monto del certificado";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Motivo":
                        if (string.IsNullOrEmpty(Motivo))
                        {
                            return "Es necesario el motivo de la cancelación del certificado";
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