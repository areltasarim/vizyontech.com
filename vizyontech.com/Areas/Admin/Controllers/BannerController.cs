using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class BannerController : Controller
    {
        BannerServis bannerServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Banner";
        private readonly string entityAltBaslik = "Banner Ekle";
        public BannerController(AppDbContext _context)
        {
            this._context = _context;
            bannerServis = new BannerServis(_context);
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await bannerServis.PageList();

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            if (Id > 0)
            {
                BannerViewModel model = new BannerViewModel()
                {
                    Banner = _context.Banner.Find(Id),
                };


                var bannerDegerleri = _context.BannerResimTranslate
                 .Where(x => x.BannerResim.BannerId == Id)
                 .ToList();

                // Sıra takibi için bir dictionary oluşturuyoruz
                var dilSiraCounter = new Dictionary<int, int>();

                // Gelen veriler üzerinden dönüyoruz
                foreach (var deger in bannerDegerleri)
                {
                    // Eğer mevcut DilId için bir giriş yoksa yeni bir liste oluştur
                    if (!model.BannerResimListesi.ContainsKey(deger.DilId))
                    {
                        model.BannerResimListesi[deger.DilId] = new List<BannerResimTranslateModel>();

                        // Bu DilId için sırayı sıfırdan başlat
                        dilSiraCounter[deger.DilId] = 0;
                    }

                    // Listeye yeni bir eleman ekle
                    model.BannerResimListesi[deger.DilId].Add(new BannerResimTranslateModel
                    {
                        BannerResimId = deger.Id,
                        BannerAdi = deger.BannerAdi,
                        Url = deger.Url,
                        UrlTipi = deger.UrlTipi,
                        EntityId = deger.EntityId,
                        SeoUrlTipi = deger.SeoUrlTipi,
                        Sira = deger.Sira,
                        ResimUrl = deger.Resim,
                        DilId = deger.DilId,
                        BannerRow = dilSiraCounter[deger.DilId] // Mevcut sıra
                    });

                    // Bu DilId için sırayı bir artır
                    dilSiraCounter[deger.DilId]++;
                }



                PopulateDropdown();
                return View(model);
            }
            PopulateDropdown();

            BannerViewModel Model = new BannerViewModel();



            return View(Model);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdate([FromForm] BannerViewModel Model, string submit)
        //{
        //    var model = await bannerServis.UpdatePage(Model, submit);

        //    var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

        //    TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

        //    return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId });

        //}


        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromForm] BannerViewModel Model, string submit)
        {

            ResultViewModel sonuc = new ResultViewModel();
            try
            {

                var model = await bannerServis.UpdatePage(Model, submit);

                sonuc.Basarilimi = model.Basarilimi;
                sonuc.MesajDurumu = model.MesajDurumu;
                sonuc.Mesaj = model.Mesaj;
                sonuc.SayfaUrl = "/Admin/Banner/" + model.Action + "/" + (model.SayfaId == null ? "" : model.SayfaId.ToString());

                return Json(sonuc);

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "danger";
                sonuc.Mesaj = "Hata Oluştu";
                return Json(sonuc);
            }
        }


        [HttpPost]
        public async Task<IActionResult> BannerResimEkle(int DilId, List<BannerResimTranslateModel> mevcutListe)
        {
            PopulateDropdown();

            mevcutListe ??= new List<BannerResimTranslateModel>();
            int mevcutRow = mevcutListe.Any() ? mevcutListe.Max(x => x.BannerRow) : 0;
            var bannerResimTranslate = new BannerResimTranslateModel
            {
                DilId = DilId,
                BannerRow = mevcutRow + 1
            };
            mevcutListe.Add(bannerResimTranslate);
            return PartialView("/Areas/Admin/Views/Banner/EditorTemplates/BannerResimTranslate.cshtml", bannerResimTranslate);
        }



        public async Task<IActionResult> Delete(BannerViewModel Model)
        {

            var result = await bannerServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = result.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await bannerServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;


            ViewData["DinamikSayfalar"] = _context.Sayfalar.ToList().Where(p => p.Id != 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToPageTree()), Value = p.Id.ToString() });
            ViewData["Kategoriler"] = _context.Kategoriler.ToList().Where(p => p.Id != 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToCategoryTree()), Value = p.Id.ToString() });
            ViewData["Urunler"] = _context.Urunler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.UrunlerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi, Value = p.Id.ToString() });

            ViewData["SabitMenuler"] = _context.SabitMenuler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.SabitMenulerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").MenuAdi, Value = p.Id.ToString() });
            ViewData["FotografGalerileri"] = _context.FotografGalerileri.ToList().Where(p => p.GaleriTipi == GaleriTipleri.Galeri).AsQueryable().Select(p => new SelectListItem() { Text = p.FotografGalerileriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").GaleriAdi, Value = p.Id.ToString() });
            ViewData["EKatalog"] = _context.FotografGalerileri.ToList().Where(p => p.GaleriTipi == GaleriTipleri.EKatalog).AsQueryable().Select(p => new SelectListItem() { Text = p.FotografGalerileriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").GaleriAdi, Value = p.Id.ToString() });

            ViewData["Dosyalar"] = _context.FotografGaleriResimleri.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.Resim.Remove(0, 32), Value = p.Id.ToString() });


        }

    }
}
