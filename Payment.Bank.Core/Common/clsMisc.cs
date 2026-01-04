using System;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using Payment.Bank.DAL;
using Payment.Bank.Entity;
using System.Linq;

namespace Payment.Bank.Core.Common
{
    public class clsMisc
    {
        #region Constantes

        Payment.Bank.DAL.Context db;
        static clsCorreosBE BE = new clsCorreosBE();

        public string SENDER_MAIL;
        public string SENDER_PSW;
        public string SMTPHOST;
        public int SMTPORT;
        public bool SMTPSSL;

        #endregion

        #region Métodos
        public clsMisc()
        {

        }
        public void SaveToEventLog(Exception ee)
        {
            try
            {
                //db = new Context();
                //BE = db.clsCorreosBE.Where(x => x.IsDefault == true).FirstOrDefault();

                //SENDER_MAIL = BE.Correo;
                //SENDER_PSW = BE.Contraseña;
                //SMTPHOST = BE.Servidores.Smtp;
                //SMTPORT = BE.Servidores.Puerto;
                //SMTPSSL = BE.Servidores.EnableSsl;

                //if (System.IO.File.Exists("LogError.txt"))
                //{
                //    System.IO.File.Delete("LogError.txt");
                //}

                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "LogError.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(ee.Message + "\n\n\n" + ee.StackTrace);
                }

                //using (StreamWriter sw = new StreamWriter("LogError.txt"))
                //{
                //    sw.Write(ee.Message + "\n\n\n" + ee.StackTrace);
                //}

                SendMail("stalin_ventura@outlook.com", "Alerta i-Bank" , ee.Message + "\n\n\n" + ee.StackTrace, null);
            }
            catch (Exception e)
            {
                SendMail("stalin_ventura@outlook.com", "Alert i-Bank", e.Message + "\n\n\n" + e.StackTrace, null);
            }
        }

        public static string Encryption(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public static string Decoding(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            string Texto = Encoding.UTF8.GetString(bytes);
            return Texto;
        }

        public String SendMail(string _Sender, string _Subject, string _message, string _Attachments = null, string _Bcc= null)
        {
            try
            {
                db = new DAL.Context();
                BE = db.clsCorreosBE.Where(x => x.IsDefault == true).FirstOrDefault();

                SENDER_MAIL = BE.Correo;
                SENDER_PSW = BE.Contraseña;
                SMTPHOST = BE.Servidores.Smtp;
                SMTPORT = BE.Servidores.Puerto;
                SMTPSSL = BE.Servidores.EnableSsl;
               

                MailMessage Message = new MailMessage();
                SmtpClient clienteSmtp = new SmtpClient();
                Message.To.Add(new MailAddress(_Sender));
                if (!string.IsNullOrEmpty(_Bcc))
                {
                    Message.Bcc.Add(new MailAddress(_Bcc));
                }
                Message.From = new MailAddress(SENDER_MAIL);
                Message.Subject = _Subject;
                Message.IsBodyHtml = true;
                Message.Body = _message;

                if (!string.IsNullOrEmpty(_Attachments))
                {
                    Message.Attachments.Add(new Attachment(_Attachments));
                }

                Message.Priority = System.Net.Mail.MailPriority.High;

                clienteSmtp.Host = SMTPHOST;
                clienteSmtp.Port = SMTPORT;
                clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                clienteSmtp.Credentials = new NetworkCredential(SENDER_MAIL, Decoding(SENDER_PSW));
                clienteSmtp.EnableSsl = SMTPSSL;


                clienteSmtp.Send(Message);
            }
            catch(Exception ex)
            {
                return "ERROR";
            }


            return "SUCCESS";


        }

        public static string GetErrorName(SqlException sqlEx)
        {
            string _errorName = "";
            foreach (SqlError error in sqlEx.Errors)
            {
                //if (error.Number == clsParametersBE.MIN_ERROR_NUMBER)
                //{
                //    _errorName = error.Message;
                //}
            }
            return _errorName;
        }
        #endregion
    }
}