using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Payment.Bank.Model
{
    public class QueryBussiness
    {
        public QueryBussiness()
        {

        }

        public string EmpresaID { get; set; }

        public string Empresa { get; set; }

        public string Rnc { get; set; }

        public string Sucursal { get; set; }

        public string DireccionSucursal { get; set; }

        public string Telefonos { get; set; }

        public int ContratoID { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime Vence { get; set; }

        public string Documento { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }
        public string Apodo { get; set; }
        public string Cliente { get; set; }

        public string Direccion { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public string Celular { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public float Interes { get; set; }

        public int DiasGracia { get; set; }

        public int CondicionID { get; set; }

        public int Tiempo { get; set; }

        public float Mora { get; set; }

        public bool MoraAutomatica { get; set; }

        public int TipoContratoID { get; set; }

        public int RutaID { get; set; }
        public int EstadoID { get; set; }
        public int ClienteID { get; set; }

        public float Monto { get; set; }

        public float Cuota { get; set; }

        public float Balance { get; set; }

        public int Cantidad { get; set; }

        public float Atraso { get; set; }

        public float Pagado { get; set; }

        public string Status { get; set; }

    }
}