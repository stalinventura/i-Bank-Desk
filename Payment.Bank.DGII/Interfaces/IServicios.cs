using Payment.Bank.DGII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.DGII.Interfaces
{
    interface IServicios
    {
        OperationResult ConsultarRnc(string rnc);
    }
}
