using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace vizyontech.com.Areas.Admin.Controllers
{
    public class FileController : Controller
    {
        public FileController()
        {
        }

        [Area("Admin")]
        [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]
        [HttpPost]
        public IActionResult DosyaYukle(IFormFile file)
        {
            string fileurl = "";
            string mesaj = "";
            bool sonuc = false;
            if (file != null)
            {
                List<string> ContentTypeListesi = new()
                {
                    "image/jpeg",
                    "image/png",
                    "image/gif",
                    "image/webp",
                    "application/pdf",
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };
                string dosyaName = ImageHelper.ImageReplaceName(file,"");
                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Dosya) + dosyaName;
                if (ContentTypeListesi.Contains(file.ContentType))
                {
                    if (file.Length < 15728640)
                    {
                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        sonuc = true;
                        mesaj = "Dosya Yükleme Başarılı";
                        fileurl = Mappath.Remove(0, 7);
                    }
                    else
                    {
                        sonuc = false;
                        mesaj = "Maksimum 15 Mb boyutunda dosya yukleyiniz.";
                        fileurl = "";
                    }
                }
                else
                {
                    sonuc = false;
                    mesaj = "Yanlış Dosya Tipi";
                    fileurl = "";
                }
            }
            else
            {
                sonuc = false;
                mesaj = "Dosya Boş";
                fileurl = "";
            }
            return new JsonResult(
                new
                {
                    Sonuc = sonuc,
                    Mesaj = mesaj,
                    FileUrl = fileurl
                }
                );
        }

        public IActionResult ResimYukle(IFormFile file)
        {
            string fileurl = "";
            string mesaj = "";
            bool sonuc = false;
            if (file != null)
            {
                List<string> ContentTypeListesi = new()
                {
                    "image/jpeg",
                    "image/png",
                    "image/gif",
                    "image/webp",
                    "application/pdf",
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                };
                string imageName = ImageHelper.ImageReplaceName(file,"");
                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Urunler/" + imageName;
                if (ContentTypeListesi.Contains(file.ContentType))
                {
                    if (file.Length < 15728640)
                    {
                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        sonuc = true;
                        mesaj = "Dosya Yükleme Başarılı";
                        fileurl = Mappath.Remove(0, 7);
                    }
                    else
                    {
                        sonuc = false;
                        mesaj = "Maksimum 15 Mb boyutunda dosya yukleyiniz.";
                        fileurl = "";
                    }
                }
                else
                {
                    sonuc = false;
                    mesaj = "Yanlış Dosya Tipi";
                    fileurl = "";
                }
            }
            else
            {
                sonuc = false;
                mesaj = "Dosya Boş";
                fileurl = "";
            }
            return new JsonResult(
                new
                {
                    Sonuc = sonuc,
                    Mesaj = mesaj,
                    FileUrl = fileurl
                }
                );
        }
    }
}