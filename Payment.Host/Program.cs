
using Payment.Host.DAL;
using Payment.Host.Entity;
using System;
using System.Linq;

namespace Payment.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(DateTime.Today);
                Console.WriteLine(Common.FingerPrint.GetKey());

                Console.ReadKey();
                Context db = new Context();
                string GetKey = Common.FingerPrint.GetKey().ToString();
                clsEmpresasBE BE = db.clsEmpresasBE.Where(x => x.EmpresaID == GetKey).FirstOrDefault();

                if (BE == null)
                {
                    Console.WriteLine("**********************************Welcome to i-Bank Registration Host**********************************");
                    Console.WriteLine(Common.FingerPrint.GetKey());
                    Console.WriteLine("");
                    Console.WriteLine("Cual es el Nombre de la Empresa?");
                    string Empresa = Console.ReadLine();
                    Console.WriteLine("Cual es la dirección de la Empresa?");
                    string Direccion = Console.ReadLine();
                    Console.WriteLine("Cual es el Telefono de la Empresa?");
                    string Telefonos = Console.ReadLine();
                    Console.WriteLine("Cual es el RNC de la Empresa?");
                    string RNC = Console.ReadLine();

                    Console.WriteLine("Cual es el numero de licencias a utilizar de la Empresa?");
                    string Device = Common.FingerPrint.GetHash(Console.ReadLine());
                    string Payment = Common.FingerPrint.GetHash(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString()).ToString();

                    db.clsEmpresasBE.Add(new clsEmpresasBE { EmpresaID = Common.FingerPrint.GetKey().Replace(" ", ""), Fecha = DateTime.Now, Empresa = Empresa, Direccion = Direccion, Telefonos = Telefonos, Key = Common.FingerPrint.GetHash("false"), Rnc = RNC, Payment = Payment, Device = Device, Usuario = Environment.MachineName, ModificadoPor = Environment.MachineName, FechaModificacion = DateTime.Now });
                    db.SaveChanges();
                    Console.WriteLine("********************************************************************");

                    //Model.Host cx = new Model.Host();
                    //var row = cx.Empresas.Where(x => x.EmpresaID == GetKey).FirstOrDefault();
                    //if(row == null)
                    //{
                    //    cx.Empresas.Add(new Empresas { EmpresaID = Common.FingerPrint.GetKey().Replace(" ", ""), Fecha = DateTime.Now, Empresa = Empresa, Direccion = Direccion, Telefonos = Telefonos, Key = Common.FingerPrint.GetHash("false"), Rnc = RNC, Payment = Payment, Device = Device, Usuario = Environment.MachineName, ModificadoPor = Environment.MachineName, FechaModificacion = DateTime.Now });
                    //    cx.SaveChanges();
                    //}
                }
                else
                {
                    //Model.Host cx = new Model.Host();
                    //var row = cx.Empresas.Where(x => x.EmpresaID == GetKey).FirstOrDefault();
                    //row.Empresa = BE.Empresa;
                    //row.Direccion = BE.Direccion;
                    //row.Telefonos = BE.Telefonos;
                    //row.Rnc = BE.Rnc;
                    //row.Siglas = BE.Siglas;
                    //row.CSR = BE.CSR;
                    //row.Logo = BE.Logo;                
                    //row.Key = BE.Key;
                    //row.Payment = BE.Payment;
                    //row.FechaModificacion = DateTime.Now;
                    //row.ModificadoPor = Environment.MachineName;
                    //cx.Entry(row).State = EntityState.Modified;
                    //cx.SaveChanges();
                }

            }
            catch { }

            string Pay = Common.FingerPrint.GetHash(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString()).ToString();
        }
    }
}
