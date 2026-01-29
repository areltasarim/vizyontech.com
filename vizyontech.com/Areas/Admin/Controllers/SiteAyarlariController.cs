using Devsense.PHP.Syntax;
using DocumentFormat.OpenXml.Office2010.Excel;
using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]
    public class SiteAyarlariController : Controller
    {
        SiteAyarlariServis _siteAyariServis = null;
        private readonly AppDbContext _context;
        private readonly IDataProtector _dataProtector;

        private readonly string entityBaslik = "Site Ayarları";
        private readonly string entityAltBaslik = "Site Ayarı Ekle";
        private readonly string entityAltBaslik2 = "Site Ayarı Düzenle";

        public SiteAyarlariController(AppDbContext _context, IDataProtectionProvider dataProtectionProvider)
        {
            this._context = _context;
            _siteAyariServis = new SiteAyarlariServis(_context);
            _dataProtector = dataProtectionProvider.CreateProtector("SiteAyarlariController");
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _siteAyariServis.PageList();
            model.Foreach(x =>
            {
                x.EncrypedId = _dataProtector.Protect(x.Id.ToString());
            });

            return View(model);
        }


        public async Task<IActionResult> AddOrUpdate(string id)
        {
            ViewData["Baslik"] = entityBaslik;


            if (string.IsNullOrEmpty(id))
            {
                var yeniModel = new SiteAyarlari();
                return View(yeniModel);
            }

            ViewData["AltBaslik"] = _context.SiteAyarlari.Any() ? entityAltBaslik2 : entityAltBaslik;

            try
            {
                var decryptedId = int.Parse(_dataProtector.Unprotect(id));
                var model = await _context.SiteAyarlari.FindAsync(decryptedId);
                PopulateDropdown();

                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch
            {
                return BadRequest("Geçersiz Id");
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(string id, SiteAyariViewModel Model, string submit)
        {

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    Model.Id = int.Parse(_dataProtector.Unprotect(id));
                }
                catch
                {
                    return BadRequest("Geçersiz ID");
                }
            }

            var model = await _siteAyariServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put<PageMessageModel>(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new { Id = id });

            }
            else
            {
                TempDataExtensions.Put<PageMessageModel>(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();

                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = id });

            }


        }

        public async Task<IActionResult> Delete(SiteAyariViewModel Model)
        {

            var result = await _siteAyariServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = result.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _siteAyariServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewBag.ParaBirimleri = _context.ParaBirimleri.ToList() as IEnumerable<ParaBirimleri>;
        }

    }
}
