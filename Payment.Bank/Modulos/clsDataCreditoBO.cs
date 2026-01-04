using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace Payment.Bank.Modulos
{
    class clsDataCreditoBO
    {
        public String DataCreditoGetByFecha(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                //DataSet dsEmpresas = default(DataSet);
                //dsEmpresas = Variables.saoWS.EmpresasGet();


                //foreach (DataRow Fila in dsEmpresas.Tables["Empresas"].Rows)
                //{
                workSheet.Name = "Listado de Equifax";
                workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                workSheet.Cells[6, "A"] = "Listado Equifax al " + Hasta.ToShortDateString();
                //}

                workSheet.Range["A1", "A1"].Font.Bold = true;
                workSheet.Range["A1", "A1"].Font.Size = 15;

                workSheet.Range["A2", "A2"].Font.Bold = false;
                workSheet.Range["A2", "A2"].Font.Size = 12;

                workSheet.Range["A3", "A3"].Font.Bold = false;
                workSheet.Range["A3", "A3"].Font.Size = 12;

                workSheet.Range["A6", "A6"].Font.Bold = true;
                workSheet.Range["A6", "A6"].Font.Size = 12;

                //Encabezado de Columnas
                int x = 8;
                workSheet.Range["A" + x, "S" + x].Font.Bold = true;
                workSheet.Range["A" + x, "S" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                //workSheet.Cells[x, "A"] = "CUENTA:";
                workSheet.Cells[x, "A"] = "CONTRATO:";
                workSheet.Cells[x, "B"] = "FECHA:";
                workSheet.Cells[x, "C"] = "VENCE:";
                workSheet.Cells[x, "D"] = "CEDULA:";
                workSheet.Cells[x, "E"] = "NOMBRES:";
                workSheet.Cells[x, "F"] = "DIRECCION:";
                workSheet.Cells[x, "G"] = "TELEFONO:";
                workSheet.Cells[x, "H"] = "MONTO:";
                workSheet.Cells[x, "I"] = "PAGADO:";
                workSheet.Cells[x, "J"] = "BALANCE:";
                workSheet.Cells[x, "K"] = "CUOTAS:";
                workSheet.Cells[x, "L"] = "CANT.:";
                workSheet.Cells[x, "M"] = "ATRASO:";
                workSheet.Cells[x, "N"] = "ULT. PAGO:";
                workSheet.Cells[x, "O"] = "FECHA ULT:";
                workSheet.Cells[x, "P"] = "STATUS:";
                workSheet.Cells[x, "Q"] = "RELACION TIPO:";
                workSheet.Cells[x, "R"] = "TIPO DE PRESTAMO:";

                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoDataCreditoGetByFecha(Hasta, SucursalID);
                //dsDataCredito = Variables.saoWS.PrintDataCreditoGetByFecha(Hasta, IsCapital);


                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;
                    workSheet.Range["A" + x, "A" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "A"] = Fila["ContratoID"].ToString();
                    workSheet.Cells[x, "B"] = string.Format("{0:dd/MM/yyyy}", Fila["Fecha"]);
                    workSheet.Cells[x, "C"] = string.Format("{0:dd/MM/yyyy}", Fila["Vence"]);
                    workSheet.Cells[x, "D"] = Fila["Documento"].ToString();
                    workSheet.Cells[x, "E"] = Fila["Cliente"].ToString();
                    workSheet.Cells[x, "F"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "G"] = Fila["Celular"].ToString();

                    workSheet.Range["H" + x, "K" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["L" + x, "L" + x].NumberFormat = "#0";
                    workSheet.Range["M" + x, "N" + x].NumberFormat = "#,##0.00";


                    workSheet.Cells[x, "H"] = Fila["Monto"];
                    workSheet.Cells[x, "I"] = Fila["Pagado"];
                    workSheet.Cells[x, "J"] = Fila["Balance"];
                    workSheet.Cells[x, "K"] = Fila["Cuota"];
                    workSheet.Cells[x, "L"] = Fila["Cantidad"].ToString();
                    workSheet.Cells[x, "M"] = Fila["Atraso"];
                    workSheet.Cells[x, "N"] = Fila["MontoUltimoPago"];

                    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Cells[x, "O"] = string.Format("{0:dd/MM/yyyy}", Fila["FechaUltimoPago"]);
                    }
                    workSheet.Cells[x, "P"] = float.Parse(Fila["Balance"].ToString()) == 0 ? "S" : Fila["Status"].ToString();
                    workSheet.Cells[x, "Q"] = Fila["Deudor"].ToString();
                    workSheet.Cells[x, "R"] = Fila["Tipo"].ToString();
                }

                //for (int i = 2; i <= 25; i++)
                //{
                //    workSheet.Columns[i].AutoFit();
                //}

                /*  x = x + 4;
                  int A = x;
                  workSheet.Cells[x, "A"] = "S:";
                  workSheet.Cells[x, "B"] = "SALDO";
                  workSheet.Cells[x+1, "A"] = "N:";
                  workSheet.Cells[x + 1, "B"] = "NORMAL";
                  workSheet.Cells[x+2, "A"] = "M:";
                  workSheet.Cells[x+2, "B"] = "MORA";
                  workSheet.Cells[x+3, "A"] = "L:";
                  workSheet.Cells[x+3, "B"] = "LEGAL";
                  workSheet.Cells[x+4, "A"] = "C:";
                  workSheet.Cells[x+4, "B"] = "CASTIGADO";

                  workSheet.Range["A" + A, "B" + x].AutoFormat(
  Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);*/

                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;
            
            }
        }

        public String NewDataCreditoGetByFecha(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                //DataSet dsEmpresas = default(DataSet);
                //dsEmpresas = Variables.saoWS.EmpresasGet();


                //foreach (DataRow Fila in dsEmpresas.Tables["Empresas"].Rows)
                //{
                workSheet.Name = "Listado de Equifax";
                workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                workSheet.Cells[6, "A"] = "Listado Equifax al " + Hasta.ToShortDateString();
                //}

                workSheet.Range["A1", "A1"].Font.Bold = true;
                workSheet.Range["A1", "A1"].Font.Size = 15;

                workSheet.Range["A2", "A2"].Font.Bold = false;
                workSheet.Range["A2", "A2"].Font.Size = 12;

                workSheet.Range["A3", "A3"].Font.Bold = false;
                workSheet.Range["A3", "A3"].Font.Size = 12;

                workSheet.Range["A6", "A6"].Font.Bold = true;
                workSheet.Range["A6", "A6"].Font.Size = 12;

                //Encabezado de Columnas
                int x = 8;
                workSheet.Range["A" + x, "BC" + x].Font.Bold = true;
                //workSheet.Range["A" + x, "BC" + x].Columns.AutoFit();
                workSheet.Range["A" + x, "BC" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                workSheet.Cells[x, "A"] = "TIPO DE ENTIDAD:";
                workSheet.Cells[x, "B"] = "NOMBRES:";
                workSheet.Cells[x, "C"] = "APELLIDOS:";

                workSheet.Cells[x, "D"] = "CEDULA O RNC:";
                workSheet.Cells[x, "E"] = "SEXO:";
                workSheet.Cells[x, "F"] = "ESTADO CIVIL:";
                workSheet.Cells[x, "G"] = "OCUPACION:";
                workSheet.Cells[x, "H"] = "CODIGO DEL CLIENTE:";
                workSheet.Cells[x, "I"] = "FECHA DE NACIMIENTO:";
                workSheet.Cells[x, "J"] = "NACIONALIDAD:";
                workSheet.Cells[x, "K"] = "DIRECCION:";
                workSheet.Cells[x, "L"] = "SECTOR:";
                workSheet.Cells[x, "M"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "N"] = "MUNICIPIO:";
                workSheet.Cells[x, "O"] = "CIUDAD:";
                workSheet.Cells[x, "P"] = "PROVINCIA:";
                workSheet.Cells[x, "Q"] = "PAIS";
                workSheet.Cells[x, "R"] = "DIRECCION DE REFERENCIA:";

                workSheet.Cells[x, "S"] = "TELEFONO:";
                workSheet.Cells[x, "T"] = "CELULAR:";
                workSheet.Cells[x, "U"] = "EMPRESA DONDE TRABAJA:";
                workSheet.Cells[x, "V"] = "CARGO:";
                workSheet.Cells[x, "W"] = "DIRECCION:";
                workSheet.Cells[x, "X"] = "SECTOR:";
                workSheet.Cells[x, "Y"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "Z"] = "MUNICIPIO:";
                workSheet.Cells[x, "AA"] = "CIUDAD:";
                workSheet.Cells[x, "AB"] = "PROVINCIA:";
                workSheet.Cells[x, "AC"] = "PAIS:";
                workSheet.Cells[x, "AD"] = "DIRECCION DE REFERENCIA:";
                workSheet.Cells[x, "AE"] = "SALARIO MENSUAL:";
                workSheet.Cells[x, "AF"] = "MONEDA SALARIO:";

                workSheet.Cells[x, "AG"] = "RELACION TIPO:";
                workSheet.Cells[x, "AH"] = "FECHA APERTURA:";
                workSheet.Cells[x, "AI"] = "FECHA VENCIMIENTO:";
                workSheet.Cells[x, "AJ"] = "FECHA ULTIMO PAGO:";
                workSheet.Cells[x, "AK"] = "NUMERO DE CUENTA:";
                workSheet.Cells[x, "AL"] = "ESTATUS:";
                workSheet.Cells[x, "AM"] = "TIPO DE FINANCIAMIENTO:";
                workSheet.Cells[x, "AN"] = "MONEDA:";
                workSheet.Cells[x, "AO"] = "CREDITO APROBADO:";
                workSheet.Cells[x, "AP"] = "MONTO ADEUDADO:";
                workSheet.Cells[x, "AQ"] = "PAGO MANDATORIO O CUOTA:";
                workSheet.Cells[x, "AR"] = "MONTO ULTIMO PAGO:";
                workSheet.Cells[x, "AS"] = "TOTAL DE ATRASO:";
                workSheet.Cells[x, "AT"] = "TASA DE INTERES:";
                workSheet.Cells[x, "AU"] = "FORMA DE PAGO:";
                workSheet.Cells[x, "AV"] = "CANTIDAD DE CUOTAS:";
                workSheet.Cells[x, "AW"] = "ATRASO DE 1 A 30 DIAS:";
                workSheet.Cells[x, "AX"] = "ATRASO DE 31 A 60 DIAS:";
                workSheet.Cells[x, "AY"] = "ATRASO DE 61 A 90 DIAS:";
                workSheet.Cells[x, "AZ"] = "ATRASO DE 91 A 120 DIAS:";
                workSheet.Cells[x, "BA"] = "ATRASO DE 121 A 150 DIAS:";
                workSheet.Cells[x, "BB"] = "ATRASO DE 151 A 180 DIAS:";
                workSheet.Cells[x, "BC"] = "ATRASO DE MAS 181 DIAS:";



                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoDataCreditoGetByFecha(Hasta, SucursalID);


                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;
                    workSheet.Cells[x, "A"] = Fila["TipoEntidad"].ToString();
                    workSheet.Cells[x, "B"] = Fila["Nombres"].ToString();
                    workSheet.Cells[x, "C"] = Fila["Apellidos"].ToString();
                    workSheet.Cells[x, "D"] = Fila["Documento"].ToString();
                    workSheet.Cells[x, "E"] = Fila["Sexo"].ToString();
                    workSheet.Cells[x, "F"] = Fila["EstadoCivil"].ToString();
                    workSheet.Cells[x, "G"] = Fila["Ocupacion"].ToString();

                    workSheet.Range["H" + x, "H" + x].NumberFormat = "000000";
                    workSheet.Range["H" + x, "H" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "H"] = Fila["ClienteID"].ToString();

                    workSheet.Range["I" + x, "I" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "I"] = string.Format("{0:dd/MM/yyyy}", Fila["FechaNacimiento"]);

                    workSheet.Cells[x, "J"] = Fila["Nacionalidad"].ToString();
                    workSheet.Cells[x, "K"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "L"] = "";
                    workSheet.Cells[x, "M"] = "";
                    workSheet.Cells[x, "N"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "O"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "P"] = Fila["Provincia"].ToString();
                    workSheet.Cells[x, "Q"] = Fila["Pais"].ToString();
                    workSheet.Cells[x, "R"] = "";

                    workSheet.Range["S" + x, "S" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "S"] = Fila["Telefono"].ToString();

                    workSheet.Range["T" + x, "T" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "T"] = Fila["Celular"].ToString();

                    workSheet.Cells[x, "U"] = Fila["Institucion"].ToString();
                    workSheet.Cells[x, "V"] = Fila["Ocupacion"].ToString();
                    workSheet.Cells[x, "W"] = Fila["DireccionInstitucion"].ToString();

                    workSheet.Cells[x, "X"] = "";
                    workSheet.Cells[x, "Y"] = "";
                    workSheet.Cells[x, "Z"] = "";
                    workSheet.Cells[x, "AA"] = "";

                    workSheet.Cells[x, "AB"] = "";
                    workSheet.Cells[x, "AC"] = "";
                    workSheet.Cells[x, "AD"] = "";
                    workSheet.Cells[x, "AE"] = "";
                    workSheet.Cells[x, "AF"] = "";

                    workSheet.Cells[x, "AG"] = Fila["Deudor"].ToString();

                    workSheet.Range["AH" + x, "AH" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AH"] = string.Format("{0:dd/MM/yyyy}", Fila["Fecha"]);

                    workSheet.Range["AI" + x, "AI" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AI"] = string.Format("{0:dd/MM/yyyy}", Fila["Vence"]);

                    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AJ" + x, "AJ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        workSheet.Cells[x, "AJ"] = string.Format("{0:dd/MM/yyyy}", Fila["FechaUltimoPago"]);
                    }

                    workSheet.Range["AK" + x, "AK" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "AK"] = Fila["ContratoID"].ToString();
                    //workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) <= clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.MinimumAmount  ? "S" : Fila["Status"].ToString();
                    //workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Mas180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";
                    workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";



                    workSheet.Cells[x, "AM"] = Fila["Tipo"].ToString();

                    workSheet.Cells[x, "AN"] = "RD$";

                    workSheet.Range["AO" + x, "AO" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AO" + x, "AO" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AO"] = string.Format("{0:N2}", Fila["Monto"].ToString());

                    workSheet.Range["AP" + x, "AP" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AP" + x, "AP" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AP"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Balance"].ToString());

                    workSheet.Range["AQ" + x, "AQ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AQ" + x, "AQ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AQ"] = string.Format("{0:N2}", Fila["Cuota"].ToString());

                    if (Fila["MontoUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AR" + x, "AS" + x].NumberFormat = "#,##0.00";
                        workSheet.Range["AR" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        workSheet.Cells[x, "AR"] = string.Format("{0:N2}", Fila["MontoUltimoPago"].ToString());
                    }

                    workSheet.Range["AS" + x, "AS" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AS" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AS"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Atraso"].ToString());

                    workSheet.Range["AT" + x, "AT" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AT" + x, "AT" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AT"] = string.Format("{0:N2}", Fila["Interes"].ToString());

                    workSheet.Cells[x, "AU"] = Fila["Condicion"].ToString();
                    workSheet.Cells[x, "AV"] = Fila["Tiempo"].ToString();

          
                    workSheet.Range["AW" + x, "AW" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AW" + x, "AW" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AW"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString());

                    workSheet.Range["AX" + x, "AX" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AX" + x, "AX" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AX"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString());

                    workSheet.Range["AY" + x, "AY" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AY" + x, "AY" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AY"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString());

                    workSheet.Range["AZ" + x, "AZ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AZ" + x, "AZ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AZ"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString());

                    workSheet.Range["BA" + x, "BA" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BA" + x, "BA" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BA"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString());

                    workSheet.Range["BB" + x, "BB" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BB" + x, "BB" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BB"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString());

                    workSheet.Range["BC" + x, "BC" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BC" + x, "BC" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BC"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : (Convert.ToDouble(Fila["Entre0y30"].ToString()) + Convert.ToDouble(Fila["Entre31y60"].ToString()) + Convert.ToDouble(Fila["Entre61y90"].ToString()) + Convert.ToDouble(Fila["Entre91y120"].ToString()) + Convert.ToDouble(Fila["Entre121y150"].ToString()) + Convert.ToDouble(Fila["Entre151y180"].ToString()) + Convert.ToDouble(Fila["Entre181y210"].ToString()) + Convert.ToDouble(Fila["Entre211y240"].ToString()) + Convert.ToDouble(Fila["Entre241y270"].ToString()) + Convert.ToDouble(Fila["Mas270"].ToString())).ToString());

                    workSheet.Range["A" + x, "BC" + x].Columns.AutoFit();

                    //workSheet.Cells[x, "B"] = string.Format("{0:dd/MM/yyyy}", Fila["Fecha"]);
                    //    workSheet.Cells[x, "C"] = string.Format("{0:dd/MM/yyyy}", Fila["Vence"]);
                    //    workSheet.Cells[x, "D"] = Fila["Documento"].ToString();
                    //    workSheet.Cells[x, "E"] = Fila["Cliente"].ToString();
                    //    workSheet.Cells[x, "F"] = Fila["Direccion"].ToString();
                    //    workSheet.Cells[x, "G"] = Fila["Celular"].ToString();

                    //    workSheet.Range["H" + x, "K" + x].NumberFormat = "#,##0.00";
                    //    workSheet.Range["L" + x, "L" + x].NumberFormat = "#0";
                    //    workSheet.Range["M" + x, "N" + x].NumberFormat = "#,##0.00";


                    //    workSheet.Cells[x, "H"] = Fila["Monto"];
                    //    workSheet.Cells[x, "I"] = Fila["Pagado"];
                    //    workSheet.Cells[x, "J"] = Fila["Balance"];
                    //    workSheet.Cells[x, "K"] = Fila["Cuota"];
                    //    workSheet.Cells[x, "L"] = Fila["Cantidad"].ToString();
                    //    workSheet.Cells[x, "M"] = Fila["Atraso"];
                    //    workSheet.Cells[x, "N"] = Fila["MontoUltimoPago"];

                    //    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    //    {
                    //        workSheet.Cells[x, "O"] = string.Format("{0:dd/MM/yyyy}", Fila["FechaUltimoPago"]);
                    //    }
                    //    workSheet.Cells[x, "P"] = float.Parse(Fila["Balance"].ToString()) == 0 ? "S" : Fila["Status"].ToString();
                    //    workSheet.Cells[x, "Q"] = Fila["Deudor"].ToString();
                    //    workSheet.Cells[x, "R"] = Fila["Tipo"].ToString();
                }

                x = 8;
                workSheet.Range["A" + x, "BC" + x].Columns.AutoFit();

                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }

        public String EquifaxGetByFecha(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                workSheet.Name = "Listado de Equifax";
                workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                workSheet.Cells[6, "A"] = "Listado Equifax al " + Hasta.ToShortDateString();

                workSheet.Range["A1", "A1"].Font.Bold = true;
                workSheet.Range["A1", "A1"].Font.Size = 15;

                workSheet.Range["A2", "A2"].Font.Bold = false;
                workSheet.Range["A2", "A2"].Font.Size = 12;

                workSheet.Range["A3", "A3"].Font.Bold = false;
                workSheet.Range["A3", "A3"].Font.Size = 12;

                workSheet.Range["A6", "A6"].Font.Bold = true;
                workSheet.Range["A6", "A6"].Font.Size = 12;

                //Encabezado de Columnas
                int x = 8;
                workSheet.Range["A" + x, "BG" + x].Font.Bold = true;
                workSheet.Range["A" + x, "BG" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                workSheet.Cells[x, "A"] = "TIPO DE ENTIDAD:";
                workSheet.Cells[x, "B"] = "NOMBRES:";
                workSheet.Cells[x, "C"] = "APELLIDOS:";

                workSheet.Cells[x, "D"] = "CEDULA O RNC:";
                workSheet.Cells[x, "E"] = "SEXO:";
                workSheet.Cells[x, "F"] = "ESTADO CIVIL:";
                workSheet.Cells[x, "G"] = "OCUPACION:";
                workSheet.Cells[x, "H"] = "CODIGO DEL CLIENTE:";
                workSheet.Cells[x, "I"] = "FECHA DE NACIMIENTO:";
                workSheet.Cells[x, "J"] = "NACIONALIDAD:";
                workSheet.Cells[x, "K"] = "DIRECCION:";
                workSheet.Cells[x, "L"] = "SECTOR:";
                workSheet.Cells[x, "M"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "N"] = "MUNICIPIO:";
                workSheet.Cells[x, "O"] = "CIUDAD:";
                workSheet.Cells[x, "P"] = "PROVINCIA:";
                workSheet.Cells[x, "Q"] = "PAIS";
                workSheet.Cells[x, "R"] = "DIRECCION DE REFERENCIA:";

                workSheet.Cells[x, "S"] = "TELEFONO:";
                workSheet.Cells[x, "T"] = "CELULAR:";
                workSheet.Cells[x, "U"] = "EMPRESA DONDE TRABAJA:";
                workSheet.Cells[x, "V"] = "CARGO:";
                workSheet.Cells[x, "W"] = "DIRECCION:";
                workSheet.Cells[x, "X"] = "SECTOR:";
                workSheet.Cells[x, "Y"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "Z"] = "MUNICIPIO:";
                workSheet.Cells[x, "AA"] = "CIUDAD:";
                workSheet.Cells[x, "AB"] = "PROVINCIA:";
                workSheet.Cells[x, "AC"] = "PAIS:";
                workSheet.Cells[x, "AD"] = "DIRECCION DE REFERENCIA:";
                workSheet.Cells[x, "AE"] = "SALARIO MENSUAL:";
                workSheet.Cells[x, "AF"] = "MONEDA SALARIO:";

                workSheet.Cells[x, "AG"] = "RELACION TIPO:";
                workSheet.Cells[x, "AH"] = "FECHA APERTURA:";
                workSheet.Cells[x, "AI"] = "FECHA VENCIMIENTO:";
                workSheet.Cells[x, "AJ"] = "FECHA ULTIMO PAGO:";
                workSheet.Cells[x, "AK"] = "NUMERO DE CUENTA:";
                workSheet.Cells[x, "AL"] = "ESTATUS:";
                workSheet.Cells[x, "AM"] = "TIPO DE FINANCIAMIENTO:";
                workSheet.Cells[x, "AN"] = "MONEDA:";
                workSheet.Cells[x, "AO"] = "CREDITO APROBADO:";
                workSheet.Cells[x, "AP"] = "MONTO ADEUDADO:";
                workSheet.Cells[x, "AQ"] = "PAGO MANDATORIO O CUOTA:";
                workSheet.Cells[x, "AR"] = "MONTO ULTIMO PAGO:";
                workSheet.Cells[x, "AS"] = "TOTAL DE ATRASO:";
                workSheet.Cells[x, "AT"] = "TASA DE INTERES:";
                workSheet.Cells[x, "AU"] = "FORMA DE PAGO:";
                workSheet.Cells[x, "AV"] = "CANTIDAD DE CUOTAS:";
                workSheet.Cells[x, "AW"] = "ATRASO DE 1 A 30 DIAS:";
                workSheet.Cells[x, "AX"] = "ATRASO DE 31 A 60 DIAS:";
                workSheet.Cells[x, "AY"] = "ATRASO DE 61 A 90 DIAS:";
                workSheet.Cells[x, "AZ"] = "ATRASO DE 91 A 120 DIAS:";
                workSheet.Cells[x, "BA"] = "ATRASO DE 121 A 150 DIAS:";
                workSheet.Cells[x, "BB"] = "ATRASO DE 151 A 180 DIAS:";
                workSheet.Cells[x, "BC"] = "ATRASO DE 181 A 210 DIAS:";
                workSheet.Cells[x, "BD"] = "ATRASO DE 211 A 240 DIAS:";
                workSheet.Cells[x, "BE"] = "ATRASO DE 141 A 270 DIAS:";
                workSheet.Cells[x, "BF"] = "ATRASO DE MAS 270 DIAS:";
                workSheet.Cells[x, "BG"] = "CUENTA DE REFERENCIA:";



                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoDataCreditoGetByFecha(Hasta, SucursalID);


                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;

                    //Correccion de centavos

                    Fila["Entre0y30"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    Fila["Entre31y60"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    Fila["Entre61y90"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    Fila["Entre91y120"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    Fila["Entre121y150"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    Fila["Entre151y180"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    Fila["Entre181y210"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    Fila["Entre211y240"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    Fila["Entre241y270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    Fila["Mas270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));

                    //Fin de la correccion


                    workSheet.Cells[x, "A"] = Fila["TipoEntidad"].ToString();
                    workSheet.Cells[x, "B"] = Fila["Nombres"].ToString();
                    workSheet.Cells[x, "C"] = Fila["Apellidos"].ToString();

                    workSheet.Cells[x, "D"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "D"] = Fila["Documento"].ToString().Replace("-", "").Replace("_", "");
                    workSheet.Cells[x, "E"] = Fila["Sexo"].ToString();
                    workSheet.Cells[x, "F"] = Fila["EstadoCivil"].ToString();
                    workSheet.Cells[x, "G"] = Fila["Ocupacion"].ToString();

                    workSheet.Range["H" + x, "H" + x].NumberFormat = "000000";
                    workSheet.Range["H" + x, "H" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "H"] = Fila["ClienteID"].ToString();

                    workSheet.Range["I" + x, "I" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "I"] = string.Format("{0:yyyy/MM/dd}", Fila["FechaNacimiento"]).Replace("/", "").Replace("-", "");

                    workSheet.Cells[x, "J"] = Fila["Nacionalidad"].ToString();
                    workSheet.Cells[x, "K"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "L"] = "";
                    workSheet.Cells[x, "M"] = "";
                    workSheet.Cells[x, "N"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "O"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "P"] = Fila["Provincia"].ToString();
                    workSheet.Cells[x, "Q"] = Fila["Pais"].ToString();
                    workSheet.Cells[x, "R"] = "";

                    workSheet.Range["S" + x, "S" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "S"] = Fila["Telefono"].ToString();

                    workSheet.Range["T" + x, "T" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "T"] = Fila["Celular"].ToString();

                    workSheet.Cells[x, "U"] = Fila["Institucion"].ToString();
                    workSheet.Cells[x, "V"] = Fila["Ocupacion"].ToString();
                    workSheet.Cells[x, "W"] = Fila["DireccionInstitucion"].ToString();

                    workSheet.Cells[x, "X"] = "";
                    workSheet.Cells[x, "Y"] = "";
                    workSheet.Cells[x, "Z"] = "";
                    workSheet.Cells[x, "AA"] = "";

                    workSheet.Cells[x, "AB"] = "";
                    workSheet.Cells[x, "AC"] = "";
                    workSheet.Cells[x, "AD"] = "";
                    workSheet.Cells[x, "AE"] = "";
                    workSheet.Cells[x, "AF"] = "";

                    workSheet.Cells[x, "AG"] = Fila["Deudor"].ToString();

                    workSheet.Range["AH" + x, "AH" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AH"] = string.Format("{0:yyyy/MM/dd}", Fila["Fecha"]).Replace("/", "").Replace("-", "");

                    workSheet.Range["AI" + x, "AI" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AI"] = string.Format("{0:yyyy/MM/dd}", Fila["Vence"]).Replace("/", "").Replace("-", "");

                    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AJ" + x, "AJ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        workSheet.Cells[x, "AJ"] = string.Format("{0:yyyy/MM/dd}", Fila["FechaUltimoPago"]).Replace("/", "").Replace("-", ""); ;
                    }

                    workSheet.Range["AK" + x, "AK" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "AK"] = Fila["ContratoID"].ToString();
                    workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";
                    //workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : Fila["Status"].ToString();// == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";



                    workSheet.Cells[x, "AM"] = Fila["Tipo"].ToString();

                    workSheet.Cells[x, "AN"] = "RD$";

                    workSheet.Range["AO" + x, "AO" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AO" + x, "AO" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AO"] = string.Format("{0:N2}", Fila["Monto"].ToString());

                    workSheet.Range["AP" + x, "AP" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AP" + x, "AP" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AP"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Balance"].ToString());

                    workSheet.Range["AQ" + x, "AQ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AQ" + x, "AQ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AQ"] = string.Format("{0:N2}", Fila["Cuota"].ToString());

                    if (Fila["MontoUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AR" + x, "AS" + x].NumberFormat = "#,##0.00";
                        workSheet.Range["AR" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        workSheet.Cells[x, "AR"] = string.Format("{0:N2}", Fila["MontoUltimoPago"].ToString());
                    }

                    workSheet.Range["AS" + x, "AS" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AS" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AS"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Convert.ToDouble(Fila["Atraso"]) < 1 ? "0.00" : Fila["Atraso"].ToString()); //Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());

                    workSheet.Range["AT" + x, "AT" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AT" + x, "AT" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AT"] = string.Format("{0:N2}", Fila["Interes"].ToString());

                    workSheet.Cells[x, "AU"] = Fila["Condicion"].ToString();
                    workSheet.Cells[x, "AV"] = Fila["Tiempo"].ToString();

                    double Entre0y30 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    double Entre31y60 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    double Entre61y90 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    double Entre91y120 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    double Entre121y150 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    double Entre151y180 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    double Entre181y210 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    double Entre211y240 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    double Entre241y270 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    double Mas270 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));
                    if (Mas270 > 1)
                    {
                        Entre0y30 = 0;
                        Entre31y60 = 0;
                        Entre61y90 = 0;
                        Entre91y120 = 0;
                        Entre121y150 = 0;
                        Entre151y180 = 0;
                        Entre181y210 = 0;
                        Entre211y240 = 0;
                        Entre241y270 = 0;
                        Mas270 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                    }
                    else
                    {
                        if (Entre241y270 > 1)
                        {
                            Entre0y30 = 0;
                            Entre31y60 = 0;
                            Entre61y90 = 0;
                            Entre91y120 = 0;
                            Entre121y150 = 0;
                            Entre151y180 = 0;
                            Entre181y210 = 0;
                            Entre211y240 = 0;
                            Entre241y270 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                            Mas270 = 0;
                        }
                        else
                        {
                            if (Entre211y240 > 1)
                            {
                                Entre0y30 = 0;
                                Entre31y60 = 0;
                                Entre61y90 = 0;
                                Entre91y120 = 0;
                                Entre121y150 = 0;
                                Entre151y180 = 0;
                                Entre181y210 = 0;
                                Entre211y240 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                Entre241y270 = 0;
                                Mas270 = 0;
                            }
                            else
                            {
                                if (Entre181y210 > 1)
                                {
                                    Entre0y30 = 0;
                                    Entre31y60 = 0;
                                    Entre61y90 = 0;
                                    Entre91y120 = 0;
                                    Entre121y150 = 0;
                                    Entre151y180 = 0;
                                    Entre181y210 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                    Entre211y240 = 0;
                                    Entre241y270 = 0;
                                    Mas270 = 0;
                                }
                                else
                                {
                                    if (Entre151y180 > 1)
                                    {
                                        Entre0y30 = 0;
                                        Entre31y60 = 0;
                                        Entre61y90 = 0;
                                        Entre91y120 = 0;
                                        Entre121y150 = 0;
                                        Entre151y180 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                        Entre181y210 = 0;
                                        Entre211y240 = 0;
                                        Entre241y270 = 0;
                                        Mas270 = 0;
                                    }
                                    else
                                    {
                                        if (Entre121y150 > 1)
                                        {
                                            Entre0y30 = 0;
                                            Entre31y60 = 0;
                                            Entre61y90 = 0;
                                            Entre91y120 = 0;
                                            Entre121y150 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                            Entre151y180 = 0;
                                            Entre181y210 = 0;
                                            Entre211y240 = 0;
                                            Entre241y270 = 0;
                                            Mas270 = 0;
                                        }
                                        else
                                        {
                                            if (Entre91y120 > 1)
                                            {
                                                Entre0y30 = 0;
                                                Entre31y60 = 0;
                                                Entre61y90 = 0;
                                                Entre91y120 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                Entre121y150 = 0;
                                                Entre151y180 = 0;
                                                Entre181y210 = 0;
                                                Entre211y240 = 0;
                                                Entre241y270 = 0;
                                                Mas270 = 0;
                                            }
                                            else
                                            {
                                                if (Entre61y90 > 1)
                                                {
                                                    Entre0y30 = 0;
                                                    Entre31y60 = 0;
                                                    Entre61y90 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                    Entre91y120 = 0;
                                                    Entre121y150 = 0;
                                                    Entre151y180 = 0;
                                                    Entre181y210 = 0;
                                                    Entre211y240 = 0;
                                                    Entre241y270 = 0;
                                                    Mas270 = 0;
                                                }
                                                else
                                                {
                                                    if (Entre31y60 > 1)
                                                    {
                                                        Entre0y30 = 0;
                                                        Entre31y60 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                        Entre61y90 = 0;
                                                        Entre91y120 = 0;
                                                        Entre121y150 = 0;
                                                        Entre151y180 = 0;
                                                        Entre181y210 = 0;
                                                        Entre211y240 = 0;
                                                        Entre241y270 = 0;
                                                        Mas270 = 0;
                                                    }
                                                    else
                                                    {
                                                        if (Entre0y30 > 1)
                                                        {
                                                            Entre0y30 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                            Entre31y60 = 0;
                                                            Entre61y90 = 0;
                                                            Entre91y120 = 0;
                                                            Entre121y150 = 0;
                                                            Entre151y180 = 0;
                                                            Entre181y210 = 0;
                                                            Entre211y240 = 0;
                                                            Entre241y270 = 0;
                                                            Mas270 = 0;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    workSheet.Range["AW" + x, "AW" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AW" + x, "AW" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AW"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString());

                    workSheet.Range["AX" + x, "AX" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AX" + x, "AX" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AX"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString());

                    workSheet.Range["AY" + x, "AY" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AY" + x, "AY" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AY"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString());

                    workSheet.Range["AZ" + x, "AZ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AZ" + x, "AZ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AZ"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString());

                    workSheet.Range["BA" + x, "BA" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BA" + x, "BA" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BA"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString());

                    workSheet.Range["BB" + x, "BB" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BB" + x, "BB" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BB"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString());

                    workSheet.Range["BC" + x, "BC" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BC" + x, "BC" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BC"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString());

                    workSheet.Range["BD" + x, "BD" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BD" + x, "BD" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BD"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString());

                    workSheet.Range["BE" + x, "BE" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BE" + x, "BE" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BE"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString());

                    workSheet.Range["BF" + x, "BF" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BF" + x, "BF" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BF"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString());


                    workSheet.Cells[x, "BG"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "BG"] = Fila["Documento"].ToString().Replace("-", "").Replace("_", "");

                    workSheet.Range["A" + x, "BG" + x].Columns.AutoFit();


                }

                x = 8;
                workSheet.Range["A" + x, "BG" + x].Columns.AutoFit();

                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }

        public String EquifaxGetByFechaModificado(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                workSheet.Name = "Listado de Equifax";
                workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                workSheet.Cells[6, "A"] = "Listado Equifax al " + Hasta.ToShortDateString();
             
                workSheet.Range["A1", "A1"].Font.Bold = true;
                workSheet.Range["A1", "A1"].Font.Size = 15;

                workSheet.Range["A2", "A2"].Font.Bold = false;
                workSheet.Range["A2", "A2"].Font.Size = 12;

                workSheet.Range["A3", "A3"].Font.Bold = false;
                workSheet.Range["A3", "A3"].Font.Size = 12;

                workSheet.Range["A6", "A6"].Font.Bold = true;
                workSheet.Range["A6", "A6"].Font.Size = 12;

                //Encabezado de Columnas
                int x = 8;
                workSheet.Range["A" + x, "BG" + x].Font.Bold = true;
                workSheet.Range["A" + x, "BG" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                workSheet.Cells[x, "A"] = "TIPO DE ENTIDAD:";
                workSheet.Cells[x, "B"] = "NOMBRES:";
                workSheet.Cells[x, "C"] = "APELLIDOS:";

                workSheet.Cells[x, "D"] = "CEDULA O RNC:";
                workSheet.Cells[x, "E"] = "SEXO:";
                workSheet.Cells[x, "F"] = "ESTADO CIVIL:";
                workSheet.Cells[x, "G"] = "OCUPACION:";
                workSheet.Cells[x, "H"] = "CODIGO DEL CLIENTE:";
                workSheet.Cells[x, "I"] = "FECHA DE NACIMIENTO:";
                workSheet.Cells[x, "J"] = "NACIONALIDAD:";
                workSheet.Cells[x, "K"] = "DIRECCION:";
                workSheet.Cells[x, "L"] = "SECTOR:";
                workSheet.Cells[x, "M"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "N"] = "MUNICIPIO:";
                workSheet.Cells[x, "O"] = "CIUDAD:";
                workSheet.Cells[x, "P"] = "PROVINCIA:";
                workSheet.Cells[x, "Q"] = "PAIS";
                workSheet.Cells[x, "R"] = "DIRECCION DE REFERENCIA:";

                workSheet.Cells[x, "S"] = "TELEFONO:";
                workSheet.Cells[x, "T"] = "CELULAR:";
                workSheet.Cells[x, "U"] = "EMPRESA DONDE TRABAJA:";
                workSheet.Cells[x, "V"] = "CARGO:";
                workSheet.Cells[x, "W"] = "DIRECCION:";
                workSheet.Cells[x, "X"] = "SECTOR:";
                workSheet.Cells[x, "Y"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "Z"] = "MUNICIPIO:";
                workSheet.Cells[x, "AA"] = "CIUDAD:";
                workSheet.Cells[x, "AB"] = "PROVINCIA:";
                workSheet.Cells[x, "AC"] = "PAIS:";
                workSheet.Cells[x, "AD"] = "DIRECCION DE REFERENCIA:";
                workSheet.Cells[x, "AE"] = "SALARIO MENSUAL:";
                workSheet.Cells[x, "AF"] = "MONEDA SALARIO:";

                workSheet.Cells[x, "AG"] = "RELACION TIPO:";
                workSheet.Cells[x, "AH"] = "FECHA APERTURA:";
                workSheet.Cells[x, "AI"] = "FECHA VENCIMIENTO:";
                workSheet.Cells[x, "AJ"] = "FECHA ULTIMO PAGO:";
                workSheet.Cells[x, "AK"] = "NUMERO DE CUENTA:";
                workSheet.Cells[x, "AL"] = "ESTATUS:";
                workSheet.Cells[x, "AM"] = "TIPO DE FINANCIAMIENTO:";
                workSheet.Cells[x, "AN"] = "MONEDA:";
                workSheet.Cells[x, "AO"] = "CREDITO APROBADO:";
                workSheet.Cells[x, "AP"] = "MONTO ADEUDADO:";
                workSheet.Cells[x, "AQ"] = "PAGO MANDATORIO O CUOTA:";
                workSheet.Cells[x, "AR"] = "MONTO ULTIMO PAGO:";
                workSheet.Cells[x, "AS"] = "TOTAL DE ATRASO:";
                workSheet.Cells[x, "AT"] = "TASA DE INTERES:";
                workSheet.Cells[x, "AU"] = "FORMA DE PAGO:";
                workSheet.Cells[x, "AV"] = "CANTIDAD DE CUOTAS:";
                workSheet.Cells[x, "AW"] = "ATRASO DE 1 A 30 DIAS:";
                workSheet.Cells[x, "AX"] = "ATRASO DE 31 A 60 DIAS:";
                workSheet.Cells[x, "AY"] = "ATRASO DE 61 A 90 DIAS:";
                workSheet.Cells[x, "AZ"] = "ATRASO DE 91 A 120 DIAS:";
                workSheet.Cells[x, "BA"] = "ATRASO DE 121 A 150 DIAS:";
                workSheet.Cells[x, "BB"] = "ATRASO DE 151 A 180 DIAS:";
                workSheet.Cells[x, "BC"] = "ATRASO DE 181 A 210 DIAS:";
                workSheet.Cells[x, "BD"] = "ATRASO DE 211 A 240 DIAS:";
                workSheet.Cells[x, "BE"] = "ATRASO DE 141 A 270 DIAS:";
                workSheet.Cells[x, "BF"] = "ATRASO DE MAS 270 DIAS:";
                workSheet.Cells[x, "BG"] = "CUENTA DE REFERENCIA:";



                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoDataCreditoGetByFecha(Hasta, SucursalID);


                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;

                    //Correccion de centavos

                    Fila["Entre0y30"] =  Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()))  < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    Fila["Entre31y60"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    Fila["Entre61y90"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    Fila["Entre91y120"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    Fila["Entre121y150"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    Fila["Entre151y180"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    Fila["Entre181y210"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    Fila["Entre211y240"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    Fila["Entre241y270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    Fila["Mas270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));

                    //Fin de la correccion


                    workSheet.Cells[x, "A"] = Fila["TipoEntidad"].ToString();
                    workSheet.Cells[x, "B"] = Fila["Nombres"].ToString();
                    workSheet.Cells[x, "C"] = Fila["Apellidos"].ToString();

                    workSheet.Cells[x, "D"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "D"] = Fila["Documento"].ToString().Replace("-","").Replace("_","");
                    workSheet.Cells[x, "E"] = Fila["Sexo"].ToString();
                    workSheet.Cells[x, "F"] = Fila["EstadoCivil"].ToString();
                    workSheet.Cells[x, "G"] = Fila["Ocupacion"].ToString();

                    workSheet.Range["H" + x, "H" + x].NumberFormat = "000000";
                    workSheet.Range["H" + x, "H" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "H"] = Fila["ClienteID"].ToString();

                    workSheet.Range["I" + x, "I" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "I"] = string.Format("{0:yyyy/MM/dd}", Fila["FechaNacimiento"]).Replace("/","").Replace("-","");

                    workSheet.Cells[x, "J"] = Fila["Nacionalidad"].ToString();
                    workSheet.Cells[x, "K"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "L"] = "";
                    workSheet.Cells[x, "M"] = "";
                    workSheet.Cells[x, "N"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "O"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "P"] = Fila["Provincia"].ToString();
                    workSheet.Cells[x, "Q"] = Fila["Pais"].ToString();
                    workSheet.Cells[x, "R"] = "";

                    workSheet.Range["S" + x, "S" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "S"] = Fila["Telefono"].ToString();

                    workSheet.Range["T" + x, "T" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "T"] = Fila["Celular"].ToString();

                    workSheet.Cells[x, "U"] = Fila["Institucion"].ToString();
                    workSheet.Cells[x, "V"] = Fila["Ocupacion"].ToString();
                    workSheet.Cells[x, "W"] = Fila["DireccionInstitucion"].ToString();

                    workSheet.Cells[x, "X"] = "";
                    workSheet.Cells[x, "Y"] = "";
                    workSheet.Cells[x, "Z"] = "";
                    workSheet.Cells[x, "AA"] = "";

                    workSheet.Cells[x, "AB"] = "";
                    workSheet.Cells[x, "AC"] = "";
                    workSheet.Cells[x, "AD"] = "";
                    workSheet.Cells[x, "AE"] = "";
                    workSheet.Cells[x, "AF"] = "";

                    workSheet.Cells[x, "AG"] = Fila["Deudor"].ToString();

                    workSheet.Range["AH" + x, "AH" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AH"] = string.Format("{0:yyyy/MM/dd}", Fila["Fecha"]).Replace("/", "").Replace("-", "");

                    workSheet.Range["AI" + x, "AI" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AI"] = string.Format("{0:yyyy/MM/dd}", Fila["Vence"]).Replace("/", "").Replace("-", "");

                    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AJ" + x, "AJ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        workSheet.Cells[x, "AJ"] = string.Format("{0:yyyy/MM/dd}", Fila["FechaUltimoPago"]).Replace("/", "").Replace("-", ""); ;
                    }

                    workSheet.Range["AK" + x, "AK" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "AK"] = Fila["ContratoID"].ToString();
                    workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";
                    //workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : Fila["Status"].ToString();// == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";



                    workSheet.Cells[x, "AM"] = Fila["Tipo"].ToString();

                    workSheet.Cells[x, "AN"] = "RD$";

                    workSheet.Range["AO" + x, "AO" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AO" + x, "AO" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AO"] = string.Format("{0:N2}", Fila["Monto"].ToString());

                    workSheet.Range["AP" + x, "AP" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AP" + x, "AP" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AP"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Balance"].ToString());

                    workSheet.Range["AQ" + x, "AQ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AQ" + x, "AQ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AQ"] = string.Format("{0:N2}", Fila["Cuota"].ToString());

                    if (Fila["MontoUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AR" + x, "AS" + x].NumberFormat = "#,##0.00";
                        workSheet.Range["AR" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        workSheet.Cells[x, "AR"] = string.Format("{0:N2}", Fila["MontoUltimoPago"].ToString());
                    }

                    workSheet.Range["AS" + x, "AS" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AS" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AS"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Convert.ToDouble(Fila["Atraso"]) < 1 ? "0.00" : Fila["Atraso"].ToString()); //Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());

                    workSheet.Range["AT" + x, "AT" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AT" + x, "AT" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AT"] = string.Format("{0:N2}", Fila["Interes"].ToString());

                    workSheet.Cells[x, "AU"] = Fila["Condicion"].ToString();
                    workSheet.Cells[x, "AV"] = Fila["Tiempo"].ToString();

                    double Entre0y30 =    Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    double Entre31y60 =   Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    double Entre61y90 =   Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    double Entre91y120 =  Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    double Entre121y150 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    double Entre151y180 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    double Entre181y210 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    double Entre211y240 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    double Entre241y270 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    double Mas270 =       Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));
                    if (Mas270 > 1)
                    {
                        Entre0y30 = 0;
                        Entre31y60 = 0;
                        Entre61y90 = 0;
                        Entre91y120 = 0;
                        Entre121y150 = 0;
                        Entre151y180 = 0;
                        Entre181y210 = 0;
                        Entre211y240 = 0;
                        Entre241y270 = 0;
                        Mas270 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                    }
                    else
                    {
                        if (Entre241y270 > 1)
                        {
                            Entre0y30 = 0;
                            Entre31y60 = 0;
                            Entre61y90 = 0;
                            Entre91y120 = 0;
                            Entre121y150 = 0;
                            Entre151y180 = 0;
                            Entre181y210 = 0;
                            Entre211y240 = 0;
                            Entre241y270 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                            Mas270 = 0;
                        }
                        else
                        {
                            if (Entre211y240 > 1)
                            {
                                Entre0y30 = 0;
                                Entre31y60 = 0;
                                Entre61y90 = 0;
                                Entre91y120 = 0;
                                Entre121y150 = 0;
                                Entre151y180 = 0;
                                Entre181y210 = 0;
                                Entre211y240 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                Entre241y270 = 0;
                                Mas270 = 0;
                            }
                            else
                            {
                                if (Entre181y210 > 1)
                                {
                                    Entre0y30 = 0;
                                    Entre31y60 = 0;
                                    Entre61y90 = 0;
                                    Entre91y120 = 0;
                                    Entre121y150 = 0;
                                    Entre151y180 = 0;
                                    Entre181y210 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                    Entre211y240 = 0;
                                    Entre241y270 = 0;
                                    Mas270 = 0;
                                }
                                else
                                {
                                    if (Entre151y180 > 1)
                                    {
                                        Entre0y30 = 0;
                                        Entre31y60 = 0;
                                        Entre61y90 = 0;
                                        Entre91y120 = 0;
                                        Entre121y150 = 0;
                                        Entre151y180 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                        Entre181y210 = 0;
                                        Entre211y240 = 0;
                                        Entre241y270 = 0;
                                        Mas270 = 0;
                                    }
                                    else
                                    {
                                        if (Entre121y150 > 1)
                                        {
                                            Entre0y30 = 0;
                                            Entre31y60 = 0;
                                            Entre61y90 = 0;
                                            Entre91y120 = 0;
                                            Entre121y150 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                            Entre151y180 = 0;
                                            Entre181y210 = 0;
                                            Entre211y240 = 0;
                                            Entre241y270 = 0;
                                            Mas270 = 0;
                                        }
                                        else
                                        {
                                            if (Entre91y120 > 1)
                                            {
                                                Entre0y30 = 0;
                                                Entre31y60 = 0;
                                                Entre61y90 = 0;
                                                Entre91y120 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                Entre121y150 = 0;
                                                Entre151y180 = 0;
                                                Entre181y210 = 0;
                                                Entre211y240 = 0;
                                                Entre241y270 = 0;
                                                Mas270 = 0;
                                            }
                                            else
                                            {
                                                if (Entre61y90 > 1)
                                                {
                                                    Entre0y30 = 0;
                                                    Entre31y60 = 0;
                                                    Entre61y90 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                    Entre91y120 = 0;
                                                    Entre121y150 = 0;
                                                    Entre151y180 = 0;
                                                    Entre181y210 = 0;
                                                    Entre211y240 = 0;
                                                    Entre241y270 = 0;
                                                    Mas270 = 0;
                                                }
                                                else
                                                {
                                                    if (Entre31y60 > 1)
                                                    {
                                                        Entre0y30 = 0;
                                                        Entre31y60 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                        Entre61y90 = 0;
                                                        Entre91y120 = 0;
                                                        Entre121y150 = 0;
                                                        Entre151y180 = 0;
                                                        Entre181y210 = 0;
                                                        Entre211y240 = 0;
                                                        Entre241y270 = 0;
                                                        Mas270 = 0;
                                                    }
                                                    else
                                                    {
                                                        if (Entre0y30 > 1)
                                                        {
                                                            Entre0y30 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                            Entre31y60 = 0;
                                                            Entre61y90 = 0;
                                                            Entre91y120 = 0;
                                                            Entre121y150 = 0;
                                                            Entre151y180 = 0;
                                                            Entre181y210 = 0;
                                                            Entre211y240 = 0;
                                                            Entre241y270 = 0;
                                                            Mas270 = 0;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    workSheet.Range["AW" + x, "AW" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AW" + x, "AW" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AW"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre0y30.ToString());

                    workSheet.Range["AX" + x, "AX" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AX" + x, "AX" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AX"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre31y60.ToString());

                    workSheet.Range["AY" + x, "AY" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AY" + x, "AY" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AY"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre61y90.ToString());

                    workSheet.Range["AZ" + x, "AZ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AZ" + x, "AZ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AZ"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre91y120.ToString());

                    workSheet.Range["BA" + x, "BA" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BA" + x, "BA" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BA"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre121y150.ToString());

                    workSheet.Range["BB" + x, "BB" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BB" + x, "BB" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BB"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre151y180.ToString());

                    workSheet.Range["BC" + x, "BC" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BC" + x, "BC" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BC"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre181y210.ToString());

                    workSheet.Range["BD" + x, "BD" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BD" + x, "BD" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BD"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre211y240.ToString());

                    workSheet.Range["BE" + x, "BE" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BE" + x, "BE" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BE"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre241y270.ToString());

                    workSheet.Range["BF" + x, "BF" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BF" + x, "BF" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BF"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Mas270.ToString());


                    workSheet.Cells[x, "BG"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "BG"] = Fila["Documento"].ToString().Replace("-", "").Replace("_", "");

                    workSheet.Range["A" + x, "BG" + x].Columns.AutoFit();

  
                }

                x = 8;
                workSheet.Range["A" + x, "BG" + x].Columns.AutoFit();

                excelApp.Visible = true;

                return "Archivo Generado Correctamente";
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }

        public String KalifikaGetByFechaModificado(DateTime Hasta, int SucursalID)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.SheetsInNewWorkbook = 1;
                excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

                workSheet.Name = "Listado de KALIFIkA";
                workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                workSheet.Cells[6, "A"] = "Listado kalifika al " + Hasta.ToShortDateString();

                workSheet.Range["A1", "A1"].Font.Bold = true;
                workSheet.Range["A1", "A1"].Font.Size = 15;

                workSheet.Range["A2", "A2"].Font.Bold = false;
                workSheet.Range["A2", "A2"].Font.Size = 12;

                workSheet.Range["A3", "A3"].Font.Bold = false;
                workSheet.Range["A3", "A3"].Font.Size = 12;

                workSheet.Range["A6", "A6"].Font.Bold = true;
                workSheet.Range["A6", "A6"].Font.Size = 12;

                //Encabezado de Columnas
                int x = 8;
                workSheet.Range["A" + x, "BG" + x].Font.Bold = true;
                workSheet.Range["A" + x, "BG" + x].AutoFormat(
                Excel.XlRangeAutoFormat.xlRangeAutoFormatTable1);

                workSheet.Cells[x, "A"] = "TIPO DE ENTIDAD:";
                workSheet.Cells[x, "B"] = "NOMBRES:";
                workSheet.Cells[x, "C"] = "APELLIDOS:";

                workSheet.Cells[x, "D"] = "CEDULA O RNC:";
                workSheet.Cells[x, "E"] = "SEXO:";
                workSheet.Cells[x, "F"] = "ESTADO CIVIL:";
                workSheet.Cells[x, "G"] = "OCUPACION:";
                workSheet.Cells[x, "H"] = "CODIGO DEL CLIENTE:";
                workSheet.Cells[x, "I"] = "FECHA DE NACIMIENTO:";
                workSheet.Cells[x, "J"] = "NACIONALIDAD:";
                workSheet.Cells[x, "K"] = "DIRECCION:";
                workSheet.Cells[x, "L"] = "SECTOR:";
                workSheet.Cells[x, "M"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "N"] = "MUNICIPIO:";
                workSheet.Cells[x, "O"] = "CIUDAD:";
                workSheet.Cells[x, "P"] = "PROVINCIA:";
                workSheet.Cells[x, "Q"] = "PAIS";
                workSheet.Cells[x, "R"] = "DIRECCION DE REFERENCIA:";

                workSheet.Cells[x, "S"] = "TELEFONO:";
                workSheet.Cells[x, "T"] = "CELULAR:";
                workSheet.Cells[x, "U"] = "EMPRESA DONDE TRABAJA:";
                workSheet.Cells[x, "V"] = "CARGO:";
                workSheet.Cells[x, "W"] = "DIRECCION:";
                workSheet.Cells[x, "X"] = "SECTOR:";
                workSheet.Cells[x, "Y"] = "CALLE / NUMERO:";
                workSheet.Cells[x, "Z"] = "MUNICIPIO:";
                workSheet.Cells[x, "AA"] = "CIUDAD:";
                workSheet.Cells[x, "AB"] = "PROVINCIA:";
                workSheet.Cells[x, "AC"] = "PAIS:";
                workSheet.Cells[x, "AD"] = "DIRECCION DE REFERENCIA:";
                workSheet.Cells[x, "AE"] = "SALARIO MENSUAL:";
                workSheet.Cells[x, "AF"] = "MONEDA SALARIO:";

                workSheet.Cells[x, "AG"] = "RELACION TIPO:";
                workSheet.Cells[x, "AH"] = "FECHA APERTURA:";
                workSheet.Cells[x, "AI"] = "FECHA VENCIMIENTO:";
                workSheet.Cells[x, "AJ"] = "FECHA ULTIMO PAGO:";
                workSheet.Cells[x, "AK"] = "NUMERO DE CUENTA:";
                workSheet.Cells[x, "AL"] = "ESTATUS:";
                workSheet.Cells[x, "AM"] = "TIPO DE FINANCIAMIENTO:";
                workSheet.Cells[x, "AN"] = "MONEDA:";
                workSheet.Cells[x, "AO"] = "CREDITO APROBADO:";
                workSheet.Cells[x, "AP"] = "MONTO ADEUDADO:";
                workSheet.Cells[x, "AQ"] = "PAGO MANDATORIO O CUOTA:";
                workSheet.Cells[x, "AR"] = "MONTO ULTIMO PAGO:";
                workSheet.Cells[x, "AS"] = "TOTAL DE ATRASO:";
                workSheet.Cells[x, "AT"] = "TASA DE INTERES:";
                workSheet.Cells[x, "AU"] = "FORMA DE PAGO:";
                workSheet.Cells[x, "AV"] = "CANTIDAD DE CUOTAS:";
                workSheet.Cells[x, "AW"] = "ATRASO DE 1 A 30 DIAS:";
                workSheet.Cells[x, "AX"] = "ATRASO DE 31 A 60 DIAS:";
                workSheet.Cells[x, "AY"] = "ATRASO DE 61 A 90 DIAS:";
                workSheet.Cells[x, "AZ"] = "ATRASO DE 91 A 120 DIAS:";
                workSheet.Cells[x, "BA"] = "ATRASO DE 121 A 150 DIAS:";
                workSheet.Cells[x, "BB"] = "ATRASO DE 151 A 180 DIAS:";
                workSheet.Cells[x, "BC"] = "ATRASO DE 181 A 210 DIAS:";
                workSheet.Cells[x, "BD"] = "ATRASO DE 211 A 240 DIAS:";
                workSheet.Cells[x, "BE"] = "ATRASO DE 141 A 270 DIAS:";
                workSheet.Cells[x, "BF"] = "ATRASO DE MAS 270 DIAS:";
                workSheet.Cells[x, "BG"] = "CUENTA DE REFERENCIA:";



                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoDataCreditoGetByFecha(Hasta, SucursalID);


                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;

                    //Correccion de centavos

                    Fila["Entre0y30"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    Fila["Entre31y60"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    Fila["Entre61y90"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    Fila["Entre91y120"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    Fila["Entre121y150"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    Fila["Entre151y180"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    Fila["Entre181y210"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    Fila["Entre211y240"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    Fila["Entre241y270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    Fila["Mas270"] = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString())) < 1 ? 0 : Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));

                    //Fin de la correccion


                    workSheet.Cells[x, "A"] = Fila["TipoEntidad"].ToString();
                    workSheet.Cells[x, "B"] = Fila["Nombres"].ToString();
                    workSheet.Cells[x, "C"] = Fila["Apellidos"].ToString();

                    workSheet.Cells[x, "D"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "D"] = Fila["Documento"].ToString().Replace("-", "").Replace("_", "");
                    workSheet.Cells[x, "E"] = Fila["Sexo"].ToString();
                    workSheet.Cells[x, "F"] = Fila["EstadoCivil"].ToString();
                    workSheet.Cells[x, "G"] = Fila["Ocupacion"].ToString();

                    workSheet.Range["H" + x, "H" + x].NumberFormat = "000000";
                    workSheet.Range["H" + x, "H" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "H"] = Fila["ClienteID"].ToString();

                    workSheet.Range["I" + x, "I" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "I"] = string.Format("{0:yyyy/MM/dd}", Fila["FechaNacimiento"]).Replace("/", "").Replace("-", "");

                    workSheet.Cells[x, "J"] = Fila["Nacionalidad"].ToString();
                    workSheet.Cells[x, "K"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "L"] = "";
                    workSheet.Cells[x, "M"] = "";
                    workSheet.Cells[x, "N"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "O"] = Fila["Ciudad"].ToString();
                    workSheet.Cells[x, "P"] = Fila["Provincia"].ToString();
                    workSheet.Cells[x, "Q"] = Fila["Pais"].ToString();
                    workSheet.Cells[x, "R"] = "";

                    workSheet.Range["S" + x, "S" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "S"] = Fila["Telefono"].ToString();

                    workSheet.Range["T" + x, "T" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "T"] = Fila["Celular"].ToString();

                    workSheet.Cells[x, "U"] = Fila["Institucion"].ToString();
                    workSheet.Cells[x, "V"] = Fila["Ocupacion"].ToString();
                    workSheet.Cells[x, "W"] = Fila["DireccionInstitucion"].ToString();

                    workSheet.Cells[x, "X"] = "";
                    workSheet.Cells[x, "Y"] = "";
                    workSheet.Cells[x, "Z"] = "";
                    workSheet.Cells[x, "AA"] = "";

                    workSheet.Cells[x, "AB"] = "";
                    workSheet.Cells[x, "AC"] = "";
                    workSheet.Cells[x, "AD"] = "";
                    workSheet.Cells[x, "AE"] = "";
                    workSheet.Cells[x, "AF"] = "";

                    workSheet.Cells[x, "AG"] = Fila["Deudor"].ToString();

                    workSheet.Range["AH" + x, "AH" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AH"] = string.Format("{0:yyyy/MM/dd}", Fila["Fecha"]).Replace("/", "").Replace("-", "");

                    workSheet.Range["AI" + x, "AI" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    workSheet.Cells[x, "AI"] = string.Format("{0:yyyy/MM/dd}", Fila["Vence"]).Replace("/", "").Replace("-", "");

                    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AJ" + x, "AJ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        workSheet.Cells[x, "AJ"] = string.Format("{0:yyyy/MM/dd}", Fila["FechaUltimoPago"]).Replace("/", "").Replace("-", ""); ;
                    }

                    workSheet.Range["AK" + x, "AK" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "AK"] = Fila["ContratoID"].ToString();
                    workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";
                    //workSheet.Cells[x, "AL"] = float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : Fila["Status"].ToString();// == "S" ? "0" : Fila["Mas270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()) > 0.1 ? "C" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()) > 0.1 ? "L" : float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()) + float.Parse(Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()) > 0.1 ? "M" : float.Parse(Fila["Balance"].ToString()) < 0.1 ? "S" : "N";



                    workSheet.Cells[x, "AM"] = Fila["Tipo"].ToString();

                    workSheet.Cells[x, "AN"] = "RD$";

                    workSheet.Range["AO" + x, "AO" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AO" + x, "AO" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AO"] = string.Format("{0:N2}", Fila["Monto"].ToString());

                    workSheet.Range["AP" + x, "AP" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AP" + x, "AP" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AP"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Balance"].ToString());

                    workSheet.Range["AQ" + x, "AQ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AQ" + x, "AQ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AQ"] = string.Format("{0:N2}", Fila["Cuota"].ToString());

                    if (Fila["MontoUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["AR" + x, "AS" + x].NumberFormat = "#,##0.00";
                        workSheet.Range["AR" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        workSheet.Cells[x, "AR"] = string.Format("{0:N2}", Fila["MontoUltimoPago"].ToString());
                    }

                    workSheet.Range["AS" + x, "AS" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AS" + x, "AS" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AS"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Convert.ToDouble(Fila["Atraso"]) < 1 ? "0.00" : Fila["Atraso"].ToString()); //Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());

                    workSheet.Range["AT" + x, "AT" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AT" + x, "AT" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AT"] = string.Format("{0:N2}", Fila["Interes"].ToString());

                    workSheet.Cells[x, "AU"] = Fila["Condicion"].ToString();
                    workSheet.Cells[x, "AV"] = Fila["Tiempo"].ToString();

                    double Entre0y30 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre0y30"].ToString()));
                    double Entre31y60 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre31y60"].ToString()));
                    double Entre61y90 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre61y90"].ToString()));
                    double Entre91y120 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre91y120"].ToString()));
                    double Entre121y150 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre121y150"].ToString()));
                    double Entre151y180 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre151y180"].ToString()));
                    double Entre181y210 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre181y210"].ToString()));
                    double Entre211y240 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre211y240"].ToString()));
                    double Entre241y270 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Entre241y270"].ToString()));
                    double Mas270 = Convert.ToDouble(string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Fila["Mas270"].ToString()));
                    if (Mas270 > 1)
                    {
                        Entre0y30 = 0;
                        Entre31y60 = 0;
                        Entre61y90 = 0;
                        Entre91y120 = 0;
                        Entre121y150 = 0;
                        Entre151y180 = 0;
                        Entre181y210 = 0;
                        Entre211y240 = 0;
                        Entre241y270 = 0;
                        Mas270 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                    }
                    else
                    {
                        if (Entre241y270 > 1)
                        {
                            Entre0y30 = 0;
                            Entre31y60 = 0;
                            Entre61y90 = 0;
                            Entre91y120 = 0;
                            Entre121y150 = 0;
                            Entre151y180 = 0;
                            Entre181y210 = 0;
                            Entre211y240 = 0;
                            Entre241y270 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                            Mas270 = 0;
                        }
                        else
                        {
                            if (Entre211y240 > 1)
                            {
                                Entre0y30 = 0;
                                Entre31y60 = 0;
                                Entre61y90 = 0;
                                Entre91y120 = 0;
                                Entre121y150 = 0;
                                Entre151y180 = 0;
                                Entre181y210 = 0;
                                Entre211y240 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                Entre241y270 = 0;
                                Mas270 = 0;
                            }
                            else
                            {
                                if (Entre181y210 > 1)
                                {
                                    Entre0y30 = 0;
                                    Entre31y60 = 0;
                                    Entre61y90 = 0;
                                    Entre91y120 = 0;
                                    Entre121y150 = 0;
                                    Entre151y180 = 0;
                                    Entre181y210 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                    Entre211y240 = 0;
                                    Entre241y270 = 0;
                                    Mas270 = 0;
                                }
                                else
                                {
                                    if (Entre151y180 > 1)
                                    {
                                        Entre0y30 = 0;
                                        Entre31y60 = 0;
                                        Entre61y90 = 0;
                                        Entre91y120 = 0;
                                        Entre121y150 = 0;
                                        Entre151y180 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                        Entre181y210 = 0;
                                        Entre211y240 = 0;
                                        Entre241y270 = 0;
                                        Mas270 = 0;
                                    }
                                    else
                                    {
                                        if (Entre121y150 > 1)
                                        {
                                            Entre0y30 = 0;
                                            Entre31y60 = 0;
                                            Entre61y90 = 0;
                                            Entre91y120 = 0;
                                            Entre121y150 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                            Entre151y180 = 0;
                                            Entre181y210 = 0;
                                            Entre211y240 = 0;
                                            Entre241y270 = 0;
                                            Mas270 = 0;
                                        }
                                        else
                                        {
                                            if (Entre91y120 > 1)
                                            {
                                                Entre0y30 = 0;
                                                Entre31y60 = 0;
                                                Entre61y90 = 0;
                                                Entre91y120 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                Entre121y150 = 0;
                                                Entre151y180 = 0;
                                                Entre181y210 = 0;
                                                Entre211y240 = 0;
                                                Entre241y270 = 0;
                                                Mas270 = 0;
                                            }
                                            else
                                            {
                                                if (Entre61y90 > 1)
                                                {
                                                    Entre0y30 = 0;
                                                    Entre31y60 = 0;
                                                    Entre61y90 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                    Entre91y120 = 0;
                                                    Entre121y150 = 0;
                                                    Entre151y180 = 0;
                                                    Entre181y210 = 0;
                                                    Entre211y240 = 0;
                                                    Entre241y270 = 0;
                                                    Mas270 = 0;
                                                }
                                                else
                                                {
                                                    if (Entre31y60 > 1)
                                                    {
                                                        Entre0y30 = 0;
                                                        Entre31y60 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                        Entre61y90 = 0;
                                                        Entre91y120 = 0;
                                                        Entre121y150 = 0;
                                                        Entre151y180 = 0;
                                                        Entre181y210 = 0;
                                                        Entre211y240 = 0;
                                                        Entre241y270 = 0;
                                                        Mas270 = 0;
                                                    }
                                                    else
                                                    {
                                                        if (Entre0y30 > 1)
                                                        {
                                                            Entre0y30 = Convert.ToDouble(Fila["Atraso"]) < 1 ? 0.00 : Convert.ToDouble(Fila["Atraso"].ToString());
                                                            Entre31y60 = 0;
                                                            Entre61y90 = 0;
                                                            Entre91y120 = 0;
                                                            Entre121y150 = 0;
                                                            Entre151y180 = 0;
                                                            Entre181y210 = 0;
                                                            Entre211y240 = 0;
                                                            Entre241y270 = 0;
                                                            Mas270 = 0;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    workSheet.Range["AW" + x, "AW" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AW" + x, "AW" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AW"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre0y30.ToString());

                    workSheet.Range["AX" + x, "AX" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AX" + x, "AX" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AX"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre31y60.ToString());

                    workSheet.Range["AY" + x, "AY" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AY" + x, "AY" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AY"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre61y90.ToString());

                    workSheet.Range["AZ" + x, "AZ" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["AZ" + x, "AZ" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "AZ"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre91y120.ToString());

                    workSheet.Range["BA" + x, "BA" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BA" + x, "BA" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BA"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre121y150.ToString());

                    workSheet.Range["BB" + x, "BB" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BB" + x, "BB" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BB"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre151y180.ToString());

                    workSheet.Range["BC" + x, "BC" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BC" + x, "BC" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BC"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre181y210.ToString());

                    workSheet.Range["BD" + x, "BD" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BD" + x, "BD" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BD"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre211y240.ToString());

                    workSheet.Range["BE" + x, "BE" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BE" + x, "BE" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BE"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Entre241y270.ToString());

                    workSheet.Range["BF" + x, "BF" + x].NumberFormat = "#,##0.00";
                    workSheet.Range["BF" + x, "BF" + x].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[x, "BF"] = string.Format("{0:N2}", Fila["Status"].ToString() == "S" ? "0" : Mas270.ToString());


                    workSheet.Cells[x, "BG"].NumberFormat = "00000000000";
                    workSheet.Cells[x, "BG"] = Fila["Documento"].ToString().Replace("-", "").Replace("_", "");

                    workSheet.Range["A" + x, "BG" + x].Columns.AutoFit();


                }

                x = 8;
                workSheet.Range["A" + x, "BG" + x].Columns.AutoFit();

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
