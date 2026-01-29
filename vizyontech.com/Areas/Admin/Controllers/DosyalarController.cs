using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]
    public class DosyalarController : Controller
    {
        private DosyalarServis _dosyaServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Sayfa Dosyaları";
        private readonly string entityAltBaslik = "Dosya Ekle";

        public DosyalarController(AppDbContext _context)
        {
            this._context = _context;
            _dosyaServis = new DosyalarServis(_context);
        }

        public async Task<IActionResult> Index(DosyaSayfaTipleri SayfaTipi)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _dosyaServis.PageList(SayfaTipi);

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.Dosyalar.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(DosyaViewModel Model, int DosyaKategoriId, string submit)
        {
            var model = await _dosyaServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId, DosyaKategoriId = DosyaKategoriId });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();

                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId, DosyaKategoriId = DosyaKategoriId });
            }
        }

        public async Task<IActionResult> Delete(DosyaViewModel Model,int DosyaKategoriId)
        {
            var model = await _dosyaServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new { DosyaKategoriId = DosyaKategoriId });
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes, int DosyaKategoriId)
        {
            var model = await _dosyaServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new { DosyaKategoriId = DosyaKategoriId });
        }

        //DROPZONE RESIM YUKLEME, SILME VE SIRALAMA

        public async Task<IActionResult> PageFiles(int Id)
        {

            var model = _context.Dosyalar.Where(x => x.Id == Id).FirstOrDefault();
            var resimadi = model.DosyalarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").DosyaAdi;

            string dosyaFormat = "";
            string dosyaBoyutUyari = "";
            float dosyaBoyut = 0;
            if (Request.HasFormContentType && Request.Form != null && Request.Form.Count() > 0)
            {


                List<string> ContentTypeListesi = new();

                ContentTypeListesi = new List<string> { "image/jpeg", "image/png", "image/gif", "image/webp", "application/pdf", "application/msword" };
                dosyaFormat = "Geçerli Bir Dosya Tipi Seçiniz";
                dosyaBoyutUyari = "Maksimum 30 Mb boyutunda dosya yükleyiniz.";
                dosyaBoyut = 31457280;


                foreach (var formFile in Request.Form.Files)
                {
                    if (ContentTypeListesi.Contains(formFile.ContentType))
                    {
                        string imageName = ImageHelper.ImageReplaceName(formFile, "");

                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Urunler/" + imageName;

                        FileInfo serverfile = new FileInfo(Mappath);
                        if (!serverfile.Directory.Exists)
                        {
                            serverfile.Directory.Create();
                        }

                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }

                        var sayfaResim = new DosyaGaleri()
                        {
                            DosyaAdi = Path.GetFileNameWithoutExtension(formFile.FileName),
                            Dosya = Mappath.Remove(0, 7),
                            DosyaId = Id,
                            Sira = 0
                        };

                        _context.DosyaGaleri.Add(sayfaResim);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Json(new ResultViewModel { Basarilimi = false, Mesaj = dosyaFormat, NotfyAlert = true, BootBoxAlert = false });
                    }
                    if (formFile.Length > dosyaBoyut)
                    {
                        return Json(new ResultViewModel { Basarilimi = false, Mesaj = dosyaBoyutUyari, NotfyAlert = true, BootBoxAlert = false });

                    }
                }

                var resimListesi = _context.DosyaGaleri.Where(x => x.DosyaId == model.Id);
                int ResimIndex = 0;
                foreach (var item in resimListesi)
                {
                    ResimIndex++;
                    item.Sira = ResimIndex;
                    _context.DosyaGaleri.Update(item);
                }
                await _context.SaveChangesAsync();

            }

            return View(_context.DosyaGaleri.Where(x => x.DosyaId == Id).ToList());
        }


        public async Task<IActionResult> PageFileSortOrder(string sira)
        {
            var model = await _dosyaServis.FilesSortOrder(sira);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }


        public async Task<IActionResult> PageFilesDelete(int id)
        {
            var model = await _dosyaServis.FilesDelete(id);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }
        //DROPZONE RESIM YUKLEME, SILME VE SIRALAMA

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }
    }
}