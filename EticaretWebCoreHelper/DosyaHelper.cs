using EticaretWebCoreEntity.Enums;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Exchange.WebServices.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace EticaretWebCoreHelper
{
    public static class DosyaHelper
    {
        public static Task<ResultViewModel> DosyaYukle(IFormFile dosya, string dosyaYolu, List<string> dosyaTipi, long dosyaBoyutu, DosyaYoluTipleri dosyaYoluTipi)
        {
            var result = new ResultViewModel();

            try
            {
                string imageName = ImageHelper.ImageReplaceName(dosya, "");

                string Mappath = ImageHelper.DosyaYolu(dosyaYoluTipi) + dosyaYolu + "/" + imageName;
                FileInfo serverfile = new FileInfo(Mappath);
                if (!serverfile.Directory.Exists)
                {
                    serverfile.Directory.Create();
                }
                if (dosya.Length > dosyaBoyutu)
                {
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = $"Maksimum {dosyaBoyutu} bayt boyutunda dosya yükleyiniz.";

                    return Task.FromResult(result);
                }
                else if (!dosyaTipi.Contains(dosya.ContentType))
                {
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = "World veya Pdf formatinda dosya yükleyiniz.";

                    return Task.FromResult(result);
                }
                else
                {
                    // Resim dosyası ise WebP formatına dönüştür
                    if (dosyaTipi.Any(t => t.StartsWith("image/")))
                    {
                        // Dosya adını .webp uzantısı ile değiştir
                        string webpPath = Path.ChangeExtension(Mappath, ".webp");
                        
                        using (var image = Image.Load(dosya.OpenReadStream()))
                        {
                            // ImageSharp uzantıya göre otomatik encoder seçer
                            // WebP encoder yoksa JPEG olarak kaydeder
                            try
                            {
                                image.Save(webpPath);
                            }
                            catch
                            {
                                // WebP desteklenmiyorsa JPEG olarak kaydet
                                string jpegPath = Path.ChangeExtension(Mappath, ".jpg");
                                image.Save(jpegPath);
                                webpPath = jpegPath;
                            }
                        }
                        
                        result.Basarilimi = true;
                        result.Sonuc = webpPath.Remove(0, 7).ToString();
                    }
                    else
                    {
                        // Resim değilse normal kaydet
                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            dosya.CopyTo(stream);
                        }
                        result.Basarilimi = true;
                        result.Sonuc = Mappath.Remove(0, 7).ToString();
                    }

                    return Task.FromResult(result);
                }
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Dosya eklerken hata oluştu: " + hata.Message;

            }
            return Task.FromResult(result);
        }

    }
}
