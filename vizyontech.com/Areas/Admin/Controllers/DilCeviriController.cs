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
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class DilCeviriController : Controller
    {
        DilCeviriServis _dilCeviriServis = null;
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Dil Çeviri";
        private readonly string entityAltBaslik = "Dil Çeviri Ekle";

        public DilCeviriController(AppDbContext _context)
        {
            this._context = _context;
            _dilCeviriServis = new DilCeviriServis(_context);


        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _dilCeviriServis.PageList();

            return View(model);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            if (Id > 0)
            {
                DilCeviriViewModel model = new DilCeviriViewModel()
                {
                    DilCeviri = _context.DilCeviri.Find(Id),
                };
                PopulateDropdown();

                return View(model);
            }
            PopulateDropdown();
            DilCeviriViewModel Model = new DilCeviriViewModel();

            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(DilCeviriViewModel Model, string submit)
        {
            var model = await _dilCeviriServis.UpdatePage(Model, submit);

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

        public async Task<IActionResult> Delete(DilCeviriViewModel Model)
        {
            var model = await _dilCeviriServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _dilCeviriServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["DilKodlari"] = _context.DilKodlari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DilKodu, Value = p.Id.ToString() });
        }
    }
}
