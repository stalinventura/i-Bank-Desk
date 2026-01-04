using Payment.Bank.Controles;
using System;
using System.Net;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Payment.Bank.Modulos
{
    public static class clsMessage
    {
        public static void ErrorMessage(string message, string Title = "")
        {
            frmMessageBox Mensaje = new frmMessageBox();

            Mensaje.OnInit(message, Title, frmMessageBox.MessageType.Error);
            Mensaje.Title = Title;
            Mensaje.btnAceptar.Visibility = System.Windows.Visibility.Collapsed;
            Mensaje.btnSalir.Click += (obj, args) =>
            {
                Mensaje.Close();
            };

            //int cont = 0;
            //DispatcherTimer Timer = new DispatcherTimer();
            //Timer.Interval = new TimeSpan(0, 0, 5);
            //Timer.Tick += (s, a) =>
            //{
            //    try
            //    {
            //        cont++;
            //        if (cont == 10)
            //        {
            //            Mensaje.Close();
            //        }
            //    }
            //    catch { }
            //};
            //Timer.Start();
            Mensaje.ShowDialog();
        }

        public static void InfoMessage(string message, string Title = null)
        {
            frmMessageBox Mensaje = new frmMessageBox();
            Mensaje.OnInit(message, Title, frmMessageBox.MessageType.Info);
            Mensaje.Title = Title;
            Mensaje.btnSalir.Visibility = System.Windows.Visibility.Collapsed;
            Mensaje.btnAceptar.Click += (obj, args) =>
            {
                Mensaje.Close();
            };


            int cont = 0;
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 5);
            Timer.Tick += (s, a) =>
            {
                try
                {
                    cont++;
                    if (cont == 10)
                    {
                        //Mensaje.Close();
                    }
                }
                catch { }
            };
            Timer.Start();

            Mensaje.ShowDialog();
        }

    }
}
