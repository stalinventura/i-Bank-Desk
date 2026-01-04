using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Roles")]
    public class clsRolesBE: IDataErrorInfo
    {
		public clsRolesBE()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "int")]
        public int RolID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Rol { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Descripcion { get; set; }

        [Column(TypeName = "bit")]
        public bool IsAdmin { get; set; }

        [Column(TypeName = "bit")]
        public bool CanQuery { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllApplication { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllContract { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllReceipt { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllBank { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllCheck { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllApplicationCheck { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllAccountting { get; set; }

        [Column(TypeName = "bit")]
        public bool ViewAllExpenditure { get; set; }


        [Column(TypeName = "int")]
        public int GraficoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones

        public virtual ICollection<clsUsuariosBE> Usuarios { get; set; }
        public virtual clsGraficosBE Graficos { get; set; }
        public virtual ICollection<clsPermisosRolesBE> PermisosRoles { get; set; }

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
                    case "RolID":
                        if ((int)RolID == -1)
                        {
                            return "Es necesario seleccionar un rol";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Rol":
                        if (String.IsNullOrEmpty(Rol))
                        {
                            return "Es necesario el rol";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Descripcion":
                        if (String.IsNullOrEmpty(Descripcion))
                        {
                            return "Es necesario la descripcion del rol";
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