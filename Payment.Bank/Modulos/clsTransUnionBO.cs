using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace Payment.Bank.Modulos
{
    class clsTransUnionBO
    {
        public String TransUnionGetByFecha(DateTime Hasta, int SucursalID)
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
                workSheet.Name = "Listado de TransUnion";
                workSheet.Cells[1, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Empresa;
                workSheet.Cells[2, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Direccion;
                workSheet.Cells[3, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Telefonos;
                workSheet.Cells[4, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Rnc;
                workSheet.Cells[5, "A"] = clsVariablesBO.UsuariosBE.Sucursales.Empresas.Siglas;
                workSheet.Cells[6, "A"] = "Listado TransUnion al " + Hasta.ToShortDateString();
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
                workSheet.Cells[x, "I"] = "0-30 DIAS:";
                workSheet.Cells[x, "J"] = "31-60 DIAS:";
                workSheet.Cells[x, "K"] = "61-90 DIAS:";
                workSheet.Cells[x, "L"] = "+90 DIAS:";
                workSheet.Cells[x, "M"] = "BALANCE:";
                workSheet.Cells[x, "N"] = "ATRASO:";
                workSheet.Cells[x, "O"] = "ULT. PAGO:";
                workSheet.Cells[x, "P"] = "FECHA ULT:";
                workSheet.Cells[x, "Q"] = "STATUS:";
                //workSheet.Cells[x, "R"] = "RELACION TIPO:";
                workSheet.Cells[x, "R"] = "TIPO DE PRESTAMO:";

                Core.Manager db = new Core.Manager();
                var dsDataCredito = db.PrintListadoDataCreditoGetByFecha(Hasta, SucursalID);
                //dsDataCredito = Variables.saoWS.PrintDataCreditoGetByFecha(Hasta, IsCapital);


                foreach (DataRow Fila in dsDataCredito.Tables["Contratos"].Rows)
                {
                    x++;
                    workSheet.Range["A" + x, "A" + x].NumberFormat = "000000";
                    workSheet.Cells[x, "A"] = Fila["ContratoID"].ToString();

                    //workSheet.Range["B" + x, "B" + x].NumberFormat = "dd-MM-yyyy;@";
                    workSheet.Range["B" + x, "B" + x].NumberFormat = "DD/MM/YYYY";
                    workSheet.Cells[x, "B"] = Convert.ToDateTime(Fila["Fecha"]); //Fila["Fecha"].ToString();

                    workSheet.Range["C" + x, "C" + x].NumberFormat = "DD/MM/YYYY";
                    workSheet.Cells[x, "C"] = Convert.ToDateTime(Fila["Vence"]);

                    workSheet.Cells[x, "D"] = Fila["Documento"].ToString();
                    workSheet.Cells[x, "E"] = Fila["Cliente"].ToString();
                    workSheet.Cells[x, "F"] = Fila["Direccion"].ToString();
                    workSheet.Cells[x, "G"] = Fila["Celular"].ToString();

                    workSheet.Range["H" + x, "O" + x].NumberFormat = "#,##0.00";

                    workSheet.Cells[x, "H"] = Fila["Monto"];
                    workSheet.Cells[x, "I"] = Fila["ENTRE0Y30"];
                    workSheet.Cells[x, "J"] = Fila["ENTRE31Y60"];
                    workSheet.Cells[x, "K"] = Fila["ENTRE61Y90"];
                    workSheet.Cells[x, "L"] = Fila["MAS90"];
                    workSheet.Cells[x, "M"] = Fila["Balance"];
                    workSheet.Cells[x, "N"] = Fila["Atraso"];
                    workSheet.Cells[x, "O"] = Fila["MontoUltimoPago"];

                    if (Fila["FechaUltimoPago"].ToString() != String.Empty)
                    {
                        workSheet.Range["P" + x, "P" + x].NumberFormat = "DD/MM/YYYY";
                        workSheet.Cells[x, "P"] = Convert.ToDateTime(Fila["FechaUltimoPago"]);
                    }
                    workSheet.Cells[x, "Q"] = Fila["Status"].ToString() == "N" ? "Normal": Fila["Status"].ToString() == "M" ? "Mora" : Fila["Status"].ToString() == "L" ? "Legal" : Fila["Status"].ToString() == "S" ? "Cancelado" : "Castigado" ;
                    //workSheet.Cells[x, "R"] = Fila["Deudor"].ToString();
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
    }
}
