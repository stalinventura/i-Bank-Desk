using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.PointOfService;

namespace Payment.Bank.Modulos
{
    
    class clsPrinterBE
    {
        DataSet dsRecibos = default(DataSet);
        string _PrinterName;
        string FontName;
        Single _width = 0;
        Single _height = 0;
        //string Informacion;

        public void OpenCashDrawer()
        {
            try
            {
                CashDrawer myCashDrawer;
                PosExplorer explorer = new PosExplorer();

                DeviceInfo ObjDevicesInfo = explorer.GetDevice("CashDrawer");
                myCashDrawer = (CashDrawer)explorer.CreateInstance(ObjDevicesInfo);

                myCashDrawer.Open();
                myCashDrawer.Claim(1000);
                myCashDrawer.DeviceEnabled = true;
                myCashDrawer.OpenDrawer();
                myCashDrawer.DeviceEnabled = false;
                myCashDrawer.Release();
                myCashDrawer.Close();
            }
            catch { }
        }

        public void RecibosPrint(int ReciboID, String PrinterName, string _FontName, Single width, Single height)
        {
            try
            {
                _PrinterName = PrinterName;
                FontName = "Arial";
                _width = width;
                _height = height;
                Core.Manager db = new Core.Manager();


                dsRecibos = db.PrintRecibosGetByReciboID(ReciboID);

                PrintDocument recordDoc = new PrintDocument();

                recordDoc.DocumentName = "Recibos";
                if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "4A95-335C-BF45-3BC1-26B4-335B-CD3B-8EC1")
                {
                    recordDoc.PrintPage += Print_RecibosResumen; //new PrintPageEventHandler(ReceiptPrinter.PrintReceiptPage); // function below
                }
                else
                {
                    recordDoc.PrintPage += Print_Recibos;
                }
                recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
                // Comment if debugging 
                PrinterSettings ps = new PrinterSettings();
                ps.PrinterName = _PrinterName;
                recordDoc.DefaultPageSettings.Margins.Top = 0;
                recordDoc.DefaultPageSettings.Margins.Right = 0;
                recordDoc.DefaultPageSettings.Margins.Left = 0;
                recordDoc.PrinterSettings = ps;
                recordDoc.Print();
                // --------------------------------------

                recordDoc.Dispose();


                try
                {
                    clsCashDrawer.Open(PrinterName);
                }
                catch { }

            }
            catch { }
        }

        void Print_Recibos(object sender, PrintPageEventArgs e)
        {
            try
            {
                float x = 0;
                float y = 5;

                float width = _width; //_width + 0.0F; //250.0F; // max width I found through trial and error
                float height = _height; //_height + 0.0F; ; //0.0F;

                Font FontBold = new Font(FontName, 9, FontStyle.Bold);
                Font FontRegular = new Font(FontName, 7, FontStyle.Regular);
                //Font BarCode = new Font("Free 3 of 9 Extended", 36, FontStyle.Regular);

                var myFonts = new System.Drawing.Text.PrivateFontCollection();
                myFonts.AddFontFile(@".\Font\IDAutomationHC39M.TTF");
                var BarCode = new System.Drawing.Font(myFonts.Families[0], 16);

                SolidBrush drawBrush = new SolidBrush(Color.Black);

                // Set format of string.
                StringFormat drawFormatCenter = new StringFormat();
                drawFormatCenter.Alignment = StringAlignment.Center;
                StringFormat drawFormatLeft = new StringFormat();
                drawFormatLeft.Alignment = StringAlignment.Near;
                StringFormat drawFormatRight = new StringFormat();
                drawFormatRight.Alignment = StringAlignment.Far;

                DataRow Header;
                Header = dsRecibos.Tables[0].Rows[0];


                if (!string.IsNullOrEmpty(Header["logo"].ToString()) && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.GetLogo)
                {
                    // Create image.
                    Image logo = Image.FromStream(new MemoryStream((byte[])Header["logo"]));
                    
                    // Create coordinates for upper-left corner of image.
                    float w = 120.0F;
                    float h = 120.0F;

                    // Create rectangle for source image.
                    RectangleF srcRect = new RectangleF(0, 0, logo.Width, logo.Height);
                    //RectangleF srcRect = new RectangleF(0, 0, 150, 150);

                    GraphicsUnit units = GraphicsUnit.Pixel;

                    // Draw image to screen.
                    e.Graphics.DrawImage(logo, (width / 2 - logo.Width / 2), y, srcRect, units);
                    y += logo.Height + 10;
                }

                // Draw string to screen.
                string text = Header["Empresa"].ToString();
                e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontBold).Height;

                if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "4A95-335C-BF45-3BC1-26B4-335B-CD3B-8EC1")
                {
                    text = Header["Siglas"].ToString();
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                    y += e.Graphics.MeasureString(text, FontRegular).Height;
                }

                text = Header["DireccionEmpresa"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = Header["Telefonos"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = Header["Rnc"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontBold).Height + 10;


                text = "COMPROBANTE DE INGRESOS";
                e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontBold).Height + 5;

                text = Header["Sucursal"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontBold).Height + 10;



                text = "   FECHA: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

                text = Convert.ToDateTime(Header["Fecha"]).ToShortDateString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

                text = "NUMERO: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

                text = Header["ReciboID"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "     HORA: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

                text = Convert.ToDateTime(Header["Fecha"]).ToShortTimeString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

                text = "CONTRATO: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

                text = Header["ContratoID"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = " CLIENTE: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

                text = Header["Nombres"].ToString().ToUpper() + " " + Header["Apellidos"].ToString().ToUpper();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "  DIRECC: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

                text = Header["Direccion"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

                y += e.Graphics.MeasureString(text, FontRegular).Height + 5;


                for (int i = 1; i <= width; i++)
                {
                    text = "-";
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
                }
                y += e.Graphics.MeasureString(text, FontRegular).Height + 2;

                text = "NUMERO";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatLeft);

                text = "DESCRIPCION";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

                text = "MONTO";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;

                for (int i = 1; i <= width; i++)
                {
                    text = "-";
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
                }
                y += e.Graphics.MeasureString(text, FontRegular).Height;


                float Capital = 0;
                float Comision = 0;
                float Interes = 0;
                float Mora = 0;
                float Legal = 0;
                float Seguro = 0;

                foreach (DataRow detalle in dsRecibos.Tables[1].Rows)
                {

                    text = String.Format("{0:N}", detalle["SubTotal"]);
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

                    text =  detalle["Concepto"].ToString();
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

                    text = string.Format("{0} / {1}", String.Format("{0:00}", detalle["Numero"].ToString()), String.Format("{0:00}", Header["Tiempo"].ToString()));
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x+5, y, width, height), drawFormatLeft);
                    y += e.Graphics.MeasureString(text, FontRegular).Height;

                    Capital += float.Parse(detalle["Capital"].ToString());
                    Comision += float.Parse(detalle["Comision"].ToString());
                    Interes += float.Parse(detalle["Interes"].ToString());
                    Mora += float.Parse(detalle["Mora"].ToString());
                    Legal += float.Parse(detalle["Legal"].ToString());
                    Seguro += float.Parse(detalle["Seguro"].ToString());
                }
                for (int i = 1; i <= width; i++)
                {
                    text = "-";
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
                }
                y += e.Graphics.MeasureString(text, FontRegular).Height;


                var fila = dsRecibos.Tables[2].Rows[0];

                if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "F25B-0FE6-AC8B-2B81-9025-7847-F504-5C9F" || clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "6E23-B9BE-666E-81C5-FF25-07CE-D182-57D3")
                {
                    text = "ATRASO: " + String.Format("{0:N}", Convert.ToDouble(fila["Atraso"]));
                    e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                }
                else
                {
                    text = "BALANCE: " + String.Format("{0:N}", Convert.ToDouble(fila["Balance"]));
                    e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                }

                text = "FORMA: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

                text = Header["Nombre"].ToString().ToUpper();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;

                if (!(clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "F25B-0FE6-AC8B-2B81-9025-7847-F504-5C9F" || clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "6E23-B9BE-666E-81C5-FF25-07CE-D182-57D3"))
                {
                    text = "       ATRASO: " + String.Format("{0:N}", Convert.ToDouble(fila["Atraso"]));
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y + 5, width, height), drawFormatLeft);
                }

                text = "SUB-TOTAL: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

                text = String.Format("{0:N}", Header["SubTotal"]);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;


                text = "DESCUENTO: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

                text = String.Format("{0:N}", Header["DESCUENTO"]);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "TOTAL: ";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

                text = String.Format("{0:N}", Header["Monto"]);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

                if (float.Parse(Header["Total"].ToString()) > float.Parse(Header["Monto"].ToString()))
                {
                    text = "PAGADO:";
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-60, y, width, height), drawFormatRight);

                    text = String.Format("{0:N}", Header["Total"]);
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

                    y += e.Graphics.MeasureString(text, FontBold).Height;

                    text = "CAMBIO:";
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-60, y, width, height), drawFormatRight);

                    text = String.Format("{0:N}", Header["Cambio"]);
                    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

                    y += e.Graphics.MeasureString(text, FontBold).Height;
                }


                y += e.Graphics.MeasureString(text, FontBold).Height + 30;

                text = "------------------------------------------------------------";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 2;

                text = "DISTRIBUCIÓN DE PAGO";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "------------------------------------------------------------";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height +5;

                text = "CAPITAL     :";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(35, y, width, height), drawFormatLeft);
   
                text = String.Format("{0:N}", Capital);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "COMISION  :";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(35, y, width, height), drawFormatLeft);

                text = String.Format("{0:N}", Comision);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "INTERES     :";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(35, y, width, height), drawFormatLeft);

                text = String.Format("{0:N}", Interes);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "MORA         :";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(35, y, width, height), drawFormatLeft);

                text = String.Format("{0:N}", Mora);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "LEGAL        :";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(35, y, width, height), drawFormatLeft);

                text = String.Format("{0:N}", Legal);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);
                y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = "SEGURO     :";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(35, y, width, height), drawFormatLeft);

                text = String.Format("{0:N}", Seguro);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 2;

                text = "------------------------------------------------------------";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 30;



                text = "***LE ATENDIO: " + Header["Usuario"].ToString().ToUpper() + "***";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

                //text = "**GRACIAS POR PREFERIRNOS**";
                text = Header["Informacion"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 10;


                text = "*" + Header["ReciboID"].ToString() + "*";
                e.Graphics.DrawString(text, BarCode, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 80;

                text = "--- " + Header["RUTA"].ToString().ToUpper() + " ---";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height + 55;

            }
            catch(Exception ex) 
            { }
        }

        //void Print_Recibos(object sender, PrintPageEventArgs e)
        //{
        //    try
        //    {
        //        float x = 0;
        //        float y = 5;
        //        //float width = 250.0F; //_width + 0.0F; //250.0F; // max width I found through trial and error
        //        //float height = 0.0F; //_height + 0.0F; ; //0.0F;

        //        float width = _width; //_width + 0.0F; //250.0F; // max width I found through trial and error
        //        float height = _height; //_height + 0.0F; ; //0.0F;

        //        Font FontBold = new Font(FontName, 9, FontStyle.Bold);
        //        Font FontRegular = new Font(FontName, 7, FontStyle.Regular);
        //        //Font BarCode = new Font("Free 3 of 9 Extended", 36, FontStyle.Regular);


        //        var myFonts = new System.Drawing.Text.PrivateFontCollection();
        //        myFonts.AddFontFile(@".\Font\IDAutomationHC39M.TTF");
        //        var BarCode = new System.Drawing.Font(myFonts.Families[0], 16);



        //        SolidBrush drawBrush = new SolidBrush(Color.Black);

        //        // Set format of string.
        //        StringFormat drawFormatCenter = new StringFormat();
        //        drawFormatCenter.Alignment = StringAlignment.Center;
        //        StringFormat drawFormatLeft = new StringFormat();
        //        drawFormatLeft.Alignment = StringAlignment.Near;
        //        StringFormat drawFormatRight = new StringFormat();
        //        drawFormatRight.Alignment = StringAlignment.Far;

        //        DataRow Header;
        //        Header = dsRecibos.Tables[0].Rows[0];


        //        if (!string.IsNullOrEmpty(Header["logo"].ToString()) && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.GetLogo)
        //        {
        //            // Create image.
        //            Image logo = Image.FromStream(new MemoryStream((byte[])Header["logo"]));

        //            // Create coordinates for upper-left corner of image.
        //            float w = 120.0F;
        //            float h = 120.0F;

        //            // Create rectangle for source image.
        //            RectangleF srcRect = new RectangleF(0, 0, logo.Width, logo.Height);

        //            GraphicsUnit units = GraphicsUnit.Pixel;

        //            // Draw image to screen.
        //            e.Graphics.DrawImage(logo, (width / 2 - logo.Width / 2), y, srcRect, units);
        //            y += logo.Height + 10;
        //        }

        //        // Draw string to screen.
        //        string text = Header["Empresa"].ToString();
        //        e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontBold).Height;

        //        if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "4A95-335C-BF45-3BC1-26B4-335B-CD3B-8EC1")
        //        {
        //            text = Header["Siglas"].ToString();
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //            y += e.Graphics.MeasureString(text, FontRegular).Height;
        //        }

        //        text = Header["DireccionEmpresa"].ToString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = Header["Telefonos"].ToString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = Header["Rnc"].ToString();
        //        e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontBold).Height + 10;

        //        text = Header["Sucursal"].ToString();
        //        e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontBold).Height + 10;

        //        text = "RECIBO DE INGRESOS";
        //        e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontBold).Height + 10;

        //        text = "   FECHA: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

        //        text = Convert.ToDateTime(Header["Fecha"]).ToShortDateString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

        //        text = "NUMERO: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

        //        text = Header["ReciboID"].ToString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = "     HORA: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

        //        text = Convert.ToDateTime(Header["Fecha"]).ToShortTimeString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

        //        //text = "   FECHA: ";
        //        //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

        //        //text = Convert.ToDateTime(Header["Fecha"]).ToShortDateString();
        //        //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

        //        text = "CONTRATO: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

        //        text = Header["ContratoID"].ToString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = " CLIENTE: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

        //        text = Header["Nombres"].ToString().ToUpper() + " " + Header["Apellidos"].ToString().ToUpper();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = "  DIRECC: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

        //        text = Header["Direccion"].ToString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        //text = "TELEFON: " + Header["Telefono"].ToString();
        //        //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
        //        //y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        for (int i = 1; i <= width; i++)
        //        {
        //            text = "-";
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
        //        }
        //        y += e.Graphics.MeasureString(text, FontRegular).Height + 2;

        //        text = "CAPITAL";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatLeft);

        //        text = "COMISION";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-175, y, width, height), drawFormatRight);

        //        text = "INTERES";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-125, y, width, height), drawFormatRight);

        //        text = "MORA";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-85, y, width, height), drawFormatRight);

        //        text = "LEGAL";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);

        //        text = "SEGURO";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        for (int i = 1; i <= width; i++)
        //        {
        //            text = "-";
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
        //        }
        //        y += e.Graphics.MeasureString(text, FontRegular).Height;



        //        foreach (DataRow detalle in dsRecibos.Tables[1].Rows)
        //        {

        //            text = String.Format("{0:N}", detalle["Seguro"]);
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

        //            text = String.Format("{0:N}", detalle["Legal"]);
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);

        //            text = String.Format("{0:N}", detalle["Mora"]);
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-85, y, width, height), drawFormatRight);

        //            text = String.Format("{0:N}", detalle["Interes"]);
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-130, y, width, height), drawFormatRight);

        //            text = String.Format("{0:N}", detalle["Comision"]);
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-175, y, width, height), drawFormatRight);

        //            text = String.Format("{0:N}", Convert.ToDouble(detalle["Capital"]));
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(30, y, width, height), drawFormatLeft);

        //            y += e.Graphics.MeasureString(text, FontRegular).Height;

        //            text = string.Format("{0}/{1} - {2}", detalle["Numero"].ToString(), Header["Tiempo"], detalle["Concepto"].ToString().ToUpper());
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
        //            y += e.Graphics.MeasureString(text, FontRegular).Height;
        //        }
        //        for (int i = 1; i <= width; i++)
        //        {
        //            text = "-";
        //            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
        //        }
        //        y += e.Graphics.MeasureString(text, FontRegular).Height;


        //        var fila = dsRecibos.Tables[2].Rows[0];

        //        text = "BALANCE: " + String.Format("{0:N}", Convert.ToDouble(fila["Balance"]));
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

        //        text = "FORMA: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

        //        text = Header["Nombre"].ToString().ToUpper();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = "SUB-TOTAL: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

        //        text = String.Format("{0:N}", Header["SubTotal"]);
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;


        //        text = "DESCUENTO: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

        //        text = String.Format("{0:N}", Header["DESCUENTO"]);
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

        //        y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        text = "TOTAL: ";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

        //        text = String.Format("{0:N}", Header["Monto"]);
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

        //        //y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        //if (Variables.Cambio == true)
        //        //{
        //        //    text = "RECIBIDO: ";
        //        //    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

        //        //    text = String.Format("{0:N}", Header["Pagado"]);
        //        //    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

        //        //    y += e.Graphics.MeasureString(text, FontRegular).Height;

        //        //    text = "CAMBIO: ";
        //        //    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);


        //        //    text = String.Format("{0:N}", (Convert.ToDouble(Header["Pagado"]) - Convert.ToDouble(Header["Monto"])));
        //        //    e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);
        //        //}

        //        y += e.Graphics.MeasureString(text, FontRegular).Height + 30;

        //        text = "____________________________________";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

        //        text = "***LE ATENDIO: " + Header["Usuario"].ToString().ToUpper() + "***";
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

        //        //text = "**GRACIAS POR PREFERIRNOS**";
        //        text = Header["Informacion"].ToString();
        //        e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

        //        //text = Informacion;
        //        //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
        //        //y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

        //        text = "*" + Header["ReciboID"].ToString() + "*";
        //        e.Graphics.DrawString(text, BarCode, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
        //        y += e.Graphics.MeasureString(text, FontRegular).Height + 55;
        //    }
        //    catch (Exception ex)
        //    { }
        //}

        void Print_RecibosResumen(object sender, PrintPageEventArgs e)
        {
            float x = 0;
            float y = 5;
            //float width = 250.0F; //_width + 0.0F; //250.0F; // max width I found through trial and error
            //float height = 0.0F; //_height + 0.0F; ; //0.0F;

            float width = _width; //_width + 0.0F; //250.0F; // max width I found through trial and error
            float height = _height; //_height + 0.0F; ; //0.0F;

            Font FontBold = new Font(FontName, 9, FontStyle.Bold);
            Font FontRegular = new Font(FontName, 7, FontStyle.Regular);
            //Font BarCode = new Font("Free 3 of 9 Extended", 36, FontStyle.Regular);


            var myFonts = new System.Drawing.Text.PrivateFontCollection();
            myFonts.AddFontFile(@".\Font\IDAutomationHC39M.TTF");
            var BarCode = new System.Drawing.Font(myFonts.Families[0], 16);



            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            DataRow Header;
            Header = dsRecibos.Tables[0].Rows[0];

            if (!string.IsNullOrEmpty(Header["logo"].ToString()) && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.GetLogo)
            {
                // Create image.
                Image logo = Image.FromStream(new MemoryStream((byte[])Header["logo"]));

                // Create coordinates for upper-left corner of image.
                float w = 120.0F;
                float h = 120.0F;

                // Create rectangle for source image.
                RectangleF srcRect = new RectangleF(0, 0, logo.Width, logo.Height);

                GraphicsUnit units = GraphicsUnit.Pixel;

                // Draw image to screen.
                e.Graphics.DrawImage(logo, (width / 2 - logo.Width / 2), y, srcRect, units);
                y += logo.Height + 10;
            }

            // Draw string to screen.
            string text = Header["Empresa"].ToString();
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height;

            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "4A95-335C-BF45-3BC1-26B4-335B-CD3B-8EC1")
            {
                text = Header["Siglas"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height;
            }

            text = Header["DireccionEmpresa"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = Header["Telefonos"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = Header["Rnc"].ToString();
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height + 10;

            text = Header["Sucursal"].ToString();
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height + 10;

            text = "RECIBO DE INGRESOS";
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height + 10;

            text = "   FECHA: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Convert.ToDateTime(Header["Fecha"]).ToShortDateString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            text = "NUMERO: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

            text = Header["ReciboID"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "     HORA: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Convert.ToDateTime(Header["Fecha"]).ToShortTimeString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            //text = "   FECHA: ";
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            //text = Convert.ToDateTime(Header["Fecha"]).ToShortDateString();
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            text = "CONTRATO: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

            text = Header["ContratoID"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = " CLIENTE: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Header["Nombres"].ToString().ToUpper() + " " + Header["Apellidos"].ToString().ToUpper();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "  DIRECC: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Header["Direccion"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            //text = "TELEFON: " + Header["Telefono"].ToString();
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
            //y += e.Graphics.MeasureString(text, FontRegular).Height;

            for (int i = 1; i <= width; i++)
            {
                text = "-";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
            }
            y += e.Graphics.MeasureString(text, FontRegular).Height + 2;

            text = "CONCEPTO";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatLeft);

            //text = "COMISION";
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-175, y, width, height), drawFormatRight);

            //text = "INTERES";
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-125, y, width, height), drawFormatRight);

            //text = "MORA";
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-85, y, width, height), drawFormatRight);

            //text = "LEGAL";
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);

            text = "SUB TOTAL";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            for (int i = 1; i <= width; i++)
            {
                text = "-";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
            }
            y += e.Graphics.MeasureString(text, FontRegular).Height;



            foreach (DataRow detalle in dsRecibos.Tables[1].Rows)
            {
                text = string.Format("{0}/{1} - {2}", detalle["Numero"].ToString(), Header["Tiempo"], detalle["Concepto"].ToString().ToUpper());
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                //y += e.Graphics.MeasureString(text, FontRegular).Height;

                text = String.Format("{0:N}", detalle["SubTotal"]);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

                //text = String.Format("{0:N}", detalle["Legal"]);
                //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-45, y, width, height), drawFormatRight);

                //text = String.Format("{0:N}", detalle["Mora"]);
                //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-85, y, width, height), drawFormatRight);

                //text = String.Format("{0:N}", detalle["Interes"]);
                //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-130, y, width, height), drawFormatRight);

                //text = String.Format("{0:N}", detalle["Comision"]);
                //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-175, y, width, height), drawFormatRight);

                //text = String.Format("{0:N}", Convert.ToDouble(detalle["Capital"]));
                //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(30, y, width, height), drawFormatLeft);

                y += e.Graphics.MeasureString(text, FontRegular).Height;
            }
            for (int i = 1; i <= width; i++)
            {
                text = "-";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
            }
            y += e.Graphics.MeasureString(text, FontRegular).Height;


            var fila = dsRecibos.Tables[2].Rows[0];

            //text = "BALANCE: " + String.Format("{0:N}", Convert.ToDouble(fila["Balance"]));
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = "FORMA: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = Header["Nombre"].ToString().ToUpper();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "SUB-TOTAL: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = String.Format("{0:N}", Header["SubTotal"]);
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;


            text = "DESCUENTO: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = String.Format("{0:N}", Header["DESCUENTO"]);
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "TOTAL: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = String.Format("{0:N}", Header["Monto"]);
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height + 30;

            text = "____________________________________";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            text = "***LE ATENDIO: " + Header["Usuario"].ToString().ToUpper() + "***";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            //text = "**GRACIAS POR PREFERIRNOS**";
            text = Header["Informacion"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            //text = Informacion;
            //e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            //y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            text = "*" + Header["ReciboID"].ToString() + "*";
            e.Graphics.DrawString(text, BarCode, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 55;

        }

        public void RentasPrint(int PagoRentaID, String PrinterName, string _FontName, Single width, Single height)
        {
            try
            {
                _PrinterName = PrinterName;
                FontName = _FontName;
                _width = width;
                _height = height;
                Core.Manager db = new Core.Manager();


                dsRecibos = db.PrintPagoRentasGetByPagoRentaID(PagoRentaID);

                PrintDocument recordDoc = new PrintDocument();

                recordDoc.DocumentName = "Rentas";
                recordDoc.PrintPage += Print_Pago_Rentas;
                
                recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
                // Comment if debugging 
                PrinterSettings ps = new PrinterSettings();
                ps.PrinterName = _PrinterName;
                recordDoc.DefaultPageSettings.Margins.Top = 0;
                recordDoc.DefaultPageSettings.Margins.Right = 0;
                recordDoc.DefaultPageSettings.Margins.Left = 0;
                recordDoc.PrinterSettings = ps;
                recordDoc.Print();
                // --------------------------------------

                recordDoc.Dispose();

                try
                {
                    clsCashDrawer.Open(PrinterName);
                }
                catch { }

            }
            catch { }
        }

        void Print_Pago_Rentas(object sender, PrintPageEventArgs e)
        {
            float x = 0;
            float y = 5;
            //float width = 250.0F; //_width + 0.0F; //250.0F; // max width I found through trial and error
            //float height = 0.0F; //_height + 0.0F; ; //0.0F;

            float width = _width; //_width + 0.0F; //250.0F; // max width I found through trial and error
            float height = _height; //_height + 0.0F; ; //0.0F;

            Font FontBold = new Font(FontName, 9, FontStyle.Bold);
            Font FontRegular = new Font(FontName, 7, FontStyle.Regular);
            //Font BarCode = new Font("Free 3 of 9 Extended", 36, FontStyle.Regular);


            var myFonts = new System.Drawing.Text.PrivateFontCollection();
            myFonts.AddFontFile(@".\Font\IDAutomationHC39M.TTF");
            var BarCode = new System.Drawing.Font(myFonts.Families[0], 16);



            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            DataRow Header;
            Header = dsRecibos.Tables[0].Rows[0];

            if (!string.IsNullOrEmpty(Header["logo"].ToString()) && clsVariablesBO.UsuariosBE.Sucursales.Configuraciones.GetLogo)
            {
                // Create image.
                Image logo = Image.FromStream(new MemoryStream((byte[])Header["logo"]));

                // Create coordinates for upper-left corner of image.
                float w = 120.0F;
                float h = 120.0F;

                // Create rectangle for source image.
                RectangleF srcRect = new RectangleF(0, 0, logo.Width, logo.Height);

                GraphicsUnit units = GraphicsUnit.Pixel;

                // Draw image to screen.
                e.Graphics.DrawImage(logo, (width / 2 - logo.Width / 2), y, srcRect, units);
                y += logo.Height + 10;
            }

            // Draw string to screen.
            string text = Header["Empresa"].ToString();
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height;

            if (clsVariablesBO.UsuariosBE.Sucursales.EmpresaID == "4A95-335C-BF45-3BC1-26B4-335B-CD3B-8EC1")
            {
                text = Header["Siglas"].ToString();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
                y += e.Graphics.MeasureString(text, FontRegular).Height;
            }

            text = Header["DireccionEmpresa"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = Header["Telefonos"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = Header["Rnc"].ToString();
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height + 10;

            text = Header["Sucursal"].ToString();
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height + 10;

            text = "COMPROBANTE DE INGRESOS";
            e.Graphics.DrawString(text, FontBold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontBold).Height + 10;

            text = "   FECHA: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Convert.ToDateTime(Header["Fecha"]).ToShortDateString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            text = "NUMERO: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

            text = Header["PagoRentaID"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "     HORA: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Convert.ToDateTime(Header["Fecha"]).ToShortTimeString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            text = "CONTRATO: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-52, y, width, height), drawFormatRight);

            text = Header["AlquilerID"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-2, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = " CLIENTE: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Header["Nombres"].ToString().ToUpper() + " " + Header["Apellidos"].ToString().ToUpper();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "  DIRECC: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = Header["Direccion"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(50, y, width, height), drawFormatLeft);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            for (int i = 1; i <= width; i++)
            {
                text = "-";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
            }
            y += e.Graphics.MeasureString(text, FontRegular).Height + 2;

            text = "CONCEPTO";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatLeft);

            text = "MONTO";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            for (int i = 1; i <= width; i++)
            {
                text = "-";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
            }
            y += e.Graphics.MeasureString(text, FontRegular).Height;



            foreach (DataRow detalle in dsRecibos.Tables[1].Rows)
            {
                text = detalle["Concepto"].ToString().ToUpper();
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

                text = String.Format("{0:N}", detalle["Monto"]);
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(0, y, width, height), drawFormatRight);

                y += e.Graphics.MeasureString(text, FontRegular).Height;
            }
            for (int i = 1; i <= width; i++)
            {
                text = "-";
                e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x + i, y, width, height), drawFormatLeft);
            }
            y += e.Graphics.MeasureString(text, FontRegular).Height;


            var fila = dsRecibos.Tables[2].Rows[0];

            text = "BALANCE: " + String.Format("{0:N}", Convert.ToDouble(fila["Balance"]));
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);

            text = "FORMA: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = Header["Nombre"].ToString().ToUpper();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "SUB-TOTAL: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = String.Format("{0:N}", Header["SubTotal"]);
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;


            text = "DESCUENTO: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = String.Format("{0:N}", Header["DESCUENTO"]);
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height;

            text = "TOTAL: ";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-55, y, width, height), drawFormatRight);

            text = String.Format("{0:N}", Header["Monto"]);
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatRight);

            y += e.Graphics.MeasureString(text, FontRegular).Height + 30;

            text = "____________________________________";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            text = "***LE ATENDIO: " + Header["Usuario"].ToString().ToUpper() + "***";
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            //text = "**GRACIAS POR PREFERIRNOS**";
            text = Header["Informacion"].ToString();
            e.Graphics.DrawString(text, FontRegular, drawBrush, new RectangleF(-5, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 10;

            text = "*" + Header["PagoRentaID"].ToString() + "*";
            e.Graphics.DrawString(text, BarCode, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, FontRegular).Height + 55;

        }
    }
}
