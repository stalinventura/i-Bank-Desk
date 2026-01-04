using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.Modulos
{
    public class FileUploader
    {
        public void UploadFile(Uri uri, string file)
        {
            UploadFileAsync(uri, file).GetAwaiter().GetResult();
        }

        public async Task UploadFileAsync(Uri uri, string file)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential("Administrator", "$Stalin03228100");
                    client.UploadProgressChanged += UploadProgressChanged;
                    await Task.Factory.StartNew(() => client.UploadFileAsync(uri, "PUT", file))
                                      .ConfigureAwait(false);
                }
            }catch(Exception ex)
            { }
        }

        private void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Console.WriteLine($"Progress: {e.ProgressPercentage}%");
        }
    }
}
