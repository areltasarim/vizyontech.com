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

    public class UrunOzellikGruplariController : Controller
    {
        private readonly UrunOzellikGruplariServis _urunOzellikGrupServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Ürün Özellik Grupları";
        private readonly string entityAltBaslik = "Grup Ekle";

        public UrunOzellikGruplariController(AppDbContext _context, UrunOzellikGruplariServis _urunOzellikGrupServis)
        {
            this._context = _context;
            this._urunOzellikGrupServis = _urunOzellikGrupServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _urunOzellikGrupServis.PageList();

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
                var urunOzellikGrup = _context.UrunOzellikGruplari.Find(Id);
                foreach (var item in diller)
                {
                    var UrunOzellikGruplariTranslate = urunOzellikGrup.UrunOzellikGruplariTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (UrunOzellikGruplariTranslate == null)
                    {
                        UrunOzellikGruplariTranslate = new UrunOzellikGruplariTranslate()
                        {
                            UrunOzellikGrupId = Id,
                            GrupAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(UrunOzellikGruplariTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                UrunOzellikGrupViewModel model = new UrunOzellikGrupViewModel()
                {
                    UrunOzellikGrup = _context.UrunOzellikGruplari.Find(Id),
                };
                PopulateDropdown();
                return View(model);
            }
            else
            {
                UrunOzellikGrupViewModel Model = new UrunOzellikGrupViewModel();

                foreach (var item in diller)
                {
                    var UrunOzellikGruplariTranslate = Model.UrunOzellikGrup.UrunOzellikGruplariTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (UrunOzellikGruplariTranslate == null)
                    {
                        UrunOzellikGruplariTranslate = new UrunOzellikGruplariTranslate()
                        {
                            GrupAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.UrunOzellikGrup.UrunOzellikGruplariTranslate.Add(UrunOzellikGruplariTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(UrunOzellikGrupViewModel Model, string submit)
        {
            var model = await _urunOzellikGrupServis.UpdatePage(Model, submit);

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
        public async Task<IActionResult> Delete(UrunOzellikGrupViewModel Model)
        {

            var model = await _urunOzellikGrupServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _urunOzellikGrupServis.DeleteAllPage(Deletes);

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
