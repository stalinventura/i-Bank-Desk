using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace Payment.Bank.Modulos
{
    class clsNewTransUnionBO
    {
        public String TransUnionGetByFecha2025(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;


                workSheet.Name = "Listado de TransUnion";


                ////Encabezado de Columnas
                int x = 1;
                workSheet.Range["A" + x, "AW" + x].Font.Bold = true;
                workSheet.Range["A" + x, "AW" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                workSheet.Cells[x, "A"] = "Tipo de Entidad";
                workSheet.Cells[x, "B"] = "Codigo del Cliente";
                workSheet.Cells[x, "C"] = "Relacion del Cliente";
                workSheet.Cells[x, "D"] = "Nombre Completo";
                workSheet.Cells[x, "E"] = "Numero de Cedula";
                workSheet.Cells[x, "F"] = "Numero de Pasaporte";
                workSheet.Cells[x, "G"] = "Razon Social";
                workSheet.Cells[x, "H"] = "Siglas";
                workSheet.Cells[x, "I"] = "Rnc";
                workSheet.Cells[x, "J"] = "Telefono Residencia";
                workSheet.Cells[x, "K"] = "Telefoo Oficina/Empresa";
                workSheet.Cells[x, "L"] = "Telefono Movil";
                workSheet.Cells[x, "M"] = "Fax";
                workSheet.Cells[x, "N"] = "Email o Correo Electronico";
                workSheet.Cells[x, "O"] = "Otro";
                workSheet.Cells[x, "P"] = "Calle/Avenida";
                workSheet.Cells[x, "Q"] = "Esquina";
                workSheet.Cells[x, "R"] = "Numero";
                workSheet.Cells[x, "S"] = "Edificio/Apartamento/Residencial";
                workSheet.Cells[x, "T"] = "Urbanizacion";
                workSheet.Cells[x, "U"] = "Sector";
                workSheet.Cells[x, "V"] = "Ciudad";
                workSheet.Cells[x, "W"] = "Provincia/Municipio";
                workSheet.Cells[x, "X"] = "Numero de Prestamo";
                workSheet.Cells[x, "Y"] = "Unidad Monetaria";
                workSheet.Cells[x, "Z"] = "Tipo de Prestamo";

                workSheet.Cells[x, "AA"] = "Forma de Pago";
                workSheet.Cells[x, "AB"] = "Tipo de Cuota";
                workSheet.Cells[x, "AC"] = "Fecha de Apertura";
                workSheet.Cells[x, "AD"] = "Fecha de Vencimiento";

                workSheet.Cells[x, "AE"] = "Tasa de Interes (Inicial)";
                workSheet.Cells[x, "AF"] = "Monto del Prestamo";
                workSheet.Cells[x, "AG"] = "Monto de la Cuota";
                workSheet.Cells[x, "AH"] = "Cantidad de Cuotas";

                workSheet.Cells[x, "AI"] = "Tasa de Interes (Vigente)";
                workSheet.Cells[x, "AJ"] = "Fecha Ultimo Pago";
                workSheet.Cells[x, "AK"] = "Monto Ultimo Pago";
                workSheet.Cells[x, "AL"] = "Balance Actual";
                workSheet.Cells[x, "AM"] = "Monto en Atraso";
                workSheet.Cells[x, "AN"] = "Cantidad de Cuotas Atrasadas";
                workSheet.Cells[x, "AO"] = "Estatus de la Cuenta";
                workSheet.Cells[x, "AP"] = "Estado de la Cuenta";
                workSheet.Cells[x, "AQ"] = "Saldo Vencido 1-30 Dias";
                workSheet.Cells[x, "AR"] = "Saldo Vencido 31-60 Dias";
                workSheet.Cells[x, "AS"] = "Saldo Vencido 61-90 Dias";
                workSheet.Cells[x, "AT"] = "Saldo Vencido 91-120 Dias";
                workSheet.Cells[x, "AU"] = "Saldo Vencido 121-150 Dias";
                workSheet.Cells[x, "AV"] = "Saldo Vencido 151-180 Dias";
                workSheet.Cells[x, "AW"] = "Saldo Vencido 181 Dias o Mas";

                workSheet.Range["A" + x, "AW" + x].Columns.AutoFit();


                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoTransUnionGetByFecha(Hasta, SucursalID);

                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;

                    //Correccion de centavos

                    //Fila["Entre0y30"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    //Fila["Entre31y60"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    //Fila["Entre61y90"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    //Fila["Entre91y120"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    //Fila["Entre121y150"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    //Fila["Entre151y180"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    //Fila["Entre181y210"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    //Fila["Entre211y240"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    //Fila["Entre241y270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    //Fila["Mas270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));

                    //Fin de la correccion


                    workSheet.Cells[x, "A"] = Fila["TipoEntidad"].ToString();

                    workSheet.Cells[x, "B"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "B"] = Fila["Codigo"].ToString().Replace("-", "");
                    workSheet.Cells[x, "C"] = Fila["RelacionCliente"].ToString();
                    workSheet.Cells[x, "D"] = Fila["Cliente"].ToString();
                    workSheet.Cells[x, "E"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "E"] = Fila["Cedula"].ToString().Replace("-", "");
                    workSheet.Cells[x, "F"] = Fila["Pasaporte"].ToString().Replace("-", "");
                    workSheet.Cells[x, "G"] = Fila["Empresa"].ToString();
                    workSheet.Cells[x, "H"] = Fila["Siglas"].ToString();
                    workSheet.Cells[x, "I"] = Fila["Rnc"].ToString();

                    workSheet.Cells[x, "J"] = string.IsNullOrEmpty(Fila["Telefono"].ToString()) ? "" : Fila["Telefono"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "K"] = string.IsNullOrEmpty(Fila["Oficina"].ToString()) ? "" : Fila["Oficina"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "L"] = string.IsNullOrEmpty(Fila["Celular"].ToString()) ? "" : Fila["Celular"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "M"] = string.IsNullOrEmpty(Fila["Fax"].ToString()) ? "" : Fila["Fax"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "N"] = Fila["Correo"].ToString();
                    workSheet.Cells[x, "O"] = string.Empty;

                    workSheet.Cells[x, "P"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "Q"] = Fila["Esquina"].ToString();
                    workSheet.Cells[x, "R"] = Fila["Casa"].ToString();
                    workSheet.Cells[x, "S"] = Fila["Edificio"].ToString();
                    workSheet.Cells[x, "T"] = Fila["Urbanizacion"].ToString();

                    workSheet.Cells[x, "U"] = Fila["Sector"].ToString();
                    workSheet.Cells[x, "V"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "W"] = Fila["Provincia"].ToString();

                    workSheet.Cells[x, "X"] = Fila["ContratoID"].ToString();

                    workSheet.Cells[x, "Y"] = Fila["Moneda"].ToString();
                    workSheet.Cells[x, "Z"] = Fila["TipoSolicitud"].ToString() == "N" ? "CONSUMO" : Fila["TipoSolicitud"].ToString() == "H" ? "HIPOTECA" : Fila["TipoSolicitud"].ToString() == "V" ? "VEHICULO" : "";

                    workSheet.Cells[x, "AA"] = Fila["Condicion"].ToString();
                    workSheet.Cells[x, "AB"] = int.Parse(Fila["TipoContratoID"].ToString()) == 1 ? "Capital": int.Parse(Fila["TipoContratoID"].ToString()) == 2? "Fija": int.Parse(Fila["TipoContratoID"].ToString()) == 3? "Vencimiento": "Intereses";

                    workSheet.Cells[x, "AC"] = Fila["Fecha"].ToString();
                    workSheet.Cells[x, "AD"] = Fila["Vence"].ToString();

                    workSheet.Cells[x, "AE"].NumberFormat = "#0.00";
                    workSheet.Cells[x, "AE"] = float.Parse(Fila["Interes"].ToString()).ToString().Replace(",", "");

                    workSheet.Cells[x, "AF"].NumberFormat = "#0";
                    workSheet.Cells[x, "AF"] = float.Parse(Fila["Monto"].ToString()).ToString().Replace(",", "");

                    workSheet.Cells[x, "AG"].NumberFormat = "#0";
                    workSheet.Cells[x, "AG"] = float.Parse(Fila["Cuota"].ToString());
                    workSheet.Cells[x, "AH"] = Fila["Tiempo"].ToString();

                    workSheet.Cells[x, "AI"].NumberFormat = "#0.00";
                    workSheet.Cells[x, "AI"] = float.Parse(Fila["Interes"].ToString()).ToString().Replace(",", "");

                    // Si fecha ultimo pago no ha pagado dejar en blanco
                    workSheet.Cells[x, "AJ"] = string.IsNullOrEmpty(Fila["FechaUltimoPago"].ToString()) ? "" : $"{((DateTime)Fila["FechaUltimoPago"]).Year}{string.Format("{0:00}", ((DateTime)Fila["FechaUltimoPago"]).Month)}{string.Format("{0:00}", ((DateTime)Fila["FechaUltimoPago"]).Day)}";

                    // Si monto ultimo pago no ha pagado dejar en blanco
                    workSheet.Cells[x, "AK"] = string.IsNullOrEmpty(Fila["MontoUltimoPago"].ToString()) ? "" : workSheet.Cells[x, "AK"].NumberFormat = "#0";
                    workSheet.Cells[x, "AK"] = string.IsNullOrEmpty(Fila["MontoUltimoPago"].ToString()) ? "" : float.Parse(Fila["MontoUltimoPago"].ToString()).ToString().Replace(",", "");



                    workSheet.Cells[x, "AL"].NumberFormat = "#0";
                    workSheet.Cells[x, "AL"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["Balance"].ToString()).ToString().Replace(",", "") : "0";
                   
                    workSheet.Cells[x, "AM"].NumberFormat = "#0";
                    workSheet.Cells[x, "AM"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["Atraso"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AN"] = Fila["EstadoID"].ToString() == "1" ? Fila["Cantidad"].ToString() : "0";
                    workSheet.Cells[x, "AO"] = Fila["EstadoID"].ToString() == "1" ? Fila["Status"].ToString() == "N" ? "Normal" : Fila["Status"].ToString() == "M" ? "Mora" : Fila["Status"].ToString() == "L" ? "Legal" : Fila["Status"].ToString() == "S" ? "Cancelado" : "Castigado" : "Cancelado";
                   
                    workSheet.Cells[x, "AP"] = Fila["EstadoID"].ToString() == "1" ? "A" : "C";

                    workSheet.Cells[x, "AQ"].NumberFormat = "#0";
                    workSheet.Cells[x, "AQ"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE0Y30"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE0Y30"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AR"].NumberFormat = "#0";
                    workSheet.Cells[x, "AR"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE31Y60"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE31Y60"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AS"].NumberFormat = "#0";
                    workSheet.Cells[x, "AS"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE61Y90"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE61Y90"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AT"].NumberFormat = "#0";
                    workSheet.Cells[x, "AT"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE91Y120"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE91Y120"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AU"].NumberFormat = "#0";
                    workSheet.Cells[x, "AU"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE121Y150"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE121Y150"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AV"].NumberFormat = "#0";
                    workSheet.Cells[x, "AV"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE151Y180"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE151Y180"].ToString()).ToString().Replace(",", "") : "0";
                    
                    workSheet.Cells[x, "AW"].NumberFormat = "#0";
                    workSheet.Cells[x, "AW"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["MAS180"].ToString()) < 0 ? "0" : float.Parse(Fila["MAS180"].ToString()).ToString().Replace(",", "") : "0";

                    workSheet.Range["A" + x, "AV" + x].Columns.AutoFit();

                }

                workSheet.Range["A" + 1, "AV" + 1].Columns.AutoFit();

                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }
        public String TransUnionGetByFecha2024(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;


                workSheet.Name = "Listado de TransUnion";
                //workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                //workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Direccion;
                //workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Telefonos;
                //workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                //workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                //workSheet.Cells[6, "A"] = "Listado TransUnion al " + Hasta.ToShortDateString();


                //workSheet.Range["A1", "A1"].Font.Bold = true;
                //workSheet.Range["A1", "A1"].Font.Size = 15;

                //workSheet.Range["A2", "A2"].Font.Bold = false;
                //workSheet.Range["A2", "A2"].Font.Size = 12;

                //workSheet.Range["A3", "A3"].Font.Bold = false;
                //workSheet.Range["A3", "A3"].Font.Size = 12;

                //workSheet.Range["A6", "A6"].Font.Bold = true;
                //workSheet.Range["A6", "A6"].Font.Size = 12;

                ////Encabezado de Columnas
                int x = 1;
                workSheet.Range["A" + x, "AU" + x].Font.Bold = true;
                workSheet.Range["A" + x, "AU" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                workSheet.Cells[x, "A"] = "Tipo de Entidad";
                workSheet.Cells[x, "B"] = "Codigo del Cliente";
                workSheet.Cells[x, "C"] = "Codigo de la Sucursal";
                workSheet.Cells[x, "D"] = "Relacion del Cliente";
                workSheet.Cells[x, "E"] = "Nombre Completo";
                workSheet.Cells[x, "F"] = "Numero de Cedula";
                workSheet.Cells[x, "G"] = "Numero de Pasaporte";
                workSheet.Cells[x, "H"] = "Razon Social";
                workSheet.Cells[x, "I"] = "Siglas";
                workSheet.Cells[x, "J"] = "Rnc";
                workSheet.Cells[x, "K"] = "Telefono Residencia";
                workSheet.Cells[x, "L"] = "Telefoo Oficina/Empresa";
                workSheet.Cells[x, "M"] = "Telefono Movil";
                workSheet.Cells[x, "N"] = "Fax";
                workSheet.Cells[x, "O"] = "Email o Correo Electronico";
                workSheet.Cells[x, "P"] = "Otro";
                workSheet.Cells[x, "Q"] = "Calle/Avenida";
                workSheet.Cells[x, "R"] = "Esquina";
                workSheet.Cells[x, "S"] = "Numero";
                workSheet.Cells[x, "T"] = "Edificio/Apartamento/Residencial";
                workSheet.Cells[x, "U"] = "Urbanizacion";
                workSheet.Cells[x, "V"] = "Sector";
                workSheet.Cells[x, "W"] = "Ciudad";
                workSheet.Cells[x, "X"] = "Provincia/Municipio";
                workSheet.Cells[x, "Y"] = "Numero de Cuenta";
                workSheet.Cells[x, "Z"] = "Unidad Monetaria";
                workSheet.Cells[x, "AA"] = "Tipo de Cuenta";
                workSheet.Cells[x, "AB"] = "Fecha de Apertura";
                workSheet.Cells[x, "AC"] = "Fecha de Vencimiento";
                workSheet.Cells[x, "AD"] = "Limite de Credito";
                workSheet.Cells[x, "AE"] = "Credito Mas Alto Utilizado";
                workSheet.Cells[x, "AF"] = "Monto de la Cuota";
                workSheet.Cells[x, "AG"] = "Cantidad de Cuotas";
                workSheet.Cells[x, "AH"] = "Fecha Ultimo Pago";
                workSheet.Cells[x, "AI"] = "Monto Ultimo Pago";
                workSheet.Cells[x, "AJ"] = "Balance Actual";
                workSheet.Cells[x, "AK"] = "Monto en Atraso";
                workSheet.Cells[x, "AL"] = "Cantidad de Cuotas Atrasadas";
                workSheet.Cells[x, "AM"] = "Estatus de la Cuenta";
                workSheet.Cells[x, "AN"] = "Estado de la Cuenta";
                workSheet.Cells[x, "AO"] = "Saldo Vencido 1-30 Dias";
                workSheet.Cells[x, "AP"] = "Saldo Vencido 31-60 Dias";
                workSheet.Cells[x, "AQ"] = "Saldo Vencido 61-90 Dias";
                workSheet.Cells[x, "AR"] = "Saldo Vencido 91-120 Dias";
                workSheet.Cells[x, "AS"] = "Saldo Vencido 121-150 Dias";
                workSheet.Cells[x, "AT"] = "Saldo Vencido 151-180 Dias";
                workSheet.Cells[x, "AU"] = "Saldo Vencido 181 Dias o Mas";

                workSheet.Range["A" + x, "AU" + x].Columns.AutoFit();


                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoTransUnionGetByFecha(Hasta, SucursalID);

                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;
                    workSheet.Cells[x, "A"] = Fila["TipoEntidad"].ToString();

                    workSheet.Cells[x, "B"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "B"] = Fila["Codigo"].ToString().Replace("-", "");
                    workSheet.Cells[x, "C"] = Fila["SucursalID"].ToString();
                    workSheet.Cells[x, "D"] = Fila["RelacionCliente"].ToString();
                    workSheet.Cells[x, "E"] = Fila["Cliente"].ToString();
                    workSheet.Cells[x, "F"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "F"] = Fila["Cedula"].ToString().Replace("-", "");
                    workSheet.Cells[x, "G"] = Fila["Pasaporte"].ToString().Replace("-", "");
                    workSheet.Cells[x, "H"] = Fila["Empresa"].ToString();
                    workSheet.Cells[x, "I"] = Fila["Siglas"].ToString();
                    workSheet.Cells[x, "J"] = Fila["Rnc"].ToString();

                    workSheet.Cells[x, "K"] = string.IsNullOrEmpty(Fila["Telefono"].ToString()) ? "" : Fila["Telefono"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "L"] = string.IsNullOrEmpty(Fila["Oficina"].ToString()) ? "" : Fila["Oficina"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "M"] = string.IsNullOrEmpty(Fila["Celular"].ToString()) ? "" : Fila["Celular"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "N"] = string.IsNullOrEmpty(Fila["Fax"].ToString()) ? "" : Fila["Fax"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "O"] = Fila["Correo"].ToString();
                    workSheet.Cells[x, "P"] = string.Empty;

                    workSheet.Cells[x, "Q"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "R"] = Fila["Esquina"].ToString();
                    workSheet.Cells[x, "S"] = Fila["Casa"].ToString();
                    workSheet.Cells[x, "T"] = Fila["Edificio"].ToString();
                    workSheet.Cells[x, "U"] = Fila["Urbanizacion"].ToString();

                    workSheet.Cells[x, "V"] = Fila["Sector"].ToString();
                    workSheet.Cells[x, "W"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "X"] = Fila["Provincia"].ToString();

                    workSheet.Cells[x, "Y"] = Fila["ContratoID"].ToString();

                    workSheet.Cells[x, "Z"] = Fila["Moneda"].ToString();
                    workSheet.Cells[x, "AA"] = Fila["TipoSolicitud"].ToString() == "N" ? "CONSUMO" : Fila["TipoSolicitud"].ToString() == "H" ? "HIPOTECA" : Fila["TipoSolicitud"].ToString() == "V" ? "VEHICULO" : "";

                    workSheet.Cells[x, "AB"] = Fila["Fecha"].ToString();
                    workSheet.Cells[x, "AC"] = Fila["Vence"].ToString();

                    workSheet.Cells[x, "AD"].NumberFormat = "#0";
                    workSheet.Cells[x, "AD"] = float.Parse(Fila["Monto"].ToString()).ToString().Replace(",", "");
                    workSheet.Cells[x, "AE"].NumberFormat = "#0";
                    workSheet.Cells[x, "AE"] = float.Parse(Fila["MaximoCredito"].ToString()).ToString().Replace(",", "");
                
                    workSheet.Cells[x, "AF"].NumberFormat = "#0";
                    workSheet.Cells[x, "AF"] = float.Parse(Fila["Cuota"].ToString());
                    workSheet.Cells[x, "AG"] = Fila["Tiempo"].ToString();

                    // Si fecha ultimo pago no ha pagado dejar en blanco
                    workSheet.Cells[x, "AH"] = string.IsNullOrEmpty(Fila["FechaUltimoPago"].ToString()) ? "" : $"{((DateTime)Fila["FechaUltimoPago"]).Year}{string.Format("{0:00}", ((DateTime)Fila["FechaUltimoPago"]).Month)}{string.Format("{0:00}", ((DateTime)Fila["FechaUltimoPago"]).Day)}";

                    // Si monto ultimo pago no ha pagado dejar en blanco
                    workSheet.Cells[x, "AI"] = string.IsNullOrEmpty(Fila["MontoUltimoPago"].ToString()) ? "" : workSheet.Cells[x, "AI"].NumberFormat = "#0";
                    workSheet.Cells[x, "AI"] = string.IsNullOrEmpty(Fila["MontoUltimoPago"].ToString()) ? "" : float.Parse(Fila["MontoUltimoPago"].ToString()).ToString().Replace(",", "");
                    
                  
                    workSheet.Cells[x, "AJ"].NumberFormat = "#0";
                    workSheet.Cells[x, "AJ"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["Balance"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AK"].NumberFormat = "#0";
                    workSheet.Cells[x, "AK"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["Atraso"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AL"] = Fila["EstadoID"].ToString() == "1" ? Fila["Cantidad"].ToString() : "0";
                    workSheet.Cells[x, "AM"] = Fila["EstadoID"].ToString() == "1" ? Fila["Status"].ToString() == "N" ? "Normal" : Fila["Status"].ToString() == "M" ? "Mora" : Fila["Status"].ToString() == "L" ? "Legal" : Fila["Status"].ToString() == "S" ? "Cancelado" : "Castigado" : "Cancelado";
                    workSheet.Cells[x, "AN"] = Fila["EstadoID"].ToString() == "1" ? "A" : "C";
  
                    workSheet.Cells[x, "AO"].NumberFormat = "#0";
                    workSheet.Cells[x, "AO"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE0Y30"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE0Y30"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AP"].NumberFormat = "#0";
                    workSheet.Cells[x, "AP"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE31Y60"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE31Y60"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AQ"].NumberFormat = "#0";
                    workSheet.Cells[x, "AQ"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE61Y90"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE61Y90"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AR"].NumberFormat = "#0";
                    workSheet.Cells[x, "AR"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE91Y120"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE91Y120"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AS"].NumberFormat = "#0";
                    workSheet.Cells[x, "AS"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE121Y150"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE121Y150"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AT"].NumberFormat = "#0";
                    workSheet.Cells[x, "AT"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE151Y180"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE151Y180"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AU"].NumberFormat = "#0";
                    workSheet.Cells[x, "AU"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["MAS180"].ToString()) < 0 ? "0" : float.Parse(Fila["MAS180"].ToString()).ToString().Replace(",", "") : "0";

                    workSheet.Range["A" + x, "AU" + x].Columns.AutoFit();
                   
                }

                workSheet.Range["A" + 1, "AU" + 1].Columns.AutoFit();

                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }
        public String TransUnionGetByFecha(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;


                workSheet.Name = "Listado de TransUnion";
                //workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                //workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Direccion;
                //workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Telefonos;
                //workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                //workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                //workSheet.Cells[6, "A"] = "Listado TransUnion al " + Hasta.ToShortDateString();
                

                //workSheet.Range["A1", "A1"].Font.Bold = true;
                //workSheet.Range["A1", "A1"].Font.Size = 15;

                //workSheet.Range["A2", "A2"].Font.Bold = false;
                //workSheet.Range["A2", "A2"].Font.Size = 12;

                //workSheet.Range["A3", "A3"].Font.Bold = false;
                //workSheet.Range["A3", "A3"].Font.Size = 12;

                //workSheet.Range["A6", "A6"].Font.Bold = true;
                //workSheet.Range["A6", "A6"].Font.Size = 12;

                ////Encabezado de Columnas
                int x = 0;
                //workSheet.Range["A" + x, "S" + x].Font.Bold = true;
                //workSheet.Range["A" + x, "S" + x].AutoFormat(
                //Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                //workSheet.Cells[x, "A"] = "CONTRATO:";
                //workSheet.Cells[x, "B"] = "FECHA:";
                //workSheet.Cells[x, "C"] = "VENCE:";
                //workSheet.Cells[x, "D"] = "CEDULA:";
                //workSheet.Cells[x, "E"] = "NOMBRES:";
                //workSheet.Cells[x, "F"] = "DIRECCION:";
                //workSheet.Cells[x, "G"] = "TELEFONO:";
                //workSheet.Cells[x, "H"] = "MONTO:";
                //workSheet.Cells[x, "I"] = "0-30 DIAS:";
                //workSheet.Cells[x, "J"] = "31-60 DIAS:";
                //workSheet.Cells[x, "K"] = "61-90 DIAS:";
                //workSheet.Cells[x, "L"] = "+90 DIAS:";
                //workSheet.Cells[x, "M"] = "BALANCE:";
                //workSheet.Cells[x, "N"] = "ATRASO:";
                //workSheet.Cells[x, "O"] = "ULT. PAGO:";
                //workSheet.Cells[x, "P"] = "FECHA ULT:";
                //workSheet.Cells[x, "Q"] = "STATUS:";
                //workSheet.Cells[x, "R"] = "TIPO DE PRESTAMO:";

                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoTransUnionGetByFecha(Hasta, SucursalID);

                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;
                    //workSheet.Range["A" + x, "A" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "A"] = "I";

                    workSheet.Cells[x, "B"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "B"] = Fila["Cedula"].ToString().Replace("-", "");

                    //workSheet.Range["C" + x, "C" + x].NumberFormat = "DD/MM/YYYY";
                    workSheet.Cells[x, "C"] = Fila["RelacionCliente"].ToString();
                    workSheet.Cells[x, "D"] = Fila["Cliente"].ToString();

                    workSheet.Cells[x, "E"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "E"] = Fila["Cedula"].ToString().Replace("-", "");
                    workSheet.Cells[x, "F"] = string.Empty;

                    workSheet.Cells[x, "G"] = "";//clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa; 
                    workSheet.Cells[x, "H"] = "";//clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                    workSheet.Cells[x, "I"] = "";//clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc.Replace("RNC:","").Replace("-","");

                    //workSheet.Range["H" + x, "O" + x].NumberFormat = "#,##0.00";

                    workSheet.Cells[x, "J"] = string.IsNullOrEmpty(Fila["Telefono"].ToString())? "" : Fila["Telefono"].ToString().Replace("(","").Replace(")","").Replace("-","");
                    workSheet.Cells[x, "K"] = string.Empty;
                    workSheet.Cells[x, "L"] = string.IsNullOrEmpty(Fila["Celular"].ToString()) ? "" : Fila["Celular"].ToString().Replace("(", "").Replace(")", "").Replace("-", "");
                    workSheet.Cells[x, "M"] = string.Empty;
                    workSheet.Cells[x, "N"] = Fila["Correo"].ToString();
                    workSheet.Cells[x, "O"] = string.Empty;

                    workSheet.Cells[x, "P"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "Q"] = string.Empty;
                    workSheet.Cells[x, "R"] = string.Empty;
                    workSheet.Cells[x, "S"] = string.Empty;
                    workSheet.Cells[x, "T"] = string.Empty;
                    workSheet.Cells[x, "U"] = string.Empty;
                    workSheet.Cells[x, "V"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "W"] = Fila["Provincia"].ToString();

                    workSheet.Cells[x, "X"] = Fila["ContratoID"].ToString();
                    workSheet.Cells[x, "Y"] = "R";
                    workSheet.Cells[x, "Z"] = Fila["TipoSolicitud"].ToString() == "N"? "CONSUMO" : Fila["TipoSolicitud"].ToString() == "H" ? "HIPOTECA" : Fila["TipoSolicitud"].ToString() == "V" ? "VEHICULO" : "";
                    workSheet.Cells[x, "AA"] = Fila["Condicion"].ToString();
                    workSheet.Cells[x, "AB"] = Fila["TipoContrato"].ToString();
                    workSheet.Cells[x, "AC"] = Fila["Fecha"].ToString();
                    workSheet.Cells[x, "AD"] = Fila["Vence"].ToString();

                    workSheet.Cells[x, "AE"].NumberFormat = "00.00";
                    workSheet.Cells[x, "AE"] = float.Parse(Fila["Interes"].ToString());
                    workSheet.Cells[x, "AF"].NumberFormat = "#0";
                    workSheet.Cells[x, "AF"] = float.Parse(Fila["Monto"].ToString()).ToString().Replace(",","");
                    workSheet.Cells[x, "AG"].NumberFormat = "#0";
                    workSheet.Cells[x, "AG"] = float.Parse(Fila["Cuota"].ToString()).ToString().Replace(",", "");
                    workSheet.Cells[x, "AH"] = Fila["Tiempo"].ToString();


                    workSheet.Cells[x, "AI"].NumberFormat = "00.00";
                    workSheet.Cells[x, "AI"] = float.Parse(Fila["Interes"].ToString());
                    // Si fecha ultimo pago no ha pagado dejar en blanco
                    workSheet.Cells[x, "AJ"] = string.IsNullOrEmpty(Fila["FechaUltimoPago"].ToString()) ? "" : $"{((DateTime)Fila["FechaUltimoPago"]).Year}{string.Format("{0:00}", ((DateTime)Fila["FechaUltimoPago"]).Month)}{string.Format("{0:00}", ((DateTime)Fila["FechaUltimoPago"]).Day)}";

                    // Si monto ultimo pago no ha pagado dejar en blanco
                    workSheet.Cells[x, "AK"] = string.IsNullOrEmpty(Fila["MontoUltimoPago"].ToString()) ? "" : workSheet.Cells[x, "AK"].NumberFormat = "#0";
                    workSheet.Cells[x, "AK"] = string.IsNullOrEmpty(Fila["MontoUltimoPago"].ToString()) ? "" : float.Parse(Fila["MontoUltimoPago"].ToString()).ToString().Replace(",", "");
                    workSheet.Cells[x, "AL"].NumberFormat = "#0";
                    workSheet.Cells[x, "AL"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["Balance"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AM"].NumberFormat = "#0";
                    workSheet.Cells[x, "AM"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["Atraso"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AN"] = Fila["EstadoID"].ToString() == "1" ? Fila["Cantidad"].ToString(): "0";
                    workSheet.Cells[x, "AO"] = Fila["EstadoID"].ToString() == "1" ? Fila["Status"].ToString() == "N" ? "Normal" : Fila["Status"].ToString() == "M" ? "Mora" : Fila["Status"].ToString() == "L" ? "Legal" : Fila["Status"].ToString() == "S" ? "Cancelado" : "Castigado" : "Cancelado";
                    workSheet.Cells[x, "AP"] = Fila["EstadoID"].ToString() == "1" ? "A" : "C";

                    workSheet.Cells[x, "AQ"].NumberFormat = "#0";
                    workSheet.Cells[x, "AQ"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE0Y30"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE0Y30"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AR"].NumberFormat = "#0";
                    workSheet.Cells[x, "AR"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE31Y60"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE31Y60"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AS"].NumberFormat = "#0";
                    workSheet.Cells[x, "AS"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE61Y90"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE61Y90"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AT"].NumberFormat = "#0";
                    workSheet.Cells[x, "AT"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE91Y120"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE91Y120"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AU"].NumberFormat = "#0";
                    workSheet.Cells[x, "AU"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE121Y150"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE121Y150"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AV"].NumberFormat = "#0";
                    workSheet.Cells[x, "AV"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["ENTRE151Y180"].ToString()) < 0 ? "0" : float.Parse(Fila["ENTRE151Y180"].ToString()).ToString().Replace(",", "") : "0";
                    workSheet.Cells[x, "AW"].NumberFormat = "#0";
                    workSheet.Cells[x, "AW"] = Fila["EstadoID"].ToString() == "1" ? float.Parse(Fila["MAS180"].ToString()) < 0 ? "0" : float.Parse(Fila["MAS180"].ToString()).ToString().Replace(",", "") : "0";

                }



                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;
            
            }
        }   

    }
}

