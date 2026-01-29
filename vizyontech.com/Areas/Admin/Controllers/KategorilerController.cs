using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class KategorilerController : Controller
    {
        private readonly KategorilerServis _kategoriServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Kategoriler";
        private readonly string entityAltBaslik = "Kategori Ekle";

        public KategorilerController(AppDbContext _context, KategorilerServis _kategoriServis)
        {
            this._context = _context;
            this._kategoriServis = _kategoriServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _kategoriServis.PageList();

            return View(model);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            PopulateDropdown();

            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                var Kategori = _context.Kategoriler.Find(Id);
                foreach (var item in diller)
                {
                    var KategoriTranslate = Kategori.KategorilerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (KategoriTranslate == null)
                    {
                        KategoriTranslate = new KategorilerTranslate()
                        {
                            KategoriId = Id,
                            KategoriAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(KategoriTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                KategoriViewModel model = new KategoriViewModel()
                {
                    Kategori = _context.Kategoriler.Find(Id),
                };
                PopulateDropdown();
                return View(model);
            }
            else
            {
                PopulateDropdown();

                KategoriViewModel Model = new KategoriViewModel();

                foreach (var item in diller)
                {
                    var KategoriTranslate = Model.Kategori.KategorilerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (KategoriTranslate == null)
                    {
                        KategoriTranslate = new KategorilerTranslate()
                        {
                            KategoriAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.Kategori.KategorilerTranslate.Add(KategoriTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(KategoriViewModel Model, string submit)
        {
            var model = await _kategoriServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();
                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId});

            }
        }

        public IActionResult KategoriBanner(int KategoriId)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            ViewData["AltBaslik"] = "Banner";


            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            var diller = ViewBag.Diller;


            var kategoriBanner = _context.KategoriBanner.Where(x => x.KategoriId == KategoriId).ToList();

            foreach (var item in kategoriBanner)
            {
                foreach (var dil in diller)
                {
                    var kategoriBannerTranslate = kategoriBanner.Where(x => x.DilId == dil.Id).FirstOrDefault();
                    if (kategoriBannerTranslate == null)
                    {
                        kategoriBannerTranslate = new KategoriBanner()
                        {
                            KategoriId = KategoriId,
                            Sira = item.Sira,
                            DilId = dil.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = dil.DilAdi }
                        };
                        _context.Entry(kategoriBannerTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
            }



            KategoriBannerViewModel model = new KategoriBannerViewModel();

            foreach (var item in kategoriBanner)
            {
                model.KategoriBannerListe.Add(new KategoriBanner
                {
                    KategoriId = KategoriId,
                    Url = item.Url,
                    Resim = item.Resim,
                    Sira = item.Sira,
                    DilId = item.DilId,
                });
            }
            model.KategoriId = KategoriId;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> KategoriBannerEkle(KategoriBannerViewModel kategoriBanner)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            var diller = ViewBag.Diller;

            foreach (var dil in diller as IEnumerable<Diller>)
            {
                kategoriBanner.KategoriBannerListe.Add(new KategoriBanner
                {
                    KategoriId = kategoriBanner.KategoriId,
                    DilId = dil.Id,
                    Url = kategoriBanner.Url
                });
            }

            return PartialView("/Areas/Admin/Views/Kategoriler/EditorTemplates/_KategoriBannerEkle.cshtml", kategoriBanner);
        }



        [HttpPost]
        public async Task<IActionResult> KategoriBannerKaydet(KategoriBannerViewModel Model, string submit)
        {

            var model = await _kategoriServis.BannerEkleGuncelle(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

            return RedirectToAction(model.Action, "Kategoriler", new { KategoriId = model.SayfaId });
        }


       

        public async Task<IActionResult> Delete(KategoriViewModel Model)
        {

            var model = await _kategoriServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _kategoriServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            ViewData["Kategoriler"] = _context.Kategoriler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToCategoryTree()), Value = p.Id.ToString() });

        }
    }
}
