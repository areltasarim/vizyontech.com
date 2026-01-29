using EticaretWebCoreEntity;
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

    public class OneCikanKategorilerController : Controller
    {
        OneCikanKategorilerServis _OneCikanKategorilerServis = null;
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Modüller";
        private readonly string entityAltBaslik = "Modül Ekle";

        public OneCikanKategorilerController(AppDbContext _context)
        {
            this._context = _context;
            _OneCikanKategorilerServis = new OneCikanKategorilerServis(_context);


        }

        public IActionResult AddOrUpdate(int ModulId, int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;





            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                var Kategori = _context.OneCikanKategoriler.Find(Id);
                foreach (var item in diller)
                {
                    var OneCikanKategorilerTranslate = Kategori.OneCikanKategorilerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (OneCikanKategorilerTranslate == null)
                    {
                        OneCikanKategorilerTranslate = new OneCikanKategorilerTranslate()
                        {
                            OneCikanKategoriId = Id,
                            ModulAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(OneCikanKategorilerTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                OneCikanKategoriViewModel model = new OneCikanKategoriViewModel()
                {
                    OneCikanKategori = _context.OneCikanKategoriler.Find(Id),
                };
                PopulateDropdown();

                var modul = _context.Moduller.Where(x => x.Id == ModulId).FirstOrDefault();
                model.Durum = modul.Durum;
                model.Sira = modul.Sira;

                return View(model);
            }
            else
            {
                PopulateDropdown();

                OneCikanKategoriViewModel Model = new OneCikanKategoriViewModel();

                foreach (var item in diller)
                {
                    var OneCikanKategorilerTranslate = Model.OneCikanKategori.OneCikanKategorilerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (OneCikanKategorilerTranslate == null)
                    {
                        OneCikanKategorilerTranslate = new OneCikanKategorilerTranslate()
                        {
                            ModulAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.OneCikanKategori.OneCikanKategorilerTranslate.Add(OneCikanKategorilerTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(OneCikanKategoriViewModel Model, string submit)
        {
            var model = await _OneCikanKategorilerServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, model.Controller, new { Id = model.SayfaId, ModulId = model.SayfaUrl });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("AddOrUpdate", "OneCikanKategoriler", new { Id = model.SayfaId, ModulId = model.SayfaUrl });
            }
        }
        public async Task<IActionResult> Delete(OneCikanUrunViewModel Model)
        {
            var model = await _OneCikanKategorilerServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", "Moduller");
        }
        public IActionResult KategoriAutoComplete(string term)
        {
            var result = _context.Kategoriler.ToList().Where(p => p.Id != 1).Where(x => x.KategorilerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).KategoriAdi.ToLower().Contains(term.ToLower())).Select(x => new { value = string.Join(" > ", x.ToCategoryTree()), id = x.Id.ToString() });

            return Json(result);
        }
        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["DilKodlari"] = _context.DilKodlari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DilKodu, Value = p.Id.ToString() });
        }
    }
}
