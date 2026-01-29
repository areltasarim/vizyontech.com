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

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = "AdminAuth")]

    public class IllerController : Controller
    {
        IllerServis _IlServis = null;
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "İller";
        private readonly string entityAltBaslik = "İl Ekle";

        public IllerController(AppDbContext _context)
        {
            this._context = _context;
            _IlServis = new IllerServis(_context);


        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _IlServis.PageList();

            return View(model);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.Iller.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(IlViewModel Model, string submit)
        {
            var model = await _IlServis.UpdatePage(Model, submit);

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

        public async Task<IActionResult> Delete(IlViewModel Model)
        {
            var model = await _IlServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _IlServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewData["Ulkeler"] = _context.Ulkeler.Select(x => new SelectListItem { Text = x.UlkeAdi, Value = x.Id.ToString() }).ToList();
        }
    }
}
