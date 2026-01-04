using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Nominas")]
	public class clsNominasBE: IDataErrorInfo
    {
		public clsNominasBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int NominaID { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Codigo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Nomina { set; get; }

        [Column(TypeName = "bit")]
        public bool IsMensual { get; set; }

        [Column(TypeName = "bit")]
        public bool IsRegalia { get; set; }

        [Column(TypeName = "int")]
        public int BancoID { get; set; }

        [Column(TypeName = "float")]
        public float SubTotal { get; set; }

        [Column(TypeName = "float")]
        public float Descuento { get; set; }

        [Column(TypeName = "float")]
        public float Total { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Informacion { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Motivo { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { set; get; }

        [Column(TypeName = "bit")]
        public bool Contabilizado { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual ICollection<clsDetalleNominasBE> DetalleNominas { get; set; }
        public virtual clsBancosBE Bancos { get; set; }

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
                    case "Codigo":
                        if (string.IsNullOrEmpty(Codigo))
                        {
                            return "Es necesario el codigo de la nomina.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Nomina":
                        if (string.IsNullOrEmpty(Nomina))
                        {
                            return "Es necesario la descipcion de la nomina.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "BancoID":
                        if ((int)BancoID == -1)
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