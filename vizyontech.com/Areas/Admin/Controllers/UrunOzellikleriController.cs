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

    public class UrunOzellikleriController : Controller
    {
        private readonly UrunOzellikleriServis _urunOzellikServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Ürün Özellik";
        private readonly string entityAltBaslik = "Ürün Özellik Ekle";

        public UrunOzellikleriController(AppDbContext _context, UrunOzellikleriServis _urunOzellikServis)
        {
            this._context = _context;
            this._urunOzellikServis = _urunOzellikServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _urunOzellikServis.PageList();

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
                var urunOzellikleri = _context.UrunOzellikleri.Find(Id);
                foreach (var item in diller)
                {
                    var UrunOzellikleriTranslate = urunOzellikleri.UrunOzellikleriTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (UrunOzellikleriTranslate == null)
                    {
                        UrunOzellikleriTranslate = new UrunOzellikleriTranslate()
                        {
                            UrunOzellikId = Id,
                            OzellikAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(UrunOzellikleriTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                UrunOzellikViewModel model = new UrunOzellikViewModel()
                {
                    UrunOzellik = _context.UrunOzellikleri.Find(Id),
                };
                PopulateDropdown();
                return View(model);
            }
            else
            {
                UrunOzellikViewModel Model = new UrunOzellikViewModel();

                foreach (var item in diller)
                {
                    var UrunOzellikleriTranslate = Model.UrunOzellik.UrunOzellikleriTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (UrunOzellikleriTranslate == null)
                    {
                        UrunOzellikleriTranslate = new UrunOzellikleriTranslate()
                        {
                            OzellikAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.UrunOzellik.UrunOzellikleriTranslate.Add(UrunOzellikleriTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(UrunOzellikViewModel Model, string submit)
        {
            var model = await _urunOzellikServis.UpdatePage(Model, submit);

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
        public async Task<IActionResult> Delete(UrunOzellikViewModel Model)
        {

            var model = await _urunOzellikServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _urunOzellikServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }


        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["UrunOzellikGruplari"] = _context.UrunOzellikGruplari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.UrunOzellikGruplariTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").GrupAdi, Value = p.Id.ToString() });
        }
    }
}
