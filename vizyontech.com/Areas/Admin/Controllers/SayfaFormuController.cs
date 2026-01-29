using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class SayfaFormuController : Controller
    {
        SayfaFormuServis _sayfaFormuServis = null;
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Sayfa Formu";
        private readonly string entityAltBaslik = "Sayfa Formu Ekle";

        public SayfaFormuController(AppDbContext _context)
        {
            this._context = _context;
            _sayfaFormuServis = new SayfaFormuServis(_context);


        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _sayfaFormuServis.PageList();

            return View(model);
        }

        public async Task<IActionResult> Delete(SayfaFormuViewModel Model, SayfaFormTipleri SayfaFormTipi)
        {
            var model = await _sayfaFormuServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new {SayfaFormTipi = SayfaFormTipi });
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _sayfaFormuServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
        }
    }
}
