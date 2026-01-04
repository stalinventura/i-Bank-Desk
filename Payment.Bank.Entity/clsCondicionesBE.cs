

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Condiciones")]
	public class clsCondicionesBE: IDataErrorInfo
    {
		public clsCondicionesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CondicionID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Condicion { set; get; }

        [Column(TypeName = "int")]
        public int Dias { set; get; }

        [Column(TypeName = "int")]
        public int Meses { set; get; }

        [Column(TypeName = "bit")]
        public bool IsDefault { set; get; }

        [Column(TypeName = "bit")]
        public bool SendSMS { set; get; }

        [Column(TypeName = "int")]
        public int Tiempo { get; set; }

        [Column(TypeName = "float")]
        public float Interes { get; set; }

        [Column(TypeName = "float")]
        public float Comision { get; set; }

        [Column(TypeName = "float")]
        public float Mora { get; set; }

        [Column(TypeName = "bit")]
        public Boolean ShowComision { get; set; }

        [Column(TypeName = "bit")]
        public Boolean DataCredito { get; set; }

        [Column(TypeName = "bit")]
        public Boolean MoraAutomatica { get; set; }

        [Column(TypeName = "bit")]
        public Boolean LetterDelay { set; get; }

        [Column(TypeName = "bit")]
        public Boolean IsLegal { get; set; }

        [Column(TypeName = "float")]
        public float Legal { get; set; }

        public Boolean IsSeguro { get; set; }
        [Column(TypeName = "float")]
        public float Seguro { get; set; }

        [Column(TypeName = "int")]
        public int DiasGracia { get; set; }

        [Column(TypeName = "bit")]
        public bool IsGenerateRequest { get; set; }

        [Column(TypeName = "int")]
        public int BancoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsSolicitudesBE> Solicitudes { set; get; }

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
                    case "Comision":
                        if ((double)Comision < 0)
                        {
                            return "Es necesario la comision";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Interes":
                        if ((double)Interes <= 0)
                        {
                            return "Es necesario la tasa de interes";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Mora":
                        if ((double)Mora < 0 && MoraAutomatica == true)
                        {
                            return "Es necesario el valor de la mora";
                        }
                        else
                        {
                            goto default;
                        }
                    case "DiasGracia":
                        if ((int)DiasGracia < 0 && MoraAutomatica == true)
                        {
                            return "Es necesario los dias de gracia del contrato";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Tiempo":
                        if ((int)Tiempo < 0)
                        {
                            return "Es necesario el tiempo";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Seguro":
                        if ((double)Seguro <= 0 && IsSeguro == true)
                        {
                            return "Es necesario el porciento del seguro.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Legal":
                        if ((double)Legal <= 0 && IsLegal == true)
                        {
                            return "Es necesario el porciento de legal.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "BancoID":
                        if (IsGenerateRequest && BancoID ==-1)
                        {
                            return "Es necesario seleccionar un banco.";
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