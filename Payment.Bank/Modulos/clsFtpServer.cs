using Payment.Bank.Entity;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Payment.Bank.Modulos
{
    public class clsFtpServer
    {
        //public string Host { get; set; }
        string Host = "ftp://10.0.0.100";
        public string Username { get; set; }
        public string Password { get; set; }


        public clsFtpServer(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Password = password;
        }


        public async Task FtpUpload(clsCopiasBE BE)
        {
            bool response = true;
            if (!DirectoryExist(BE.Database))
            {
                response = DirectoryCreate(BE.Database);
            }

            if (response)
            {
                if (!DirectoryExist($"/{BE.Database}/{BE.Empresas.Empresa}"))
                {
                    response = DirectoryCreate($"{BE.Database}/{BE.Empresas.Empresa}");
                }

                if (response)
                {
                    if (!DirectoryExist($"/{BE.Database}/{BE.Empresas.Empresa}/{BE.EmpresaID}"))
                    {
                        response = DirectoryCreate($"{BE.Database}/{BE.Empresas.Empresa}/{BE.EmpresaID}");
                    }

                    if (response)
                    {

                    }
                }
            }

            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{Host}/i-bank/{BE.EmpresaID}/{file.Name}");
            //request.Method = WebRequestMethods.Ftp.UploadFile;

            //request.Credentials = new NetworkCredential(Username, Password);

            //using (FileStream fileStream = File.Open(file.FullName, FileMode.Open, FileAccess.Read))
            //{
            //    using (Stream requestStream = request.GetRequestStream())
            //    {
            //        await fileStream.CopyToAsync(requestStream);
            //        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            //        {    
            //            Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            //        }
            //    }
            //}

        }

        private bool DirectoryCreate(string path)
        {
            bool IsCreated = true;
            try
            {
                WebRequest request = WebRequest.Create($"{Host}/{path}");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(Username, Password);

                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine(resp.StatusCode);
                }
            }
            catch (Exception ex)
            {
                IsCreated = false;
            }

            return IsCreated;
        }

        private bool DirectoryExist(string path)
        {
            bool isExist = false;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{Host}/{path}");
                request.Credentials = new NetworkCredential(Username, Password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    isExist = true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }
                }
            }

            return isExist;
        }

        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage.ToString());
        }
    }
}
