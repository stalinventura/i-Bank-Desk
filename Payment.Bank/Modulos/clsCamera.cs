using System;
using WebCam_Capture;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Payment.Bank.Modulos
{
    class clsCamera
    {
        private WebCamCapture webcam = new WebCamCapture();
        private System.Windows.Controls.Image _FrameImage;
        private int FrameNumber = 30;

        public void InitializeWebCam(System.Windows.Controls.Image ImageControl)
        {
            webcam = new WebCamCapture();
            webcam.FrameNumber = (ulong)(0);
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.ImageCaptured += webcam_ImageCaptured;
            _FrameImage = ImageControl;
        }

        public static Image ResizeImage(Image srcImage, int newWidth, int newHeight)
        {
            using (Bitmap imagenBitmap = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                imagenBitmap.SetResolution(Convert.ToInt32(srcImage.HorizontalResolution), Convert.ToInt32(srcImage.VerticalResolution));

                using (Graphics imagenGraphics = Graphics.FromImage(imagenBitmap))
                {
                    imagenGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    imagenGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imagenGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    imagenGraphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);

                    MemoryStream imagenMemoryStream = new MemoryStream();
                    imagenBitmap.Save(imagenMemoryStream, ImageFormat.Jpeg);
                    srcImage = Image.FromStream(imagenMemoryStream);
                }
            }
            return srcImage;
        }


        private void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            //ImageSource img;                
            //img.Width = clsHelper.LoadBitmap((System.Drawing.Bitmap)e.WebCamImage).Width - 100;
            //clsImagenes.  clsImagenes.PhotoToByte(clsHelper.LoadBitmap((System.Drawing.Bitmap)e.WebCamImage));
            _FrameImage.Source = clsHelper.LoadBitmap((System.Drawing.Bitmap)e.WebCamImage);

        }

        public void Start()
        {
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.Start(0);
        }

        public void Stop()
        {
            webcam.Stop();
        }

        public void Continue()
        {
            // change the capture time frame
            webcam.TimeToCapture_milliseconds = FrameNumber;

            // resume the video capture from the stop
            webcam.Start(this.webcam.FrameNumber);
        }

        public void ResolutionSetting()
        {
            webcam.Config();
        }

        public void AdvanceSetting()
        {
            webcam.Config2();
        }

    }
}
