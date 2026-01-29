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

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class ModullerController : Controller
    {
        ModullerServis _modullerServis = null;
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Modüller";
        private readonly string entityAltBaslik = "Modül Ekle";

        public ModullerController(AppDbContext _context)
        {
            this._context = _context;
            _modullerServis = new ModullerServis(_context);


        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _modullerServis.PageList();
            Moduller();
            return View(model);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            if (Id > 0)
            {
                ModulViewModel model = new ModulViewModel()
                {
                    Modul = _context.Moduller.Find(Id),
                };
                PopulateDropdown();

                return View(model);
            }
            PopulateDropdown();
            ModulViewModel Model = new ModulViewModel();

            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ModulViewModel Model, string submit)
        {
            var model = await _modullerServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("Index", controllerValue, new { Id = model.SayfaId });
            }
        }

        public async Task<IActionResult> Delete(ModulViewModel Model)
        {
            var model = await _modullerServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _modullerServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public void Moduller()
        {
            List<ModulListesi> model = new()
            {
                //new ModulListesi {
                //    Id = 1,
                //    ModulAdi = "Öne Çıkan Kategoriler",
                //    ModulTipi = ModulTipleri.OneCikanKategoriler,
                //    Contoller = "OneCikanKategoriler",
                //    Action = "AddOrUpdate",
                //},
                // new ModulListesi {
                //    Id = 2,
                //    ModulAdi = "Tab Ürünler",
                //    ModulTipi = ModulTipleri.TabUrunler,
                //    Contoller = "OneCikanUrunler",
                //    Action = "AddOrUpdate",
                //},
                  new ModulListesi {
                    Id = 3,
                    ModulAdi = "Öne Çıkan Ürünler",
                    ModulTipi = ModulTipleri.OneCikanUrunler,
                    Contoller = "OneCikanUrunler",
                    Action = "AddOrUpdate",
                },
                //new ModulListesi {
                //    Id = 3,
                //    ModulAdi = "Aksesuar",
                //    ModulTipi = ModulTipleri.Aksesuar,
                //    Contoller = "OneCikanUrunler",
                //    Action = "AddOrUpdate",
                //},
                // new ModulListesi {
                //    Id = 4,
                //    ModulAdi = "Çok Satanlar",
                //    ModulTipi = ModulTipleri.CokSatanlar,
                //    Contoller = "OneCikanUrunler",
                //    Action = "AddOrUpdate",
                //},
            };
            ViewData["ModulListesi"] = model;
        }


       

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["DilKodlari"] = _context.DilKodlari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DilKodu, Value = p.Id.ToString() });
        }
    }
}
