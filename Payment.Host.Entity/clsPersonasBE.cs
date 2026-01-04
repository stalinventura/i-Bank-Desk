using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Host.Entity
{

    [Table("Personas")]
    public class clsPersonasBE:IDataErrorInfo
    {
        public clsPersonasBE()
        {
        }

        [Key]
        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Documento { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaNacimiento { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Nombres { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Apellidos { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Apodo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string Fotografia { set; get; }

        [Column(TypeName = "int")]
        public int CiudadID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Direccion { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Correo { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Telefono { set; get; }

        [Column(TypeName = "int")]
        public int OperadorID { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Celular { set; get; }

        [Column(TypeName = "int")]
        public int SexoID { set; get; }

        [Column(TypeName = "int")]
        public int EstadoCivilID { set; get; }

        [Column(TypeName = "bit")]
        public bool IsAsync { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { set; get; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { set; get; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { set; get; }

        //Relaciones
        public virtual clsSexosBE Sexos { get; set; }
        public virtual clsCiudadesBE Ciudades { get; set; }
        public virtual clsEstadosCivilesBE EstadosCiviles { get; set; }
        public virtual clsOperadoresBE Operadores { get; set; }
        public virtual clsFotografiasBE Fotografias { get; set; }
        public virtual clsUsuariosBE Usuarios { get; set; }
        public virtual ICollection<clsReferenciasBE> Referencias { get; set; }
        public virtual ICollection<clsHistorialBE> Historial { get; set; }

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
                    #region Personas

                    case "Documento":
                        if (string.IsNullOrEmpty(Documento))
                        {
                            return "Es necesario el numero de documento.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Nombres":
                        if (string.IsNullOrEmpty(Nombres))
                        {
                            return "Es necesario el nombre";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Apellidos":
                        if (string.IsNullOrEmpty(Apellidos))
                        {
                            return "Es necesario el apellido";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Apodo":
                        if (string.IsNullOrEmpty(Apodo))
                        {
                            return "Es necesario el apodo";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Fotografia":
                        if (Fotografia == null)
                        {
                            return "Es necesario la fotografía";
                        }
                        else
                        {
                            goto default;
                        }

                    case "CiudadID":
                        if ((int)CiudadID == -1)
                        {
                            return "Es necesario seleccionar una ciudad.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "SexoID":
                        if ((int)SexoID == -1)
                        {
                            return "Es necesario seleccionar un sexo.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Correo":
                        if (String.IsNullOrEmpty(Correo))
                        {
                            return "Es necesario el Correo electrónico";
                        }
                        else
                        {
                            string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
                            Regex re = new Regex(emailExpression);
                            if (!re.IsMatch(Correo))
                            {
                                return "Es invalido el formato de correo ";
                            }
                            else
                            {
                                goto default;
                            }
                        }
                    case "EstadoCivilID":
                        if ((int)EstadoCivilID == -1)
                        {
                            return "Es necesario seleccionar el estado civil";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Direccion":
                        if (string.IsNullOrEmpty(Direccion))
                        {
                            return "Es necesario la dirección.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "OperadorID":
                        if ((int)OperadorID == -1)
                        {
                            return "Es necesario seleccionar una operador telefonico.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Telefono":
                        if (string.IsNullOrEmpty(Telefono))
                        {
                            return "Es necesario el numero de Telefono.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "Celular":
                        if (string.IsNullOrEmpty(Celular))
                        {
                            return "Es necesario el numero de celular.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "FechaNacimiento":
                        if (FechaNacimiento >= DateTime.Now.AddYears(-14))
                        {
                            return "Es necesario una fecha de nacimiento valida.";
                        }
                        else
                        {
                            goto default;
                        }

                    #endregion

                    default:
                        return null;
                }
            }
        }
        #endregion  
    }
}