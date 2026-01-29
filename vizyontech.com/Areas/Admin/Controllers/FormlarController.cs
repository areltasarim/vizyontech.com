using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
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

    public class FormlarController : Controller
    {
        FormlarServis _formServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Formlar";
        private readonly string entityAltBaslik = "Form Ekle";
        public FormlarController(AppDbContext _context)
        {
            this._context = _context;
            _formServis = new FormlarServis(_context);
        }
        public async Task<IActionResult> Index(int FormBaslikId)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _formServis.PageList(FormBaslikId);

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var formDeger = _context.FormDegerleri.Where(p => p.FormId == Id).FirstOrDefault();

            if (Id > 0)
            {
                FormViewModel model = new FormViewModel()
                {
                    Form = _context.Formlar.Find(Id),
                    FormDeger = _context.FormDegerleri.Find(formDeger?.Id),
                };

                PopulateDropdown();

                return View(model);

            }
            PopulateDropdown();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(FormViewModel Model, int FormBaslikId, string submit)
        {
            var model = await _formServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId, FormBaslikId = FormBaslikId });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();

                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId, FormBaslikId = FormBaslikId });
            }
        }

        public async Task<IActionResult> Delete(FormViewModel Model, int FormBaslikId)
        {

            var model = await _formServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

            return RedirectToAction("Index", controllerValue, new { FormBaslikId = FormBaslikId, SayfaTipi = model.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes, int FormBaslikId)
        {
            var model = await _formServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new { FormBaslikId = FormBaslikId, SayfaTipi = model.SayfaUrl });
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }

    }
}
