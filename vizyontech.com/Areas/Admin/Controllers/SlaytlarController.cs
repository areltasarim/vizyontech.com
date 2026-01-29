using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class SlaytlarController : Controller
    {
        private readonly SlaytlarServis _slaytServis;

        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Slaytlar";
        private readonly string entityAltBaslik = "Slayt Ekle";

        public SlaytlarController(AppDbContext _context, SlaytlarServis slaytServis)
        {
            this._context = _context;
            _slaytServis = slaytServis;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _slaytServis.PageList();

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            if (Id > 0)
            {
                SlaytViewModel model = new SlaytViewModel()
                {
                    Slayt = _context.Slaytlar.Find(Id),
                };

                PopulateDropdown();

                return View(model);

            }
            PopulateDropdown();

            SlaytViewModel Model = new SlaytViewModel();

            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(SlaytViewModel Model, string submit)
        {
            var model = await _slaytServis.UpdatePage(Model, submit);

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

        public IActionResult UrunAutoComplete(string term)
        {

            var result = _context.Urunler.Where(x => x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi.ToLower().Contains(term.ToLower())).Select(x => new { value = x.UrunlerTranslate.Where(x=> x.Diller.DilKodlari.DilKodu == "tr-TR").FirstOrDefault().UrunAdi, id = x.Id.ToString() });

            return Json(result);
        }

        public async Task<IActionResult> Delete(SlaytViewModel Model)
        {

            var result = await _slaytServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = result.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _slaytServis.DeleteAllPage(Deletes);

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
