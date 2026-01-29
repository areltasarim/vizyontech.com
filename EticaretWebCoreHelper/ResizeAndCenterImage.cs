using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace EticaretWebCoreHelper
{
    public static class ResizeAndCenterImageHelper
    {
        public static async Task<string> ResizeAndSaveImage(string inputPathOrUrl, string outputFolder)
        {
            Image image;

            if (inputPathOrUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(inputPathOrUrl);
                    image = Image.Load(imageBytes);
                }
            }
            else
            {
                image = Image.Load(inputPathOrUrl);
            }

            int canvasWidth = 600;
            int canvasHeight = 600;
            Color backgroundColor = Color.White;

            // Orantıyı koruyarak resim boyutlandırma (beyaz boşluklarla)
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(canvasWidth, canvasHeight),
                Mode = ResizeMode.Pad,
                PadColor = backgroundColor
            }));

            // Eğer klasör yoksa oluştur
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Yeni dosya adını oluştur
            string newFileName = $"{Guid.NewGuid()}.jpg"; // Benzersiz isim
            string outputPath = Path.Combine(outputFolder, newFileName);

            // Resmi kaydet
            image.Save(outputPath, new JpegEncoder());

            return outputPath; // Yeni oluşturulan dosyanın tam yolunu döndür
        }


    }
}
