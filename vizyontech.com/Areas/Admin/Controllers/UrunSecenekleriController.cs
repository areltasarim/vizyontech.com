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

    public class UrunSecenekleriController : Controller
    {
        private readonly UrunSecenekleriServis _urunSecenekServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Ürün Seçenekleri";
        private readonly string entityAltBaslik = "Ürün Seçenek Ekle";

        public UrunSecenekleriController(AppDbContext _context, UrunSecenekleriServis _urunSecenekServis)
        {
            this._context = _context;
            this._urunSecenekServis = _urunSecenekServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _urunSecenekServis.PageList();

            return View(model);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;
            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                UrunSecenekViewModel model = new UrunSecenekViewModel()
                {
                    UrunSecenek = _context.UrunSecenekleri.Find(Id),
                };

                var urunSecenek = _context.UrunSecenekleri.Find(Id);
                foreach (var item in diller)
                {
                    var urunSecenekTranslate = urunSecenek.UrunSecenekleriTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (urunSecenekTranslate == null)
                    {
                        urunSecenekTranslate = new UrunSecenekleriTranslate()
                        {
                            UrunSecenekId = Id,
                            SecenekAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(urunSecenekTranslate).State = EntityState.Added;
                    }

                    //var urunSecenekDegerleri = _context.UrunSecenekDegerleriTranslate.Where(x => x.UrunSecenekDegerleri.UrunSecenekId == urunSecenek.Id && x.DilId == item.Id);
                    //foreach (var deger in urunSecenekDegerleri)
                    //{
                    //    model.UrunSecenekDegerListesi.Add(new UrunSecenekDegerleriTranslate
                    //    {
                    //        DegerAdi = deger.DegerAdi,
                    //        DilId = item.Id
                    //    });
                    //}


                }
                _context.SaveChanges();

                var urunSecenekDegerleri = _context.UrunSecenekDegerleriTranslate.Where(x => x.UrunSecenekDegerleri.UrunSecenekId == urunSecenek.Id).ToList();
                foreach (var deger in urunSecenekDegerleri)
                {
                    urunSecenekDegerleri.Where(x => x.DilId == deger.DilId);
                    model.UrunSecenekDegerListesi.Add(new UrunSecenekDegerleriTranslate
                    {
                        DegerAdi = deger.DegerAdi,
                        DilId = deger.DilId,
                        Sira = deger.UrunSecenekDegerleri.Sira,
                        UrunSecenekDegerId = deger.UrunSecenekDegerId
                    });
                }

                PopulateDropdown();

                return View(model);
            }
            else
            {
                UrunSecenekViewModel Model = new UrunSecenekViewModel();
                foreach (var item in diller)
                {
                    var urunSecenekTranslate = Model.UrunSecenek.UrunSecenekleriTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (urunSecenekTranslate == null)
                    {
                        urunSecenekTranslate = new UrunSecenekleriTranslate()
                        {
                            SecenekAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.UrunSecenek.UrunSecenekleriTranslate.Add(urunSecenekTranslate);
                    }
                }
                PopulateDropdown();
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(UrunSecenekViewModel Model, string submit)
        {
            var model = await _urunSecenekServis.UpdatePage(Model, submit);

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
                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId });

            }
        }

        [HttpPost]
        public async Task<ActionResult> UrunSecenekDegerEkle(UrunSecenekViewModel urunSecenekDeger)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            var diller = ViewBag.Diller;

            foreach (var dil in diller as IEnumerable<Diller>)
            {
                urunSecenekDeger.UrunSecenekDegerListesi.Add(new UrunSecenekDegerleriTranslate
                {
                    DilId = dil.Id,
                    UrunSecenekDegerleri = new UrunSecenekDegerleri()
                    {
                        Sira = 1
                    }
                });
            }
            //urunSecenekDeger.UrunSecenekDegerListesi.Add(new UrunSecenekDegerleriTranslate());
            return PartialView("/Areas/Admin/Views/UrunSecenekleri/EditorTemplates/_UrunSecenekDegerEkle.cshtml", urunSecenekDeger);
        }


        public async Task<IActionResult> Delete(UrunSecenekViewModel Model)
        {

            var model = await _urunSecenekServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _urunSecenekServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }
    }
}
