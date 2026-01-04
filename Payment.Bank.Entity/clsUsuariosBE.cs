using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Entity
{
    [Table("Usuarios")]
	public class clsUsuariosBE: IDataErrorInfo
    {
		public clsUsuariosBE()
		{
		}

        [Key]
        [ForeignKey("Personas")]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName ="DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Contraseña { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Pin { get; set; }

        [Column(TypeName = "int")]
        public int RolID { get; set; }

        [Column(TypeName = "int")]
        public int SucursalID { get; set; }
        [Column(TypeName = "int")]
        public int JornadaID { get; set; }

        [Column(TypeName = "bit")]
        public bool EstadoID { get; set; }

        [Column(TypeName = "int")]
        public int PreguntaID { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Respuesta { get; set; }

        [Column(TypeName = "int")]
        public int LenguajeID { get; set; }

        public string TokenID { get; set; }
        public DateTime FechaTokenID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }


        //Relaciones
       
        public virtual clsRolesBE Roles { get; set; }
        public virtual clsPreguntasBE Preguntas { get; set; }
        public virtual clsLenguajesBE Lenguajes { get; set; }
        public virtual clsPersonasBE Personas { get; set; }
        public virtual clsSucursalesBE Sucursales { get; set; }
        public virtual clsJornadasBE Jornadas { get; set; }
        public virtual ICollection<clsTurnosBE> Turnos { get; set; }
        public virtual ICollection<clsUsuariosRutasBE> UsuariosRutas { get; set; }

        #region IDataErrorInfo Members
        public string Error
        {
            get
            {
                return "error";
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                if (columnName == "RolID")
                {
                    if ((int)RolID == -1)
                        result = "Seleccione un rol o funcion";
                }

                if (columnName == "LenguajeID")
                {
                    if ((int)LenguajeID == -1)
                        result = "Seleccione un Lenguaje";
                }

                if (columnName == "PreguntaID")
                {
                    if ((int)PreguntaID == -1)
                        result = "Seleccione una pregunta de seguridad";
                }


                if (columnName == "JornadaID")
                {
                    if ((int)JornadaID == -1)
                        result = "Seleccione una jornada laboral";
                }

                if (columnName == "Respuesta")
                {
                    if (string.IsNullOrEmpty(Respuesta))
                        result = "Responda a su pregunta de seguridad";
                }

                if (columnName == "Contraseña")
                {
                    if (string.IsNullOrEmpty(Contraseña))
                        result = "Es necesario establecer la Contraseña";
                }

                return result;
            }
        }

        #endregion
    }
}