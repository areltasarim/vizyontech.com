using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public class ImageHelper
    {


        public static byte[] Resize(Stream FileStream, int Width = 500, int Height = 500)
        {
            byte[] bytes = null;
            using (var image = new Bitmap(Width, Height))
            {
                using (var g = Graphics.FromImage(image))
                {
                    g.Clear(Color.White);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    using (var originalImage = Bitmap.FromStream(FileStream))
                    {

                        var newWidth = originalImage.Width;
                        var newHeight = originalImage.Height;
                        if (originalImage.Width > Width)
                        {
                            newWidth = Width;
                            newHeight = (int)((Width / (float)originalImage.Width) * newHeight);
                        }
                        if (newHeight > Height)
                        {
                            newWidth = (int)((Height / (float)newHeight) * newWidth);
                            newHeight = Height;
                        }

                        var targetRect = new Rectangle(((Width - newWidth) / 2), ((Height - newHeight) / 2), newWidth, newHeight);
                        g.DrawImage(originalImage, targetRect);
                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Jpeg);
                            bytes = ms.ToArray();
                        }
                    }
                }
            }
            return bytes;
        }

        public static string ImageResize(SixLabors.ImageSharp.Image img, int MaxWidth, int MaxHeight)
        {
            if(img.Width>MaxWidth || img.Height>MaxHeight)
            {
                double widthratio = (double)img.Width / (double)MaxWidth;
                double heightratio = (double)img.Height / (double)MaxHeight;
                double ratio = Math.Max(widthratio, heightratio);
                int newWidth = (int)(img.Width / ratio);
                int newHeight = (int)(img.Height / ratio);
                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() + "," + img.Width.ToString();
            }
        }

        public static void ResizeImageSon(string originalPath, string originalFileName, string newPath, string newFileName, int maximumWidth, int maximumHeight, bool enforceRatio, bool addPadding)
        {
            var image = Image.FromFile(originalPath + "\\" + originalFileName);
            var imageEncoders = ImageCodecInfo.GetImageEncoders();
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            var canvasWidth = maximumWidth;
            var canvasHeight = maximumHeight;
            var newImageWidth = maximumWidth;
            var newImageHeight = maximumHeight;
            var xPosition = 0;
            var yPosition = 0;


            if (enforceRatio)
            {
                var ratioX = maximumWidth / (double)image.Width;
                var ratioY = maximumHeight / (double)image.Height;
                var ratio = ratioX < ratioY ? ratioX : ratioY;
                newImageHeight = (int)(image.Height * ratio);
                newImageWidth = (int)(image.Width * ratio);

                if (addPadding)
                {
                    xPosition = (int)((maximumWidth - (image.Width * ratio)) / 2);
                    yPosition = (int)((maximumHeight - (image.Height * ratio)) / 2);
                }
                else
                {
                    canvasWidth = newImageWidth;
                    canvasHeight = newImageHeight;
                }
            }

            var thumbnail = new Bitmap(canvasWidth, canvasHeight);
            var graphic = Graphics.FromImage(thumbnail);

            if (enforceRatio && addPadding)
            {
                graphic.Clear(Color.White);
            }

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.DrawImage(image, xPosition, yPosition, newImageWidth, newImageHeight);

            thumbnail.Save(newPath + "\\" + newFileName, imageEncoders[1], encoderParameters);
        }
        public static string ReplaceName(IFormFile Resim, string Ek)
        {
            Replace rep = new Replace();
            string fileName = Path.GetFileName(Resim.FileName);
            string uzanti = Path.GetExtension(Resim.FileName);
          
            string resimtamad = rep.FriendlyUrl(fileName.Substring(0, (fileName.Length - uzanti.Length))) + Ek + uzanti;
            return resimtamad;
        }

        public static string ImageReplaceName(IFormFile Resim, string Ek)
        {
            Replace rep = new Replace();
            string fileName = Path.GetFileName(Resim.FileName);
            string uzanti = Path.GetExtension(Resim.FileName);
            
            // Resim dosyası ise .webp uzantısı kullan
            if (Resim.ContentType != null && Resim.ContentType.StartsWith("image/"))
            {
                uzanti = ".webp";
            }
            
            string _Ek = "";
            if (!string.IsNullOrWhiteSpace(Ek))
            {
                _Ek = rep.FriendlyUrl(Ek) + "_";
            }
            string resimtamad = _Ek + rep.FriendlyUrl(fileName.Substring(0, (fileName.Length - Path.GetExtension(Resim.FileName).Length))) + "_" + DateTime.Now.Day.ToString() + DateTime.Now.Month +
                  DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Second +
                  DateTime.Now.Minute + DateTime.Now.Millisecond + uzanti;
            return resimtamad;
        }


        public static string DosyaYolu(DosyaYoluTipleri dosyaTipi)
        {
            string dosyaDizin = "";
            if (dosyaTipi == DosyaYoluTipleri.Resim)
            {
                dosyaDizin = "Images/";
            }
            if (dosyaTipi == DosyaYoluTipleri.Dosya)
            {
                dosyaDizin = "Dosyalar/";
            }

            string model = "wwwroot/Content/Upload/"+ dosyaDizin;

            return model;
        }

        public static string DosyaYok(DosyaYoluTipleri dosyaTipi)
        {
            string dosyaDizin = "";
            if (dosyaTipi == DosyaYoluTipleri.Resim)
            {
                dosyaDizin = "/Content/Upload/Images/resimyok.png";
            }
            if (dosyaTipi == DosyaYoluTipleri.Breadcumb)
            {
                dosyaDizin = "/Content/Upload/Images/breadcumbdefault.png";
            }
            if (dosyaTipi == DosyaYoluTipleri.Dosya)
            {
                dosyaDizin = "#";
            }
            return dosyaDizin;
        }
    }
}
