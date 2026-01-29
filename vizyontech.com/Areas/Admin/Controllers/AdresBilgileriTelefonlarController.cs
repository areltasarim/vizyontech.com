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

    public class AdresBilgileriTelefonlarController : Controller
    {
        AdresBilgileriTelefonlarServis _adresBilgisiServis = null;

        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Ek Telefonlar";
        private readonly string entityAltBaslik = "Telefon Ekle";

        public AdresBilgileriTelefonlarController(AppDbContext _context)
        {
            this._context = _context;
            _adresBilgisiServis = new AdresBilgileriTelefonlarServis(_context);
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _adresBilgisiServis.PageList();

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.AdresBilgileriTelefonlar.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(AdresBilgileriTelefonlarViewModel Model, int AdresBilgiId, string submit)
        {
            var model = await _adresBilgisiServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put<PageMessageModel>(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId, AdresBilgiId = AdresBilgiId });

            }
            else
            {
                TempDataExtensions.Put<PageMessageModel>(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();

                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId, AdresBilgiId = AdresBilgiId});
            }

        }

        public async Task<IActionResult> Delete(AdresBilgileriTelefonlarViewModel Model, int AdresBilgiId)
        {

            var result = await _adresBilgisiServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { AdresBilgiId = AdresBilgiId });

        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }

    }
}
