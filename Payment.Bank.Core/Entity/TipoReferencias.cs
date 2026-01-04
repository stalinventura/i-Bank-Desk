using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Bank.Core.Entity
{

    public class TipoReferencias
    {
        public TipoReferencias()
        {
        }
        
        public int TipoReferenciaID { get; set; }
        public DateTime Fecha { get; set; }        
        public string TipoReferencia { get; set; }
        public Boolean IsDefault { set; get; }
        public string Usuario { get; set; }
        public string ModificadoPor { get; set; }        
        public DateTime FechaModificacion { get; set; }
        
    }
}