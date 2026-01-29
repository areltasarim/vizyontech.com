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

    public class SayfaOzellikleriController : Controller
    {
        private readonly SayfaOzellikleriServis _SayfaOzellikServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Sayfa Özellik";
        private readonly string entityAltBaslik = "Sayfa Özellik Ekle";

        public SayfaOzellikleriController(AppDbContext _context, SayfaOzellikleriServis _SayfaOzellikServis)
        {
            this._context = _context;
            this._SayfaOzellikServis = _SayfaOzellikServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _SayfaOzellikServis.PageList();

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
                var Kategori = _context.SayfaOzellikleri.Find(Id);
                foreach (var item in diller)
                {
                    var sayfaOzellikleriTranslate = _context.SayfaOzellikleriTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (sayfaOzellikleriTranslate == null)
                    {
                        sayfaOzellikleriTranslate = new SayfaOzellikleriTranslate()
                        {
                            SayfaOzellikId = Id,
                            OzellikAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(sayfaOzellikleriTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                SayfaOzellikViewModel model = new SayfaOzellikViewModel()
                {
                    SayfaOzellik = _context.SayfaOzellikleri.Find(Id),
                };
                PopulateDropdown();
                return View(model);
            }
            else
            {
                SayfaOzellikViewModel Model = new SayfaOzellikViewModel();

                foreach (var item in diller)
                {
                    var sayfaOzellikleriTranslate = Model.SayfaOzellik.SayfaOzellikleriTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (sayfaOzellikleriTranslate == null)
                    {
                        sayfaOzellikleriTranslate = new SayfaOzellikleriTranslate()
                        {
                            OzellikAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.SayfaOzellik.SayfaOzellikleriTranslate.Add(sayfaOzellikleriTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(SayfaOzellikViewModel Model, string submit)
        {
            var model = await _SayfaOzellikServis.UpdatePage(Model, submit);

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
        public async Task<IActionResult> Delete(SayfaOzellikViewModel Model)
        {

            var model = await _SayfaOzellikServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _SayfaOzellikServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }


        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["SayfaOzellikGruplari"] = _context.SayfaOzellikGruplari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.SayfaOzellikGruplariTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").GrupAdi, Value = p.Id.ToString() });
        }
    }
}
