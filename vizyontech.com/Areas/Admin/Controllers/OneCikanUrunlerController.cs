using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace okulmobilyam.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class OneCikanUrunlerController : Controller
    {
        private readonly OneCikanUrunlerServis _oneCikanUrunlerServis;

        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Öne Çıkan Ürünler";
        private readonly string entityAltBaslik = "Öne Çıkan Ürün Ekle";

        public OneCikanUrunlerController(AppDbContext _context, OneCikanUrunlerServis oneCikanUrunlerServis)
        {
            this._context = _context;
            _oneCikanUrunlerServis = oneCikanUrunlerServis;
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;





            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                var Urun = _context.OneCikanUrunler.Find(Id);
                foreach (var item in diller)
                {
                    var OneCikanUrunlerTranslate = Urun.OneCikanUrunlerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (OneCikanUrunlerTranslate == null)
                    {
                        OneCikanUrunlerTranslate = new OneCikanUrunlerTranslate()
                        {
                            OneCikanUrunId = Id,
                            ModulAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(OneCikanUrunlerTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                OneCikanUrunViewModel model = new OneCikanUrunViewModel()
                {
                    OneCikanUrun = _context.OneCikanUrunler.Find(Id),
                };
                PopulateDropdown();

                var modul = _context.Moduller.Where(x => x.EntityId == Id).FirstOrDefault();
                model.Durum = modul.Durum;
                model.Sira = modul.Sira;

                return View(model);
            }
            else
            {
                PopulateDropdown();

                OneCikanUrunViewModel Model = new OneCikanUrunViewModel();

                foreach (var item in diller)
                {
                    var OneCikanUrunlerTranslate = Model.OneCikanUrun.OneCikanUrunlerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (OneCikanUrunlerTranslate == null)
                    {
                        OneCikanUrunlerTranslate = new OneCikanUrunlerTranslate()
                        {
                            ModulAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.OneCikanUrun.OneCikanUrunlerTranslate.Add(OneCikanUrunlerTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(OneCikanUrunViewModel Model, string submit)
        {
            var model = await _oneCikanUrunlerServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, model.Controller, new { Id = model.SayfaId, ModulId = model.SayfaUrl });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("AddOrUpdate", "OneCikanUrunler", new { Id = model.SayfaId, ModulId = model.SayfaUrl });
            }
        }

        public async Task<IActionResult> Delete(OneCikanUrunViewModel Model)
        {
            var model = await _oneCikanUrunlerServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", "Moduller");
        }


        public IActionResult UrunAutoComplete(string term)
        {
            var result = _context.Urunler
                .Where(x =>
                    x.UrunlerTranslate
                        .SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower())
                        .UrunAdi.ToLower().Contains(term.ToLower()) ||
                    x.UrunKodu.ToLower().Contains(term.ToLower())
                )
                .Select(x => new
                {
                    value = x.UrunlerTranslate
                        .SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR")
                        .UrunAdi + " (" + x.UrunKodu + ")",
                    id = x.Id.ToString()
                });

            return Json(result);
        }


        public async Task<IActionResult> PageImages(int Id, int? DilId)
        {

            var model = _context.OneCikanUrunler.Where(x => x.Id == Id).FirstOrDefault();
            var resimadi = model.OneCikanUrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").ModulAdi;

            if (Request.HasFormContentType && Request.Form != null && Request.Form.Count() > 0)
            {
                {
                    foreach (var formFile in Request.Form.Files)
                    {
                        string imageName = ImageHelper.ImageReplaceName(formFile, resimadi);

                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "OneCikanUrunler/" + imageName;

                        FileInfo serverfile = new FileInfo(Mappath);
                        if (!serverfile.Directory.Exists)
                        {
                            serverfile.Directory.Create();
                        }

                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }

                        var sayfaResim = new OneCikanUrunResimleri()
                        {
                            Resim = Mappath.Remove(0, 7),
                            OneCikanUrunId = Id,
                            DilId = DilId,
                            Sira = 0
                        };

                        _context.OneCikanUrunResimleri.Add(sayfaResim);
                        await _context.SaveChangesAsync();

                    }

                }
            }

            return View(_context.OneCikanUrunResimleri.Where(x => x.OneCikanUrunId == Id && x.DilId == DilId).ToList());
        }

        public async Task<IActionResult> PageImageSortOrder(string sira)
        {
            var model = await _oneCikanUrunlerServis.ImageSortOrder(sira);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }

        public async Task<IActionResult> PageImagesDelete(int id)
        {
            var model = await _oneCikanUrunlerServis.ImageDelete(id);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }


        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["DilKodlari"] = _context.DilKodlari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DilKodu, Value = p.Id.ToString() });
        }
    }
}
