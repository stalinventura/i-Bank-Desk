using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Turnos")]
	public class clsTurnosBE : IDataErrorInfo
	{
		public clsTurnosBE()
		{
		}
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int TurnoID { set; get; }

        [Column(TypeName= "DateTime")]
		public DateTime Fecha { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName ="int")]
        public int CajaID { set; get; }

        [Column(TypeName = "float")]
        public float Monto { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Comentario { set; get; }

        [Column(TypeName = "bit")]
        public bool EstadoID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsUsuariosBE Usuarios { get; set; }
        public virtual clsCajasBE Cajas { get; set; }
        public virtual ICollection<clsArqueosBE> Arqueos { get; set; }

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
                    case "TurnoID":
                        if ((int)TurnoID == -1)
                        {
                            return "Es necesario seleccionar turno";
                        }
                        else
                        {
                            goto default;
                        }
                    case "CajaID":
                        if ((int)CajaID == -1)
                        {
                            return "Es necesario seleccionar una caja.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Documento":
                        if (string.IsNullOrEmpty(Documento) || Documento == "-1")
                        {
                            return "Es necesario seleccionar un cajero";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Monto":
                        if (Monto == 0)
                        {
                            return "Es necesario especificar un monto inicial";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Comentario":
                        if (string.IsNullOrEmpty(Comentario))
                        {
                            return "Es necesario especificar un comentario";
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