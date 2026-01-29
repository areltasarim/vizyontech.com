using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class KargoMetodlariController : Controller
    {
        KargoMetodlariServis _kargoMetoduServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Kargo Metodları";
        private readonly string entityAltBaslik = "Kargo Metodu Ekle";
        public KargoMetodlariController(AppDbContext _context)
        {
            this._context = _context;
            _kargoMetoduServis = new KargoMetodlariServis(_context);
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _kargoMetoduServis.PageList();

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.KargoMetodlari.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(KargoMetoduViewModel Model, string submit)
        {
            var model = await _kargoMetoduServis.UpdatePage(Model, submit);

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

        public async Task<IActionResult> Delete(KargoMetoduViewModel Model)
        {

            var result = await _kargoMetoduServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = result.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _kargoMetoduServis.DeleteAllPage(Deletes);

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
