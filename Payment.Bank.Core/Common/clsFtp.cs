using Payment.Bank.Core.Model;
using Payment.Bank.DAL;
using Payment.Bank.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Payment.Bank.Core;

namespace Payment.Bank.Core.Common
{
    public class clsFtp
    {
        private string Host { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }


        public async void UploadFile(string EmpresaID, string Url)
        {
            try
            {
                clsCopiasBE BE = new clsCopiasBE();
                Context db = new Context();
                Manager core = new Manager();

                BE = db.clsCopiasBE.Where(x => x.EmpresaID == EmpresaID).FirstOrDefault();

                string payment = core.EmpresasGetByEmpresaID(BE.EmpresaID).Payment;
                var rowXML = Common.clsMisc.Decoding(payment == null ? "" : payment);

                if (!string.IsNullOrEmpty(rowXML))
                {
                    String xml = rowXML;
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");

                    Host = Nodo.Element("ftp").Value;
                    Username = Nodo.Element("User").Value;
                    Password =Common.clsMisc.Decoding(Nodo.Element("Pwd").Value);

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
                                FileInfo file = new FileInfo(Url);

                                try
                                {
                                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri($"{Host}/{BE.Database}/{BE.Empresas.Empresa}/{BE.EmpresaID}/{file.Name}"));
                                    request.Method = WebRequestMethods.Ftp.UploadFile;
                                    request.Credentials = new NetworkCredential(Username, Password);

                                    FileStream stream = File.OpenRead(file.FullName);

                                    Stream requestStream = request.GetRequestStream();
                                    byte[] buffer = new byte[1024];
                                    int bytesRead;
                                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                                    {
                                        requestStream.Write(buffer, 0, bytesRead);
                                    }

                                    requestStream.Close();
                                    stream.Close();

                                    FtpWebResponse response1 = (FtpWebResponse)request.GetResponse();
                                    Console.WriteLine("File uploaded successfully.");
                                    response1.Close();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error uploading file.");
                                }

                            }
                        }
                    }

                    foreach(var file in GetFiles(EmpresaID))
                    {
                        TimeSpan ts = DateTime.Now - file.Fecha;
                        if (ts.Days > 30)
                        {
                            DeleteFile(EmpresaID, file.Copia);
                        }
                    }
                }
            }
            catch(Exception ex)
            { }
        }

        private void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {  
            Console.WriteLine($"Upload file {e.ProgressPercentage}% completed");
        }

        public void DownloadFile(string EmpresaID, string filename, string url)
        {
            try
            {
                clsCopiasBE BE = new clsCopiasBE();
                Context db = new Context();
                Manager core = new Manager();

                BE = db.clsCopiasBE.Where(x => x.EmpresaID == EmpresaID).FirstOrDefault();

                string payment = core.EmpresasGetByEmpresaID(BE.EmpresaID).Payment;
                var rowXML = Common.clsMisc.Decoding(payment == null ? "" : payment);

                if (!string.IsNullOrEmpty(rowXML))
                {
                    String xml = rowXML;
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");

                    Host = Nodo.Element("ftp").Value;
                    Username = Nodo.Element("User").Value;
                    Password = Common.clsMisc.Decoding(Nodo.Element("Pwd").Value);

                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(Username, Password);
                        client.DownloadProgressChanged += DownloadProgressChanged;

                        byte[] fileData = client.DownloadData(new Uri($"{Host}/{BE.Database}/{BE.Empresas.Empresa}/{BE.EmpresaID}/{filename}"));

                        using (FileStream file = File.Create(url))
                        {
                            file.Write(fileData, 0, fileData.Length);
                            file.Close();
                        }

                    }


                }
            }
            catch(Exception ex)
            { }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine($"Download file {e.ProgressPercentage}% completed");
        }

        public List<Copias> GetFiles(string EmpresaID)
        {
            try
            {
                clsCopiasBE BE = new clsCopiasBE();
                Context db = new Context();
                Manager core = new Manager();

                BE = db.clsCopiasBE.Where(x => x.EmpresaID == EmpresaID).FirstOrDefault();

                string payment = core.EmpresasGetByEmpresaID(BE.EmpresaID).Payment;
                var rowXML = Common.clsMisc.Decoding(payment == null ? "" : payment);

                if (!string.IsNullOrEmpty(rowXML))
                {
                    String xml = rowXML;
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");

                    Host = Nodo.Element("ftp").Value;
                    Username = Nodo.Element("User").Value;
                    Password = Common.clsMisc.Decoding(Nodo.Element("Pwd").Value);

                    string Path = $"{Host}/{BE.Database}/{BE.Empresas.Empresa}/{BE.EmpresaID}";
                    FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Path);
                    ftpRequest.Credentials = new NetworkCredential(Username, Password);
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;

                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());

                    List<Copias> files = new List<Copias>();
                    string line = streamReader.ReadLine();
                    int x = 100001;
                    while (!string.IsNullOrEmpty(line))
                    {
                        var lineArr = line.Split('/');
                        line = lineArr[lineArr.Count() - 1];
                        string date = line.Replace($"{BE.Database}-", "").Replace(".zip", "").Replace("-", "");
                        int dd = int.Parse(date.Substring(0, 2));
                        int MM = int.Parse(date.Substring(2, 2));
                        int aaaa = int.Parse(date.Substring(4, 4));
                        int hh = int.Parse(date.Substring(8, 2));
                        int mm = int.Parse(date.Substring(10, 2));
                        int ss = int.Parse(date.Substring(12, 2));
                        int ms = int.Parse(date.Substring(14, 3));

                        files.Add(new Copias { CopiaID = x, Fecha = new DateTime(aaaa, MM, dd, hh, mm, ss, ms), Copia = line });
                        line = streamReader.ReadLine();
                        x++;
                    }

                    streamReader.Close();
                    return files;
                }
                return new List<Copias>();
            }
            catch(Exception ex)
            {
                return new List<Copias>();
            }
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

        public OperationResult DeleteFile(string EmpresaID, string filename)
        {
            try
            {
                clsCopiasBE BE = new clsCopiasBE();
                Context db = new Context();
                Manager core = new Manager();

                BE = db.clsCopiasBE.Where(x => x.EmpresaID == EmpresaID).FirstOrDefault();

                string payment = core.EmpresasGetByEmpresaID(BE.EmpresaID).Payment;
                var rowXML = Common.clsMisc.Decoding(payment == null ? "" : payment);

                if (!string.IsNullOrEmpty(rowXML))
                {
                    String xml = rowXML;
                    XDocument doc = XDocument.Parse(xml);
                    XElement Nodo = doc.Root.Element("Payment");

                    Host = Nodo.Element("ftp").Value;
                    Username = Nodo.Element("User").Value;
                    Password = Common.clsMisc.Decoding(Nodo.Element("Pwd").Value);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{Host}/{BE.Database}/{BE.Empresas.Empresa}/{BE.EmpresaID}/{filename}");
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    request.Credentials = new System.Net.NetworkCredential(Username, Password);
                    request.GetResponse().Close();

                    return new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                }
                return new OperationResult { ResponseCode = "01", ResponseMessage = "" };
            }
            catch(Exception ex) { return new OperationResult { ResponseCode = "00", ResponseMessage = ex.Message }; }
        }
    }
}
