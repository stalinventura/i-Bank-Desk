using System;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace Payment.Bank.Modulos
{
    class clsHelper
    {
        //Block Memory Leak
      
       // [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static bool DeleteObject(IntPtr handle) {   return true;   }
        public static BitmapSource bs;
        public static IntPtr ip;

        public static BitmapSource LoadBitmap(System.Drawing.Bitmap bitmap)
        {

            //ip = source.GetHbitmap();
            //bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            //DeleteObject(ip);
            //return bs;
                        
            var bitmapData = bitmap.LockBits(
    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
    System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;

        }
        public static void SaveImageCapture(BitmapSource bitmap, string photo)
        {
            try
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.QualityLevel = 100;

                // Save Image
                string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), photo);

                FileStream fstream = new FileStream(filename, FileMode.Create);
                encoder.Save(fstream);
                fstream.Close();
            }
            catch { }

        }

    }
}
