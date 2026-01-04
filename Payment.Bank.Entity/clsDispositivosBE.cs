using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Dispositivos")]
	public class clsDispositivosBE: IDataErrorInfo
    {
		public clsDispositivosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(50)]
        public string DispositivoID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [MaxLength(100)]
        public string EmpresaID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(150)]
        public string Dispositivo { get; set; }

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
        public virtual clsEmpresasBE Empresas { get; set; }
        public virtual ICollection<clsImpresionesBE> Impresiones { get; set; }


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
                    case "DispositivoID":
                        if (string.IsNullOrEmpty(DispositivoID))
                        {
                            return "Seleccione un dispositivo";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Dispositivo":
                        if (string.IsNullOrEmpty(Dispositivo))
                        {
                            return "Es necesario nombre del dispositivo";
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