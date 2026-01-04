using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Payment.Bank.Entity
{
    [Table("Contratos")]
    public class clsContratosBE: IDataErrorInfo
	{        
		public clsContratosBE()
		{
            SolicitudCheques = new HashSet<clsSolicitudChequesBE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContratoID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Vence { get; set; }

        [Column(TypeName = "int")]
        public int SolicitudID { get; set; }

        [Column(TypeName = "int")]
        public int TipoContratoID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        public string Acto { get; set; }

        [Column(TypeName = "float")]
        public float Comision { get; set; }

        [Column(TypeName = "float")]
        public float Interes { get; set; }

        [Column(TypeName = "float")]
        public float Mora { get; set; }

        [Column(TypeName = "float")]
        public float Cuota { get; set; }

        [Column(TypeName = "bit")]
        public Boolean ShowComision { get; set; }

        [Column(TypeName = "bit")]
        public Boolean DataCredito { get; set; }

        [Column(TypeName = "bit")]
        public Boolean MoraAutomatica { get; set; }

        [Column(TypeName = "bit")]
        public Boolean LetterDelay { get; set; }

        [Column(TypeName = "int")]
        public int DiasGracia { get; set; }

        [Column(TypeName = "int")]
        public int NotarioPublicoID { get; set; }

        [Column(TypeName = "int")]
        public int OficialID { get; set; }

        [Column(TypeName = "int")]
        public int ZonaID { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(250)]
        public string Motivo { get; set; }

        [Column(TypeName = "int")]
        public int EstadoID { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsLegal { get; set; }

        [Column(TypeName = "float")]
        public float Legal { get; set; }

        [Column(TypeName = "bit")]
        public Boolean IsSeguro { get; set; }

        [Column(TypeName = "float")]
        public float Seguro { get; set; }

        [Column(TypeName = "int")]
        public int? ObjetivoID { get; set; }

        [Column(TypeName = "bit")]
        public Boolean InteresDiario { get; set; }

        [Column(TypeName = "bit")]
        public Boolean MoraDiaria { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string Usuario { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(13)]
        public string ModificadoPor { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime FechaModificacion { get; set; }

        //Relaciones
        public virtual clsSolicitudesBE Solicitudes { get; set; }
        public virtual clsEstadoContratosBE Estados { get; set; }
        public virtual clsTipoContratosBE TipoContratos { get; set; }
        public virtual clsObjetivosBE Objetivos { get; set; }
        public virtual ICollection<clsCuotasBE> Cuotas { get; set; }
        public virtual clsOficialesBE Oficiales { get; set; }
        public virtual clsNotariosPublicoBE NotariosPublico { get; set; }
        public virtual ICollection<clsRecibosBE> Recibos { get; set; }
        public virtual ICollection<clsLlamadasBE> Llamadas { get; set; }
        public virtual clsZonasBE Zonas { get; set; }
        public virtual ICollection<clsSolicitudChequesBE> SolicitudCheques { get; set; }
        public virtual ICollection<clsSmsBE> Sms { get; set; }

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
                    case "TipoContratoID":
                        if ((int)TipoContratoID == -1)
                        {
                            return "Es necesario seleccionar un tipo de contrato.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Comision":
                        if ((double)Comision  < 0)
                        {
                            return "Es necesario la comision del contrato";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Interes":
                        if ((double)Interes <= 0)
                        {
                            return "Es necesario la tasa de interes del contrato";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Mora":
                        if ((double)Mora < 0)
                        {
                            return "Es necesario el valor de la mora";
                        }
                        else
                        {
                            goto default;
                        }
                    case "DiasGracia":
                        if ((int)DiasGracia < 0)
                        {
                            return "Es necesario los dias de gracia del contrato";
                        }
                        else
                        {
                            goto default;
                        }
                    case "NotarioPublicoID":
                        if (NotarioPublicoID == -1)
                        {
                            return "Es necesario seleccionar el notario publico";
                        }
                        else
                        {
                            goto default;
                        }
                    case "OficialID":
                        if ((int)OficialID == -1)
                        {
                            return "Es necesario seleccionar el oficial del contrato";
                        }
                        else
                        {
                            goto default;
                        }
                    case "ZonaID":
                        if ((int)ZonaID == -1)
                        {
                            return "Es necesario seleccionar una zona.";
                        }
                        else
                        {
                            goto default;
                        }

                    case "ObjetivoID":
                        if ((int)ObjetivoID == -1)
                        {
                            return "Es necesario seleccionar un objetivo.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Motivo":
                        if (string.IsNullOrEmpty(Motivo))
                        {
                            return "Es necesario el motivo de la suspension del contrato";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Legal":
                        if ((double)Legal <= 0)
                        {
                            return "Es necesario el porciento de gastos de cierre.";
                        }
                        else
                        {
                            goto default;
                        }
                    case "Seguro":
                        if ((double)Seguro <= 0)
                        {
                            return "Es necesario el porciento del seguro del contrato.";
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