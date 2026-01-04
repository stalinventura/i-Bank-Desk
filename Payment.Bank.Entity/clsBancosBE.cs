using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Bancos")]
	public class clsBancosBE:IDataErrorInfo
	{
		public clsBancosBE()
		{
		}

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BancoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int SucursalID { set; get; }

        [Column(TypeName = "int")]
        public int AuxiliarID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Banco { get; set; }
             
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsAuxiliaresBE Auxiliares  { get; set; }
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual ICollection<clsSolicitudChequesBE> SolicitudCheques { get; set; }
        public virtual ICollection<clsNominasBE> Nominas { get; set; }

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
                    case "Banco":
                        if (string.IsNullOrEmpty(Banco))
                        {
                            return "Es necesario nombre del banco";
                        }
                        else
                        {
                            goto default;
                        }

                    case "AuxiliarID":
                        if (AuxiliarID == -1)
                        {
                            return "Es necesario seleccionar un auxiliar";
                        }
                        else
                        {
                            goto default;
                        }

                    case "SucursalID":
                        if (SucursalID == -1)
                        {
                            return "Es necesario seleccionar una sucursal";
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